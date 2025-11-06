using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class EpisodeSummaryEpisodeInformationCtl2 : BaseViewPopupCtl
    {
        protected int gridAllergyPageCount = 1;
        protected int _MRN = 0;

        public override void InitializeDataControl(string queryString)
        {
            LoadSubjectiveContent(Convert.ToInt32(queryString));
            BindGridViewAllergy(1, true, ref gridAllergyPageCount);
        }

        private void LoadSubjectiveContent(int visitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY ID DESC", visitID);
            vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(filterExpression).FirstOrDefault();

            if (oChiefComplaint != null)
            {
                _MRN = oChiefComplaint.MRN;
                txtMedicalHistory.Text = oChiefComplaint.PastMedicalHistory;
                txtMedicationHistory.Text = oChiefComplaint.PastMedicationHistory;
                txtFamilyHistory.Text = oChiefComplaint.FamilyHistory;
                txtNursingObjectives.Text = oChiefComplaint.NursingObjectives;
                rblIsNeedDischargePlan.SelectedValue = oChiefComplaint.IsNeedDischargePlan ? "1" : "0";
                txtEstimatedLOS.Text = !string.IsNullOrEmpty(oChiefComplaint.EstimatedLOS.ToString()) ? oChiefComplaint.EstimatedLOS.ToString() : "0";
                rblEstimatedLOSUnit.SelectedValue = oChiefComplaint.IsEstimatedLOSInDays ? "1" : "0";
            }
        }

        private void BindGridViewAllergy(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", _MRN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAllergyRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientAllergy> lstEntity = BusinessLayer.GetvPatientAllergyList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdAllergyView.DataSource = lstEntity;
            grdAllergyView.DataBind();

            chkIsPatientAllergyExists.Checked = !(lstEntity.Count > 0);
        }

        protected void cbpAllergyView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewAllergy(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewAllergy(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}