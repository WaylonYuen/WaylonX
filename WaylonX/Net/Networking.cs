using System;

namespace WaylonX.Net {

    /// <summary>
    /// 網絡模式
    /// </summary>
    public enum NetworkMode {
        Unknown,    //未知
        Connect,    //連線模式
        Listen,     //監聽模式
    }

    /// <summary>
    /// 網絡狀態
    /// </summary>
    public enum NetworkState {
        Unknown,    //未知
        Connected,  //已連線
        Connecting, //連線中(資料未同步)
        Disconnect, //斷開連線
        Overtime,   //超時
    }

}
