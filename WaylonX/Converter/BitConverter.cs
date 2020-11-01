using System;
using System.Net;
using System.Text;

namespace WaylonX.Converter {

    public static class BitConverter {

        public static byte[] GetBytes(string[] strs_data, bool useNetOrder) {

            //計算MsgBody的長度
            int strs_dataLenght = 0;

            //計算每個字串組中的長度總和 e.g: string[3] strs_data = {"Hello", "Welcome", "Hi"}; 分別為： 5 + 7 + 2 = strs_dataLenght;
            for (int i = 0; i < strs_data.Length; i++) {
                if (strs_data[i] == "") break;
                strs_dataLenght += Encoding.UTF8.GetBytes(strs_data[i]).Length;
            }

            //定義封包體的字節數組: 保留每個字串組前4Bytes，以保存每組的字串長度。
            byte[] bys_data = new byte[strs_dataLenght + (strs_data.Length * 4)];

            //紀錄存入消息體數組的字節數目前的索引位置
            int tmp_index = 0;
            for (int i = 0; i < strs_data.Length; i++) {

                //單個消息，單個字串組
                byte[] bys_tmp = Encoding.UTF8.GetBytes(strs_data[i]); //將第i個字串組取出

                //計算第i個字組長度存放到tmp_index中
                if (useNetOrder) {                 
                    System.BitConverter.GetBytes(IPAddress.HostToNetworkOrder(bys_tmp.Length)).CopyTo(bys_data, tmp_index);
                } else {
                    System.BitConverter.GetBytes(bys_tmp.Length).CopyTo(bys_data, tmp_index);
                }

                tmp_index += 4; //右邊往左邊存， 1個int = 4bytes
                bys_tmp.CopyTo(bys_data, tmp_index);    //存入
                tmp_index += bys_tmp.Length; //加上字串組長度，索引至新位置；準備存第二組字串組。
            }

            return bys_data;
        }

    }

}
