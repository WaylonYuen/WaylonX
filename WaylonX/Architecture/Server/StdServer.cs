using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WaylonX.Cloud;
using WaylonX.Loggers;
using WaylonX.Net;
using WaylonX.Packets;
using WaylonX.Packets.Header;
using WaylonX.Threading;
using WaylonX.Users;

namespace WaylonX.Architecture.Server {

    /// <summary>
    /// 服務器
    /// </summary>
    public abstract class StdServer : StdServerArchitecture {

        #region Property

        //資料參數
        public ServerInfoEventArgs Metrics { get => CSDargs as ServerInfoEventArgs; }

        #endregion

        #region Constructor
        public StdServer(string name) : base(name) { }
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
            IsClose = !NetworkManagement.StartToListen(ConnChannel.Main, MainConn, Metrics.Backlog);

            //如果成功監聽則
            if (!IsClose) {
                ToString("服務器啟動成功");
            } else {
                Shared.Logger.Warn("服務器啟動失敗");
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
            Shared.TaskBuffer.CategoryRegister(Category.Database);

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
            ThreadPool.SetMinThreads(3, 3);
            ThreadPool.SetMaxThreads(5, 5);

            //特殊任務線程
            WaylonX.Threading.Thread.Create(AwaitClientThread, true).Start();   //啟動等待客戶端線程
            WaylonX.Threading.Thread.Create(ReadLogsThread, true).Start();      //啟動Log日誌線程

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

            ILinkInfo linkInfo = NetworkManagement.GetValue(ConnChannel.Main);

            if (linkInfo != null) {

                var IPEndPoint = linkInfo.IPEndPoint;   //取得EndPoint

                //Optimization: Client可發送指令給Server進行各種決策(此處為退出指令)
                //建立客戶端 -> 讓Server Accept()中斷從而跳出Await線程
                var client = new TcpClient();
                client.Connect(IPEndPoint.Address.ToString(), IPEndPoint.Port);
                client.Close();
            }

            //關掉用戶
        }

        #endregion

        #region Thread

        /// <summary>
        /// 等待客戶端_線程
        /// </summary>
        protected override void AwaitClientThread() {

            Shared.Logger.ServerInfo("Thread Start -> Call Func : AwaitClientThread()");

            //取得socket
            Socket socket = NetworkManagement.GetValue(ConnChannel.Main).Socket;

            //Socket不存在則表示連線有問題,執行關閉程序
            if (socket == null) {
                Shared.Logger.ServerInfo("Thread Close -> Call Func : AwaitClientThread()");
                Shared.Logger.Error("No Main Socket -> Call Func : AwaitClientThread()");
                this.Close();
                return;
            }

            //持續等待 -> 直到canClose Flag is true
            while (!IsClose) {

                try {

                    //監聽連線請求並建立用戶項, 程序會阻塞於Accept();
                    var userWorkItem = new UserWorkItem(new User(socket.Accept(), NetworkState.Connecting));

                    //Optimization: 當關閉線程後再度連線則會退出Accept, 需優化成接收指令確認是不是退出
                    if (IsClose) break;

                    //子線程
                    try {
                        //為user建立封包監聽子線程 & 啟動子線程
                        userWorkItem.AddAndStartThread(
                            WaylonX.Threading.Thread.Create(new ParameterizedThreadStart(PacketReceiverThread), true),
                            "ReceivePacketThread",
                            true);

                        //為user建立在線監測子線程 & 啟動子線程
                        userWorkItem.AddAndStartThread(
                            WaylonX.Threading.Thread.Create(new ParameterizedThreadStart(AliveThread), true),
                            "AliveThread",
                            true);

                        //捕獲例外
                    } catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                    }

                    #region User Init

                    //發送封包同步驗證碼
                    IUser user = userWorkItem.User; //接口約束
                    userWorkItem.User.Send(new Packet(Emergency.None, Encryption.None, Category.General, Callback.PacketHeaderSync, user.VerificationCode));

                    //添加用戶到用戶清單
                    UserManagement.UserList.Add(userWorkItem);
                    //show 用戶上線資料

                    #endregion

                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }

            }

