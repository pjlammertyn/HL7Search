using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Text;
using System.Threading;

using HHR.Business.Entities;
using HHR.ZIS.Business.Entities;
using HHR.ZIS.Business.Logic;
using HHR.ZIS.Data;

//using HHR.HL7.Check;

namespace HHR.HL7.Search
{
	public class OutputDoc : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		#region Fields

        //private FieldInfo[] Fields = null;
        //private IDalVisit DalVisit1 = null;
        //private IDalVisit DalVisit2 = null;
        //private IDalAdmission DalAdmission1 = null;
        //private IDalAdmission DalAdmission2 = null;
        //private IDalStay DalStay1 = null;
        //private IDalStay DalStay2 = null;
        //private IDalVisitInsurance DalVisitInsurance1 = null;
        //private IDalVisitInsurance DalVisitInsurance2 = null;
        //private IDalDoctorAttendation DalDoctorAttendation1 = null;
        //private IDalDoctorAttendation DalDoctorAttendation2 = null;
        //private IDalLeave DalLeave1 = null;
        //private IDalLeave DalLeave2 = null;
        //private enumVisitType _VisitType = enumVisitType.Unknown;
        private DateTime _CheckDate;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem menuItemSearchPatientId;
		private System.Windows.Forms.MenuItem menuItemSearchVisitNumber;
		private System.ComponentModel.IContainer components = null;
        //private Thread thread = null;
        //private Thread threadDal1 = null;
        //private Thread threadDal2 = null;
        //private bVisitSet visitSet1 = null;
		private System.Windows.Forms.RichTextBox richTextBox;
        //private bVisitSet visitSet2 = null;
        //private bool _ExportDifferentVisitNumbers = false;
        //private bool _CorrectDifferences = false;
        private ZisCheck zisCheck;

		#endregion
	
		#region Constructor

        //public OutputDoc(bool exportDifferentVisitNumbers, bool correctDifferences, enumVisitType visitType, string text, IDalVisit DalVisit1, IDalVisit DalVisit2,
        //    IDalAdmission DalAdmission1, IDalAdmission DalAdmission2,
        //    IDalStay DalStay1, IDalStay DalStay2, 
        //    IDalDoctorAttendation DalDoctorAttendation1, IDalDoctorAttendation DalDoctorAttendation2,
        //    IDalVisitInsurance DalVisitInsurance1, IDalVisitInsurance DalVisitInsurance2,
        //    IDalLeave DalLeave1, IDalLeave DalLeave2)
        //{
        //    // This call is required by the Windows Form Designer.
        //    InitializeComponent();

        //    _VisitType = visitType;
        //    _CheckDate = DateTime.Now;
        //    SetText(text + " @ " + _CheckDate.ToString("HH:mm"));
        //    _ExportDifferentVisitNumbers = exportDifferentVisitNumbers;
        //    _CorrectDifferences = correctDifferences;

        //    this.DalVisit1 = DalVisit1;
        //    this.DalVisit2 = DalVisit2;
        //    this.DalAdmission1 = DalAdmission1;
        //    this.DalAdmission2 = DalAdmission2;
        //    this.DalStay1 = DalStay1;
        //    this.DalStay2 = DalStay2;
        //    this.DalDoctorAttendation1 = DalDoctorAttendation1;
        //    this.DalDoctorAttendation2 = DalDoctorAttendation2;
        //    this.DalVisitInsurance1 = DalVisitInsurance1;
        //    this.DalVisitInsurance2 = DalVisitInsurance2;
        //    this.DalLeave1 = DalLeave1;
        //    this.DalLeave2 = DalLeave2;
        //}

        public OutputDoc(ZisCheck zisCheck)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            this.zisCheck = zisCheck;
            this.zisCheck.AppendPatientText += new ZisCheck.AppendTextEventHandler(zisCheck_AppendText);
            this.zisCheck.AppendVisitDetailText += new ZisCheck.AppendTextEventHandler(zisCheck_AppendText);
            this.zisCheck.AppendVisitText += new ZisCheck.AppendTextEventHandler(zisCheck_AppendText);
            this.zisCheck.RunWorkerCompleted += new RunWorkerCompletedEventHandler(zisCheck_RunWorkerCompleted);

            _CheckDate = DateTime.Now;
            this.TabText = _CheckDate.ToString();
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.menuItemSearchPatientId = new System.Windows.Forms.MenuItem();
            this.menuItemSearchVisitNumber = new System.Windows.Forms.MenuItem();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
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
            // richTextBox
            // 
            this.richTextBox.ContextMenu = this.contextMenu;
            this.richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox.Location = new System.Drawing.Point(0, 0);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(292, 273);
            this.richTextBox.TabIndex = 0;
            this.richTextBox.Text = "";
            // 
            // OutputDoc
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.richTextBox);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Name = "OutputDoc";
            this.TabPageContextMenu = this.contextMenu;
            this.Load += new System.EventHandler(this.OutputDoc_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.OutputDoc_Closing);
            this.ResumeLayout(false);

		}
		#endregion

		#region Events

		private void OutputDoc_Load(object sender, System.EventArgs e)
		{
            SetCursor(Cursors.WaitCursor);

            //thread = new Thread(new ThreadStart(CheckZisDb));
            //thread.IsBackground = true;
            //thread.Start();
            this.zisCheck.RunWorkerAsync();
		}

		private void OutputDoc_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            //if (thread != null)
            //    thread.Interrupt();
		}

		private void menuItemSearchPatientId_Click(object sender, System.EventArgs e)
		{
			decimal patientId = decimal.Zero;
			try
			{
				patientId = Convert.ToDecimal(this.richTextBox.SelectedText);
			}
			catch(Exception)
			{
				MessageBox.Show("The patient id must be numeric!", "No valid patient id!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			//invoke event
			PatientIdEventArgs eventArgs = new PatientIdEventArgs(patientId);
			OnSearchPatientId(eventArgs);
		}

		private void menuItemSearchVisitNumber_Click(object sender, System.EventArgs e)
		{
			decimal visitNumber = decimal.Zero;
			try
			{
				visitNumber = Convert.ToDecimal(this.richTextBox.SelectedText);
			}
			catch(Exception)
			{
				MessageBox.Show("The visit number must be numeric!", "No valid visit number!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			//invoke event
			VisitNumberEventArgs eventArgs = new VisitNumberEventArgs(new decimal[] {visitNumber});
			OnSearchVisitNumber(eventArgs);
		}

        private void zisCheck_AppendText(object sender, AppendTextEventArgs e)
        {
            AppendText(e.Text);
        }

        private void zisCheck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AppendText(string.Format("Done!{0}", Environment.NewLine));

            SetCursor(Cursors.IBeam);
        }

		#endregion

		#region DockContent Overriden Methods

		protected override string GetPersistString()
		{
			return GetType().ToString() + "," + _CheckDate.ToString();
		}

		#endregion

        #region Methods

        #region Invoke Methods

        private void SetText(string text)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new StringParameterDelegate(SetText), new object[] { text });
                return;
            }
            // Must be on the UI thread if we've got this far
            this.Text = text;
        }

        private void AppendText(string value)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new StringParameterDelegate(AppendText), new object[] { value });
                return;
            }
            // Must be on the UI thread if we've got this far
            try
            {
                if (this.richTextBox != null)
                    this.richTextBox.AppendText(value);
            }
            catch (ObjectDisposedException)
            { }
        }

        private void SetCursor(Cursor cursor)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new SetCursorDelegate(SetCursor), new object[] { cursor });
                return;
            }
            // Must be on the UI thread if we've got this far
            if (this.richTextBox != null)
                this.richTextBox.Cursor = cursor;
        }


        #endregion

