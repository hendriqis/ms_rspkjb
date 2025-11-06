using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program.PatientPage
{
    public partial class PatientReferralEntryCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            SetControlProperties();
            if (string.IsNullOrEmpty(param) || param == "0")
            {
                IsAdd = true;
            }
            else
            {
                IsAdd = false;
                PatientReferral obj = BusinessLayer.GetPatientReferralList(string.Format("VisitID = {0} AND ID = {1}", AppSession.RegisteredPatient.VisitID, param)).FirstOrDefault();
                if (obj != null)
                {
                    hdnID.Value = obj.ID.ToString();
                    EntityToControl(obj);
                }
                else
                {
                    hdnID.Value = "0";
                }
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtReferralDate, new ControlEntrySetting(true, true, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtReferralTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboPhysician2, new ControlEntrySetting(true, false, true));
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.OUTPATIENT)
            {
                SetControlEntrySetting(cboClinic, new ControlEntrySetting(true, false, true));
            }
            else
            {
                SetControlEntrySetting(cboClinic, new ControlEntrySetting(true, false, false));
            }
            SetControlEntrySetting(cboReferralType, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtDiagnosisText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMedicalResumeText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPlanningResumeText, new ControlEntrySetting(true, true, true));
        }
        protected void cbpPhysician_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindServiceUnitPhysician();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        private void BindServiceUnitPhysician()
        {

            string filterexpression = string.Format("HealthcareID = '{0}' AND DepartmentID = 'OUTPATIENT' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID);
            if (cboPhysician2.Value != null)
            {
                filterexpression += string.Format(" AND ParamedicID='{0}'", cboPhysician2.Value.ToString());
            }
            List<vServiceUnitParamedic> lstServiceUnit = BusinessLayer.GetvServiceUnitParamedicList(filterexpression);
            Methods.SetComboBoxField<vServiceUnitParamedic>(cboClinic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboClinic.SelectedIndex = 0;
        }
        private void SetControlProperties()
        {
            #region Setting Parameter
            string filterSetvar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}', '{2}', '{3}')",
                    AppSession.UserLogin.HealthcareID,
                    Constant.SettingParameter.EM0067,
                    Constant.SettingParameter.EM0068,
                    Constant.SettingParameter.EM_Display_Next_Visit_Schedule
                    );
            List<SettingParameterDt> lstSetpardt = BusinessLayer.GetSettingParameterDtList(filterSetvar).ToList();
            hdnIsDefaultServiceUnitParamedic.Value = lstSetpardt.Where(p => p.ParameterCode == Constant.SettingParameter.EM0067).FirstOrDefault().ParameterValue;
            hdnIsDefaultAppointment.Value = lstSetpardt.Where(p => p.ParameterCode == Constant.SettingParameter.EM0068).FirstOrDefault().ParameterValue;
            hdnIsAllowCreateAppointment.Value = lstSetpardt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM_Display_Next_Visit_Schedule).ParameterValue;

            #endregion

            #region Physician Combobox
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND IsDeleted = 0", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic, "ParamedicName", "ParamedicID");
            List<vParamedicMaster> lstParamedic2 = lstParamedic.Where(lst => lst.ParamedicID != AppSession.UserLogin.ParamedicID && lst.IsDeleted == false).ToList();
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician2, lstParamedic2, "ParamedicName", "ParamedicID");

            cboPhysician.Value = AppSession.UserLogin.ParamedicID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboPhysician.ClientEnabled = false;
                cboPhysician.Value = userLoginParamedic.ToString();
            }

            cboPhysician.Enabled = false;
            #endregion

            List<vConsultVisit> dataVisitID = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0}) OR RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID));
            string lstVisit = "";
            if (dataVisitID != null)
            {
                foreach (vConsultVisit visit in dataVisitID)
                {
                    if (lstVisit != "")
                    {
                        lstVisit += ",";
                    }
                    lstVisit += visit.VisitID;
                }
            }

            hdnLinkedVisitID.Value = string.Format("({0})", lstVisit);

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
           
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.OUTPATIENT)
            {
                List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = 'OUTPATIENT' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID));
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboClinic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                trClinic.Attributes.Remove("style");
            }
            else
            {
                trClinic.Attributes.Add("style", "display:none");
            }

            if (IsAdd)
            {
                cboPhysician2.Value = string.Empty;
                txtDiagnosisText.Text = string.Empty;
                txtMedicalResumeText.Text = string.Empty;
                txtPlanningResumeText.Text = string.Empty;
            }

            string filterReferralType = string.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.REFERRAL_TYPE);
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                filterReferralType += string.Format(" AND StandardCodeID IN ('{0}','{1}')", Constant.ReferralType.RAWAT_BERSAMA, Constant.ReferralType.KONSULTASI);
            }
            else
            {
                filterReferralType += string.Format(" AND StandardCodeID != '{0}'", Constant.ReferralType.RAWAT_BERSAMA);
            }
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterReferralType);
            Methods.SetComboBoxField<StandardCode>(cboReferralType, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.REFERRAL_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            //pakai setvar 
            if (hdnIsDefaultAppointment.Value == "1")
            {
                int index = lstStandardCode.FindLastIndex(lst => lst.StandardCodeID == Constant.ReferralType.APPOINTMENT);
                cboReferralType.SelectedIndex = index;
            }

        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            errMessage = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PatientReferralDao referralDao = new PatientReferralDao(ctx);
            ConsultVisitDao cvDao = new ConsultVisitDao(ctx);
            ParamedicTeamDao teamDao = new ParamedicTeamDao(ctx);
            try
            {
                PatientReferral entity = new PatientReferral();
                ControlToEntity(entity);
                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
                {
                    entity.IsProceed = true;
                    entity.ProceedDateTime = DateTime.Now;
                    entity.ProceedBy = AppSession.UserLogin.UserID;
                }
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                referralDao.Insert(entity);

                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.OUTPATIENT)
                {
                    ConsultVisit entityCV = cvDao.Get(entity.VisitID);
                    entityCV.GCDischargeCondition = Constant.PatientOutcome.BELUM_SEMBUH;
                    entityCV.GCDischargeMethod = Constant.DischargeMethod.REFFERRED_TO_OUTPATIENT;
                    entityCV.ReferralUnitID = Convert.ToInt32(cboClinic.Value.ToString());
                    entityCV.ReferralPhysicianID = Convert.ToInt32(cboPhysician2.Value.ToString());
                    entityCV.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    cvDao.Update(entityCV);
                }

                if (entity.GCRefferalType == Constant.ReferralType.KONSULTASI || entity.GCRefferalType == Constant.ReferralType.RAWAT_BERSAMA || entity.GCRefferalType == Constant.ReferralType.HANDOVER)
                {
                    List<ParamedicTeam> entityTeam = BusinessLayer.GetParamedicTeamList(string.Format("RegistrationID = {0} AND ParamedicID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID, Convert.ToInt32(cboPhysician2.Value.ToString())), ctx);
                    if (entityTeam.Count < 1)
                    {
                        ParamedicTeam obj = new ParamedicTeam();
                        obj.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
                        obj.ParamedicID = Convert.ToInt32(cboPhysician2.Value.ToString());
                        obj.GCParamedicRole = Constant.ParamedicRole.KONSULEN;
                        obj.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        teamDao.Insert(obj);
                    }
                }

                ctx.CommitTransaction();
                return result;
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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            errMessage = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PatientReferralDao referralDao = new PatientReferralDao(ctx);
            ConsultVisitDao cvDao = new ConsultVisitDao(ctx);
            ParamedicTeamDao teamDao = new ParamedicTeamDao(ctx);
            try
            {
                PatientReferral entity = referralDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
                {
                    entity.IsProceed = true;
                    entity.ProceedDateTime = DateTime.Now;
                    entity.ProceedBy = AppSession.UserLogin.UserID;
                }
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                referralDao.Update(entity);

                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.OUTPATIENT)
                {
                    ConsultVisit entityCV = cvDao.Get(entity.VisitID);
                    entityCV.GCDischargeCondition = Constant.PatientOutcome.BELUM_SEMBUH;
                    entityCV.GCDischargeMethod = Constant.DischargeMethod.REFFERRED_TO_OUTPATIENT;
                    entityCV.ReferralUnitID = Convert.ToInt32(cboClinic.Value.ToString());
                    entityCV.ReferralPhysicianID = Convert.ToInt32(cboPhysician2.Value.ToString());
                    entityCV.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    cvDao.Update(entityCV);
                }

                if (entity.GCRefferalType == Constant.ReferralType.KONSULTASI || entity.GCRefferalType == Constant.ReferralType.RAWAT_BERSAMA || entity.GCRefferalType == Constant.ReferralType.HANDOVER)
                {
                    List<ParamedicTeam> entityTeam = BusinessLayer.GetParamedicTeamList(string.Format("RegistrationID = {0} AND ParamedicID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID, Convert.ToInt32(cboPhysician2.Value.ToString())), ctx);
                    if (entityTeam.Count > 0)
                    {
                        ParamedicTeam obj = teamDao.Get(entityTeam.FirstOrDefault().ID);
                        obj.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
                        obj.ParamedicID = Convert.ToInt32(cboPhysician2.Value.ToString());
                        obj.GCParamedicRole = Constant.ParamedicRole.KONSULEN;
                        obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        teamDao.Update(obj);
                    }
                }

                ctx.CommitTransaction();
                return result;
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

        private void ControlToEntity(PatientReferral entity)
        {
            entity.ReferralDate = Helper.GetDatePickerValue(txtReferralDate);
            entity.ReferralTime = txtReferralTime.Text;
            entity.GCRefferalType = cboReferralType.Value.ToString();
            entity.FromPhysicianID = Convert.ToInt32(cboPhysician.Value);
            entity.ToPhysicianID = Convert.ToInt32(cboPhysician2.Value);
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.OUTPATIENT)
            {
                entity.ToHealthcareServiceUnitID = Convert.ToInt32(cboClinic.Value.ToString());
            }
            if (entity.GCRefferalType != Constant.ReferralType.APPOINTMENT)
            {
                entity.ToVisitID = AppSession.RegisteredPatient.VisitID;
            }
            entity.DiagnosisText = txtDiagnosisText.Text;
            entity.MedicalResumeText = txtMedicalResumeText.Text;
            entity.PlanningResumeText = txtPlanningResumeText.Text;
        }

        private void EntityToControl(PatientReferral obj)
        {
            cboPhysician2.Value = obj.ToPhysicianID.ToString();
            cboReferralType.Value = obj.GCRefferalType.ToString();
            cboClinic.Value = obj.ToHealthcareServiceUnitID.ToString();
            txtDiagnosisText.Text = obj.DiagnosisText;
            txtMedicalResumeText.Text = obj.MedicalResumeText;
            txtPlanningResumeText.Text = obj.PlanningResumeText;
        }
    }
}