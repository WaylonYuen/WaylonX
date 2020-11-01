using System.Net;
using WaylonX.Converter;
using WaylonX.Extension;

namespace WaylonX.Packets {

    public class Packet : StdPacket {

        /// <summary>
        /// Std封包結構SIZE: 結構大小最小不會小於此SIZE
        /// </summary>
        public override int PacketStructSIZE => base.PacketStructSIZE;

        /// <summary>
        /// 封包型態
        /// </summary>
        public override PacketType PacketType { get => PacketType.Packet; }

        #region Constructor

        /// <summary>
        /// 標準封包 : 無Header 的封包 -> 空的封包
        /// </summary>
        /// <param name="user"></param>
        public Packet() { }

        /// <summary>
        /// 標準封包 : 無Header 的封包 -> byte[]
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bys_data"></param>
        public Packet(byte[] bys_data)
            : base(Emergency.None, Encryption.None, Category.None, Callback.None, bys_data) {
        }

        /// <summary>
        /// 標準封包 : 無Header 的封包 -> short
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bys_data"></param>
        public Packet(int data)
            : base(Emergency.None, Encryption.None, Category.None, Callback.None, data) {
        }

        /// <summary>
        /// 標準封包 : 無Header 的封包 -> long
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bys_data"></param>
        public Packet(bool data)
            : base(Emergency.None, Encryption.None, Category.None, Callback.None, data) {
        }

        /// <summary>
        /// 標準封包 : 無Header 的封包 -> float
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bys_data"></param>
        public Packet(float data)
            : base(Emergency.None, Encryption.None, Category.None, Callback.None, data) {
        }

        /// <summary>
        /// 標準封包 : 無Header 的封包 -> string
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bys_data"></param>
        public Packet(string data)
            : base(Emergency.None, Encryption.None, Category.None, Callback.None, data) {
        }

        /// <summary>
        /// 標準封包 : 封包資料 -> byte[]
        /// </summary>
        /// <param name="user"></param>
        /// <param name="emergency"></param>
        /// <param name="encryption"></param>
        /// <param name="category"></param>
        /// <param name="callback"></param>
        /// <param name="bys_data"></param>
        public Packet(Emergency emergency, Encryption encryption, Category category, Callback callback, byte[] bys_data)
            : base(emergency, encryption, category, callback, bys_data) {
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
        public Packet(Emergency emergency, Encryption encryption, Category category, Callback callback, short data)
            : base(emergency, encryption, category, callback, data) {
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
        public Packet(Emergency emergency, Encryption encryption, Category category, Callback callback, int data)
            : base(emergency, encryption, category, callback, data) {
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
        public Packet(Emergency emergency, Encryption encryption, Category category, Callback callback, long data)
            : base(emergency, encryption, category, callback, data) {
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
        public Packet(Emergency emergency, Encryption encryption, Category category, Callback callback, bool data)
            : base(emergency, encryption, category, callback, data) {
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
        public Packet(Emergency emergency, Encryption encryption, Category category, Callback callback, float data)
            : base(emergency, encryption, category, callback, data) {
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
        public Packet(Emergency emergency, Encryption encryption, Category category, Callback callback, string data)
            : base(emergency, encryption, category, callback, data) {
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
        public Packet(Emergency emergency, Encryption encryption, Category category, Callback callback, string[] data)
            : base(emergency, encryption, category, callback, data) {
        }

        #endregion

        /// <summary>
        /// 封裝
        /// </summary>
        /// <returns></returns>
        public override byte[] ToPackup() {

            //此處封裝方法被泛用型where所指定了接口（IPacketBase),因此必然有ToPackup()方法

            var bys_header = m_Header.ToPackup();   //呼叫header進行封裝
            var bys_body = m_Body.ToPackup();       //呼叫Body進行封裝

            var bys_packetData = bys_header.MergedWith(ref bys_body);   //組合封包資料

            //建立封包
            var bys_packet = new byte[BasicTypes.SizeOf.Int + bys_header.Length + bys_body.Length];

            //添加封包資訊描述 Bug
            System.BitConverter.GetBytes(IPAddress.HostToNetworkOrder(bys_header.Length + bys_body.Length)).CopyTo(bys_packet, 0);

            //打包封包
            bys_packetData.CopyTo(bys_packet, 4);

            return bys_packet;
        }

        public new Packet Unpack(byte[] bys_packet) {
            base.Unpack(bys_packet);
            return this;
        }
    }

}
