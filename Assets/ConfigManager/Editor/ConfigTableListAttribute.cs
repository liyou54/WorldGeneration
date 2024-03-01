using System;
using UnityEngine;

namespace Sirenix.OdinInspector.Editor.Drawers
{
    public class ConfigTableListAttribute : Attribute
    {
        /// <summary>
        /// If ShowPaging is enabled, this will override the default setting specified in the Odin Preferences window.
        /// </summary>
        public int NumberOfItemsPerPage;

        /// <summary>
        /// Mark the table as read-only. This removes all editing capabilities from the list such as Add and delete,
        /// but without disabling GUI for each element drawn as otherwise would be the case if the <see cref="T:Sirenix.OdinInspector.ReadOnlyAttribute" /> was used.
        /// </summary>
        public bool IsReadOnly;

        /// <summary>
        /// The default minimum column width - 40 by default. This can be overwriten by individual columns using the <see cref="T:Sirenix.OdinInspector.TableColumnWidthAttribute" />.
        /// </summary>
        public int DefaultMinColumnWidth = 40;

        /// <summary>
        /// If true, a label is drawn for each element which shows the index of the element.
        /// </summary>
        public bool ShowIndexLabels;

        /// <summary>Whether to draw all rows in a scroll-view.</summary>
        public bool DrawScrollView = true;

        /// <summary>
        /// The number of pixels before a scroll view appears. 350 by default.
        /// </summary>
        public int MinScrollViewHeight = 350;

        /// <summary>
        /// The number of pixels before a scroll view appears. 0 by default.
        /// </summary>
        public int MaxScrollViewHeight;

        /// <summary>
        /// If true, expanding and collapsing the table from the table title-bar is no longer an option.
        /// </summary>
        public bool AlwaysExpanded;

        /// <summary>
        /// Whether to hide the toolbar containing the add button and pagin etc.s
        /// </summary>
        public bool HideToolbar;

        /// <summary>The cell padding.</summary>
        public int CellPadding = 2;

        [SerializeField] [HideInInspector] private bool showPagingHasValue;
        [SerializeField] [HideInInspector] private bool showPaging;

        /// <summary>
        /// Whether paging buttons should be added to the title bar. The default value of this, can be customized from the Odin Preferences window.
        /// </summary>
        public bool ShowPaging
        {
            get => this.showPaging;
            set
            {
                this.showPaging = value;
                this.showPagingHasValue = true;
            }
        }

        /// <summary>Whether the ShowPaging property has been set.</summary>
        public bool ShowPagingHasValue => this.showPagingHasValue;

        /// <summary>Sets the Min and Max ScrollViewHeight.</summary>
        public int ScrollViewHeight
        {
            get => Math.Min(this.MinScrollViewHeight, this.MaxScrollViewHeight);
            set => this.MinScrollViewHeight = this.MaxScrollViewHeight = value;
        }
    }
}