using System;

namespace WaylonX.Extension {

    /// <summary>
    /// 字節組方法擴展
    /// </summary>
    public static class ByteExtension {

        /// <summary>
        /// 字節組合併
        /// </summary>
        /// <param name="bys_head"></param>
        /// <param name="bys_join"></param>
        /// <returns></returns>
        public static byte[] MergedWith(this byte[] bys_head, ref byte[] bys_join) {

            var bys_result = new byte[bys_head.Length + bys_join.Length];

            bys_head.CopyTo(bys_result, 0);
            bys_join.CopyTo(bys_result, bys_head.Length);

            return bys_result;
        }

        /// <summary>
        /// 字節組分割器
        /// </summary>
        /// <param name="bys_source">被分割的字節組</param>
        /// <param name="bys_partA">A部分</param>
        /// <param name="bys_partB">B部分</param>
        /// <param name="indexOfSplittePoint">分割點索引值</param>
        /// <returns>分割是否成功</returns>
        public static bool SplittedOut(this byte[] bys_source, out byte[] bys_partA, out byte[] bys_partB, int indexOfSplittePoint) {

            if (indexOfSplittePoint <= 0 || bys_source.Length <= indexOfSplittePoint) {
                bys_partA = null;
                bys_partB = null;           
                return false;
            }

            bys_partB = new byte[bys_source.Length - indexOfSplittePoint];
            bys_partA = new byte[bys_source.Length - bys_partB.Length];

            Array.Copy(bys_source, 0, bys_partA, 0, bys_partA.Length);
            Array.Copy(bys_source, indexOfSplittePoint, bys_partB, 0, bys_partB.Length);

            return true;
        }

        /// <summary>
        /// 字節組提取器
        /// </summary>
        /// <param name="bys_source">被提取的字節組</param>
        /// <param name="bys_data">提取出來的內容</param>
        /// <param name="index">提取內容的索引值</param>
        /// <param name="dataLength">提取內容的長度</param>
        /// <returns>提取是否成功</returns>
        public static bool ExtractTo(this byte[] bys_source, out byte[] bys_data, int index, int dataLength) {

            if ((index + dataLength) >= bys_source.Length || dataLength <= 0) {
                bys_data = null;
                return false;
            }

            bys_data = new byte[dataLength];
            Array.Copy(bys_source, index, bys_data, 0, dataLength);

            return true;
        }

    }

}