//        #region Check Methods

//        public void CheckZisDb()
//        {
//            AppendText(string.Format("Starting...{0}", Environment.NewLine));

//            threadDal1 = new Thread(new ThreadStart(GetDal1));
//            threadDal1.IsBackground = true;
//            threadDal1.Start();
//            threadDal2 = new Thread(new ThreadStart(GetDal2));
//            threadDal2.IsBackground = true;
//            threadDal2.Start();
			
//            try
//            {
//                threadDal1.Join();
//                threadDal2.Join();
//            }
//            catch(ThreadInterruptedException)
//            {}

//            bVisitSet differentVisitSet = CompareVisitSets(visitSet1,visitSet2);
//            if(_ExportDifferentVisitNumbers)
//                ExportDifferentVisitSet(differentVisitSet);
//            //if(_CorrectDifferences)
//            //    CorrectDifferentVisitSet(differentVisitSet);

//            differentVisitSet.Dispose();

//            AppendText(string.Format("Done!{0}", Environment.NewLine));

//            SetCursor(Cursors.IBeam);
//        }

//        private void GetDal1()
//        {
//            bAdmissionSet admissionSet = null;
//            bStaySet staySet = null;
//            bDoctorAttendationSet doctorAttendationSet = null;
//            bVisitInsuranceSet visitInsuranceSet = null;
//            bLeaveSet leaveSet = null;

//            if (DalVisit1 != null)
//            {
//                visitSet1 = GetOpenVisitSetByDateAndVisitType(this.DalVisit1, this._CheckDate, this._VisitType);
//                visitSet1.Sort(new CompareVisitByNumber());

//                if (DalAdmission1 != null)
//                {
//                    admissionSet = GetAdmissionSetByOpenVisits(this.DalAdmission1, this._CheckDate, this._VisitType);
//                    admissionSet.Sort(new CompareAdmission());

//                    MapAdmissionSet(visitSet1, admissionSet);
//                }

//                if (DalStay1 != null)
//                {
//                    staySet = GetStaySetByOpenVisits(this.DalStay1, this._CheckDate, this._VisitType);
//                    staySet.Sort(new CompareStay());

//                    MapStaySet(visitSet1, staySet);
//                }

//                if (DalDoctorAttendation1 != null)
//                {
//                    doctorAttendationSet = GetDoctorAttendationSetByOpenVisits(this.DalDoctorAttendation1, this._CheckDate, this._VisitType);
//                    doctorAttendationSet.Sort(new CompareDoctorAttendation());

//                    MapDoctorAttendationSet(visitSet1, doctorAttendationSet);
//                }

//                if (DalVisitInsurance1 != null)
//                {
//                    visitInsuranceSet = GetVisitInsuranceSetByOpenVisits(this.DalVisitInsurance1, this._CheckDate, this._VisitType);
//                    visitInsuranceSet.Sort(new CompareVisitInsurance());

//                    MapVisitInsuranceSet(visitSet1, visitInsuranceSet);
//                }

//                if (DalLeave1 != null)
//                {
//                    leaveSet = GetLeaveSetByOpenVisits(this.DalLeave1, this._CheckDate, this._VisitType);

//                    MapLeaveSet(visitSet1, leaveSet);
//                }
//            }
//        }

//        private void GetDal2()
//        {
//            bAdmissionSet admissionSet = null;
//            bStaySet staySet = null;
//            bDoctorAttendationSet doctorAttendationSet = null;
//            bVisitInsuranceSet visitInsuranceSet = null;
//            bLeaveSet leaveSet = null;

//            if (DalVisit2 != null)
//            {
//                visitSet2 = GetOpenVisitSetByDateAndVisitType(this.DalVisit2, this._CheckDate, this._VisitType);
//                visitSet2.Sort(new CompareVisit());

//                if (DalAdmission2 != null)
//                {
//                    admissionSet = GetAdmissionSetByOpenVisits(this.DalAdmission2, this._CheckDate, this._VisitType);
//                    admissionSet.Sort(new CompareAdmission());

//                    MapAdmissionSet(visitSet2, admissionSet);
//                }

//                if (DalStay2 != null)
//                {
//                    staySet = GetStaySetByOpenVisits(this.DalStay2, this._CheckDate, this._VisitType);
//                    staySet.Sort(new CompareStay());

//                    MapStaySet(visitSet2, staySet);
//                }

//                if (DalDoctorAttendation2 != null)
//                {
//                    doctorAttendationSet = GetDoctorAttendationSetByOpenVisits(this.DalDoctorAttendation2, this._CheckDate, this._VisitType);
//                    doctorAttendationSet.Sort(new CompareDoctorAttendation());

//                    MapDoctorAttendationSet(visitSet2, doctorAttendationSet);
//                }

//                if (DalVisitInsurance2 != null)
//                {
//                    visitInsuranceSet = GetVisitInsuranceSetByOpenVisits(this.DalVisitInsurance2, this._CheckDate, this._VisitType);
//                    visitInsuranceSet.Sort(new CompareVisitInsurance());

//                    MapVisitInsuranceSet(visitSet2, visitInsuranceSet);
//                }

//                if (DalLeave2 != null)
//                {
//                    leaveSet = GetLeaveSetByOpenVisits(this.DalLeave2, this._CheckDate, this._VisitType);
//                    leaveSet.Sort(new CompareLeave());

//                    MapLeaveSet(visitSet2, leaveSet);
//                }
//            }
//        }

	
//        #endregion

//        #region Mapping Methods

//        private void MapAdmissionSet(bVisitSet visitSet, bAdmissionSet admissionSet)
//        {
//            if (visitSet == null || admissionSet == null)
//                return;

//            Type type = typeof(bVisit);
//            FieldInfo fieldInfo = type.GetField("_AdmissionSet", 
//                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//            FieldInfo fieldInfo2 = type.GetField("_AdmissionSetTicks", 
//                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//            FieldInfo fieldInfo3 = type.GetField("TicksTimeOut", 
//                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
//            bool bFound = false;

