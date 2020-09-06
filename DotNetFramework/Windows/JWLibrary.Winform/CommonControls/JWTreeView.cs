using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace JWLibrary.Winform.CommonControls {

    public class JWTreeView : TreeView {

        #region Fields

        private const int SB_HORZ = 0;
        private System.ComponentModel.Container components = null;
        private object dataSource;
        private string dataMember;
        private CurrencyManager listManager;

        private string idPropertyName;
        private string namePropertyName;
        private string parentIdPropertyName;
        private string valuePropertyName;

        private PropertyDescriptor idProperty;
        private PropertyDescriptor nameProperty;
        private PropertyDescriptor parentIdProperty;
        private PropertyDescriptor valueProperty;

        private TypeConverter valueConverter;

        private SortedList items_Positions;
        private SortedList items_Identifiers;

        private bool selectionChanging;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public JWTreeView() {
            this.DoubleBuffered = true;

            this.idPropertyName = string.Empty;
            this.namePropertyName = string.Empty;
            this.parentIdPropertyName = string.Empty;
            this.items_Positions = new SortedList();
            this.items_Identifiers = new SortedList();
            this.selectionChanging = false;

            this.FullRowSelect = true;
            this.HideSelection = false;
            this.HotTracking = true;
            this.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.DataTreeView_AfterSelect);
            this.BindingContextChanged += new System.EventHandler(this.DataTreeView_BindingContextChanged);
            this.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.DataTreeView_AfterLabelEdit);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion Constructors

        #region Win32

        [DllImport("User32.dll")]
        private static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);

        #endregion Win32

        #region Internal classes

        /// <summary>
        /// Tree node with additional data related information.
        /// </summary>
        public class DataTreeViewNode : TreeNode {

            #region Fields

            private int position;

            private object parentID;

            #endregion Fields

            #region Constructors

            /// <summary>
            /// Default constructor of the node.
            /// </summary>
            public DataTreeViewNode(int position) {
                this.position = position;
            }

            #endregion Constructors



            #region Properties

            /// <summary>
            /// Identifier of the node.
            /// </summary>
            public object ID {
                get {
                    return this.Tag;
                }
                set {
                    this.Tag = value;
                }
            }

            /// <summary>
            /// Identifier of the parent node.
            /// </summary>
            public object ParentID {
                get {
                    return this.parentID;
                }
                set {
                    this.parentID = value;
                }
            }

            /// <summary>
            /// Position in the current currency manager.
            /// </summary>
            public int Position {
                get {
                    return this.position;
                }
                set {
                    this.position = value;
                }
            }

            #endregion Properties
        }

        #endregion Internal classes

        #region Properties

        /// <summary>
        /// Data source of the tree.
        /// </summary>
        [
        DefaultValue((string)null),
        TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"),
        RefreshProperties(RefreshProperties.Repaint),
        Category("Data"),
        Description("Data source of the tree.")
        ]
        public object DataSource {
            get {
                return this.dataSource;
            }
            set {
                if (this.dataSource != value) {
                    this.dataSource = value;
                    this.ResetData();
                }
            }
        }

        /// <summary>
        /// Data member of the tree.
        /// </summary>
        [
        DefaultValue(""),
        Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)),
        RefreshProperties(RefreshProperties.Repaint),
        Category("Data"),
        Description("Data member of the tree.")
        ]
        public string DataMember {
            get {
                return this.dataMember;
            }
            set {
                if (this.dataMember != value) {
                    this.dataMember = value;
                    this.ResetData();
                }
            }
        }

        /// <summary>
        /// Identifier member, in most cases this is primary column of the table.
        /// </summary>
        [
        DefaultValue(""),
        Editor(typeof(FieldTypeEditor), typeof(UITypeEditor)),
        Category("Data"),
        Description("Identifier member, in most cases this is primary column of the table.")
        ]
        public string IDColumn {
            get {
                return this.idPropertyName;
            }
            set {
                if (this.idPropertyName != value) {
                    this.idPropertyName = value;
                    this.idProperty = null;

                    if (this.valuePropertyName == null
                        || this.valuePropertyName.Length == 0) {
                        this.ValueColumn = this.idPropertyName;
                    }

                    this.ResetData();
                }
            }
        }

        /// <summary>
        /// Name member. Note: editing of this column available only with types that support converting from string.
        /// </summary>
        [
        DefaultValue(""),
        Editor(typeof(FieldTypeEditor), typeof(UITypeEditor)),
        Category("Data"),
        Description("Name member. Note: editing of this column available only with types that support converting from string.")
        ]
        public string NameColumn {
            get {
                return this.namePropertyName;
            }
            set {
                if (this.namePropertyName != value) {
                    this.namePropertyName = value;
                    this.nameProperty = null;
                    this.ResetData();
                }
            }
        }

        /// <summary>
        /// Identifier of the parent. Note: this member must have the same type as identifier column.
        /// </summary>
        [
        DefaultValue(""),
        Editor(typeof(FieldTypeEditor), typeof(UITypeEditor)),
        Category("Data"),
        Description("Identifier of the parent. Note: this member must have the same type as identifier column.")
        ]
        public string ParentIDColumn {
            get {
                return this.parentIdPropertyName;
            }
            set {
                if (this.parentIdPropertyName != value) {
                    this.parentIdPropertyName = value;
                    this.parentIdProperty = null;
                    this.ResetData();
                }
            }
        }

        /// <summary>
        /// Value member. Form this column value will be taken.
        /// </summary>
        [
        DefaultValue(""),
        Editor(typeof(FieldTypeEditor), typeof(UITypeEditor)),
        Category("Data"),
        Description("Value member. Form this column value will be taken.")
        ]
        public string ValueColumn {
            get {
                return this.valuePropertyName;
            }
            set {
                if (this.valuePropertyName != value) {
                    this.valuePropertyName = value;
                    this.valueProperty = null;
                    this.valueConverter = null;
                }
            }
        }

        /// <summary>
        /// Get value current selected item.
        /// </summary>
        [
        Category("Data"),
        Description("Get value current selected item.")
        ]
        public object Value {
            get {
                if (this.SelectedNode != null) {
                    DataTreeViewNode node = this.SelectedNode as DataTreeViewNode;
                    if (node != null && this.PrepareValueDescriptor()) {
                        return this.valueProperty.GetValue(this.listManager.List[node.Position]);
                    }
                }
                return null;
            }
        }

        public object CurrentObj {
            get {
                if (this.SelectedNode != null) {
                    DataTreeViewNode node = this.SelectedNode as DataTreeViewNode;
                    if (node != null && this.PrepareValueDescriptor()) {
                        return this.listManager.Current;
                    }
                }
                return null;
            }
        }

        #endregion Properties

        #region Events

        private void DataTreeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e) {
            this.BeginSelectionChanging();

            DataTreeViewNode node = e.Node as DataTreeViewNode;
            if (node != null) {
                this.listManager.Position = node.Position;
            }
            this.EndSelectionChanging();
        }

        private void DataTreeView_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e) {
            DataTreeViewNode node = e.Node as DataTreeViewNode;
            if (node != null) {
                if (this.PrepareValueConvertor()
                    && this.valueConverter.IsValid(e.Label)
                    ) {
                    this.nameProperty.SetValue(
                        this.listManager.List[node.Position],
                        this.valueConverter.ConvertFromString(e.Label)
                        );
                    this.listManager.EndCurrentEdit();
                    return;
                }
            }
            e.CancelEdit = true;
        }

        private void DataTreeView_BindingContextChanged(object sender, System.EventArgs e) {
            this.ResetData();
        }

        private void listManager_PositionChanged(object sender, EventArgs e) {
            this.SynchronizeSelection();
        }

        private void DataTreeView_ListChanged(object sender, ListChangedEventArgs e) {
            switch (e.ListChangedType) {
                case ListChangedType.ItemAdded:
                    if (!TryAddNode(this.CreateNode(this.listManager, e.NewIndex))) {
                        throw new ApplicationException("Item are not added.");
                    }
                    break;

                case ListChangedType.ItemChanged:
                    DataTreeViewNode chnagedNode = this.items_Positions[e.NewIndex] as DataTreeViewNode;
                    if (chnagedNode != null) {
                        this.RefreshData(chnagedNode);
                        this.ChangeParent(chnagedNode);
                    } else {
                        throw new ApplicationException("Item not found or wrong type.");
                    }
                    break;

                case ListChangedType.ItemMoved:
                    DataTreeViewNode movedNode = this.items_Positions[e.OldIndex] as DataTreeViewNode;
                    if (movedNode != null) {
                        this.items_Positions.Remove(e.OldIndex);
                        this.items_Positions.Add(e.NewIndex, movedNode);
                    } else {
                        throw new ApplicationException("Item not found or wrong type.");
                    }
                    break;

                case ListChangedType.ItemDeleted:
                    DataTreeViewNode deletedNode = this.items_Positions[e.OldIndex] as DataTreeViewNode;
                    if (deletedNode != null) {
                        this.items_Positions.Remove(e.OldIndex);
                        this.items_Identifiers.Remove(deletedNode.ID);
                        deletedNode.Remove();
                    } else {
                        /*deletedNode = this.items_Positions[listManager.Position] as DataTreeViewNode;

                        if (deletedNode != null)
                        {
                            this.items_Positions.Remove(listManager.Position);
                            this.items_Identifiers.Remove(deletedNode.ID);
                            deletedNode.Remove();
                        }
                        else
                        {
                            //throw new ApplicationException("Item not found or wrong type.");
                        }*/
                    }
                    this.ResetData();
                    break;

                case ListChangedType.Reset:
                    this.ResetData();
                    break;
            }
        }

        #endregion Events

        #region Implementation

        private void Clear() {
            this.items_Positions.Clear();
            this.items_Identifiers.Clear();

            this.Nodes.Clear();
        }

        private bool PrepareDataSource() {
            if (this.BindingContext != null) {
                if (this.dataSource != null) {
                    this.listManager = this.BindingContext[this.dataSource, this.dataMember] as CurrencyManager;
                    return true;
                } else {
                    this.listManager = null;
                    this.Clear();
                }
            }
            return false;
        }

        private bool PrepareDescriptors() {
            if (this.idPropertyName.Length != 0
                && this.namePropertyName.Length != 0
                && this.parentIdPropertyName.Length != 0) {
                if (this.idProperty == null) {
                    this.idProperty = this.listManager.GetItemProperties()[this.idPropertyName];
                }
                if (this.nameProperty == null) {
                    this.nameProperty = this.listManager.GetItemProperties()[this.namePropertyName];
                }
                if (this.parentIdProperty == null) {
                    this.parentIdProperty = this.listManager.GetItemProperties()[this.parentIdPropertyName];
                }
            }

            return (this.idProperty != null
                && this.nameProperty != null
                && this.parentIdProperty != null);
        }

        private bool PrepareValueDescriptor() {
            if (this.valueProperty == null) {
                if (this.valuePropertyName == string.Empty) {
                    this.valuePropertyName = this.idPropertyName;
                }
                this.valueProperty = this.listManager.GetItemProperties()[this.valuePropertyName];
            }

            return (this.valueProperty != null);
        }

        private bool PrepareValueConvertor() {
            if (this.valueConverter == null) {
                this.valueConverter = TypeDescriptor.GetConverter(this.nameProperty.PropertyType) as TypeConverter;
            }

            return (this.valueConverter != null
                && this.valueConverter.CanConvertFrom(typeof(string))
                );
        }

        private void WireDataSource() {
            this.listManager.PositionChanged += new EventHandler(listManager_PositionChanged);
            ((IBindingList)this.listManager.List).ListChanged += new ListChangedEventHandler(DataTreeView_ListChanged);
        }

        private void ResetData() {
            this.BeginUpdate();

            this.Clear();

            if (this.PrepareDataSource()) {
                this.WireDataSource();
                if (this.PrepareDescriptors()) {
                    ArrayList unsortedNodes = new ArrayList();
                    for (int i = 0; i < this.listManager.Count; i++) {
                        unsortedNodes.Add(this.CreateNode(this.listManager, i));
                    }

                    int startCount;

                    while (unsortedNodes.Count > 0) {
                        startCount = unsortedNodes.Count;

                        for (int i = unsortedNodes.Count - 1; i >= 0; i--) {
                            if (this.TryAddNode((DataTreeViewNode)unsortedNodes[i])) {
                                unsortedNodes.RemoveAt(i);
                            }
                        }

                        if (startCount == unsortedNodes.Count) {
                            break;
                            //throw new ApplicationException("Tree view confused when try to make your data hierarchical.");
                        }
                    }
                }
            }

            this.EndUpdate();
        }

        private bool TryAddNode(DataTreeViewNode node) {
            if (this.IsIDNull(node.ParentID) || node.ParentID.ToString() == "-1") {
                this.AddNode(this.Nodes, node);
                return true;
            } else {
                if (this.items_Identifiers.ContainsKey(node.ParentID)) {
                    TreeNode parentNode = this.items_Identifiers[node.ParentID] as TreeNode;
                    if (parentNode != null) {
                        this.AddNode(parentNode.Nodes, node);
                        return true;
                    }
                }
            }
            return false;
        }

        private void AddNode(TreeNodeCollection nodes, DataTreeViewNode node) {
            /*if (this.items_Positions.ContainsKey(node.Position))
            {
                return;
            }*/

            try {
                /*if (this.items_Identifiers.ContainsKey(node.ID))
                {
                    return;
                }*/

                this.items_Positions.Add(node.Position, node);
                this.items_Identifiers.Add(node.ID, node);
                nodes.Add(node);
            } catch { }
        }

        private void ChangeParent(DataTreeViewNode node) {
            object dataParentID = this.parentIdProperty.GetValue(this.listManager.List[node.Position]);
            if (node.ParentID.ToString() != dataParentID.ToString()) {
                if (dataParentID.ToString() == "-1") dataParentID = 0;

                DataTreeViewNode newParentNode = this.items_Identifiers[dataParentID] as DataTreeViewNode;
                if (newParentNode != null) {
                    node.Remove();
                    newParentNode.Nodes.Add(node);
                } else {
                    throw new ApplicationException("Item not found or wrong type.");
                }
            } else {
            }
        }

        private void SynchronizeSelection() {
            if (!this.selectionChanging) {
                DataTreeViewNode node = this.items_Positions[this.listManager.Position] as DataTreeViewNode;
                if (node != null) {
                    this.SelectedNode = node;
                }
            }
        }

        private void RefreshData(DataTreeViewNode node) {
            int position = node.Position;
            node.ID = this.idProperty.GetValue(this.listManager.List[position]);
            node.Text = (string)this.nameProperty.GetValue(this.listManager.List[position]);
            node.ParentID = this.parentIdProperty.GetValue(this.listManager.List[position]);
        }

        /// <summary>
        /// Create a DataTreeViewNode with currency manager and position.
        /// </summary>
        /// <param name="currencyManager"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private DataTreeViewNode CreateNode(CurrencyManager currencyManager, int position) {
            DataTreeViewNode node = new DataTreeViewNode(position);
            this.RefreshData(node);
            return node;
        }

        private object GetPerentID(CurrencyManager currencyManager, int position) {
            return this.parentIdProperty.GetValue(currencyManager.List[position]);
        }

        private bool IsIDNull(object id) {
            if (id == null
                || Convert.IsDBNull(id)) {
                return true;
            } else {
                if (id.GetType() == typeof(string)) {
                    return (((string)id).Length == 0);
                } else if (id.GetType() == typeof(Guid)) {
                    return ((Guid)id == Guid.Empty);
                }
            }
            return false;
        }

        protected override void InitLayout() {
            base.InitLayout();
            ShowScrollBar(Handle, SB_HORZ, false);
        }

        private void BeginSelectionChanging() {
            this.selectionChanging = true;
        }

        private void EndSelectionChanging() {
            this.selectionChanging = false;
        }

        #endregion Implementation
    }

    internal abstract class DropDownTypeEditor : System.Drawing.Design.UITypeEditor {

        #region Fields

        private IWindowsFormsEditorService editorService;

        #endregion Fields

        #region Events

        private void ListBox_SelectedIndexChanged(object objSender, EventArgs eventArgs) {
            if (this.editorService != null) {
                this.editorService.CloseDropDown();
            }
        }

        #endregion Events

        #region Implemenatation

        /// <summary>
        /// Overrides to set editor style in DropDown.
        /// </summary>
        /// <param name="context">Type descriptor context.</param>
        /// <returns>Style of the editor.</returns>
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            if (context != null && context.Instance != null) {
                return UITypeEditorEditStyle.DropDown;
            }
            return base.GetEditStyle(context);
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
            if (context != null && context.Instance != null && context.Container != null && provider != null) {
                this.editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                if (this.editorService != null) {
                    using (ListBox listBox = new ListBox()) {
                        listBox.BorderStyle = BorderStyle.None;

                        this.FillListBox(listBox, context, value);
                        listBox.SelectedIndexChanged += new EventHandler(ListBox_SelectedIndexChanged);

                        this.editorService.DropDownControl(listBox);

                        if (listBox.SelectedItem != null) {
                            value = GetValueFromListItem(context, listBox.SelectedItem);
                        }
                        listBox.SelectedIndexChanged -= new EventHandler(ListBox_SelectedIndexChanged);
                    }
                    this.editorService = null;
                }
            }
            return value;
        }

        /// <summary>
        /// Implement this to fill listBox.Items.
        /// </summary>
        /// <param name="listBox">ListBox control to fill.</param>
        /// <param name="context">Type descriptor context.</param>
        /// <param name="value">Current value.</param>
        protected abstract void FillListBox(ListBox listBox, ITypeDescriptorContext context, object value);

        /// <summary>
        /// Override this to change output value.
        /// </summary>
        /// <param name="context">Type descriptor context.</param>
        /// <param name="value">Value to change.</param>
        /// <returns>Changed value.</returns>
        protected virtual object GetValueFromListItem(ITypeDescriptorContext context, object value) {
            return value;
        }

        #endregion Implemenatation
    }

    internal class FieldTypeEditor : DropDownTypeEditor {

        /// <summary>
        /// Fill list box with fileds of the current datasource.
        /// </summary>
        /// <param name="listBox">ListBox control to fill.</param>
        /// <param name="context">Type descriptor context.</param>
        /// <param name="value">Current value.</param>
        protected override void FillListBox(ListBox listBox, ITypeDescriptorContext context, object value) {
            string selectedFiled = (string)value;
            if (selectedFiled == null) {
                selectedFiled = string.Empty;
            }

            PropertyDescriptor dataSourceDescriptor = TypeDescriptor.GetProperties(context.Instance)["DataSource"];
            PropertyDescriptor dataMemberDescriptor = TypeDescriptor.GetProperties(context.Instance)["DataMember"];
            if (dataSourceDescriptor == null
                || dataMemberDescriptor == null) {
                return;
            }
            object dataSource = dataSourceDescriptor.GetValue(context.Instance);
            string dataMember = (string)dataMemberDescriptor.GetValue(context.Instance);
            if (dataSource != null) {
                CurrencyManager currencyManager = new BindingContext()[dataSource, dataMember] as CurrencyManager;
                if (currencyManager != null) {
                    int lastIndex;
                    foreach (PropertyDescriptor descriptor in currencyManager.GetItemProperties()) {
                        lastIndex = listBox.Items.Add(descriptor.Name);
                        if (string.Compare(descriptor.Name, selectedFiled) == 0) {
                            listBox.SelectedIndex = lastIndex;
                        }
                    }
                }
            }
        }
    }
}