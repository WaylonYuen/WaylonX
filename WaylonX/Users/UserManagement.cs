using System;
using System.Collections.Generic;
using System.Threading;

namespace WaylonX.Users {

    /// <summary>
    /// 用戶管理
    /// </summary>
    public class UserManagement {

        /// <summary>
        /// 用戶列表
        /// </summary>
        public List<UserWorkItem> UserList { get; }

        public UserManagement() {
            UserList = new List<UserWorkItem>();
        }

    }

    /// <summary>
    /// 用戶工作項
    /// </summary>
    public class UserWorkItem {

        #region Property

        /// <summary>
        /// 用戶
        /// </summary>
        public User User { get; }

        /// <summary>
        /// 用戶線程工作項列表
        /// </summary>
        private List<Thread> ThreadWorkItem = new List<Thread>();

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">用戶</param>
        public UserWorkItem(User user) {
            User = user;
        }

        /// <summary>
        /// 添加用戶線程工作項
        /// </summary>
        /// <param name="thread">工作線程</param>
        /// <param name="name">線程名稱</param>
        /// <param name="hasObject">線程參數是否有對象</param>
        public void AddAndStartThread(Thread thread, string name, bool hasObject) {

            thread.Name = name;

            if (hasObject) {
                thread.Start(User);
            } else {
                thread.Start();
            }

            ThreadWorkItem.Add(thread);
        }

        /// <summary>
        /// 關閉用戶所有工作項
        /// </summary>
        public void Close() {
            //Undone: 消除User
            //Undone: 關閉List中的所有線程
        }
    }

}
