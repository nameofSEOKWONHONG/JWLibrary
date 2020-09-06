using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JWLibrary.Win32 {

    public class WIN32Api {

        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern IntPtr NtQueryInformationFile(IntPtr fileHandle, ref IO_STATUS_BLOCK IoStatusBlock, IntPtr pInfoBlock, uint length, FILE_INFORMATION_CLASS fileInformation);

        public struct IO_STATUS_BLOCK {
            private uint status;
            private ulong information;
        }

        public struct _FILE_INTERNAL_INFORMATION {
            public ulong IndexNumber;
        }

        // Abbreviated, there are more values than shown
        public enum FILE_INFORMATION_CLASS {
            FileDirectoryInformation = 1,     // 1
            FileFullDirectoryInformation,     // 2
            FileBothDirectoryInformation,     // 3
            FileBasicInformation,         // 4
            FileStandardInformation,      // 5
            FileInternalInformation      // 6
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetFileInformationByHandle(IntPtr hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);

        public struct BY_HANDLE_FILE_INFORMATION {
            public uint FileAttributes;
            public FILETIME CreationTime;
            public FILETIME LastAccessTime;
            public FILETIME LastWriteTime;
            public uint VolumeSerialNumber;
            public uint FileSizeHigh;
            public uint FileSizeLow;
            public uint NumberOfLinks;
            public uint FileIndexHigh;
            public uint FileIndexLow;
        }

        #region Structures

        /// <summary>
        /// The BLENDFUNCTION structure controls blending by specifying the blending functions for source and destination bitmaps.
        /// </summary>
        public struct BLENDFUNCTION {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        /// <summary>
        /// The MINMAXINFO structure contains information about a window's maximized size and position and its minimum and maximum tracking size.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT {
            public int x;
            public int y;

            public POINT(Int32 x, Int32 y) {
                this.x = x; this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE {
            public Int32 cx;
            public Int32 cy;

            public SIZE(Int32 cx, Int32 cy) {
                this.cx = cx; this.cy = cy;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct APPBARDATA {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public IntPtr lParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NCCALCSIZE_PARAMS {
            public RECT rect0;
            public RECT rect1;
            public RECT rect2;
            public WINDOWPOS IntPtr;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int flags;
        }

        #region don't use ui structure

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT {
            private int _Left;
            private int _Top;
            private int _Right;
            private int _Bottom;

            public RECT(Rectangle Rectangle) : this(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom) {
            }

            public RECT(int Left, int Top, int Right, int Bottom) {
                _Left = Left;
                _Top = Top;
                _Right = Right;
                _Bottom = Bottom;
            }

            public int X {
                get { return _Left; }
                set { _Left = value; }
            }

            public int Y {
                get { return _Top; }
                set { _Top = value; }
            }

            public int Left {
                get { return _Left; }
                set { _Left = value; }
            }

            public int Top {
                get { return _Top; }
                set { _Top = value; }
            }

            public int Right {
                get { return _Right; }
                set { _Right = value; }
            }

            public int Bottom {
                get { return _Bottom; }
                set { _Bottom = value; }
            }

            public int Height {
                get { return _Bottom - _Top; }
                set { _Bottom = value - _Top; }
            }

            public int Width {
                get { return _Right - _Left; }
                set { _Right = value + _Left; }
            }

            public Point Location {
                get { return new Point(Left, Top); }
                set {
                    _Left = value.X;
                    _Top = value.Y;
                }
            }

            public Size Size {
                get { return new Size(Width, Height); }
                set {
                    _Right = value.Height + _Left;
                    _Bottom = value.Height + _Top;
                }
            }

            public Rectangle ToRectangle() {
                return new Rectangle(this.Left, this.Top, this.Width, this.Height);
            }

            public static Rectangle ToRectangle(RECT Rectangle) {
                return Rectangle.ToRectangle();
            }

            public static RECT FromRectangle(Rectangle Rectangle) {
                return new RECT(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);
            }

            public static implicit operator Rectangle(RECT Rectangle) {
                return Rectangle.ToRectangle();
            }

            public static implicit operator RECT(Rectangle Rectangle) {
                return new RECT(Rectangle);
            }

            public static bool operator ==(RECT Rectangle1, RECT Rectangle2) {
                return Rectangle1.Equals(Rectangle2);
            }

            public static bool operator !=(RECT Rectangle1, RECT Rectangle2) {
                return !Rectangle1.Equals(Rectangle2);
            }

            public override string ToString() {
                return "{Left: " + _Left + "; " + "Top: " + _Top + "; Right: " + _Right + "; Bottom: " + _Bottom + "}";
            }

            public bool Equals(RECT Rectangle) {
                return Rectangle.Left == _Left && Rectangle.Top == _Top && Rectangle.Right == _Right && Rectangle.Bottom == _Bottom;
            }

            public override bool Equals(object Object) {
                if (Object is RECT) {
                    return Equals((RECT)Object);
                } else if (Object is Rectangle) {
                    return Equals(new RECT((Rectangle)Object));
                }
                return false;
            }
        }

        #endregion don't use ui structure

        /// <summary>
        /// Contains operating system version information. The information includes major and minor version numbers, a build number,
        /// a platform identifier, and information about product suites and the latest Service Pack installed on the system.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct OSVERSIONINFOEX {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;

            public short wServicePackMajor;
            public short wServicePackMinor;
            public short wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }

        #region Raw Input

        /// <summary>
        /// The RAWINPUTDEVICE structure defines information for the raw input devices.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RAWINPUTDEVICE {
            public ushort usUsagePage;
            public ushort usUsage;
            public uint dwFlags;
            public IntPtr hwndTarget;
        }

        /// <summary>
        /// The RAWINPUTHEADER structure contains the header information that is part of the raw input data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RAWINPUTHEADER {
            public uint dwType;
            public uint dwSize;
            public IntPtr hDevice;
            public int wParam;
        }

        /// <summary>
        /// The RAWKEYBOARD structure contains information about the state of the keyboard.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RAWKEYBOARD {

            [MarshalAs(UnmanagedType.U2)]
            public ushort MakeCode;

            [MarshalAs(UnmanagedType.U2)]
            public ushort Flags;

            [MarshalAs(UnmanagedType.U2)]
            public ushort Reserved;

            [MarshalAs(UnmanagedType.U2)]
            public ushort VKey;

            [MarshalAs(UnmanagedType.U4)]
            public uint Message;

            [MarshalAs(UnmanagedType.U4)]
            public uint ExtraInformation;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BUTTONSSTR {

            [MarshalAs(UnmanagedType.U2)]
            public ushort usButtonFlags;

            [MarshalAs(UnmanagedType.U2)]
            public ushort usButtonData;
        }

        /// <summary>
        /// The RAWMOUSE structure contains information about the state of the mouse.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct RAWMOUSE {

            [MarshalAs(UnmanagedType.U2)]
            [FieldOffset(0)]
            public ushort usFlags;

            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset(4)]
            public uint ulButtons;

            [FieldOffset(4)]
            public BUTTONSSTR buttonsStr;

            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset(8)]
            public uint ulRawButtons;

            [FieldOffset(12)]
            public int lLastX;

            [FieldOffset(16)]
            public int lLastY;

            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset(20)]
            public uint ulExtraInformation;
        }

        /// <summary>
        /// The RAWHID structure describes the format of the raw input from a Human Interface Device (HID).
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RAWHID {

            [MarshalAs(UnmanagedType.U4)]
            public int dwSizHid;

            [MarshalAs(UnmanagedType.U4)]
            public int dwCount;
        }

        /// <summary>
        /// The RAWINPUT structure contains the raw input from a device.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct RAWINPUT {

            [FieldOffset(0)]
            public RAWINPUTHEADER header;

            [FieldOffset(16)]
            public RAWMOUSE mouse;

            [FieldOffset(16)]
            public RAWKEYBOARD keyboard;

            [FieldOffset(16)]
            public RAWHID hid;
        }

        #endregion Raw Input

        #endregion Structures

        #region Imported Functions

        #region User32.dll

        [DllImport("user32.dll")]
        public static extern Boolean UpdateLayeredWindow(IntPtr hWnd,
            IntPtr hdcDst,
            ref POINT pptDst,
            ref SIZE psize,
            IntPtr hdcSrc,
            ref POINT pptSrc,
            int crKey,
            ref BLENDFUNCTION pBlend,
            int dwFlags);

        [DllImport("User32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, long flags);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern void ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int x, int y, int cx, int cy, int flags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate,
            IntPtr hrgnUpdate, uint flags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        //[DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        [DllImport("User32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevice, uint uiNumDevices, uint cbSize);

        [DllImport("User32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);

        #endregion User32.dll

        #region GDI32.dll

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern Boolean DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern Boolean DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern int SelectClipRgn(IntPtr hdc, IntPtr hrgn);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2,
            int cx, int cy);

        #endregion GDI32.dll

        #region Shell32.dll

        [DllImport("Shell32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern Int32 SHAppBarMessage(int dwMessage, IntPtr pData);

        #endregion Shell32.dll

        #region Kernel32.dll

        /// <summary>
        /// Retrieves information about the current operating system.
        /// </summary>
        /// <param name="osVersionInfo">An OSVERSIONIFOEX structure that receives the operating system information.</param>
        /// <returns>true, if the function succeeds, else, otherwise.</returns>
        [DllImport("kernel32.dll", EntryPoint = "GetVersionEx")]
        private static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);

        #endregion Kernel32.dll

        #endregion Imported Functions

        #region Constants

        #region AppBar bounding

        public const int ABE_LEFT = 0;
        public const int ABE_TOP = 1;
        public const int ABE_RIGHT = 2;
        public const int ABE_BOTTOM = 3;

        #endregion AppBar bounding

        #region Windows Shell messages and notifications

        /// <summary>
        /// Registers a new appbar and specifies the message identifier that the system should use to send it notification messages.
        /// </summary>
        public const int ABM_NEW = 0;

        /// <summary>
        /// Unregisters an appbar by removing it from the system's internal list.
        /// </summary>
        public const int ABM_REMOVE = 1;

        /// <summary>
        /// Requests a size and screen position for an appbar.
        /// </summary>
        public const int ABM_QUERYPOS = 2;

        /// <summary>
        /// Sets the size and screen position of an appbar.
        /// </summary>
        public const int ABM_SETPOS = 3;

        /// <summary>
        /// Retrieves the autohide and always-on-top states of the Windows taskbar.
        /// </summary>
        public const int ABM_GETSTATE = 4;

        /// <summary>
        /// Retrieves the bounding rectangle of the Windows taskbar.
        /// </summary>
        public const int ABM_GETTASKBARPOS = 5;

        /// <summary>
        /// Notifies the system that an appbar has been activated.
        /// </summary>
        public const int ABM_ACTIVATE = 6;

        /// <summary>
        /// Retrieves the handle to the autohide appbar associated with an edge of the screen.
        /// </summary>
        public const int ABM_GETAUTOHIDEBAR = 7;

        /// <summary>
        /// Registers or unregisters an autohide appbar for an edge of the screen.
        /// </summary>
        public const int ABM_SETAUTOHIDEBAR = 8;

        /// <summary>
        /// Notifies the system when an appbar's position has changed.
        /// </summary>
        public const int ABM_WINDOWPOSCHANGED = 9;

        /// <summary>
        /// Sets the autohide and always-on-top states of the Windows taskbar.
        /// </summary>
        public const int ABM_SETSTATE = 10;

        #endregion Windows Shell messages and notifications

        #region WM_SYSCOMMAND Params

        /// <summary>
        /// If no SysCommands should be executed
        /// </summary>
        public const int SC_NONE = 0x0;

        /// <summary>
        /// If the Form should be closed.
        /// </summary>
        public const int SC_CLOSE = 0xf060;

        /// <summary>
        /// If the Form should be maximized
        /// </summary>
        public const int SC_MAXIMIZE = 0xf030;

        /// <summary>
        /// If the Form should be minimized
        /// </summary>
        public const int SC_MINIMIZE = 0xf020;

        /// <summary>
        /// If the form should be restored from the maximize mode.
        /// </summary>
        public const int SC_RESTORE = 0xf120;

        #endregion WM_SYSCOMMAND Params

        #region Extended Device Context Flags

        /// <summary>
        /// Returns a device context corresponding to the window rectangle rather than the client rectangle.
        /// </summary>
        public const long DCX_WINDOW = 0x00000001L;

        /// <summary>
        /// Returns a device context from the cache, rather than the OWNDC or CLASSDC window. Essentially overrides CS_OWNDC and CS_CLASSDC.
        /// </summary>
        public const long DCX_CACHE = 0x00000002L;

        /// <summary>
        /// Excludes the visible regions of all sibling windows above the window identified by hWnd.
        /// </summary>
        public const long DCX_CLIPSIBLINGS = 0x00000010L;

        #endregion Extended Device Context Flags

        #region Window Messages

        /// <summary>
        /// The WM_SIZE message is sent to a window after its size has changed.
        /// </summary>
        public const int WM_SIZE = 0x0005;

        /// <summary>
        /// The WM_ACTIVATE message is sent to both the window being activated and the window being deactivated. If the windows use the same input queue, the message is sent synchronously, first to the window procedure of the top-level window being deactivated, then to the window procedure of the top-level window being activated. If the windows use different input queues, the message is sent asynchronously, so the window is activated immediately.
        /// </summary>
        public const int WM_ACTIVATE = 0x0006;

        /// <summary>
        /// The WM_SHOWWINDOW message is sent to a window when the window is about to be hidden or shown.
        /// </summary>
        public const int WM_SHOWWINDOW = 0x0018;

        /// <summary>
        /// The WM_ACTIVATEAPP message is sent when a window belonging to a different application than the active window is about to be activated. The message is sent to the application whose window is being activated and to the application whose window is being deactivated.
        /// </summary>
        public const int WM_ACTIVATEAPP = 0x001C;

        /// <summary>
        /// The WM_GETMINMAXINFO message is sent to a window when the size or position of the window is about to change.
        /// </summary>
        public const int WM_GETMINMAXINFO = 0x0024;

        /// <summary>
        /// The WM_WINDOWPOSCHANGING message is sent to a window whose size, position, or place in the Z order is about to change as a result of a call to the SetWindowPos function or another window-management function.
        /// </summary>
        public const int WM_WINDOWPOSCHANGING = 0x0046;

        /// <summary>
        /// The WM_WINDOWPOSCHANGED message is sent to a window whose size, position, or place in the Z order has changed as a result of a call to the SetWindowPos function or another window-management function.
        /// </summary>
        public const int WM_WINDOWPOSCHANGED = 0x0047;

        /// <summary>
        /// The WM_STYLECHANGED message is sent to a window after the SetWindowLong function has changed one or more of the window's styles
        /// </summary>
        public const int WM_STYLECHANGED = 0x007D;

        /// <summary>
        /// The WM_NCCALCSIZE message is sent when the size and position of a window's client area must be calculated. By processing this message, an application can control the content of the window's client area when the size or position of the window changes.
        /// </summary>
        public const int WM_NCCALCSIZE = 0x0083;

        /// <summary>
        /// The WM_NCHITTEST message is sent to a window when the cursor moves, or when a mouse button is pressed or released. If the mouse is not captured, the message is sent to the window beneath the cursor. Otherwise, the message is sent to the window that has captured the mouse.
        /// </summary>
        public const int WM_NCHITTEST = 0x0084;

        /// <summary>
        /// The WM_NCPAINT message is sent to a window when its frame must be painted.
        /// </summary>
        public const int WM_NCPAINT = 0x85;

        /// <summary>
        /// The WM_NCACTIVATE message is sent to a window when its nonclient area needs to be changed to indicate an active or inactive state.
        /// </summary>
        public const int WM_NCACTIVATE = 0x0086;

        #region Non Client mouse event

        /// <summary>
        /// The WM_NCMOUSEMOVE message is posted to a window when the cursor is moved within the nonclient area of the window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        public const int WM_NCMOUSEMOVE = 0x00A0;

        /// <summary>
        /// The WM_NCLBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0x00A1;

        /// <summary>
        /// The WM_NCLBUTTONUP message is posted when the user releases the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        public const int WM_NCLBUTTONUP = 0x00A2;

        /// <summary>
        /// The WM_NCLBUTTONDBLCLK message is posted when the user double-clicks the left mouse button
        /// while the cursor is within the nonclient area of a window.
        /// </summary>
        public const int WM_NCLBUTTONDBLCLK = 0x00A3;

        /// <summary>
        /// The WM_NCRBUTTONDOWN message is posted when the user presses the right mouse button
        /// while the cursor is within the nonclient area of a window.
        /// </summary>
        public const int WM_NCRBUTTONDOWN = 0x00A4;

        /// <summary>
        /// The WM_NCRBUTTONUP message is posted when the user releases the right mouse button
        /// while the cursor is within the nonclient area of a window.
        /// </summary>
        public const int WM_NCRBUTTONUP = 0x00A5;

        /// <summary>
        /// The WM_NCRBUTTONDBLCLK message is posted when the user double-clicks the right mouse button
        /// while the cursor is within the nonclient area of a window.
        /// </summary>
        public const int WM_NCRBUTTONDBLCLK = 0x00A6;

        /// <summary>
        /// The WM_NCMBUTTONDOWN message is posted when the user presses the middle mouse button
        /// while the cursor is within the nonclient area of a window.
        /// </summary>
        public const int WM_NCMBUTTONDOWN = 0x00A7;

        /// <summary>
        /// The WM_NCMBUTTONUP message is posted when the user releases the middle mouse button
        /// while the cursor is within the nonclient area of a window.
        /// </summary>
        public const int WM_NCMBUTTONUP = 0x00A8;

        /// <summary>
        /// The WM_NCMBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button
        /// while the cursor is within the nonclient area of a window.
        /// </summary>
        public const int WM_NCMBUTTONDBLCLK = 0x00A9;

        /// <summary>
        /// The WM_NCMOUSELEAVE message is posted to a window when the cursor leaves the nonclient area of the window specified in a prior call to TrackMouseEvent.
        /// </summary>
        public const int WM_NCMOUSELEAVE = 0x02A2;

        /// <summary>
        /// The WM_NCMOUSEHOVER message is posted to a window when the cursor hovers over the nonclient area
        /// of the window for the period of time specified in a prior call to TrackMouseEvent.
        /// </summary>
        public const int WM_NCMOUSEHOVER = 0x02A0;

        /// <summary>
        /// The WM_NCXBUTTONDOWN message is posted when the user presses the first or second
        /// X button while the cursor is in the nonclient area of a window.
        /// </summary>
        public const int WM_NCXBUTTONDOWN = 0x00AB;

        /// <summary>
        /// The WM_NCXBUTTONUP message is posted when the user releases the first or second
        /// X button while the cursor is in the nonclient area of a window.
        /// </summary>
        public const int WM_NCXBUTTONUP = 0x00AC;

        #endregion Non Client mouse event

        /// <summary>
        /// A window receives this message when the user chooses a command from the Window menu, clicks the maximize button, minimize button, restore button, close button, or moves the form. You can stop the form from moving by filtering this out.
        /// </summary>
        public const int WM_SYSCOMMAND = 0x0112;

        /// <summary>
        /// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        public const int WM_LBUTTONDOWN = 0x0201;

        /// <summary>
        /// The WM_MOUSELEAVE message is posted to a window when the cursor leaves the client area of the window specified in a prior call to TrackMouseEvent.
        /// </summary>
        public const int WM_MOUSELEAVE = 0x02A3;

        /// <summary>
        /// The WM_MOUSEHOVER message is posted to a window when the cursor hovers over the client area of the window for the period of time specified in a prior call to TrackMouseEvent.
        /// </summary>
        public const int WM_MOUSEHOVER = 0x2A1;

        /// <summary>
        /// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem key is pressed.
        /// A nonsystem key is a key that is pressed when the ALT key is not pressed.
        /// </summary>
        public const int WM_KEYDOWN = 0x0100;

        /// <summary>
        /// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user presses the F10 key
        /// (which activates the menu bar) or holds down the ALT key and then presses another key.
        /// </summary>
        public const int WM_SYSKEYDOWN = 0x0104;

        /// <summary>
        /// The WM_INPUT message is sent to the window that is getting raw input.
        /// </summary>
        public const int WM_INPUT = 0x00FF;

        #region MDI Form Messages

        /// <summary>
        /// An application sends the WM_MDIACTIVATE message to a multiple-document interface (MDI) client window to instruct the client window to activate a different MDI child window.
        /// </summary>
        public const int WM_MDIACTIVATE = 0x0222;

        /// <summary>
        /// An application sends the WM_MDICREATE message to a MDI client window to create an MDI child window.
        /// </summary>
        public const int WM_MDICREATE = 0x0220;

        /// <summary>
        /// An application sends the WM_MDIDESTROY message to a MDI client window to close an MDI child window.
        /// </summary>
        public const int WM_MDIDESTROY = 0x0221;

        /// <summary>
        /// An application sends the WM_MDIRESTORE message to a MDI client window to restore an MDI child window
        /// from maximized or minimized size.
        /// </summary>
        public const int WM_MDIRESTORE = 0x0223;

        /// <summary>
        /// An application sends the WM_MDINEXT message to a MDI client window to activate the next or previous child window.
        /// </summary>
        public const int WM_MDINEXT = 0x0224;

        /// <summary>
        /// An application sends the WM_MDIMAXIMIZE message to a MDI client window to maximize an MDI child window.
        /// </summary>
        public const int WM_MDIMAXIMIZE = 0x0225;

        /// <summary>
        /// An application sends the WM_MDITILE message to a MDI client window to arrange all of its MDI child windows in a tile format.
        /// </summary>
        public const int WM_MDITILE = 0x0226;

        /// <summary>
        /// An application sends the WM_MDICASCADE message to a MDI client window to arrange all its child windows in a cascade format.
        /// </summary>
        public const int WM_MDICASCADE = 0x0227;

        /// <summary>
        /// An application sends the WM_MDIICONARRANGE message to a MDI client window to arrange all minimized MDI child windows.
        /// It does not affect child windows that are not minimized.
        /// </summary>
        public const int WM_MDIICONARRANGE = 0x0228;

        /// <summary>
        /// An application sends the WM_MDIGETACTIVE message to a MDI client window to retrieve the handle to the active MDI child window.
        /// </summary>
        public const int WM_MDIGETACTIVE = 0x0229;

        #endregion MDI Form Messages

        #endregion Window Messages

        #region Window Style

        /// <summary>
        /// Creates a pop-up window. Cannot be used with the WS_CHILD style.
        /// </summary>
        public const uint WS_POPUP = 0x80000000;

        /// <summary>
        /// Creates a window that has a title bar (implies the WS_BORDER style). Cannot be used with the WS_DLGFRAME style.
        /// </summary>
        public const int WS_CAPTION = 0x00C00000;

        /// <summary>
        /// Creates a window that has a border.
        /// </summary>
        public const int WS_BORDER = 0x00800000;

        /// <summary>
        /// Enables the drop shadow effect on a window.
        /// </summary>
        public const int CS_DROPSHADOW = 0x00020000;

        /// <summary>
        /// Sets a new window style.
        /// </summary>
        public const int GWL_STYLE = (-16);

        /// <summary>
        /// Excludes the area occupied by child windows when you draw within the parent window. Used when you create the parent window.
        /// </summary>
        public const int WS_CLIPCHILDREN = 0x02000000;

        #endregion Window Style

        #region Raw Input Device Flags

        /// <summary>
        /// If set, this removes the top level collection from the inclusion list.
        /// This tells the operating system to stop reading from a device which matches the top level collection.
        /// </summary>
        public const int RIDEV_REMOVE = 0x00000001;

        /// <summary>
        /// If set, this specifies the top level collections to exclude when reading a complete usage page.
        /// This flag only affects a TLC whose usage page is already specified with RIDEV_PAGEONLY.
        /// </summary>
        public const int RIDEV_EXCLUDE = 0x00000010;

        /// <summary>
        /// If set, this specifies all devices whose top level collection is from the specified usUsagePage.
        /// Note that usUsage must be zero. To exclude a particular top level collection, use RIDEV_EXCLUDE.
        /// </summary>
        public const int RIDEV_PAGEONLY = 0x00000020;

        /// <summary>
        /// If set, this prevents any devices specified by usUsagePage or usUsage from generating legacy messages.
        /// This is only for the mouse and keyboard. See Remarks.
        /// </summary>
        public const int RIDEV_NOLEGACY = 0x00000030;

        /// <summary>
        /// If set, this enables the caller to receive the input even when the caller is not in the foreground.
        /// Note that hwndTarget must be specified.
        /// </summary>
        public const int RIDEV_INPUTSINK = 0x00000100;

        /// <summary>
        /// If set, the mouse button click does not activate the other window.
        /// </summary>
        public const int RIDEV_CAPTUREMOUSE = 0x00000200;

        /// <summary>
        /// If set, the application-defined keyboard device hotkeys are not handled.
        /// However, the system hotkeys; for example, ALT+TAB and CTRL+ALT+DEL, are still handled.
        /// By default, all keyboard hotkeys are handled. RIDEV_NOHOTKEYS can be specified even if
        /// RIDEV_NOLEGACY is not specified and hwndTarget is NULL.
        /// </summary>
        public const int RIDEV_NOHOTKEYS = 0x00000200;

        /// <summary>
        /// Microsoft Windows XP Service Pack 1 (SP1): If set, the application command keys are handled.
        /// RIDEV_APPKEYS can be specified only if RIDEV_NOLEGACY is specified for a keyboard device.
        /// </summary>
        public const int RIDEV_APPKEYS = 0x00000400;

        public const int RIDEV_EXMODEMASK = 0x000000F0;

        #endregion Raw Input Device Flags

        #region Raw Input Data Command

        /// <summary>
        /// Get the raw data from the RAWINPUT structure.
        /// </summary>
        public const int RID_INPUT = 0x10000003;

        /// <summary>
        /// Get the header information from the RAWINPUT structure.
        /// </summary>
        public const int RID_HEADER = 0x10000005;

        #endregion Raw Input Data Command

        #region Raw Input Type

        /// <summary>
        /// Data comes from a mouse.
        /// </summary>
        public const int RIM_TYPEMOUSE = 0;

        /// <summary>
        /// Data comes from a keyboard.
        /// </summary>
        public const int RIM_TYPEKEYBOARD = 1;

        /// <summary>
        /// Data comes from an HID that is not a keyboard or a mouse.
        /// </summary>
        public const int RIM_TYPEHID = 2;

        #endregion Raw Input Type

        #region Extended Window Style

        public const int WS_EX_LAYERED = 0x00080000;
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int WS_EX_TOOLWINDOW = 0x00000080;

        #endregion Extended Window Style

        #region Alpha Blend

        public const int ULW_ALPHA = 0x00000002;
        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;

        #endregion Alpha Blend

        #region Show Window

        /// <summary>
        /// Displays the window in its current size and position. This value is similar to SW_SHOW, except the window is not activated.
        /// </summary>
        public const int SW_SHOWNA = 8;

        /// <summary>
        /// Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position.
        /// </summary>
        public const int SW_SHOWNORMAL = 1;

        /// <summary>
        /// Retains the current size (ignores the cx and cy parameters).
        /// </summary>
        public const int SWP_NOSIZE = 0x0001;

        /// <summary>
        /// Retains the current position (ignores X and Y parameters).
        /// </summary>
        public const int SWP_NOMOVE = 0x0002;

        /// <summary>
        /// Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost
        /// or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
        /// </summary>
        public const int SWP_NOACTIVATE = 0x0010;

        /// <summary>
        /// Retains the current Z order (ignores the hWndInsertAfter parameter).
        /// </summary>
        public const int SWP_NOZORDER = 0x0004;

        /// <summary>
        /// Does not redraw changes. If this flag is set, no repainting of any kind occurs.
        /// This applies to the client area, the nonclient area (including the title bar and scroll bars),
        /// and any part of the parent window uncovered as a result of the window being moved.
        /// When this flag is set, the application must explicitly invalidate or redraw any parts
        /// of the window and parent window that need redrawing.
        /// </summary>
        public const int SWP_NOREDRAW = 0x0008;

        /// <summary>
        /// Applies new frame styles set using the SetWindowLong function.
        /// Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed.
        /// If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
        /// </summary>
        public const int SWP_FRAMECHANGED = 0x0020;

        #endregion Show Window

        #region Hit Test Result

        /// <summary>
        /// On the screen background or on a dividing line between windows (same as HTNOWHERE,
        /// except that the DefWindowProc function produces a system beep to indicate an error).
        /// </summary>
        public const int HTERROR = (-2);

        /// <summary>
        /// In a window currently covered by another window in the same thread (the message will be sent
        /// to underlying windows in the same thread until one of them returns a code that is not HTTRANSPARENT).
        /// </summary>
        public const int HTTRANSPARENT = (-1);

        /// <summary>
        /// On the screen background or on a dividing line between windows.
        /// </summary>
        public const int HTNOWHERE = 0;

        /// <summary>
        /// In a client area.
        /// </summary>
        public const int HTCLIENT = 1;

        /// <summary>
        /// In a title bar.
        /// </summary>
        public const int HTCAPTION = 2;

        /// <summary>
        /// In a window menu or in a Close button in a child window.
        /// </summary>
        public const int HTSYSMENU = 3;

        /// <summary>
        /// In a size box (same as HTSIZE).
        /// </summary>
        public const int HTGROWBOX = 4;

        /// <summary>
        /// In a size box (same as HTGROWBOX).
        /// </summary>
        public const int HTSIZE = HTGROWBOX;

        /// <summary>
        /// In a menu.
        /// </summary>
        public const int HTMENU = 5;

        /// <summary>
        /// In a horizontal scroll bar.
        /// </summary>
        public const int HTHSCROLL = 6;

        /// <summary>
        /// In the vertical scroll bar.
        /// </summary>
        public const int HTVSCROLL = 7;

        /// <summary>
        /// In a Minimize button.
        /// </summary>
        public const int HTMINBUTTON = 8;

        /// <summary>
        /// In a Maximize button.
        /// </summary>
        public const int HTMAXBUTTON = 9;

        /// <summary>
        /// In the left border of a resizable window (the user can click the mouse to resize the window horizontally).
        /// </summary>
        public const int HTLEFT = 10;

        /// <summary>
        /// In the right border of a resizable window (the user can click the mouse to resize the window horizontally).
        /// </summary>
        public const int HTRIGHT = 11;

        /// <summary>
        /// In the upper-horizontal border of a window.
        /// </summary>
        public const int HTTOP = 12;

        /// <summary>
        /// In the upper-left corner of a window border.
        /// </summary>
        public const int HTTOPLEFT = 13;

        /// <summary>
        /// In the upper-right corner of a window border.
        /// </summary>
        public const int HTTOPRIGHT = 14;

        /// <summary>
        /// In the lower-horizontal border of a resizable window (the user can click the mouse to resize the window vertically).
        /// </summary>
        public const int HTBOTTOM = 15;

        /// <summary>
        /// In the lower-left corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).
        /// </summary>
        public const int HTBOTTOMLEFT = 16;

        /// <summary>
        /// In the lower-right corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).
        /// </summary>
        public const int HTBOTTOMRIGHT = 17;

        /// <summary>
        /// In the border of a window that does not have a sizing border.
        /// </summary>
        public const int HTBORDER = 18;

        /// <summary>
        /// In a Minimize button.
        /// </summary>
        public const int HTREDUCE = HTMINBUTTON;

        /// <summary>
        /// In a Maximize button.
        /// </summary>
        public const int HTZOOM = HTMAXBUTTON;

        public const int HTSIZEFIRST = HTLEFT;
        public const int HTSIZELAST = HTBOTTOMRIGHT;
        public const int HTOBJECT = 19;

        /// <summary>
        /// In a Close button.
        /// </summary>
        public const int HTCLOSE = 20;

        /// <summary>
        /// In a Help button.
        /// </summary>
        public const int HTHELP = 21;

        #endregion Hit Test Result

        #region Redraw Window

        /// <summary>
        /// Invalidates lprcUpdate or hrgnUpdate (only one may be non-NULL). If both are NULL, the entire window is invalidated.
        /// </summary>
        public const int RDW_INVALIDATE = 0x0001;

        /// <summary>
        /// Includes child windows, if any, in the repainting operation.
        /// </summary>
        public const int RDW_ALLCHILDREN = 0x0080;

        /// <summary>
        /// Causes the affected windows (as specified by the RDW_ALLCHILDREN and RDW_NOCHILDREN flags)
        /// to receive WM_NCPAINT, WM_ERASEBKGND, and WM_PAINT messages, if necessary, before the function returns.
        /// </summary>
        public const int RDW_UPDATENOW = 0x0100;

        /// <summary>
        /// Causes the affected windows (as specified by the RDW_ALLCHILDREN and RDW_NOCHILDREN flags)
        /// to receive WM_NCPAINT and WM_ERASEBKGND messages, if necessary, before the function returns.
        /// WM_PAINT messages are received at the ordinary time.
        /// </summary>
        public const int RDW_ERASENOW = 0x0200;

        /// <summary>
        /// Causes any part of the nonclient area of the window that intersects the update region
        /// to receive a WM_NCPAINT message. The RDW_INVALIDATE flag must also be specified; otherwise,
        /// RDW_FRAME has no effect. The WM_NCPAINT message is typically not sent during the execution of RedrawWindow
        /// unless either RDW_UPDATENOW or RDW_ERASENOW is specified.
        /// </summary>
        public const int RDW_FRAME = 0x0400;

        #endregion Redraw Window

        #region Window Handle

        /// <summary>
        /// Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated.
        /// </summary>
        public static IntPtr HWND_TOPMOST = new IntPtr(-1);

        #endregion Window Handle

        #region Window Activation

        /// <summary>
        /// Window is deactivated.
        /// </summary>
        public const int WA_INACTIVE = 0;

        /// <summary>
        /// Activated by some method other than a mouse click (for example,
        /// by a call to the SetActiveWindow function or by use of the keyboard interface to select the window).
        /// </summary>
        public const int WA_ACTIVE = 1;

        /// <summary>
        /// Activated by a mouse click.
        /// </summary>
        public const int WA_CLICKACTIVE = 2;

        #endregion Window Activation

        #endregion Constants
    }

    /// <summary>
    /// Summary description for Win32.
    /// </summary>
    public class User32 {

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }

    public class Kernel32 {

        [DllImport("kernel32.dll")]
        public static extern int GlobalAddAtom(string Name);

        [DllImport("kernel32.dll")]
        public static extern int GlobalDeleteAtom(int atom);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        public static extern bool GlobalUnlock(IntPtr hMem);
    }

    public enum Modifiers { MOD_ALT = 0x0001, MOD_CONTROL = 0x0002, MOD_SHIFT = 0x0004, MOD_WIN = 0x0008 }

    public enum Msgs {
        WM_NULL = 0x0000,
        WM_CREATE = 0x0001,
        WM_DESTROY = 0x0002,
        WM_MOVE = 0x0003,
        WM_SIZE = 0x0005,
        WM_ACTIVATE = 0x0006,
        WM_SETFOCUS = 0x0007,
        WM_KILLFOCUS = 0x0008,
        WM_ENABLE = 0x000A,
        WM_SETREDRAW = 0x000B,
        WM_SETTEXT = 0x000C,
        WM_GETTEXT = 0x000D,
        WM_GETTEXTLENGTH = 0x000E,
        WM_PAINT = 0x000F,
        WM_CLOSE = 0x0010,
        WM_QUERYENDSESSION = 0x0011,
        WM_QUIT = 0x0012,
        WM_QUERYOPEN = 0x0013,
        WM_ERASEBKGND = 0x0014,
        WM_SYSCOLORCHANGE = 0x0015,
        WM_ENDSESSION = 0x0016,
        WM_SHOWWINDOW = 0x0018,
        WM_WININICHANGE = 0x001A,
        WM_SETTINGCHANGE = 0x001A,
        WM_DEVMODECHANGE = 0x001B,
        WM_ACTIVATEAPP = 0x001C,
        WM_FONTCHANGE = 0x001D,
        WM_TIMECHANGE = 0x001E,
        WM_CANCELMODE = 0x001F,
        WM_SETCURSOR = 0x0020,
        WM_MOUSEACTIVATE = 0x0021,
        WM_CHILDACTIVATE = 0x0022,
        WM_QUEUESYNC = 0x0023,
        WM_GETMINMAXINFO = 0x0024,
        WM_PAINTICON = 0x0026,
        WM_ICONERASEBKGND = 0x0027,
        WM_NEXTDLGCTL = 0x0028,
        WM_SPOOLERSTATUS = 0x002A,
        WM_DRAWITEM = 0x002B,
        WM_MEASUREITEM = 0x002C,
        WM_DELETEITEM = 0x002D,
        WM_VKEYTOITEM = 0x002E,
        WM_CHARTOITEM = 0x002F,
        WM_SETFONT = 0x0030,
        WM_GETFONT = 0x0031,
        WM_SETHOTKEY = 0x0032,
        WM_GETHOTKEY = 0x0033,
        WM_QUERYDRAGICON = 0x0037,
        WM_COMPAREITEM = 0x0039,
        WM_GETOBJECT = 0x003D,
        WM_COMPACTING = 0x0041,
        WM_COMMNOTIFY = 0x0044,
        WM_WINDOWPOSCHANGING = 0x0046,
        WM_WINDOWPOSCHANGED = 0x0047,
        WM_POWER = 0x0048,
        WM_COPYDATA = 0x004A,
        WM_CANCELJOURNAL = 0x004B,
        WM_NOTIFY = 0x004E,
        WM_INPUTLANGCHANGEREQUEST = 0x0050,
        WM_INPUTLANGCHANGE = 0x0051,
        WM_TCARD = 0x0052,
        WM_HELP = 0x0053,
        WM_USERCHANGED = 0x0054,
        WM_NOTIFYFORMAT = 0x0055,
        WM_CONTEXTMENU = 0x007B,
        WM_STYLECHANGING = 0x007C,
        WM_STYLECHANGED = 0x007D,
        WM_DISPLAYCHANGE = 0x007E,
        WM_GETICON = 0x007F,
        WM_SETICON = 0x0080,
        WM_NCCREATE = 0x0081,
        WM_NCDESTROY = 0x0082,
        WM_NCCALCSIZE = 0x0083,
        WM_NCHITTEST = 0x0084,
        WM_NCPAINT = 0x0085,
        WM_NCACTIVATE = 0x0086,
        WM_GETDLGCODE = 0x0087,
        WM_SYNCPAINT = 0x0088,
        WM_NCMOUSEMOVE = 0x00A0,
        WM_NCLBUTTONDOWN = 0x00A1,
        WM_NCLBUTTONUP = 0x00A2,
        WM_NCLBUTTONDBLCLK = 0x00A3,
        WM_NCRBUTTONDOWN = 0x00A4,
        WM_NCRBUTTONUP = 0x00A5,
        WM_NCRBUTTONDBLCLK = 0x00A6,
        WM_NCMBUTTONDOWN = 0x00A7,
        WM_NCMBUTTONUP = 0x00A8,
        WM_NCMBUTTONDBLCLK = 0x00A9,
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x0101,
        WM_CHAR = 0x0102,
        WM_DEADCHAR = 0x0103,
        WM_SYSKEYDOWN = 0x0104,
        WM_SYSKEYUP = 0x0105,
        WM_SYSCHAR = 0x0106,
        WM_SYSDEADCHAR = 0x0107,
        WM_KEYLAST = 0x0108,
        WM_IME_STARTCOMPOSITION = 0x010D,
        WM_IME_ENDCOMPOSITION = 0x010E,
        WM_IME_COMPOSITION = 0x010F,
        WM_IME_KEYLAST = 0x010F,
        WM_INITDIALOG = 0x0110,
        WM_COMMAND = 0x0111,
        WM_SYSCOMMAND = 0x0112,
        WM_TIMER = 0x0113,
        WM_HSCROLL = 0x0114,
        WM_VSCROLL = 0x0115,
        WM_INITMENU = 0x0116,
        WM_INITMENUPOPUP = 0x0117,
        WM_MENUSELECT = 0x011F,
        WM_MENUCHAR = 0x0120,
        WM_ENTERIDLE = 0x0121,
        WM_MENURBUTTONUP = 0x0122,
        WM_MENUDRAG = 0x0123,
        WM_MENUGETOBJECT = 0x0124,
        WM_UNINITMENUPOPUP = 0x0125,
        WM_MENUCOMMAND = 0x0126,
        WM_CTLCOLORMSGBOX = 0x0132,
        WM_CTLCOLOREDIT = 0x0133,
        WM_CTLCOLORLISTBOX = 0x0134,
        WM_CTLCOLORBTN = 0x0135,
        WM_CTLCOLORDLG = 0x0136,
        WM_CTLCOLORSCROLLBAR = 0x0137,
        WM_CTLCOLORSTATIC = 0x0138,
        WM_MOUSEMOVE = 0x0200,
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_LBUTTONDBLCLK = 0x0203,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_RBUTTONDBLCLK = 0x0206,
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0x0208,
        WM_MBUTTONDBLCLK = 0x0209,
        WM_MOUSEWHEEL = 0x020A,
        WM_PARENTNOTIFY = 0x0210,
        WM_ENTERMENULOOP = 0x0211,
        WM_EXITMENULOOP = 0x0212,
        WM_NEXTMENU = 0x0213,
        WM_SIZING = 0x0214,
        WM_CAPTURECHANGED = 0x0215,
        WM_MOVING = 0x0216,
        WM_DEVICECHANGE = 0x0219,
        WM_MDICREATE = 0x0220,
        WM_MDIDESTROY = 0x0221,
        WM_MDIACTIVATE = 0x0222,
        WM_MDIRESTORE = 0x0223,
        WM_MDINEXT = 0x0224,
        WM_MDIMAXIMIZE = 0x0225,
        WM_MDITILE = 0x0226,
        WM_MDICASCADE = 0x0227,
        WM_MDIICONARRANGE = 0x0228,
        WM_MDIGETACTIVE = 0x0229,
        WM_MDISETMENU = 0x0230,
        WM_ENTERSIZEMOVE = 0x0231,
        WM_EXITSIZEMOVE = 0x0232,
        WM_DROPFILES = 0x0233,
        WM_MDIREFRESHMENU = 0x0234,
        WM_IME_SETCONTEXT = 0x0281,
        WM_IME_NOTIFY = 0x0282,
        WM_IME_CONTROL = 0x0283,
        WM_IME_COMPOSITIONFULL = 0x0284,
        WM_IME_SELECT = 0x0285,
        WM_IME_CHAR = 0x0286,
        WM_IME_REQUEST = 0x0288,
        WM_IME_KEYDOWN = 0x0290,
        WM_IME_KEYUP = 0x0291,
        WM_MOUSEHOVER = 0x02A1,
        WM_MOUSELEAVE = 0x02A3,
        WM_CUT = 0x0300,
        WM_COPY = 0x0301,
        WM_PASTE = 0x0302,
        WM_CLEAR = 0x0303,
        WM_UNDO = 0x0304,
        WM_RENDERFORMAT = 0x0305,
        WM_RENDERALLFORMATS = 0x0306,
        WM_DESTROYCLIPBOARD = 0x0307,
        WM_DRAWCLIPBOARD = 0x0308,
        WM_PAINTCLIPBOARD = 0x0309,
        WM_VSCROLLCLIPBOARD = 0x030A,
        WM_SIZECLIPBOARD = 0x030B,
        WM_ASKCBFORMATNAME = 0x030C,
        WM_CHANGECBCHAIN = 0x030D,
        WM_HSCROLLCLIPBOARD = 0x030E,
        WM_QUERYNEWPALETTE = 0x030F,
        WM_PALETTEISCHANGING = 0x0310,
        WM_PALETTECHANGED = 0x0311,
        WM_HOTKEY = 0x0312,
        WM_PRINT = 0x0317,
        WM_PRINTCLIENT = 0x0318,
        WM_HANDHELDFIRST = 0x0358,
        WM_HANDHELDLAST = 0x035F,
        WM_AFXFIRST = 0x0360,
        WM_AFXLAST = 0x037F,
        WM_PENWINFIRST = 0x0380,
        WM_PENWINLAST = 0x038F,
        WM_APP = 0x8000,
        WM_USER = 0x0400,
        WM_DDE_INITIATE = 0x03E0,
        WM_DDE_TERMINATE,
        WM_DDE_ADVISE,
        WM_DDE_UNADVISE,
        WM_DDE_ACK,
        WM_DDE_DATA,
        WM_DDE_REQUEST,
        WM_DDE_POKE,
        WM_DDE_EXECUTE
    }

    /// <summary>
    /// Defines a delegate for Message handling
    /// </summary>
    public delegate void MessageEventHandler(object Sender, ref Message msg, ref bool Handled);

    /// <summary>
    /// Inherits from System.Windows.Form.NativeWindow. Provides an Event for Message handling
    /// </summary>
    public class NativeWindowWithEvent : System.Windows.Forms.NativeWindow {

        public event MessageEventHandler ProcessMessage;

        protected override void WndProc(ref Message m) {
            if (ProcessMessage != null) {
                bool Handled = false;
                ProcessMessage(this, ref m, ref Handled);
                if (!Handled) base.WndProc(ref m);
            } else base.WndProc(ref m);
        }
    }

    /// <summary>
    /// Inherits from NativeWindowWithEvent and automatic creates/destroys of a dummy window
    /// </summary>
    public class DummyWindowWithEvent : NativeWindowWithEvent, IDisposable {

        public DummyWindowWithEvent() {
            CreateParams parms = new CreateParams();
            this.CreateHandle(parms);
        }

        public void Dispose() {
            if (this.Handle != (IntPtr)0) {
                this.DestroyHandle();
            }
        }
    }
}