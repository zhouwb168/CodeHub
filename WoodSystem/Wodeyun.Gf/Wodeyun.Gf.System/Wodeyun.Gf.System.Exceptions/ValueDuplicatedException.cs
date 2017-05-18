using System;

using System.Runtime.Serialization;

namespace Wodeyun.Gf.System.Exceptions
{
    [Serializable]
    public class ValueDuplicatedException : Exception
    {
        public ValueDuplicatedException()
            : base()
        { }

        public ValueDuplicatedException(string message)
            : base(message)
        { }

        public ValueDuplicatedException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected ValueDuplicatedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        { }
    }
}
