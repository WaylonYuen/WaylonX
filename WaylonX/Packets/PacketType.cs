
namespace WaylonX.Packets {

    #region PacketType

    /// <summary>
    /// 封包架構型態
    /// </summary>
    public enum PacketType {
        None = 0,
        StdPacket,
        UserPacket,
        GamerPacket,
        Packet,
    }

    public enum PacketHeaderType {
        None = 0,
        StdPacketHeader,
        UserPacketHeader,
        GamerPacketHeader,
    }

    public enum PacketDataType {
        None = 0,
        StdPacketData,
    }

    #endregion

    /// <summary>
    /// 加密方式
    /// </summary>
    public enum Encryption {
        None = 0,
        Testing,

        RES256,
    }

    /// <summary>
    /// 緊急等級：判斷封包緊急程度(數值越大越緊急)
    /// </summary>
    public enum Emergency {
        None = 0,
        Testing,

        Level1,
        Level2,
        Level3,
    }






    /// <summary>
    /// 封包類別Type：判斷封包類型並分配到指定佇列等待線程處理 
    /// </summary>
    /// 
    /// Info佇列：         處理一般資訊
    /// Room佇列：         處理房間資料(包括同步資料)
    /// GameLogic佇列：    處理遊戲邏輯
    /// Database佇列：     處理遊戲資料
    /// GameData佇列：     遊戲資料加載
    public enum Category {
        None = 0,
        Testing,

        //緊急處理
        Emergency,

        #region Queue
        //一般(線程池)
        General, System,

        //遊戲資料同步
        RoomMgmt,
        Gaming,
        //Room,

        //特殊(獨立線程)
        SpecialCircumstances,

        //用戶資料：所有用戶數據資料
        Database,
        #endregion

        SIZE,
    }

    /// <summary>
    /// 回調方式：Delegate對方法回調的判斷
    /// </summary>
    public enum Callback {
        None = 0,
        Testing,
        Login,

        //指令
        ChessCmd,       //棋子指令
        ChessToBuild,
        SearchRoom,
        StartGame,      //遊戲開始指令
        RoundAnimation, //回合動畫

        //資料
        GamerData,      //玩家資料同步
        RoomData,       //房間資料同步
        RoundData,      //時間同步


        PacketHeaderSync,
        SIZE,
    }

}


