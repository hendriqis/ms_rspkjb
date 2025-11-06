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
using DevExpress.Web.ASPxEditors;
using System.Text;
using System.IO;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MedicalAssessmentFormEntry : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                IsAdd = false;
                hdnID.Value = param;
                SetControlProperties();
                VitalSignHd entity = BusinessLayer.GetVitalSignHd(Convert.ToInt32(hdnID.Value));
                List<vVitalSignDt> entityDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0}", param));
                EntityToControl(entity, entityDt);

                PopulateFormContent();
            }
            else
            {
                hdnID.Value = "";
                IsAdd = true;
                PopulateFormContent();
                SetControlProperties();
            }
        }

        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            string fileName = string.Format(@"{0}\medicalForm\{1}", filePath, "physicalExam.html");
            IEnumerable<string> lstText = File.ReadAllLines(fileName);
            StringBuilder innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            divFormContent.InnerHtml = innerHtml.ToString();
        }

        private void SetControlProperties()
        {
            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND ParamedicID = {2}",
                Constant.ParamedicType.Nurse, AppSession.RegisteredPatient.HealthcareServiceUnitID, paramedicID));
            if (lstParamedic.Count == 0)
            {
                lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType != '{0}'", Constant.ParamedicType.Physician));
            }
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");


            if (IsAdd)
            {
                if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
                {
                    cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                }
                else
                {
                    cboParamedicID.SelectedIndex = 0;
                }
            }
            else
            {
                if (AppSession.UserLogin.GCParamedicMasterType != Constant.ParamedicType.Nurse)
                {
                    int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                    cboParamedicID.ClientEnabled = false;
                    cboParamedicID.Value = userLoginParamedic.ToString();
                }
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtObservationTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(VitalSignHd entity, List<vVitalSignDt> lstEntityDt)
        {
            txtObservationDate.Text = entity.ObservationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = entity.ObservationTime;
            if (AppSession.UserLogin.ParamedicID != null) cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            else cboParamedicID.Value = entity.ParamedicID.ToString();
        }

        private void ControlToEntity(VitalSignHd entity, List<VitalSignDt> lstEntityDt)
        {
            entity.ObservationDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.ObservationTime = txtObservationTime.Text;

            string filterExpression = string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')",
                                                        Constant.SettingParameter.VITAL_SIGN_HEIGHT,
                                                        Constant.SettingParameter.VITAL_SIGN_WEIGHT,
                                                        Constant.SettingParameter.VITAL_SIGN_BMI,
                                                        Constant.SettingParameter.VITAL_SIGN_NBPs,
                                                        Constant.SettingParameter.VITAL_SIGN_NBPd,
                                                        Constant.SettingParameter.VITAL_SIGN_MAP,
                                                        Constant.SettingParameter.VITAL_SIGN_RR,
                                                        Constant.SettingParameter.VITAL_SIGN_HR,
                                                        Constant.SettingParameter.VITAL_SIGN_SPO2,
                                                        Constant.SettingParameter.VITAL_SIGN_AVPU,
                                                        Constant.SettingParameter.VITAL_SIGN_EWS,
                                                        Constant.SettingParameter.VITAL_SIGN_TEMPERATURE,
                                                        Constant.SettingParameter.EM0018,
                                                        Constant.SettingParameter.EM0019,
                                                        Constant.SettingParameter.EM0020,
                                                        Constant.SettingParameter.EM0021);
            List<SettingParameter> lstParam = BusinessLayer.GetSettingParameterList(filterExpression);

            string _WeightVitalSignID = "0";
            string _HeightVitalSignID = "0";
            string _BMIVitalSignID = "0";
            string _NBPsVitalSignID = "0";
            string _NBPdVitalSignID = "0";
            string _MAPVitalSignID = "0";
            string _RRVitalSignID = "0";
            string _HRVitalSignID = "0";
            string _SPO2VitalSignID = "0";
            string _AVPUVitalSignID = "0";
            string _TempVitalSignID = "0";
            string _EWSVitalSignID = "0";
            string _GCSTotalEMVVitalSignID = "0";
            string _GCS_E_VitalSignID = "0";
            string _GCS_M_VitalSignID = "0";
            string _GCS_V_VitalSignID = "0";

            if (lstParam.Count > 0)
            {
                SettingParameter oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_HEIGHT).FirstOrDefault();
                if (oParam != null)
                    _HeightVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_WEIGHT).FirstOrDefault();
                if (oParam != null)
                    _WeightVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_BMI).FirstOrDefault();
                if (oParam != null)
                    _BMIVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_NBPs).FirstOrDefault();
                if (oParam != null)
                    _NBPsVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_NBPd).FirstOrDefault();
                if (oParam != null)
                    _NBPdVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_MAP).FirstOrDefault();
                if (oParam != null)
                    _MAPVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_RR).FirstOrDefault();
                if (oParam != null)
                    _RRVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_HR).FirstOrDefault();
                if (oParam != null)
                    _HRVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_SPO2).FirstOrDefault();
                if (oParam != null)
                    _SPO2VitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_AVPU).FirstOrDefault();
                if (oParam != null)
                    _AVPUVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_TEMPERATURE).FirstOrDefault();
                if (oParam != null)
                    _TempVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_EWS).FirstOrDefault();
                if (oParam != null)
                    _EWSVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM0018).FirstOrDefault();
                if (oParam != null)
                    _GCSTotalEMVVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM0019).FirstOrDefault();
                if (oParam != null)
                    _GCS_E_VitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM0020).FirstOrDefault();
                if (oParam != null)
                    _GCS_M_VitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM0021).FirstOrDefault();
                if (oParam != null)
                    _GCS_V_VitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";
            }

            bool isWeightExists = false;
            decimal weight = 0;
            bool isHeightExists = false;
            decimal height = 0;
            bool isSystolicBPExists = false;
            decimal systolicBP = 0;
            bool isDiastolicBPExists = false;
            decimal diastolicBP = 0;

            bool isRRExists = false;
            decimal rr = 0;
            bool isHRExists = false;
            decimal hr = 0;
            bool isSPO2Exists = false;
            decimal spo2 = 0;
            bool isAVPUExists = false;
            string avpu = string.Empty;
            bool isTempExists = false;
            decimal temp = 0;

            bool isGCS_E_Exists = false;
            string gcsE = string.Empty;
            bool isGCS_M_Exists = false;
            string gcsM = string.Empty;
            bool isGCS_V_Exists = false;
            string gcsV = string.Empty;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            VitalSignHdDao entityDao = new VitalSignHdDao(ctx);
            VitalSignDtDao entityDtDao = new VitalSignDtDao(ctx);
            RegistrationDao regDao = new RegistrationDao(ctx);
            try
            {
                VitalSignHd entity = new VitalSignHd();
                List<VitalSignDt> lstEntityDt = new List<VitalSignDt>();
                ControlToEntity(entity, lstEntityDt);
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.ID = entityDao.InsertReturnPrimaryKeyID(entity);

                foreach (VitalSignDt entityDt in lstEntityDt)
                {
                    entityDt.ID = entity.ID;
                    entityDtDao.Insert(entityDt);
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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            VitalSignHdDao entityDao = new VitalSignHdDao(ctx);
            VitalSignDtDao entityDtDao = new VitalSignDtDao(ctx);
            RegistrationDao regDao = new RegistrationDao(ctx);
            try
            {
                VitalSignHd entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                List<VitalSignDt> lstEntityDt = BusinessLayer.GetVitalSignDtList(string.Format("ID = {0}", hdnID.Value), ctx);
                List<VitalSignDt> lstNewEntityDt = new List<VitalSignDt>();

                ControlToEntity(entity, lstNewEntityDt);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityDao.Update(entity);

                foreach (VitalSignDt entityDt in lstNewEntityDt)
                {
                    VitalSignDt obj = lstEntityDt.FirstOrDefault(p => p.VitalSignID == entityDt.VitalSignID);
                    entityDt.ID = entity.ID;
                    if (obj == null)
                    {
                        entityDtDao.Insert(entityDt);
                    }
                    else
                    {
                        entityDtDao.Update(entityDt);
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}