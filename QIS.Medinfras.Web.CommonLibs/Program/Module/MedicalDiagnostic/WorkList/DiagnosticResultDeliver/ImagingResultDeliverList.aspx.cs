using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ImagingResultDeliverList : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Imaging.RESULT_DELIVERY_TO_PATIENT;
        }

        protected override void OnControlEntrySetting()
        {
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            txtResultDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtResultDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsHasRegistration = 1 AND IsActive = 1");
            lstDepartment.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
            Methods.SetComboBoxField<Department>(cboVisitDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboVisitDepartment.SelectedIndex = 0;

            BindGridView(1, true, ref PageCount);

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

            filterExpression += string.Format("ResultDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtResultDateFrom.Text), Helper.GetDatePickerValue(txtResultDateTo.Text));


            if (cboVisitDepartment.Value != null)
            {
                if (cboVisitDepartment.Value.ToString() != "")
                {
                    if (filterExpression != null && filterExpression != "")
                    {
                        filterExpression += " AND ";
                    }
                    filterExpression += string.Format("VisitDepartmentID = '{0}'", cboVisitDepartment.Value.ToString());

                    if (hdnVisitHealthcareServiceUnitID.Value != null && hdnVisitHealthcareServiceUnitID.Value != "")
                    {
                        if (filterExpression != null && filterExpression != "")
                        {
                            filterExpression += " AND ";
                        }
                        filterExpression += string.Format("VisitHealthcareServiceUnitID = '{0}'", hdnVisitHealthcareServiceUnitID.Value);
                    }
                }
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvImagingResultHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_ITEM);
            }
            List<vImagingResultHd> lstEntity = BusinessLayer.GetvImagingResultHdList(filterExpression, Constant.GridViewPageSize.GRID_ITEM, pageIndex, "ID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}