using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class RequestCancelAppointmentList : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Outpatient.REQUEST_CANCEL_APPOINTMENT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnDepartmentID.Value = Page.Request.QueryString["id"];

                GetSettingParameter();
                InitializeFilterParameter();

                BindGridView(1, true, ref PageCount);

                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            }
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(String.Format("ParameterCode IN ('{0}','{1}','{2}')", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS));
            hdnHealthcareServiceUnitImagingID.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
            hdnHealthcareServiceUnitLaboratoryID.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;
            hdnIsBridgingToMedinfrasMobileApps.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
        }

        private void InitializeFilterParameter()
        {
            txtFromAppointmentRequestDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToAppointmentRequestDate.Text = DateTime.Today.AddDays(6).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected string GetFilterHealthcareServiceUnit()
        {
            string result = "";
            if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
            {
                result = string.Format("DepartmentID = '{0}' AND IsUsingRegistration = 1 AND IsDeleted = 0", hdnDepartmentID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.IMAGING)
            {
                result = string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID = '{1}' AND IsUsingRegistration = 1 AND IsDeleted = 0", Constant.Facility.DIAGNOSTIC, hdnHealthcareServiceUnitImagingID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.LABORATORY)
            {
                result = string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID = '{1}' AND IsUsingRegistration = 1 AND IsDeleted = 0", Constant.Facility.DIAGNOSTIC, hdnHealthcareServiceUnitLaboratoryID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                result = string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN ('{1}','{2}') AND IsUsingRegistration = 1 AND IsDeleted = 0", Constant.Facility.DIAGNOSTIC, hdnHealthcareServiceUnitImagingID.Value, hdnHealthcareServiceUnitLaboratoryID.Value);
            }
            return result;
        }

        protected string DateTimeNowDatePicker()
        {
            return DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string apmFromDateYear = txtFromAppointmentRequestDate.Text.Substring(6, 4);
            string apmFromDateMonth = txtFromAppointmentRequestDate.Text.Substring(3, 2);
            string apmFromDateDay = txtFromAppointmentRequestDate.Text.Substring(0, 2);
            string apmFromDateFormat = string.Format("{0}-{1}-{2}", apmFromDateYear, apmFromDateMonth, apmFromDateDay);
            DateTime apmFromDate = DateTime.ParseExact(apmFromDateFormat, Constant.FormatString.DATE_PICKER_FORMAT2, null);

            string apmToDateYear = txtToAppointmentRequestDate.Text.Substring(6, 4);
            string apmToDateMonth = txtToAppointmentRequestDate.Text.Substring(3, 2);
            string apmToDateDay = txtToAppointmentRequestDate.Text.Substring(0, 2);
            string apmToDateFormat = string.Format("{0}-{1}-{2}", apmToDateYear, apmToDateMonth, apmToDateDay);
            DateTime apmToDate = DateTime.ParseExact(apmToDateFormat, Constant.FormatString.DATE_PICKER_FORMAT2, null);

            //string filterExpression = "AppointmentID IS NULL AND IsDeleted = 0 AND IsRequestDeleted = 1";
            string filterExpression = "IsDeleted = 0 AND IsRequestDeleted = 1 AND RegistrationID IS NULL";

            if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", hdnDepartmentID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.IMAGING)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}' AND HealthcareServiceUnitID = '{1}'", Constant.Facility.DIAGNOSTIC, hdnHealthcareServiceUnitImagingID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.LABORATORY)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}' AND HealthcareServiceUnitID = '{1}'", Constant.Facility.DIAGNOSTIC, hdnHealthcareServiceUnitLaboratoryID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN ('{1}','{2}')", Constant.Facility.DIAGNOSTIC, hdnHealthcareServiceUnitImagingID.Value, hdnHealthcareServiceUnitLaboratoryID.Value);
            }

            if (!String.IsNullOrEmpty(hdnHealthcareServiceUnitID.Value))
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value);
            }

            if (!String.IsNullOrEmpty(txtFromAppointmentRequestDate.Text) && !String.IsNullOrEmpty(txtToAppointmentRequestDate.Text))
            {
                filterExpression += string.Format(" AND AppointmentRequestDate BETWEEN '{0}' AND '{1}'", apmFromDate.ToString(Constant.FormatString.DATE_FORMAT_112), apmToDate.ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvAppointmentRequestRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vAppointmentRequest> lstEntity = BusinessLayer.GetvAppointmentRequestList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AppointmentRequestID DESC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vAppointmentRequest entity = e.Item.DataItem as vAppointmentRequest;
                HtmlInputHidden hdnKey = (HtmlInputHidden)e.Item.FindControl("hdnKey");
                HtmlInputHidden hdnParamedicID = (HtmlInputHidden)e.Item.FindControl("hdnParamedicID");
                HtmlInputHidden hdnHealthcareServiceUnitPerRowID = (HtmlInputHidden)e.Item.FindControl("hdnHealthcareServiceUnitPerRowID");
                HtmlInputHidden hdnGCCustomerType = (HtmlInputHidden)e.Item.FindControl("hdnGCCustomerType");
                HtmlGenericControl lblParamedicName = (HtmlGenericControl)e.Item.FindControl("lblParamedicName");
                HtmlGenericControl lblCustomerType = (HtmlGenericControl)e.Item.FindControl("lblCustomerType");
                TextBox txtAppointmentDate = e.Item.FindControl("txtAppointmentDate") as TextBox;
                txtAppointmentDate.Text = entity.AppointmentRequestDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                hdnKey.Value = entity.AppointmentRequestID.ToString();
                if (!string.IsNullOrEmpty(entity.ParamedicName))
                {
                    lblParamedicName.InnerText = entity.ParamedicName;
                }
                else
                {
                    lblParamedicName.InnerText = "Pilih Dokter";
                }
                hdnParamedicID.Value = entity.ParamedicID.ToString();
                hdnHealthcareServiceUnitPerRowID.Value = entity.HealthcareServiceUnitID.ToString();
                lblCustomerType.InnerText = entity.CustomerType;
                if (!string.IsNullOrEmpty(entity.GCCustomerType))
                {
                    lblCustomerType.InnerText = entity.CustomerType;
                }
                else
                {
                    lblCustomerType.InnerText = "Ubah Penjamin";
                }
                hdnGCCustomerType.Value = entity.GCCustomerType;
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "confirm")
                {
                    int appointmentRegID = 0;
                    if (OnConfirmCancelAppointmentRequest(ref errMessage))
                    {
                        BridgingToMedinfrasMobileApps("save", txtFromAppointmentRequestDate.Text, txtToAppointmentRequestDate.Text, Convert.ToInt32(hdnAppointmentRequestID.Value));
                        result = "confirm|" + pageCount + "|" + appointmentRegID;
                    }
                    else
                    {
                        result += "fail|" + errMessage;
                    }
                }
                else if (param[0] == "import")
                {
                    if (!String.IsNullOrEmpty(txtFromAppointmentRequestDate.Text) && !String.IsNullOrEmpty(txtToAppointmentRequestDate.Text))
                    {
                        BridgingToMedinfrasMobileApps("import", txtFromAppointmentRequestDate.Text, txtToAppointmentRequestDate.Text, 0);
                    }
                    BindGridView(1, true, ref pageCount);
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnConfirmCancelAppointmentRequest(ref string errMessage)
        {
            bool result = true;

            AppointmentRequest entity = BusinessLayer.GetAppointmentRequest(Convert.ToInt32(hdnAppointmentRequestID.Value));
            if (entity != null)
            {
                if (entity.RegistrationID == null || entity.RegistrationID == 0)
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    AppointmentDao entityApmDao = new AppointmentDao(ctx);
                    AppointmentRequestDao entityDao = new AppointmentRequestDao(ctx);
                    try
                    {
                        entity.IsDeleted = true;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entity.LastUpdatedDate = DateTime.Now;
                        entityDao.Update(entity);

                        if (entity.AppointmentID > 0 && entity.AppointmentID != null)
                        {
                            Appointment entityApm = BusinessLayer.GetAppointmentList(string.Format("AppointmentID = {0}", entity.AppointmentID), ctx).FirstOrDefault();
                            if (entityApm != null)
                            {
                                entityApm.GCAppointmentStatus = Constant.AppointmentStatus.DELETED;
                                entityApm.GCDeleteReason = Constant.AppointmentDeleteReason.OTHER;
                                entityApm.DeleteReason = string.Format("Pembatalan dari mobile apps : {0})", entity.RequestDeletedReason);
                                entityApm.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityApm.LastUpdatedDate = DateTime.Now;
                                entityApmDao.Update(entityApm);

                            }
                        }

                        ctx.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        errMessage = ex.Message;
                        ctx.RollBackTransaction();
                    }
                    finally
                    {
                        ctx.Close();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Tidak bisa melanjutkan proses, data perjanjian sudah memiliki registrasi aktif";
                }
            }
            else
            {
                result = false;
                errMessage = "Data permintaan perjanjian tidak ditemukan";
            }

            return result;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            return result;
        }

        private void BridgingToMedinfrasMobileApps(string eventType, string fromAppointmentDate, string toAppointmentDate, int appointmentRequestID)
        {
            if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
            {
                MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                APIMessageLog entityAPILog = new APIMessageLog();
                entityAPILog.IsSuccess = true;
                entityAPILog.MessageDateTime = DateTime.Now;
                entityAPILog.Sender = "MEDINFRAS";
                entityAPILog.Recipient = "MOBILE APPS";
                if (eventType == "import")
                {
                    string apiResult = oService.OnGetCancelAppointmentRequest(fromAppointmentDate, toAppointmentDate);
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.ErrorMessage = apiResultInfo[2];
                        //entityAPILog.Response = apiResultInfo[1];
                        Exception ex = new Exception(apiResultInfo[2]);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                    {
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.Response = apiResultInfo[2];
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                }
                else if (eventType == "save")
                {
                    string apiResult = oService.OnRejectAppointmentRequest(appointmentRequestID);
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.MessageText = apiResultInfo[1];
                        //entityAPILog.Response = apiResultInfo[1];
                        Exception ex = new Exception(apiResultInfo[2]);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                    {
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.Response = apiResultInfo[2];
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                }
            }
        }
    }
}