using System;
using System.Net;
using WaylonX.Converter;

namespace WaylonX.Packets.Base {

    /// <summary>
    /// 封包組合器
    /// </summary>
    /// <typeparam name="T">Packet Header</typeparam>
    /// <typeparam name="U">Packet Data</typeparam>
    public abstract class Packaged<T, U>
        where T : IPacketBase
        where U : IPacketBase {

        #region Property

        /// <summary>
        /// 封包結構尺寸
        /// </summary>
        public int StructSIZE { get { return IndexOf.Packet; } }

        /// <summary>
        /// 封包型態
        /// </summary>
        public abstract PacketType PacketType { get; }

        /// <summary>
        /// 封包描述: Header中包含所有對此封包的描述資料
        /// </summary>
        public T Header { get => m_Header; }

        /// <summary>
        /// 封包內容: 封包主體內容
        /// </summary>
        public U Body { get => m_Body; }

        #endregion

        #region Local values

        protected T m_Header;
        protected U m_Body;

        #endregion

        /// <summary>
        /// 創建標準封包組合器
        /// </summary>
        /// <param name="user"></param>
        /// <param name="packetHeader"></param>
        /// <param name="packetData"></param>
        public Packaged(T packetHeader, U packetBody) {
            m_Header = packetHeader;
            m_Body = packetBody;
        }

        public static class SizeOf {
            public const int PacketSIZE = BasicTypes.SizeOf.Int;
            public const int PacketType = BasicTypes.SizeOf.Short;
        }

        //BUG: 架構索引 -> 此索引 和 packet索引有衝突
        public static class IndexOf {

            public const int PacketSIZE = 0;
            public const int PacketType = PacketSIZE + SizeOf.PacketSIZE;

            //封包的索引位置
            public const int Packet = PacketType + SizeOf.PacketType;
        }

        #region Methods

        /// <summary>
        /// 封裝
        /// </summary>
        /// <returns></returns>
        public virtual byte[] ToPackup() {

            //此處封裝方法被泛用型where所指定了接口（IPacketBase),因此必然有ToPackup()方法
            var bys_header = m_Header.ToPackup();   //呼叫header進行封裝
            var bys_body = m_Body.ToPackup();       //呼叫Body進行封裝
            var bys_packetData = Bytes.ToPackup(ref bys_header, ref bys_body);  //組合封包資料

            //建立封包
            var bys_packet = new byte[StructSIZE + bys_header.Length + bys_body.Length];

            //添加封包資訊描述
            System.BitConverter.GetBytes(IPAddress.HostToNetworkOrder((SizeOf.PacketType + bys_header.Length + bys_body.Length))).CopyTo(bys_packet, IndexOf.PacketSIZE);  //封裝整個個封包長度(不包含長度資訊)
            System.BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)PacketType)).CopyTo(bys_packet, IndexOf.PacketType);  //封裝封包型態

            //打包封包
            bys_packetData.CopyTo(bys_packet, IndexOf.Packet);

            //返回組合封裝
            return bys_packet;
        }

        /// <summary>
        /// 解析: 不包含封包Packaged資料描述的內容
        /// </summary>
        /// <param name="bys_packet"></param>
        public virtual void Unpack(byte[] bys_packet) {

            //分割資料: Splitter返回提取內容, out剩餘內容
            m_Header.Unpack(Bytes.Splitter(out byte[] bys_data, ref bys_packet, 0, m_Header.StructSIZE));
            m_Body.Unpack(bys_data);
        }

        /// <summary>
        /// Information
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return
                base.ToString() + "\n"
                 + m_Header.ToString()
                 + m_Body.ToString();
        }

        /// <summary>
        /// 測試用
        /// </summary>
        public void GTest() {
            Console.WriteLine("Testing");
            Console.WriteLine(m_Header.ToString());
            Console.WriteLine(m_Body.ToString());
        }

        #endregion
    }

}
