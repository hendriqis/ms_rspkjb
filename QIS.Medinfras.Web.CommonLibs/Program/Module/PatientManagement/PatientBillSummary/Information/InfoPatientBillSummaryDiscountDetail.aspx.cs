using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfoPatientBillSummaryDiscountDetail : BasePageTrx
    {
        private string deptID = "";
        private string suID = "";
        private string filterDept = "IsActive = 1 AND IsHasRegistration = 1";
        private string filterSer = "IsDeleted = 0 AND IsUsingRegistration = 1";

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.INFORMATION_DISCOUNT_DETAIL;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.INFORMATION_DISCOUNT_DETAIL;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.INFORMATION_DISCOUNT_DETAIL;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.INFORMATION_DISCOUNT_DETAIL;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.INFORMATION_DISCOUNT_DETAIL;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.INFORMATION_DISCOUNT_DETAIL;
                    return Constant.MenuCode.MedicalDiagnostic.INFORMATION_DISCOUNT_DETAIL;
                default: return Constant.MenuCode.Outpatient.INFORMATION_DISCOUNT_DETAIL;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            //vRegistration4 entity = BusinessLayer.GetvRegistration4List(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID))[0];
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnLinkedRegistrationID.Value = AppSession.RegisteredPatient.LinkedRegistrationID.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            hdnBusinessPartnerID.Value = AppSession.RegisteredPatient.BusinessPartnerID.ToString();
            
            string roleID = "";

            List<UserInRole> lst = BusinessLayer.GetUserInRoleList(string.Format("HealthcareID = '{0}' AND UserID = {1}", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
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
                lstDepartment.Add(new Department { DepartmentID = "", DepartmentName = "" });
                Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");

                hdnFilterChargesHealthcareServiceUnitID.Value = filterSer;
            }
            else
            {
                List<Department> lstDepartment = BusinessLayer.GetDepartmentList(filterDept);
                lstDepartment.Add(new Department { DepartmentID = "", DepartmentName = "" });
                Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");

                hdnFilterChargesHealthcareServiceUnitID.Value = filterSer;
            }

            cboDepartment.Value = "";
            hdnCboChargesDepartmentID.Value = "";

            string filterItemType = string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ITEM_TYPE);
            List<StandardCode> lstItemType = BusinessLayer.GetStandardCodeList(filterItemType);
            lstItemType.Add(new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboGCItemType, lstItemType, "StandardCodeName", "StandardCodeID");
            cboGCItemType.Value = "";
            hdnCboGCItemType.Value = "";

            BindGridDetail();
        }

        private void BindGridDetail()
        {
            string filter = string.Format("((RegistrationID = {0} OR (LinkedToRegistrationID = {0} AND IsChargesTransfered = 1)) AND GCTransactionStatus NOT IN ('{1}') AND GCTransactionDetailStatus NOT IN ('{1}') AND IsDeleted = 0)",
                                                hdnRegistrationID.Value, Constant.TransactionStatus.VOID);

            if (hdnCboChargesDepartmentID.Value != "" && hdnCboChargesDepartmentID.Value != "0")
            {
                filter += " AND ChargesDepartmentID = '" + cboDepartment.Value + "'";
            }

            if (txtChargesServiceUnitCode.Text != "")
            {
                filter += " AND ChargesServiceUnitCode = '" + txtChargesServiceUnitCode.Text + "'";
            }

            if (txtChargesParamedicCode.Text != "")
            {
                filter += " AND ParamedicCode = '" + txtChargesParamedicCode.Text + "'";
            }

            if (hdnCboGCItemType.Value != null && hdnCboGCItemType.Value != "")
            {
                filter += string.Format(" AND GCItemType = '{0}'", hdnCboGCItemType.Value);
            }
            
            if (chkIsHasRevenueSharing.Checked)
            {
                filter += " AND IsAllowRevenueSharing = 1";
            }

            List<vPatientChargesDt10> lst = BusinessLayer.GetvPatientChargesDt10List(filter);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');

            BindGridDetail();
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }
    }
}