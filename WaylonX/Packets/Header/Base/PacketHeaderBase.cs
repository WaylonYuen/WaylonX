using WaylonX.Packets.Base;

namespace WaylonX.Packets.Header.Base {

    /// <summary>
    /// 所有 PacketHeader 必須派生自此抽象類並實作抽象方法
    /// </summary>
    public abstract class PacketHeaderBase : IPacketBase {

        /// <summary>
        /// PacketHeader架構長度: 資料的索引起始位置即為該架構長度
        /// </summary>
        public abstract int StructSIZE { get; }  //由IPacketBase繼承過來

        /// <summary>
        /// PacketHeader型態
        /// </summary>
        public abstract PacketHeaderType PacketHeaderType { get; }

        /// <summary>
        /// 封裝
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ToPackup();  //由IPacketBase繼承過來

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="bys_packetHeader">不包含其他資料的bys</param>
        public abstract void Unpack(byte[] bys_packet);  //由IPacketBase繼承過來

    }

}
