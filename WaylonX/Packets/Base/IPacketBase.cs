using System;
using WaylonX.Users;

namespace WaylonX.Packets.Base {

    // IPackets - 封包架構介面(接口)
    // Explain:
    //  ～使用者可以根據自己的需求來繼承Customer介面(接口)以實現指定樣式的特殊封包架構.
    //  ～

    public interface IPacketBase {

        /// <summary>
        /// 封包架構長度:
        /// 資料的索引起始位置(即: 架構長度)
        /// </summary>
        int StructSIZE { get; }

        /// <summary>
        /// 封包封裝
        /// </summary>
        /// <returns>封包字節組</returns>
        byte[] ToPackup();

        /// <summary>
        /// 封包解析:
        /// 解析結果保存到對應類別架構中, 因此沒有返回值
        /// </summary>
        /// <param name="bys_packet">封包字節組</param>
        void Unpack(byte[] bys_packet);
    }

}
