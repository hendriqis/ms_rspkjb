using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class RecipeInformationPerDrug : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.RECIPE_INFORMATION_PER_DRUG;
        }

        protected string OnGetLocationFilterExpression()
        {
            return string.Format("{0};{1};;", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnID.Value = Page.Request.QueryString["id"];
            txtDateFrom.Text = txtDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            string filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsUsingRegistration = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.PHARMACY);

            List<vHealthcareServiceUnit> lstHServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex  = 0;

            List<Variable> lstType = new List<Variable>();
            lstType.Add(new Variable { Code = "0", Value = "Semua" });
            lstType.Add(new Variable { Code = "1", Value = "Pengeluaran" });
            lstType.Add(new Variable { Code = "2", Value = "Retur" });
            Methods.SetComboBoxField<Variable>(cboType, lstType, "Value", "Code");
            cboType.SelectedIndex = 0;

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList(string.Format("IsActive = 1 AND DepartmentID != '{0}' AND IsHasRegistration = 1", Constant.Facility.PHARMACY));
            lstDepartment.Insert(0, new Department { DepartmentName = string.Format("{0}", GetLabel(" ")) });
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;

            BindGridView(CurrPage, true, ref PageCount);
        }

        private string GetFilterExpression()
        {
            string filterExpression = string.Format("TransactionDate BETWEEN '{0}' AND '{1}'",Helper.GetDatePickerValue(txtDateFrom.Text).ToString("yyyyMMdd"), Helper.GetDatePickerValue(txtDateTo.Text).ToString("yyyyMMdd"));
            if (cboServiceUnit.Value != null && cboServiceUnit.Value.ToString() != "")
            {
                int locationID = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", cboServiceUnit.Value)).FirstOrDefault().LocationID;
                hdnLocationID.Value = locationID.ToString();
                filterExpression += string.Format(" AND LocationID = {0} ", hdnLocationID.Value);
            }
            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            else if (cboDepartment.Value != null && cboDepartment.Value.ToString() != "")
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value);

            if (hdnFilterExpressionQuickSearch.Value == "Search")
                hdnFilterExpressionQuickSearch.Value = " ";
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            string type = cboType.Value.ToString();
            if (type == "1")
                filterExpression += " AND UsedQuantity > 0";
            else if (type == "2")
                filterExpression += " AND UsedQuantity < 0";

            filterExpression += string.Format(" AND IsDeleted = 0 AND GCTransactionStatus != '{0}'", Constant.TransactionStatus.VOID);

            return filterExpression;
        }

        public string GetPatientChargesDtFilterExpression()
        {
            return Request.Form[hdnFilterExpression.UniqueID];
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string patientChargesDtFilterExpression = GetFilterExpression();
            hdnFilterExpression.Value = patientChargesDtFilterExpression;
            string filterExpression = String.Format("LocationID = {0} AND ItemID IN (SELECT ItemID FROM vPatientChargesDtInformation WHERE {1})",hdnLocationID.Value, patientChargesDtFilterExpression);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemBalanceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            List<vItemBalance> lstItemBalance = BusinessLayer.GetvItemBalanceList(filterExpression, 10, pageIndex, "ItemName1");
            if (lstItemBalance.Count > 0)
            {
                string lstItemID = "";
                foreach (vItemBalance itemBalance in lstItemBalance)
                {
                    if (lstItemID != "")
                        lstItemID += ",";
                    lstItemID += itemBalance.ItemID.ToString();
                }
                lstPatientChargesDt = BusinessLayer.GetvPatientChargesDtInformationList(string.Format("{0}", patientChargesDtFilterExpression, lstItemID));
            }
            grdView.DataSource = lstItemBalance;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vItemBalance entity = e.Row.DataItem as vItemBalance;
                if (lstPatientChargesDt != null)
                {
                    decimal usedQty = lstPatientChargesDt.Where(p => p.ItemID == entity.ItemID).Sum(p => p.UsedQuantity);
                    HtmlGenericControl divUsedQuantity = e.Row.FindControl("divUsedQuantity") as HtmlGenericControl;
                    divUsedQuantity.InnerHtml = string.Format("{0} {1}", usedQty, entity.ItemUnit);
                }
            }
        }

        List<vPatientChargesDtInformation> lstPatientChargesDt = null;
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}