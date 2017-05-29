using System;
using System.Diagnostics;
using System.Threading;
using System.Timers;

namespace JWLibrary.OSI
{

	public class CpuInfo : IDisposable {

		public event EventHandler<CpuUseRateChangedEventArgs> CpuUseRateChanged;

		public event EventHandler<TotalUseRateChangedEventArgs> TotalCpuUseRateChanged;

		private string _processName;
		private System.Timers.Timer _timer;
		private System.Timers.Timer _totalTimer;

		private bool isDisposing;

		public bool IsProcessBusy { get; private set; }
		public bool IsTotalProcessBusy { get; private set; }

		public CpuInfo (string processName = null) {
			_processName = processName;

			if (_timer == null) _timer = new System.Timers.Timer();
			_timer.Interval = 1 * 500;
			_timer.Elapsed += _timer_Elapsed;
			_timer.Enabled = false;

			if (_totalTimer == null) _totalTimer = new System.Timers.Timer();
			_totalTimer.Interval = 1 * 500;
			_totalTimer.Elapsed += _totalTimer_Elapsed;
			_totalTimer.Enabled = false;
		}

		//total timer
		private void _totalTimer_Elapsed (object sender, ElapsedEventArgs e) {
			if (!IsTotalProcessBusy) {
				IsTotalProcessBusy = true;

				if (_totalTimer.Enabled) {
					using (PerformanceCounter pcProcess = new PerformanceCounter("Processor", "% Processor Time", "_Total")) {
						pcProcess.NextValue();
						Thread.Sleep(1000);
						OnTotalCpuUseRateChanged(new TotalUseRateChangedEventArgs(pcProcess.NextValue()));
					}
				}

				IsTotalProcessBusy = false;
			}
		}

		//process timer
		private void _timer_Elapsed (object sender, ElapsedEventArgs e) {
			if (!IsProcessBusy) {
				IsProcessBusy = true;

				if (_timer.Enabled) {
					//get all
					if (string.IsNullOrEmpty(_processName)) {
						foreach (Process proc in Process.GetProcesses()) {
							using (PerformanceCounter pcProcess = new PerformanceCounter("Process", "% Processor Time", proc.ProcessName)) {
								pcProcess.NextValue();
								Thread.Sleep(1000);
								OnCpuUseRateChanged(new CpuUseRateChangedEventArgs(proc.ProcessName, pcProcess.NextValue() / Environment.ProcessorCount));
							}
						}
					}
					else {
						//get one
						Process[] procs = Process.GetProcessesByName(_processName);

						foreach (Process proc in procs) {
							if (proc.ProcessName == _processName) {
								using (PerformanceCounter pcProcess = new PerformanceCounter("Process", "% Processor Time", proc.ProcessName)) {
									pcProcess.NextValue();
									Thread.Sleep(1000);
									OnCpuUseRateChanged(new CpuUseRateChangedEventArgs(proc.ProcessName, pcProcess.NextValue() / Environment.ProcessorCount));
								}

								break;
							}
						}
					}
				}
				IsProcessBusy = false;
			}
		}

		protected virtual void OnCpuUseRateChanged(CpuUseRateChangedEventArgs e) {
			if (CpuUseRateChanged != null) {
				CpuUseRateChanged(this, e);
			}
		}

		protected virtual void OnTotalCpuUseRateChanged(TotalUseRateChangedEventArgs e) {
			if (TotalCpuUseRateChanged != null) {
				TotalCpuUseRateChanged(this, e);
			}
		}

		public void StartCpuUseRateCheck (string processName = null) {
			_processName = processName;

			if (_timer != null) {
				_timer.Enabled = true;
				_timer.Start();
			}
		}

		public void StartTotalCpuUseRateCheck () {
			if (_totalTimer != null) {
				_totalTimer.Enabled = true;
				_totalTimer.Start();
			}
		}

		public void Stop () {
			if (_timer != null) {
				_timer.Enabled = false;
				_timer.Stop();

				Thread.Sleep(1000);

				OnCpuUseRateChanged(new CpuUseRateChangedEventArgs(null, 0));
			}

			if (_totalTimer != null) {
				_totalTimer.Enabled = false;
				_totalTimer.Stop();

				Thread.Sleep(1000);

				OnTotalCpuUseRateChanged(new TotalUseRateChangedEventArgs(0));
			}
		}

		public void Dispose () {
			if (isDisposing)
				return;
			isDisposing = true;
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose (bool disposing) {
			//memory free is here
			if (_timer != null) {
				_timer.Elapsed -= _timer_Elapsed;
				_timer.Stop();
				_timer.Dispose();
				_timer = null;
			}

			if (_totalTimer != null) {
				_totalTimer.Elapsed -= _totalTimer_Elapsed;
				_totalTimer.Stop();
				_totalTimer.Dispose();
				_totalTimer = null;
			}
		}        
	}

	public class CpuUseRateChangedEventArgs : EventArgs {
		public readonly float CpuUseRate;
		public readonly string ProcessName;

		public CpuUseRateChangedEventArgs(string processName, float cpuUseRate) {
			this.ProcessName = processName;
			this.CpuUseRate = cpuUseRate;
		}
	}

	public class TotalUseRateChangedEventArgs : EventArgs {
		public readonly float TotalCpuUseRate;

		public TotalUseRateChangedEventArgs(float totalCpuUseRate) {
			this.TotalCpuUseRate = totalCpuUseRate;
		}
	}
}