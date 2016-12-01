using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;

namespace JWLibrary.FileWatcher
{
    #region WatcherEx Helper Objects
    ////////////////////////////////////////////////////////////////////////////////////// 
    ////////////////////////////////////////////////////////////////////////////////////// 
    /// <summary>
    /// This class allows us to pass any type of watcher arguments to the calling object's 
    /// handler via a single object instead of having to add a lot of event handlers for 
    /// the various event args types.
    /// </summary>
    public class WatcherExEventArgs
	{
		#region Properties
		public FileSystemWatcherEx Watcher    { get; set; }
		public object              Arguments  { get; set; }
		public ArgumentType        ArgType    { get; set; }
		public NotifyFilters       Filter     { get; set; }
		#endregion Properties

		#region Constructors
		public WatcherExEventArgs(FileSystemWatcherEx watcher, 
								  object              arguments,
								  ArgumentType        argType,
								  NotifyFilters       filter)
		{
			Watcher   = watcher;
			Arguments = arguments;
			ArgType   = argType;
			Filter    = filter;
		}
		public WatcherExEventArgs(FileSystemWatcherEx watcher, 
						  		  object              arguments,
								  ArgumentType        argType)
		{
			Watcher   = watcher;
			Arguments = arguments;
			ArgType   = argType;
			Filter    = NotifyFilters.Attributes;
		}
		#endregion Constructors
	}

	////////////////////////////////////////////////////////////////////////////////////// 
	////////////////////////////////////////////////////////////////////////////////////// 
	/// <summary>
	/// Event handlers for the watcher events we post back to the containing object.  We 
	/// only need one handler type because no matter what event is posted, the 
	/// WatcherEventArgs object contains the correct argument type (as an object).  This 
	/// is the event handler that the calling object will use.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void WatcherExEventHandler(object sender, WatcherExEventArgs e);

	////////////////////////////////////////////////////////////////////////////////////// 
	////////////////////////////////////////////////////////////////////////////////////// 
	/// <summary>
	/// The list of watchers created in the watcher class
	/// </summary>
	public class WatchersExList : List<FileSystemWatcherEx> { }
	#endregion WatcherEx Helper Objects

	#region WatcherEx Class
	////////////////////////////////////////////////////////////////////////////////////// 
	////////////////////////////////////////////////////////////////////////////////////// 
	/// <summary>
	/// This is the main class (and the one you'll use directly). Create an instance of 
	/// the class (passing in a WatcherInfo object for intialization), and then attach 
	/// event handlers to this object.  One or more watchers will be created to handle 
	/// the various events and filters, and will marshal these evnts into a single set 
	/// from which you can gather info.
	/// </summary>
	public class WatcherEx : IDisposable
	{
		#region Data Members
		private bool           disposed    = false;
		private WatcherInfo    watcherInfo = null;
		private WatchersExList watchers    = new WatchersExList();
		#endregion Data Members

		#region Event Definitions
		public event WatcherExEventHandler EventChangedAttribute     = delegate {};
		public event WatcherExEventHandler EventChangedCreationTime  = delegate {};
		public event WatcherExEventHandler EventChangedDirectoryName = delegate {};
		public event WatcherExEventHandler EventChangedFileName      = delegate {};
		public event WatcherExEventHandler EventChangedLastAccess    = delegate {};
		public event WatcherExEventHandler EventChangedLastWrite     = delegate {};
		public event WatcherExEventHandler EventChangedSecurity      = delegate {};
		public event WatcherExEventHandler EventChangedSize          = delegate {};
		public event WatcherExEventHandler EventCreated              = delegate {};
		public event WatcherExEventHandler EventDeleted              = delegate {};
		public event WatcherExEventHandler EventRenamed              = delegate {};
		public event WatcherExEventHandler EventError                = delegate {};
		public event WatcherExEventHandler EventDisposed             = delegate {};
		public event WatcherExEventHandler EventPathAvailability     = delegate {};
		#endregion Event Definitions

		#region Constructors
		//--------------------------------------------------------------------------------
		public WatcherEx(WatcherInfo info)
		{
			if (info == null)
			{
				throw new Exception("WatcherInfo object cannot be null");
			}
			this.watcherInfo = info;

			Initialize();
		}
		#endregion Constructors

