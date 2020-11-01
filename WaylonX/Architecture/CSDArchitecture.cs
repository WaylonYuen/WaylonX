using System;
using System.Net.Sockets;
using WaylonX.Cloud;
using WaylonX.Net;
using WaylonX.Packets;
using WaylonX.Threading;

namespace WaylonX.Architecture {

    /// <summary>
    /// Client-Server基礎Info參數
    /// </summary>
    public class CSBaseInfoEventArgs : EventArgs {

        /// <summary>
        /// 主機IP
        /// </summary>
        public string IP { get; set; }
        
        /// <summary>
        /// 主機端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 操作環境
        /// </summary>
        public Environment Environment { get; set; }

    }

    /// <summary>
    /// Client-Server-Model: 主從式架構
    /// </summary>
    public abstract class CSBase_Catalina : CSDArchitecture_Catalina {

        #region Property

        /// <summary>
        /// 網路管理類
        /// </summary>
        protected NetworkManagement NetworkManagement { get; set; }

        #endregion

        //Constructor
        public CSBase_Catalina(string name) : base(name) {
            NetworkManagement = new NetworkManagement();
        }

        #region Methods

        /// <summary>
        /// 佇列分配器 : 分配封包到對應的佇列隊伍中
        /// </summary>
        protected virtual void QueueDistributor(Packet packet) { }

        /// <summary>
        /// 監聽封包_線程
        /// </summary>
        /// <param name="obj"></param>
        protected abstract void PacketReceiverThread(object obj);

        #endregion
    }

    /// <summary>
    /// 基本服務框架 -> Client,Server,Database框架 -> Catalina版
    /// </summary>
    public abstract class CSDArchitecture_Catalina {

        #region Property

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 運行狀態判斷
        /// </summary>
        public static bool IsClose { get; protected set; }

        #endregion

        #region Local Values

        //事件執行器
        protected event EventHandler Init;
        public event EventHandler ShutDown;

        //事件參數
        protected EventArgs CSDargs { get; private set; }

        #endregion

        //Constructor
        public CSDArchitecture_Catalina(string name) {
            Name = name;
        }

        #region Trigger

        /// <summary>
        /// 啓動器
        /// </summary>
        public bool Start(EventArgs args) {

            //賦值
            IsClose = false;
            CSDargs = args;

            //註冊接收器
            //如果連線失敗則不再註冊接收器,直接返回
            if (!StartingEventReceiver()) return false;
            StartedEventReceiver();

            //執行
            if (Init != null) {
                Shared.Logger.Info("正在初始化...");
                Init.Invoke(null, EventArgs.Empty);
            }

            return true;
        }

        /// <summary>
        /// 關閉程序
        /// </summary>
        public void Close() {

            //賦值
            IsClose = true;

            //註冊接收器
            ClosingEventReceiver();
            ClosedEventReceiver();

            //執行
            if (ShutDown != null) {
                ShutDown.Invoke(null, EventArgs.Empty);
            }
        }

        #endregion

        #region Receiver

        /// <summary>
        /// 啟動程序時的事件接收器註冊
        /// </summary>
        protected virtual bool StartingEventReceiver() {
            Init += new EventHandler(OnAwake);
            Init += new EventHandler(OnConnecting);

            if (Init != null) {
                Shared.Logger.Info("正在初始化...");
                Init.Invoke(null, EventArgs.Empty);

                //取消訂閱
                Init -= new EventHandler(OnAwake);
                Init -= new EventHandler(OnConnecting);

                //檢查連線是否失敗: 失敗則直接返回
                if (IsClose) return false;
            }

            Init += new EventHandler(OnRegistered);
            Init += new EventHandler(OnStartThread);

            return true;
        }

        /// <summary>
        /// 啟動程序後的事件接收器註冊
        /// </summary>
        protected virtual void StartedEventReceiver() {

        }

        /// <summary>
        /// 關閉程序時的事件接收器註冊
        /// </summary>
        protected virtual void ClosingEventReceiver() {
            ShutDown += new EventHandler(OnCloseThread);
        }

        /// <summary>
        /// 關閉程序後的事件接收器註冊
        /// </summary>
        protected virtual void ClosedEventReceiver() {

        }

        #endregion

        #region EventReceiver

        /// <summary>
        /// 在Start函數被調用前執行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void OnAwake(object sender, EventArgs e);

        /// <summary>
        /// 連線
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void OnConnecting(object sender, EventArgs e);

        /// <summary>
        /// 回調方法註冊: 註冊後的方法才能夠被外派調用並呼叫執行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void OnRegistered(object sender, EventArgs e);

