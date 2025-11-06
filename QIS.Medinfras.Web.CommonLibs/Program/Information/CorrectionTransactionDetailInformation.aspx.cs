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
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class CorrectionTransactionDetailInformation : BasePageTrx
    {
        protected string laboratoryTransactionCode = "";
        private string deptID = "";
        private string suID = "";
        private string filterDept = "IsActive = 1";
        private string filterSer = "IsDeleted = 0";

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        public override string OnGetMenuCode()
        {
            switch (hdnModuleID.Value)
            {
                case Constant.Module.INPATIENT: return Constant.MenuCode.Inpatient.CORRECTION_TRANSACTION_INFORMATION;
                case Constant.Module.EMERGENCY: return Constant.MenuCode.EmergencyCare.CORRECTION_TRANSACTION_INFORMATION;
                case Constant.Module.PHARMACY: return Constant.MenuCode.Pharmacy.CORRECTION_TRANSACTION_INFORMATION;
                case Constant.Module.MEDICAL_DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.CORRECTION_TRANSACTION_INFORMATION;
                case Constant.Module.LABORATORY: return Constant.MenuCode.Laboratory.CORRECTION_TRANSACTION_INFORMATION;
                case Constant.Module.IMAGING: return Constant.MenuCode.Imaging.CORRECTION_TRANSACTION_INFORMATION;
                default: return Constant.MenuCode.Outpatient.CORRECTION_TRANSACTION_INFORMATION;
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

                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(
                        "HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value)).FirstOrDefault();
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
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
            {
                filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
            }
            else
            {
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            }

            filterExpression += " AND IsCorrectionTransaction = 1";

            if (cboDepartment.Value != null)
            {
                if (cboDepartment.Value.ToString() != "")
                {
                    filterExpression += String.Format(" AND DepartmentID = '{0}'", cboDepartment.Value.ToString());
                }
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

            List<vPatientChargesHd> lst = BusinessLayer.GetvPatientChargesHdList(filterExpression);
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

        //protected override bool OnCustomButtonClick(string type, ref string errMessage)
        //{
        //    if (type == "changepatienttransactionstatus")
        //    {
        //        bool result = true;
        //        IDbContext ctx = DbFactory.Configure(true);
        //        PatientChargesHdDao entityDao = new PatientChargesHdDao(ctx);
        //        PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
        //        PatientBillDao patientBillDao = new PatientBillDao();
        //        string[] listParam = hdnParam.Value.Split('|');
        //        string lstTransactionID = "";
        //        foreach (string param in listParam)
        //        {
        //            if (lstTransactionID != "")
        //                lstTransactionID += ",";
        //            lstTransactionID += param;
        //        }
        //        try
        //        {
        //            List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID IN ({0}) AND GCTransactionStatus = '{1}'", lstTransactionID, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
        //            lstTransactionID = string.Join(",", lstPatientChargesHd.Select(p => p.TransactionID).ToList());
        //            List<PatientChargesDt> lstAllPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID IN ({0}) AND LocationID IS NOT NULL AND IsApproved = 1 AND IsDeleted = 0", lstTransactionID), ctx);

        //            foreach (PatientChargesHd patientChargesHd in lstPatientChargesHd)
        //            {
        //                if (patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
        //                {
        //                    patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
        //                    patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
        //                    entityDao.Update(patientChargesHd);

        //                    List<PatientChargesDt> lstPatientChargesDt = lstAllPatientChargesDt.Where(p => p.TransactionID == patientChargesHd.TransactionID).ToList();
        //                    foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
        //                    {
        //                        patientChargesDt.IsApproved = false;
        //                        patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
        //                        entityDtDao.Update(patientChargesDt);
        //                    }
        //                }
        //            }

        //            foreach (string param in listParam)
        //            {
        //                int transactionID = Convert.ToInt32(param);

        //                PatientChargesHd entity = entityDao.Get(transactionID);
        //                entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
        //                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
        //                entityDao.Update(entity);
        //            }
        //            ctx.CommitTransaction();
        //        }
        //        catch (Exception ex)
        //        {
        //            errMessage = ex.Message;
        //            result = false;
        //            ctx.RollBackTransaction();
        //        }
        //        finally
        //        {
        //            ctx.Close();
        //        }
        //        return result;
        //    }
        //    return true;
        //}
    }
}