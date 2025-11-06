using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common.UI;
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PastMedicalHistoryContentCtl : BaseViewPopupCtl
    {
        protected string VisitID = "";
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;
        protected List<vVitalSignDt> lstVitalSignDt = null;
        private List<vTestOrderHd> lstTestOrderHd = null;

        private class MedicalSummary
        {
            public string ChiefComplaintText { get; set; }
        }

        private class DiagnosisSummary
        {
            public string DiagnosisText { get; set; }
        }

        private class MedicationSummary
        {
            public string MedicationSummaryText { get; set; }
        }

        private class TreatmentSummary
        {
            public string TreatmentSummaryText { get; set; }
        }

        public override void InitializeDataControl(string queryString)
        {
            VisitID = queryString;
            string filterExpression = string.Format("PmHxID = {0} AND IsDeleted = 0", queryString);
            vPastMedical oHistory = BusinessLayer.GetvPastMedicalList(filterExpression).FirstOrDefault();

            List<MedicalSummary> lstMedicalSummary = new List<MedicalSummary>();
            MedicalSummary oMedicalSummary = new MedicalSummary();
            oMedicalSummary.ChiefComplaintText = oHistory.MedicalSummary.Replace(@"\n", Environment.NewLine);
            lstMedicalSummary.Add(oMedicalSummary);

            rptChiefComplaint.DataSource = lstMedicalSummary;
            rptChiefComplaint.DataBind();


            List<DiagnosisSummary> lstDiagnosis = new List<DiagnosisSummary>();
            DiagnosisSummary oDiagnosisSummary = new DiagnosisSummary();
            oDiagnosisSummary.DiagnosisText = oHistory.DiagnosisSummary.Replace(@"\n", Environment.NewLine);
            lstDiagnosis.Add(oDiagnosisSummary);

            rptDiagnosis.DataSource = lstDiagnosis;
            rptDiagnosis.DataBind();

            List<MedicationSummary> lstMedicationSummary = new List<MedicationSummary>();
            MedicationSummary oMedicationSummary = new MedicationSummary();
            oMedicationSummary.MedicationSummaryText = oHistory.MedicationSummary.Replace(@"\n", Environment.NewLine);
            lstMedicationSummary.Add(oMedicationSummary);

            rptMedication.DataSource = lstMedicationSummary;
            rptMedication.DataBind();

            List<TreatmentSummary> lstTreatment = new List<TreatmentSummary>();
            TreatmentSummary oTreatment = new TreatmentSummary();
            oTreatment.TreatmentSummaryText = oHistory.TreatmentSummary.Replace(@"\n", Environment.NewLine);
            lstTreatment.Add(oTreatment);

            rptTreatment.DataSource = lstTreatment;
            rptTreatment.DataBind();
        }

        protected void rptReviewOfSystemHd_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    vReviewOfSystemHd obj = (vReviewOfSystemHd)e.Item.DataItem;
            //    Repeater rptReviewOfSystemDt = (Repeater)e.Item.FindControl("rptReviewOfSystemDt");
            //    rptReviewOfSystemDt.DataSource = lstReviewOfSystemDt.Where(p => p.ID == obj.ID).ToList();
            //    rptReviewOfSystemDt.DataBind();
            //}
        }

        protected void rptVitalSignHd_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    vVitalSignHd obj = (vVitalSignHd)e.Item.DataItem;
            //    Repeater rptVitalSignDt = (Repeater)e.Item.FindControl("rptVitalSignDt");
            //    rptVitalSignDt.DataSource = lstVitalSignDt.Where(p => p.ID == obj.ID).ToList();
            //    rptVitalSignDt.DataBind();
            //}
        }

        protected void rptTestOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    vTestOrderDt obj = (vTestOrderDt)e.Item.DataItem;

            //    HtmlGenericControl spnTestOrderDtInformation = (HtmlGenericControl)e.Item.FindControl("spnTestOrderDtInformation");
            //    vTestOrderHd entityHd = lstTestOrderHd.FirstOrDefault(p => p.TestOrderID == obj.TestOrderID);
            //    spnTestOrderDtInformation.InnerHtml = string.Format("{0}, {1}", entityHd.TestOrderDateTimeInString, entityHd.ParamedicName);
            //}
        }
    }
}