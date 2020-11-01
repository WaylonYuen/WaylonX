using System;
using WaylonX.Users;

namespace WaylonX.Architecture.Server {

    public class ServerInfoEventArgs : CSBaseInfoEventArgs {

        /// <summary>
        /// 客戶端連線積壓數
        /// </summary>
        public int Backlog { get; set; }

    }

    /// <summary>
    /// 標準服務器模型架構
    /// </summary>
    public abstract class StdServerArchitecture : CSBase_Catalina {

        #region Property

        /// <summary>
        /// 用戶管理
        /// </summary>
        protected UserManagement UserManagement { get; }

        #endregion

        //Constructor
        public StdServerArchitecture(string name) : base(name) {
            UserManagement = new UserManagement();
        }

        #region Methods

        /// <summary>
        /// 等待客戶端_線程
        /// </summary>
        protected abstract void AwaitClientThread();

        /// <summary>
        /// 客戶端連線狀態_線程
        /// </summary>
        /// <param name="socket"></param>
        protected abstract void AliveThread(object socket);

        #endregion

    }
}
