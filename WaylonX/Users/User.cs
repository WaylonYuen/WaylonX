using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using WaylonX.Net;
using WaylonX.Packets;

namespace WaylonX.Users {

    public class User : IUser {

        #region Prop

        /// <summary>
        /// 取得用戶Socket
        /// </summary>
        Socket IUserNetwork.Socket { get => m_Socket; }

        /// <summary>
        /// 取得用戶網路狀態
        /// </summary>
        NetworkState IUserNetwork.NetworkState { get => m_networkState; }

        /// <summary>
        /// 取得用戶身份驗證碼
        /// </summary>
        int IUser.VerificationCode { get => m_VerificationCode; }

        #endregion

        #region Local Values
        private Socket m_Socket;
        private NetworkState m_networkState;
        private int m_VerificationCode;

        /// <summary>
        /// 線程工作項
        /// </summary>
        private readonly List<Thread> threadWorkItem = new List<Thread>();
        #endregion

        #region Constructor

        public User(NetworkState networkState) {
            m_networkState = networkState;              //設定用戶網路狀態
            m_VerificationCode = this.GetHashCode();    //設定用戶驗證碼
        }

        public User(Socket socket, NetworkState networkState) {
            m_Socket = socket;
            m_networkState = networkState;              //設定用戶網路狀態
            m_VerificationCode = this.GetHashCode();    //設定用戶驗證碼
        }
        #endregion

        #region Interface Methods

        /// <summary>
        /// 設定用戶身份驗證碼
        /// </summary>
        /// <param name="verificationCode"></param>
        void IUser.SetVerificationCode(int verificationCode) {
            m_VerificationCode = verificationCode;
        }

        /// <summary>
        /// 設定網路參數
        /// </summary>
        void IUserNetwork.SetNetworkMetrics(Socket socket, NetworkState state) {
            m_Socket = socket;
            m_networkState = state;
        }

        /// <summary>
        /// 設定Socket
        /// </summary>
        /// <param name="socket"></param>
        bool IUserNetwork.SetSocket(Socket socket) {
            if (m_Socket == null) {
                m_Socket = socket;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 設定用戶網路狀態
        /// </summary>
        /// <param name="networkState"></param>
        void IUserNetwork.SetNetworkState(NetworkState state) {
            m_networkState = state;
        }

        /// <summary>
        /// 添加及啟動用戶線程
        /// </summary>
        /// <param name="thread">添加的線程</param>
        /// <param name="name">線程名稱</param>
        /// <param name="hasObject">物件參數</param>
        void IUserThread.AddAndStartThread(Thread thread, string name, bool hasObject) {

            thread.Name = name;

            if (hasObject) {
                thread.Start(this);
            } else {
                thread.Start();
            }

            threadWorkItem.Add(thread);
        }

        /// <summary>
        /// Opt: 關閉用戶線程
        /// </summary>
        void IUserThread.Close() {
            //Undone: 消除User
            //Undone: 關閉List中的所有線程
        }

        /// <summary>
        /// 同步發送網路封包
        /// </summary>
        /// <param name="netPacket">網路封包</param>
        public void Send(IPacket packet) {

            //封包簽名: 將用戶驗證碼設定到封包中
            packet.VerificationCode = m_VerificationCode;

            //封裝封包
            byte[] bys_packet = packet.ToPackup();

            //檢查用戶是否處於連線狀態
            if (m_Socket.Connected) {
                try {
                    m_Socket.Send(bys_packet, bys_packet.Length, 0);  //發送封包
                } catch (Exception err) {
                    Console.WriteLine($"Error -> Packet sending failed : {err.Message}");
                }
            } else {
                //throw new Exception($"{m_Socket.RemoteEndPoint} : is offline！");
                //檢查用戶是否還存在
            }

        }

        /// <summary>
        /// 異步發送網路封包
        /// </summary>
        /// <param name="packet">網路封包</param>
        public void BeginSend(IPacket packet) {

            //封包簽名: 將用戶驗證碼設定到封包中
            packet.VerificationCode = m_VerificationCode;

            //封裝封包
            byte[] bys_packet = packet.ToPackup();

            if (m_Socket.Connected) {
                try {
                    //異步發送
                    m_Socket.BeginSend(bys_packet, 0, bys_packet.Length, SocketFlags.None, new AsyncCallback(SendCallback), m_Socket);
                } catch (Exception err) {

                    //Format: cw
                    Console.WriteLine($"Error -> Packet sending failed : {err.Message}");
                }
            } else {
                throw new Exception($"{m_Socket.RemoteEndPoint} : is offline！");
                //檢查用戶是否還存在
            }
        }

        #endregion

        /// <summary>
        /// 發送方法回調
        /// </summary>
        /// <param name="iar"></param>
        private void SendCallback(IAsyncResult iar) {

            //還原Begin方法所提供的Object(此處BeginSend提供了m_Socket)
            Socket socket = (Socket)iar.AsyncState;

            //執行異步操作(暫無)

            //結束發送(結束異步線程)
            socket.EndSend(iar);
        }


    }

}
