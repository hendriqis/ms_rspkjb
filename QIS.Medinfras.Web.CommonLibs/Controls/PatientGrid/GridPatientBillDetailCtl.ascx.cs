using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class GridPatientBillDetailCtl : System.Web.UI.UserControl
    {
        protected int PageCount = 1;
        public void InitializeControl()
        {
            BindGridView(1, true, ref PageCount);
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ((BasePageCheckRegisteredPatient)Page).LoadAllWords();
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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ((BasePageCheckRegisteredPatient)Page).GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvRegistrationRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            List<vRegistration> lstEntity = BusinessLayer.GetvRegistrationList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, "RegistrationID DESC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vRegistration entity = e.Item.DataItem as vRegistration;
                Healthcare entityH = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);

                HtmlGenericControl divBusinessPartnerName = e.Item.FindControl("divBusinessPartnerName") as HtmlGenericControl;

                if (entity.CoverageTypeID != null && entity.CoverageTypeID != 0)
                {
                    if (entity.CoverageTypeCode == "R001" && entityH.Initial == "NHS")
                    {
                        divBusinessPartnerName.InnerHtml = string.Format("{0} ({1})", entity.BusinessPartnerName, entity.CoverageTypeName);
                    }
                    else
                    {
                        divBusinessPartnerName.InnerHtml = string.Format("{0}", entity.BusinessPartnerName);
                    }
                }
                else
                {
                    divBusinessPartnerName.InnerHtml = string.Format("{0}", entity.BusinessPartnerName);
                }

            }
        }

        protected string GetLabel(string code)
        {
            return ((BasePageCheckRegisteredPatient)Page).GetLabel(code);
        }
    }
}