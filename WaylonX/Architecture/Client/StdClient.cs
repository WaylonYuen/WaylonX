using System;
using System.Net;
using System.Net.Sockets;
using WaylonX;
using WaylonX.Architecture.Client;
using WaylonX.Cloud;
using WaylonX.Net;
using WaylonX.Packets;
using WaylonX.Packets.Header;
using WaylonX.Threading;
using WaylonX.Users;

namespace WaylonXUnity.Net {

    public abstract class Client : StdClientArchitecture {

        #region Property

        //資料參數
        public ClientInfoEventArgs Metrics { get => CSDargs as ClientInfoEventArgs; }

        //用戶
        public IUser User { get; set; }
        #endregion

        #region Constructor
        public Client(string name) : base(name) {
            User = new User(NetworkState.Unknown);
        }
        #endregion


        #region EventReceiver -> 不可被Receiver調用(父類已經調用)

        /// <summary>
        /// 在Start函數被調用前執行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnAwake(object sender, EventArgs e) {

            //初始化服務器參數
        }

        /// <summary>
        /// 進行連線
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnConnecting(object sender, EventArgs e) {

            //創建socket
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var MainConn = new Connection(socket, Metrics.IP, Metrics.Port);

            //啟動監聽
            IsClose = !NetworkManagement.StartToConnect(ConnChannel.Main, MainConn);

            //如果成功監聽則
            if (!IsClose) {
                
                User.SetNetworkMetrics(socket, NetworkState.Connected);
                Shared.Logger.ClientInfo("客戶端啟動成功");
            } else {
                User.SetNetworkState(NetworkState.Disconnect);
                Shared.Logger.ClientInfo("客戶端啟動失敗");
                this.Close();
            }

        }

        /// <summary>
        /// 回調方法註冊: 註冊後的方法才能夠被外派調用並呼叫執行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnRegistered(object sender, EventArgs e) {

            //註冊任務緩衝區
            //佇列字典註冊
            Shared.TaskBuffer.CategoryRegister(Category.General);

            //註冊回調
            //default null
        }

        /// <summary>
        /// 啟動線程: 用於啟動各線程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnStartThread(object sender, EventArgs e) {

            //線程池設定
            //ThreadPool.SetMinThreads(3, 3);
            //ThreadPool.SetMaxThreads(5, 5);

            //特殊任務線程
            WaylonX.Threading.Thread.Create(PacketReceiverThread, true).Start(null);   //啟動監聽封包

            //任務緩衝列隊線程
            var TaskBufferQueueInfo = new ThreadInfoEventArgs() {
                BreakTime = 500,
                Category = Category.General,
            };
            WaylonX.Threading.Thread.Create(BeginTaskBufferQueueThread, true).Start(TaskBufferQueueInfo);
        }

        /// <summary>
        /// 關閉線程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnCloseThread(object sender, EventArgs e) {

        }

        #endregion

        #region Thread

        /// <summary>
        /// 監聽封包
        /// </summary>
        protected override void PacketReceiverThread(object obj) {

            Console.WriteLine("SubThread Start -> Call Func : ReceivePacketThread()");

            var socket = NetworkManagement.GetValue(ConnChannel.Main).Socket;

            while (!IsClose) {

                var bys_packetLength = Receive(socket, BasicTypes.SizeOf.Int);

                if (bys_packetLength != null) {

                    //取得封包 長度(Receive取得後會進行: 網絡字節組 轉換成 主機字節組 並解析為 Int)
                    var packetLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bys_packetLength, 0));

                    //取得封包 &解析
                    var packet = new Packet().Unpack(Receive(socket, packetLength));

                    //檢查封包(通過則進行封包分類, 否則丟棄封包)
                    if (packet.Header.Checking(User)) {
                        QueueDistributor(packet);   //佇列分配器(將不同類別的封包分配到不同的佇列中等待處理)
                    }//丟棄
                }
            }

            Console.WriteLine("SubThread Close -> Call Func : ReceivePacketThread()");
        }

        /// <summary>
        /// 佇列分配器 : 分配封包到對應的佇列隊伍中
        /// </summary>
        protected override void QueueDistributor(Packet packet) {

            //接口約束
            IStdPacketHeader IHeader = packet.Header;

            //封包組合
            var PreHandlerPacket = new CallbackHandlerPacket(packet.Header.User, IHeader.EncryptionType, packet.Body.Bys_data);

            //Undone ->
            //如果封包內容進行了加密 & 確認了加密方式: 序在此調用其他Handler, 封包佇列暫未提供 packetHeader的傳入功能（*不可在此解密->會導致程序阻塞)

            //封包類別判斷 & 佇列分配
            switch (IHeader.CategoryType) {

                //一般封包佇列(系統)
                case Category.General:

                    if (Shared.TaskBuffer.CallbackContainsKey(Category.General, IHeader.CallbackType)) {
                        PreHandlerPacket.CallbackHandler = Shared.TaskBuffer.GetHandler(Category.General, IHeader.CallbackType);
                        Shared.TaskBuffer.Enqueue(Category.General, PreHandlerPacket);
                        return;
                    }
                    break;

                //資料庫封包佇列
                case Category.Database:

                    if (Shared.TaskBuffer.CallbackContainsKey(Category.Database, IHeader.CallbackType)) {
                        PreHandlerPacket.CallbackHandler = Shared.TaskBuffer.GetHandler(Category.Database, IHeader.CallbackType);
                        Shared.TaskBuffer.Enqueue(Category.Database, PreHandlerPacket);
                        return;
                    }
                    break;

                default:
                    //找不到對應封包類別
                    Shared.Logger.Warn("找不到封包類別 -> 請確認該類別是否進行註冊");
                    break;
            }

            //註冊表找不到對應回調
            Shared.Logger.Warn("找不到封包回調 -> 請確認該回調是否進行註冊");
        }

        #endregion

        #region Methods

        /// <summary>
        /// 取得LinkInfo
        /// </summary>
        /// <returns></returns>
        public ILinkInfo GetLinkInfo(ConnChannel channel) {
            return NetworkManagement.GetValue(channel);
        }

        #endregion

    }

}
