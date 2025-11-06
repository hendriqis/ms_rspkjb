using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.Service;
using System.Data;


namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PSleaveScheduleEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            string[] par = param.Split('|');
            hdnParamedicID.Value = par[0];
            hdnHealthcareServiceUnitID.Value = par[1];

            vServiceUnitParamedic entity = BusinessLayer.GetvServiceUnitParamedicList(string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1}", hdnParamedicID.Value, hdnHealthcareServiceUnitID.Value))[0];
            hdnHealthcareID.Value = entity.HealthcareID;
            txtParamedicName.Text = entity.ParamedicName;
            txtHealthcareName.Text = entity.HealthcareName;
            txtServiceUnit.Text = entity.ServiceUnitName;
            hdnServiceUnitID.Value = entity.ServiceUnitID.ToString();
            hdnDatePickerToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            List<StandardCode> lstVoidReason = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.PARAMEDIC_LEAVE_REASON));
            Methods.SetComboBoxField<StandardCode>(cboLeaveReason, lstVoidReason, "StandardCodeName", "StandardCodeID");
            cboLeaveReason.SelectedIndex = 0;

            BindGridView(1, true, ref PageCount);

            Helper.SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtEndDate, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtEndTime, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboLeaveReason, new ControlEntrySetting(true, true, true), "mpEntryPopup");

            hdnIsBridgingToGateway.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).ParameterValue;
            hdnProviderGatewayService.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).ParameterValue;
            hdnIsBridgingToMedinfrasMobileApps.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).ParameterValue;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            String filterExpression = string.Format("ParamedicID = {0} AND IsDeleted = 0", hdnParamedicID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvParamedicLeaveScheduleRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vParamedicLeaveSchedule> lstEntity = BusinessLayer.GetvParamedicLeaveScheduleList(filterExpression, 8, pageIndex, "StartDate ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            int pageCount = 1;

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }
            else
            {
                if (param[0] == "save")
                {
                    if (hdnID.Value != "")
                    {
                        if (OnSaveEditRecord(ref errMessage))
                        {
                            result += "success";
                            BindGridView(1, true, ref pageCount);
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                    else
                    {
                        if (OnSaveAddRecord(ref errMessage))
                        {
                            result += "success";
                            BindGridView(1, true, ref pageCount);
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                }
                else if (param[0] == "delete")
                {
                    if (OnDeleteRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Save Entity
        private void ControlToEntity(ParamedicLeaveSchedule entity)
        {
            entity.HealthcareID = Convert.ToString(hdnHealthcareID.Value);
            entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            entity.StartDate = Helper.GetDatePickerValue(txtStartDate.Text);
            entity.StartTime = txtStartTime.Text;
            entity.EndDate = Helper.GetDatePickerValue(txtEndDate.Text);
            entity.EndTime = txtEndTime.Text;
            entity.IsFullDay = chkIsFullDay.Checked;
            entity.GCParamedicLeaveReason = cboLeaveReason.Value.ToString();
            entity.LeaveOtherReason = txtOtherLeaveReason.Text;
            entity.Remarks = txtRemarks.Text;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            int id = 0;
            IDbContext ctx = DbFactory.Configure(true);
            ParamedicLeaveScheduleDao entityDao = new ParamedicLeaveScheduleDao(ctx);
            try
            {
                ParamedicLeaveSchedule entity = new ParamedicLeaveSchedule();
                ControlToEntity(entity);

                String awal = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) + " " + entity.StartTime;
                String akhir = entity.EndDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) + " " + entity.EndTime;

                DateTime date1 = DateTime.ParseExact(awal, "dd-MM-yyyy HH:mm",
                                           System.Globalization.CultureInfo.InvariantCulture);
                DateTime date2 = DateTime.ParseExact(akhir, "dd-MM-yyyy HH:mm",
                                           System.Globalization.CultureInfo.InvariantCulture);
                TimeSpan durationTemp = date2 - date1;
                double totalMin = durationTemp.TotalMinutes;

                string filterexp = String.Format("ParamedicID = {0} AND IsDeleted = 0 AND ((StartDate BETWEEN '{1}' AND '{2}') OR (EndDate BETWEEN '{1}' AND '{2}'))", entity.ParamedicID, entity.StartDate, entity.EndDate);
                string filterApoitment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{3}' AND '{4}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{3}' AND '{4}'))", entity.ParamedicID, Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, entity.StartDate.ToString("yyyy-MM-dd") + " " + entity.StartTime + ":00.000", entity.EndDate.ToString("yyyy-MM-dd") + " " + entity.EndTime + ":00.000");
                List<ParamedicLeaveSchedule> lstEntity = BusinessLayer.GetParamedicLeaveScheduleList(filterexp, ctx);
                List<GetParamedicLeaveScheduleDate> lstE = BusinessLayer.GetParamedicLeaveScheduleDateList(entity.ParamedicID, ctx);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterApoitment, ctx);

                if (lstEntity.Count() <= 0)
                {
                    if (lstE.Where(t => t.Date == entity.StartDate || t.Date == entity.EndDate).Count() <= 0)
                    {
                        if (totalMin > 0)
                        {
                            if (lstAppointment.Count <= 0)
                            {
                                entity.IsDeleted = false;
                                entity.CreatedBy = AppSession.UserLogin.UserID;
                                entity.ID = entityDao.InsertReturnPrimaryKeyID(entity);
                                ctx.CommitTransaction();

                                vParamedicLeaveSchedule1 entityParamedic = BusinessLayer.GetvParamedicLeaveSchedule1List(filterexp).FirstOrDefault();

                                //#region Is Brigding To Gateway
                                if (hdnIsBridgingToGateway.Value == "1")
                                {
                                    if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD) // RUMAH SAKIT MEDISTRA
                                    {
                                        GatewayService oService = new GatewayService();
                                        APIMessageLog entityAPILog = new APIMessageLog();
                                        string apiResult = oService.OnLeavePhysicianScheduleChanged("CREATE", entityParamedic);
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
                                    else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)// RUMAH SAKIT DR OEN KANDANG SAPI SURAKARTA 
                                    {
                                        GatewayService oService = new GatewayService();
                                        APIMessageLog entityAPILog = new APIMessageLog();
                                        string apiResult = oService.OnLeavePhysicianSchedule("3", hdnServiceUnitID.Value, entityParamedic);
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
                                            entityAPILog.MessageText = apiResultInfo[2];
                                            entityAPILog.Response = apiResultInfo[1];
                                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                        }
                                    }
                                    //else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA)// RUMAH SAKIT DR OEN SOLO BARU 
                                    //{
                                    //    GatewayService oService = new GatewayService();
                                    //    APIMessageLog entityAPILog = new APIMessageLog();
                                    //    string apiResult = oService.OnPhysicianLeaveScheduleChanged("001", id);
                                    //    string[] apiResultInfo = apiResult.Split('|');
                                    //    if (apiResultInfo[0] == "0")
                                    //    {
                                    //        entityAPILog.IsSuccess = false;
                                    //        entityAPILog.MessageText = apiResultInfo[1];
                                    //        entityAPILog.Response = apiResultInfo[1];
                                    //        Exception ex = new Exception(apiResultInfo[1]);
                                    //        Helper.InsertErrorLog(ex);
                                    //    }
                                    //    else
                                    //    {
                                    //        entityAPILog.MessageText = apiResultInfo[2];
                                    //        entityAPILog.Response = apiResultInfo[1];
                                    //        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                    //    }
                                    //}
                                }

                                if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
                                {

                                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                                    APIMessageLog entityAPILog = new APIMessageLog();
                                    string apiResult = oService.OnParamedicLeaveScheduleChanged(entity, Convert.ToInt32(hdnParamedicID.Value), txtStartDate.Text, txtEndDate.Text, txtStartTime.Text, txtEndTime.Text, "001");
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
                                //#endregion

                            }
                            else
                            {
                                errMessage = "Maaf, di periode tersebut sudah ada appointment. Cuti tidak dapat di proses";
                                result = false;
                                ctx.RollBackTransaction();
                            }
                        }
                        else
                        {
                            errMessage = "Maaf, waktu selesai melewati / sama dengan waktu mulai";
                            result = false;
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        errMessage = "Maaf, jadwal cuti di periode itu sudah ada";
                        result = false;
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    errMessage = "Maaf, jadwal cuti di periode itu sudah ada";
                    result = false;
                    ctx.RollBackTransaction();
                }
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
        #endregion

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ParamedicLeaveScheduleDao entityDao = new ParamedicLeaveScheduleDao(ctx);
            try
            {
                ParamedicLeaveSchedule entity = BusinessLayer.GetParamedicLeaveSchedule(Convert.ToInt32(hdnID.Value));
                if (!entity.IsDeleted)
                {
                    ControlToEntity(entity);

                    String awal = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) + " " + entity.StartTime;
                    String akhir = entity.EndDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) + " " + entity.EndTime;

                    DateTime date1 = DateTime.ParseExact(awal, "dd-MM-yyyy HH:mm",
                                               System.Globalization.CultureInfo.InvariantCulture);
                    DateTime date2 = DateTime.ParseExact(akhir, "dd-MM-yyyy HH:mm",
                                               System.Globalization.CultureInfo.InvariantCulture);
                    TimeSpan durationTemp = date2 - date1;
                    double totalMin = durationTemp.TotalMinutes;

                    string filterexp = String.Format("ID != {0} AND ParamedicID = {1} AND IsDeleted = 0 AND ((StartDate BETWEEN '{2}' AND '{3}') OR (EndDate BETWEEN '{2}' AND '{3}'))", entity.ID, entity.ParamedicID, entity.StartDate, entity.EndDate);
                    string filterApoitment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime ) BETWEEN '{3}' AND '{4}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{3}' AND '{4}'))", entity.ParamedicID, Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, entity.StartDate.ToString("yyyy-MM-dd") + " " + entity.StartTime + ":00.000", entity.EndDate.ToString("yyyy-MM-dd") + " " + entity.EndTime + ":00.000");
                    List<ParamedicLeaveSchedule> lstEntity = BusinessLayer.GetParamedicLeaveScheduleList(filterexp, ctx);
                    List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterApoitment, ctx);

                    if (lstEntity.Count() <= 0)
                    {
                        if (totalMin > 0)
                        {
                            if (lstAppointment.Count <= 0)
                            {
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDao.Update(entity);
                                ctx.CommitTransaction();

                                string filterExpEdit = String.Format("ID = {0}", entity.ID);
                                vParamedicLeaveSchedule1 entityParamedic = BusinessLayer.GetvParamedicLeaveSchedule1List(filterExpEdit).FirstOrDefault();

                                //#region Is Brigding To Gateway
                                if (hdnIsBridgingToGateway.Value == "1")
                                {
                                    if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD) // RUMAH SAKIT MEDISTRA
                                    {
                                        GatewayService oService = new GatewayService();
                                        APIMessageLog entityAPILog = new APIMessageLog();
                                        string apiResult = oService.OnLeavePhysicianScheduleChanged("UPDATE", entityParamedic);
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
                                    else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)// RUMAH SAKIT DR OEN KANDANG SAPI SURAKARTA 
                                    {
                                        GatewayService oService = new GatewayService();
                                        APIMessageLog entityAPILog = new APIMessageLog();
                                        string apiResult = oService.OnLeavePhysicianSchedule("3",hdnServiceUnitID.Value, entityParamedic);
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
                                            entityAPILog.MessageText = apiResultInfo[2];
                                            entityAPILog.Response = apiResultInfo[1];
                                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                        }
                                    }
                                }
                                if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
                                {

                                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                                    APIMessageLog entityAPILog = new APIMessageLog();
                                    string apiResult = oService.OnParamedicLeaveScheduleChanged(entity, Convert.ToInt32(hdnParamedicID.Value), txtStartDate.Text, txtEndDate.Text, txtStartTime.Text, txtEndTime.Text, "002");
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
                                //#endregion
                            }
                            else
                            {
                                errMessage = "Maaf, di periode tersebut sudah ada appointment. Cuti tidak dapat di proses";
                                result = false;
                                ctx.RollBackTransaction();
                            }
                        }
                        else
                        {
                            errMessage = "Maaf, waktu selesai melewati / sama dengan waktu mulai";
                            result = false;
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        errMessage = "Maaf, jadwal cuti di periode itu sudah ada";
                        result = false;
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    errMessage = "Maaf, jadwal tidak bisa diubah. Harap refresh halaman ini.";
                    result = false;
                    ctx.RollBackTransaction();
                }
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

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ParamedicLeaveScheduleDao entityDao = new ParamedicLeaveScheduleDao(ctx);

            try
            {
                ParamedicLeaveSchedule entity = BusinessLayer.GetParamedicLeaveSchedule(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
                ctx.CommitTransaction();

                string filterExpEdit = String.Format("ID = {0}", entity.ID);
                vParamedicLeaveSchedule1 entityParamedic = BusinessLayer.GetvParamedicLeaveSchedule1List(filterExpEdit).FirstOrDefault();

                //#region Is Brigding To Gateway
                if (hdnIsBridgingToGateway.Value == "1")
                {
                    if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD) // RUMAH SAKIT MEDISTRA
                    {
                        GatewayService oService = new GatewayService();
                        APIMessageLog entityAPILog = new APIMessageLog();
                        string apiResult = oService.OnLeavePhysicianScheduleChanged("DELETE", entityParamedic);
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
                    else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)// RUMAH SAKIT DR OEN KANDANG SAPI SURAKARTA 
                    {
                        GatewayService oService = new GatewayService();
                        APIMessageLog entityAPILog = new APIMessageLog();
                        string apiResult = oService.OnLeavePhysicianSchedule("3", hdnServiceUnitID.Value, entityParamedic);
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
                            entityAPILog.MessageText = apiResultInfo[2];
                            entityAPILog.Response = apiResultInfo[1];
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        }
                    }
                }

                if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
                {

                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                    APIMessageLog entityAPILog = new APIMessageLog();
                    string apiResult = oService.OnParamedicLeaveScheduleChanged(entity, Convert.ToInt32(hdnParamedicID.Value), txtStartDate.Text, txtEndDate.Text, txtStartTime.Text, txtEndTime.Text, "003");
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
                //#endregion
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