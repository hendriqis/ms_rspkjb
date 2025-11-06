using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class CancelPatientDischargeList : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.CANCEL_PATIENT_DISCHARGE;
        }
        private GetUserMenuAccess menu;
        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }
        protected int PageCount = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtDischargeDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                Helper.SetControlEntrySetting(txtDischargeDate, new ControlEntrySetting(true, true, true), "mpBedStatus");
                hdnIsBridgingToAplicares.Value = AppSession.IsBridgingToAPLICARES ? "1" : "0";
                List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID,
                    Constant.SettingParameter.IP_ICU_SERVICE_UNIT_ID,
                    Constant.SettingParameter.IP_NICU_SERVICE_UNIT_ID,
                    Constant.SettingParameter.IP_PICU_SERVICE_UNIT_ID));
                hdnHealthcareServiceUnitICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_ICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
                hdnHealthcareServiceUnitNICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_NICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
                hdnHealthcareServiceUnitPICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_PICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
                
                BindGridView();

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("GCRegistrationStatus IN ('{0}','{1}') AND DischargeDate = '{2}' AND GCDischargeCondition NOT IN ('{3}','{4}')", 
                            Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.PHYSICIAN_DISCHARGE, 
                            Helper.GetDatePickerValue(txtDischargeDate).ToString(Constant.FormatString.DATE_FORMAT_112), 
                            Constant.PatientOutcome.DEAD_BEFORE_48, Constant.PatientOutcome.DEAD_AFTER_48);
            List<vRegistration> lstEntity = BusinessLayer.GetvRegistrationList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpCancelPatientDischarge_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            IDbContext ctx = DbFactory.Configure(true);
            ConsultVisitDao entityConsultVisitDao = new ConsultVisitDao(ctx);
            BedDao entityBedDao = new BedDao(ctx);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            PatientAccompanyDao entityAccompanyDao = new PatientAccompanyDao(ctx);
            string result = "success|";
            bool flagResult = true;
            try
            {
                List<Registration> lstRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID IN ({0})", hdnSelectedRegistration.Value), ctx);
                List<Bed> lstBed = BusinessLayer.GetBedList(string.Format("IsPatientAccompany = 0 AND BedID IN (SELECT BedID FROM ConsultVisit WHERE RegistrationID IN ({0}))", hdnSelectedRegistration.Value), ctx);
                List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID IN ({0})", hdnSelectedRegistration.Value), ctx);
                hdnSelectedClassID.Value = "";
                hdnSelectedHealthcareServiceUnitID.Value = "";
                foreach (Registration registration in lstRegistration)
                {
                    registration.GCRegistrationStatus = Constant.VisitStatus.CHECKED_IN;
                    registration.LastUpdatedBy = AppSession.UserLogin.UserID;

                    List<ConsultVisit> lst = lstConsultVisit.Where(p => p.RegistrationID == registration.RegistrationID).ToList();
                    int mainVisit = 0;
                    foreach (ConsultVisit consultVisit in lst)
                    {
                        Bed bed = lstBed.FirstOrDefault(p => p.BedID == consultVisit.BedID);
                        if (bed.GCBedStatus != Constant.BedStatus.UNOCCUPIED)
                        {
                            result = string.Format("fail|{0}", string.Format("Bed {0} Cannot Be Used", bed.BedCode));
                            flagResult = false;
                            break;
                        }
                        bed.GCBedStatus = Constant.BedStatus.OCCUPIED;
                        bed.RegistrationID = registration.RegistrationID;

                        bed.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityBedDao.Update(bed);

                        consultVisit.GCVisitStatus = Constant.VisitStatus.CHECKED_IN;
                        consultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                        consultVisit.DischargeTime = null;
                        consultVisit.DischargeDate = Convert.ToDateTime("1900-01-01");
                        consultVisit.GCDischargeMethod = null;
                        consultVisit.GCDischargeCondition = null;

                        entityConsultVisitDao.Update(consultVisit);

                        if (consultVisit.IsMainVisit)
                        {
                            mainVisit = consultVisit.VisitID;
                            if (AppSession.IsBridgingToAPLICARES)
                            {
                                vHealthcareServiceUnit entityHsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = '{0}'", consultVisit.HealthcareServiceUnitID)).FirstOrDefault();
                                if (entityHsu != null)
                                {
                                    if (entityHsu.DepartmentID == Constant.Facility.INPATIENT)
                                    {
                                        hdnSelectedHealthcareServiceUnitID.Value += consultVisit.HealthcareServiceUnitID.ToString() + "|";
                                        hdnSelectedClassID.Value += consultVisit.ClassID.ToString() + "|";
                                    }
                                }
                            }
                        }
                    }

                    #region PatientAccompany
                    List<PatientAccompany> listEntityPA = BusinessLayer.GetPatientAccompanyList(String.Format("RegistrationID = {0} AND IsDeleted = 0", registration.RegistrationID) ,ctx);

                    if (listEntityPA.Count > 0) {
                        foreach (PatientAccompany entityPA in listEntityPA) {
                            Bed entityBed = BusinessLayer.GetBed(Convert.ToInt32(entityPA.BedID));

                            if (entityBed.GCBedStatus == Constant.BedStatus.UNOCCUPIED)
                            {
                                entityBed.RegistrationID = registration.RegistrationID;
                                entityBed.IsPatientAccompany = true;
                                entityBed.GCBedStatus = Constant.BedStatus.OCCUPIED;
                                entityBed.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityBedDao.Update(entityBed);
                            }
                            else 
                            {
                                result += String.Format("{0},", entityBed.BedCode);
                            }
                        }
                    }
                    #endregion

                    if (AppSession.IsBridgingToQueue)
                    {
                        //If Bridging to Queue - Send Information
                        try
                        {
                            VisitInfo visitInfo = new VisitInfo();
                            vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", mainVisit),ctx).FirstOrDefault();
                            visitInfo = ConvertVisitToDTO(oVisit);
                            PatientData patientInfo = ConvertPatientToDTO(oVisit);
                            APIMessageLog entityAPILog = new APIMessageLog()
                            {
                                MessageDateTime = DateTime.Now,
                                Recipient = Constant.BridgingVendor.QUEUE,
                                Sender = Constant.BridgingVendor.HIS,
                                IsSuccess = true
                            };
                            QueueService oService = new QueueService();
                            string apiResult = oService.SendVisitInformation(registration, visitInfo, patientInfo);
                            string[] apiResultInfo = apiResult.Split('|');
                            if (apiResultInfo[0] == "0")
                            {
                                entityAPILog.IsSuccess = false;
                                entityAPILog.Response = apiResultInfo[1];
                                Exception ex = new Exception(apiResultInfo[1]);
                                Helper.InsertErrorLog(ex);
                            }
                            else entityAPILog.MessageText = apiResultInfo[1];
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        }
                        catch (Exception ex)
                        {
                            Helper.InsertErrorLog(ex);
                        }
                    }
                    entityRegistrationDao.Update(registration);
                }
                if (hdnSelectedHealthcareServiceUnitID.Value.Length > 0)
                {
                    hdnSelectedHealthcareServiceUnitID.Value = hdnSelectedHealthcareServiceUnitID.Value.Remove(hdnSelectedHealthcareServiceUnitID.Value.Length - 1);
                    hdnSelectedClassID.Value = hdnSelectedClassID.Value.Remove(hdnSelectedClassID.Value.Length - 1);
                }

                if (flagResult) 
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    ctx.CommitTransaction();
                }
                else 
                { 
                    ctx.RollBackTransaction(); 
                }
            }
            catch (Exception ex)
            {
                result = string.Format("fail|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Bridging to Queue - Methods
        private VisitInfo ConvertVisitToDTO(vConsultVisit entityVisit)
        {
            VisitInfo visitInfo = new VisitInfo();
            visitInfo.VisitID = entityVisit.VisitID;
            visitInfo.VisitDate = entityVisit.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            visitInfo.VisitTime = entityVisit.VisitTime;
            visitInfo.DepartmentCode = Constant.Facility.INPATIENT;
            visitInfo.ServiceUnitCode = entityVisit.ServiceUnitCode;
            visitInfo.ServiceUnitName = entityVisit.ServiceUnitName;
            visitInfo.RoomID = entityVisit.RoomID;
            visitInfo.RoomCode = entityVisit.RoomCode;
            visitInfo.RoomName = entityVisit.RoomName;
            visitInfo.PhysicianID = entityVisit.ParamedicID;
            visitInfo.PhysicianCode = entityVisit.ParamedicCode;
            visitInfo.PhysicianName = entityVisit.ParamedicName;
            visitInfo.SpecialtyName = entityVisit.SpecialtyName;
            visitInfo.BedCode = entityVisit.BedCode;
            visitInfo.ExtensionNo = entityVisit.ExtensionNo;
            return visitInfo;
        }
        private PatientData ConvertPatientToDTO(vConsultVisit oVisit)
        {
            PatientData oData = new PatientData();
            oData.PatientID = oVisit.MRN;
            oData.MedicalNo = oVisit.MedicalNo;
            oData.FirstName = oVisit.FirstName;
            oData.MiddleName = oVisit.MiddleName;
            oData.LastName = oVisit.LastName;
            oData.PrefferedName = oVisit.PreferredName;
            oData.Gender = string.Format("{0}^{1}", oVisit.GCGender.Substring(5), oVisit.Gender);
            oData.Religion = string.Empty;
            oData.MaritalStatus = string.Empty;
            oData.Nationality = string.Empty;

            oData.DateOfBirth = oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112);
            oData.CityOfBirth = oVisit.CityOfBirth;
            oData.HomeAddress = oVisit.HomeAddress;
            oData.MobileNo1 = oVisit.MobilePhoneNo1;
            oData.MobileNo2 = oVisit.MobilePhoneNo2;
            oData.EmailAddress = string.Empty;
            return oData;
        }
        #endregion
    }
}