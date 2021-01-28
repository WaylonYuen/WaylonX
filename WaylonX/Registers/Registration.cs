using System;

namespace WaylonX.Registers {

    public interface IRegistration {

        /// <summary>
        /// 執行回調註冊器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRegister(object sender, EventArgs e);
    }

    /// <summary>
    /// 註冊器
    /// </summary>
    public class Registration {

        /// <summary>
        /// 註冊器事件
        /// </summary>
        public event EventHandler Register;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="handler"></param>
        public Registration() { }

        /// <summary>
        /// 訂閱器:繼承了IRegister的類別會被此方法訂閱
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public T Subscriber<T>(T obj) {

            if (obj is IRegistration registration) {
                this.Register += registration.OnRegister;
            }

            return obj;
        }

        /// <summary>
        /// 退訂器:繼承了IRegister的類別會被此方法退訂
        /// </summary>
        /// <typeparam name="T">泛用型</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public T Unsubscriber<T>(T obj) {

            if (obj is IRegistration registration) {
                this.Register -= registration.OnRegister;
            }

            return obj;
        }

        /// <summary>
        /// 執行器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool Excute(object sender, EventArgs e) {

            if (Register != null) {
                Register.Invoke(this, e);
                return true;
            }

            return false;
        }

    }

}
