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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ChangePatientTransactionStatusEntry : BasePageTrx
    {
        protected string laboratoryTransactionCode = "";
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
            switch (hdnModuleID.Value)
            {
                case Constant.Module.INPATIENT: return Constant.MenuCode.Inpatient.CHANGE_PATIENT_TRANSACTION_STATUS;
                case Constant.Module.EMERGENCY: return Constant.MenuCode.EmergencyCare.CHANGE_PATIENT_TRANSACTION_STATUS;
                case Constant.Module.PHARMACY: return Constant.MenuCode.Pharmacy.CHANGE_PATIENT_TRANSACTION_STATUS;
                case Constant.Module.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.CHANGE_PATIENT_TRANSACTION_STATUS;
                case Constant.Module.MEDICAL_DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.CHANGE_PATIENT_TRANSACTION_STATUS;
                case Constant.Module.LABORATORY: return Constant.MenuCode.Laboratory.CHANGE_PATIENT_TRANSACTION_STATUS;
                case Constant.Module.IMAGING: return Constant.MenuCode.Imaging.CHANGE_PATIENT_TRANSACTION_STATUS;
                default: return Constant.MenuCode.Outpatient.CHANGE_PATIENT_TRANSACTION_STATUS;
            }
        }

        protected override void InitializeDataControl()
        {
            string moduleName = Helper.GetModuleName();
            string moduleID = Helper.GetModuleID(moduleName);
            hdnModuleID.Value = moduleID;

            laboratoryTransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                hdnHealthcareServiceUnitID.Value = param[0];
                hdnVisitID.Value = param[1];

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
                hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnDepartmentID.Value = entity.DepartmentID;

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
                    List<vServiceUnitUserRole> lstServiceUnit = BusinessLayer.GetvServiceUnitUserRoleList(string.Format("HealthcareID = '{0}' AND RoleID IN ({1}) AND IsDeleted = 0",
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
                    Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");

                    hdnFilterServiceUnitID.Value = filterSer;
                }
                else
                {
                    List<Department> lstDepartment = BusinessLayer.GetDepartmentList(filterDept);
                    Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");

                    hdnFilterServiceUnitID.Value = filterSer;
                }

                cboDepartment.SelectedIndex = 0;

                BindGridDetail();
            }
        }

        private void BindGridDetail()
        {

            string filterExpression = "";

            filterExpression = string.Format("(RegistrationID = {0} OR (LinkedToRegistrationID = {0} AND IsChargesTransfered = 1)) AND GCTransactionStatus IN ('{1}')",
                                                hdnRegistrationID.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);

            if (cboDepartment.Value != "" && cboDepartment.Value != null)
            {
                filterExpression += String.Format(" AND DepartmentID = '{0}'", cboDepartment.Value.ToString());
            }

            if (!String.IsNullOrEmpty(hdnServiceUnitOrderID.Value.ToString()))
            {
                filterExpression += String.Format(" AND HealthcareServiceUnitID = {0}", Convert.ToInt32(hdnServiceUnitOrderID.Value.ToString()));
            }
            else
            {
                if (suID != "" && suID != null)
                {
                    string hsu = "";
                    List<vHealthcareServiceUnit> lstHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("ServiceUnitID IN ({0})", suID));
                    foreach (vHealthcareServiceUnit hsuDt in lstHSU)
                    {
                        hsu += "," + hsuDt.HealthcareServiceUnitID;
                    }
                    hsu = hsu.Substring(1, hsu.Length - 1);

                    filterExpression += String.Format(" AND HealthcareServiceUnitID IN ({0})", hsu);
                }
            }

            filterExpression += " ORDER BY TransactionID DESC";

            List<vPatientChargesHd3> lst = BusinessLayer.GetvPatientChargesHd3List(filterExpression);
            lvwView.DataSource = lst;
            lvwView.DataBind();
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