		#region Dispose Methods
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Disposes all of the FileSystemWatcher objects, and disposes this object.
		/// </summary>
		public void Dispose()
		{
			Debug.WriteLine("WatcherEx.Dispose()");
		    if (!this.disposed)
		    {
				DisposeWatchers();
	            this.disposed = true;
				GC.Collect();
				GC.WaitForPendingFinalizers();
		    }
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Disposes of all of our watchers (called from Dispose, or as a result of 
		/// loosing access to a folder)
		/// </summary>
		public void DisposeWatchers()
		{
			Debug.WriteLine("WatcherEx.DisposeWatchers()");
            for (int i = 0; i < this.watchers.Count; i++)
            {
                this.watchers[i].Dispose();
            }
            this.watchers.Clear();
		}
		#endregion Dispose Methods

		#region Helper Methods
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Determines if the specified NotifyFilter item has been specified to be 
		/// handled by this object.
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		public bool HandleNotifyFilter(NotifyFilters filter)
		{
			return (((NotifyFilters)(this.watcherInfo.ChangesFilters & filter)) == filter);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Determines if the specified WatcherChangeType item has been specified to be 
		/// handled by this object.
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		public bool HandleWatchesFilter(WatcherChangeTypes filter)
		{
			return (((WatcherChangeTypes)(this.watcherInfo.WatchesFilters & filter)) == filter);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Initializes this oibject by creating all of the required internal 
		/// FileSystemWatcher objects necessary to mointor the folder/file for the 
		/// desired changes
		/// </summary>
		private void Initialize()
		{
			Debug.WriteLine("WatcherEx.Initialize()");
			// the buffer can be from 4 to 64 kbytes.  Default is 8
			this.watcherInfo.BufferKBytes = Math.Max(4, Math.Min(this.watcherInfo.BufferKBytes, 64));

			// create the main watcher (handles create/delete, rename, error, and dispose)
			// can't pass a null enum type, so we just pass ta dummy one on the first call
			CreateWatcher(false, this.watcherInfo.ChangesFilters);
			// create a change watcher for each NotifyFilter item
			CreateWatcher(true, NotifyFilters.Attributes);
			CreateWatcher(true, NotifyFilters.CreationTime);
			CreateWatcher(true, NotifyFilters.DirectoryName);
			CreateWatcher(true, NotifyFilters.FileName);
			CreateWatcher(true, NotifyFilters.LastAccess);
			CreateWatcher(true, NotifyFilters.LastWrite);
			CreateWatcher(true, NotifyFilters.Security);
			CreateWatcher(true, NotifyFilters.Size);

			Debug.WriteLine(string.Format("WatcherEx.Initialize() - {0} watchers created", this.watchers.Count));
		}


		//--------------------------------------------------------------------------------
		/// <summary>
		/// Actually creates the necessary FileSystemWatcher objects, depending oin which 
		/// notify filters and change types the user specified.
		/// </summary>
		/// <param name="changeType"></param>
		/// <param name="filter"></param>
		private void CreateWatcher(bool changedWatcher, NotifyFilters filter)
		{
			Debug.WriteLine(string.Format("WatcherEx.CreateWatcher({0}, {1})", changedWatcher.ToString(), filter.ToString()));

			FileSystemWatcherEx watcher = null;
			int bufferSize = (int)this.watcherInfo.BufferKBytes * 1024;
			// Each "Change" filter gets its own watcher so we can determine *what* 
			// actually changed. This will allow us to react only to the change events 
			// that we actually want.  The reason I do this is because some programs 
			// fire TWO events for  certain changes. For example, Notepad sends two 
			// events when a file is created. One for CreationTime, and one for 
			// Attributes.
			if (changedWatcher)
			{
				// if we're not handling the currently specified filter, get out
				if (HandleNotifyFilter(filter))
				{
					watcher                       = new FileSystemWatcherEx(this.watcherInfo.WatchPath);
					watcher.IncludeSubdirectories = this.watcherInfo.IncludeSubFolders;
					watcher.Filter                = this.watcherInfo.FileFilter;
					watcher.NotifyFilter          = filter;
					watcher.InternalBufferSize    = bufferSize;
					switch (filter)
					{
						case NotifyFilters.Attributes    :
							watcher.Changed += new FileSystemEventHandler(watcher_ChangedAttribute);
							break;
						case NotifyFilters.CreationTime  : 
							watcher.Changed += new FileSystemEventHandler(watcher_ChangedCreationTime);
							break;
						case NotifyFilters.DirectoryName : 
							watcher.Changed += new FileSystemEventHandler(watcher_ChangedDirectoryName);
							break;
						case NotifyFilters.FileName      : 
							watcher.Changed += new FileSystemEventHandler(watcher_ChangedFileName);
							break;
						case NotifyFilters.LastAccess    : 
							watcher.Changed += new FileSystemEventHandler(watcher_ChangedLastAccess);
							break;
						case NotifyFilters.LastWrite     : 
							watcher.Changed += new FileSystemEventHandler(watcher_ChangedLastWrite);
							break;
						case NotifyFilters.Security      : 
							watcher.Changed += new FileSystemEventHandler(watcher_ChangedSecurity);
							break;
						case NotifyFilters.Size          : 
							watcher.Changed += new FileSystemEventHandler(watcher_ChangedSize);
							break;
					}
				}
			}
			// All other FileSystemWatcher events are handled through a single "main" 
			// watcher.
			else
			{
				if (HandleWatchesFilter(WatcherChangeTypes.Created) ||
					HandleWatchesFilter(WatcherChangeTypes.Deleted) ||
					HandleWatchesFilter(WatcherChangeTypes.Renamed) ||
					this.watcherInfo.WatchForError ||
					this.watcherInfo.WatchForDisposed)
				{
					watcher                       = new FileSystemWatcherEx(this.watcherInfo.WatchPath, watcherInfo.MonitorPathInterval);
					watcher.IncludeSubdirectories = this.watcherInfo.IncludeSubFolders;
					watcher.Filter                = this.watcherInfo.FileFilter;
					watcher.InternalBufferSize    = bufferSize;
				}

				if (HandleWatchesFilter(WatcherChangeTypes.Created)) 
				{
					watcher.Created += new FileSystemEventHandler(watcher_CreatedDeleted);
				}
				if (HandleWatchesFilter(WatcherChangeTypes.Deleted))
				{
					watcher.Deleted += new FileSystemEventHandler(watcher_CreatedDeleted);
				}
				if (HandleWatchesFilter(WatcherChangeTypes.Renamed))
				{
					watcher.Renamed += new RenamedEventHandler(watcher_Renamed);
				}
				if (watcherInfo.MonitorPathInterval > 0)
				{
					watcher.EventPathAvailability += new PathAvailabilityHandler(watcher_EventPathAvailability);
				}
			}
			if (watcher != null)
			{
				if (this.watcherInfo.WatchForError)
				{
					watcher.Error += new ErrorEventHandler(watcher_Error);
				}
				if (this.watcherInfo.WatchForDisposed)
				{
					watcher.Disposed += new EventHandler(watcher_Disposed);
				}
				this.watchers.Add(watcher);
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Starts all of the internal FileSystemWatcher objects by setting their 
		/// EnableRaisingEvents property to true.
		/// </summary>
		public void Start()
		{
			Debug.WriteLine("WatcherEx.Start()");
			this.watchers[0].StartFolderMonitor();
			for (int i = 0; i < this.watchers.Count; i++)
			{
				this.watchers[i].EnableRaisingEvents = true;
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Stops all of the internal FileSystemWatcher objects by setting their 
		/// EnableRaisingEvents property to true.
		/// </summary>
		public void Stop()
		{
			Debug.WriteLine("WatcherEx.Stop()");
			this.watchers[0].StopFolderMonitor();
			for (int i = 0; i < this.watchers.Count; i++)
			{
				this.watchers[i].EnableRaisingEvents = false;
			}
		}
		#endregion Helper Methods

		#region Native Watcher Events
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the watcher responsible for monitoring attribute changes is 
		/// triggered.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void watcher_ChangedAttribute(object sender, FileSystemEventArgs e)
		{
			Debug.WriteLine("EVENT - Changed Attribute");
			EventChangedAttribute(this, new WatcherExEventArgs(sender as FileSystemWatcherEx, e, ArgumentType.FileSystem, NotifyFilters.Attributes));
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the watcher responsible for monitoring creation time changes is 
		/// triggered.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void watcher_ChangedCreationTime(object sender, FileSystemEventArgs e)
		{
			Debug.WriteLine("EVENT - Changed CreationTime");
			EventChangedCreationTime(this, new WatcherExEventArgs(sender as FileSystemWatcherEx, e, ArgumentType.FileSystem, NotifyFilters.CreationTime));
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the watcher responsible for monitoring directory name changes is 
		/// triggered.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void watcher_ChangedDirectoryName(object sender, FileSystemEventArgs e)
		{
			Debug.WriteLine("EVENT - Changed DirectoryName");
			EventChangedDirectoryName(this, new WatcherExEventArgs(sender as FileSystemWatcherEx, e, ArgumentType.FileSystem, NotifyFilters.DirectoryName));
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the watcher responsible for monitoring file name changes is 
		/// triggered.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void watcher_ChangedFileName(object sender, FileSystemEventArgs e)
		{
			Debug.WriteLine("EVENT - Changed FileName");
			EventChangedFileName(this, new WatcherExEventArgs(sender as FileSystemWatcherEx, e, ArgumentType.FileSystem, NotifyFilters.FileName));
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the watcher responsible for monitoring last access date/time 
		/// changes is triggered.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void watcher_ChangedLastAccess(object sender, FileSystemEventArgs e)
		{
			Debug.WriteLine("EVENT - Changed LastAccess");
			EventChangedLastAccess(this, new WatcherExEventArgs(sender as FileSystemWatcherEx, e, ArgumentType.FileSystem, NotifyFilters.LastAccess));
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the watcher responsible for monitoring last write date/time 
		/// changes is triggered.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void watcher_ChangedLastWrite(object sender, FileSystemEventArgs e)
		{
			Debug.WriteLine("EVENT - Changed LastWrite");
			EventChangedLastWrite(this, new WatcherExEventArgs(sender as FileSystemWatcherEx, e, ArgumentType.FileSystem, NotifyFilters.LastWrite));
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the watcher responsible for monitoring security changes is 
		/// triggered.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void watcher_ChangedSecurity(object sender, FileSystemEventArgs e)
		{
			Debug.WriteLine("EVENT - Changed Security");
			EventChangedSecurity(this, new WatcherExEventArgs(sender as FileSystemWatcherEx, e, ArgumentType.FileSystem, NotifyFilters.Security));
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the watcher responsible for monitoring size changes is 
		/// triggered.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void watcher_ChangedSize(object sender, FileSystemEventArgs e)
		{
			Debug.WriteLine("EVENT - Changed Size");
			EventChangedSize(this, new WatcherExEventArgs(sender as FileSystemWatcherEx, e, ArgumentType.FileSystem, NotifyFilters.Size));
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when an internal watcher is disposed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void watcher_Disposed(object sender, EventArgs e)
		{
			Debug.WriteLine("EVENT - Disposed");
			EventDisposed(this, new WatcherExEventArgs(sender as FileSystemWatcherEx, e, ArgumentType.StandardEvent));
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the main watcher detects an error (the watcher that detected the 
		/// error is part of the event's arguments object)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void watcher_Error(object sender, ErrorEventArgs e)
		{
			Debug.WriteLine("EVENT - Error");
			EventError(this, new WatcherExEventArgs(sender as FileSystemWatcherEx, e, ArgumentType.Error));
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the main watcher detects a file rename.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void watcher_Renamed(object sender, RenamedEventArgs e)
		{
			Debug.WriteLine("EVENT - Renamed");
			EventRenamed(this, new WatcherExEventArgs(sender as FileSystemWatcherEx, e, ArgumentType.Renamed));
		}

		//--------------------------------------------------------------------------------
		private void watcher_CreatedDeleted(object sender, FileSystemEventArgs e)
		{
 			switch (e.ChangeType)	
			{
				case WatcherChangeTypes.Created :
					Debug.WriteLine("EVENT - Created");
					EventCreated(this, new WatcherExEventArgs(sender as FileSystemWatcherEx, e, ArgumentType.FileSystem));
					break;
				case WatcherChangeTypes.Deleted :
					Debug.WriteLine("EVENT - Changed Deleted");
					EventDeleted(this, new WatcherExEventArgs(sender as FileSystemWatcherEx, e, ArgumentType.FileSystem));
					break;
			}
		}

		//--------------------------------------------------------------------------------
		void watcher_EventPathAvailability(object sender, PathAvailablitiyEventArgs e)
		{
			Debug.WriteLine("EVENT - PathAvailability");
			EventPathAvailability(this, new WatcherExEventArgs(sender as FileSystemWatcherEx, e, ArgumentType.PathAvailability));
			if (e.PathIsAvailable)
			{
				DisposeWatchers();
				Initialize();
			}
		}

		#endregion Native Watcher Events

	}
	#endregion WatcherEx Class
}
