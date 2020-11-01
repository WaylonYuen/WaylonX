using System;

namespace WaylonX.Packets.Header.Base {

    /// <summary>
    /// 數據包身份識別
    /// </summary>
    public interface IPacketHeaderIdentity {

        /// <summary>
        /// 驗證碼
        /// </summary>
        int VerificationCode { get; set; }

    }

    /// <summary>
    /// 數據包安全
    /// </summary>
    public interface IPacketHeaderSecurity {

        /// <summary>
        /// 加密方法
        /// </summary>
        Encryption EncryptionType { get; }

    }

    /// <summary>
    /// 網路數據包線程描述
    /// </summary>
    public interface IPacketHeaderThreads {

        /// <summary>
        /// 緊急程度
        /// </summary>
        Emergency EmergencyType { get; set; }

        /// <summary>
        /// 封包類別: 風包頻道
        /// </summary>
        Category CategoryType { get; }

        /// <summary>
        /// 封包回調
        /// </summary>
        Callback CallbackType { get; }
    }

}
