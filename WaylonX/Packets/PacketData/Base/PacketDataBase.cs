using System;
using WaylonX.Packets.Base;
using WaylonX;

namespace WaylonX.Packets.PacketData.Base {

    /// <summary>
    /// 封包資料基類: 所有封包資料必須衍生自此抽象類
    /// </summary>
    public abstract class PacketDataBase : IPacketBase {

        /// <summary>
        /// PacketData架構長度: 資料的索引起始位置即為該架構長度
        /// </summary>
        public abstract int StructSIZE { get; }

        /// <summary>
        /// PacketData型態
        /// </summary>
        public abstract PacketDataType PacketDataType { get; }

        /// <summary>
        /// 主要資料 : Packet所要傳達的真正內容資訊
        /// </summary>
        public byte[] Bys_data { get; protected set; }

        /// <summary>
        /// 封包資料的長度描述
        /// </summary>
        public static class SizeOf {

            /// <summary>
            /// 封包型態的長度
            /// </summary>
            public const int packetType = BasicTypes.SizeOf.Short;

            /// <summary>
            /// 資料內容的長度
            /// </summary>
            public const int DataLength = BasicTypes.SizeOf.Int;

        }

        /// <summary>
        /// 封裝
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ToPackup();

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="bys_packetHeader">不包含其他資料的bys</param>
        public abstract void Unpack(byte[] bys_packet);
    }
}
