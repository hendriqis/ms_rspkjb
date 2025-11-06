using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class EpisodeMedicationPicksCtl1 : BaseProcessPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            hdnPopupVisitID.Value = param;

            List<Variable> lstDisplay = new List<Variable>() { new Variable() { Code = "All", Value = "0" }
                , new Variable() { Code = "Obat Oral/Supposituria/Topikal", Value = "1" }, new Variable() { Code = "Obat Injeksi", Value = "2" }};
            Methods.SetComboBoxField(cboDisplay, lstDisplay, "Code", "Value");
            cboDisplay.Value = hdnDisplayMode.Value;

            List<Variable> lstStatus = new List<Variable>() { 
                new Variable() { Code = "All", Value = "0" }, 
            new Variable() { Code = "Active", Value = "1" }, 
            new Variable() { Code = "Stop", Value = "2" }};
            Methods.SetComboBoxField(cboMedicationStatus, lstStatus, "Code", "Value");
            cboMedicationStatus.Value = hdnMedicationStatus.Value;

            BindGridView(1, true, ref PageCount);
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected void cbpPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }


        private string GetFilterExpression()
        {
            string filterExpression = string.Format("MRN = {0} AND VisitID = {1} AND GCPrescriptionType = '{2}' AND GCTransactionStatus NOT IN ('{3}','{4}')", AppSession.RegisteredPatient.MRN, hdnPopupVisitID.Value, Constant.PrescriptionType.DISCHARGE_PRESCRIPTION, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.VOID);
            return filterExpression;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            try
            {
                string displayMode = cboDisplay.Value.ToString();

                List<PatientMedicationSummary> lstEntity = BusinessLayer.GetPatientMedicationSummaryList(AppSession.RegisteredPatient.RegistrationID, displayMode, cboMedicationStatus.Value.ToString());

                lvwView.DataSource = lstEntity;
                lvwView.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            string medicationLineText = hdnSelectedItem.Value.Replace("|", Environment.NewLine);
            retval = medicationLineText;
            return result;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}