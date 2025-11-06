using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Data.Common;
using Newtonsoft.Json;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AppointmentConfirmedUploadExcelCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] arrayParam = param.Split('|');
            //hdnDepartmentID.Value = arrayParam[0];
            //vHealthcareServiceUnit entityHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID = '{0}' AND IsDeleted = 0", hdnDepartmentID.Value)).FirstOrDefault();
            //hdnHealthcareServiceUnitID.Value = entityHSU.HealthcareServiceUnitID.ToString();
            //GetSettingParameter();
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(String.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                AppSession.UserLogin.HealthcareID, //0
                Constant.SettingParameter.MC_KELOMPOK_AUTO_CREATE_PATIENT, //1
                Constant.SettingParameter.CAPITALIZE_PATIENT_NAME, //2
                Constant.SettingParameter.CAPITALIZE_PATIENT_ADDRESS //3
            ));

            hdnIsAutoCreatePatient.Value = setvar.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MC_KELOMPOK_AUTO_CREATE_PATIENT).ParameterValue;
            hdnFlagToUpperPatientName.Value = setvar.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.CAPITALIZE_PATIENT_NAME).ParameterValue;
            hdnFlagToUpperPatientAddress.Value = setvar.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.CAPITALIZE_PATIENT_ADDRESS).ParameterValue;
        }

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string errConfirmationMessage = "";
            string[] param = e.Parameter.Split('|');
            string[] fileInfo = hdnFileName.Value.Split('.');
            string fileName = fileInfo[1].ToLower();
            string fileExtension = fileInfo[0];
            string lstSheets = string.Empty;

            if (param[0] == "upload")
            {
                result = "upload|";
                onUploadExcel(param[0], fileName, fileExtension, ref lstSheets, ref errMessage, ref errConfirmationMessage);
                if (string.IsNullOrEmpty(errMessage))
                {
                    if (!string.IsNullOrEmpty(lstSheets))
                    {
                        result += string.Format("success|{0}", lstSheets);
                    }
                    else
                    {
                        result += "failed";
                    }
                }
                else
                {
                    result += string.Format("failed|{0}", errMessage);
                }
            }
            else if (param[0] == "load")
            {
                result = "load|";
                onUploadExcel(param[0], fileName, fileExtension, ref lstSheets, ref errMessage, ref errConfirmationMessage);
                if (string.IsNullOrEmpty(errMessage) && string.IsNullOrEmpty(errConfirmationMessage))
                {
                    result += string.Format("success|");
                }
                else
                {
                    if (!string.IsNullOrEmpty(errMessage))
                    {
                        result += string.Format("failed|{0}", errMessage);
                    }
                    else if (!string.IsNullOrEmpty(errConfirmationMessage))
                    {
                        result += string.Format("confirmation|{0}", errConfirmationMessage);
                    }
                }
            }
            else if (param[0] == "process")
            {
                result = "process|";
                if (onProcessExcelData(fileName, fileExtension, ref errMessage))
                {
                    result += "success";
                }
                else
                {
                    result += "fail|" + errMessage;
                }
            }
            else if (param[0] == "replace")
            {
                result = "replace|";
                onUploadExcel(param[0], fileName, fileExtension, ref lstSheets, ref errMessage, ref errConfirmationMessage);
                if (!string.IsNullOrEmpty(errMessage))
                {
                    result += string.Format("failed|{0}", errMessage);
                }
                else if (!string.IsNullOrEmpty(errConfirmationMessage))
                {
                    result += string.Format("confirmation|{0}", errConfirmationMessage);
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpChangeSheet_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";

            String[] sheets = hdnSheetsName.Value.Split(',');
            List<StandardCode> lst = new List<StandardCode>();
            for (int i = 0; i < sheets.Length; i++)
            {
                lst.Insert(i, new StandardCode { StandardCodeName = sheets[i].Replace("$", "").Replace("'", ""), StandardCodeID = sheets[i] });
            }
            Methods.SetComboBoxField<StandardCode>(cboSheetsName, lst, "StandardCodeName", "StandardCodeID");

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool onVoidRecord(ref string errMessage)
        {
            bool result = true;
            //IDbContext ctx = DbFactory.Configure(true);
            //AppointmentDao appointmentDao = new AppointmentDao(ctx);
            //RegistrationBPJSDao registrationBPJSDao = new RegistrationBPJSDao(ctx);
            //AppointmentRequestDao appointmentRequestDao = new AppointmentRequestDao(ctx);
            //try
            //{
            //    if (!String.IsNullOrEmpty(hdnAppointmentID.Value) && !String.IsNullOrEmpty(hdnAppointmentRequestID.Value) && !String.IsNullOrEmpty(hdnRegistrationID.Value))
            //    {
            //        Appointment entity = appointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
            //        RegistrationBPJS entityRegistration = registrationBPJSDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
            //        AppointmentRequest entityRequest = appointmentRequestDao.Get(Convert.ToInt32(hdnAppointmentRequestID.Value));

            //        entity.GCAppointmentStatus = Constant.AppointmentStatus.DELETED;
            //        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            //        appointmentDao.Update(entity);

            //        if (entityRegistration != null)
            //        {
            //            entityRegistration.AppointmentID = null;
            //            entityRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
            //            registrationBPJSDao.Update(entityRegistration);
            //        }

            //        entityRequest.AppointmentID = null;
            //        entityRequest.LastUpdatedBy = AppSession.UserLogin.UserID;
            //        appointmentRequestDao.Update(entityRequest);

            //        ctx.CommitTransaction();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    result = false;
            //    errMessage = ex.Message;
            //    Helper.InsertErrorLog(ex);
            //    ctx.RollBackTransaction();
            //}
            //finally
            //{
            //    ctx.Close();
            //}
            return result;
        }

        private void onUploadExcel(string type, string fileName, string fileExtension, ref string lstSheets, ref string errMessage, ref string errConfirmationMessage)
        {
            StringBuilder error = new StringBuilder();
            StringBuilder errorConfirmation = new StringBuilder();

            if (!string.IsNullOrEmpty(hdnUploadedFile1.Value))
            {
                string imageData = hdnUploadedFile1.Value;
                if (imageData != "")
                {
                    string[] parts = Regex.Split(imageData, ",").Skip(1).ToArray();
                    imageData = String.Join(",", parts);
                }

                string qispath = AppConfigManager.QISPhysicalUploadExcelTempDirectory;
                qispath += string.Format("{0}\\", hdnDepartmentID.Value);

                if (!Directory.Exists(qispath))
                {
                    Directory.CreateDirectory(qispath);
                }
                string fullPath = string.Format("{0}{1}", qispath, hdnFileName.Value);
                FileStream fs = new FileStream(fullPath, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);

                byte[] data = Convert.FromBase64String(imageData);
                bw.Write(data);
                bw.Close();

                if (type == "upload")
                {
                    List<String> lstSheetName = new List<String>();
                    Helper.ReadExcelFile_GetSheetsName(fullPath, fileName, ref lstSheetName);

                    if (lstSheetName.Count > 0)
                    {
                        int i = 0;
                        string sheets = string.Empty;
                        foreach (String str in lstSheetName)
                        {
                            lstSheets += string.Format("{0},", str);
                            i++;
                        }

                        lstSheets = lstSheets.Remove(lstSheets.Length - 1, 1);
                    }
                }
                else if (type == "load")
                {
                     List<ZendeskAppountmentConfirmUpload> lstObj = Helper.ConvertExcelFileToJSONToModel<List<ZendeskAppountmentConfirmUpload>>(fullPath, fileName, ref errMessage, cboSheetsName.Value.ToString());

                     switch (type)
                     {
                         case "load":
                             ValidationLoadExcel(lstObj,  ref error, ref errorConfirmation);
                             break;
                       
                     }

                     if (lstObj.Count == lstObj.Count)
                     {
                         grdView.DataSource = lstObj.OrderBy(o => o.TicketId).ToList();
                         grdView.DataBind();
                     }
                     else
                     {
                         if (string.IsNullOrEmpty(error.ToString()))
                         {
                             error.AppendLine(string.Format("No Data Found"));
                         }
                     }
                }
            }

            if (!string.IsNullOrEmpty(error.ToString()))
            {
                errMessage = error.ToString().Replace(Environment.NewLine, "<br />");
            }
            else if (!string.IsNullOrEmpty(errorConfirmation.ToString()))
            {
                errConfirmationMessage = errorConfirmation.ToString().Replace(Environment.NewLine, "<br />");
            }
        }

        private void ValidationLoadExcel(List<ZendeskAppountmentConfirmUpload> lstObj, ref StringBuilder error, ref StringBuilder errorConfirmation)
        {
            bool isValid = true;
            if (lstObj.Count > 0)
            {
                foreach (ZendeskAppountmentConfirmUpload row in lstObj) {
                    if (string.IsNullOrEmpty(row.TicketId) || string.IsNullOrEmpty(row.RequesterWANumber) || string.IsNullOrEmpty(row.RequesterName) || string.IsNullOrEmpty(row.NamaPasien) || string.IsNullOrEmpty(row.OPA) ) 
                    {
                        error.AppendLine(string.Format("Error di Tiket {0} terdapat kolom yang kosong", row.TicketId));
                        isValid = false;
                    }
                }
            }
            else {
                error.AppendLine(string.Format("Ada duplikasi nomor identitas."));
                isValid = false;
            }
        }

        private void ReplaceExistingPatient(List<ZendeskAppountmentConfirmUpload> lstObj, ref List<ZendeskAppountmentConfirmUpload> newLstObj, ref StringBuilder error, ref StringBuilder errorConfirmation)
        {
            foreach (ZendeskAppountmentConfirmUpload obj in lstObj)
            {
                 

                newLstObj.Add(obj);
            }
        }

        private void EntityToControl(AppointmentRequestMCU obj, AppointmentRequest entity, Patient entityPatient, Guest entityGuest, string batchNo, IDbContext ctx)
        {
            //ctx.CommandType = CommandType.Text;
            //ctx.Command.Parameters.Clear();

            //entity.GCAppointmentRequestMethod = Constant.AppointmentRequestMethod.APPOINTMENT_REQUEST_MCU;
            //entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value.ToString());
            //entity.AppointmentDate = Helper.GetDatePickerValue(Helper.ConvertDate112ToDatePickerFormat(obj.AppointmentDate));
            //  if (entityPatient != null)
            //{
            //    entity.MRN = entityPatient.MRN;
            //    entityPatient.CorporateAccountNo = obj.CorporateAccountNo;
            //    entityPatient.CorporateAccountName = obj.CorporateAccountName;
            //    entityPatient.CorporateAccountDepartment = obj.CorporateAccountDepartment;
            //}
            //else if (entityGuest != null)
            //{
            //    entity.GuestID = entityGuest.GuestID;
            //    entityGuest.CorporateAccountNo = obj.CorporateAccountNo;
            //    entityGuest.CorporateAccountName = obj.CorporateAccountName;
            //    entityGuest.CorporateAccountDepartment = obj.CorporateAccountDepartment;
            //}
            //entity.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);

            //BusinessPartners entityBP = BusinessLayer.GetBusinessPartnersList(String.Format("BusinessPartnerCode = '{0}' AND IsDeleted = 0", obj.BusinessPartnerCode), ctx).FirstOrDefault();
            //if (entityBP != null)
            //{
            //    entity.BusinessPartnerID = entityBP.BusinessPartnerID;
            //    Customer entityCustomer = BusinessLayer.GetCustomerList(string.Format("BusinessPartnerID = '{0}'", entity.BusinessPartnerID)).FirstOrDefault();
            //    if (entityCustomer != null)
            //    {
            //        entity.GCCustomerType = entityCustomer.GCCustomerType;
            //    }
            //}

            //ctx.CommandType = CommandType.Text;
            //ctx.Command.Parameters.Clear();

            //ItemMaster entityIM = BusinessLayer.GetItemMasterList(string.Format("ItemCode = '{0}' AND IsDeleted = 0", obj.MCUItem), ctx).FirstOrDefault();
            //if (entityIM != null)
            //{
            //    entity.ItemID = entityIM.ItemID;
            //}
            //entity.IsDeleted = false;
            //entity.CreatedBy = AppSession.UserLogin.UserID;
            //entity.CreatedDate = DateTime.Now;

            //entity.RequestBatchNo = batchNo;

            //ctx.CommandType = CommandType.Text;
            //ctx.Command.Parameters.Clear();
        }

        private bool onProcessExcelData(string fileName, string fileExtension, ref string errMessage)
        {
            bool result = true;

            string batchNo = string.Empty;

            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao apmDao = new AppointmentDao(ctx);

            string qispath = AppConfigManager.QISPhysicalUploadExcelTempDirectory;
            qispath += string.Format("{0}\\", hdnDepartmentID.Value);
            string fullPath = string.Format("{0}{1}", qispath, hdnFileName.Value);
            List<ZendeskAppountmentConfirmUpload> lstObj = Helper.ConvertExcelFileToJSONToModel<List<ZendeskAppountmentConfirmUpload>>(fullPath, fileName, ref errMessage, cboSheetsName.Value.ToString());
            if (lstObj.Count > 0 ) 
            {
                try {

                    foreach (ZendeskAppountmentConfirmUpload row in lstObj) {
                        Appointment apmData = BusinessLayer.GetAppointmentList(string.Format("AppointmentNo='{0}' AND GCAppointmentStatus IN('{1}')", row.OPA.Trim(), Constant.AppointmentStatus.STARTED)).FirstOrDefault();
                        if (apmData != null) {
                            if (row.AppointmentReason.Trim() == "1")
                            {
                                apmData.GCAppointmentStatus = Constant.AppointmentStatus.CONFIRMED;
                                apmData.LastUpdatedBy = AppSession.UserLogin.UserID;
                                apmData.LastUpdatedDate = DateTime.Now;
                                apmDao.Update(apmData);
                            }
                            else if (row.AppointmentReason.Trim() == "0")
                            {
                                apmData.GCAppointmentStatus = Constant.AppointmentStatus.DELETED;
                                apmData.DeleteReason = string.Format("Cancel by file upload status ref no {0} ",row.TicketId);
                                apmData.GCDeleteReason = Constant.DeleteReason.OTHER;
                                apmData.LastUpdatedBy = AppSession.UserLogin.UserID;
                                apmData.LastUpdatedDate = DateTime.Now; 
                                apmDao.Update(apmData);
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
            }

             

            return result;
        }

        private bool IsMedicalNoValid(string medicalNo, IDbContext ctx)
        {
            string filterExpression = string.Format("MedicalNo = '{0}'", medicalNo);
            Patient oPatient = BusinessLayer.GetPatientList(filterExpression, ctx).FirstOrDefault();
            return oPatient == null;
        }

        private void ControlToEntity(AppointmentRequestMCU obj, Patient entity, Address entityAddress, Address entityOfficeAddress, Address entityDomicileAddress, PatientTagField entityTagField, IDbContext ctx)
        {
            #region Patient Data
            entity.LastName = obj.PatientName;
            entity.PreferredName = obj.PatientName;
            bool flagToUpperPatientName = hdnFlagToUpperPatientName.Value == "1";
            if (flagToUpperPatientName)
            {
                entity.FirstName = entity.FirstName.ToUpper();
                entity.MiddleName = entity.MiddleName.ToUpper();
                entity.LastName = entity.LastName.ToUpper();
                entity.PreferredName = entity.PreferredName.ToUpper();
            }
            if (obj.Gender == "M")
            {
                entity.GCSex = Constant.Gender.MALE;
                entity.GCGender = Constant.Gender.MALE;
            }
            else
            {
                entity.GCSex = Constant.Gender.FEMALE;
                entity.GCGender = Constant.Gender.FEMALE;
            }
            entity.CityOfBirth = obj.CityOfBirth;
            entity.DateOfBirth = Helper.GetDatePickerValue(Helper.ConvertDate112ToDatePickerFormat(obj.DateOfBirth));
            string suffix = string.Empty;
            string title = string.Empty;
            entity.Name = obj.PatientName.Length > 35 ? obj.PatientName.Substring(0, 35) : obj.PatientName;
            entity.FullName = Helper.GenerateFullName(Helper.GenerateName(entity.LastName, string.Empty, string.Empty), title, suffix);
            //entity.Name2 = entity.FullName;
            entity.RegisteredDate = DateTime.Now.Date;
            #endregion

            #region Patient Address
            entityAddress.StreetName = obj.Address;
            entityAddress.City = obj.City;

            entityDomicileAddress.StreetName = obj.Address;
            entityDomicileAddress.City = obj.City;

            bool flagToUpperPatientAddress = hdnFlagToUpperPatientAddress.Value == "1";
            if (flagToUpperPatientAddress)
            {
                entityAddress.StreetName = entityAddress.StreetName.ToUpper();
                entityDomicileAddress.StreetName = entityDomicileAddress.StreetName.ToUpper();
            }
            else
            {
                entityAddress.StreetName = entityAddress.StreetName;
                entityDomicileAddress.StreetName = entityDomicileAddress.StreetName;
            }
            #endregion

            #region Patient Contact
            entityAddress.PhoneNo1 = obj.PhoneNo;
            entity.MobilePhoneNo1 = obj.MobilePhoneNo;
            entity.EmailAddress = obj.EmailAddress;
            entityAddress.Email = obj.EmailAddress;
            if (!string.IsNullOrEmpty(obj.IdentityCardType))
            {
                if (obj.IdentityCardType.Equals("KTP"))
                {
                    entity.GCIdentityNoType = Constant.StandardCode.IDENTITY_NUMBERY_TYPE + "^001";
                }
                else if (obj.IdentityCardType.Equals("SIM"))
                {
                    entity.GCIdentityNoType = Constant.StandardCode.IDENTITY_NUMBERY_TYPE + "^002";
                }
                else if (obj.IdentityCardType.Equals("KITAS"))
                {
                    entity.GCIdentityNoType = Constant.StandardCode.IDENTITY_NUMBERY_TYPE + "^003";
                }
                else
                {
                    entity.GCIdentityNoType = Constant.StandardCode.IDENTITY_NUMBERY_TYPE + "^004";
                }
            }
            if (!string.IsNullOrEmpty(obj.IdentityCardNo))
            {
                if (entity.GCIdentityNoType == Constant.IdentityCardType.KTP)
                {
                    if (obj.IdentityCardNo.Length == 16)
                    {
                        entity.SSN = !string.IsNullOrEmpty(obj.IdentityCardNo) ? obj.IdentityCardNo : string.Empty;
                    }
                    //else
                    //{
                    //    result = false;
                    //    errMessage = string.Format("Error pada nomor {0} : Nomor KTP pasien dengan nomor rekam medis <b>{1}</b> harus 16 digit.", obj.No, obj.MedicalNo);
                    //    break;
                    //}
                }
            }
            #endregion

            #region Additonal Information
            entity.GCReligion = string.IsNullOrEmpty(obj.Religion) ? string.Format("{0}^OTH", Constant.StandardCode.RELIGION) : string.Format("{0}^{1}", Constant.StandardCode.RELIGION, obj.Religion);
            entity.GCNationality = Constant.Nationality.INDONESIA;
            entity.CorporateAccountNo = obj.CorporateAccountNo;
            entity.CorporateAccountName = obj.CorporateAccountName;
            entity.CorporateAccountDepartment = obj.CorporateAccountDepartment;
            #endregion

            #region Patient Payer

            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();

            vCustomer entityCust = BusinessLayer.GetvCustomerList(string.Format("BusinessPartnerCode = '{0}'", obj.BusinessPartnerCode), ctx).FirstOrDefault();
            string GCCustomerType = entityCust.GCCustomerType;
            if (GCCustomerType != Constant.CustomerType.PERSONAL)
            {
                if (string.IsNullOrEmpty(GCCustomerType))
                {
                    entity.BusinessPartnerID = null;
                }
                else
                {
                    entity.BusinessPartnerID = entityCust.BusinessPartnerID;
                }
            }
            else
            {
                entity.BusinessPartnerID = 1;
            }
            #endregion

            #region Patient Status
            entity.GCPatientStatus = Constant.PatientStatus.ACTIVE;
            #endregion
        }


        
    }
     
}