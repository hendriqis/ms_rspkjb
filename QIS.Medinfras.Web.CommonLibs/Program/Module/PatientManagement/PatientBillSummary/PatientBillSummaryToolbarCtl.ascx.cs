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
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryToolbarCtl : BaseUserControlCtl
    {
        string menuCode = "";
        IDbContext ctx = DbFactory.Configure();
        List<GetUserMenuAccess> lstMenu = null;
        List<GetPatientChargesHdPerRegistration> lstEntityPatientChargesHd = null;
        int healthcareServiceUnitID = 0;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
                hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
                healthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;

                string moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);
                string linkedRegistration = AppSession.RegisteredPatient.LinkedRegistrationID.ToString();

                hdnModuleIDToolbar.Value = AppSession.RegisteredPatient.OpenFromModuleID;

                string[] url = AppConfigManager.QISAppVirtualDirectory.Split('/');
                hdnUrlBack.Value = string.Format("/{0}/{1}/", url[3], url[4]);

                hdnLinkedRegistrationID.Value = linkedRegistration;
                menuCode = ((BasePageContent)Page).OnGetMenuCode();

                GetSettingParameterDt();

                string filterExpression = string.Format("(RegistrationID = {0} OR (LinkedToRegistrationID = {0} AND IsChargesTransfered = 1))", hdnRegistrationID.Value);

                lstEntityPatientChargesHd = BusinessLayer.GetPatientChargesHdPerRegistration(Convert.ToInt32(hdnRegistrationID.Value), ctx);
                string parentCode = "";
                switch (ModuleID)
                {
                    case "OP": parentCode = Constant.MenuCode.Outpatient.BILL_SUMMARY; break;
                    case "IP": parentCode = Constant.MenuCode.Inpatient.BILL_SUMMARY; break;
                    case "ER": parentCode = Constant.MenuCode.EmergencyCare.BILL_SUMMARY; break;
                    case "LB": parentCode = Constant.MenuCode.Laboratory.BILL_SUMMARY; break;
                    case "IS": parentCode = Constant.MenuCode.Imaging.BILL_SUMMARY; break;
                    case "MD": parentCode = Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY; break;
                    case "PH": parentCode = Constant.MenuCode.Pharmacy.BILL_SUMMARY; break;
                    case "MC": parentCode = Constant.MenuCode.MedicalCheckup.BILL_SUMMARY; break;
                    case "RT": parentCode = Constant.MenuCode.Radiotheraphy.BILL_SUMMARY; break;
                }

                lstMenu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("(ParentCode = '{0}' OR ParentID IN (SELECT MenuID FROM Menu WHERE ParentID = (SELECT MenuID FROM Menu WHERE MenuCode = '{0}')))", parentCode));
                rptHeader.DataSource = lstMenu.Where(p => p.ParentCode == parentCode).OrderBy(p => p.MenuIndex).ToList();
                rptHeader.DataBind();

                GetUserMenuAccess selectedMenu = lstMenu.FirstOrDefault(p => p.MenuCode == menuCode);
                rptMenuChild.DataSource = lstMenu.Where(p => p.ParentID == selectedMenu.ParentID).OrderBy(p => p.MenuIndex).ToList();
                rptMenuChild.DataBind();

                TogglePatientNotification();
            }
        }

        private void GetSettingParameterDt()
        {
            string filterSetvar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                                    AppSession.UserLogin.HealthcareID,
                                                    Constant.SettingParameter.FN_IS_CHECK_TEST_RESULT,
                                                    Constant.SettingParameter.FN_ALLOW_CLOSE_REGISTRATION_WITHOUT_BILL,
                                                    Constant.SettingParameter.IS_CLOSE_REGISTRATION_CHECK_OUTSTANDING_IMAGING_RESULT,
                                                    Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID,
                                                    Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM,
                                                    Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID,
                                                    Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI
                                               );
            List<SettingParameterDt> lstSetvar = BusinessLayer.GetSettingParameterDtList(filterSetvar);


            hdnIsCheckResultTest.Value = lstSetvar.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_CHECK_TEST_RESULT).FirstOrDefault().ParameterValue;

            hdnIsAllowClosePatientWithoutBill.Value = lstSetvar.Where(a => a.ParameterCode == Constant.SettingParameter.FN_ALLOW_CLOSE_REGISTRATION_WITHOUT_BILL).FirstOrDefault().ParameterValue;
            hdnIsCloseRegistrationCheckImagingOutstanding.Value = lstSetvar.Where(a => a.ParameterCode == Constant.SettingParameter.IS_CLOSE_REGISTRATION_CHECK_OUTSTANDING_IMAGING_RESULT).FirstOrDefault().ParameterValue;

            hdnLaboratoryServiceUnitID.Value = lstSetvar.Where(a => a.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
            hdnLaboratoryHealthcareServiceUnitID.Value = lstSetvar.Where(a => a.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;
            hdnImagingServiceUnitID.Value = lstSetvar.Where(a => a.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
            hdnImagingHealthcareServiceUnitID.Value = lstSetvar.Where(a => a.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
        }

        private void TogglePatientNotification()
        {
            List<PatientVisitNote> lstEntity = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND IsDeleted = 0 AND IsDeleted = 0 AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.REGISTRATION_NOTES));
            if (lstEntity.Count == 0)
            {
                divVisitNote.Attributes.Add("style", "display:none");

            }

            string filterExpression = string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID);
            List<vRegistrationOutstandingInfo> lstInfo = BusinessLayer.GetvRegistrationOutstandingInfoList(filterExpression);
            foreach (vRegistrationOutstandingInfo entityInfo in lstInfo)
            {
                bool outstanding = (entityInfo.ServiceOrder + entityInfo.PrescriptionOrder + entityInfo.PrescriptionReturnOrder + entityInfo.LaboratoriumOrder + entityInfo.RadiologiOrder + entityInfo.OtherOrder > 0);

                if (!outstanding)
                {
                    hdnIsHasOutstandingOrder.Value = "0";
                }
                else
                {
                    hdnIsHasOutstandingOrder.Value = "1";
                }

                bool allowClose = (entityInfo.ServiceOrder + entityInfo.PrescriptionOrder + entityInfo.PrescriptionReturnOrder + entityInfo.LaboratoriumOrder + entityInfo.RadiologiOrder + entityInfo.OtherOrder + entityInfo.Charges + entityInfo.Billing == 0);
                if (!allowClose)
                {
                    divCloseRegistration.Style.Add("display", "none");
                }
            }

            Registration entityReg = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
            if (entityReg.IsLockDown)
            {
                divLockTransaction.Style.Add("display", "none");
            }
            else
            {
                divUnlockTransaction.Style.Add("display", "none");
            }
        }

        protected void rptHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetUserMenuAccess obj = (GetUserMenuAccess)e.Item.DataItem;
                HtmlGenericControl liCaption = (HtmlGenericControl)e.Item.FindControl("liCaption");
                IEnumerable<GetUserMenuAccess> mn = lstMenu.Where(p => p.ParentID == obj.MenuID && p.MenuCode == menuCode);
                if (mn.Count() > 0)
                {
                    liCaption.Attributes.Add("class", "selected");
                }
                List<GetUserMenuAccess> lstMn = lstMenu.Where(p => p.ParentID == obj.MenuID).OrderBy(p => p.MenuIndex).ToList();
                if (lstMn.Count > 0)
                    liCaption.Attributes.Add("url", lstMn[0].MenuUrl);
                else
                    liCaption.Visible = false;

            }
        }

        protected void rptMenuChild_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetUserMenuAccess obj = (GetUserMenuAccess)e.Item.DataItem;
                HtmlGenericControl liCaption = (HtmlGenericControl)e.Item.FindControl("liCaption");
                if (obj.MenuCode == menuCode) liCaption.Attributes.Add("class", "selected");
                HttpRequest temp = HttpContext.Current.Request;
                //TODO : TEMPORARY CODE
                List<string> lstPage = new List<string>() { "patientbillsummarycharges", "patientbillsummarylab", "patientbillsummarypharmacy", "patientbillsummaryprescriptionreturn", "patientbillsummarytestorder" };

                if (lstPage.Any(s => obj.MenuUrl.ToLower().Contains(s)))
                {
                    string currentDepartment = "";
                    if (obj.MenuUrl.ToLower().Contains("id=md"))
                    {
                        if (lstEntityPatientChargesHd.Where(t => t.DepartmentID == Constant.Facility.DIAGNOSTIC && (t.HealthcareServiceUnitID != Convert.ToInt32(hdnImagingHealthcareServiceUnitID.Value) && !t.IsLaboratoryUnit && t.HealthcareServiceUnitID != healthcareServiceUnitID)).Count() > 0)
                        {
                            liCaption.Attributes.Add("style", "font-weight: bold");
                        }
                    }
                    else if (obj.MenuUrl.ToLower().Contains("id=op") || currentDepartment == Constant.Facility.OUTPATIENT)
                    {
                        if (lstEntityPatientChargesHd.Where(t => t.DepartmentID == Constant.Facility.OUTPATIENT).Count() > 0)
                        {
                            liCaption.Attributes.Add("style", "font-weight: bold");
                        }
                    }
                    else if (obj.MenuUrl.ToLower().Contains("id=er") || currentDepartment == Constant.Facility.EMERGENCY)
                    {
                        if (lstEntityPatientChargesHd.Where(t => t.DepartmentID == Constant.Facility.EMERGENCY).Count() > 0)
                        {
                            liCaption.Attributes.Add("style", "font-weight: bold");
                        }
                    }
                    else if (obj.MenuUrl.ToLower().Contains("id=ip") || currentDepartment == Constant.Facility.INPATIENT)
                    {
                        if (lstEntityPatientChargesHd.Where(t => t.DepartmentID == Constant.Facility.INPATIENT).Count() > 0)
                        {
                            liCaption.Attributes.Add("style", "font-weight: bold");
                        }
                    }
                    else if (obj.MenuUrl.ToLower().Contains("id=lb"))
                    {
                        if (lstEntityPatientChargesHd.Where(t => t.IsLaboratoryUnit).Count() > 0)
                        {
                            liCaption.Attributes.Add("style", "font-weight: bold");
                        }
                    }
                    else if (obj.MenuUrl.ToLower().Contains("id=ph"))
                    {
                        if (lstEntityPatientChargesHd.Where(t => t.DepartmentID == Constant.Facility.PHARMACY).Count() > 0)
                        {
                            liCaption.Attributes.Add("style", "font-weight: bold");
                        }
                    }
                    else if (obj.MenuUrl.ToLower().Contains("summaryprescriptionreturn"))
                    {
                        if (lstEntityPatientChargesHd.Where(t => t.DepartmentID == Constant.Facility.PHARMACY && t.PrescriptionReturnOrderID != 0).Count() > 0)
                        {
                            liCaption.Attributes.Add("style", "font-weight: bold");
                        }
                    }
                    else if (obj.MenuUrl.ToLower().Contains("id=is"))
                    {
                        if (lstEntityPatientChargesHd.Where(t => t.HealthcareServiceUnitID == Convert.ToInt32(hdnImagingHealthcareServiceUnitID.Value)).Count() > 0)
                        {
                            liCaption.Attributes.Add("style", "font-weight: bold");
                        }
                    }
                    else
                    {
                        if (lstEntityPatientChargesHd.Where(t => t.HealthcareServiceUnitID == healthcareServiceUnitID).Count() > 0)
                        {
                            liCaption.Attributes.Add("style", "font-weight: bold");
                        }
                    }
                }
            }
        }

        protected void cbpProcessRegistration_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = "";
            param = e.Parameter;
            result += param + "|";
            if (param.ToLower().Equals("lock") || param.ToLower().Equals("unlock"))
            {

                if (OnLockUnlockRecord(ref errMessage, param))
                {
                    result += "success";
                }
                else result += "fail|" + errMessage;
            }
            else
            {
                if (OnBeforeProcessRecord(ref errMessage))
                {
                    if (OnProcessRecord(ref errMessage))
                        result = "success";
                    else
                        result += "fail|" + errMessage;
                }
                else
                    result += "fail|" + errMessage;
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnBeforeProcessRecord(ref string errMessage)
        {
            try
            {
                string filterExpression = string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID);
                List<vRegistrationOutstandingInfo> lstInfo = BusinessLayer.GetvRegistrationOutstandingInfoList(filterExpression);
                vRegistrationOutstandingInfo entity = lstInfo.FirstOrDefault();
                if (entity != null)
                {
                    if (entity.Charges > 0 || entity.Billing > 0)
                    {
                        errMessage = "Masih Ada Transaksi Yang Belum Lunas. Pendaftaran Belum Bisa Ditutup";
                        return false;
                    }
                }

                if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                {
                    List<Bed> lstBed = BusinessLayer.GetBedList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value));
                    if (lstBed.Count() > 0)
                    {
                        errMessage = "Pasien Belum Dipulangkan. Pendaftaran Belum Bisa Ditutup";
                        return false;
                    }
                }

                if (hdnIsAllowClosePatientWithoutBill.Value == "0")
                {
                    List<PatientPaymentHd> lstPatientPayment = BusinessLayer.GetPatientPaymentHdList(string.Format("RegistrationID = {0} AND GCTransactionStatus <> '{1}' AND GCPaymentType = '{2}'",
                                                                    hdnRegistrationID.Value, Constant.TransactionStatus.VOID, Constant.PaymentType.DEPOSIT_IN));
                    if (lstPatientPayment.Count() == 0)
                    {
                        List<PatientBill> lstPatientBill = BusinessLayer.GetPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus <> '{1}'", hdnRegistrationID.Value, Constant.TransactionStatus.VOID));
                        if (lstPatientBill.Count() == 0)
                        {
                            errMessage = "Pasien ini tidak memiliki tagihan, jika pasien batal registrasi, harap melakukan VOID Registrasi.";
                            return false;
                        }
                    }
                }

                if (hdnIsCloseRegistrationCheckImagingOutstanding.Value == "1")
                {
                    string filterCharges = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' AND HealthcareServiceUnitID = {2}", AppSession.RegisteredPatient.VisitID, Constant.TransactionStatus.VOID, hdnImagingHealthcareServiceUnitID.Value);
                    List<PatientChargesHd> lstCharges = BusinessLayer.GetPatientChargesHdList(filterCharges);
                    if (lstCharges.Count > 0)
                    {
                        string filterImagingResult = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}'", AppSession.RegisteredPatient.VisitID, Constant.TransactionStatus.VOID);
                        List<ImagingResultHd> lstImagingResult = BusinessLayer.GetImagingResultHdList(filterImagingResult);
                        if (lstImagingResult.Count() == 0)
                        {
                            errMessage = "Pasien belum memiliki Hasil Radiologi, tidak dapat menutup registrasi.";
                            return false;
                        }
                    }
                }
                
                foreach (vRegistrationOutstandingInfo entityInfo in lstInfo)
                {
                    bool allowClose = (entityInfo.ServiceOrder + entityInfo.PrescriptionOrder + entityInfo.PrescriptionReturnOrder + entityInfo.LaboratoriumOrder + entityInfo.RadiologiOrder + entityInfo.OtherOrder + entityInfo.Charges + entityInfo.Billing == 0);
                    if (!allowClose)
                    {
                        errMessage = "Pasien ini masih memiliki transaksi / order yang belum selesai, tidak dapat menutup registrasi.";
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnProcessRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityDao = new RegistrationDao(ctx);
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
            try
            {
                AuditLog entityAuditLog = new AuditLog();
                Registration registration = entityDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                entityAuditLog.OldValues = JsonConvert.SerializeObject(registration);
                registration.GCRegistrationStatus = Constant.VisitStatus.CLOSED;
                registration.LastUpdatedBy = AppSession.UserLogin.UserID;
                registration.ClosedBy = AppSession.UserLogin.UserID;
                registration.ClosedDate = DateTime.Parse(String.Format("{0}", DateTime.Now));
                registration.ClosedTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                registration.IsBillingClosed = true;
                registration.BillingClosedBy = AppSession.UserLogin.UserID;
                registration.BillingClosedDate = DateTime.Now;
                registration.IsBillingReopen = false;
                registration.BillingReopenBy = null;
                registration.BillingReopenDate = null;
                entityDao.Update(registration);

                entityAuditLog.ObjectType = Constant.BusinessObjectType.REGISTRATION;
                entityAuditLog.NewValues = JsonConvert.SerializeObject(registration);
                entityAuditLog.UserID = AppSession.UserLogin.UserID;
                entityAuditLog.LogDate = DateTime.Now;
                entityAuditLog.TransactionID = registration.RegistrationID;
                entityAuditLogDao.Insert(entityAuditLog);

                List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND GCVisitStatus <> '{1}'", registration.RegistrationID, Constant.VisitStatus.CANCELLED), ctx);
                foreach (ConsultVisit consultVisit in lstConsultVisit)
                {
                    consultVisit.GCVisitStatus = Constant.VisitStatus.CLOSED;
                    consultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                    consultVisitDao.Update(consultVisit);
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

        private bool OnLockUnlockRecord(ref string errMessage, string temp)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            try
            {
                if (string.IsNullOrEmpty(hdnLinkedRegistrationID.Value))
                {
                    hdnLinkedRegistrationID.Value = "0";
                }
                string filterExpression = string.Format("RegistrationID IN ('{0}','{1}')", hdnRegistrationID.Value, hdnLinkedRegistrationID.Value);

                List<Registration> lstEntity = BusinessLayer.GetRegistrationList(filterExpression);
                foreach (Registration entity in lstEntity)
                {
                    if (temp.ToLower().Equals("lock"))
                    {
                        entity.IsLockDown = true;
                        entity.LockDownBy = AppSession.UserLogin.UserID;
                        entity.LockDownDate = DateTime.Now;
                        AppSession.RegisteredPatient.IsLockDown = true;
                    }
                    else
                    {
                        entity.IsLockDown = false;
                        entity.LockDownBy = null;
                        entity.LockDownDate = null;
                        AppSession.RegisteredPatient.IsLockDown = false;
                    }
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityRegistrationDao.Update(entity);
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
    }
}