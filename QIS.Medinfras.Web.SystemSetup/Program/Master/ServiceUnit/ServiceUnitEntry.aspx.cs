using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ServiceUnitEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            String DepartmentID = hdnDepartmentID.Value;
            switch (DepartmentID)
            {
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.SystemSetup.CLINIC;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.SystemSetup.WARD;
                case Constant.Facility.DIAGNOSTIC: return Constant.MenuCode.SystemSetup.DIAGNOSTIC_SUPPORT;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.SystemSetup.PHARMACY;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.SystemSetup.MEDICAL_CHECKUP;
                default: return Constant.MenuCode.SystemSetup.EMERGENCY;
            }
        }

        protected String GetPageTitle()
        {
            String DepartmentID = hdnDepartmentID.Value;
            switch (DepartmentID)
            {
                case Constant.Facility.OUTPATIENT: return GetLabel("Klinik");
                case Constant.Facility.INPATIENT: return GetLabel("Ruang Perawatan");
                case Constant.Facility.DIAGNOSTIC: return GetLabel("Penunjang Medis");
                case Constant.Facility.PHARMACY: return GetLabel("Farmasi");
                case Constant.Facility.MEDICAL_CHECKUP: return GetLabel("Medical Check Up");
                default: return GetLabel("Unit Gawat Darurat");
            }
        }

        protected override void InitializeDataControl()
        {
            GetSettingParameter();

            String[] param = Request.QueryString["id"].Split('|');
            hdnDepartmentID.Value = param[0];
            if (param.Length > 1)
            {
                IsAdd = false;
                Int32 serviceUnitID = Convert.ToInt32(param[1]);
                hdnID.Value = serviceUnitID.ToString();
                ServiceUnitMaster entity = BusinessLayer.GetServiceUnitMaster(Convert.ToInt32(serviceUnitID));
                HealthcareServiceUnit entityHSU = BusinessLayer.GetHealthcareServiceUnitList(string.Format("ServiceUnitID = {0} AND HealthcareID = '{1}'", entity.ServiceUnitID, AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                if (entityHSU != null)
                {
                    hdnHealthcareServiceUnitID.Value = entityHSU.HealthcareServiceUnitID.ToString();
                }
                else
                {
                    hdnHealthcareServiceUnitID.Value = "0";
                    hdnHealthcareServiceUnitICUID.Value = "0";
                    hdnHealthcareServiceUnitPICUID.Value = "0";
                    hdnIsBridgingToMedinfrasMobileApps.Value = "0";
                }

                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtServiceUnitCode.Focus();

            if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
            {
                chkIsLaboratoryUnit.Attributes.Add("style", "display:none");
                chkIsNutritionUnit.Attributes.Add("style", "display:none");
                chkIsIntensiveUnit.Attributes.Add("style", "display:none");
                chkIsHasDiagnosticResult.Attributes.Add("style", "display:none");
                chkIsAllowMultiVisitSchedule.Attributes.Add("style", "display:none");
            }
            else if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                chkIsLaboratoryUnit.Attributes.Add("style", "display:none");
                chkIsNutritionUnit.Attributes.Add("style", "display:none");
                chkIsIntensiveUnit.Attributes.Remove("style");
                chkIsHasDiagnosticResult.Attributes.Add("style", "display:none");
                chkIsAllowMultiVisitSchedule.Attributes.Add("style", "display:none");
            }
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                chkIsLaboratoryUnit.Attributes.Remove("style");
                chkIsNutritionUnit.Attributes.Remove("style");
                chkIsIntensiveUnit.Attributes.Add("style", "display:none");
                chkIsHasDiagnosticResult.Attributes.Remove("style");

                if (hdnIsAllowMultiVisitSchedule.Value == "1")
                {
                    chkIsAllowMultiVisitSchedule.Attributes.Remove("style");
                }
                else
                {
                    chkIsAllowMultiVisitSchedule.Attributes.Add("style", "display:none");
                }
            }
            else if (hdnDepartmentID.Value == Constant.Facility.PHARMACY)
            {
                chkIsLaboratoryUnit.Attributes.Add("style", "display:none");
                chkIsNutritionUnit.Attributes.Add("style", "display:none");
                chkIsIntensiveUnit.Attributes.Add("style", "display:none");
                chkIsHasDiagnosticResult.Attributes.Add("style", "display:none");
                chkIsAllowMultiVisitSchedule.Attributes.Add("style", "display:none");
            }
            else if (hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP)
            {
                chkIsLaboratoryUnit.Attributes.Add("style", "display:none");
                chkIsNutritionUnit.Attributes.Add("style", "display:none");
                chkIsIntensiveUnit.Attributes.Add("style", "display:none");
                chkIsHasDiagnosticResult.Attributes.Add("style", "display:none");
                chkIsAllowMultiVisitSchedule.Attributes.Add("style", "display:none");
            }
            else
            {
                chkIsLaboratoryUnit.Attributes.Add("style", "display:none");
                chkIsNutritionUnit.Attributes.Add("style", "display:none");
                chkIsIntensiveUnit.Attributes.Add("style", "display:none");
                chkIsHasDiagnosticResult.Attributes.Add("style", "display:none");
                chkIsAllowMultiVisitSchedule.Attributes.Add("style", "display:none");
            }
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}')",
                    AppSession.UserLogin.HealthcareID, //0
                    Constant.SettingParameter.IP_ICU_SERVICE_UNIT_ID, //1
                    Constant.SettingParameter.IP_NICU_SERVICE_UNIT_ID, //2
                    Constant.SettingParameter.IP_PICU_SERVICE_UNIT_ID, //3
                    Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS, //4
                    Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE //5
                ));
            
            hdnHealthcareServiceUnitICUID.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_ICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
            hdnHealthcareServiceUnitNICUID.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_NICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
            hdnHealthcareServiceUnitPICUID.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_PICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
            hdnIsBridgingToMedinfrasMobileApps.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
            hdnIsAllowMultiVisitSchedule.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE).FirstOrDefault().ParameterValue;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtServiceUnitName2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtShortName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtServiceInterval, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(chkIsUsingJobOrder, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAllowRegistration, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHasPrescription, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsLaboratoryUnit, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsNutritionUnit, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsIntensiveUnit, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHasDiagnosticResult, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAllowMultiVisitSchedule, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGLAccountSegmentNo, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtIHSLocationID, new ControlEntrySetting(true, true, false, 0));
        }

        private void EntityToControl(ServiceUnitMaster entity)
        {
            txtServiceUnitCode.Text = entity.ServiceUnitCode;
            txtServiceUnitName.Text = entity.ServiceUnitName;
            txtServiceUnitName2.Text = entity.ServiceUnitName2;
            txtShortName.Text = entity.ShortName;
            txtServiceInterval.Text = entity.ServiceInterval.ToString();
            txtIHSLocationID.Text = entity.IHSLocationID;
            chkIsUsingJobOrder.Checked = entity.IsUsingJobOrder;
            chkIsAllowRegistration.Checked = entity.IsUsingRegistration;
            chkIsHasPrescription.Checked = entity.IsHasPrescription;
            chkIsLaboratoryUnit.Checked = entity.IsLaboratoryUnit;
            chkIsNutritionUnit.Checked = entity.IsNutritionUnit;
            chkIsIntensiveUnit.Checked = entity.IsIntensiveUnit;
            chkIsHasDiagnosticResult.Checked = entity.IsHasDiagnosticResult;
            chkIsAllowMultiVisitSchedule.Checked = entity.IsAllowMultiVisitSchedule;

            if (!String.IsNullOrEmpty(entity.BPJSPoli))
            {
                if (entity.BPJSPoli.Contains("|"))
                {
                    string[] bpjsInfo = entity.BPJSPoli.Split('|');
                    txtVKlaimPoliCode.Text = bpjsInfo[0];
                    txtVKlaimPoliName.Text = bpjsInfo[1];
                }
            }
            else
            {
                txtVKlaimPoliCode.Text = string.Empty;
                txtVKlaimPoliName.Text = string.Empty;
            }
            if (!String.IsNullOrEmpty(entity.InhealthPoli))
            {
                if (entity.InhealthPoli.Contains("|"))
                {
                    string[] inhealthInfo = entity.InhealthPoli.Split('|');
                    txtInhealthKodePoli.Text = inhealthInfo[0];
                    txtInhealthNamaPoli.Text = inhealthInfo[1];
                }
            }
            else
            {
                txtInhealthKodePoli.Text = string.Empty;
                txtInhealthNamaPoli.Text = string.Empty;
            }
            txtGLAccountSegmentNo.Text = entity.GLAccountNoSegment;
        }

        private void ControlToEntity(ServiceUnitMaster entity)
        {
            entity.ServiceUnitCode = txtServiceUnitCode.Text;
            entity.ServiceUnitName = txtServiceUnitName.Text;
            entity.ServiceUnitName2 = txtServiceUnitName2.Text;
            entity.ShortName = txtShortName.Text;
            entity.ServiceInterval = Convert.ToByte(txtServiceInterval.Text);
            entity.IHSLocationID = txtIHSLocationID.Text;
            entity.DepartmentID = hdnDepartmentID.Value;
            entity.IsUsingJobOrder = chkIsUsingJobOrder.Checked;
            entity.IsUsingRegistration = chkIsAllowRegistration.Checked;
            entity.IsHasPrescription = chkIsHasPrescription.Checked;
            entity.IsLaboratoryUnit = chkIsLaboratoryUnit.Checked;
            entity.IsNutritionUnit = chkIsNutritionUnit.Checked;
            entity.IsIntensiveUnit = chkIsIntensiveUnit.Checked;
            entity.IsHasDiagnosticResult = chkIsHasDiagnosticResult.Checked;

            if (hdnIsAllowMultiVisitSchedule.Value == "1")
            {
                entity.IsAllowMultiVisitSchedule = chkIsAllowMultiVisitSchedule.Checked;
            }
            else
            {
                entity.IsAllowMultiVisitSchedule = false;
            }

            entity.GLAccountNoSegment = txtGLAccountSegmentNo.Text;

            if (!string.IsNullOrEmpty(txtVKlaimPoliCode.Text))
            {
                entity.BPJSPoli = string.Format("{0}|{1}", txtVKlaimPoliCode.Text, txtVKlaimPoliName.Text);
            }
            else
            {
                entity.BPJSPoli = "";
            }

            if (!string.IsNullOrEmpty(txtInhealthKodePoli.Text))
            {
                entity.InhealthPoli = string.Format("{0}|{1}", txtInhealthKodePoli.Text, txtInhealthNamaPoli.Text);
            }
            else
            {
                entity.InhealthPoli = "";
            }
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ServiceUnitCode = '{0}'", txtServiceUnitCode.Text);
            List<ServiceUnitMaster> lst = BusinessLayer.GetServiceUnitMasterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Service Unit with Code " + txtServiceUnitCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 ID = Convert.ToInt32(hdnID.Value);
            string FilterExpression = string.Format("ServiceUnitCode = '{0}' AND ServiceUnitID != {1}", txtServiceUnitCode.Text, ID);
            List<ServiceUnitMaster> lst = BusinessLayer.GetServiceUnitMasterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Service Unit with Code " + txtServiceUnitCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            ServiceUnitMasterDao entityDao = new ServiceUnitMasterDao(ctx);
            bool result = false;
            try
            {
                ServiceUnitMaster entity = new ServiceUnitMaster();
                ControlToEntity(entity);
                entity.DepartmentID = hdnDepartmentID.Value;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();
                ctx.CommitTransaction();
                BridgingToMedinfrasMobileApps(retval, entity.ServiceUnitName, entity.ServiceUnitName2, entity.ShortName, "001");
                result = true;
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
            try
            {
                ServiceUnitMaster entity = BusinessLayer.GetServiceUnitMaster(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateServiceUnitMaster(entity);
                BridgingToMedinfrasMobileApps(entity.ServiceUnitID.ToString(), entity.ServiceUnitName, entity.ServiceUnitName2, entity.ShortName, "002");
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private void BridgingToMedinfrasMobileApps(string serviceUnitID, string serviceUnitName, string serviceUnitName2, string shortName, string eventType)
        {

            if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
            {
                if (serviceUnitID != string.Empty)
                {
                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                    APIMessageLog entityAPILog = new APIMessageLog();
                    string apiResult = oService.OnServiceUnitMasterChanged(serviceUnitID, serviceUnitName, serviceUnitName2, shortName, eventType);
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.Response = apiResultInfo[1];
                        Exception ex = new Exception(apiResultInfo[1]);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                    {
                        entityAPILog.MessageText = apiResultInfo[0];
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                }
            }
        }
    }
}