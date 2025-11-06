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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class BodyDiagramList : BasePagePatientPageList
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            if (Page.Request.QueryString.Count > 0)
                return Constant.MenuCode.EMR.HEALTH_RECORD_BODY_DIAGRAM;
            return Constant.MenuCode.EMR.BODY_DIAGRAM;
        }

        protected override void InitializeDataControl()
        {
            string filterExpression = "";
            if (Page.Request.QueryString.Count > 0)
                filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);
            else
                filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            int pageCount = BusinessLayer.GetvPatientBodyDiagramHdRowCount(filterExpression);
            hdnPageCount.Value = pageCount.ToString();

            if (pageCount > 0)
            {
                hdnPageIndex.Value = "0";
                OnLoadEntity(0);
            }
            else
            {
                divBodyDiagram.Style.Add("display", "none");
                tblEmpty.Style.Remove("display");
            }
        }

        protected void OnLoadEntity(int PageIndex)
        {
            string filterExpression = "";
            if (Page.Request.QueryString.Count > 0)
                filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);
            else
                filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            vPatientBodyDiagramHd entity = BusinessLayer.GetvPatientBodyDiagramHd(filterExpression, PageIndex, "ID DESC");
            EntityToControl(entity);

            filterExpression = string.Format("ID = {0} AND IsDeleted = 0", entity.ID);
            rptRemarks.DataSource = BusinessLayer.GetvPatientBodyDiagramDtList(filterExpression);
            rptRemarks.DataBind();
        }

        private void EntityToControl(vPatientBodyDiagramHd entity)
        {
            spnParamedicName.InnerHtml = entity.ParamedicName;
            spnObservationDateTime.InnerHtml = entity.DisplayObservationDateTime;
            spnDiagramName.InnerHtml = entity.DiagramName;

            imgBodyDiagram.Src = entity.FileImageUrl;
            hdnBodyDiagramID.Value = entity.ID.ToString();

        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            int pageIndex = Convert.ToInt32(hdnPageIndex.Value);
            int pageCount = Convert.ToInt32(hdnPageCount.Value);
            if (e.Parameter == "refresh")
            {
                string filterExpression = "";
                if (Page.Request.QueryString.Count > 0)
                    filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);
                else
                    filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

                pageCount = BusinessLayer.GetvPatientBodyDiagramHdRowCount(filterExpression);
                result = "count|" + pageCount;
                if (pageCount > 0)
                    OnLoadEntity(0);
            }
            else if (e.Parameter == "edit")
            {
                result = "edit";
                OnLoadEntity(pageIndex);
            }
            else
            {
                if (e.Parameter == "next")
                {
                    pageIndex++;
                    if (pageIndex == pageCount)
                        pageIndex = 0;
                }
                else if (e.Parameter == "prev")
                {
                    pageIndex--;
                    if (pageIndex < 0)
                        pageIndex = pageCount - 1;
                }
                OnLoadEntity(pageIndex);
                result = "index|" + pageIndex;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientAssessment/BodyDiagram/BodyDiagramAddCtl.ascx");
            queryString = "";
            popupWidth = 1200;
            popupHeight = 600;
            popupHeaderText = "Body Diagram";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnBodyDiagramID.Value != "")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientAssessment/BodyDiagram/BodyDiagramEditCtl.ascx");
                queryString = hdnBodyDiagramID.Value;
                popupWidth = 1100;
                popupHeight = 520;
                popupHeaderText = "Body Diagram";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnBodyDiagramID.Value != "")
            {
                PatientBodyDiagramHd entity = BusinessLayer.GetPatientBodyDiagramHd(Convert.ToInt32(hdnBodyDiagramID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientBodyDiagramHd(entity);
                return true;
            }
            return false;
        }
    }
}