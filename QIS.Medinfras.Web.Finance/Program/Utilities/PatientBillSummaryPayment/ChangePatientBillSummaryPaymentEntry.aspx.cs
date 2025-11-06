using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Controls;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ChangePatientBillSummaryPaymentEntry : BasePageTrx
    {
        private string deptID = "";
        private string suID = "";
        private string filterDept = "IsActive = 1 AND IsHasRegistration = 1";
        private string filterSer = "IsDeleted = 0 AND IsUsingRegistration = 1";

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.FN_CHANGE_PAYMENT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            MenuMaster oMenu = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            hdnPageTitle.Value = oMenu.MenuCaption;

            string moduleName = Helper.GetModuleName();
            string moduleID = Helper.GetModuleID(moduleName);
            hdnModuleID.Value = moduleID;

            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                hdnVisitID.Value = param[0];
                hdnHealthcareServiceUnitID.Value = param[1];

                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(
                        "HealthcareServiceUnitID = {0} AND IsDeleted = 0 AND IsUsingRegistration = 1", hdnHealthcareServiceUnitID.Value)).FirstOrDefault();
                hdnDepartmentID.Value = hsu.DepartmentID;
                hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
                hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                EntityToControl(entity);

                string roleID = "";

                List<UserInRole> lst = BusinessLayer.GetUserInRoleList(string.Format(
                        "HealthcareID = '{0}' AND UserID = {1}", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
                foreach (UserInRole uir in lst)
                {
                    roleID += "," + uir.RoleID;
                }
                roleID = roleID.Substring(1, roleID.Length - 1);

                if (roleID != "")
                {
                    List<vServiceUnitUserRole> lstServiceUnit = BusinessLayer.GetvServiceUnitUserRoleList(string.Format("HealthcareID = '{0}' AND RoleID IN ({1})",
                            AppSession.UserLogin.HealthcareID, roleID));
                    foreach (vServiceUnitUserRole sur in lstServiceUnit)
                    {
                        deptID += "," + "'" + sur.DepartmentID + "'";
                        suID += "," + "'" + sur.ServiceUnitID + "'";
                    }

                    if (lstServiceUnit.Count > 0)
                    {
                        deptID = deptID.Substring(1, deptID.Length - 1);
                        filterDept += string.Format(" AND DepartmentID IN ({0})", deptID);

                        suID = suID.Substring(1, suID.Length - 1);
                        filterSer += string.Format(" AND ServiceUnitID IN ({0})", suID);
                    }


                    List<Department> lstDepartment = BusinessLayer.GetDepartmentList(filterDept);
                    //Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");

                    hdnFilterServiceUnitID.Value = filterSer;
                }

                BindGridDetail();
            }
        }

        private void BindGridDetail()
        {
            string filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus = '{1}' AND PaymentID IN (SELECT PaymentID FROM PatientPaymentDt WHERE IsDeleted = 0 AND GCPaymentMethod IN ('{2}','{3}'))",
                                                        hdnRegistrationID.Value,
                                                        Constant.TransactionStatus.OPEN,
                                                        Constant.PaymentMethod.DEBIT_CARD,
                                                        Constant.PaymentMethod.CREDIT_CARD
                                                    );

            List<vPatientPaymentHd> lst = BusinessLayer.GetvPatientPaymentHdList(filterExpression);
            grdView.DataSource = lst;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void EntityToControl(vConsultVisit2 entity)
        {
            ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }
    }
}