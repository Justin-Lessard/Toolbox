using System;
using System.Diagnostics;
using System.Threading;

namespace Utils
{
    public class PerformanceProfiler
    {
        private static double Profile(string description, Action func, int iterations, int warmup_itteration)
        {
            //Run at highest priority to minimize fluctuations caused by other processes/threads
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            // warm up
            for (int i = 0; i < warmup_itteration; i++)
                func();

            var watch = new Stopwatch();

            // clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            watch.Start();
            for (int i = 0; i < iterations; i++)
            {
                func();
            }
            watch.Stop();

            Console.Write(description);
            Console.WriteLine(" Time Elapsed {0} ms", watch.Elapsed.TotalMilliseconds);

            return watch.Elapsed.TotalMilliseconds;
        }

        #endregion Private Methods
    }
}
