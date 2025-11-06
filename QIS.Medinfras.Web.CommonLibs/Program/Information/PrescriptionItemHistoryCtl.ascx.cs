using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PrescriptionItemHistoryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            
        }

        protected void cbpViewPopUpHistoryCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            //int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref PageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref PageCount);
                    result = "refresh|" + PageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = String.Format("RegistrationID IN (SELECT RegistrationID FROM Registration WHERE MRN = {0} AND GCRegistrationStatus != '{1}') AND GCTransactionStatus != '{2}'", AppSession.RegisteredPatient.MRN,Constant.VisitStatus.CANCELLED, Constant.TransactionStatus.VOID);

            if (!string.IsNullOrEmpty(txtDuration.Text) && txtDuration.Text != "0" )
            {
                filterExpression += String.Format(" AND TransactionDate BETWEEN  CONVERT(VARCHAR(10),DATEADD(day,{0}*-1,GetDate()),112) AND CONVERT(VARCHAR(10),GetDate(),112) ", Convert.ToInt16(txtDuration.Text));
            }

            if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += String.Format("AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesDtPrescriptionRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vPatientChargesDtPrescription> lstEntity = BusinessLayer.GetvPatientChargesDtPrescriptionList(filterExpression, 8, pageIndex, "TransactionDate DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();

        }
    }
}