using System;
using System.Timers;

namespace Collectors.CollectorBase.Model
{
	public class Scheduler : IDisposable
	{
		System.Timers.Timer? _timer;

		public Scheduler()
		{
		}

        public void CheckStatus(object stateInfo)
		{ }

        private int _days;
        public int Days
        {
            get => _days;
            set => _days = value;
        }

        private int _hours;
		public int Hours
		{
			get => _hours;
			set => _hours = value;
		}

		private int _minutes;
		public int Minutes
		{
			get => _minutes;
			set => _minutes = value;
		}

		private int _seconds;
		public int Seconds
		{
			get => _seconds;
			set => _seconds = value;
		}

		private long _totalMs;
		private void CalcMilliseconds()
		{
			_totalMs = ((_days * 24) * 3600000) + (_hours * 3600000) + (_minutes * 60000) + (_seconds * 1000);
		}

		private EventHandler _handler;
		public void RegisterCallback(EventHandler handler)
		{
			if(handler == null)
			{
				throw new ArgumentNullException(nameof(_handler));
			}

			_handler = handler;
		}

		public void StartTimer()
		{
			CalcMilliseconds();

			_timer = new();
			_timer.Interval = _totalMs;
			_timer.Elapsed += new ElapsedEventHandler(_handler);
			_timer.Enabled = true;
			_timer.Start();
		}

		public void StopTimer()
		{
			_timer.Enabled = false;
			_timer.Stop();
		}

		public void ResetTimer()
		{
			_timer.Dispose();
			StartTimer();
		}

        public void Dispose()
        {
			_timer.Dispose();
        }
    }
}

