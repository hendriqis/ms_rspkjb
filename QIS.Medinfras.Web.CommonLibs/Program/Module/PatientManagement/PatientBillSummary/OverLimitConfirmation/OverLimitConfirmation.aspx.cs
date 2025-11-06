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
    public partial class OverLimitConfirmation : BasePageTrx
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
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_OVER_LIMIT_CONFIRMATION;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_OVER_LIMIT_CONFIRMATION;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_OVER_LIMIT_CONFIRMATION;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_SUMMARY_OVER_LIMIT_CONFIRMATION;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.BILL_SUMMARY_OVER_LIMIT_CONFIRMATION;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_SUMMARY_OVER_LIMIT_CONFIRMATION;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.BILL_SUMMARY_OVER_LIMIT_CONFIRMATION;
                    return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_OVER_LIMIT_CONFIRMATION;
                default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_OVER_LIMIT_CONFIRMATION;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();

            laboratoryTransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(
                    "HealthcareServiceUnitID = {0} AND IsDeleted = 0 AND IsUsingRegistration = 1", hdnHealthcareServiceUnitID.Value)).FirstOrDefault();
            hdnDepartmentID.Value = hsu.DepartmentID;
            hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();

            vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
            hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
            hdnRegistrationID.Value = entity.RegistrationID.ToString();

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

        private void BindGridDetail()
        {

            string filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
            {
                filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1)) AND GCTransactionStatus IN ('{2}','{3}')", hdnVisitID.Value, hdnLinkedRegistrationID.Value, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            }
            else
            {
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
                filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}')", Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            }

            if (cboDepartment.Value.ToString() != "" && cboDepartment.Value != null)
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

            filterExpression += " ORDER BY TransactionID DESC, ID ASC";

            List<vPatientChargesDtControlLimit> lst = BusinessLayer.GetvPatientChargesDtControlLimitList(filterExpression);
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtInfoDao entityDao = new PatientChargesDtInfoDao(ctx);
            if (type == "confirm")
            {
                try
                {
                    string filterDtInfo = string.Format("ID IN ({0}) AND IsConfirmed = 0 AND IsUnitPriceOverLimit = 1", hdnParam.Value);
                    List<PatientChargesDtInfo> lst = BusinessLayer.GetPatientChargesDtInfoList(filterDtInfo);
                    if (lst.Count > 0)
                    {
                        foreach (PatientChargesDtInfo dt in lst)
                        {
                            dt.IsConfirmed = true;
                            dt.ConfirmedBy = AppSession.UserLogin.UserID;
                            dt.ConfirmedDate = DateTime.Now;
                            dt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityDao.Update(dt);
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Tidak ada transaksi yang dikonfirmasi.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
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
            }
            return result;
        }
    }
}