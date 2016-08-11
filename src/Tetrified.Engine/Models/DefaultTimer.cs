using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Tetrified.Engine.Models
{
    public class DefaultTimer : ITimer
    {
        private readonly IList<RunTask> _tasks;

        public DefaultTimer(TimeSpan intervalSpeed)
        {
            _tasks = new List<RunTask>();

            IntervalSpeed = intervalSpeed;
        }

        public TimeSpan IntervalSpeed { get; set; }
        
        public void RunUntil(Func<bool> operation)
        {
            var task = new RunTask(operation, IntervalSpeed);

            var inactive = _tasks.Where(t => !t.Running).ToList();

            foreach(var t in inactive)
            {
                _tasks.Remove(t);
            }

            _tasks.Add(task);

            task.Start();
        }

        public void Shutdown()
        {
            foreach(var task in _tasks)
            {
                // task.Running = false;

                try
                {
                    task.Thread.Abort();
                }
                catch { }
            }
        }

        public void Dispose()
        {
            Shutdown();
        }

        private class RunTask
        {
            public RunTask(Func<bool> operation, TimeSpan interval)
            {
                Operation = operation;
                Timer = new Stopwatch();
                Interval = interval;

                Thread = new Thread(p =>
                {
                    var t = (RunTask)p;

                    t.Timer.Start();
                    t.Running = true;

                    while (t.Running)
                    {
                        //if (t.LastRun < (Timer.Elapsed - Interval))
                        //{
                        //    t.LastRun = Timer.Elapsed;
                        //}

                        t.LastRun = Timer.Elapsed;

                        if (t.Operation())
                        {
                            break;
                        }

                        Thread.Sleep(Interval);
                    }

                    t.Running = false;
                    t.Timer.Stop();
                });
            }

            public void Start()
            {
                Thread.Start(this);
            }

            public TimeSpan Interval { get; set; }

            public Thread Thread { get; private set; }

            public Func<bool> Operation { get; private set; }

            public bool Running { get; private set; }

            public TimeSpan LastRun { get; private set; }

            public Stopwatch Timer { get; private set; }
        }
    }
}
