using System;

namespace WaylonX.Converter {

    /// <summary>
    /// 字節處理
    /// </summary>
    public static class Bytes {

        /// <summary>
        /// 提取內容：提取指定位置內容
        /// </summary>
        /// <param name="source_Bytes">被提取的Bytes</param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// /// <param name="InfoStr"></param>
        /// <returns></returns>
        public static byte[] Extract(byte[] source_Bytes, int length) {

            try {
                var dataResult_Bytes = new byte[length];
                Array.Copy(source_Bytes, 0, dataResult_Bytes, 0, length);
                return dataResult_Bytes;

            } catch (Exception e) {
                var Undone = e.Message;
                //Display.Format(State.Catch, "提取內容錯誤", $"Extract() >> {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 提取內容：提取指定位置內容
        /// </summary>
        /// <param name="source_Bytes">被提取的Bytes</param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// /// <param name="InfoStr"></param>
        /// <returns></returns>
        public static byte[] Extract(byte[] source_Bytes, int startIndex, int length) {

            try {
                var dataResult_Bytes = new byte[length];
                Array.Copy(source_Bytes, startIndex, dataResult_Bytes, 0, length);
                return dataResult_Bytes;

            } catch (Exception e) {
                var Undone = e.Message;
                //Display.Format(State.Catch, "提取內容錯誤", $"Extract() >> {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 提取內容：提取指定位置內容
        /// </summary>
        /// <param name="source_Bytes">被提取的Bytes</param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// /// <param name="InfoStr"></param>
        /// <returns></returns>
        public static byte[] Extract(byte[] source_Bytes, int startIndex, int destinationIndex, int length) {

            try {
                var dataResult_Bytes = new byte[length];
                Array.Copy(source_Bytes, startIndex, dataResult_Bytes, destinationIndex, length);
                return dataResult_Bytes;

            } catch (Exception e) {
                var Undone = e.Message;
                //Display.Format(State.Catch, "提取內容錯誤", $"Extract() >> {e.Message}");
                return null;
            }
        }

        //Hack: need -> 範圍檢查
        /// <summary>
        /// 字節分離器 : out剩餘內容
        /// </summary>
        /// <param name="bys_remainder">賦值剩餘內容</param>
        /// <param name="bys_source">源資料</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="length">提取內容的長度</param>
        /// <returns>提取內容</returns>
        public static byte[] Splitter(out byte[] bys_remainder, ref byte[] bys_source, int startIndex, int length) {

            //範圍檢查:
            //~ startIndex必須大於0、 length必須大於0
            //~ (startIndex + length) < bys_source.Length, 終點索引位置必須要小於源資料長度

            //提取目標資料
            var bys_result = new byte[length];
            Array.Copy(bys_source, startIndex, bys_result, 0, length);

            //輸出剩餘資料
            bys_remainder = new byte[bys_source.Length - length];
            Array.Copy(bys_source, startIndex + length, bys_remainder, 0, bys_source.Length - length);

            return bys_result;  //返回目標資料
        }

        /// <summary>
        /// 字節組合併
        /// </summary>
        /// <param name="bys_head"></param>
        /// <param name="bys_join"></param>
        /// <returns></returns>
        public static byte[] ToPackup(ref byte[] bys_head, ref byte[] bys_join) {

            //if (bys_head == null && bys_join != null) return bys_join;
            //if (bys_head != null && bys_join == null) return bys_head;
            //if (bys_head == null && bys_join == null) return null;

            var bys_result = new byte[bys_head.Length + bys_join.Length];

            bys_head.CopyTo(bys_result, 0);
            bys_join.CopyTo(bys_result, bys_head.Length);

            return bys_result;
        }

    }

}
