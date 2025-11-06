using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class BPJSDocument : BasePageTrx
    {
        protected int PageCount = 1;
        protected List<vPatientDocument> lstPatientDocument = null;

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.BPJS_TEMPORARY_CLAIM_INPATIENT;
        }

        protected override void InitializeDataControl()
        {
            string moduleName = Helper.GetModuleName();
            string moduleID = Helper.GetModuleID(moduleName);
            hdnModuleID.Value = moduleID;
            hdnPatientDocumentUrl.Value = string.Format(@"{0}{1}", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISPatientDocumentsPath.Replace("#MRN", AppSession.RegisteredPatient.MedicalNo));
            hdnIsBridgingToGateway.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).ParameterValue;
            hdnProviderGatewayService.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).ParameterValue;

            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                hdnRegistrationID.Value = param[0];
                hdnIsFromMenuInitial.Value = param[1];

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = (SELECT VisitID FROM vConsultVisit WHERE RegistrationID = {0})", hdnRegistrationID.Value)).FirstOrDefault();
                hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnDepartmentID.Value = entity.DepartmentID;
                EntityToControl(entity);

                BindGridView(1, false, ref PageCount);
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("IsDeleted = 0 AND VisitID = (SELECT cv.VisitID FROM ConsultVisit cv WITH(NOLOCK) WHERE cv.RegistrationID = {0})", hdnRegistrationID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDocumentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientDocument> lstEntity = BusinessLayer.GetvPatientDocumentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            lstPatientDocument = BusinessLayer.GetvPatientDocumentList(string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY ID DESC", AppSession.RegisteredPatient.VisitID));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "delete")
                {
                    OnDeleteDocumentRecord();
                    result = "refresh";
                }
                else if (param[0] == "send")
                {
                    if (hdnPatientDocumentUrl.Value != string.Empty)
                    {
                        string imagePath = string.Format("{0}{1}", hdnPatientDocumentUrl.Value, param[1]);
                        if (hdnIsBridgingToGateway.Value == "1")
                        {
                            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                            {
                                string base64Pdf = ConvertImageToBase64(imagePath);
                                GatewayToService(base64Pdf, param[1]);
                            }
                        }
                        result = "refresh";
                    }
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

        private void EntityToControl(vConsultVisit2 entity)
        {
            ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected void cbpOpenDocument_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            try
            {
                string path = AppConfigManager.QISPhysicalDirectory;
                path += string.Format("{0}\\{1}", AppConfigManager.QISPatientDocumentsPath.Replace('/', '\\'), param[1]);
                path = path.Replace("#MRN", AppSession.RegisteredPatient.MedicalNo);
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    //file.Open(FileMode.Open);
                }

                result += string.Format("success|{0}", errMessage);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void OnDeleteDocumentRecord()
        {
            if (hdnID.Value != "")
            {
                PatientDocument entity = BusinessLayer.GetPatientDocument(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientDocument(entity);
            }
        }

        private string ConvertImageToBase64(string imagePath)
        {
            string base64String = string.Empty;

            WebClient wc = new WebClient();
            byte[] bytes = wc.DownloadData(imagePath);
            MemoryStream ms = new MemoryStream(bytes);
            using (System.Drawing.Image image = System.Drawing.Image.FromStream(ms))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                }
            }
            
            return base64String;
        }

        private void GatewayToService(string base64Pdf, string fileName)
        {
            GatewayService oService = new GatewayService();
            APIMessageLog entityAPILog = new APIMessageLog();
            string apiResult = oService.OnSendStreamPDF(base64Pdf, fileName);
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