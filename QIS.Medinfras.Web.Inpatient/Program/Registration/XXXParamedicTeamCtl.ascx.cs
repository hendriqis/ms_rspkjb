using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class XXXParamedicTeamCtl : BaseViewPopupCtl
    {
        protected string GCTypeRMO = string.Empty;

        protected string GCTypeDPJP = string.Empty;

        public override void InitializeDataControl(string param)
        {
            hdnRegistrationID.Value = param;
            vRegistration header = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value)).FirstOrDefault();
            txtRegistrationNo.Text = header.RegistrationNo;
            txtPatientName.Text = header.PatientName;
            hdnMainParamedicID.Value = header.ParamedicID.ToString();
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}', '{1}')", Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_RMO, Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_DPJP));
            GCTypeDPJP = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_DPJP).FirstOrDefault().ParameterValue;
            GCTypeRMO = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_RMO).FirstOrDefault().ParameterValue;
            List<StandardCode> lstCusttype = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0 AND StandardCodeID <> '{1}'",
                    Constant.StandardCode.PARAMEDIC_ROLE, GCTypeDPJP));
            Methods.SetComboBoxField<StandardCode>(cboParamedicRole, lstCusttype, "StandardCodeName", "StandardCodeID");

            Helper.SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboParamedicRole, new ControlEntrySetting(true, true, true), "mpEntryPopup");

            BindGridView();
        }

        private void BindGridView()
        {
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}', '{1}')", Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_RMO, Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_DPJP));
            GCTypeDPJP = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_DPJP).FirstOrDefault().ParameterValue;
            GCTypeRMO = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_RMO).FirstOrDefault().ParameterValue;
            if (chkDisplayRMO.Checked)
            {
                grdView.DataSource = BusinessLayer.GetvParamedicTeamList(string.Format("RegistrationID = {0} AND IsDeleted = 0", hdnRegistrationID.Value));
            }
            else
            {
                grdView.DataSource = BusinessLayer.GetvParamedicTeamList(string.Format("RegistrationID = {0} AND GCParamedicRole != '{1}' AND IsDeleted = 0", hdnRegistrationID.Value, GCTypeRMO));
            }
            grdView.DataBind();
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnIsAdd.Value.ToString() == "0")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;

        }

        private void ControlToEntity(ParamedicTeam entity)
        {
            entity.GCParamedicRole = cboParamedicRole.Value.ToString();
            entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ParamedicTeam entity = new ParamedicTeam();
                ControlToEntity(entity);
                entity.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertParamedicTeam(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                ParamedicTeam entity = BusinessLayer.GetParamedicTeamList(string.Format("RegistrationID = {0} AND ParamedicID = {1}", Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnParamedicID.Value))).FirstOrDefault();
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateParamedicTeam(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                ParamedicTeam entity = BusinessLayer.GetParamedicTeamList(string.Format("RegistrationID = {0} AND ParamedicID = {1}", Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnParamedicID.Value))).FirstOrDefault();
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateParamedicTeam(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}