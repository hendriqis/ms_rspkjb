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
    public partial class PDFDocumentList : BasePage
    {
        protected int PageCount = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hdnPatientDocumentUrl.Value = string.Format(@"{0}/{1}/",AppConfigManager.QISVirtualDirectory,AppConfigManager.QISPatientDocumentsPath.Replace("#MRN",AppSession.RegisteredPatient.MedicalNo));
                hdnPatientDocumentUrl1.Value = string.Format(@"{0}/{1}/", AppConfigManager.QISVirtualDirectory, "Patient/#MRN/".Replace("#MRN", AppSession.RegisteredPatient.MedicalNo));
                BindGridView(1, true, ref PageCount);
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, Constant.FileType.IMAGE);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDocumentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientDocument> lstEntity = BusinessLayer.GetvPatientDocumentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "DocumentDate DESC");
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

        //protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        vPatientDocument item = e.Row.DataItem as vPatientDocument;
        //        var hyperLink = e.Row.FindControl("lnkActionLog") as HyperLink;
        //        if (hyperLink != null)
        //        {
        //            hyperLink.NavigateUrl = string.Format(@"{0}/{1}/{2}",AppConfigManager.QISVirtualDirectory,AppConfigManager.QISPatientDocumentsPath.Replace("#MRN",AppSession.RegisteredPatient.MedicalNo),item.FileName);
        //            hyperLink.Target = "popupWindow";
        //        }
        //    }
        //}
    }
}