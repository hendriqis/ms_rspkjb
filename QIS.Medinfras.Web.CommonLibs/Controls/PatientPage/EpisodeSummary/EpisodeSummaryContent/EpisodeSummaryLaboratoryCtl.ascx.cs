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
    public partial class EpisodeSummaryLaboratoryCtl : BaseViewPopupCtl
    {
        private List<vTestOrderDt> ListvTestOrderDt = null;

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
            filterExpression += string.Format("TransactionID IN (SELECT TransactionID FROM PatientChargesHd WHERE VisitID = {0} AND TransactionCode IN ('{1}','{2}') AND GCTransactionStatus <> '{3}') AND HealthcareServiceUnitID = {4} AND IsDeleted = 0 ORDER BY ID DESC", visitID, Constant.TransactionCode.LABORATORY_CHARGES, Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES, Constant.TransactionStatus.VOID, AppSession.LaboratoryServiceUnitID);

            List<vPatientChargesDt> lstEntity = BusinessLayer.GetvPatientChargesDtList(filterExpression);
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
                        oDetail.ResultValue = result.IsNumeric ? result.MetricResultValue.ToString("G29") : result.TextValue ;
                        oDetail.ResultUnit = result.IsNumeric ? result.MetricUnit : string.Empty;
                        oDetail.RefRange = result.IsNumeric ? (string.Format("{0} - {1}", result.MinMetricNormalValue.ToString("G29"), result.MaxMetricNormalValue.ToString("G29"))) : string.Empty;
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