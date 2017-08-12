using System;
using System.Diagnostics;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Esb.Module.Throttle
{
    public class PerformanceCounterValue
    {
        private readonly PerformanceCounter _counter;
        private readonly int _readIntervalMilliseconds;
        private float _lastReadValue;
        private DateTime _nextReadDate;

        public PerformanceCounterValue(PerformanceCounter counter, int readIntervalMilliseconds)
        {
            Guard.AgainstNull(counter, "counter");

            _counter = counter;
            _readIntervalMilliseconds = readIntervalMilliseconds < 1000 ? 1000 : readIntervalMilliseconds;
            _nextReadDate = DateTime.Now;
        }

        public float NextValue()
        {
            if (DateTime.Now >= _nextReadDate)
            {
                _lastReadValue = _counter.NextValue();
                _nextReadDate = DateTime.Now.AddMilliseconds(_readIntervalMilliseconds);
            }

            return _lastReadValue;
        }
    }
}