        /// <summary>
        /// 啟動線程: 用於啟動各線程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void OnStartThread(object sender, EventArgs e);

        /// <summary>
        /// 關閉線程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void OnCloseThread(object sender, EventArgs e);

        #endregion


        /// <summary>
        /// 同步任務緩衝區列隊
        /// </summary>
        /// <param name="breakTime">線程空閒時間</param>
        /// <param name="category">任務緩衝區類別</param>
        public static void TaskBufferQueueThread(object args) {

            var Info = args as ThreadInfoEventArgs;

            Shared.Logger.ServerInfo("Thread Start -> Call Func : " + Info.Category.ToString() + ".TaskBufferQueueThread()");

            while (!IsClose) {

                //檢查佇列是否有隊伍
                if (Shared.TaskBuffer.PacketQueueDict[Info.Category].Count > 0) {
                    if (Shared.TaskBuffer.PacketQueueDict[Info.Category].TryDequeue(out CallbackHandlerPacket e)) {
                        e.Excute(); //執行Handler
                    }

                } else {
                    //讓出線程（即：退出隊伍N秒重新排隊）
                    System.Threading.Thread.Sleep(Info.BreakTime);
                }
            }

            Shared.Logger.ServerInfo("Thread Close -> Call Func : " + Info.Category.ToString() + ".TaskBufferQueueThread()");
        }


        /// <summary>
        /// 異步任務緩衝區列隊
        /// </summary>
        /// <param name="breakTime">線程空閒時間</param>
        /// <param name="category">任務緩衝區類別</param>
        public static void BeginTaskBufferQueueThread(object args) {

            var Info = args as ThreadInfoEventArgs;

            Shared.Logger.ServerInfo("Thread Start -> Call Func : " + Info.Category.ToString() + ".BeginTaskBufferQueueThread()");

            while (!IsClose) {

                //檢查佇列是否有隊伍
                if (Shared.TaskBuffer.PacketQueueDict[Info.Category].Count > 0) {
                    if (Shared.TaskBuffer.PacketQueueDict[Info.Category].TryDequeue(out CallbackHandlerPacket e)) {
                        e.BeginExcute(); //執行Handler
                    }

                } else {
                    //讓出線程（即：退出隊伍N秒重新排隊）
                    System.Threading.Thread.Sleep(Info.BreakTime);
                }
            }

            Shared.Logger.ServerInfo("Thread Close -> Call Func : " + Info.Category.ToString() + ".BeginTaskBufferQueueThread()");
        }



        #region Methods

        /// <summary>
        /// 接收資料
        /// </summary>
        /// <param name="socket">連線資料</param>
        /// <param name="dataLength">資料長度</param>
        /// <returns></returns>
        public static byte[] Receive(Socket socket, int dataLength) {

            //安全性檢查
            if (dataLength <= 0) {
                return null;
            }

            //創建指定內容大小的載體
            var data_Bytes = new byte[dataLength];

            //防粘包: 未接收完指定長度的資料則不斷等待接收
            while (dataLength > 0) {

                //創建資料容器載體
                var recvData_Bytes = new byte[(dataLength < 1024) ? dataLength : 1024];

                //檢查緩存區是否有資料需要讀取: True為有資料, False為緩存區沒有資料              
                if (!(socket.Available == 0)) { //為避免線程阻塞在讀取部分而設置的緩存去內容判斷

                    //防粘包：如果當前接收的字節組大於or等於緩存區，則按緩存區大小接收; 否則按剩餘需要接收的字節組接收。
                    int recvAlready =
                            (dataLength >= recvData_Bytes.Length)
                                ? socket.Receive(recvData_Bytes, recvData_Bytes.Length, 0)  //以緩存區大小接收資料
                                : socket.Receive(recvData_Bytes, dataLength, 0);            //以剩餘資料大小接收資料

                    //將接收到的字節數保存 
                    recvData_Bytes.CopyTo(data_Bytes, data_Bytes.Length - dataLength);

                    //減掉已經接收到的字節數
                    dataLength -= recvAlready;

                } else {

                    System.Threading.Thread.Sleep(50);   //本地緩存為空

                    if (IsClose) {
                        data_Bytes = null;
                        break;
                    }
                }
            }

            return data_Bytes;
        }

        #endregion

    }

    /// <summary>
    /// 任務環境
    /// </summary>
    public enum Environment {
        Unknow,
        Unity,      //Debug.Log
        Terminal,   //CW
    }
}
