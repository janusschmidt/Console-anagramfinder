using System;
using System.Diagnostics;
using System.Globalization;

namespace Findkaninen
{
    public class LapTimer
    {
        private readonly Stopwatch timer = new Stopwatch();
        private long lastLapTimeMilliseconds;

        public long ElapsedMilliseconds
        {
            get { return timer.ElapsedMilliseconds; } 
        }

        public void Start()
        {
            timer.Restart();
            lastLapTimeMilliseconds = 0;
        }

        public void AppendFormat(string commentFormat, params object[] args)
        {
            Console.Write(commentFormat, args);
        }

        public void AppendLine()
        {
            Console.WriteLine();
        }
        
        public long Lap(string commentFormat)
        {
            var laptime = timer.ElapsedMilliseconds - lastLapTimeMilliseconds;
            Console.WriteLine(commentFormat, laptime);
            lastLapTimeMilliseconds += timer.ElapsedMilliseconds;
            return laptime;
        }

        public long Lap()
        {
            var laptime = timer.ElapsedMilliseconds - lastLapTimeMilliseconds;
            Console.WriteLine(laptime.ToString(CultureInfo.InvariantCulture));
            lastLapTimeMilliseconds += timer.ElapsedMilliseconds;
            return laptime;
        }

        public void Total(string commentFormat)
        {
            Console.WriteLine(commentFormat, timer.ElapsedMilliseconds);
        }
    }
}
