using System;
using System.Collections.Concurrent;

namespace WaylonX.Packets {

    public static class PacketBuffers<TCategory, TCallback, THandler> {

        /// <summary>
        /// 佇列字典
        /// </summary>
        public static ConcurrentDictionary<TCategory, ConcurrentQueue<CallbackHandlerPacket>> QueueDict;

        /// <summary>
        /// 回調字典
        /// </summary>
        public static ConcurrentDictionary<TCategory, ConcurrentDictionary<TCallback, THandler>> CallbackDict;


        public static void QueueRegister() {

            //if (!QueueDict.ContainsKey(callback)) {
            //    CallbackDict.TryAdd(callback, handler);
            //    return;
            //}

            ////回調已經註冊過了
        }

        public static void CallbackRegister() {

        }

    }

}
