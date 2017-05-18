using System;

using System.Runtime.Serialization;

namespace Wodeyun.Gf.System.Exceptions
{
    [Serializable]
    public class ValueNotFoundException : Exception
    {
        public ValueNotFoundException()
            : base()
        { }

        public ValueNotFoundException(string message)
            : base(message)
        { }

        public ValueNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected ValueNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        { }
    }
}
