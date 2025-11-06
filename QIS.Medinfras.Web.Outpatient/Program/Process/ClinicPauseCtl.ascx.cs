using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ClinicPauseCtl : BaseViewPopupCtl
    {
        string isRadiologi = "";
        int paramedicID = 0;
        int hsuID = 0;
        string departmentID = string.Empty;
        int tempDate = 0;
        int operationalTimeID = 0;
        public override void InitializeDataControl(string param)
        {
            string[] parameter = param.Split('|');
            hdnParamedicID.Value = parameter[0];
            hdnHealthcareServiceUnitID.Value = parameter[1];
            hdnDepartmentID.Value = parameter[2];
            hdnTempDate.Value = parameter[3];
            hdnOperationalTimeID.Value = parameter[4];

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.CLINIC_PAUSE_REASON));
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboPauseReason, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.CLINIC_PAUSE_REASON).ToList(), "StandardCodeName", "StandardCodeID");
            cboPauseReason.SelectedIndex = 0;

            hdnIsBridgingToGateway.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).ParameterValue;
            hdnProviderGatewayService.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).ParameterValue;

        }

        protected void cbpPrintPatientLabel_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    result = "changepage";
                }
                else if (param[0] == "pause")
                {
                    OnPauseClinicRecord(hdnParamedicID.Value, hdnHealthcareServiceUnitID.Value, hdnDepartmentID.Value, hdnTempDate.Value, hdnOperationalTimeID.Value);
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void OnPauseClinicRecord(string paramedicID, string hsuID, string departmentID, string tempDate, string operationalTimeID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ClinicServiceStatusDao entityDao = new ClinicServiceStatusDao(ctx);

            try
            {
                ClinicServiceStatus entity = BusinessLayer.GetClinicServiceStatusList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1}", hsuID, paramedicID)).FirstOrDefault();
                if (entity != null)
                {
                    if (entity.GCClinicStatus == Constant.ClinicStatus.STARTED)
                    {
                        entity.GCClinicStatus = Constant.ClinicStatus.PAUSED;
                        entity.GCPausedReason = cboPauseReason.Value.ToString();
                        entity.OtherPausedReason = txtPauseReason.Text;
                        entity.PauseDate = DateTime.Now;
                        entity.PauseTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        entity.PauseBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);
                        ctx.CommitTransaction();
                        result = true;

                        if (hdnIsBridgingToGateway.Value == "1")
                        {
                            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                            {
                                string filterExpression = string.Format("GCVisitStatus = '{0}' AND HealthcareServiceUnitID = {1} AND ParamedicID = {2} AND VisitDate = '{3}' ORDER BY VisitID DESC", Constant.VisitStatus.RECEIVING_TREATMENT, hsuID, paramedicID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
                                ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(filterExpression).FirstOrDefault();
                                if (entityCV != null)
                                {
                                    SendNotification("2", entityCV.QueueNo.ToString(), entityCV.HealthcareServiceUnitID.ToString(), entityCV.ParamedicID.ToString(), txtPauseReason.Text);
                                }
                            }
                        }
                    }
                }
                else
                {
                    ctx.RollBackTransaction();
                    result = false;
                }
            }
            catch
            {
                ctx.RollBackTransaction();
                result = false;
            }
            finally
            {
                ctx.Close();
            }
        }

        private void SendNotification(string jenisNotif, string queueNo, string hsuID, string paramedicID, string pauseReason)
        {
            GatewayService oService = new GatewayService();
            APIMessageLog entityAPILog = new APIMessageLog();
            string apiResult = oService.OnChangeClinicStatus(jenisNotif, queueNo, hsuID, paramedicID, pauseReason);
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