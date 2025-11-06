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
using System.Data;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PrescriptionStatusEdit2 : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        private string refreshGridInterval = "10";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.PRESCRIPTION_STATUS_EDIT;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override void InitializeDataControl()
        {
            OnControlEntrySetting();
            txtPrescriptionDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            BindGridView(1, true, ref PageCount);
        }

        protected override void OnControlEntrySetting() 
        {
            string filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsUsingRegistration = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.PHARMACY);

            List<vHealthcareServiceUnit> lstHServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID",DevExpress.Web.ASPxEditors.DropDownStyle.DropDownList);
            cboServiceUnit.SelectedIndex = 0;

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration = 1 AND IsActive = 1 AND DepartmentID != '{0}'", Constant.Facility.PHARMACY));
            lstDepartment.Insert(0, new Department { DepartmentName = string.Format("{0}", GetLabel(" ")) });
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;
        }

        private string GetFilterExpression()
        {
            string filterExpression = String.Format("DispensaryServiceUnitID = {0} AND GCTransactionStatus = '{1}'",cboServiceUnit.Value.ToString(),  Constant.TransactionStatus.PROCESSED);
            if (!chkIsIgnoreDate.Checked) filterExpression += string.Format(" AND PrescriptionDate = '{0}'", Helper.GetDatePickerValue(txtPrescriptionDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            if (cboDepartment.Value != null) filterExpression += String.Format(" AND DepartmentID = '{0}'",cboDepartment.Value);

            if (hdnFilterExpressionQuickSearch.Value == "Search")
                hdnFilterExpressionQuickSearch.Value = " ";
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            else
                filterExpression += string.Format("");
            switch (hdnContentID.Value) 
            {
                case "containerProses":
                    filterExpression += String.Format("AND GCTransactionStatus NOT IN ('{0}','{1}') AND GCOrderStatus = '{2}'",Constant.TransactionStatus.OPEN,Constant.TransactionStatus.VOID, Constant.OrderStatus.IN_PROGRESS);
                    break;
                case "containerDone":
                    filterExpression += String.Format("AND GCTransactionStatus NOT IN ('{0}','{1}') AND GCOrderStatus = '{2}'",Constant.TransactionStatus.OPEN,Constant.TransactionStatus.VOID, Constant.OrderStatus.COMPLETED);
                    break;
                default :
                    filterExpression += String.Format("AND GCTransactionStatus NOT IN ('{0}','{1}') AND GCOrderStatus = '{2}'",Constant.TransactionStatus.OPEN,Constant.TransactionStatus.VOID, Constant.OrderStatus.RECEIVED);
                    break;
            }
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPrescriptionOrderHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPrescriptionOrderHd> lstPrescriptionOrderHD = BusinessLayer.GetvPrescriptionOrderHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
           // grdView.DataSource = lstPrescriptionOrderHD;
           // grdView.DataBind();
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

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnLstSelected.Value.ToString() != "")
                {
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveEditRecordEntityDt(ref String errMessage)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityOrderHdDao = new PrescriptionOrderHdDao(ctx);
            
            try
            {
                List<PrescriptionOrderHd> lstEntity = BusinessLayer.GetPrescriptionOrderHdList(String.Format("PrescriptionOrderID IN ({0})",hdnLstSelected.Value));
                foreach (PrescriptionOrderHd obj in lstEntity) 
                {
                    switch (obj.GCOrderStatus) 
                    {
                        case Constant.TestOrderStatus.RECEIVED:
                            obj.StartDate = DateTime.Now.Date;
                            obj.StartTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                            obj.GCOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                            break;
                        case Constant.TestOrderStatus.IN_PROGRESS:
                            obj.CompleteDate = DateTime.Now.Date;
                            obj.CompleteTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                            obj.GCOrderStatus = Constant.TestOrderStatus.COMPLETED;
                            break;
                        case Constant.TestOrderStatus.COMPLETED:
                            obj.ClosedDate = DateTime.Now.Date;
                            obj.ClosedTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                            obj.GCOrderStatus = Constant.TestOrderStatus.CLOSED;
                            break;
                    }
                    entityOrderHdDao.Update(obj);
                }
                ctx.CommitTransaction();
            }
            catch(Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }

    }
}