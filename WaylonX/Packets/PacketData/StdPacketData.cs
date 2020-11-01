using System;
using WaylonX.Packets.PacketData.Base;

namespace WaylonX.Packets.PacketData {

    /// <summary>
    /// 標準資料封包架構
    /// </summary>
    public class StdPacketData : PacketDataBase {

        #region Property

        /// <summary>
        /// StdPacketHeader架構長度: 資料的索引起始位置即為該架構長度
        /// </summary>
        public override int StructSIZE { get => IndexOf.Data; }

        /// <summary>
        /// PacketData型態
        /// </summary>
        public override PacketDataType PacketDataType { get => PacketDataType.StdPacketData; }  //指定封包資料型態

        #endregion

        #region Constructor

        /// <summary>
        /// 標準封包構造器
        /// </summary>
        /// <param name="bys_data">內容資料</param>
        public StdPacketData(byte[] bys_data) {
            Bys_data = bys_data;    //資料保存
        }

        #endregion

        #region Inner class

        public static class IndexOf {
            public const int Data = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 封裝
        /// </summary>
        /// <returns></returns>
        public override byte[] ToPackup() {
            return Bys_data;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="bys_packetData"></param>
        public override void Unpack(byte[] bys_packetData) {
            Bys_data = bys_packetData;
        }

        /// <summary>
        /// Information
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return
                "\n" + base.ToString() + ":\n"
                 + "----------------------------------------------\n"
                 + "DataLength\t" + Bys_data.Length + "\n"
                 + "End-------------------------------------------\n\n";
        }

        /// <summary>
        /// 測試用
        /// </summary>
        public static void Testing() {

            var packetData = new StdPacketData(BitConverter.GetBytes(1234));

            var bys_data = packetData.ToPackup();

            packetData.Unpack(bys_data);
            Console.WriteLine(packetData.ToString());

            Console.WriteLine($"data: {BitConverter.ToInt32(packetData.Bys_data, 0)}");

        }

        #endregion
    }
}
