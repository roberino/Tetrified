using System;

namespace Tetrified.Engine.Models
{
    public interface ITimer
    {
        TimeSpan IntervalSpeed { get; set; }
        void RunUntil(Func<bool> operation);
    }
}
