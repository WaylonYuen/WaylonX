using System;
using MySql.Data.MySqlClient;
using WaylonX.Architecture;

namespace WaylonX_Database {

    /// <summary>
    /// 資料庫連線資訊EventArgs
    /// </summary>
    public class DatabaseConnectInfoEventArgs : EventArgs {

        /// <summary>
        /// 連接地址
        /// </summary>
        public string DBHost { get; set; }

        /// <summary>
        /// 用戶ID
        /// </summary>
        public string DBUser { get; set; }

        /// <summary>
        /// 數據庫名稱
        /// </summary>
        public string DBName { get; set; }

        /// <summary>
        /// 連接端口
        /// </summary>
        public string DBPort { get; set; }

        /// <summary>
        /// 字型協議
        /// </summary>
        public string DBFormat { get; set; }

        /// <summary>
        /// 數據庫密碼
        /// </summary>
        private string DBPass;

        /// <summary>
        /// 設定資料庫密碼
        /// </summary>
        /// <param name="pw"></param>
        public DatabaseConnectInfoEventArgs InputPassword(string pw) {
            DBPass = pw;
            return this;
        }

        /// <summary>
        /// 取得連線資訊字串
        /// </summary>
        /// <returns></returns>
        public string GetConnInfoStr() {
            return "server=" + DBHost +
                    ";user=" + DBUser +
                    ";database=" + DBName +
                    ";port=" + DBPort +
                    ";password=" + DBPass +
                    ";CharSet=" + DBFormat;
        }

    }

    /// <summary>
    /// 資料庫基本架構
    /// </summary>
    public abstract class DBBase_Catalina : CSDArchitecture_Catalina, IMySqlCmd {

        #region Property

        /// <summary>
        /// MySql連線器
        /// </summary>
        public static MySqlConnection DBConnection { get; protected set; }

        #endregion

        //Constructor
        public DBBase_Catalina(string name) : base(name) {

        }

        #region Methods

        private static class MySqlCmds {

            //查詢
            public const string Search = "select @select from @from where @where";
            public static MySqlCommand SearchCmd = new MySqlCommand(Search, DBConnection);

            //添加
            public const string Insert = "Insert into @into Values(@values)";
            public static MySqlCommand InsertCmd = new MySqlCommand(Insert, DBConnection);

            //更新
            public const string Update = "Update @from Set @set Where @where";
            public static MySqlCommand UpdateCmd = new MySqlCommand(Update, DBConnection);

            //刪除
            //public const string Delete =
            //public static MySqlCommand DeleteCmd = new MySqlCommand(Delete, DBConnection);

            //純量 or Functions
            public const string Scalar = "select @func from @from where @where";
            public static MySqlCommand ScalarCmd = new MySqlCommand(Scalar, DBConnection);

        }

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        bool IMySqlCmd.Search(string select, string from, string where) {

            var cmd = new MySqlCommand(MySqlCmds.Search, DBConnection);

            cmd.Parameters.AddWithValue("select", select);
            cmd.Parameters.AddWithValue("from", from);
            cmd.Parameters.AddWithValue("where", where);

            return ExcuteSearch(cmd);
        }

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        bool IMySqlCmd.Search(string MySqlCmdStr) {
            return ExcuteSearch(new MySqlCommand(MySqlCmds.Search, DBConnection));
        }

        /// <summary>
        /// 執行查找
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private bool ExcuteSearch(MySqlCommand cmd) {
            bool isExist = true;
            using MySqlDataReader DataReader = cmd.ExecuteReader();
            return isExist &= DataReader.Read();
        }

        #region NonQuery()

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        int IMySqlCmd.Insert(string into, string values) {

            var cmd = new MySqlCommand(MySqlCmds.Insert, DBConnection);

            cmd.Parameters.AddWithValue("into", into);
            cmd.Parameters.AddWithValue("values", values);

            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        int IMySqlCmd.Update(string from, string set, string where) {

            var cmd = new MySqlCommand(MySqlCmds.Search, DBConnection);

            cmd.Parameters.AddWithValue("from", from);
            cmd.Parameters.AddWithValue("set", set);
            cmd.Parameters.AddWithValue("where", where);

            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Undone: 刪除
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        int IMySqlCmd.Delete(MySqlCommand cmd) {
            return cmd.ExecuteNonQuery();
        }

        #endregion

        /// <summary>
        /// 純量運算
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        object IMySqlCmd.Scalar(string func, string from, string where) {

            var cmd = new MySqlCommand(MySqlCmds.Search, DBConnection);

            cmd.Parameters.AddWithValue("func", func);
            cmd.Parameters.AddWithValue("from", from);
            cmd.Parameters.AddWithValue("where", where);

            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// 通用
        /// </summary>
        /// <param name="cmdStr"></param>
        /// <returns></returns>
        MySqlCommand IMySqlCmd.Universal(string cmdStr) {
            return new MySqlCommand(cmdStr, DBConnection);
        }

        #endregion

    }

}
