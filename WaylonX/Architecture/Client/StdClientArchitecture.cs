using System;

namespace WaylonX.Architecture.Client {

    public class ClientInfoEventArgs : CSBaseInfoEventArgs {

    }

    /// <summary>
    /// 標準客戶端架構
    /// </summary>
    public abstract class StdClientArchitecture : CSBase_Catalina {

        //Constructor
        public StdClientArchitecture(string name) : base(name) {

        }

    }
}
