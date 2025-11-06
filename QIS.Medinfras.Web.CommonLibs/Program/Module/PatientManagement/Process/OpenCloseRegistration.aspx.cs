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
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class OpenCloseRegistration : BasePageTrx
    {
        protected string GetErrorMsgOpenedRegistration()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_OPENED_REGISTRATION_VALIDATION);
        }
        protected string GetErrorMsgClosedRegistration()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_CLOSED_REGISTRATION_VALIDATION);
        }
        protected string GetErrorMsgSelectRegistrationFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_REGISTRATION_FIRST_VALIDATION);
        }

        private const string LABORATORY = "LABORATORY";
        private const string IMAGING = "IMAGING";
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.OPEN_CLOSE_PATIENT_REGISTRATION;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.OPEN_CLOSE_PATIENT_REGISTRATION;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.OPEN_CLOSE_PATIENT_REGISTRATION;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.OPEN_CLOSE_PATIENT_REGISTRATION;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.OPEN_CLOSE_PATIENT_REGISTRATION;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.OPEN_CLOSE_PATIENT_REGISTRATION;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.OPEN_CLOSE_PATIENT_REGISTRATION;
                    return Constant.MenuCode.MedicalDiagnostic.OPEN_CLOSE_PATIENT_REGISTRATION;
                default: return Constant.MenuCode.Outpatient.OPEN_CLOSE_PATIENT_REGISTRATION;
            }
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        private GetUserMenuAccess menu;
        protected override void InitializeDataControl()
        {
            hdnDepartmentID.Value = Page.Request.QueryString["id"];
            if (hdnDepartmentID.Value == LABORATORY || hdnDepartmentID.Value == IMAGING)
                hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;
            //InitializeControlProperties();
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            SettingParameterDt setpar = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_IS_CHECK_TEST_RESULT);
            hdnIsCheckResultTest.Value = setpar.ParameterValue;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtRegistrationNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPayer, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsClose, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRegistrationTime, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtServiceUnit, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(txtMRN, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtPatientName, new ControlEntrySetting(false, false, false));
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "closeregistration")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                RegistrationDao entityDao = new RegistrationDao(ctx);
                ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
                TestOrderHdDao testOrderHdDao = new TestOrderHdDao(ctx);
                ServiceOrderHdDao serviceOrderHdDao = new ServiceOrderHdDao(ctx);
                AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
                try
                {
                    if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                    {
                        int count = BusinessLayer.GetvBedRowCount(string.Format("RegistrationID = {0}", hdnRegistrationID.Value));
                        if (count > 0)
                        {
                            errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_DISCHARGE_REGISTRATION_VALIDATION);
                            result = false;
                        }
                    }
                    if (result)
                    {
                        List<vRegistrationOutstandingInfo> lstOutstanding = BusinessLayer.GetvRegistrationOutstandingInfoList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value), ctx);

                        foreach (vRegistrationOutstandingInfo entityInfo in lstOutstanding)
                        {
                            bool outstanding = (entityInfo.ServiceOrder + entityInfo.PrescriptionOrder + entityInfo.PrescriptionReturnOrder + entityInfo.LaboratoriumOrder + entityInfo.RadiologiOrder + entityInfo.OtherOrder + entityInfo.Charges + entityInfo.Billing > 0);

                            if (!outstanding)
                            {
                                hdnIsHasOutstandingOrder.Value = "0";
                            }
                            else
                            {
                                hdnIsHasOutstandingOrder.Value = "1";
                            }
                        }

                        if (hdnIsHasOutstandingOrder.Value == "1")
                        {
                            errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_FINISH_TRANSACTION_VALIDATION);
                            result = false;
                        }
                        else
                        {
                            AuditLog entityAuditLog = new AuditLog();
                            Registration entity = entityDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                            entityAuditLog.OldValues = JsonConvert.SerializeObject(entity);

                            entity.GCRegistrationStatus = Constant.VisitStatus.CLOSED;
                            entity.ClosedBy = AppSession.UserLogin.UserID;
                            entity.ClosedDate = DateTime.Now;
                            entity.ClosedTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                            entity.IsBillingClosed = true;
                            entity.BillingClosedBy = AppSession.UserLogin.UserID;
                            entity.BillingClosedDate = DateTime.Now;
                            entity.IsBillingReopen = false;
                            entity.BillingReopenBy = null;
                            entity.BillingReopenDate = null;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);

                            entityAuditLog.ObjectType = Constant.BusinessObjectType.REGISTRATION;
                            entityAuditLog.NewValues = JsonConvert.SerializeObject(entity);
                            entityAuditLog.UserID = AppSession.UserLogin.UserID;
                            entityAuditLog.LogDate = DateTime.Now;
                            entityAuditLog.TransactionID = entity.RegistrationID;
                            entityAuditLogDao.Insert(entityAuditLog);

                            List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND GCVisitStatus != '{1}'", entity.RegistrationID, Constant.VisitStatus.CANCELLED), ctx);
                            foreach (ConsultVisit consultVisit in lstConsultVisit)
                            {
                                List<TestOrderHd> lstTestOrderHd = BusinessLayer.GetTestOrderHdList(string.Format("VisitID IN ({0}) AND GCTransactionStatus IN ('{1}','{2}')", consultVisit.VisitID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
                                foreach (TestOrderHd testOrderHd in lstTestOrderHd)
                                {
                                    testOrderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    testOrderHd.VoidBy = AppSession.UserLogin.UserID;
                                    testOrderHd.VoidDate = DateTime.Now;
                                    testOrderHd.GCVoidReason = Constant.DeleteReason.OTHER;
                                    testOrderHd.VoidReason = "Void dari Proses Harian Tutup Pendaftaran.";
                                    testOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    testOrderHdDao.Update(testOrderHd);
                                }

                                List<ServiceOrderHd> lstServiceOrderHd = BusinessLayer.GetServiceOrderHdList(string.Format("VisitID IN ({0}) AND GCTransactionStatus IN ('{1}','{2}')", consultVisit.VisitID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
                                foreach (ServiceOrderHd serviceOrderHd in lstServiceOrderHd)
                                {
                                    serviceOrderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    serviceOrderHd.VoidBy = AppSession.UserLogin.UserID;
                                    serviceOrderHd.VoidDate = DateTime.Now;
                                    serviceOrderHd.GCVoidReason = Constant.DeleteReason.OTHER;
                                    serviceOrderHd.VoidReason = "Void dari Proses Harian Tutup Pendaftaran.";
                                    serviceOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    serviceOrderHdDao.Update(serviceOrderHd);
                                }

                                consultVisit.GCVisitStatus = Constant.VisitStatus.CLOSED;
                                consultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                                consultVisitDao.Update(consultVisit);
                            }
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
            else if (type == "openregistration")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                RegistrationDao entityDao = new RegistrationDao(ctx);
                ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
                AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
                try
                {
                    if (hdnDepartmentID.Value != Constant.Facility.INPATIENT)
                    {
                        List<Registration> lstReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0} AND IsChargesTransfered = 1", hdnRegistrationID.Value));
                        int count = lstReg.Count;
                        if (count > 0)
                        {
                            Registration reg = BusinessLayer.GetRegistration(Convert.ToInt32(lstReg.FirstOrDefault().LinkedToRegistrationID));
                            //errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_TRANFERRED_REGISTRATION);
                            errMessage = string.Format("Tagihan Pasien telah ditransfer ke registrasi {0}. Tidak dapat diubah lagi status pendaftarannya", reg.RegistrationNo);
                            result = false;
                        }
                    }

                    if (result)
                    {
                        string registrationStatus = "";
                        if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                        {
                            registrationStatus = Constant.VisitStatus.DISCHARGED;
                        }
                        else
                        {
                            registrationStatus = Constant.VisitStatus.CHECKED_IN;
                        }

                        AuditLog entityAuditLog = new AuditLog();
                        Registration entity = entityDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                        entityAuditLog.OldValues = JsonConvert.SerializeObject(entity);

                        List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND GCVisitStatus != '{1}'", entity.RegistrationID, Constant.VisitStatus.CANCELLED), ctx);
                        foreach (ConsultVisit consultVisit in lstConsultVisit)
                        {
                            if (consultVisit.DischargeDate != null && consultVisit.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                            {
                                registrationStatus = Constant.VisitStatus.DISCHARGED;
                            }
                            else if (consultVisit.PhysicianDischargedBy != null && consultVisit.PhysicianDischargedBy != 0)
                            {
                                registrationStatus = Constant.VisitStatus.PHYSICIAN_DISCHARGE;
                            }
                            
                            consultVisit.GCVisitStatus = registrationStatus;
                            consultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                            consultVisitDao.Update(consultVisit);
                        }

                        entity.GCRegistrationStatus = registrationStatus;
                        entity.ClosedBy = entity.ClosedBy; //save last user ClosedBy
                        entity.ClosedDate = DateTime.Parse(String.Format("1900-01-01", DateTime.Now));
                        entity.ClosedTime = "00:00";
                        entity.IsBillingClosed = false;
                        entity.BillingClosedBy = null;
                        entity.BillingClosedDate = null;
                        entity.IsBillingReopen = true;
                        entity.BillingReopenBy = AppSession.UserLogin.UserID;
                        entity.BillingReopenDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);

                        entityAuditLog.ObjectType = Constant.BusinessObjectType.REGISTRATION;
                        entityAuditLog.NewValues = JsonConvert.SerializeObject(entity);
                        entityAuditLog.UserID = AppSession.UserLogin.UserID;
                        entityAuditLog.LogDate = DateTime.Now;
                        entityAuditLog.TransactionID = entity.RegistrationID;
                        entityAuditLogDao.Insert(entityAuditLog);

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