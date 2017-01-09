using System;
using System.Timers;

namespace JWLibrary.FFmpeg
{
    class FrameDropChecker : IDisposable
    {
        #region delegate events
        public event EventHandler<EventArgs> FrameDroped;
        protected virtual void OnFrameDroped(object sender, EventArgs e)
        {
            if (FrameDroped != null)
            {
                FrameDroped(this, e);
            }
        }
        #endregion

        #region variable
        public int FrameDropCount { get; set; }
        public bool IsLimit { get; set; }
        private const int FRAME_LIMIT_COUNT = 50;
        private Timer _timer;
        private int _timerElapsedCount;
        #endregion

        #region constructor
        public FrameDropChecker()
        {
            _timer = new Timer();
            _timer.Interval = 500;
            _timer.Elapsed += _timer_Elapsed;
        }

        public FrameDropChecker(double chkTime)
        {
            _timer = new Timer();
            _timer.Interval = chkTime;
            _timer.Elapsed += _timer_Elapsed;
        }
        #endregion

        #region funtions
        public void FrameDropCheckStart()
        {
            _timerElapsedCount = 0;
            FrameDropCount = 0;
            IsLimit = false;
            _timer.Start();
        }

        public void FrameDropCheckStop()
        {
            _timer.Enabled = false;
            _timer.Stop();
        }
        #endregion

        #region event
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (FRAME_LIMIT_COUNT < FrameDropCount)
            {
                IsLimit = true;
                OnFrameDroped(this, new EventArgs());
            }
            else
            {
                if (_timerElapsedCount == FRAME_LIMIT_COUNT)
                    _timerElapsedCount = 0;
            }

            _timerElapsedCount++;
        }
        #endregion

        #region dispose
        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }
        #endregion
    }
}