//            foreach (bVisit visit in visitSet)
//            {
//                fieldInfo.SetValue(visit, new bAdmissionSet());
//                if (fieldInfo2 != null) 
//                    fieldInfo2.SetValue(visit, DateTime.Now.Ticks);
//                if (fieldInfo3 != null) 
//                    fieldInfo3.SetValue(visit, 6000000000);  //10 min

//                bFound = false;

//                for (int i = 0; i < admissionSet.Count; i++)
//                {
//#if !NHIBER
//                    if ((admissionSet[i] as bAdmission).VisitNumber.Equals(visit.Number))
//#else
//                    if ((admissionSet[i] as bAdmission).Visit.Equals(visit))
//#endif
//                    {
//                        visit.AdmissionSet.Add(admissionSet[i]);
//                        admissionSet.RemoveAt(i);
//                        bFound = true;
//                        i--;
//                    }
//                    else
//                    {
//                        if (bFound)
//                            break;
//                    }
//                }
//            }

//            if (admissionSet.Count == 0)
//                return;

//            StringBuilder sb = new StringBuilder();
//            foreach (bAdmission admission in admissionSet)
//            {
//                sb.Append(admission.VisitNumber);
//                sb.Append(" - ");
//                sb.Append(admission.FromDate.ToString("yyyy/MM/dd HH:mm"));
//                sb.Append("; ");
//            }
//            sb.Remove(sb.Length - 2, 2); //remove the last ;
//            AppendText(string.Format("{0} admissions not mapped: ({1}){2}{3}", admissionSet.Count, sb.ToString(), Environment.NewLine, Environment.NewLine));
//        }

//        private void MapStaySet(bVisitSet visitSet, bStaySet staySet)
//        {
//            if (visitSet == null || staySet == null)
//                return;

//            Type type = typeof(bVisit);
//            FieldInfo fieldInfo = type.GetField("_StaySet", 
//                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//            FieldInfo fieldInfo2 = type.GetField("_StaySetTicks", 
//                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//            FieldInfo fieldInfo3 = type.GetField("TicksTimeOut", 
//                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
//            bool bFound = false;

//            foreach (bVisit visit in visitSet)
//            {
//                fieldInfo.SetValue(visit, new bStaySet());
//                if (fieldInfo2 != null) 
//                    fieldInfo2.SetValue(visit, DateTime.Now.Ticks);
//                if (fieldInfo3 != null) 
//                    fieldInfo3.SetValue(visit, 6000000000);  //10 min

//                bFound = false;

//                for (int i = 0; i < staySet.Count; i++)
//                {
//#if !NHIBER
//                    if ((staySet[i] as bStay).VisitNumber.Equals(visit.Number))
//#else
//                    if ((staySet[i] as bStay).Visit.Equals(visit))
//#endif
//                    {
//                        visit.StaySet.Add(staySet[i]);
//                        staySet.RemoveAt(i);
//                        bFound = true;
//                        i--;
//                    }
//                    else
//                    {
//                        if (bFound)
//                            break;
//                    }
//                }
//            }

//            if (staySet.Count == 0)
//                return;

//            StringBuilder sb = new StringBuilder();
//            foreach (bStay stay in staySet)
//            {
//                sb.Append(stay.VisitNumber);
//                sb.Append(" - ");
//                sb.Append(stay.FromDate.ToString("yyyy/MM/dd HH:mm"));
//                sb.Append("; ");
//            }
//            sb.Remove(sb.Length - 2, 2); //remove the last ;
//            AppendText(string.Format("{0} stays not mapped: ({1}){2}{3}", staySet.Count, sb.ToString(), Environment.NewLine, Environment.NewLine));
//        }

//        private void MapDoctorAttendationSet(bVisitSet visitSet, bDoctorAttendationSet doctorAttendationSet)
//        {
//            if (visitSet == null || doctorAttendationSet == null)
//                return;

//            Type type = typeof(bVisit);
//            FieldInfo fieldInfo = type.GetField("_DoctorAttendationSet", 
//                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//            FieldInfo fieldInfo2 = type.GetField("_DoctorAttendationSetTicks", 
//                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//            FieldInfo fieldInfo3 = type.GetField("TicksTimeOut", 
//                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
//            bool bFound = false;

//            foreach (bVisit visit in visitSet)
//            {
//                fieldInfo.SetValue(visit, new bDoctorAttendationSet());
//                if (fieldInfo2 != null) 
//                    fieldInfo2.SetValue(visit, DateTime.Now.Ticks);
//                if (fieldInfo3 != null) 
//                    fieldInfo3.SetValue(visit, 6000000000);  //10 min

//                bFound = false;

//                for (int i = 0; i < doctorAttendationSet.Count; i++)
//                {
//#if !NHIBER
//                    if ((doctorAttendationSet[i] as bDoctorAttendation).VisitNumber.Equals(visit.Number))
//#else
//                    if ((doctorAttendationSet[i] as bDoctorAttendation).Visit.Equals(visit))
//#endif
//                    {
//                        visit.DoctorAttendationSet.Add(doctorAttendationSet[i]);
//                        doctorAttendationSet.RemoveAt(i);
//                        bFound = true;
//                        i--;
//                    }
//                    else
//                    {
//                        if (bFound)
//                            break;
//                    }
//                }
//            }

//            if (doctorAttendationSet.Count == 0)
//                return;

//            StringBuilder sb = new StringBuilder();
//            foreach (bDoctorAttendation doctorAttendation in doctorAttendationSet)
//            {
//                sb.Append(doctorAttendation.VisitNumber);
//                sb.Append(" - ");
//                sb.Append(doctorAttendation.FromDate.ToString("yyyy/MM/dd HH:mm"));
//                sb.Append("; ");
//            }
//            sb.Remove(sb.Length - 2, 2); //remove the last ;
//            AppendText(string.Format("{0} doctorAttendations not mapped: ({1}){2}{3}", doctorAttendationSet.Count, sb.ToString(), Environment.NewLine, Environment.NewLine));
//        }	

//        private void MapVisitInsuranceSet(bVisitSet visitSet, bVisitInsuranceSet visitInsuranceSet)
//        {
//            if (visitSet == null || visitInsuranceSet == null)
//                return;

//            Type type = typeof(bVisit);
//            FieldInfo fieldInfo = type.GetField("_VisitInsuranceSet", 
//                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//            FieldInfo fieldInfo2 = type.GetField("_VisitInsuranceSetTicks", 
//                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//            FieldInfo fieldInfo3 = type.GetField("TicksTimeOut", 
//                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
//            bool bFound = false;

