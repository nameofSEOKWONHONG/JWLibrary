using System.IO;

namespace JWLibrary.Core.FileWatcher
{
    ////////////////////////////////////////////////////////////////////////////////////// 
    ////////////////////////////////////////////////////////////////////////////////////// 
    /// <summary>
    /// Contains settings that initializes the filesystem watchers created within the 
    /// Watcher class.
    /// </summary>
    public class WatcherInfo
	{
		public string                  WatchPath         { get; set; }
		public bool                    IncludeSubFolders { get; set; }
		public bool                    WatchForError     { get; set; }
		public bool                    WatchForDisposed  { get; set; }
		public System.IO.NotifyFilters ChangesFilters    { get; set; }
		public WatcherChangeTypes      WatchesFilters    { get; set; }
		public string                  FileFilter        { get; set; }
		public uint                    BufferKBytes      { get; set; }
		// only applicable if using WatcherEx class
		public int                     MonitorPathInterval     { get; set; }

		//--------------------------------------------------------------------------------
		public WatcherInfo()
		{
			this.WatchPath           = "";
			this.IncludeSubFolders   = false;
			this.WatchForError       = false;
			this.WatchForDisposed    = false;
			this.ChangesFilters      = NotifyFilters.Attributes;
			this.WatchesFilters      = WatcherChangeTypes.All;
			this.FileFilter          = "";
			this.BufferKBytes        = 8;
			this.MonitorPathInterval = 0;
		}
	}

}