            Shared.Logger.ServerInfo("Thread Close -> Call Func : AwaitClientThread()");
        }

        /// <summary>
        /// 監聽封包_子線程
        /// </summary>
        /// <param name="obj"></param>
        protected override void PacketReceiverThread(object obj) {

            //undone, 需將此線程保存到用戶裡面,以便用戶離線後檢查此線程是否停止運作

            Shared.Logger.ServerInfo("SubThread Start -> Call Func : ReceivePacketThread()");

            //拆箱: 將Obj還原成 target
            var user = obj as User;

            //指定user網絡接口進行接口約束
            IUserNetwork userNet = user;

            // if 服務器已關閉 or 用戶網絡狀態不等於Connected狀態, 則停止此線程
            while (!IsClose) {

                //if (userNet.NetworkState != NetworkState.Connected) {
                //    break;
                //}

                var bys_packetLength = Receive(userNet.Socket, BasicTypes.SizeOf.Int);

                if (bys_packetLength != null) {

                    //取得封包 長度(Receive取得後會進行: 網絡字節組 轉換成 主機字節組 並解析為 Int)
                    var packetLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bys_packetLength, 0));

                    //取得封包 &解析
                    var packet = new Packet().Unpack(Receive(userNet.Socket, packetLength));

                    //檢查封包(通過則進行封包分類, 否則丟棄封包)
                    if (packet.Header.Checking(user)) {
                        QueueDistributor(packet);   //佇列分配器(將不同類別的封包分配到不同的佇列中等待處理)
                    }//丟棄
                }

            }

            //清除Client socket
            //Undone: 用戶需被移除 於 List中
            //userNet.Socket.Shutdown(SocketShutdown.Both);
            //userNet.Socket.Close();

            Shared.Logger.ServerInfo("SubThread Close -> Call Func : ReceivePacketThread()");
        }

        /// <summary>
        /// 客戶端連線狀態_子線程
        /// </summary>
        /// <param name="socket"></param>
        protected override void AliveThread(object socket) {
            Shared.Logger.ServerInfo("subThread Start -> Call Func : AliveThread()");

            //Logic

            Shared.Logger.ServerInfo("subThread Close -> Call Func : AliveThread()");
        }

        /// <summary>
        /// 讀取日誌線程
        /// </summary>
        private void ReadLogsThread() {

            Shared.Logger.ServerInfo("Thread Start -> Call Func : ReadLogsThread()");

            while (!IsClose) {
                if (StdLogger.logQueue.Count > 0) {
                    if (StdLogger.logQueue.TryDequeue(out string log)) {
                        Console.WriteLine(log);
                    }
                } else {
                    System.Threading.Thread.Sleep(500);   //讓出線程（即：退出隊伍N秒重新排隊）
                }
            }

            Shared.Logger.ServerInfo("Thread Close -> Call Func : ReadLogsThread()");
        }

        #endregion

        #region Methods

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
                    Shared.Logger.ServerInfo("找不到封包類別 -> 請確認該類別是否進行註冊");
                    break;
            }

            //註冊表找不到對應回調
            Shared.Logger.ServerInfo("找不到封包回調 -> 請確認該回調是否進行註冊");
        }

        public string ToString(string info) {

            var logs = Shared.Logger.GetContainer(StdLogger.LogType.ServerInfo, "Server", info);
            logs.Add("Server Name", Name);

            var ILinkInfo = NetworkManagement.GetValue(ConnChannel.Main);

            logs.Add("AddressFamily", ILinkInfo.IPEndPoint.AddressFamily.ToString());
            logs.Add("ProtocolType", ILinkInfo.Socket.ProtocolType.ToString());
            logs.Add("IP", ILinkInfo.IPEndPoint.ToString());
            logs.Add("Port", ILinkInfo.IPEndPoint.Port.ToString());
            return logs.Excute();
        }

        #endregion

    }

}