//            foreach (bVisit visit in visitSet)
//            {
//                fieldInfo.SetValue(visit, new bVisitInsuranceSet());
//                if (fieldInfo2 != null) 
//                    fieldInfo2.SetValue(visit, DateTime.Now.Ticks);
//                if (fieldInfo3 != null) 
//                    fieldInfo3.SetValue(visit, 6000000000);  //10 min

//                bFound = false;

//                for (int i = 0; i < visitInsuranceSet.Count; i++)
//                {
//#if !NHIBER
//                    if ((visitInsuranceSet[i] as bVisitInsurance).VisitNumber.Equals(visit.Number))
//#else
//                    if ((visitInsuranceSet[i] as bVisitInsurance).Visit.Equals(visit))
//#endif
//                    {
//                        visit.VisitInsuranceSet.Add(visitInsuranceSet[i]);
//                        visitInsuranceSet.RemoveAt(i);
//                        bFound = true;
//                        i--;
//                    }
//                    else
//                    {
//                        if (bFound)
//                            break;
//                    }
//                }
//            }

//            if (visitInsuranceSet.Count == 0)
//                return;

//            StringBuilder sb = new StringBuilder();
//            foreach (bVisitInsurance visitInsurance in visitInsuranceSet)
//            {
//                sb.Append(visitInsurance.VisitNumber);
//                sb.Append(" - ");
//                sb.Append(visitInsurance.FromDate.ToString("yyyy/MM/dd HH:mm"));
//                sb.Append("; ");
//            }
//            sb.Remove(sb.Length - 2, 2); //remove the last ;
//            AppendText(string.Format("{0} visitInsurances not mapped: ({1}){2}{3}", visitInsuranceSet.Count, sb.ToString(), Environment.NewLine, Environment.NewLine));
//        }	
				
//        private void MapLeaveSet(bVisitSet visitSet, bLeaveSet leaveSet)
//        {
//            if (visitSet == null || leaveSet == null)
//                return;

//            Type type = typeof(bVisit);
//            FieldInfo fieldInfo = type.GetField("_LeaveSet", 
//                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//            FieldInfo fieldInfo2 = type.GetField("_LeaveSetTicks", 
//                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//            FieldInfo fieldInfo3 = type.GetField("TicksTimeOut", 
//                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
//            bool bFound = false;

//            foreach (bVisit visit in visitSet)
//            {
//                fieldInfo.SetValue(visit, new bLeaveSet());
//                if (fieldInfo2 != null) 
//                    fieldInfo2.SetValue(visit, DateTime.Now.Ticks);
//                if (fieldInfo3 != null) 
//                    fieldInfo3.SetValue(visit, 6000000000);  //10 min

//                bFound = false;

//                for (int i = 0; i < leaveSet.Count; i++)
//                {
//#if !NHIBER
//                    if ((leaveSet[i] as bLeave).VisitNumber.Equals(visit.Number))
//#else
//                    if ((leaveSet[i] as bLeave).Visit.Equals(visit))
//#endif
//                    {
//                        visit.LeaveSet.Add(leaveSet[i]);
//                        leaveSet.RemoveAt(i);
//                        bFound = true;
//                        i--;
//                    }
//                    else
//                    {
//                        if (bFound)
//                            break;
//                    }
//                }
//            }

//            if (leaveSet.Count == 0)
//                return;

//            StringBuilder sb = new StringBuilder();
//            foreach (bLeave leave in leaveSet)
//            {
//                sb.Append(leave.VisitNumber);
//                sb.Append(" - ");
//                sb.Append(leave.FromDate.ToString("yyyy/MM/dd HH:mm"));
//                sb.Append("; ");
//            }
//            sb.Remove(sb.Length - 2, 2); //remove the last ;
//            AppendText(string.Format("{0} leaves not mapped: ({1}){2}{3}", leaveSet.Count, sb.ToString(), Environment.NewLine, Environment.NewLine));
//        }	
		
		
//        #endregion

//        #region Dal Methods

//        private bVisitSet GetOpenVisitSetByDateAndVisitType(IDalVisit dalVisit, DateTime date, enumVisitType visitType)
//        {
//            bVisitSet visitSet = null;
//            DateTime dt = DateTime.Now;

//            try 
//            {
//                visitSet = dalVisit.GetOpenVisitSetByDateAndVisitType(date, visitType);
//            }
//            catch(Exception ex)
//            {
//                AppendText(ex.ToString());
//                visitSet = new bVisitSet();
//            }
//            AppendText(string.Format("Queried {0}{1}", dalVisit.GetType().FullName, Environment.NewLine));
//            AppendText(string.Format("Elapsed time: {0}{1}", new TimeSpan(DateTime.Now.Ticks - dt.Ticks).ToString(), Environment.NewLine));
//            AppendText(string.Format("Number of visits: {0}{1}{2}", visitSet.Count, Environment.NewLine, Environment.NewLine));
//            return visitSet;
//        }

//        private bAdmissionSet GetAdmissionSetByOpenVisits(IDalAdmission dalAdmission, DateTime date, enumVisitType visitType)
//        {
//            bAdmissionSet admissionSet = null;
//            DateTime dt = DateTime.Now;

//            try 
//            {
//                admissionSet = dalAdmission.GetAdmissionSetByOpenVisits(date, visitType);
//            }
//            catch(Exception ex)
//            {
//                AppendText(ex.ToString());
//                admissionSet = new bAdmissionSet();
//            }
//            AppendText(string.Format("Queried {0}{1}", dalAdmission.GetType().FullName, Environment.NewLine));
//            AppendText(string.Format("Elapsed time: {0}{1}", new TimeSpan(DateTime.Now.Ticks - dt.Ticks).ToString(), Environment.NewLine));
//            AppendText(string.Format("Number of admissions: {0}{1}{2}", admissionSet.Count, Environment.NewLine, Environment.NewLine));
//            return admissionSet;
//        }

//        private bStaySet GetStaySetByOpenVisits(IDalStay dalStay, DateTime date, enumVisitType visitType)
//        {
//            bStaySet staySet = null;
//            DateTime dt = DateTime.Now;

//            try 
//            {
//                staySet = dalStay.GetStaySetByOpenVisits(date, visitType);
//            }
//            catch(Exception ex)
//            {
//                AppendText(ex.ToString());
//                staySet = new bStaySet();
//            }
//            AppendText(string.Format("Queried {0}{1}", dalStay.GetType().FullName, Environment.NewLine));
//            AppendText(string.Format("Elapsed time: {0}{1}", new TimeSpan(DateTime.Now.Ticks - dt.Ticks).ToString(), Environment.NewLine));
//            AppendText(string.Format("Number of stays: {0}{1}{2}", staySet.Count, Environment.NewLine, Environment.NewLine));
//            return staySet;
//        }

