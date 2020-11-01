using System;

namespace WaylonX {

    public class BasicTypes {

        public struct SizeOf {

            public const int Boolean = 1;
            public const int Int = 4;
            public const int Short = 2;
            public const int Long = 8;
            public const int Float = 4;
            public const int Double = 8;
        }

        public enum TypeOf {
            None,
            Byte,
            Bytes,
            Boolean,
            Short,
            Int,
            Long,
            Char,
            String,
            Strings,
        }
    }
}
