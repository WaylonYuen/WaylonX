using System;
using System.Net;
using System.Net.Sockets;

namespace WaylonX.Net {

    /// <summary>
    /// 網路連線
    /// </summary>
    public class Connection : ILinkInfo, IConnection {

        #region Property

        Socket ILinkInfo.Socket { get { return socket; } }

        IPEndPoint ILinkInfo.IPEndPoint { get { return iPEndPoint; } }

        NetworkMode ILinkInfo.NetworkMode { get { return networkMode; } }

        
        #endregion

        #region Local Values

        private readonly Socket socket;
        private readonly IPEndPoint iPEndPoint;      
        private NetworkMode networkMode;

        #endregion

        /// <summary>
        /// Instructor
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="iPEndPoint"></param>
        public Connection(Socket socket, string ip, int port) {
            this.socket = socket;
            iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            networkMode = NetworkMode.Unknown;
        }

        /// <summary>
        /// Socket連線
        /// </summary>
        /// <returns></returns>
        bool IConnection.Connect() {

            bool isConnected = false;

            //執行指定協定連接方式
            switch (socket.ProtocolType) {

                //TCP協定
                case ProtocolType.Tcp:
                    try {
                        socket.Connect(iPEndPoint);   //協議綁定
                        isConnected = true;
                    } catch (Exception e) {
                        Console.WriteLine($"Exception: IConnection.Connect() 連線失敗 -> {e.Message}");
                        //throw new Exception("\n! 連接失敗:" + e.Message);
                    }
                    break;

                //UDP協定
                case ProtocolType.Udp:
                    //UNDONE: UDP -> NetMode.Connet unfinished.
                    //isConnected = true;
                    break;

                default:
                    break;
            }

            //設定NetworkMode
            if (isConnected) {
                networkMode = NetworkMode.Connect;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Socket監聽
        /// </summary>
        /// <returns></returns>
        bool IConnection.Listen(int backlog) {

            bool isListening = false;

            //執行指定協定監聽方式
            switch (socket.ProtocolType) {

                //TCP協定
                case ProtocolType.Tcp:
                    try {
                        socket.Bind(iPEndPoint);  //協議綁定
                        socket.Listen(backlog);
                        isListening = true;
                        break;

                    } catch (Exception e) {
                        throw new Exception("\n! 綁定&監聽失敗:" + e.Message);
                    }

                //UDP協定
                case ProtocolType.Udp:
                    try {
                        socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
                        socket.Bind(iPEndPoint);  //協議綁定
                        isListening = true;
                        break;

                    } catch (Exception e) {
                        throw new Exception("\n! 綁定&監聽失敗:" + e.Message);    //暫時性
                    }

                default:
                    break;
            }

            //設定NetworkMode
            if (isListening) {
                networkMode = NetworkMode.Listen;
                return true;
            }

            return false;
        }



    }
}
