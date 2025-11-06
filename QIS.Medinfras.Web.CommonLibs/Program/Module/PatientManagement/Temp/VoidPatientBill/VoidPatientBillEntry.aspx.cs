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
    public partial class VoidPatientBillEntry : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            //switch (hdnDepartmentID.Value)
            //{
            //    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.VOID_PATIENT_BILL;
            //    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.VOID_PATIENT_BILL;
            //    case Constant.Facility.DIAGNOSTIC:
            //        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
            //            return Constant.MenuCode.Laboratory.VOID_PATIENT_BILL;
            //        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
            //            return Constant.MenuCode.Imaging.VOID_PATIENT_BILL;
            //        return Constant.MenuCode.MedicalDiagnostic.VOID_PATIENT_BILL;
            //    default: return Constant.MenuCode.Outpatient.VOID_PATIENT_BILL;
            //}
            return "";
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                hdnRegistrationID.Value = Page.Request.QueryString["id"];
                vRegistration3 entity = BusinessLayer.GetvRegistration3List(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
                EntityToControl(entity);

                List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DELETE_REASON));
                Methods.SetComboBoxField<StandardCode>(cboVoidReason, lstSc, "StandardCodeName", "StandardCodeID");
                cboVoidReason.SelectedIndex = 0;

                BindGridDetail();
            }
        }

        private void BindGridDetail()
        {
            List<PatientBill> lst = BusinessLayer.GetPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus = '{1}'", hdnRegistrationID.Value, Constant.TransactionStatus.OPEN));
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void EntityToControl(vRegistration3 entity)
        {
            ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "transactionverification")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
                PatientBillDao patientBillDao = new PatientBillDao(ctx);
                try
                {
                    string[] listParam = hdnParam.Value.Split('|');
                    foreach (string param in listParam)
                    {
                        int patientBillID = Convert.ToInt32(param);

                        PatientBill patientBill = patientBillDao.Get(patientBillID);
                        patientBill.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        patientBill.GCVoidReason = cboVoidReason.Value.ToString();
                        if (patientBill.GCVoidReason == Constant.DeleteReason.OTHER)
                            patientBill.VoidReason = txtVoidReason.Text;
                        patientBill.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientBill.LastUpdatedDate = DateTime.Now;
                        patientBillDao.Update(patientBill);

                        List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PatientBillingID = {0}", patientBill.PatientBillingID), ctx);
                        foreach (PatientChargesHd patientChargesHd in lstPatientChargesHd)
                        {
                            patientChargesHd.PatientBillingID = null;
                            patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientChargesHdDao.Update(patientChargesHd);
                        }
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
                return result;
            }
            return true;
        }
    }
}