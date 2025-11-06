using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class CustomerContractInformationList : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case "OP": return Constant.MenuCode.Outpatient.CUSTOMER_CONTRACT_INFORMATION;
                case "ER": return Constant.MenuCode.EmergencyCare.CUSTOMER_CONTRACT_INFORMATION;
                case "IP": return Constant.MenuCode.Inpatient.CUSTOMER_CONTRACT_INFORMATION;
                case "IS": return Constant.MenuCode.Imaging.CUSTOMER_CONTRACT_INFORMATION;
                case "LB": return Constant.MenuCode.Laboratory.CUSTOMER_CONTRACT_INFORMATION;
                case "MD": return Constant.MenuCode.MedicalDiagnostic.CUSTOMER_CONTRACT_INFORMATION;
                case "PH": return Constant.MenuCode.Pharmacy.CUSTOMER_CONTRACT_INFORMATION;
                case "RT": return Constant.MenuCode.Radiotheraphy.CUSTOMER_CONTRACT_INFORMATION;
                default: return Constant.MenuCode.Finance.CUSTOMER_CONTRACT_INFORMATION;
            }
        }

        protected override void OnControlEntrySetting()
        {
            rblDataSource.SelectedIndex = 0;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            txtEndDateContract.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Variable> lstVar = new List<Variable>(){
                new Variable{ Code = " ",Value = "0"},
                new Variable{ Code = "1",Value = "1"},
                new Variable{ Code = "2",Value = "2"},
                new Variable{ Code = "3",Value = "3"},
                new Variable{ Code = "4",Value = "4"},
                new Variable{ Code = "5",Value = "5"},
                new Variable{ Code = "6",Value = "6"},
                new Variable{ Code = "7",Value = "7"},
                new Variable{ Code = "8",Value = "8"},
                new Variable{ Code = "9",Value = "9"}, 
                new Variable{ Code = "10",Value = "10"},
                new Variable{ Code = "11",Value = "11"},
                new Variable{ Code = "12",Value = "12"}};
            Methods.SetComboBoxField<Variable>(cboContractMonth, lstVar, "Code", "Value");
            cboContractMonth.SelectedIndex = 0;

            BindGridView(1, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";

            if (e.Parameter != null && e.Parameter != "")
            {
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

        protected string GetFilterExpression()
        {
            string filterExpression = "";

            if (rblDataSource.SelectedValue == "filterDate")
            {
                if (!chkEndDate.Checked)
                {
                    filterExpression += String.Format("EndDate = '{0}'", Helper.GetDatePickerValue(txtEndDateContract.Text));
                    if (hdnFilterExpressionQuickSearch.Value == "Search")
                        hdnFilterExpressionQuickSearch.Value = " ";
                    if (hdnFilterExpressionQuickSearch.Value != "")
                        filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

                    filterExpression += " AND IsDeleted = 0";
                }
                else
                {
                    filterExpression += "IsDeleted = 0";

                    if (hdnFilterExpressionQuickSearch.Value == "Search")
                        hdnFilterExpressionQuickSearch.Value = " ";
                    if (hdnFilterExpressionQuickSearch.Value != "")
                        filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
                }
            }
            else
            {
                String ContractMonth = Convert.ToString(cboContractMonth.Value);
                if (ContractMonth != "0")
                {
                    filterExpression = string.Format("Selisih BETWEEN '{0}' AND '{1}'", cboContractMonth.Value, cboContractMonth.Value);
                    if (hdnFilterExpressionQuickSearch.Value == "Search")
                        hdnFilterExpressionQuickSearch.Value = " ";
                    if (hdnFilterExpressionQuickSearch.Value != "")
                        filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

                    filterExpression += " AND IsDeleted = 0";
                }
                else
                {
                    filterExpression += "IsDeleted = 0";

                    if (hdnFilterExpressionQuickSearch.Value == "Search")
                        hdnFilterExpressionQuickSearch.Value = " ";
                    if (hdnFilterExpressionQuickSearch.Value != "")
                        filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
                }
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvCustomerContractCustomRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 15);
            }
            List<vCustomerContractCustom> lstEntity = BusinessLayer.GetvCustomerContractList(filterExpression, Constant.GridViewPageSize.GRID_ITEM, pageIndex, "BusinessPartnerCode");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}