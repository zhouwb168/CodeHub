using System;

using System.Runtime.Serialization;

namespace Wodeyun.Gf.System.Exceptions
{
    [Serializable]
    public class ConnectionLostException : Exception
    {
        public ConnectionLostException()
            : base()
        { }

        public ConnectionLostException(string message)
            : base(message)
        { }

        public ConnectionLostException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected ConnectionLostException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        { }
    }
}
