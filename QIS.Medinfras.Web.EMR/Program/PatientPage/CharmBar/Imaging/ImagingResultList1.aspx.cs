using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Text;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class ImagingResultList1 : BasePage
    {
        protected int PageCount = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hdnHealthcareServiceUnitID.Value = BusinessLayer.GetHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1}", AppSession.UserLogin.HealthcareID, BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue))[0].HealthcareServiceUnitID.ToString();
                
                SettingParameterDt setvar = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_RIS_WEB_VIEW_URL);

                if (setvar != null && !String.IsNullOrEmpty(setvar.ParameterValue))
                {
                    hdnViewerUrl.Value = setvar.ParameterValue;
                    //if (AppSession.RIS_HL7_MESSAGE_FORMAT == Constant.RIS_HL7MessageFormat.FUJI_FILM)
                    //{
                    //    hdnViewerUrl.Value += "&winpass=true&filter=accessionnumber=";
                    //}
                }
                else
                {
                    hdnViewerUrl.Value = AppSession.RIS_WEB_VIEW_URL;
                    //if (AppSession.RIS_HL7_MESSAGE_FORMAT == Constant.RIS_HL7MessageFormat.FUJI_FILM)
                    //{
                    //    hdnViewerUrl.Value += "&winpass=true&filter=accessionnumber=";
                    //}
                }

                hdnDocumentPath.Value = string.Format(@"{0}/{1}/", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISPatientDocumentsPath.Replace("#MRN", AppSession.RegisteredPatient.MedicalNo));

                BindGridView(1, true, ref PageCount);
                BindGridViewDt(1, true, ref PageCount);

                BindGridView(1, true, ref PageCount);
            }
        }

        #region Header
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("MRN = {0} AND HealthcareServiceUnitID = {1} AND GCTransactionStatus != '{2}'", AppSession.RegisteredPatient.MRN, hdnHealthcareServiceUnitID.Value, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientChargesHd> lstEntity = BusinessLayer.GetvPatientChargesHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TransactionID DESC");
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
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Detail
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnID.Value != "")
            {
                filterExpression = string.Format("TransactionID = {0} AND IsDeleted = 0", hdnID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPatientChargesDtImagingResultRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vPatientChargesDtImagingResult> lstEntity = BusinessLayer.GetvPatientChargesDtImagingResultList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind();
        }
        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        public string GetImagingResultImage()
        {
            string result = "";
            result = GeneratePreviewUrl(hdnReferenceNo.Value);
            return result;
        }

        private string GeneratePreviewUrl(string referenceNo)
        {
            string result = "";
            string postData = referenceNo;

            string url = string.Format("{0}?{1}", AppSession.RIS_WEB_VIEW_URL, postData);
            if (AppSession.RIS_HL7_MESSAGE_FORMAT == Constant.RIS_HL7MessageFormat.MEDAVIS)
            {
                url = string.Format("{0}{1}", AppSession.RIS_WEB_VIEW_URL, referenceNo);
            }
            else
            {
                #region Post Parameter Data
                TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                string consID = referenceNo;
                string pass = AppSession.RIS_Consumer_Pwd;
                string data = unixTimestamp.ToString() + consID;

                postData = string.Format("X-cons-id={0}&X-timestamp={1}&X-signature={2}", consID, unixTimestamp.ToString(), HttpUtility.UrlEncode(Methods.GenerateSignature(data, pass)));

                UTF8Encoding encoding = new UTF8Encoding();
                byte[] postDataBytes = encoding.GetBytes(postData);
                #endregion

                url = string.Format("{0}?{1}", AppSession.RIS_WEB_VIEW_URL, postData);
            }

            return result = string.Format("{0}|{1}", "1", url);
        }
    }
}