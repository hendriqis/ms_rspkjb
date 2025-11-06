using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AppointmentBillSummaryToolbarCtl : BaseUserControlCtl
    {
        string menuCode = "";
        List<GetUserMenuAccess> lstMenu = null;
        List<vPatientChargesHd1> lstEntityPatientChargesHd = null;
        int healthcareServiceUnitID = 0;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                //hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
                //hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
                //healthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;

                string moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);
                menuCode = ((BasePageContent)Page).OnGetMenuCode();
                string parentCode = "";
                switch (ModuleID)
                {
                    case "OP": parentCode = Constant.MenuCode.Outpatient.DRAFT_CHARGES_ENTRY_HEADER; break;
                    case "IP": parentCode = Constant.MenuCode.Inpatient.BILL_SUMMARY; break;
                    case "ER": parentCode = Constant.MenuCode.EmergencyCare.BILL_SUMMARY; break;
                    case "LB": parentCode = Constant.MenuCode.Laboratory.BILL_SUMMARY; break;
                    case "IS": parentCode = Constant.MenuCode.Imaging.BILL_SUMMARY; break;
                    case "MD": parentCode = Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY; break;
                    case "PH": parentCode = Constant.MenuCode.Pharmacy.BILL_SUMMARY; break;
                    case "MC": parentCode = Constant.MenuCode.MedicalCheckup.BILL_SUMMARY; break;
                }
                lstMenu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("(ParentCode = '{0}' OR ParentID IN (SELECT MenuID FROM Menu WHERE ParentID = (SELECT MenuID FROM Menu WHERE MenuCode = '{0}')))", parentCode));
                rptHeader.DataSource = lstMenu.Where(p => p.ParentCode == parentCode).OrderBy(p => p.MenuIndex).ToList();
                rptHeader.DataBind();
                GetUserMenuAccess selectedMenu = lstMenu.FirstOrDefault(p => p.MenuCode == menuCode);
                rptMenuChild.DataSource = lstMenu.Where(p => p.ParentID == selectedMenu.ParentID).OrderBy(p => p.MenuIndex).ToList();
                rptMenuChild.DataBind();
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
                    //if(HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Equals(obj.MenuUrl))
                    //{
                    //    currentDepartment = hdnDepartmentID.Value;
                    //}
                    if (obj.MenuUrl.ToLower().Contains("id=md"))
                    {
                        List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID, Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID));
                        string laboratoryID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
                        string imagingID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                        List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID IN ({1},{2}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, laboratoryID, imagingID));
                        int tempHSUImagingID = lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(imagingID)).HealthcareServiceUnitID;
                        int tempHSULaboratoryID = lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(laboratoryID)).HealthcareServiceUnitID;

                        if (lstEntityPatientChargesHd.Where(t => t.DepartmentID == Constant.Facility.DIAGNOSTIC && (t.HealthcareServiceUnitID != tempHSUImagingID && t.HealthcareServiceUnitID != tempHSULaboratoryID && t.HealthcareServiceUnitID != healthcareServiceUnitID)).Count() > 0)
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
                        //if (lstEntityPatientChargesHd.Where(t => t.DepartmentID == Constant.Facility.LABORATORY).Count() > 0)
                        //{
                        //    liCaption.Attributes.Add("style", "font-weight: bold");
                        //}
                        SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID);
                        string labServiceUnitID = settingParameter.ParameterValue;
                        vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND ServiceUnitID = {1}", AppSession.UserLogin.HealthcareID, labServiceUnitID)).FirstOrDefault();
                        if (lstEntityPatientChargesHd.Where(t => t.HealthcareServiceUnitID == hsu.HealthcareServiceUnitID).Count() > 0)
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
                        SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
                        string imagingServiceUnitID = settingParameter.ParameterValue;
                        vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND ServiceUnitID = {1}", AppSession.UserLogin.HealthcareID, imagingServiceUnitID)).FirstOrDefault();
                        if (lstEntityPatientChargesHd.Where(t => t.HealthcareServiceUnitID == hsu.HealthcareServiceUnitID).Count() > 0)
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
            if (OnBeforeProcessRecord(ref errMessage))
            {
                if (OnProcessRecord(ref errMessage))
                    result = "success";
                else
                    result += "fail|" + errMessage;
            }
            else
                result += "fail|" + errMessage;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnBeforeProcessRecord(ref string errMessage)
        {
            try
            {
                vRegistrationOutstandingInfo entity = BusinessLayer.GetvRegistrationOutstandingInfoList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value)).FirstOrDefault();

                if (entity.Charges > 0 || entity.Billing > 0)
                {
                    errMessage = "Masih Ada Transaksi Yang Belum Lunas. Pendaftaran Belum Bisa Ditutup";
                    return false;
                }

                if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                {
                    int count = BusinessLayer.GetvBedRowCount(string.Format("RegistrationID = {0}", hdnRegistrationID.Value));
                    if (count > 0)
                    {
                        errMessage = "Pasien Belum Dipulangkan. Pendaftaran Belum Bisa Ditutup";
                        return false;
                    }
                }

                if (hdnIsAllowClosePatientWithoutBill.Value == "0")
                {
                    List<PatientBill> lstPatientBill = BusinessLayer.GetPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus <> '{1}'", hdnRegistrationID.Value, Constant.TransactionStatus.VOID));
                    if (lstPatientBill.Count() <= 0)
                    {
                        errMessage = "Pasien ini tidak memiliki tagihan, jika pasien batal registrasi, harap melakukan VOID Registrasi.";
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
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
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}