//        private bDoctorAttendationSet GetDoctorAttendationSetByOpenVisits(IDalDoctorAttendation dalDoctorAttendation, DateTime date, enumVisitType visitType)
//        {
//            bDoctorAttendationSet doctorAttendationSet = null;
//            DateTime dt = DateTime.Now;

//            try 
//            {
//                doctorAttendationSet = dalDoctorAttendation.GetDoctorAttendationSetByOpenVisits(date, visitType);
//            }
//            catch(Exception ex)
//            {
//                AppendText(ex.ToString());
//                doctorAttendationSet = new bDoctorAttendationSet();
//            }
//            AppendText(string.Format("Queried {0}{1}", dalDoctorAttendation.GetType().FullName, Environment.NewLine));
//            AppendText(string.Format("Elapsed time: {0}{1}", new TimeSpan(DateTime.Now.Ticks - dt.Ticks).ToString(), Environment.NewLine));
//            AppendText(string.Format("Number of doctorAttendations: {0}{1}{2}", doctorAttendationSet.Count, Environment.NewLine, Environment.NewLine));
//            return doctorAttendationSet;
//        }

//        private bVisitInsuranceSet GetVisitInsuranceSetByOpenVisits(IDalVisitInsurance dalVisitInsurance, DateTime date, enumVisitType visitType)
//        {
//            bVisitInsuranceSet visitInsuranceSet = null;
//            DateTime dt = DateTime.Now;

//            try 
//            {
//                visitInsuranceSet = dalVisitInsurance.GetVisitInsuranceSetByOpenVisits(date, visitType);
//            }
//            catch(Exception ex)
//            {
//                AppendText(ex.ToString());
//                visitInsuranceSet = new bVisitInsuranceSet();
//            }
//            AppendText(string.Format("Queried {0}{1}", dalVisitInsurance.GetType().FullName, Environment.NewLine));
//            AppendText(string.Format("Elapsed time: {0}{1}", new TimeSpan(DateTime.Now.Ticks - dt.Ticks).ToString(), Environment.NewLine));
//            AppendText(string.Format("Number of visitInsurances: {0}{1}{2}", visitInsuranceSet.Count, Environment.NewLine, Environment.NewLine));
//            return visitInsuranceSet;
//        }

//        private bLeaveSet GetLeaveSetByOpenVisits(IDalLeave dalLeave, DateTime date, enumVisitType visitType)
//        {
//            bLeaveSet leaveSet = null;
//            DateTime dt = DateTime.Now;

//            try 
//            {
//                leaveSet = dalLeave.GetLeaveSetByOpenVisits(date, visitType);
//            }
//            catch(Exception ex)
//            {
//                AppendText(ex.ToString());
//                leaveSet = new bLeaveSet();
//            }
//            AppendText(string.Format("Queried {0}{1}", dalLeave.GetType().FullName, Environment.NewLine));
//            AppendText(string.Format("Elapsed time: {0}{1}", new TimeSpan(DateTime.Now.Ticks - dt.Ticks).ToString(), Environment.NewLine));
//            AppendText(string.Format("Number of leaves: {0}{1}{2}", leaveSet.Count, Environment.NewLine, Environment.NewLine));
//            return leaveSet;
//        }		
		
//        private bVisitSet CompareVisitSets(bVisitSet visitSet1, bVisitSet visitSet2)
//        {
//            if (visitSet1 == null || visitSet2 == null)
//                return null;

//            IComparer<bVisit> comparer = null;
//            int iIndex = 0;
//            string compareResult = null;
//            //int iDifferentEntities = 0;
//            bool bDifferent = false;
//            bVisitSet differentVisitSet = new bVisitSet();

//            AppendText(Environment.NewLine);
//            AppendText("COMPARING RESULTS:");
//            AppendText(Environment.NewLine);
//            AppendText(Environment.NewLine);

//            comparer = new CompareVisitByNumber();
//            visitSet1.Sort(new CompareVisitByAdmissionDate()); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//            visitSet2.Sort(comparer);
//            foreach (bVisit visit in visitSet1)
//            {
//                bDifferent = false;
//                iIndex = visitSet2.BinarySearch(visit, comparer);
//                if ( iIndex < 0 )
//                {
//                    AppendText(string.Format("Visit with visitnumber {0} for patient {1} exists in {2} but not in {3}{4}", visit.Number, visit.PatientId, DalVisit1.GetType().FullName, DalVisit2.GetType().FullName, Environment.NewLine));
//                    bDifferent = true;
//                }
//                else
//                {
//                    compareResult = CompareEntities(visit, (bEntityBase)visitSet2[iIndex], "\t");
//                    if(compareResult != null && compareResult.Length > 0)
//                    {
//                        AppendText(string.Format("Visitnumber: {0}, patientid: {1}{2}", visit.Number, visit.PatientId, Environment.NewLine));
//                        AppendText(string.Format(compareResult + "{0}", Environment.NewLine));
//                        bDifferent = true;
//                    }

//                    if (DalAdmission1 != null && DalAdmission2 != null)
//                    {
//                        //CompareAdmissionSets(DalAdmission1.GetAdmissionsByVisit(visit), DalAdmission2.GetAdmissionsByVisit((bVisit)visitSet2[iIndex]));
//                        if(!CompareAdmissionSets(visit.AdmissionSet, ((bVisit)visitSet2[iIndex]).AdmissionSet))
//                            bDifferent = true;
//                    }

//                    if (DalStay1 != null && DalStay2 != null)
//                    {
//                        //CompareStaySets(DalStay1.GetStaySetByVisit(visit), DalStay2.GetStaySetByVisit((bVisit)visitSet2[iIndex]));
//                        if(!CompareStaySets(visit.StaySet, ((bVisit)visitSet2[iIndex]).StaySet))
//                            bDifferent = true;
//                    }

//                    if (DalDoctorAttendation1 != null && DalDoctorAttendation2 != null)
//                    {
//                        //CompareDoctorAttendationSets(DalDoctorAttendation1.GetDoctorAttendationSetByVisit(visit), DalDoctorAttendation2.GetDoctorAttendationSetByVisit((bVisit)visitSet2[iIndex]));
//                        if(!CompareDoctorAttendationSets(visit.DoctorAttendationSet, ((bVisit)visitSet2[iIndex]).DoctorAttendationSet))
//                            bDifferent = true;
//                    }

//                    if (DalVisitInsurance1 != null && DalVisitInsurance2 != null)
//                    {
//                        //CompareVisitInsuranceSets(DalVisitInsurance1.GetVisitInsuranceSetByVisit(visit), DalVisitInsurance2.GetVisitInsuranceSetByVisit((bVisit)visitSet2[iIndex]));
//                        if(!CompareVisitInsuranceSets(visit.VisitInsuranceSet, ((bVisit)visitSet2[iIndex]).VisitInsuranceSet))
//                            bDifferent = true;
//                    }

