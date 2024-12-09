using System.Diagnostics;

namespace StardewFixes.Utilities
{
    public static class Performance
    {
        public static double GetCpuUsage()
        {
            try
            {
                using (var process = Process.GetCurrentProcess())
                {
                    TimeSpan totalCpuTime = process.TotalProcessorTime;

                    double elapsedSeconds = (DateTime.Now - process.StartTime).TotalSeconds;
                    double cpuUsage = (totalCpuTime.TotalSeconds / elapsedSeconds) * 100 / Environment.ProcessorCount;

                    return cpuUsage;
                }
            }
            catch
            {
                return 0.0;
            }
        }

        public static double GetMemoryUsage()
        {
            try
            {
                using (var process = Process.GetCurrentProcess())
                {
                    return process.WorkingSet64 / (1024.0 * 1024.0);
                }
            }
            catch
            {
                return 0.0;
            }
        }
    }
}
