using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Core.Data.TUI {
    /// <summary>
    /// ref : https://github.com/nhn/tui.pagination/blob/production/docs/getting-started.md
    /// </summary>
    public interface ITUIPagingBase {
        /// <summary>
        /// Total number of items
        /// </summary>
        int TotalItems { get; set; }
        /// <summary>
        /// Number of items to draw per page
        /// </summary>
        int ItemsPerPage { get; set; }
        /// <summary>
        /// Number of pages to display
        /// </summary>
        int VisiblePages { get; set; }
        /// <summary>
        /// Current page to display
        /// </summary>
        int Page { get; set; }


    }

    public interface ITUIPagingOption {
        /// <summary>
        /// Whether the page is moved to centered or not
        /// </summary>
        bool CenterAlign { get; set; }
        /// <summary>
        /// The style class name of the first page button
        /// </summary>
        string FirstItemClassName { get; set; }
        /// <summary>
        /// The style class name of the last page button
        /// </summary>
        string LastItemClassName { get; set; }
        /// <summary>
        /// Template for page and move buttons
        /// </summary>
        TUITemplage Template { get; set; }
    }

    public partial class TUITemplage {
        public string Page { get; set; }
        public string CurrentPage { get; set; }
        public string MoveButton { get; set; }
        public string DisableMoveButton { get; set; }
        public string MoreButton { get; set; }
    }
}
