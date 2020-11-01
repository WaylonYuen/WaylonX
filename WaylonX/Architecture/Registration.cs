using System;

namespace WaylonX.Architecture {

    /// <summary>
    /// 佇列回調註冊器
    /// </summary>
    public static class Registration {

        /// <summary>
        /// 監聽註冊器事件: 封包接收頻道
        /// </summary>
        public static event EventHandler MonitorRegister;

        /// <summary>
        /// 回調註冊器事件
        /// </summary>
        public static event EventHandler CallbackRegister;

        /// <summary>
        /// 執行註冊器
        /// </summary>
        public static void Excute() {

            //創建封包頻道
            if (MonitorRegister != null) {
                MonitorRegister.Invoke(null, EventArgs.Empty);
            }

            //創建封包回調
            if (CallbackRegister != null) {
                CallbackRegister.Invoke(null, EventArgs.Empty);
            }
        }

    }
}
