using System;
using WaylonX.Loggers;
using WaylonX.Packets;
using WaylonX.Threading;

namespace WaylonX.Cloud {

    public static class Shared {

        /// <summary>
        /// 任務緩衝區: 必須註冊
        /// </summary>
        public static TaskBuffer<Category, Callback, CallbackHandler> TaskBuffer { get; set; }

        /// <summary>
        /// 日誌
        /// </summary>
        public static StdLogger Logger { get; set; }

        /// <summary>
        /// 創建實例
        /// </summary>
        public static void Instance() {
            TaskBuffer = new TaskBuffer<Category, Callback, CallbackHandler>();
            Logger = new StdLogger();
        }

    }
}
