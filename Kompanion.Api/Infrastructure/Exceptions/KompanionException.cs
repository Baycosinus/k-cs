using System;

namespace Kompanion.Api.Infrastructure.Exceptions
{
    public class KompanionException : Exception
    {
        public KompanionException(int code, string message) : base(message)
        {
            base.HResult = code;
        }
    }
}