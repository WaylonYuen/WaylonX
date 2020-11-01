using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WaylonX.Loggers {

    public class StdLogger {

        /// <summary>
        /// log資訊佇列
        /// </summary>
        public static ConcurrentQueue<string> logQueue = new ConcurrentQueue<string>();

        /// <summary>
        /// 日誌紀錄
        /// </summary>
        private class Log {

            private readonly LogType Type;
            private readonly string Detail;

            public Log(LogType type, string detail) {
                Type = type;
                Detail = detail;
            }

            public string WriteLogs() {

                string logType = " ";
                char typeLabel = ' ';
                char typeTag = ' ';

                switch (Type) {

                    //Program
                    case LogType.Info:
                        logType = "Info";
                        typeLabel = '#';
                        //Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;

                    case LogType.GameInfo:
                        logType = "Game";
                        typeLabel = '#';
                        //Console.ForegroundColor = ConsoleColor.Cyan;
                        break;

                    case LogType.ClientInfo:
                        logType = "Client";
                        typeLabel = '#';
                        //Console.ForegroundColor = ConsoleColor.Gray;
                        break;

                    case LogType.ServerInfo:
                        logType = "Server";
                        typeLabel = '#';
                        //Console.ForegroundColor = ConsoleColor.Gray;

                        break;

                    case LogType.DatabaseInfo:
                        logType = "Database";
                        typeLabel = '#';
                        //Console.ForegroundColor = ConsoleColor.White;
                        break;

                    case LogType.Catch:
                        logType = "Catch";
                        typeLabel = '>';
                        typeTag = '%';
                        //Console.ForegroundColor = ConsoleColor.Green;
                        break;

                    case LogType.Warn:
                        logType = "Warning";
                        typeLabel = '>';
                        typeTag = '@';
                        //Console.ForegroundColor = ConsoleColor.Yellow;
                        break;

                    case LogType.Error:
                        logType = "Error";
                        typeLabel = '>';
                        typeTag = '!';
                        //Console.ForegroundColor = ConsoleColor.Red;
                        break;

                    case LogType.Correct:
                        logType = "Correct";
                        typeLabel = 'R';
                        typeTag = '<';
                        break;

                    default:
                        //沒有這個類別
                        break;
                }

                //整合
                string Logs = $"  {typeTag} {DateTime.Now:dd/MM/yyyy HH:mm:ss-fff} [ {typeLabel} {logType,2} ]: Details -> {Detail}";

                //Undone: 寫到外部Log日誌

                return Logs;               
            }

            /// <summary>
            /// 將日誌Push到Logger自帶的佇列中
            /// </summary>
            public void EnQueue() {
                logQueue.Enqueue(WriteLogs());
                //Console.WriteLine(WriteLogs());
                //Console.ResetColor();
            }
        }

        /// <summary>
        /// 日誌容器
        /// </summary>
        public class Container {

            private readonly LogType Type;

            private readonly string Discription;

            private readonly string BaseToString;

            private Dictionary<string, string> containerDict = new Dictionary<string, string>();

            public Container(LogType type, string discription, string baseToString) {
                Type = type;
                Discription = discription;
                BaseToString = baseToString;
            }

            public void Add(string title, string detail) {
                containerDict.Add(title, detail);
            }

            public string WriteLogs() {

                int maxLength = 0;

                //取得最長的title 以便做格式化
                foreach (var info in containerDict) {
                    if (maxLength < info.Key.Length) {
                        maxLength = info.Key.Length;
                    }
                }


                string logType = " ";
                char typeLabel = ' ';
                char typeTag = 'v';

                switch (Type) {

                    case LogType.Info:
                        logType = "Info";
                        typeLabel = '#';
                        //Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;

                    case LogType.GameInfo:
                        logType = "Game";
                        typeLabel = '#';
                        //Console.ForegroundColor = ConsoleColor.Cyan;
                        break;

                    case LogType.ClientInfo:
                        logType = "Client";
                        typeLabel = '#';
                        //Console.ForegroundColor = ConsoleColor.Gray;
                        break;

                    case LogType.ServerInfo:
                        logType = "Server";
                        typeLabel = '#';
                        //Console.ForegroundColor = ConsoleColor.Gray;
                        break;

                    case LogType.DatabaseInfo:
                        logType = "Database";
                        typeLabel = '#';
                        //Console.ForegroundColor = ConsoleColor.White;
                        break;

                    case LogType.Catch:
                        logType = "Catch";
                        typeLabel = '>';
                        typeTag = '%';
                        //Console.ForegroundColor = ConsoleColor.Green;
                        break;

                    case LogType.Warn:
                        logType = "Warning";
                        typeLabel = '>';
                        typeTag = '@';
                        //Console.ForegroundColor = ConsoleColor.Yellow;
                        break;

                    case LogType.Error:
                        logType = "Error";
                        typeLabel = '>';
                        typeTag = '!';
                        //Console.ForegroundColor = ConsoleColor.Red;
                        break;

                    case LogType.Correct:
                        logType = "Correct";
                        typeLabel = 'R';
                        typeTag = '<';
                        break;

                    default:
                        //沒有這個類別
                        break;
                }

                string str_Logs = "\n  " + typeTag + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss-fff") + " [ " + typeLabel + " " + logType + " ]: Details -> " + Discription + " : " + BaseToString + "\n    Info: |\n";

                int endingLenght = 0;
                foreach (var info in containerDict) {

                    //動態調整空格對齊
                    str_Logs += "\t  |  >>  ";
                    for (int i = 0; i < (maxLength - info.Key.Length); i++) {
                        str_Logs += " ";
                    }
                    str_Logs += info.Key + " : " + info.Value + "\n";

                    if ((maxLength + info.Value.Length) > endingLenght) {
                        endingLenght = maxLength + info.Value.Length;
                    }
                }

                //動態調整結尾包裹
                str_Logs += "\t  |";

                int Offset = 10;
                for (int i = 0; i < endingLenght + Offset; i++) {
                    str_Logs += "-";
                }
                str_Logs += "end]\n";

                //Undone: 寫到外部Log日誌

                return str_Logs;
            }

            public string Excute() {
                string logs = WriteLogs();
                logQueue.Enqueue(logs);
                //Console.WriteLine(logs);
                //Console.ResetColor();

                return logs;
            }
        }

        #region Logged Methods

        public void Info(string content) {
            var Log = new Log(LogType.Info, content);
            Log.EnQueue();
        }

        public void GameInfo(string content) {
            var Log = new Log(LogType.GameInfo, content);
            Log.EnQueue();
        }

        public void ClientInfo(string content) {
            var Log = new Log(LogType.ClientInfo, content);
            Log.EnQueue();
        }

        public void ServerInfo(string content) {
            var Log = new Log(LogType.ServerInfo, content);
            Log.EnQueue();
        }

        public void DatabaseInfo(string content) {
            var Log = new Log(LogType.DatabaseInfo, content);
            Log.EnQueue();
        }

        public void Debug(string content) {
            var Log = new Log(LogType.Debug, content);
            Log.EnQueue();
        }

        public void Warn(string content) {
            var Log = new Log(LogType.Warn, content);
            Log.EnQueue();
        }

        public void Error(string content) {
            var Log = new Log(LogType.Error, content);
            Log.EnQueue();
        }

        /// <summary>
        /// 取得Log容器: 用以儲存相關的多筆需要紀錄的資訊
        /// </summary>
        /// <param name="discription">容器描述 -> 此容器紀錄的資訊概括</param>
        /// <param name="baseToString">來自被呼叫的父類方法ToString</param>
        /// <returns>Log容器</returns>
        public Container GetContainer(LogType type, string discription, string baseToString) {
            return new Container(type, discription, baseToString);
        }

        public void Testing() {

            Console.WriteLine("Start Test");
            while (logQueue.Count > 0) {
                logQueue.TryDequeue(out string logged);
                Console.WriteLine(logged);
            }
            Console.WriteLine("End Test");
        }
        #endregion

        public enum LogType {
            Info,
            GameInfo,
            ClientInfo,
            ServerInfo,
            DatabaseInfo,
            Debug,
            Warn,
            Error,
            Catch,
            Correct,
        }
    }
}
