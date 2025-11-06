using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class VisitPackageInformationCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            Patient patient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", param)).FirstOrDefault();
            if (patient != null)
            {
                txtCustomerName.Text = patient.FullName;
            }

            BindGridView(param);
        }

        protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView(string param)
        {
            List<vVisitPackageBalanceHdChargesInfo> lstBalance = BusinessLayer.GetvVisitPackageBalanceHdChargesInfoList(string.Format("MRN = {0} AND HealthcareServiceUnitID = {1} AND Quantity > 0", param, AppSession.RegisteredPatient.HealthcareServiceUnitID)).GroupBy(g => g.VisitPackageBalanceTransactionID).Select(s => s.FirstOrDefault()).ToList();
            grdView.DataSource = lstBalance;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vVisitPackageBalanceHdChargesInfo entity = e.Row.DataItem as vVisitPackageBalanceHdChargesInfo;
                if (entity.ExpiredDate < DateTime.Now)
                {
                    e.Row.BackColor = System.Drawing.Color.Red;
                }
            }
        }

        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";
            if (hdnID.Value != "")
            {
                filterExpression = string.Format("VisitPackageBalanceTransactionID = {0} AND PackageBalanceQtyTaken > 0", hdnID.Value);
                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvVisitPackageBalanceHdChargesInfoRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
                }

                List<vVisitPackageBalanceHdChargesInfo> lstEntity = BusinessLayer.GetvVisitPackageBalanceHdChargesInfoList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "ID ASC");
                grdViewDetail.DataSource = lstEntity;
                grdViewDetail.DataBind();
            }
        }

    }
}