//                    visitSet2.RemoveAt(iIndex);
//                }
//                if(bDifferent)
//                    differentVisitSet.Add(visit);
//            }
//            //AppendText(Environment.NewLine});
//            foreach (bVisit visit in visitSet2)
//            {
//                AppendText(string.Format("Visit with visitnumber {0} for patient {1} exists in {2} but not in {3}{4}", visit.Number, visit.PatientId, DalVisit2.GetType().FullName, DalVisit1.GetType().FullName, Environment.NewLine));
//            }

//            if (differentVisitSet.Count > 0)
//            {
//                AppendText(string.Format("{0}{1} of the {2} visits are different!{3}", Environment.NewLine, differentVisitSet.Count, visitSet1.Count, Environment.NewLine));
//            }

//            return differentVisitSet;
//        }

		
//        #endregion

//        #region Compare Methods
		
//        private bool CompareAdmissionSets(bAdmissionSet admissionSet1, bAdmissionSet admissionSet2)
//        {
//            if (admissionSet1 == null || admissionSet2 == null)
//                return true;

//            IComparer<bAdmission> comparer = null;
//            int iIndex = 0;
//            string compareResult = null;
//            bool bResult = true;
	
//            comparer = new CompareAdmissionByFromDate();
//            admissionSet1.Sort(comparer);
//            admissionSet2.Sort(comparer);
//            foreach (bAdmission admission in admissionSet1)
//            {
//                iIndex = admissionSet2.BinarySearch(admission, comparer);
//                if ( iIndex < 0 )
//                {
//                    AppendText(string.Format("\tAdmission with visitnumber {0} and fromdate {1} exists in {2} but not in {3}{4}", admission.VisitNumber, admission.FromDate, DalAdmission1.GetType().FullName, DalAdmission2.GetType().FullName, Environment.NewLine));
//                    bResult = false;
//                }
//                else
//                {
//                    compareResult = CompareEntities(admission, (bEntityBase)admissionSet2[iIndex], "\t\t");
//                    if(compareResult != null && compareResult.Length > 0)
//                    {
//                        AppendText(string.Format("\t\t" + "Admission visitnumber: {0}, fromdate: {1}{2}", admission.VisitNumber, admission.FromDate, Environment.NewLine));
//                        AppendText(string.Format(compareResult + "{0}", Environment.NewLine));
//                        bResult = false;
//                    }

//                    admissionSet2.RemoveAt(iIndex);
//                }
//            }
//            //AppendText(Environment.NewLine});
//            foreach (bAdmission admission in admissionSet2)
//            {
//                AppendText(string.Format("Admission with visitnumber {0} and fromdate {1} exists in {2} but not in {3}{4}", admission.VisitNumber, admission.FromDate, DalAdmission2.GetType().FullName, DalAdmission1.GetType().FullName, Environment.NewLine));
//                bResult = false;
//            }

//            return bResult;
//        }

//        private bool CompareStaySets(bStaySet staySet1, bStaySet staySet2)
//        {
//            if (staySet1 == null || staySet2 == null)
//                return true;

//            IComparer<bStay> comparer = null;
//            int iIndex = 0;
//            string compareResult = null;
//            bool bResult = true;
	
//            comparer = new CompareStayByFromDate();
//            staySet1.Sort(comparer);
//            staySet2.Sort(comparer);
//            foreach (bStay stay in staySet1)
//            {
//                iIndex = staySet2.BinarySearch(stay, comparer);
//                if ( iIndex < 0 )
//                {
//                    AppendText(string.Format("\tStay with visitnumber {0} and fromdate {1} exists in {2} but not in {3}{4}", stay.VisitNumber, stay.FromDate, DalStay1.GetType().FullName, DalStay2.GetType().FullName, Environment.NewLine));
//                    bResult = false;
//                }
//                else
//                {
//                    compareResult = CompareEntities(stay, (bEntityBase)staySet2[iIndex], "\t\t");
//                    if(compareResult != null && compareResult.Length > 0)
//                    {
//                        AppendText(string.Format("\t\t" + "Stay visitnumber: {0}, fromdate: {1}{2}", stay.VisitNumber, stay.FromDate, Environment.NewLine));
//                        AppendText(string.Format(compareResult + "{0}", Environment.NewLine));
//                        bResult = false;
//                    }

//                    staySet2.RemoveAt(iIndex);
//                }
//            }
//            //AppendText(Environment.NewLine});
//            foreach (bStay stay in staySet2)
//            {
//                AppendText(string.Format("Stay with visitnumber {0} and fromdate {1} exists in {2} but not in {3}{4}", stay.VisitNumber, stay.FromDate, DalStay2.GetType().FullName, DalStay1.GetType().FullName, Environment.NewLine));
//                bResult = false;
//            }

//            return bResult;
//        }

//        private bool CompareDoctorAttendationSets(bDoctorAttendationSet doctorAttendationSet1, bDoctorAttendationSet doctorAttendationSet2)
//        {
//            if (doctorAttendationSet1 == null || doctorAttendationSet2 == null)
//                return true;

//            IComparer<bDoctorAttendation> comparer = null;
//            int iIndex = 0;
//            string compareResult = null;
//            bool bResult = true;
	
//            comparer = new CompareDoctorAttendationByFromDate();
//            doctorAttendationSet1.Sort(comparer);
//            doctorAttendationSet2.Sort(comparer);
//            foreach (bDoctorAttendation doctorAttendation in doctorAttendationSet1)
//            {
//                iIndex = doctorAttendationSet2.BinarySearch(doctorAttendation, comparer);
//                if ( iIndex < 0 )
//                {
//                    AppendText(string.Format("\tDoctorAttendation with visitnumber {0} and fromdate {1} exists in {2} but not in {3}{4}", doctorAttendation.VisitNumber, doctorAttendation.FromDate, DalDoctorAttendation1.GetType().FullName, DalDoctorAttendation2.GetType().FullName, Environment.NewLine));
//                    bResult = false;
//                }
//                else
//                {
//                    compareResult = CompareEntities(doctorAttendation, (bEntityBase)doctorAttendationSet2[iIndex], "\t\t");
//                    if(compareResult != null && compareResult.Length > 0)
//                    {
//                        AppendText(string.Format("\t\t" + "DoctorAttendation visitnumber: {0}, fromdate: {1}{2}", doctorAttendation.VisitNumber, doctorAttendation.FromDate, Environment.NewLine));
//                        AppendText(string.Format(compareResult + "{0}", Environment.NewLine));
//                        bResult = false;
//                    }

