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
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryVoidBill : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_VOID_BILL;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_VOID_BILL;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_VOID_BILL;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_SUMMARY_VOID_BILL;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.BILL_SUMMARY_VOID_BILL;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_SUMMARY_VOID_BILL;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.BILL_SUMMARY_VOID_BILL;
                    return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_VOID_BILL;
                default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_VOID_BILL;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID))[0];
            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            vRegistration2 entity = BusinessLayer.GetvRegistration2List(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
            EntityToControl(entity);

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DELETE_REASON));
            Methods.SetComboBoxField<StandardCode>(cboVoidReason, lstSc, "StandardCodeName", "StandardCodeID");
            cboVoidReason.SelectedIndex = 0;

            BindGridDetail();
        }

        private void BindGridDetail()
        {
            List<PatientBill> lst = BusinessLayer.GetPatientBillList(string.Format("RegistrationID = {0} AND TotalPatientPaymentAmount = 0 AND TotalPayerPaymentAmount = 0 AND GCTransactionStatus = '{1}'", hdnRegistrationID.Value, Constant.TransactionStatus.OPEN));
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void EntityToControl(vRegistration2 entity)
        {
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "voidbilling")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
                PatientPaymentDtVirtualDao patientPaymentDtVirtualDao = new PatientPaymentDtVirtualDao(ctx);
                PatientBillDao patientBillDao = new PatientBillDao(ctx);
                PatientBillDiscountDao patientBillDtDao = new PatientBillDiscountDao(ctx);
                RegistrationDao registrationDao = new RegistrationDao(ctx);
                RegistrationPayerDao registrationPayerDao = new RegistrationPayerDao(ctx);
                AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
                try
                {
                    bool isPassed = true;
                    decimal coverageLimit = 0;

                    AuditLog entityAuditLog = new AuditLog();
                    Registration entityReg = registrationDao.Get(AppSession.RegisteredPatient.RegistrationID);
                    RegistrationPayer oRegistrationPayer = BusinessLayer.GetRegistrationPayerList(string.Format("RegistrationID = {0} AND IsPrimaryPayer = 1 AND IsDeleted = 0", entityReg.RegistrationID), ctx).FirstOrDefault();
                    entityAuditLog.OldValues = JsonConvert.SerializeObject(entityReg);
                    string[] listParam = hdnParam.Value.Split('|');
                    foreach (string param in listParam)
                    {
                        int patientBillID = Convert.ToInt32(param);

                        PatientBill patientBill = patientBillDao.Get(patientBillID);
                        PatientPaymentDtVirtual entityDtVirtual = BusinessLayer.GetPatientPaymentDtVirtualList(string.Format("PatientBillingID = {0} AND IsDeleted = 0", patientBillID), ctx).FirstOrDefault();
                        if (patientBill.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            List<PatientBillDiscount> patientBillDiscountLst = BusinessLayer.GetPatientBillDiscountList(string.Format("PatientBillingID = {0}", patientBillID), ctx);
                            if (patientBill.TotalPatientPaymentAmount == 0 && patientBill.TotalPayerPaymentAmount == 0)
                            {
                                coverageLimit += patientBill.CoverageAmount;
                                patientBill.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                patientBill.GCVoidReason = cboVoidReason.Value.ToString();
                                patientBill.TotalPatientAmount = 0;
                                patientBill.TotalPayerAmount = 0;
                                if (patientBill.GCVoidReason == Constant.DeleteReason.OTHER)
                                {
                                    patientBill.VoidReason = txtVoidReason.Text;
                                }
                                patientBill.LastUpdatedBy = AppSession.UserLogin.UserID;
                                patientBill.LastUpdatedDate = DateTime.Now;
                                patientBillDao.Update(patientBill);

                                foreach (PatientBillDiscount patientBillDiscount in patientBillDiscountLst)
                                {
                                    if (!patientBillDiscount.IsDeleted)
                                    {
                                        patientBillDiscount.IsDeleted = true;
                                        patientBillDiscount.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        patientBillDtDao.Update(patientBillDiscount);
                                    }
                                }

                                List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PatientBillingID = {0}", patientBill.PatientBillingID), ctx);
                                foreach (PatientChargesHd patientChargesHd in lstPatientChargesHd)
                                {
                                    patientChargesHd.PatientBillingID = null;
                                    patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    patientChargesHdDao.Update(patientChargesHd);
                                }
                            }

                            if (entityDtVirtual != null && !entityDtVirtual.IsDeleted)
                            {
                                entityDtVirtual.IsDeleted = true;
                                entityDtVirtual.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtVirtual.LastUpdatedDate = DateTime.Now;
                                patientPaymentDtVirtualDao.Update(entityDtVirtual);
                            }

                        }
                        else
                        {
                            isPassed = false;
                            break;
                        }
                    }

                    if (isPassed)
                    {
                        entityReg.CoverageLimitAmount -= coverageLimit;
                        if (entityReg.GCCustomerType == Constant.CustomerType.BPJS)
                        {
                            entityReg.BPJSAmount -= coverageLimit;
                        }

                        entityAuditLog.ObjectType = Constant.BusinessObjectType.REGISTRATION;
                        entityAuditLog.NewValues = JsonConvert.SerializeObject(entityReg);
                        entityAuditLog.UserID = AppSession.UserLogin.UserID;
                        entityAuditLog.LogDate = DateTime.Now;
                        entityAuditLog.TransactionID = entityReg.RegistrationID;
                        entityAuditLogDao.Insert(entityAuditLog);
                        registrationDao.Update(entityReg);

                        if (oRegistrationPayer != null)
                        {
                            oRegistrationPayer.CoverageLimitAmount -= coverageLimit;
                            registrationPayerDao.Update(oRegistrationPayer);
                        }

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf, tagihan tidak dapat dibatalkan karena sudah diproses. Silahkan cek terlebih dahulu.";
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

                try
                {
                    if (result)
                    {
                        if (AppSession.IsBridgingToQueue)
                        {
                            //If Bridging to Queue - Send Information
                            if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT || hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC || hdnDepartmentID.Value == Constant.Facility.LABORATORY || hdnDepartmentID.Value == Constant.Facility.PHARMACY || hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                            {
                                try
                                {
                                    string[] listParam = hdnParam.Value.Split('|');
                                    foreach (string param in listParam)
                                    {
                                        int patientBillID = Convert.ToInt32(param);

                                        Registration oRegistration = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
                                        APIMessageLog entityAPILog = new APIMessageLog()
                                        {
                                            MessageDateTime = DateTime.Now,
                                            Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                                            Sender = Constant.BridgingVendor.HIS,
                                            IsSuccess = true
                                        };

                                        QueueService oService = new QueueService();
                                        string apiResult = oService.BAR_P05(AppSession.UserLogin.HealthcareID, oRegistration, "04",patientBillID);
                                        string[] apiResultInfo = apiResult.Split('|');
                                        if (apiResultInfo[0] == "0")
                                        {
                                            entityAPILog.IsSuccess = false;
                                            entityAPILog.MessageText = apiResultInfo[2];
                                            entityAPILog.Response = apiResult;
                                            entityAPILog.ErrorMessage = apiResultInfo[1];
                                            BusinessLayer.InsertAPIMessageLog(entityAPILog);

                                            Exception ex = new Exception(apiResultInfo[1]);
                                            Helper.InsertErrorLog(ex);
                                        }
                                        else
                                        {
                                            entityAPILog.MessageText = apiResultInfo[2];
                                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    Helper.InsertErrorLog(ex);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Helper.InsertErrorLog(ex);
                }

                return result;
            }
            return true;
        }
    }
}