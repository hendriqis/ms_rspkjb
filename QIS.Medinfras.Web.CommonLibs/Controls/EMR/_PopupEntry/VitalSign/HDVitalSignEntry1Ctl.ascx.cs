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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class HDVitalSignEntry1Ctl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnUserID.Value = AppSession.UserLogin.UserID.ToString();

            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                IsAdd = paramInfo[0] == "0";
                hdnVisitID.Value = paramInfo[1];
                hdnMRN.Value = paramInfo[2];
                hdnID.Value = paramInfo[3];
                OnControlEntrySettingLocal();
                ReInitControl();
                if (!IsAdd)
                {
                    VitalSignHd entity = BusinessLayer.GetVitalSignHd(Convert.ToInt32(hdnID.Value));
                    List<vVitalSignDt> entityDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0}", hdnID.Value));
                    EntityToControl(entity, entityDt);
                }
            }
            else
            {
                OnControlEntrySettingLocal();
                ReInitControl();
                hdnID.Value = "0";
                IsAdd = true;
            }
        }

        private void OnControlEntrySettingLocal()
        {
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType != '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1})", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            if (AppSession.UserLogin.ParamedicID != null)
            {
                cboParamedicID.ClientEnabled = false;
                List<vParamedicMaster> lstParamLogin = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType != '{0}' AND ParamedicID IN ({1})", Constant.ParamedicType.Physician, AppSession.UserLogin.ParamedicID));
                Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamLogin, "ParamedicName", "ParamedicID");
                cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            }

            SetControlEntrySetting(txtLogDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtLogTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtQB, new ControlEntrySetting(true, true, false, "0"));
            SetControlEntrySetting(txtQInf, new ControlEntrySetting(true, true, false, "0"));
            SetControlProperties();
        }

        private void EntityToControl(VitalSignHd entity, List<vVitalSignDt> lstEntityDt)
        {
            txtLogDate.Text = entity.ObservationDate.ToString();
            txtLogTime.Text = entity.ObservationTime;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            txtRemarks.Text = entity.Remarks;

            #region Vital Sign Dt
            foreach (vVitalSignDt item in lstEntityDt)
            {

                //HtmlInputHidden hdnVitalSignID = (HtmlInputHidden)item.FindControl("hdnVitalSignID");
                //HtmlInputHidden hdnVitalSignType = (HtmlInputHidden)item.FindControl("hdnVitalSignType");

                //vVitalSignDt entityDt = lstEntityDt.FirstOrDefault(p => p.VitalSignID == Convert.ToInt32(hdnVitalSignID.Value));
                //if (entityDt != null)
                //{
                //    if (hdnVitalSignType.Value == Constant.ControlType.TEXT_BOX)
                //    {
                //        TextBox txt = (TextBox)item.FindControl("txtVitalSignType");
                //        txt.Text = entityDt.VitalSignValue;
                //    }
                //    else if (hdnVitalSignType.Value == Constant.ControlType.COMBO_BOX)
                //    {
                //        ASPxComboBox ddl = (ASPxComboBox)item.FindControl("cboVitalSignType");
                //        ddl.Value = entityDt.GCVitalSignValue;
                //    }
                //}
            }
            #endregion
        }

        private void ControlToEntity(PatientMedicalDevice entity)
        {
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PatientMedicalDeviceDao deviceDao = new PatientMedicalDeviceDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);

            try
            {
                PatientMedicalDevice entity = new PatientMedicalDevice();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                deviceDao.InsertReturnPrimaryKeyID(entity);


                Patient oPatient = patientDao.Get(entity.MRN);
                if (!oPatient.IsUsingImplant)
                {
                    #region Update Patient Status - Using Implant
                    oPatient.IsUsingImplant = true;
                    oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                    patientDao.Update(oPatient);
                    #endregion
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            try
            {
                PatientMedicalDevice entityUpdate = BusinessLayer.GetPatientMedicalDevice(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientMedicalDevice(entityUpdate);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }
    }
}