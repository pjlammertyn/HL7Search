using HL7toXDocumentParser;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace HHR.HL7.Search
{
    public class SyntaxHighlightingWindow : ParserWindow
    {
        #region Fields

        System.ComponentModel.IContainer components = null;
        RichTextBox richTextBox;
        System.Windows.Forms.ContextMenu contextMenu;
        System.Windows.Forms.MenuItem menuItemSearchPatientId;
        System.Windows.Forms.MenuItem menuItemSearchVisitNumber;
        static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //BackgroundWorker creatorBackgroundWorker = null;
        XDocument xDocument = null;
        Parser hl7v2Parser = new Parser();
        RichTextBox rtb = null;
        IList<string> keywords = new List<string>();
        IList<char> seperators = new List<char>();
        string MSH_9_1;
        string MSH_9_2;
        string key;

        #endregion

        #region Constructor

        public SyntaxHighlightingWindow(string key)
        {
            this.key = key;

            // This call is required by the Windows Form Designer.
            InitializeComponent();
        }

        ~SyntaxHighlightingWindow()
        {
            if (rtb != null)
                rtb.Dispose();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SyntaxHighlightingWindow));
            this.richTextBox = new RichTextBox();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.menuItemSearchPatientId = new System.Windows.Forms.MenuItem();
            this.menuItemSearchVisitNumber = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // syntaxHighlightingTextBox
            // 
            this.richTextBox.ContextMenu = this.contextMenu;
            this.richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox.Location = new System.Drawing.Point(0, 0);
            this.richTextBox.Name = "syntaxHighlightingTextBox";
            this.richTextBox.Size = new System.Drawing.Size(292, 273);
            this.richTextBox.TabIndex = 3;
            this.richTextBox.Text = "";
            this.richTextBox.TextChanged += syntaxHighlightingTextBox_TextChanged;
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.menuItemSearchPatientId,
																						this.menuItemSearchVisitNumber});
            // 
            // menuItemSearchPatientId
            // 
            this.menuItemSearchPatientId.Index = 0;
            this.menuItemSearchPatientId.Text = "Search selected PatientId...";
            this.menuItemSearchPatientId.Click += new System.EventHandler(this.menuItemSearchPatientId_Click);
            // 
            // menuItemSearchVisitNumber
            // 
            this.menuItemSearchVisitNumber.Index = 1;
            this.menuItemSearchVisitNumber.Text = "Search selected VisitNumber...";
            this.menuItemSearchVisitNumber.Click += new System.EventHandler(this.menuItemSearchVisitNumber_Click);
            // 
            // SyntaxHighlightingWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.richTextBox);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SyntaxHighlightingWindow";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
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
                    Text = string.Format("{0} Message", key);            
                    richTextBox.Text = null;
                });
                return;
            }

            var text = string.Empty;
            try
            {
                xDocument = hl7v2Parser.Parse(Entity.Message);

                //var version = (from elem in xDocument.Descendants("MSH.12") select elem.Value).FirstOrDefault();
                var eventType = (from elem in xDocument.Descendants("MSH.9.2") select elem.Value).FirstOrDefault();
                if (eventType != null && eventType.Length == 3)
                    text = string.Format("{0} Message, {1} ({2})", key, eventType, EventTypes[eventType]);

                //SEPERATORS
                seperators.Clear();
                foreach (char c in (from elem in xDocument.Descendants("MSH.1") select elem.Value).FirstOrDefault())
                    seperators.Add(c);
                foreach (char c in (from elem in xDocument.Descendants("MSH.2") select elem.Value).FirstOrDefault())
                    seperators.Add(c);

                //KEYWORDS
                MSH_9_1 = (from elem in xDocument.Descendants("MSH.9.1") select elem.Value).FirstOrDefault();
                MSH_9_2 = (from elem in xDocument.Descendants("MSH.9.2") select elem.Value).FirstOrDefault();
                keywords.Clear();
                foreach (var item in xDocument.Root.Elements())
                    keywords.Add(item.Name.ToString());           
            }
            catch (Exception ex)
            {
                if (log.IsWarnEnabled)
                    log.Warn("Error syntax highlight HL7 message: " + Environment.NewLine + Entity.Message, ex);
            }

            this.UIThread(() =>
                {
                    Text = text;
                    richTextBox.Text = Entity.Message;
                });
        }
 
        #endregion

        #region DockContent Overriden Methods

        protected override string GetPersistString()
        {
            return GetType().ToString() + "," + this.Text;
        }

        #endregion

        #region Events

        void syntaxHighlightingTextBox_TextChanged(object sender, EventArgs e)
        {
            if (seperators.Count == 0)
                return;

            //seperators
            var sb = new StringBuilder();
            sb.Append("[");
            foreach (var item in seperators) //[|^\\&~]
            {
                sb.Append(Regex.Escape(item.ToString()));
            }
            sb.Append("]");
            var boldFont = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold);
            var rex = new Regex(sb.ToString());
            var mc = rex.Matches(richTextBox.Text);
            int StartCursorPosition = richTextBox.SelectionStart;
            foreach (Match m in mc)
            {
                int startIndex = m.Index;
                int StopIndex = m.Length;
                richTextBox.Select(startIndex, StopIndex);
                richTextBox.SelectionFont = boldFont;
                richTextBox.SelectionStart = StartCursorPosition;
                richTextBox.SelectionFont = richTextBox.Font;
            }

            //keywords
            sb = new StringBuilder();
            sb.Append("(");
            foreach (var item in keywords)
            {
                if (sb.Length > 1)
                    sb.Append("|");
                sb.Append(item);
            }
            sb.Append(")");
            rex = new Regex(sb.ToString());
            mc = rex.Matches(richTextBox.Text);
            StartCursorPosition = richTextBox.SelectionStart;
            foreach (Match m in mc)
            {
                int startIndex = m.Index;
                int StopIndex = m.Length;
                richTextBox.Select(startIndex, StopIndex);
                richTextBox.SelectionColor = Color.Blue;
                richTextBox.SelectionStart = StartCursorPosition;
                richTextBox.SelectionColor = Color.Black;
            }

            //MSH.9.1
            rex = new Regex(MSH_9_1);
            mc = rex.Matches(richTextBox.Text);
            StartCursorPosition = richTextBox.SelectionStart;
            foreach (Match m in mc)
            {
                int startIndex = m.Index;
                int StopIndex = m.Length;
                richTextBox.Select(startIndex, StopIndex);
                richTextBox.SelectionColor = Color.Red;
                richTextBox.SelectionStart = StartCursorPosition;
                richTextBox.SelectionColor = Color.Black;
            }

            //MSH.9.2
            rex = new Regex(MSH_9_2);
            mc = rex.Matches(richTextBox.Text);
            StartCursorPosition = richTextBox.SelectionStart;
            foreach (Match m in mc)
            {
                int startIndex = m.Index;
                int StopIndex = m.Length;
                richTextBox.Select(startIndex, StopIndex);
                richTextBox.SelectionColor = Color.Green;
                richTextBox.SelectionStart = StartCursorPosition;
                richTextBox.SelectionColor = Color.Black;
            }
        }

        void menuItemSearchPatientId_Click(object sender, System.EventArgs e)
        {
            OnSearchPatientId(new PatientIdEventArgs(this.richTextBox.SelectedText));
        }

        void menuItemSearchVisitNumber_Click(object sender, System.EventArgs e)
        {
            OnSearchVisitNumber(new VisitNumberEventArgs(new string[] { this.richTextBox.SelectedText }));
        }

        #endregion
    }
}

