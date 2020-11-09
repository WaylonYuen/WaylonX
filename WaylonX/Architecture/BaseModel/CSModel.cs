using System;
using WaylonX.Net;
using WaylonX.Packets;

namespace WaylonX.Architecture.BaseModel {

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
    public abstract class CSModel : CSDArchitecture {

        #region Property

        /// <summary>
        /// 網路管理類
        /// </summary>
        protected NetworkManagement NetworkManagement { get; set; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">架構名稱</param>
        public CSModel(string name) : base(name) {
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
}
