using System;
using System.Collections.Concurrent;
using System.Threading;
using WaylonX.Packets;

namespace WaylonX.Threading {

    public interface IThread {
        ConcurrentDictionary<Callback, CallbackHandler> CallbackDict { get; }
    }

    public class Thread {

        /// <summary>
        /// 創建無參數線程
        /// </summary>
        /// <param name="method"></param>
        /// <param name="isBackground"></param>
        /// <returns></returns>
        public static System.Threading.Thread Create(ThreadStart method, bool isBackground) {
            return new System.Threading.Thread(method) { IsBackground = isBackground };
        }

        /// <summary>
        /// 創建帶參數線程
        /// </summary>
        /// <param name="medthod"></param>
        /// <returns></returns>
        public static System.Threading.Thread Create(ParameterizedThreadStart method, bool isBackground) {
            return new System.Threading.Thread(method) { IsBackground = isBackground };
        }
    }
}
