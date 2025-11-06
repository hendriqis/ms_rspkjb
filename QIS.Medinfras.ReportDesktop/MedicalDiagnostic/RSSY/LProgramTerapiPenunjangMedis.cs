using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Text;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LProgramTerapiPenunjangMedis : BaseDailyPortraitRpt
    {
        public LProgramTerapiPenunjangMedis()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            //string[] temp = param[1].Split(';');
            // lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

            string testOrderID = param[0];
            string testOrderDate = string.Empty;
            vRegistration reg = BusinessLayer.GetvRegistrationList(testOrderID).FirstOrDefault();
            if (reg != null)
            {
                testOrderDate = reg.RegistrationDateInString;
                lblMedicalNo.Text = reg.MedicalNo;
                lblPatientName.Text = reg.PatientName;
                lblParamedicOrder.Text = string.Format("({0})", reg.ParamedicName);
                ConsultVisit cv = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", reg.RegistrationID)).FirstOrDefault();
                vPatientDiagnosis diag = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", cv.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();
                if (string.IsNullOrEmpty(diag.DiagnoseID))
                {
                    lblDiagnose.Text = diag.DiagnosisText;
                }
                else
                {
                    lblDiagnose.Text = string.Format("({0}) {1}", diag.DiagnoseID, diag.DiagnoseName);
                }
                List<vTestOrderHd> lstSch = BusinessLayer.GetvTestOrderHdList(string.Format("RegistrationID = {0} AND IsMultiVisitScheduleOrder = 1 AND GCTransactionStatus != '{1}'", reg.RegistrationID, Constant.TransactionStatus.VOID));
                if (lstSch.Count > 0)
                {
                    StringBuilder lstItem = new StringBuilder();
                    string lstOrderID = string.Empty;
                    foreach (vTestOrderHd hd in lstSch)
                    {
                        lstOrderID += string.Format("{0},", hd.TestOrderID);
                    }
                    lstOrderID = lstOrderID.Remove(lstOrderID.Length - 1, 1);
                    List<vTestOrderDt> lstDt = BusinessLayer.GetvTestOrderDtList(string.Format("TestOrderID IN ({0}) AND IsDeleted = 0", lstOrderID));
                    foreach (vTestOrderDt a in lstDt)
                    {
                        lstItem.AppendLine(string.Format("{0} {1}X", a.ItemName1, a.ItemQty.ToString("N0")));
                    }
                    lblOrderItem.Text = lstItem.ToString();
                }

            }

            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            if (h != null)
            {
                lblHealthcareAndDate.Text = string.Format("{0}, {1}", h.City, testOrderDate);
            }

            base.InitializeReport(param);
        }

    }
}
