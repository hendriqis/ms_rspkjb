using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TemplatePanelList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            String MenuID = Request.QueryString["id"];
            switch (MenuID)
            {
                //case "SERVICES": return Constant.MenuCode.Finance.TEMPLATE_PANEL_SERVICES;
                case "LABORATORY": return Constant.MenuCode.Laboratory.TEMPLATE_PANEL_LABORATORY;
                case "IMAGING": return Constant.MenuCode.Imaging.TEMPLATE_PANEL_IMAGING;
                case "DIAGNOSTIC": return Constant.MenuCode.MedicalDiagnostic.TEMPLATE_PANEL_DIAGNOSTIC;
                //case "MCU": return Constant.MenuCode.MedicalCheckup.TEMPLATE_PANEL_MCU;
                default: return Constant.MenuCode.Finance.TEMPLATE_PANEL_SERVICES;
            }
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnTemplateID.Value = keyValue;
            BindGridView(1, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Test Template Code", "Test Template Name" };
            fieldListValue = new string[] { "TestTemplateCode", "TestTemplateName" };
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetTestTemplateHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<TestTemplateHd> lstEntity = BusinessLayer.GetTestTemplateHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestTemplateCode ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            String MenuID = Request.QueryString["id"];
            switch (MenuID)
            {
                case "LABORATORY": return filterExpression = (string.Format("GCItemType IN ('{0}') AND IsDeleted = 0", Constant.ItemType.LABORATORIUM));
                case "IMAGING": return filterExpression = (string.Format("GCItemType IN ('{0}') AND IsDeleted = 0", Constant.ItemType.RADIOLOGI));
                case "DIAGNOSTIC": return filterExpression = (string.Format("GCItemType IN ('{0}') AND IsDeleted = 0", Constant.ItemType.PENUNJANG_MEDIS));
                //case "MCU": return filterExpression = (string.Format("GCItemType = '{0}' AND IsDeleted = 0", Constant.ItemType.MEDICAL_CHECKUP));
                default: return filterExpression = (string.Format("IsDeleted = 0"));
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            int pageCount = 1;
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
           
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

                result += "|" + pageCount;
                ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
                panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            String MenuID = Request.QueryString["id"];
            url = ResolveUrl(string.Format("~/Libs/Program/Master/TemplatePanel/TemplatePanelEntry.aspx?id={0}", MenuID));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            String MenuID = Request.QueryString["id"];
            if (hdnTemplateID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Libs/Program/Master/TemplatePanel/TemplatePanelEntry.aspx?id={0}|{1}", MenuID, hdnTemplateID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestTemplateHdDao entityDao = new TestTemplateHdDao(ctx);
            try
            {
                if (hdnTemplateID.Value.ToString() != "")
                {
                    TestTemplateHd entity = entityDao.Get(Convert.ToInt32(hdnTemplateID.Value));

                    List<TestTemplateDt> lstEntityDt = BusinessLayer.GetTestTemplateDtList(String.Format("TestTemplateID = {0}", entity.TestTemplateID));
                    if (lstEntityDt.Count == 0)
                    {
                        entity.IsDeleted = true;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);
                        ctx.CommitTransaction();
                        result = true;
                    }
                    else
                    {
                        result = false;
                        errMessage = "Template " + entity.TestTemplateName + " tidak dapat dihapus karena memiliki Detail Item.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Tidak ada Template yang dapat dihapus.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}