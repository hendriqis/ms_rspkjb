using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TemplatePrescriptionList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.MASTER_PHYSICIAN_PRESCRIPTION_TEMPLATE;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvPhysicianTextRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Template Code", "Template Name" };
            fieldListValue = new string[] { "PrescriptionTemplateCode", "PrescriptionTemplateName" };
        }

        private string GetFilterExpression()
        {
            List<vPhysicianText> lstEntity = BusinessLayer.GetvPhysicianTextList(string.Format("IsDeleted = 0"));
            //string filterExpression = string.Format("UserID = {0} AND IsDeleted = 0", AppSession.UserLogin.UserID);
            string filterExpression = string.Format("ParamedicID = {0} AND IsDeleted = 0", AppSession.UserLogin.ParamedicID);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPhysicianTextRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPrescriptionTemplateHD> lstEntity = BusinessLayer.GetvPrescriptionTemplateHDList(string.Format(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex));
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


        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            url = ResolveUrl("~/Libs/Program/Master/PhysicianText/TemplatePrescriptionEntry.aspx");
            return true;
        }
        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnPrescriptionTemplateID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Libs/Program/Master/PhysicianText/TemplatePrescriptionEntry.aspx?id={0}", hdnPrescriptionTemplateID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnPrescriptionTemplateID.Value.ToString() != "")
            {
                PrescriptionTemplateHd entity = BusinessLayer.GetPrescriptionTemplateHd(Convert.ToInt32(hdnPrescriptionTemplateID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePrescriptionTemplateHd(entity);
                return true;
            }
            return false;
        }
    }
}