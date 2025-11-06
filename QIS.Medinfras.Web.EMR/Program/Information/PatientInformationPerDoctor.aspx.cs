using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;


namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientInformationPerDoctor : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.INFORMATION_PATIENT_PER_DOCTOR;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            fillYear();
            fillMonth();
            cboMonth.SelectedIndex = 0;
            cboYear.SelectedIndex = 0;
            hdnParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            BindGridView();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                {
                    BindGridView();
                    result = "refresh";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string year = cboYear.Text;
            int Month = Convert.ToInt32(cboMonth.Value);
            bool includeInpatient = chkIncludeInpatient.Checked;


            List<PatientDoctorPerMonth> lstPatientDoctorPerMonth = new List<PatientDoctorPerMonth>();

            string filterExpression = string.Format("(ParamedicID = {0} OR {1} IN (SELECT pt.ParamedicID FROM ParamedicTeam pt WITH(NOLOCK) WHERE pt.RegistrationID = vConsultVisit.RegistrationID AND pt.IsDeleted = 0)) AND DATEPART(YEAR, VisitDate) = {2} AND DATEPART(Month, VisitDate) = {3}", hdnParamedicID.Value, AppSession.UserLogin.ParamedicID, year, Month);
            List<vConsultVisit> lstConsultVisit = BusinessLayer.GetvConsultVisitList(filterExpression);

            string filterDept = string.Format("IsHasRegistration = 1 AND IsActive = 1");
            if (!includeInpatient)
            {
                filterDept += string.Format(" AND DepartmentID != 'INPATIENT'");
            }
            List<Department> lstDepartment = BusinessLayer.GetDepartmentList(filterDept);
            foreach (Department entityDepartment in lstDepartment)
            {
                PatientDoctorPerMonth entityPatientDoctor = new PatientDoctorPerMonth();
                entityPatientDoctor.DepartmentID = entityDepartment.DepartmentID;
                entityPatientDoctor.DepartmentName = entityDepartment.DepartmentName;
                IEnumerable<vConsultVisit> lstTempConsultVisit = lstConsultVisit.Where(t => t.DepartmentID == entityDepartment.DepartmentID);
                foreach (vConsultVisit entityConsultVisit in lstTempConsultVisit)
                {
                    if (entityConsultVisit.VisitDate.Day == 1)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl1_void++;
                        else entityPatientDoctor.Tgl1_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 2)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl2_void++;
                        else entityPatientDoctor.Tgl2_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 3)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl3_void++;
                        else entityPatientDoctor.Tgl3_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 4)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl4_void++;
                        else entityPatientDoctor.Tgl4_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 5)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl5_void++;
                        else entityPatientDoctor.Tgl5_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 6)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl6_void++;
                        else entityPatientDoctor.Tgl6_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 7)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl7_void++;
                        else entityPatientDoctor.Tgl7_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 8)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl8_void++;
                        else entityPatientDoctor.Tgl8_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 9)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl9_void++;
                        else entityPatientDoctor.Tgl9_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 10)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl10_void++;
                        else entityPatientDoctor.Tgl10_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 11)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl11_void++;
                        else entityPatientDoctor.Tgl11_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 12)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl12_void++;
                        else entityPatientDoctor.Tgl12_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 13)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl13_void++;
                        else entityPatientDoctor.Tgl13_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 14)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl14_void++;
                        else entityPatientDoctor.Tgl14_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 15)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl15_void++;
                        else entityPatientDoctor.Tgl15_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 16)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl16_void++;
                        else entityPatientDoctor.Tgl16_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 17)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl17_void++;
                        else entityPatientDoctor.Tgl17_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 18)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl18_void++;
                        else entityPatientDoctor.Tgl18_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 19)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl19_void++;
                        else entityPatientDoctor.Tgl19_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 20)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl20_void++;
                        else entityPatientDoctor.Tgl20_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 21)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl21_void++;
                        else entityPatientDoctor.Tgl21_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 22)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl22_void++;
                        else entityPatientDoctor.Tgl22_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 23)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl23_void++;
                        else entityPatientDoctor.Tgl23_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 24)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl24_void++;
                        else entityPatientDoctor.Tgl24_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 25)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl25_void++;
                        else entityPatientDoctor.Tgl25_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 26)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl26_void++;
                        else entityPatientDoctor.Tgl26_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 27)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl27_void++;
                        else entityPatientDoctor.Tgl27_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 28)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl28_void++;
                        else entityPatientDoctor.Tgl28_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 29)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl29_void++;
                        else entityPatientDoctor.Tgl29_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 30)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl30_void++;
                        else entityPatientDoctor.Tgl30_valid++;
                    }
                    if (entityConsultVisit.VisitDate.Day == 31)
                    {
                        if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CANCELLED) entityPatientDoctor.Tgl31_void++;
                        else entityPatientDoctor.Tgl31_valid++;
                    }
                }
                lstPatientDoctorPerMonth.Add(entityPatientDoctor);
            }
            lvwView.DataSource = lstPatientDoctorPerMonth;
            lvwView.DataBind();
        }

        private void fillYear()
        {
            int currentYear = DateTime.Now.Year;
            for (int a = 0; a < 10; a++)
            {
                cboYear.Items.Add((currentYear - a).ToString());
            }
        }

        private void fillMonth()
        {
            List<Month> lstMonth = new List<Month>();
            lstMonth.Add(new Month() { MonthNumber = 1, MonthText = "January" });
            lstMonth.Add(new Month() { MonthNumber = 2, MonthText = "February" });
            lstMonth.Add(new Month() { MonthNumber = 3, MonthText = "Maret" });
            lstMonth.Add(new Month() { MonthNumber = 4, MonthText = "April" });
            lstMonth.Add(new Month() { MonthNumber = 5, MonthText = "Mei" });
            lstMonth.Add(new Month() { MonthNumber = 6, MonthText = "Juni" });
            lstMonth.Add(new Month() { MonthNumber = 7, MonthText = "Juli" });
            lstMonth.Add(new Month() { MonthNumber = 8, MonthText = "Agustus" });
            lstMonth.Add(new Month() { MonthNumber = 9, MonthText = "September" });
            lstMonth.Add(new Month() { MonthNumber = 10, MonthText = "Oktober" });
            lstMonth.Add(new Month() { MonthNumber = 11, MonthText = "November" });
            lstMonth.Add(new Month() { MonthNumber = 12, MonthText = "Desember" });

            Methods.SetComboBoxField<Month>(cboMonth, lstMonth, "MonthText", "MonthNumber");
        }

        public class Month
        {
            private int _MonthNumber;

            public int MonthNumber
            {
                get { return _MonthNumber; }
                set { _MonthNumber = value; }
            }
            private string _MonthText;

            public string MonthText
            {
                get { return _MonthText; }
                set { _MonthText = value; }
            }
        }

        public class PatientDoctorPerMonth
        {
            private string _DepartmentID;

            public string DepartmentID
            {
                get { return _DepartmentID; }
                set { _DepartmentID = value; }
            }
            private string _DepartmentName;

            public string DepartmentName
            {
                get { return _DepartmentName; }
                set { _DepartmentName = value; }
            }
            private int _tgl1_valid = 0;

            public int Tgl1_valid
            {
                get { return _tgl1_valid; }
                set { _tgl1_valid = value; }
            }
            private int _tgl1_void = 0;

            public int Tgl1_void
            {
                get { return _tgl1_void; }
                set { _tgl1_void = value; }
            }
            private int _tgl2_valid = 0;

            public int Tgl2_valid
            {
                get { return _tgl2_valid; }
                set { _tgl2_valid = value; }
            }
            private int _tgl2_void = 0;

            public int Tgl2_void
            {
                get { return _tgl2_void; }
                set { _tgl2_void = value; }
            }
            private int _tgl3_valid = 0;

            public int Tgl3_valid
            {
                get { return _tgl3_valid; }
                set { _tgl3_valid = value; }
            }
            private int _tgl3_void = 0;

            public int Tgl3_void
            {
                get { return _tgl3_void; }
                set { _tgl3_void = value; }
            }
            private int _tgl4_valid = 0;

            public int Tgl4_valid
            {
                get { return _tgl4_valid; }
                set { _tgl4_valid = value; }
            }
            private int _tgl4_void = 0;

            public int Tgl4_void
            {
                get { return _tgl4_void; }
                set { _tgl4_void = value; }
            }
            private int _tgl5_valid = 0;

            public int Tgl5_valid
            {
                get { return _tgl5_valid; }
                set { _tgl5_valid = value; }
            }
            private int _tgl5_void = 0;

            public int Tgl5_void
            {
                get { return _tgl5_void; }
                set { _tgl5_void = value; }
            }
            private int _tgl6_valid = 0;

            public int Tgl6_valid
            {
                get { return _tgl6_valid; }
                set { _tgl6_valid = value; }
            }
            private int _tgl6_void = 0;

            public int Tgl6_void
            {
                get { return _tgl6_void; }
                set { _tgl6_void = value; }
            }
            private int _tgl7_valid = 0;

            public int Tgl7_valid
            {
                get { return _tgl7_valid; }
                set { _tgl7_valid = value; }
            }
            private int _tgl7_void = 0;

            public int Tgl7_void
            {
                get { return _tgl7_void; }
                set { _tgl7_void = value; }
            }
            private int _tgl8_valid = 0;

            public int Tgl8_valid
            {
                get { return _tgl8_valid; }
                set { _tgl8_valid = value; }
            }
            private int _tgl8_void = 0;

            public int Tgl8_void
            {
                get { return _tgl8_void; }
                set { _tgl8_void = value; }
            }
            private int _tgl9_valid = 0;

            public int Tgl9_valid
            {
                get { return _tgl9_valid; }
                set { _tgl9_valid = value; }
            }
            private int _tgl9_void = 0;

            public int Tgl9_void
            {
                get { return _tgl9_void; }
                set { _tgl9_void = value; }
            }
            private int _tgl10_valid = 0;

            public int Tgl10_valid
            {
                get { return _tgl10_valid; }
                set { _tgl10_valid = value; }
            }
            private int _tgl10_void = 0;

            public int Tgl10_void
            {
                get { return _tgl10_void; }
                set { _tgl10_void = value; }
            }
            private int _tgl11_valid = 0;

            public int Tgl11_valid
            {
                get { return _tgl11_valid; }
                set { _tgl11_valid = value; }
            }
            private int _tgl11_void = 0;

            public int Tgl11_void
            {
                get { return _tgl11_void; }
                set { _tgl11_void = value; }
            }
            private int _tgl12_valid = 0;

            public int Tgl12_valid
            {
                get { return _tgl12_valid; }
                set { _tgl12_valid = value; }
            }
            private int _tgl12_void = 0;

            public int Tgl12_void
            {
                get { return _tgl12_void; }
                set { _tgl12_void = value; }
            }
            private int _tgl13_valid = 0;

            public int Tgl13_valid
            {
                get { return _tgl13_valid; }
                set { _tgl13_valid = value; }
            }
            private int _tgl13_void = 0;

            public int Tgl13_void
            {
                get { return _tgl13_void; }
                set { _tgl13_void = value; }
            }
            private int _tgl14_valid = 0;

            public int Tgl14_valid
            {
                get { return _tgl14_valid; }
                set { _tgl14_valid = value; }
            }
            private int _tgl14_void = 0;

            public int Tgl14_void
            {
                get { return _tgl14_void; }
                set { _tgl14_void = value; }
            }
            private int _tgl15_valid = 0;

            public int Tgl15_valid
            {
                get { return _tgl15_valid; }
                set { _tgl15_valid = value; }
            }
            private int _tgl15_void = 0;

            public int Tgl15_void
            {
                get { return _tgl15_void; }
                set { _tgl15_void = value; }
            }
            private int _tgl16_valid = 0;

            public int Tgl16_valid
            {
                get { return _tgl16_valid; }
                set { _tgl16_valid = value; }
            }
            private int _tgl16_void = 0;

            public int Tgl16_void
            {
                get { return _tgl16_void; }
                set { _tgl16_void = value; }
            }
            private int _tgl17_valid = 0;

            public int Tgl17_valid
            {
                get { return _tgl17_valid; }
                set { _tgl17_valid = value; }
            }
            private int _tgl17_void = 0;

            public int Tgl17_void
            {
                get { return _tgl17_void; }
                set { _tgl17_void = value; }
            }
            private int _tgl18_valid = 0;

            public int Tgl18_valid
            {
                get { return _tgl18_valid; }
                set { _tgl18_valid = value; }
            }
            private int _tgl18_void = 0;

            public int Tgl18_void
            {
                get { return _tgl18_void; }
                set { _tgl18_void = value; }
            }
            private int _tgl19_valid = 0;

            public int Tgl19_valid
            {
                get { return _tgl19_valid; }
                set { _tgl19_valid = value; }
            }
            private int _tgl19_void = 0;

            public int Tgl19_void
            {
                get { return _tgl19_void; }
                set { _tgl19_void = value; }
            }
            private int _tgl20_valid = 0;

            public int Tgl20_valid
            {
                get { return _tgl20_valid; }
                set { _tgl20_valid = value; }
            }
            private int _tgl20_void = 0;

            public int Tgl20_void
            {
                get { return _tgl20_void; }
                set { _tgl20_void = value; }
            }
            private int _tgl21_valid = 0;

            public int Tgl21_valid
            {
                get { return _tgl21_valid; }
                set { _tgl21_valid = value; }
            }
            private int _tgl21_void = 0;

            public int Tgl21_void
            {
                get { return _tgl21_void; }
                set { _tgl21_void = value; }
            }
            private int _tgl22_valid = 0;

            public int Tgl22_valid
            {
                get { return _tgl22_valid; }
                set { _tgl22_valid = value; }
            }
            private int _tgl22_void = 0;

            public int Tgl22_void
            {
                get { return _tgl22_void; }
                set { _tgl22_void = value; }
            }
            private int _tgl23_valid = 0;

            public int Tgl23_valid
            {
                get { return _tgl23_valid; }
                set { _tgl23_valid = value; }
            }
            private int _tgl23_void = 0;

            public int Tgl23_void
            {
                get { return _tgl23_void; }
                set { _tgl23_void = value; }
            }
            private int _tgl24_valid = 0;

            public int Tgl24_valid
            {
                get { return _tgl24_valid; }
                set { _tgl24_valid = value; }
            }
            private int _tgl24_void = 0;

            public int Tgl24_void
            {
                get { return _tgl24_void; }
                set { _tgl24_void = value; }
            }
            private int _tgl25_valid = 0;

            public int Tgl25_valid
            {
                get { return _tgl25_valid; }
                set { _tgl25_valid = value; }
            }
            private int _tgl25_void = 0;

            public int Tgl25_void
            {
                get { return _tgl25_void; }
                set { _tgl25_void = value; }
            }
            private int _tgl26_valid = 0;

            public int Tgl26_valid
            {
                get { return _tgl26_valid; }
                set { _tgl26_valid = value; }
            }
            private int _tgl26_void = 0;

            public int Tgl26_void
            {
                get { return _tgl26_void; }
                set { _tgl26_void = value; }
            }
            private int _tgl27_valid = 0;

            public int Tgl27_valid
            {
                get { return _tgl27_valid; }
                set { _tgl27_valid = value; }
            }
            private int _tgl27_void = 0;

            public int Tgl27_void
            {
                get { return _tgl27_void; }
                set { _tgl27_void = value; }
            }
            private int _tgl28_valid = 0;

            public int Tgl28_valid
            {
                get { return _tgl28_valid; }
                set { _tgl28_valid = value; }
            }
            private int _tgl28_void = 0;

            public int Tgl28_void
            {
                get { return _tgl28_void; }
                set { _tgl28_void = value; }
            }
            private int _tgl29_valid = 0;

            public int Tgl29_valid
            {
                get { return _tgl29_valid; }
                set { _tgl29_valid = value; }
            }
            private int _tgl29_void = 0;

            public int Tgl29_void
            {
                get { return _tgl29_void; }
                set { _tgl29_void = value; }
            }
            private int _tgl30_valid = 0;

            public int Tgl30_valid
            {
                get { return _tgl30_valid; }
                set { _tgl30_valid = value; }
            }
            private int _tgl30_void = 0;

            public int Tgl30_void
            {
                get { return _tgl30_void; }
                set { _tgl30_void = value; }
            }
            private int _tgl31_valid = 0;

            public int Tgl31_valid
            {
                get { return _tgl31_valid; }
                set { _tgl31_valid = value; }
            }
            private int _tgl31_void = 0;

            public int Tgl31_void
            {
                get { return _tgl31_void; }
                set { _tgl31_void = value; }
            }
        }


    }
}