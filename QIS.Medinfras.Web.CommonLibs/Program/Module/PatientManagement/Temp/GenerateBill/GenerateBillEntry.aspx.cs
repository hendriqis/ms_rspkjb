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
    public partial class GenerateBillEntry : BasePageTrx
    {
        protected string laboratoryTransactionCode = "";
        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        public override string OnGetMenuCode()
        {
            //switch (hdnDepartmentID.Value)
            //{
            //    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.GENERATE_BILL;
            //    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.GENERATE_BILL;
            //    case Constant.Facility.DIAGNOSTIC:
            //        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
            //            return Constant.MenuCode.Laboratory.GENERATE_BILL;
            //        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
            //            return Constant.MenuCode.Imaging.GENERATE_BILL;
            //        return Constant.MenuCode.MedicalDiagnostic.GENERATE_BILL;
            //    default: return Constant.MenuCode.Outpatient.GENERATE_BILL;
            //}
            return "";
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                laboratoryTransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
                hdnRegistrationID.Value = Page.Request.QueryString["id"];
                vRegistration3 entity = BusinessLayer.GetvRegistration3List(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
                EntityToControl(entity);

                BindGridDetail();
            }
        }

        private void BindGridDetail()
        {
            List<vPatientChargesHd> lst = BusinessLayer.GetvPatientChargesHdList(string.Format("RegistrationID = {0} AND GCTransactionStatus = '{1}'", hdnRegistrationID.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL));
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void EntityToControl(vRegistration3 entity)
        {
            ((PatientBannerCtl) ctlPatientBanner).InitializePatientBanner(entity);
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "generatebill")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PatientChargesHdDao entityDao = new PatientChargesHdDao(ctx);
                PatientBillDao patientBillDao = new PatientBillDao();
                try
                {
                    PatientBill patientBill = new PatientBill();
                    patientBill.BillingDate = DateTime.Now;
                    patientBill.BillingTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    patientBill.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                    string transactionCode = "";
                    switch (hdnDepartmentID.Value)
                    {
                        case Constant.Facility.INPATIENT: transactionCode = Constant.TransactionCode.IP_PATIENT_BILL; break;
                        case Constant.Facility.EMERGENCY: transactionCode = Constant.TransactionCode.ER_PATIENT_BILL; break;
                        case Constant.Facility.DIAGNOSTIC:
                            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                                transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_BILL;
                            else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                transactionCode = Constant.TransactionCode.IMAGING_PATIENT_BILL;
                            transactionCode = Constant.TransactionCode.ER_PATIENT_BILL; break;
                        default: transactionCode = Constant.TransactionCode.OTHER_PATIENT_BILL; break;
                    }
                    patientBill.PatientBillingNo = BusinessLayer.GenerateTransactionNo(transactionCode, patientBill.BillingDate, ctx);
                    patientBill.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    patientBill.TotalPatientAmount = Convert.ToDecimal(hdnTotalPatientAmount.Value);
                    patientBill.TotalPayerAmount = Convert.ToDecimal(hdnTotalPayerAmount.Value);
                    patientBill.TotalAmount = Convert.ToDecimal(hdnTotalAmount.Value);
                    patientBill.GCVoidReason = null;
                    patientBill.VoidReason = null;
                    patientBill.LastUpdatedBy = patientBill.CreatedBy = AppSession.UserLogin.UserID;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    patientBillDao.Insert(patientBill);
                    patientBill.PatientBillingID = BusinessLayer.GetPatientBillMaxID(ctx);

                    string[] listParam = hdnParam.Value.Split('|');
                    foreach (string param in listParam)
                    {
                        int transactionID = Convert.ToInt32(param);

                        PatientChargesHd entity = entityDao.Get(transactionID);
                        entity.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        entity.PatientBillingID = patientBill.PatientBillingID;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);
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