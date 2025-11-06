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
using System.IO;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class PatientDocumentList : BasePagePatientPageList
    {
        protected int PageCount = 1;
        protected List<vPatientDocument> lstPatientDocument = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PATIENT_DOCUMENT;
        }

        protected override void InitializeDataControl()
        {
            hdnPatientDocumentUrl.Value = string.Format(@"{0}/{1}/", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISPatientDocumentsPath.Replace("#MRN", AppSession.RegisteredPatient.MedicalNo));

            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                hdnRegistrationID.Value = param[0];
                BindGridView(1, true, ref PageCount);
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
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Program/PatientPage/Objective/Document/NewPatientDocumentCtl.ascx");
            queryString = "";
            popupWidth = 700;
            popupHeight = 500;
            popupHeaderText = "Patient e-Document";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Program/PatientPage/Objective/Document/NewPatientDocumentCtl.ascx");
                queryString = hdnID.Value;
                popupWidth = 700;
                popupHeight = 500;
                popupHeaderText = "Patient e-Document";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                PatientDocument entity = BusinessLayer.GetPatientDocument(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientDocument(entity);
                return true;
            }
            return false;
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
                path += string.Format("{0}\\{1}", AppConfigManager.QISPatientDocumentsPath.Replace('/', '\\'),param[1]);
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
    }
}