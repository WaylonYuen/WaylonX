using System.Text;
using WaylonX.Converter;
using WaylonX.Packets.Base;
using WaylonX.Packets.Header;
using WaylonX.Packets.Header.Base;
using WaylonX.Packets.PacketData;

namespace WaylonX.Packets {

    /// <summary>
    /// 封包接口
    /// </summary>
    public interface IPacket : IPacketBase, IPacketHeaderIdentity {

        /// <summary>
        /// 封包型態
        /// </summary>
        PacketType PacketType { get; }

    }

    /// <summary>
    /// 標準封包 : 資料內容僅提供基礎型態
    /// </summary>
    public class StdPacket : Packaged<UserPacketHeader, StdPacketData>, IPacket {

        #region Property

        /// <summary>
        /// Std封包結構SIZE: 結構大小最小不會小於此SIZE
        /// </summary>
        public virtual int PacketStructSIZE { get => m_Header.StructSIZE + m_Body.StructSIZE; }

        /// <summary>
        /// 封包型態
        /// </summary>
        public override PacketType PacketType { get => PacketType.UserPacket; }

        /// <summary>
        /// 封包驗證碼
        /// </summary>
        public int VerificationCode { get => m_Header.VerificationCode; set => m_Header.VerificationCode = value; }

        #endregion

        #region Constructor

        /// <summary>
        /// 標準封包 : 無Header 的封包 -> 空的封包
        /// </summary>
        /// <param name="user"></param>
        public StdPacket()
            : base(new UserPacketHeader(Emergency.None, Encryption.None, Category.None, Callback.None), new StdPacketData(null)) {
        }

        #region 用於內部預先創建的封包,等發送的時候再附上header

        /// <summary>
        /// 標準封包 : 無Header 的封包 -> byte[]
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bys_data"></param>
        public StdPacket(byte[] bys_data)
            : base(new UserPacketHeader(Emergency.None, Encryption.None, Category.None, Callback.None), new StdPacketData(bys_data)) {
        }

        /// <summary>
        /// 標準封包 : 無Header 的封包 -> short
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bys_data"></param>
        public StdPacket(int data)
            : base(new UserPacketHeader(Emergency.None, Encryption.None, Category.None, Callback.None), new StdPacketData(System.BitConverter.GetBytes(data))) {
        }

        /// <summary>
        /// 標準封包 : 無Header 的封包 -> long
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bys_data"></param>
        public StdPacket(bool data)
            : base(new UserPacketHeader(Emergency.None, Encryption.None, Category.None, Callback.None), new StdPacketData(System.BitConverter.GetBytes(data))) {
        }

        /// <summary>
        /// 標準封包 : 無Header 的封包 -> float
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bys_data"></param>
        public StdPacket(float data)
            : base(new UserPacketHeader(Emergency.None, Encryption.None, Category.None, Callback.None), new StdPacketData(System.BitConverter.GetBytes(data))) {
        }

        /// <summary>
        /// 標準封包 : 無Header 的封包 -> string
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bys_data"></param>
        public StdPacket(string data)
            : base(new UserPacketHeader(Emergency.None, Encryption.None, Category.None, Callback.None), new StdPacketData(Encoding.UTF8.GetBytes(data))) {
        }

        #endregion

        /// <summary>
        /// 標準封包 : 封包資料 -> byte[]
        /// </summary>
        /// <param name="user"></param>
        /// <param name="emergency"></param>
        /// <param name="encryption"></param>
        /// <param name="category"></param>
        /// <param name="callback"></param>
        /// <param name="bys_data"></param>
        public StdPacket(Emergency emergency, Encryption encryption, Category category, Callback callback, byte[] bys_data)
            : base(new UserPacketHeader(emergency, encryption, category, callback), new StdPacketData(bys_data)) {
        }

