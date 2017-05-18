using System;

using System.Runtime.Serialization;

namespace Wodeyun.Gf.System.Exceptions
{
    [Serializable]
    public class ValueFoundException : Exception
    {
        public ValueFoundException()
            : base()
        { }

        public ValueFoundException(string message)
            : base(message)
        { }

        public ValueFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected ValueFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        { }
    }
}
