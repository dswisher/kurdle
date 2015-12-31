using System;


namespace Kurdle
{
    public class KurdleException : Exception
    {
        public KurdleException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}
