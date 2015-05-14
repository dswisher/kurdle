using System;


namespace Kurdle.Misc
{
    public class ProjectException : Exception
    {
        public ProjectException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}
