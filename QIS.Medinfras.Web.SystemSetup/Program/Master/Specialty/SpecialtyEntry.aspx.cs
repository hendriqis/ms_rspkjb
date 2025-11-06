using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class SpecialtyEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.SPECIALTY;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String specialtyID = Request.QueryString["id"];
                hdnID.Value = specialtyID.ToString();
                Specialty entity = BusinessLayer.GetSpecialty(specialtyID);
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtSpecialtyID.Focus();
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS));
            hdnIsBridgingToMedinfrasMobileApps.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtSpecialtyID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtSpecialtyName, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(Specialty entity)
        {
            txtSpecialtyID.Text = entity.SpecialtyID;
            txtSpecialtyName.Text = entity.SpecialtyName;
        }

        private void ControlToEntity(Specialty entity)
        {
            entity.SpecialtyID = txtSpecialtyID.Text;
            entity.SpecialtyName = txtSpecialtyName.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("SpecialtyID = '{0}'", txtSpecialtyID.Text);
            List<Specialty> lst = BusinessLayer.GetSpecialtyList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Specialty with Code " + txtSpecialtyID.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                Specialty entity = new Specialty();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertSpecialty(entity);
                retval = entity.SpecialtyID;
                BridgingToMedinfrasMobileApps(entity.SpecialtyID, entity.SpecialtyName, entity.SpecialtyName2, "001");
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                Specialty entity = BusinessLayer.GetSpecialty(hdnID.Value);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateSpecialty(entity);
                BridgingToMedinfrasMobileApps(entity.SpecialtyID, entity.SpecialtyName, entity.SpecialtyName2, "002");
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private void BridgingToMedinfrasMobileApps(string specialtyID, string specialtyName, string specialtyName2, string eventType)
        {
            if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
            {

                if (specialtyID != string.Empty)
                {
                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                    APIMessageLog entityAPILog = new APIMessageLog();
                    string apiResult = oService.OnSpecialtyMasterChanged(specialtyID, specialtyName, specialtyName2, eventType);
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