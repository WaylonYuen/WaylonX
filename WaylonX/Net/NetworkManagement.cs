using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace WaylonX.Net {

    /// <summary>
    /// 連線頻道
    /// </summary>
    public enum ConnChannel {

        /// <summary>
        /// 主連線頻道
        /// </summary>
        Main,

        /// <summary>
        /// 資料庫連線
        /// </summary>
        Database,
    }

    public class NetworkManagement {

        #region Property
        private Dictionary<ConnChannel, ILinkInfo> ConnDict { get; set; } //網路連線資料表
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkManagement() {
            ConnDict = new Dictionary<ConnChannel, ILinkInfo>();
        }

        #endregion

        #region Dict Methods

        /// <summary>
        /// 頻道是否存在
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public bool Check(ConnChannel channel) {
            return ConnDict.ContainsKey(channel);
        }

        /// <summary>
        /// 添加頻道(擁有檢查機制)
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="Conn"></param>
        /// <returns></returns>
        public bool ConnAdd(ConnChannel channel, Connection Conn) {

            //檢查該連線是否存在
            if (!ConnDict.ContainsKey(channel)) {
                ConnDict.Add(channel, Conn);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 取得資訊(擁有檢查機制)
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public ILinkInfo GetValue(ConnChannel channel) {

            //檢查該連線是否存在
            return ConnDict.ContainsKey(channel) ? ConnDict[channel] : null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 啟動連線模式
        /// </summary>
        /// <param name="connection">連線Info</param>
        /// <param name="connectionChannel">連線頻道</param>
        /// <returns>連線是否成功</returns>
        public bool StartToConnect(ConnChannel channel, Connection Conn) {

            //檢查該連線是否存在
            if (!ConnDict.ContainsKey(channel)) {

                //接口過濾
                IConnection IConnection = Conn;

                //啟動連線並判斷連線是否成功
                if (IConnection.Connect()) {        //由於Connect()方法被限定在IConnection接口中,因此必須接口過濾
                    ConnDict.Add(channel, Conn);    //保存該連線資料
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 啟動監聽模式 : 若監聽量參數為0表示無
        /// </summary>
        /// <param name="connection">監聽Info</param>
        /// <param name="backlog">監聽量</param>
        /// <returns>監聽是否成功</returns>
        public bool StartToListen(ConnChannel channel, Connection Conn, int backlog) {

            //檢查該連線是否存在
            if (!ConnDict.ContainsKey(channel)) {

                //接口過濾
                IConnection IConnection = Conn;

                //啟動監聽並判斷監聽是否成功
                if (IConnection.Listen(backlog)) {  //由於Connect()方法被限定在IConnection接口中,因此必須接口過濾
                    ConnDict.Add(channel, Conn);    //保存該監聽資料
                    return true;
                }
            }

            return false;
        }

        #endregion
    }


}
