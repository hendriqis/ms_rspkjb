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
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class ImagingTestResultList1 : BasePagePatientPageList
    {
        protected int PageCount = 1;
        protected string viewerUrl = string.Empty;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.IMAGING_TEST_RESULT_1;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        public override bool IsEntryUsePopup()
        {
            return false;
        }

        protected override void InitializeDataControl()
        {
            hdnHealthcareServiceUnitID.Value = BusinessLayer.GetHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1}", AppSession.UserLogin.HealthcareID, BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue))[0].HealthcareServiceUnitID.ToString();
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}', '{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_RIS_WEB_VIEW_URL, Constant.SettingParameter.IS_RIS_USING_RESULT_IN_PDF));
            hdnRISVendor.Value = AppSession.RIS_HL7_MESSAGE_FORMAT;
            viewerUrl = setvar.Where(w => w.ParameterCode == Constant.SettingParameter.IS_RIS_WEB_VIEW_URL).FirstOrDefault().ParameterValue;
            hdnIsRISUsingPDFResult.Value = setvar.Where(w => w.ParameterCode == Constant.SettingParameter.IS_RIS_USING_RESULT_IN_PDF).FirstOrDefault().ParameterValue;
            if (setvar != null && !string.IsNullOrEmpty(viewerUrl))
            {
                hdnViewerUrl.Value = viewerUrl;
            }
            else
            {
                hdnViewerUrl.Value = AppSession.RIS_WEB_VIEW_URL;
            }

            hdnDocumentPath.Value = string.Format(@"{0}/{1}/", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISPatientDocumentsPath.Replace("#MRN", AppSession.RegisteredPatient.MedicalNo));

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
                if (entityLinkedRegistration != null)
                {
                    hdnLinkedVisitID.Value = entityLinkedRegistration.VisitID.ToString();
                }
            }

            BindGridView(1, true, ref PageCount);
            BindGridViewDt(1, true, ref PageCount);
        }

        #region Header
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ({0},{1}) AND HealthcareServiceUnitID = {2} AND GCTransactionStatus != '{3}'", AppSession.RegisteredPatient.VisitID, hdnLinkedVisitID.Value, hdnHealthcareServiceUnitID.Value, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientChargesHd> lstEntity = BusinessLayer.GetvPatientChargesHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
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
        protected void grdViewDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientChargesDtImagingResult entity = e.Row.DataItem as vPatientChargesDtImagingResult;
                HtmlInputButton btnPDF = (HtmlInputButton)e.Row.FindControl("btnViewPDF");
                if (hdnIsRISUsingPDFResult.Value == "0")
                {
                    btnPDF.Attributes.Add("style", "display:none");
                }
            }
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