using System;

namespace Tetrified.Engine.Models
{
    public interface ITimer : IDisposable
    {
        TimeSpan IntervalSpeed { get; set; }
        void RunUntil(Func<bool> operation);

        void Shutdown();
    }
}
