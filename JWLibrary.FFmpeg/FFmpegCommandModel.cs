namespace JWLibrary.FFmpeg
{
    public class FFmpegCommandModel
    {
        public FFmpegCommandModel()
        {
            this._outPutQuality = "30";
            this._videoSource = "screen-capture-recorder";
            this._audioSource = "virtual-audio-capturer";
        }

        private string _videoSource;

        /// <summary>
        /// recording video source (init : screen-capture-recorder)
        /// </summary>
        public string VideoSource
        {
            get { return _videoSource; }
            set { _videoSource = value; }
        }

        private string _audioSource;

        /// <summary>
        /// recording audio source (init : virtual-audio-capturer)
        /// </summary>
        public string AudioSource
        {
            get { return _audioSource; }
            set { _audioSource = value; }
        }

        private string _executeFileName;

        /// <summary>
        /// not use
        /// </summary>
        public string ExecuteFileName
        {
            get { return this._executeFileName; }
            set { this._executeFileName = value; }
        }
        private string _generalCommand;

        /// <summary>
        /// not use
        /// </summary>
        public string GeneralCommand
        {
            get { return this._generalCommand; }
            set { this._generalCommand = value; }
        }
        private string _frameRate;
        public string FrameRate
        {
            get { return this._frameRate; }
            set { this._frameRate = value; }
        }
        private string _videoDeviceName;

        /// <summary>
        /// not use
        /// </summary>
        public string VideoDeviceName
        {
            get { return this._videoDeviceName; }
            set { this._videoDeviceName = value; }
        }
        private string _audioDeviceName;

        /// <summary>
        /// not use
        /// </summary>
        public string AudioDeviceName
        {
            get { return this._audioDeviceName; }
            set { this._audioDeviceName = value; }
        }

        private string _offsetX;
        public string OffsetX
        {
            get { return this._offsetX; }
            set { this._offsetX = value; }
        }

        private string _offsetY;
        public string OffsetY
        {
            get { return this._offsetY; }
            set { this._offsetY = value; }
        }

        private string _width;
        public string Width
        {
            get { return this._width; }
            set { this._width = value; }
        }
        private string _height;
        public string Height
        {
            get { return this._height; }
            set { this._height = value; }
        }

        private string _fullFileName;
        /// <summary>
        /// Base name is local file name.
        /// If you using twitchTV and Youtube live, enter live token or path.
        /// But twitchTV and youtube live not implemented yet.
        /// </summary>
        public string FullFileName
        {
            get { return this._fullFileName; }
            set { this._fullFileName = value; }
        }

        private string _preset;
        public string Preset
        {
            get
            {
                return _preset;
            }
            set
            {
                this._preset = value;
            }
        }

        private string _audioQuality;
        public string AudioQuality
        {
            get
            {
                return _audioQuality;
            }
            set
            {
                this._audioQuality = value;
            }
        }

        private string _format;
        public string Format
        {
            get
            {
                return _format;
            }
            set
            {
                this._format = value;
            }
        }

        private string _option1;
        /// <summary>
        /// Optional ffmpeg argument
        /// </summary>
        public string Option1
        {
            get
            {
                return _option1;
            }
            set
            {
                this._option1 = value;
            }
        }

        private string _outPutQuality;
        /// <summary>
        /// Initial value is 30.
        /// </summary>
        public string OutPutQuality
        {
            get
            {
                return _outPutQuality;
            }
            set
            {
                this._outPutQuality = value;
            }
        }
    }
}