        /// <summary>
        /// 標準封包 : 封包資料 -> short
        /// </summary>
        /// <param name="user"></param>
        /// <param name="emergency"></param>
        /// <param name="encryption"></param>
        /// <param name="category"></param>
        /// <param name="callback"></param>
        /// <param name="data"></param>
        public StdPacket(Emergency emergency, Encryption encryption, Category category, Callback callback, short data)
            : base(new UserPacketHeader(emergency, encryption, category, callback), new StdPacketData(System.BitConverter.GetBytes(data))) {
        }

        /// <summary>
        /// 標準封包 : 封包資料 -> int
        /// </summary>
        /// <param name="user"></param>
        /// <param name="emergency"></param>
        /// <param name="encryption"></param>
        /// <param name="category"></param>
        /// <param name="callback"></param>
        /// <param name="data"></param>
        public StdPacket(Emergency emergency, Encryption encryption, Category category, Callback callback, int data)
            : base(new UserPacketHeader(emergency, encryption, category, callback), new StdPacketData(System.BitConverter.GetBytes(data))) {
        }

        /// <summary>
        /// 標準封包 : 封包資料 -> long
        /// </summary>
        /// <param name="user"></param>
        /// <param name="emergency"></param>
        /// <param name="encryption"></param>
        /// <param name="category"></param>
        /// <param name="callback"></param>
        /// <param name="data"></param>
        public StdPacket(Emergency emergency, Encryption encryption, Category category, Callback callback, long data)
            : base(new UserPacketHeader(emergency, encryption, category, callback), new StdPacketData(System.BitConverter.GetBytes(data))) {
        }

        /// <summary>
        /// 標準封包 : 封包資料 -> bool
        /// </summary>
        /// <param name="user"></param>
        /// <param name="emergency"></param>
        /// <param name="encryption"></param>
        /// <param name="category"></param>
        /// <param name="callback"></param>
        /// <param name="data"></param>
        public StdPacket(Emergency emergency, Encryption encryption, Category category, Callback callback, bool data)
            : base(new UserPacketHeader(emergency, encryption, category, callback), new StdPacketData(System.BitConverter.GetBytes(data))) {
        }

        /// <summary>
        /// 標準封包 : 封包資料 -> float
        /// </summary>
        /// <param name="user"></param>
        /// <param name="emergency"></param>
        /// <param name="encryption"></param>
        /// <param name="category"></param>
        /// <param name="callback"></param>
        /// <param name="data"></param>
        public StdPacket(Emergency emergency, Encryption encryption, Category category, Callback callback, float data)
            : base(new UserPacketHeader(emergency, encryption, category, callback), new StdPacketData(System.BitConverter.GetBytes(data))) {
        }

        /// <summary>
        /// 標準封包 : 封包資料 -> string
        /// </summary>
        /// <param name="user"></param>
        /// <param name="emergency"></param>
        /// <param name="encryption"></param>
        /// <param name="category"></param>
        /// <param name="callback"></param>
        /// <param name="data"></param>
        public StdPacket(Emergency emergency, Encryption encryption, Category category, Callback callback, string data)
            : base(new UserPacketHeader(emergency, encryption, category, callback), new StdPacketData(Encoding.UTF8.GetBytes(data))) {
        }

        /// <summary>
        /// 標準封包 : 封包資料 -> string[]
        /// </summary>
        /// <param name="user"></param>
        /// <param name="emergency"></param>
        /// <param name="encryption"></param>
        /// <param name="category"></param>
        /// <param name="callback"></param>
        /// <param name="data"></param>
        public StdPacket(Emergency emergency, Encryption encryption, Category category, Callback callback, string[] data)
            : base(new UserPacketHeader(emergency, encryption, category, callback), new StdPacketData(Converter.BitConverter.GetBytes(data, true))) {
        }

        #endregion

        /// <summary>
        /// 設定 Packet Header
        /// </summary>
        /// <param name="emergency">緊急程度</param>
        /// <param name="encryption">加密方式</param>
        /// <param name="category">類別</param>
        /// <param name="callback">封包回調</param>
        public void ResetHeaderRef(Emergency emergency, Encryption encryption, Category category, Callback callback) {
            m_Header = new UserPacketHeader(emergency, encryption, category, callback);
        }

    }
}