#region Imports

using HL7toXDocumentParser;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using ILog = log4net.ILog;
using LogManager = log4net.LogManager;

#endregion

namespace HHR.HL7.Search
{
    /// <summary>
    /// Summary description for HL7TreeWindow.
    /// </summary>
    public class HL7TreeWindow : ParserWindow
    {
        #region Fields

        System.Windows.Forms.TreeView treeView;
        static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Parser hl7v2Parser = new Parser();

        #endregion

        #region Constructor

        public HL7TreeWindow()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
        }

        #endregion

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(HL7TreeWindow));
            this.treeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.ImageIndex = -1;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = -1;
            this.treeView.Size = new System.Drawing.Size(292, 273);
            this.treeView.TabIndex = 0;
            // 
            // HL7TreeWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.treeView);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HL7TreeWindow";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRightAutoHide;
            this.ResumeLayout(false);

        }

        #endregion

        #region ParserWindow overriden methods

        protected override void ProcessEntity()
        {
            if (Entity == null || Entity.Message == null)
            {
                this.UIThread(() =>
                {
                    this.Cursor = Cursors.WaitCursor;
                    treeView.BeginUpdate();
                    treeView.Nodes.Clear();
                    treeView.EndUpdate();
                    this.Cursor = Cursors.Default;
                });
                return;
            }
                
            XDocument xDocument = hl7v2Parser.Parse(Entity.Message);
            var version = (from elem in xDocument.Descendants("MSH.12") select elem.Value).FirstOrDefault();
            var eventType = (from elem in xDocument.Descendants("MSH.9.2") select elem.Value).FirstOrDefault();

            this.UIThread(() =>
            {
                this.Cursor = Cursors.WaitCursor;
                treeView.BeginUpdate();
                if (eventType != null && eventType.Length == 3)
                    this.Text = "HL7 v" + version + " Tree, " + eventType + " (" + EventTypes[eventType] + ")";
                if (treeView.Nodes != null)
                    treeView.Nodes.Clear();
                var tNode = new TreeNode(xDocument.Root.Name.ToString());
                treeView.Nodes.Add(tNode);
                AddNode(xDocument.Root, tNode);
                treeView.ExpandAll();
                treeView.EndUpdate();
                this.Cursor = Cursors.Default;
            });
        }

        #endregion

        #region DockContent Overriden Methods

        protected override string GetPersistString()
        {
            return GetType().ToString() + "," + this.Text;
        }

        #endregion

        #region Methods

        void AddNode(XElement inXmlNode, TreeNode inTreeNode)
        {
            TreeNode tNode;

            if (inXmlNode.HasElements)
            {
                foreach (var item in inXmlNode.Elements())
                {
                    tNode = new TreeNode(item.Name.ToString());
                    if (inTreeNode != null && inTreeNode.Nodes != null)
                        inTreeNode.Nodes.Add(tNode);
                    AddNode(item, tNode);
                }
            }
            else
            {
                tNode = new TreeNode(inXmlNode.Value.Trim());
                if (inTreeNode != null && inTreeNode.Nodes != null)
                    inTreeNode.Nodes.Add(tNode);
                inTreeNode.Text = inXmlNode.Name.ToString();
            }
        }

        #endregion
    }
}
