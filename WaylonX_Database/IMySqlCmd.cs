using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace WaylonX_Database {

    public interface IMySqlCmd {

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        bool Search(string select, string from, string where);

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        bool Search(string MySqlCmdStr);

        #region NonQuery()

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        int Insert(string into, string values);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        int Update(string from, string set, string where);

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        int Delete(MySqlCommand cmd);

        #endregion

        /// <summary>
        /// 純量運算
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        object Scalar(string func, string from, string where);

        /// <summary>
        /// 通用
        /// </summary>
        /// <param name="cmdStr"></param>
        /// <returns></returns>
        MySqlCommand Universal(string cmdStr);

    }
}
