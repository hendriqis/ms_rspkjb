using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class VoidPatientDischarge : BasePageTrx
    {
        private string refreshGridInterval = "";
        private string departmentID = "";

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.CANCEL_PATIENT_DISCHARGE;
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.CANCEL_PATIENT_DISCHARGE;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.CANCEL_PATIENT_DISCHARGE;
                default: return Constant.MenuCode.Inpatient.CANCEL_PATIENT_DISCHARGE;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

            departmentID = Page.Request.QueryString["id"];
            hdnParam.Value = departmentID;
            string filterExpression = "";

            if (departmentID == Constant.Facility.EMERGENCY)
            {
                hdnHealthCareServiceUnitID.Value = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.EMERGENCY))[0].HealthcareServiceUnitID.ToString();

                filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, departmentID);
            }
            else if (departmentID == Constant.Facility.DIAGNOSTIC && AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.OtherMedicalDiagnostic)
            {
                hdnHealthCareServiceUnitID.Value = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();

                filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, departmentID);
            }
            else
            {
                int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND IsDeleted = 0", departmentID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));

                filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, departmentID);
                if (departmentID == Constant.Facility.DIAGNOSTIC)
                    filterExpression += string.Format(" AND HealthcareServiceUnitID NOT IN ({0},{1})", AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
                if (serviceUnitUserCount > 0)
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND IsDeleted = 0)", departmentID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
            }

            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            lstServiceUnit = lstServiceUnit.OrderBy(unit => unit.ServiceUnitName).ToList();
            lstServiceUnit.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

            txtDischargeDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            hdnIsBridgingToAplicares.Value = AppSession.IsBridgingToAPLICARES ? "1" : "0";
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                            "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                                                                            AppSession.UserLogin.HealthcareID,
                                                                            Constant.SettingParameter.IP_ICU_SERVICE_UNIT_ID,
                                                                            Constant.SettingParameter.IP_NICU_SERVICE_UNIT_ID,
                                                                            Constant.SettingParameter.IP_PICU_SERVICE_UNIT_ID,
                                                                            Constant.SettingParameter.IS_PATIENT_DISCHARGE_DEAD
                                                                        )
                                                                    );
            hdnHealthcareServiceUnitICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_ICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
            hdnHealthcareServiceUnitNICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_NICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
            hdnHealthcareServiceUnitPICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_PICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
            hdnIsAllowCancelDischargeForDischargeDead.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_PATIENT_DISCHARGE_DEAD).FirstOrDefault().ParameterValue;
            

        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "";
            SettingParameterDt entity = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_PATIENT_DISCHARGE_DEAD);

            if (entity.ParameterValue == "0")
            {
                filterExpression = string.Format("GCRegistrationStatus IN ('{0}') AND DischargeDate = '{1}' AND GCDischargeCondition NOT IN ('{2}','{3}')",
                                        Constant.VisitStatus.DISCHARGED,
                                        Helper.GetDatePickerValue(txtDischargeDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                                        Constant.PatientOutcome.DEAD_BEFORE_48, Constant.PatientOutcome.DEAD_AFTER_48);
            }
            else
            {
                filterExpression = string.Format("GCRegistrationStatus IN ('{0}') AND DischargeDate = '{1}'",
                                        Constant.VisitStatus.DISCHARGED,
                                        Helper.GetDatePickerValue(txtDischargeDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (cboServiceUnit.Value.ToString() != "0" && cboServiceUnit.Value != null)
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
            }
            else
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", hdnParam.Value);
            }

            if (hdnFilterExpressionQuickSearch.Value != "Search" && hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            filterExpression += " ORDER BY RegistrationID";

            List<vRegistration> lstEntity = BusinessLayer.GetvRegistrationList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpCancelPatientDischarge_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            IDbContext ctx = DbFactory.Configure(true);

            BedDao entityBedDao = new BedDao(ctx);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            ConsultVisitDao entityConsultVisitDao = new ConsultVisitDao(ctx);
            PatientAccompanyDao entityAccompanyDao = new PatientAccompanyDao(ctx);

            string result = "success|";
            bool flagResult = true;

            try
            {
                hdnSelectedHealthcareServiceUnitID.Value = "";
                hdnSelectedRoomID.Value = "";
                hdnSelectedClassID.Value = "";

                string resultReg = "success|", resultRegTemp = "";
                string resultAccompany = "Bed Accompany";

                List<Registration> lstRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID IN ({0})", hdnSelectedRegistration.Value), ctx);
                foreach (Registration registration in lstRegistration)
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    if (registration.GCRegistrationStatus == Constant.VisitStatus.DISCHARGED)
                    {
                        registration.GCRegistrationStatus = Constant.VisitStatus.CHECKED_IN;
                        registration.LastUpdatedBy = AppSession.UserLogin.UserID;

                        int mainVisit = 0;
                        List<ConsultVisit> lst = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND GCVisitStatus != '{1}'", registration.RegistrationID, Constant.VisitStatus.CANCELLED), ctx);
                        foreach (ConsultVisit consultVisit in lst)
                        {
                            if (consultVisit.BedID != null && consultVisit.BedID != 0)
                            {
                                Bed bed = entityBedDao.Get(Convert.ToInt32(consultVisit.BedID));
                                if (bed != null)
                                {
                                    if (bed.GCBedStatus != Constant.BedStatus.UNOCCUPIED)
                                    {
                                        resultRegTemp += string.Format(" {0},", bed.BedCode);
                                        flagResult = false;
                                        break;
                                    }
                                    bed.GCBedStatus = Constant.BedStatus.OCCUPIED;
                                    bed.RegistrationID = registration.RegistrationID;

                                    bed.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityBedDao.Update(bed);
                                }
                            }

                            consultVisit.GCVisitStatus = Constant.VisitStatus.CHECKED_IN;
                            consultVisit.DischargeTime = null;
                            consultVisit.DischargeDate = Convert.ToDateTime(Constant.ConstantDate.DEFAULT_NULL);
                            if (hdnIsAllowCancelDischargeForDischargeDead.Value == "1")
                            {
                                consultVisit.DateOfDeath = Convert.ToDateTime(Constant.ConstantDate.DEFAULT_NULL);
                            }
                            consultVisit.GCDischargeMethod = null;
                            consultVisit.GCDischargeCondition = null;
                            consultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityConsultVisitDao.Update(consultVisit);

                            if (consultVisit.IsMainVisit)
                            {
                                mainVisit = consultVisit.VisitID;
                                if (AppSession.IsBridgingToAPLICARES)
                                {
                                    vHealthcareServiceUnit entityHsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = '{0}'", consultVisit.HealthcareServiceUnitID), ctx).FirstOrDefault();
                                    if (entityHsu != null)
                                    {
                                        if (entityHsu.DepartmentID == Constant.Facility.INPATIENT)
                                        {
                                            hdnSelectedHealthcareServiceUnitID.Value += consultVisit.HealthcareServiceUnitID.ToString() + "|";
                                            hdnSelectedRoomID.Value += consultVisit.RoomID.ToString() + "|";
                                            hdnSelectedClassID.Value += consultVisit.ClassID.ToString() + "|";
                                        }
                                    }
                                }
                            }
                        }

                        #region PatientAccompany
                        List<PatientAccompany> listEntityPA = BusinessLayer.GetPatientAccompanyList(String.Format("RegistrationID = {0} AND IsDeleted = 0", registration.RegistrationID), ctx);

                        if (listEntityPA.Count > 0)
                        {
                            foreach (PatientAccompany entityPA in listEntityPA)
                            {
                                Bed entityBed = entityBedDao.Get(Convert.ToInt32(entityPA.BedID));
                                if (entityBed.GCBedStatus == Constant.BedStatus.UNOCCUPIED)
                                {
                                    entityBed.RegistrationID = registration.RegistrationID;
                                    entityBed.IsPatientAccompany = true;
                                    entityBed.GCBedStatus = Constant.BedStatus.OCCUPIED;
                                    entityBed.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityBedDao.Update(entityBed);
                                }
                                else
                                {
                                    resultAccompany += String.Format(" {0},", entityBed.BedCode);
                                }
                            }
                        }

                        #endregion

                        if (hdnParam.Value == Constant.Facility.INPATIENT)
                        {
                            if (AppSession.IsBridgingToQueue)
                            {
                                //If Bridging to Queue - Send Information
                                try
                                {
                                    VisitInfo visitInfo = new VisitInfo();
                                    vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", mainVisit), ctx).FirstOrDefault();
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
                        }

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityRegistrationDao.Update(registration);

                        if (hdnSelectedHealthcareServiceUnitID.Value.Length > 0)
                        {
                            hdnSelectedHealthcareServiceUnitID.Value = hdnSelectedHealthcareServiceUnitID.Value.Remove(hdnSelectedHealthcareServiceUnitID.Value.Length - 1);
                            hdnSelectedRoomID.Value = hdnSelectedRoomID.Value.Remove(hdnSelectedRoomID.Value.Length - 1);
                            hdnSelectedClassID.Value = hdnSelectedClassID.Value.Remove(hdnSelectedClassID.Value.Length - 1);
                        }
                    }
                    else
                    {
                        resultReg = "fail|StatusChanged";
                        flagResult = false;
                        break;
                    }
                }

                resultAccompany += " Cannot Be Used";

                if (resultRegTemp != "")
                {
                    resultRegTemp = resultRegTemp.Remove(resultRegTemp.Length - 1);
                    resultReg = string.Format("fail|{0}", string.Format("Bed {0} Cannot Be Used", resultRegTemp));
                }

                if (resultReg.Contains("fail"))
                {
                    resultReg += " & " + resultAccompany;
                    if (resultReg.Contains("StatusChanged"))
                    {
                        resultReg = "fail|Tidak dapat dibatalkan pulang karena status registrasi sudah berubah. Harap refresh halaman ini.";
                    }
                }

                result = resultReg;

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
                Helper.InsertErrorLog(ex);
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