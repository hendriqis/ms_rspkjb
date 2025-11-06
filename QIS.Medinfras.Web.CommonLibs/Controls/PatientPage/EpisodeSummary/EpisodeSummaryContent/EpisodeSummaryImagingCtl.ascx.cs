using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class EpisodeSummaryImagingCtl : BaseViewPopupCtl
    {
        private List<vTestOrderDt> ListvTestOrderDt = null;
        public override void InitializeDataControl(string queryString)
        {
            BindGridView(Convert.ToInt32(queryString));
        }

        private void BindGridView(int visitID)
        {
            string filterExpression = string.Empty;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("TransactionID IN (SELECT TransactionID FROM PatientChargesHd WHERE VisitID = {0} AND TransactionCode IN ('{1}','{2}') AND GCTransactionStatus <> '{3}') AND IsDeleted = 0 ORDER BY ID DESC", visitID, Constant.TransactionCode.IMAGING_CHARGES, Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES, Constant.TransactionStatus.VOID);

            List<vPatientChargesDt> lstEntity = BusinessLayer.GetvPatientChargesDtList(filterExpression);
            //lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} ORDER BY DisplayOrder", visitID));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientChargesDt oChargesDt = e.Row.DataItem as vPatientChargesDt;
                vImagingResultDt oResultDt = BusinessLayer.GetvImagingResultDtList(string.Format("ID IN (SELECT ID FROM ImagingResultHd WHERE ChargeTransactionID = {0}) AND ItemID = {1} AND IsDeleted = 0", oChargesDt.TransactionID, oChargesDt.ItemID)).FirstOrDefault();

                Literal literal = (Literal)e.Row.FindControl("literal");
                literal.Text = oResultDt.TestResult1;


                //HtmlTextArea taResultValue = (HtmlTextArea)e.Row.FindControl("taResultValue");
                //if (oResultDt != null && taResultValue != null)
                //    taResultValue.InnerText = oResultDt.TestResult1;
            }
        }
    }
}