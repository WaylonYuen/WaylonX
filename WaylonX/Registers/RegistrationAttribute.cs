using System;

namespace WaylonX.Registers {

    /// <summary>
    /// 註冊器特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)] //只能夠描述class
    public class RegistrationAttribute : Attribute {

        /// <summary>
        /// 自身是否擁有字典,用於註冊
        /// </summary>
        public bool HasDict { get => hasDict; }

        /// <summary>
        /// 是否擁有字典
        /// </summary>
        private readonly bool hasDict;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hasDict">自身是否擁有字典</param>
        public RegistrationAttribute(bool hasDict) {
            this.name = "null";
            this.hasDict = hasDict;
        }


        //Test

        public string Name { get => name; }

        private readonly string name;

        public RegistrationAttribute(string name, bool hasDict) {
            this.name = name;
            this.hasDict = hasDict;
        }



        /// <summary>
        /// 檢查是否標記了指定特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T GetFieldValue<T>(T obj) {

            var attributes = obj.GetType().GetCustomAttributes(typeof(RegistrationAttribute), false);

            foreach (var attribute in attributes) {
                if (attribute is RegistrationAttribute a) {

                    Console.WriteLine($"attributes: {a.Name}");

                    //Test
                    //if (a.HasDict) {
                    //    Console.WriteLine($"hasDict: {obj}");
                    //}
                    //Console.WriteLine($"Attributes: {obj}");


                }
            }

            return obj;
        }

    }
}
