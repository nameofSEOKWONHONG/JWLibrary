using System;
using System.IO;
using System.Threading;

namespace JWLibrary.Core.FileWatcher
{
    //////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////
    public class FileSystemWatcherEx : FileSystemWatcher
	{
		// set a reasonable maximum interval time
		public readonly int MaxInterval = 60000;

		public  event  PathAvailabilityHandler EventPathAvailability = delegate{};

		private bool   IsNetworkAvailable = true;
		private int    Interval           = 100;
		private Thread thread             = null;
		public  string Name               = "FileSystemWatcherEx";
		private bool   Run                = false;

		#region Constructors
		//--------------------------------------------------------------------------------
		public FileSystemWatcherEx():base()
		{
			CreateThread();
		}

		//--------------------------------------------------------------------------------
		public FileSystemWatcherEx(string path):base(path)
		{
			CreateThread();
		}

		//--------------------------------------------------------------------------------
		public FileSystemWatcherEx(int interval):base()
		{
		    this.Interval = interval;
		    CreateThread();
		}

		//--------------------------------------------------------------------------------
		public FileSystemWatcherEx(string path, int interval):base(path)
		{
		    this.Interval = interval;
		    CreateThread();
		}

		//--------------------------------------------------------------------------------
		public FileSystemWatcherEx(int interval, string name):base()
		{
		    this.Interval = interval;
			this.Name = name;
		    CreateThread();
		}

		//--------------------------------------------------------------------------------
		public FileSystemWatcherEx(string path, int interval, string name):base(path)
		{
		    this.Interval = interval;
			this.Name = name;
		    CreateThread();
		}
		#endregion Constructors

		#region Helper Methods
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Creates the thread if the interval is greater than 0 milliseconds 
		/// </summary>
		private void CreateThread()
		{
			// Normalize  the interval
			this.Interval = Math.Max(0, Math.Min(this.Interval, this.MaxInterval));
			// If the interval is 0, this indicates we don't want to monitor the path 
			// for availability.
			if (this.Interval > 0)
			{
				this.thread              = new Thread(new ThreadStart(MonitorFolderAvailability));
				this.thread.Name         = this.Name;
				this.thread.IsBackground = true;
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Attempts to start the monitoring thread
		/// </summary>
		public void StartFolderMonitor()
		{
			this.Run = true;
			if (this.thread != null)
			{
				this.thread.Start();
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Attempts to start the monitoring thread
		/// </summary>
		public void StopFolderMonitor()
		{
			this.Run = false;
		}
		#endregion Helper Methods

		//--------------------------------------------------------------------------------
		/// <summary>
		/// The thread method. It sits and spins making sure the folder exists
		/// </summary>
		public void MonitorFolderAvailability()
		{
			while (this.Run)
			{
				if (this.IsNetworkAvailable)
				{
					if (!Directory.Exists(base.Path))
					{
						this.IsNetworkAvailable = false;
						RaiseEventNetworkPathAvailablity();
					}
				}
				else
				{
					if (Directory.Exists(base.Path))
					{
						this.IsNetworkAvailable = true;
						RaiseEventNetworkPathAvailablity();
					}
				}
				Thread.Sleep(this.Interval);
			}
		}

		//--------------------------------------------------------------------------------
		private void RaiseEventNetworkPathAvailablity()
		{
			EventPathAvailability(this, new PathAvailablitiyEventArgs(this.IsNetworkAvailable));
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////
	public class PathAvailablitiyEventArgs : EventArgs
	{
		public bool PathIsAvailable { get; set; }

		public PathAvailablitiyEventArgs(bool available)
		{
			this.PathIsAvailable = available;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////
	public delegate void PathAvailabilityHandler(object sender, PathAvailablitiyEventArgs e);

}


