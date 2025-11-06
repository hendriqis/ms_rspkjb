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
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Outpatient.Program
{
    public partial class ClinicServiceTime : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPageDate = 1;
        protected int PageCountDate = 1;
        List<vParamedicSchedule> lstParamedicSchedule = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Outpatient.CLINIC_SERVICE_TIME;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
            string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}'", AppSession.UserLogin.HealthcareID, Constant.Facility.OUTPATIENT);
            if (serviceUnitUserCount > 0)
                filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2})", Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            txtScheduleDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnIsBridgingToGateway.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).ParameterValue;
            hdnProviderGatewayService.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).ParameterValue;
            BindGridViewDate(1, true, ref PageCountDate);
        }

        private void BindGridViewDate(int pageIndexDate, bool isCountPageCount, ref int pageCountDate)
        {
            DateTime ScheduleDate = Helper.GetDatePickerValue(txtScheduleDate.Text);
            int daynumber = (int)ScheduleDate.DayOfWeek;

            if (daynumber == 0)
            {
                daynumber = 7;
            }

            string filterExpression = String.Format("IsUsingRegistration = 1 AND IsDeleted = 0 AND DepartmentID = '{0}' AND ServiceUnitID IN (SELECT ServiceUnitID FROM vHealthcareServiceUnit WHERE HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM ParamedicSchedule WHERE DayNumber = '{1}'))", Constant.Facility.OUTPATIENT, daynumber);
            string filterExpressionDate = String.Format("IsUsingRegistration = 1 AND IsDeleted = 0 AND DepartmentID = '{0}' AND ServiceUnitID IN (SELECT ServiceUnitID FROM vHealthcareServiceUnit WHERE HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM ParamedicScheduleDate WHERE ScheduleDate = '{1}'))", Constant.Facility.OUTPATIENT, ScheduleDate);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetServiceUnitMasterRowCount(filterExpression) + BusinessLayer.GetServiceUnitMasterRowCount(filterExpressionDate);
                pageCountDate = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            filterExpression += String.Format(" ORDER BY ServiceUnitName ASC");
            filterExpressionDate += String.Format("ORDER BY ServiceUnitName ASC");

            List<ServiceUnitMaster> lstEntity = BusinessLayer.GetServiceUnitMasterList(filterExpression);
            List<ServiceUnitMaster> lstEntityDate = BusinessLayer.GetServiceUnitMasterList(filterExpressionDate);
            lstEntity.AddRange(lstEntityDate);

            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpInfoParamedicScheduleDateView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCountDate = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDate(Convert.ToInt32(param[1]), false, ref pageCountDate);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDate(1, true, ref pageCountDate);
                    result = "refresh|" + pageCountDate;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            DateTime ScheduleDate = Helper.GetDatePickerValue(txtScheduleDate.Text);
            int daynumber = (int)ScheduleDate.DayOfWeek;
            int healthcareServiceUnitID = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("ServiceUnitID = {0}", Convert.ToInt32(hdnID.Value))).FirstOrDefault().HealthcareServiceUnitID;
            if (daynumber == 0)
            {
                daynumber = 7;
            }

            String FilterScheduleDate = Helper.GetDatePickerValue(txtScheduleDate).ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
            string filterExpressionDate = string.Format("TempDate IN ('{0}','{1}') AND HealthcareServiceUnitID = {2}", daynumber, FilterScheduleDate, hdnID.Value);

            List<GetParamedicScheduleClinicStatus> lstEntityDate = null;
            lstEntityDate = BusinessLayer.GetParamedicScheduleClinicStatusList(daynumber, Helper.GetDatePickerValue(txtScheduleDate).ToString(Constant.FormatString.DATE_FORMAT_112), healthcareServiceUnitID);
            lvwView.DataSource = lstEntityDate;
            lvwView.DataBind();

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    result = "changepage";
                }
                else if (param[0] == "start")
                {
                    int paramedicID = Convert.ToInt32(param[1]);
                    int hsuID = Convert.ToInt32(param[2]);
                    string departmentID = param[3];
                    int tempDate = Convert.ToInt32(param[4]);
                    int operationalTimeID = Convert.ToInt32(param[5]);
                    OnStartClinicRecord(paramedicID, hsuID, departmentID, tempDate, operationalTimeID);

                    lstEntityDate = BusinessLayer.GetParamedicScheduleClinicStatusList(daynumber, Helper.GetDatePickerValue(txtScheduleDate).ToString(Constant.FormatString.DATE_FORMAT_112), healthcareServiceUnitID);
                    lvwView.DataSource = lstEntityDate;
                    lvwView.DataBind(); lvwView.DataBind();
                }
                else if (param[0] == "stop")
                {
                    int paramedicID = Convert.ToInt32(param[1]);
                    int hsuID = Convert.ToInt32(param[2]);
                    string departmentID = param[3];
                    int tempDate = Convert.ToInt32(param[4]);
                    int operationalTimeID = Convert.ToInt32(param[5]);
                    OnStopClinicRecord(paramedicID, hsuID, departmentID, tempDate, operationalTimeID);

                    lstEntityDate = BusinessLayer.GetParamedicScheduleClinicStatusList(daynumber, Helper.GetDatePickerValue(txtScheduleDate).ToString(Constant.FormatString.DATE_FORMAT_112), healthcareServiceUnitID);
                    lvwView.DataSource = lstEntityDate;
                    lvwView.DataBind();
                }
                else
                {
                    lstEntityDate = BusinessLayer.GetParamedicScheduleClinicStatusList(daynumber, Helper.GetDatePickerValue(txtScheduleDate).ToString(Constant.FormatString.DATE_FORMAT_112), healthcareServiceUnitID);
                    lvwView.DataSource = lstEntityDate;
                    lvwView.DataBind();
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void OnStartClinicRecord(int paramedicID, int hsuID, string departmentID, int tempDate, int operationalTimeID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ClinicServiceStatusDao entityDao = new ClinicServiceStatusDao(ctx);

            try
            {
                ClinicServiceStatus entity = new ClinicServiceStatus();
                ClinicServiceStatus entityClinic = BusinessLayer.GetClinicServiceStatusList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND StartDate = '{2}'", hsuID, paramedicID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT))).FirstOrDefault();
                if (entityClinic == null)
                {
                    entity.HealthcareServiceunitID = hsuID;
                    //entity.RoomID = 
                    entity.ParamedicID = paramedicID;
                    entity.GCClinicStatus = Constant.ClinicStatus.STARTED;
                    entity.StartDate = DateTime.Now;
                    entity.StartTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    entity.StartBy = AppSession.UserLogin.UserID;
                    entity.GCPausedReason = string.Empty;
                    entity.OtherPausedReason = string.Empty;
                    entityDao.InsertReturnPrimaryKeyID(entity);

                    if (hdnIsBridgingToGateway.Value == "1")
                    {
                        if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                        {
                            string filterExpression = string.Format("GCVisitStatus = '{0}' AND HealthcareServiceUnitID = {1} AND ParamedicID = {2} AND VisitDate = '{3}' ORDER BY VisitID", Constant.VisitStatus.CHECKED_IN, hsuID, paramedicID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
                            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(filterExpression).FirstOrDefault();
                            if (entityCV != null)
                            {
                                SendNotification("1", entityCV.QueueNo.ToString(), entityCV.HealthcareServiceUnitID.ToString(), entityCV.ParamedicID.ToString());
                            }
                            else
                            {
                                SendNotification("1", "0", hsuID.ToString(), paramedicID.ToString());
                            }
                        }
                    }
                }
                else
                {
                    entityClinic.GCClinicStatus = Constant.ClinicStatus.STARTED;
                    entityClinic.StartDate = DateTime.Now;
                    entityClinic.StartTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    entityClinic.StartBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entityClinic);

                    if (hdnIsBridgingToGateway.Value == "1")
                    {
                        if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                        {
                            string filterExpression = string.Format("GCVisitStatus = '{0}' AND HealthcareServiceUnitID = {1} AND ParamedicID = {2} AND VisitDate = '{3}' ORDER BY VisitID DESC", Constant.VisitStatus.RECEIVING_TREATMENT, hsuID, paramedicID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
                            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(filterExpression).FirstOrDefault();
                            if (entityCV != null)
                            {
                                SendNotification("3", entityCV.QueueNo.ToString(), entityCV.HealthcareServiceUnitID.ToString(), entityCV.ParamedicID.ToString());
                            }
                        }
                    }
                }

                ctx.CommitTransaction();
            }
            catch
            {
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
        }

        private void OnStopClinicRecord(int paramedicID, int hsuID, string departmentID, int tempDate, int operationalTimeID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ClinicServiceStatusDao entityDao = new ClinicServiceStatusDao(ctx);

            try
            {
                ClinicServiceStatus entity = BusinessLayer.GetClinicServiceStatusList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1}", hsuID, paramedicID)).FirstOrDefault();
                if (entity != null)
                {
                    entity.GCClinicStatus = Constant.ClinicStatus.STOPPED;
                    entity.StopDate = DateTime.Now;
                    entity.StopTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    entity.StopBy = AppSession.UserLogin.UserID;
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
                                SendNotification("4", entityCV.QueueNo.ToString(), entityCV.HealthcareServiceUnitID.ToString(), entityCV.ParamedicID.ToString());
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

        private void SendNotification(string jenisNotif, string queueNo, string hsuID, string paramedicID)
        {
            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
            {
                GatewayService oService = new GatewayService();
                APIMessageLog entityAPILog = new APIMessageLog();
                string apiResult = oService.OnChangeClinicStatus(jenisNotif, queueNo, hsuID, paramedicID, string.Empty);
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