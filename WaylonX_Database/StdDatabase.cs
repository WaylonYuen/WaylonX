using System;
using System.Threading;
using MySql.Data.MySqlClient;
using WaylonX.Cloud;
using WaylonX.Loggers;
using WaylonX.Packets;
using WaylonX.Threading;

namespace WaylonX_Database {

    /// <summary>
    /// 資料庫系統: 已綁定Server架構
    /// </summary>
    public abstract class StdDatabase : DBBase_Catalina {

        #region Property

        //資料參數
        public DatabaseConnectInfoEventArgs Metrics { get => CSDargs as DatabaseConnectInfoEventArgs; }

        #endregion

        #region Constructor
        public StdDatabase(string name) : base(name) { }
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

            DBConnection = new MySqlConnection(Metrics.GetConnInfoStr());

            try {
                DBConnection.Open();
                ToString("數據庫已連接");

            } catch (MySqlException ex) {

                var logs = Shared.Logger.GetContainer(StdLogger.LogType.Warn, "Database", ToString());
                logs.Add("Database Name", Name);

                switch (ex.Number) {

                    case 0:
                        logs.Add("Catch 0", "無法連線到資料庫,找不到資料庫.");
                        break;

                    case 1045:
                        logs.Add("Catch 1045", "使用者帳號或密碼錯誤.");
                        break;

                    default:
                        logs.Add("Catch null", "未開啟目標數據庫..");
                        break;
                }

                logs.Add("Exception Msg", ex.Message);
                logs.Excute();

                this.Close(); //執行關閉程序
            }

        }

        /// <summary>
        /// 回調方法註冊: 註冊後的方法才能夠被外派調用並呼叫執行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnRegistered(object sender, EventArgs e) {

            //註冊回調
            //Default null
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

            //任務緩衝列隊線程
            var TaskBufferQueueInfo = new ThreadInfoEventArgs() {
                BreakTime = 500,
                Category = Category.Database,
            };
            WaylonX.Threading.Thread.Create(TaskBufferQueueThread, true).Start(TaskBufferQueueInfo);
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


        #endregion

        #region Methods

        public string ToString(string info) {

            var logs = Shared.Logger.GetContainer(StdLogger.LogType.DatabaseInfo, "Database", info);
            logs.Add("Database Name", Name);

            logs.Add("Name of DB", Metrics.DBName);
            logs.Add("User", Metrics.DBUser);
            logs.Add("HostIP", Metrics.DBHost);
            logs.Add("HostPort", Metrics.DBPort);
            logs.Add("Format", Metrics.DBFormat);
            return logs.Excute();
        }

        #endregion

    }

}
