using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class ParamedicTeamList : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PHYSICIAN_TEAM;
        }

        protected override void InitializeDataControl()
        {
            hdnRegisteredParamedic.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
            BindGridView();
        }

        #region List - Handler
        private void BindGridView()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("RegistrationID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID);

            List<vParamedicTeam> lstEntity = BusinessLayer.GetvParamedicTeamList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');

                BindGridView();
                result = "refresh|1";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Entry
        protected override void SetControlProperties()
        {
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}', '{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_RMO, Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_DPJP));
            string GCTypeDPJP = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_DPJP).FirstOrDefault().ParameterValue;
            string GCTypeRMO = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_RMO).FirstOrDefault().ParameterValue;

            List<StandardCode> lstParamedicRole = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0 AND StandardCodeID <> '{1}'", Constant.StandardCode.PARAMEDIC_ROLE, GCTypeDPJP));
            Methods.SetComboBoxField<StandardCode>(cboPhysicianRole, lstParamedicRole, "StandardCodeName", "StandardCodeID");
            cboPhysicianRole.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboPhysicianRole, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, true));
        }

        private void ControlToEntity(ParamedicTeam entity)
        {
            entity.GCParamedicRole = cboPhysicianRole.Value.ToString();
            if (hdnParamedicID.Value != null && hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
            {
                entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ParamedicTeamDao entityDao = new ParamedicTeamDao(ctx);
            try
            {
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                List<ParamedicTeam> entityTeam = BusinessLayer.GetParamedicTeamList(string.Format("RegistrationID = {0} AND ParamedicID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID, hdnParamedicID.Value), ctx);
                if (entityTeam.Count < 1)
                {
                    ParamedicTeam entity = new ParamedicTeam();
                    ControlToEntity(entity);
                    entity.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entityDao.Insert(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Dokter yang dipilih sudah terdaftar di Tim Dokter.";
                    ctx.RollBackTransaction();
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

        protected override bool OnSaveEditRecord(ref string errMessage)
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

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                ParamedicTeam entity = BusinessLayer.GetParamedicTeam(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateParamedicTeam(entity);
                return true;
            }
            return false;
        }
        #endregion
    }
}