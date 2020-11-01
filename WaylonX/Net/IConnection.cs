using System;
using System.Net;
using System.Net.Sockets;

namespace WaylonX.Net {

    /// <summary>
    /// 網絡連線方法接口
    /// </summary>
    public interface IConnection {

        /// <summary>
        /// 啟動連線
        /// </summary>
        /// <returns>是否連線成功</returns>
        bool Connect();

        /// <summary>
        /// 啟動監聽
        /// </summary>
        /// <param name="backlog">監聽數量</param>
        /// <returns>是否監聽成功</returns>
        bool Listen(int backlog);
    }

    /// <summary>
    /// 連線資料接口
    /// </summary>
    public interface ILinkInfo {

        #region Property

        /// <summary>
        /// Socket
        /// </summary>
        Socket Socket { get; }

        /// <summary>
        /// ip end point
        /// </summary>
        IPEndPoint IPEndPoint { get; }

        /// <summary>
        /// 網絡模式
        /// </summary>
        NetworkMode NetworkMode { get; }

        
        #endregion
    }

}
