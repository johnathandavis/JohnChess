using System;
using System.Collections.Generic;
using System.Threading;

namespace JohnChess.AI
{
    public class HighSpeedTelemetry
    {
        private long counter = 0;
        private long[] vals;

        public HighSpeedTelemetry()
        {
            var counters = (Counters[])Enum.GetValues(typeof(Counters));
            vals = new long[counters.Length];
        }

        public void IncrementCounter(Counters c)
        {
            Interlocked.Increment(ref vals[(int)c]);
        }

        public long GetCounter(Counters c)
        {
            return vals[(int)c];
        }
    }
}