//                    doctorAttendationSet2.RemoveAt(iIndex);
//                }
//            }
//            //AppendText(Environment.NewLine});
//            foreach (bDoctorAttendation doctorAttendation in doctorAttendationSet2)
//            {
//                AppendText(string.Format("DoctorAttendation with visitnumber {0} and fromdate {1} exists in {2} but not in {3}{4}", doctorAttendation.VisitNumber, doctorAttendation.FromDate, DalDoctorAttendation2.GetType().FullName, DalDoctorAttendation1.GetType().FullName, Environment.NewLine));
//                bResult = false;
//            }

//            return bResult;
//        }

//        private bool CompareVisitInsuranceSets(bVisitInsuranceSet visitInsuranceSet1, bVisitInsuranceSet visitInsuranceSet2)
//        {
//            if (visitInsuranceSet1 == null || visitInsuranceSet2 == null)
//                return true;

//            IComparer<bVisitInsurance> comparer = null;
//            int iIndex = 0;
//            string compareResult = null;
//            bool bResult = true;
	
//            comparer = new CompareVisitInsuranceByFromDate();
//            visitInsuranceSet1.Sort(comparer);
//            visitInsuranceSet2.Sort(comparer);
//            foreach (bVisitInsurance visitInsurance in visitInsuranceSet1)
//            {
//                iIndex = visitInsuranceSet2.BinarySearch(visitInsurance, comparer);
//                if ( iIndex < 0 )
//                {
//                    AppendText(string.Format("\tVisitInsurance with visitnumber {0} and fromdate {1} exists in {2} but not in {3}{4}", visitInsurance.VisitNumber, visitInsurance.FromDate, DalVisitInsurance1.GetType().FullName, DalVisitInsurance2.GetType().FullName, Environment.NewLine));
//                    bResult = false;
//                }
//                else
//                {
//                    compareResult = CompareEntities(visitInsurance, (bEntityBase)visitInsuranceSet2[iIndex], "\t\t");
//                    if(compareResult != null && compareResult.Length > 0)
//                    {
//                        AppendText(string.Format("\t\t" + "VisitInsurance visitnumber: {0}, fromdate: {1}{2}", visitInsurance.VisitNumber, visitInsurance.FromDate, Environment.NewLine));
//                        AppendText(string.Format(compareResult + "{0}", Environment.NewLine));
//                        bResult = false;
//                    }

//                    visitInsuranceSet2.RemoveAt(iIndex);
//                }
//            }
//            //AppendText(Environment.NewLine});
//            foreach (bVisitInsurance visitInsurance in visitInsuranceSet2)
//            {
//                AppendText(string.Format("VisitInsurance with visitnumber {0} and fromdate {1} exists in {2} but not in {3}{4}", visitInsurance.VisitNumber, visitInsurance.FromDate, DalVisitInsurance2.GetType().FullName, DalVisitInsurance1.GetType().FullName, Environment.NewLine));
//                bResult = false;
//            }

//            return bResult;
//        }

//        private bool CompareLeaveSets(bLeaveSet leaveSet1, bLeaveSet leaveSet2)
//        {
//            if (leaveSet1 == null || leaveSet2 == null)
//                return true;

//            IComparer<bLeave> comparer = null;
//            int iIndex = 0;
//            string compareResult = null;
//            bool bResult = true;
	
//            comparer = new CompareLeaveByFromDate();
//            leaveSet1.Sort(comparer);
//            leaveSet2.Sort(comparer);
//            foreach (bLeave leave in leaveSet1)
//            {
//                iIndex = leaveSet2.BinarySearch(leave, comparer);
//                if ( iIndex < 0 )
//                {
//                    AppendText(string.Format("\tLeave with visitnumber {0} and fromdate {1} exists in {2} but not in {3}{4}", leave.VisitNumber, leave.FromDate, DalLeave1.GetType().FullName, DalLeave2.GetType().FullName, Environment.NewLine));
//                    bResult = false;
//                }
//                else
//                {
//                    compareResult = CompareEntities(leave, (bEntityBase)leaveSet2[iIndex], "\t\t");
//                    if(compareResult != null && compareResult.Length > 0)
//                    {
//                        AppendText(string.Format("\t\t" + "Leave visitnumber: {0}, fromdate: {1}{2}", leave.VisitNumber, leave.FromDate, Environment.NewLine));
//                        AppendText(string.Format(compareResult + "{0}", Environment.NewLine));
//                        bResult = false;
//                    }

//                    leaveSet2.RemoveAt(iIndex);
//                }
//            }
//            //AppendText(Environment.NewLine});
//            foreach (bLeave leave in leaveSet2)
//            {
//                AppendText(string.Format("Leave with visitnumber {0} and fromdate {1} exists in {2} but not in {3}{4}", leave.VisitNumber, leave.FromDate, DalLeave2.GetType().FullName, DalLeave1.GetType().FullName, Environment.NewLine));
//                bResult = false;
//            }

//            return bResult;
//        }

//        private FieldInfo[] GetFields(Type t)
//        {
//            List<FieldInfo> fields = null;

//            fields = new List<FieldInfo>();

//            foreach (FieldInfo field in t.GetFields(
//                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
//            {
//                if (!field.IsNotSerialized)
//                    fields.Add(field);
//            }

//            return fields.ToArray();
//        }

//        private string CompareEntities(bEntityBase entity1, bEntityBase entity2, string prefix)
//        {
//            object obj1 = null;
//            object obj2 = null;
//            StringBuilder sb = null;

//            sb = new StringBuilder();
//            //if (Fields == null)
//                Fields = GetFields(entity1.GetType());

//            foreach (FieldInfo field in Fields)
//            {
//                if (field.Name.Equals("_PkId"))
//                    continue;

//                obj1 = ModifyObjectData(field.GetValue(entity1), field.Name, entity1.GetType());
//                obj2 = ModifyObjectData(field.GetValue(entity2), field.Name, entity2.GetType());

//                if (obj1 != null && !obj1.Equals(obj2))
//                {
//                    string s1 = obj1.ToString();
//                    string s2 = obj2.Equals(null) ? string.Empty : obj2.ToString();
//                    sb.AppendFormat("{0}Different {1}: '{2}' != '{3}'{4}", prefix, field.Name, 
//                        s1.Equals("\0") ? string.Empty : s1, s2.Equals("\0") ? string.Empty : s2,
//                        Environment.NewLine);
//                }
//                else if (obj2 != null && !obj2.Equals(obj1))
//                {
//                    string s1 = obj1.Equals(null) ? string.Empty : obj1.ToString();
//                    string s2 = obj2.ToString();
//                    sb.AppendFormat("{0}Different {1}: '{2}' != '{3}'{4}", prefix, field.Name, 
//                        s1.Equals("\0") ? string.Empty : s1, s2.Equals("\0") ? string.Empty : s2,
//                        Environment.NewLine);
//                }
//            }

