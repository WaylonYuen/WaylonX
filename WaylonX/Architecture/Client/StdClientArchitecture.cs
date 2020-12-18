using System;
using WaylonX.Architecture.BaseModel;

namespace WaylonX.Architecture.Client {

    public class ClientInfoEventArgs : CSBaseInfoEventArgs {

    }

    /// <summary>
    /// 標準客戶端架構
    /// </summary>
    public abstract class StdClientArchitecture : CSModel {

        //Constructor
        public StdClientArchitecture(string name) : base(name) {

        }

    }
}
