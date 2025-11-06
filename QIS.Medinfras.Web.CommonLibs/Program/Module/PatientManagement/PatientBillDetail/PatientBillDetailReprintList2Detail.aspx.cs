using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillDetailReprintList2Detail : BasePageTrx
    {
        protected string laboratoryTransactionCode = "";
        private string deptID = "";
        private string suID = "";
        private string filterDept = "IsActive = 1";
        private string filterSer = "IsDeleted = 0";

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"].Split('|')[0];
            hdnRequestID.Value = id;
            switch (id)
            {
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.PATIENT_BILL_DETAIL_REPRINT_2_DETAIL;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_BILL_DETAIL_REPRINT_2_DETAIL;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.PATIENT_BILL_DETAIL_REPRINT_2_DETAIL;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.PATIENT_BILL_DETAIL_REPRINT_2_DETAIL;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.PATIENT_BILL_DETAIL_REPRINT_2_DETAIL;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.PATIENT_BILL_DETAIL_REPRINT_2_DETAIL;
                    return Constant.MenuCode.MedicalDiagnostic.PATIENT_BILL_DETAIL_REPRINT_2_DETAIL;
                default: return Constant.MenuCode.EmergencyCare.PATIENT_BILL_DETAIL_REPRINT_2_DETAIL;
            }
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            laboratoryTransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                hdnVisitID.Value = param[1];

                vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
                hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();

                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(
                        "HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value)).FirstOrDefault();
                hdnDepartmentID.Value = hsu.DepartmentID;
                hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();

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

                //BindGridDetail();
            }
        }

        private void BindGridDetail()
        {
            string filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
            {
                filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
            }
            else
            {
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            }

            filterExpression += string.Format(" AND GCTransactionStatus != '{0}'", Constant.TransactionStatus.VOID);

            if (cboDepartment.Value != "" && cboDepartment.Value != null)
            {
                filterExpression += String.Format(" AND DepartmentID = '{0}'", cboDepartment.Value.ToString());
            }

            if (!String.IsNullOrEmpty(hdnServiceUnitID.Value.ToString()))
            {
                filterExpression += String.Format(" AND HealthcareServiceUnitID = {0}", Convert.ToInt32(hdnServiceUnitID.Value.ToString()));
            }
            else
            {
                if (suID != "" && suID != null)
                {
                    string hsu = "";
                    List<vHealthcareServiceUnit> lstHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("ServiceUnitID IN ({0})", suID));
                    foreach(vHealthcareServiceUnit hsuDt in lstHSU)
                    {
                        hsu += "," + hsuDt.HealthcareServiceUnitID;
                    }
                    hsu = hsu.Substring(1, hsu.Length - 1);

                    filterExpression += String.Format(" AND HealthcareServiceUnitID IN ({0})", hsu);
                }
            }

            List<vPatientChargesHd> lst = BusinessLayer.GetvPatientChargesHdList(filterExpression);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }
    }
}