using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class EpisodeSummary : BasePagePatientPageList
    {
        public string chartData = "";
        private List<vTestOrderHd> lstTestOrderHd = null;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.EPISODE_SUMMARY;
        }

        protected override void InitializeDataControl()
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            rptChiefComplaint.DataSource = BusinessLayer.GetvChiefComplaintList(filterExpression, 3, 1, "ID DESC");
            rptChiefComplaint.DataBind();

            rptDiagnosis.DataSource = BusinessLayer.GetvPatientDiagnosisList(filterExpression, 3, 1, "GCDiagnoseType");
            rptDiagnosis.DataBind();

            rptPatientInstruction.DataSource = BusinessLayer.GetvPatientInstructionList(filterExpression, 3, 1, "PatientInstructionID DESC");
            rptPatientInstruction.DataBind();

            rptTestOrder.DataSource = BusinessLayer.GetvTestOrderDtList(filterExpression, 3, 1, "ID DESC");

            filterExpression = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}'", AppSession.RegisteredPatient.VisitID, Constant.TransactionStatus.VOID);
            lstTestOrderHd = BusinessLayer.GetvTestOrderHdList(filterExpression);
            rptTestOrder.DataBind();

            filterExpression = string.Format("VisitID = {0} AND OrderIsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            rptMedication.DataSource = BusinessLayer.GetvPrescriptionOrderDtCustomList(filterExpression,10,1, "PrescriptionOrderDetailID DESC");
            rptMedication.DataBind();

            filterExpression = string.Format("FromVisitID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}')", AppSession.RegisteredPatient.VisitID, Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED);
            rptFollowUpVisit.DataSource = BusinessLayer.GetvAppointmentList(filterExpression, 3, 1, "AppointmentID DESC");
            rptFollowUpVisit.DataBind();

            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
        }

        protected void rptTestOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vTestOrderDt obj = (vTestOrderDt)e.Item.DataItem;

                HtmlGenericControl spnTestOrderDtInformation = (HtmlGenericControl)e.Item.FindControl("spnTestOrderDtInformation");
                vTestOrderHd entityHd = lstTestOrderHd.FirstOrDefault(p => p.TestOrderID == obj.TestOrderID);
                if (entityHd != null)
                {
                    spnTestOrderDtInformation.InnerHtml = string.Format("{0}, {1}", entityHd.TestOrderDateTimeInString, entityHd.ParamedicName);  
                }
            }
        }
    }
}