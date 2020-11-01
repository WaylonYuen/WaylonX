using System;
using WaylonX.Users;

namespace WaylonX.Packets.Header {

    public sealed class UserPacketHeader : StdPacketHeader<IUser> {

        #region Property

        public IUser User { get => m_user; }

        /// <summary>
        /// PacketHeader型態
        /// </summary>
        public override PacketHeaderType PacketHeaderType { get => PacketHeaderType.UserPacketHeader; }

        #endregion

        #region Constructor

        [Obsolete("This constructor will be optimization in the future", false)]
        /// <summary>
        /// 快捷構造器
        /// </summary>
        /// <param name="verificationCode"></param>
        /// <param name="category"></param>
        /// <param name="callback"></param>
        public UserPacketHeader(Category category, Callback callback)
            : base(category, callback) { }

        /// <summary>
        /// 標準構造器
        /// </summary>
        /// <param name="verificationCode"></param>
        /// <param name="emergency"></param>
        /// <param name="encryption"></param>
        /// <param name="category"></param>
        /// <param name="callback"></param>
        public UserPacketHeader(Emergency emergency, Encryption encryption, Category category, Callback callback)
            : base(emergency, encryption, category, callback){ }

        #endregion

        #region Methods

        /// <summary>
        /// 檢查
        /// </summary>
        /// <param name="bys_packet"></param>
        /// <returns></returns>
        public bool Checking(IUser user) {

            //封包條件檢查
            switch (m_callback) {

                //放行以下封包
                case Callback.Testing:
                case Callback.PacketHeaderSync:
                    m_user = user; //設定封包對象
                    return true;
            }

            //檢查封包驗證碼
            if (user.VerificationCode.Equals(VerificationCode)) {
                m_user = user;
                return true;
            }

            //不通過
            return false;
        }

        #endregion
    }
}
