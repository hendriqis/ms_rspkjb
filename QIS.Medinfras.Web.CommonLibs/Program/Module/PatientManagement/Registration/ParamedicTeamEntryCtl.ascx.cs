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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ParamedicTeamEntryCtl : BaseViewPopupCtl
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
            
            List<StandardCode> lstParamedicRole = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0 AND StandardCodeID <> '{1}'", Constant.StandardCode.PARAMEDIC_ROLE, GCTypeDPJP));
            Methods.SetComboBoxField<StandardCode>(cboParamedicRole, lstParamedicRole, "StandardCodeName", "StandardCodeID");

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
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ParamedicTeamDao entityDao = new ParamedicTeamDao(ctx);
            try
            {
                List<ParamedicTeam> entityTeam = BusinessLayer.GetParamedicTeamList(string.Format("RegistrationID = {0} AND ParamedicID = {1} AND IsDeleted = 0", hdnRegistrationID.Value, hdnParamedicID.Value));
                if (entityTeam.Count < 1)
                {
                    ParamedicTeam entity = new ParamedicTeam();
                    ControlToEntity(entity);
                    entity.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entityDao.Insert(entity);
                    ctx.CommitTransaction();
                    result = true;
                }
                else
                {
                    errMessage = "Dokter yang dipilih sudah terdaftar di Tim Dokter.";
                    ctx.RollBackTransaction();
                    result = false;
                }
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

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ParamedicTeamDao entityDao = new ParamedicTeamDao(ctx);
            try
            {
                ParamedicTeam entity = BusinessLayer.GetParamedicTeam(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

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

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ParamedicTeamDao entityDao = new ParamedicTeamDao(ctx);
            try
            {
                ParamedicTeam entity = BusinessLayer.GetParamedicTeam(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

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
    }
}