using System;
using System.Diagnostics;
using WaylonX.Users;

namespace WaylonX.Packets {

    /// <summary>
    /// 委派: 回調處理器
    /// </summary>
    /// <param name="packet">回調處理封包</param>
    public delegate void CallbackHandler(CallbackHandlerPacket packet);

    /// <summary>
    /// 回調處理封包
    /// </summary>
    public class CallbackHandlerPacket {

        #region Property

        /// <summary>
        /// 回調處理器
        /// </summary>
        public CallbackHandler CallbackHandler { get; set; }

        /// <summary>
        /// 用戶
        /// </summary>
        public IUser User { get; }

        /// <summary>
        /// 加密方法
        /// </summary>
        public Encryption Encryption { get; }

        /// <summary>
        /// 內容
        /// </summary>
        public byte[] Bys_Data { get; }

        #endregion

        public CallbackHandlerPacket(IUser user, Encryption encryption, byte[] bys_data) {
            User = user;
            Encryption = encryption;
            Bys_Data = bys_data;
        }

        /// <summary>
        /// 同步回調執行
        /// </summary>
        public void Excute() {
            CallbackHandler?.Invoke(this);
        }

        /// <summary>
        /// 異步回調執行
        /// </summary>
        public void BeginExcute() {
            CallbackHandler?.BeginInvoke(this, new AsyncCallback(ExcuteCallback), CallbackHandler);
        }

        /// <summary>
        /// 異步回調
        /// </summary>
        /// <param name="iar"></param>
        private void ExcuteCallback(IAsyncResult iar) {

            //還原Begin方法所提供的Object(此處BeginExcute提供了CallbackHandler)
            var packet = (CallbackHandler)iar.AsyncState;

            //執行異步操作(暫無)

            //結束異步
            packet.EndInvoke(iar);

        }

    }
}
