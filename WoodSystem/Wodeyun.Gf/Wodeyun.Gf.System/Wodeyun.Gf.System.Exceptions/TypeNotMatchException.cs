using System;

using System.Runtime.Serialization;

namespace Wodeyun.Gf.System.Exceptions
{
    [Serializable]
    public class TypeNotMatchException : Exception
    {
        public TypeNotMatchException()
            : base()
        { }

        public TypeNotMatchException(string message)
            : base(message)
        { }

        public TypeNotMatchException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected TypeNotMatchException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        { }
    }
}
