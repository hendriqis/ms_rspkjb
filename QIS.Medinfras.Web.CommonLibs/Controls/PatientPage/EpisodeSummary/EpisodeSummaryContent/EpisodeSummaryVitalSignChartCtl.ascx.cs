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
    public partial class EpisodeSummaryVitalSignChartCtl : BaseViewPopupCtl
    {
        class LaboratoryDtInfo
        {
            public int ChargesID { get; set; }
            public int ItemID { get; set; }
            public string FractionName { get; set; }
            public string ResultValue { get; set; }
            public string ResultUnit { get; set; }
            public string RefRange { get; set; }
            public string ResultFlag { get; set; }
        }

        public override void InitializeDataControl(string queryString)
        {
            BindGridView(Convert.ToInt32(queryString));
        }

        private void BindGridView(int visitID)
        {
            string filterExpression = string.Empty;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("TransactionID IN (SELECT TransactionID FROM PatientChargesHd WHERE VisitID = {0} AND TransactionCode = '{1}' AND GCTransactionStatus <> '{2}') AND IsDeleted = 0 ORDER BY ID DESC", visitID, Constant.TransactionCode.LABORATORY_CHARGES, Constant.TransactionStatus.VOID);

            List<vPatientChargesDt> lstEntity = BusinessLayer.GetvPatientChargesDtList(filterExpression);
            //lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} ORDER BY DisplayOrder", visitID));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientChargesDt obj = (vPatientChargesDt)e.Row.DataItem;
                List<LaboratoryDtInfo> lstResultDt = new List<LaboratoryDtInfo>();
                List<vLaboratoryResultDt> lstLabEntity = BusinessLayer.GetvLaboratoryResultDtList(string.Format("ChargeTransactionID = {0} AND ItemID = {1} ORDER BY DisplayOrder", obj.TransactionID, obj.ItemID));
                Repeater rptLaboratoryDt = (Repeater)e.Row.FindControl("rptLaboratoryDt");
                if (lstLabEntity.Count > 0)
                {
                    foreach (vLaboratoryResultDt result in lstLabEntity)
                    {
                        LaboratoryDtInfo oDetail = new LaboratoryDtInfo();
                        oDetail.ChargesID = result.ChargeTransactionID;
                        oDetail.ItemID = result.ItemID;
                        oDetail.FractionName = result.FractionName1;
                        oDetail.ResultValue = result.MetricResultValue.ToString("G29");
                        oDetail.ResultUnit = result.MetricUnit;
                        oDetail.RefRange = string.Format("{0} - {1}", result.MinMetricNormalValue.ToString("G29"), result.MaxMetricNormalValue.ToString("G29"));
                        oDetail.ResultFlag = result.ResultFlag;

                        lstResultDt.Add(oDetail);
                    }
                }
                rptLaboratoryDt.DataSource = lstResultDt;
                rptLaboratoryDt.DataBind();
            }
        }
    }
}