//            return sb.ToString();
//        }

//        private object ModifyObjectData(object obj, string fieldName, Type tEntity)
//        {
//            if (obj == null)
//                return obj;

//            if (obj.GetType().Equals(typeof(DateTime)))
//            {
//                if ((DateTime)obj > Constants.MaxDate)
//                    obj = Constants.MaxDate;
//                else if (obj.Equals(new DateTime(2099, 12, 31, 23, 59, 0)))
//                    obj = Constants.MaxDate;
//                else if (((DateTime)obj).Second != 0)
//                    obj = new DateTime(((DateTime)obj).Year, ((DateTime)obj).Month, ((DateTime)obj).Day, 
//                            ((DateTime)obj).Hour, ((DateTime)obj).Minute, 0);
//            }

//            if (tEntity.Equals(typeof(bVisitInsurance)) && fieldName.Equals("_SerialNumber"))
//            {
//                obj = 0;
//            }

//            if ((this.DalVisit1.GetType().Equals(typeof(HHR.Zis.Data.AgfaImplementation.DalVisit)) 
//                || this.DalVisit2.GetType().Equals(typeof(HHR.Zis.Data.AgfaImplementation.DalVisit))) &&
//                tEntity.Equals(typeof(bVisit)) && fieldName.Equals("_ApplicationCode"))
//            {
//                obj = string.Empty;
//            }

////			if ((this.DalVisit1.GetType().Equals(typeof(HHR.ZIS.Data.XTensoImplementation.DalVisit)) 
////				|| this.DalVisit2.GetType().Equals(typeof(HHR.ZIS.Data.XTensoImplementation.DalVisit))) &&
////				tEntity.Equals(typeof(bStay)) && fieldName.Equals("_ToDate"))
////			{
////				obj = string.Empty;
////			}

//            if ((this.DalVisit1.GetType().Equals(typeof(HHR.ZIS.Data.SqlImplementation.DalVisit))
//                || this.DalVisit2.GetType().Equals(typeof(HHR.ZIS.Data.SqlImplementation.DalVisit))) &&
//                tEntity.Equals(typeof(bVisit)) && fieldName.Equals("_ExpectedDischargeDate"))
//            {
//                obj = Constants.MaxDate;
//            }

//            if ((this.DalVisit1.GetType().Equals(typeof(HHR.ZIS.Data.XTensoImplementation.DalVisit))
//                || this.DalVisit2.GetType().Equals(typeof(HHR.ZIS.Data.XTensoImplementation.DalVisit))) &&
//                tEntity.Equals(typeof(bVisit)) && fieldName.Equals("_ExpectedDischargeDate"))
//            {
//                obj = Constants.MaxDate;
//            }

//            if ((this.DalVisit1.GetType().Equals(typeof(HHR.ZIS.Data.XTensoImplementation.DalVisit)) 
//                || this.DalVisit2.GetType().Equals(typeof(HHR.ZIS.Data.XTensoImplementation.DalVisit))) &&
//                tEntity.Equals(typeof(bVisit)) && fieldName.Equals("_PatientExternalId"))
//            {
//                obj = string.Empty;
//            }

//            if (tEntity.Equals(typeof(bVisit)) && fieldName.Equals("_MotherVisitNumber"))
//            {
//                obj = string.Empty;
//            }

//            return obj;
//        }


//        #endregion

//        #region Export-Correct Methods

//        private void ExportDifferentVisitSet(bVisitSet visitSet)
//        {
//            if (visitSet.Count == 0)
//                return;

//            AppendText(string.Format("Exporting Different VisitSet!{0}", Environment.NewLine));
//            StringBuilder sbVisitNumbers = new StringBuilder();
//            foreach (bVisit visit in visitSet)
//            {
//                sbVisitNumbers.Append(visit.Number);
//                sbVisitNumbers.Append(", ");
//            }
//            sbVisitNumbers.Remove(sbVisitNumbers.Length - 2, 2); //remove the last ,

//            HHR.Log.Business.Logic.bLogLogic.Log(visitSet.Count + " different visits", 
//                sbVisitNumbers.ToString());
//        }

//        private void CorrectDifferentVisitSet(bVisitSet visitSet)
//        {
//            if (visitSet.Count == 0)
//                return;

//            AppendText(string.Format("Correcting Different VisitSet!{0}", Environment.NewLine));
//            foreach (bVisit visit in visitSet)
//                ImportVisit(visit);
//        }

//        private void ImportVisit(bVisit visit)
//        {
//            AppendText(string.Format("Importing visit '{0}'{1}", visit.Number, Environment.NewLine));
			
//            DeleteVisit(bVisitLogic.GetVisitByNumber(visit.Number)); //DELETE
//            visit.Persist();

//            DalVisitInsurance1.GetVisitInsuranceSetByVisit(visit).Persist();
//            DalAdmission1.GetAdmissionsByVisit(visit).Persist();
//            DalStay1.GetStaySetByVisit(visit).Persist();
//            DalDoctorAttendation1.GetDoctorAttendationSetByVisit(visit).Persist();
//            DalLeave1.GetLeaveSetByVisit(visit).Persist();
//        }

//        private void DeleteVisit(bVisit visit)
//        {
//            if (visit == null || visit.Equals(bVisit.Empty))
//                return;

//            visit.AdmissionSet.Delete();
//            visit.AdmissionSet.Clear();
//            visit.StaySet.Delete();
//            visit.StaySet.Clear();
//            visit.DoctorAttendationSet.Delete();
//            visit.DoctorAttendationSet.Clear();
//            visit.VisitInsuranceSet.Delete();
//            visit.VisitInsuranceSet.Clear();
//            visit.LeaveSet.Delete();
//            visit.LeaveSet.Clear();
//            visit.Delete();
//        }


//        #endregion

        #endregion

		#region Delegates

        private delegate void SetCursorDelegate(Cursor cursor);
		private delegate void StringParameterDelegate (string value);
		public delegate void SearchPatientIdEventHandler(object sender, PatientIdEventArgs e);
		public delegate void SearchVisitNumberEventHandler(object sender, VisitNumberEventArgs e);

		#endregion

		#region Event Declaration

		public event SearchPatientIdEventHandler SearchPatientId;

		protected virtual void OnSearchPatientId(PatientIdEventArgs e)
		{
			if (SearchPatientId != null) 
			{
				// Invokes the delegates. 
				SearchPatientId(this, e);
			}
		}

		public event SearchVisitNumberEventHandler SearchVisitNumber;

		protected virtual void OnSearchVisitNumber(VisitNumberEventArgs e)
		{
			if (SearchVisitNumber != null) 
			{
				// Invokes the delegates. 
				SearchVisitNumber(this, e);
			}
		}

		#endregion
	}
}

