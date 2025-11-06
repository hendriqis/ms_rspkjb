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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransferOrderList : BasePageCheckRegisteredPatient
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            switch (hdnUnit.Value)
            {
                case Constant.Facility.LABORATORY: return Constant.MenuCode.Laboratory.TRANSFER_ORDER;
                default: return Constant.MenuCode.Laboratory.TRANSFER_ORDER;
            }
            
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override bool IsShowRightPanel()
        {
            return false;
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }

        private string refreshGridInterval = "10";

        protected override void InitializeDataControl()
        {
            hdnUnit.Value = Page.Request.QueryString["id"];

            string filterUnit = string.Format("IsUsingRegistration = 1 AND IsDeleted = 0");

            if (hdnUnit.Value == Constant.Facility.LABORATORY)
            {
                filterUnit += " AND IsLaboratoryUnit = 1";
            }

            List<vHealthcareServiceUnit> lstHServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterUnit);
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnitOrder, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboToServiceUnitOrder, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnitOrder.SelectedIndex = 1;

            string deptFilter = string.Format("IsHasRegistration = 1 AND IsActive = 1 AND DepartmentID != '{0}'", Constant.Facility.PHARMACY);
            if (hdnIsUsingUDD.Value == "1")
            {
                deptFilter = string.Format("IsHasRegistration = 1 AND IsActive = 1 AND DepartmentID NOT IN ('{0}','{1}')", Constant.Facility.PHARMACY, Constant.Facility.INPATIENT);
            }

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList(deptFilter);
            List<Department> lstDepartmentOrder = BusinessLayer.GetDepartmentList(deptFilter);
            lstDepartmentOrder.Insert(0, new Department { DepartmentName = string.Format("{0}", GetLabel(" ")) });
            Methods.SetComboBoxField<Department>(cboDepartmentOrder, lstDepartmentOrder, "DepartmentName", "DepartmentID");
            cboDepartmentOrder.SelectedIndex = 0;

            txtTestOrderDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            SettingControlProperties();
            grdOrderPatient.InitializeControl();

            Helper.SetControlEntrySetting(txtTestOrderDate, new ControlEntrySetting(true, true, false), "mpPatientListOrder");
            Helper.SetControlEntrySetting(cboDepartmentOrder, new ControlEntrySetting(true, true, false), "mpPatientListOrder");

            hdnPageTitle.Value = BusinessLayer.GetvMenuList(string.Format("MenuCode = '{0}'", this.OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        private void SettingControlProperties()
        {
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;

            if (filterExpression != "")
                filterExpression += " AND ";

            filterExpression += string.Format("ScheduledDate = '{0}' AND GCVisitStatus NOT IN ('{2}','{3}','{4}') AND GCTransactionStatus IN ('{5}','{6}')",
                Helper.GetDatePickerValue(txtTestOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                cboServiceUnitOrder.Value, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.APPROVED);

            if (cboServiceUnitOrder.Value.ToString() != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnitOrder.Value);
            if (cboDepartmentOrder.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartmentOrder.Value);
            if (hdnServiceUnitIDOrder.Value != "0" && hdnServiceUnitIDOrder.Value != "")
                filterExpression += string.Format(" AND VisitHSUID = '{0}'", hdnServiceUnitIDOrder.Value);
            if (hdnFilterExpressionQuickSearchOrder.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchOrder.Value);
            return filterExpression;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "transfer")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);
                try
                {
                    List<TestOrderHd> lstOrder = BusinessLayer.GetTestOrderHdList(string.Format("TestOrderID IN ({0})", hdnParam.Value), ctx);

                    DateTime logDate = DateTime.Now.Date;
                    string logTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                    foreach (TestOrderHd order in lstOrder)
                    {
                        TestOrderHd oHeader = orderHdDao.Get(order.TestOrderID);
                        if ((oHeader.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL))
                        {
                            oHeader.HealthcareServiceUnitID = Convert.ToInt32(cboToServiceUnitOrder.Value.ToString());
                            oHeader.LastUpdatedBy = AppSession.UserLogin.UserID;
                            orderHdDao.Update(oHeader);
                        }
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
                return result;
            }
            return false;
        }
    }
}