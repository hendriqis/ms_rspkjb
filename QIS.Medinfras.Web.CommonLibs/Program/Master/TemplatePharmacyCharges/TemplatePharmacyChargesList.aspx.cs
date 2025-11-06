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
    public partial class TemplatePharmacyChargesList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            String MenuID = Request.QueryString["id"];
            switch (MenuID)
            {
                case "PH": return Constant.MenuCode.Pharmacy.TEMPLATE_PHARMACY_CHARGES;
                default: return Constant.MenuCode.Pharmacy.TEMPLATE_PHARMACY_CHARGES;
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
            fieldListText = new string[] { "Charges Template Code", "Charges Template Name" };
            fieldListValue = new string[] { "ChargesTemplateCode", "ChargesTemplateName" };
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvChargesTemplateHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vChargesTemplateHd> lstEntity = BusinessLayer.GetvChargesTemplateHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ChargesTemplateCode ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            String MenuID = Request.QueryString["id"];
            switch (MenuID)
            {
                case "PH": return filterExpression = (string.Format("DepartmentID = '{0}' AND IsDeleted = 0", Constant.Facility.PHARMACY));
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
            url = ResolveUrl(string.Format("~/Libs/Program/Master/TemplatePharmacyCharges/TemplatePharmacyChargesEntry.aspx?id={0}", MenuID));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            String MenuID = Request.QueryString["id"];
            if (hdnTemplateID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Libs/Program/Master/TemplatePharmacyCharges/TemplatePharmacyChargesEntry.aspx?id={0}|{1}", MenuID, hdnTemplateID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ChargesTemplateHdDao entityDao = new ChargesTemplateHdDao(ctx);
            try
            {
                if (hdnTemplateID.Value.ToString() != "")
                {
                    ChargesTemplateHd entity = entityDao.Get(Convert.ToInt32(hdnTemplateID.Value));

                    List<ChargesTemplateDt> lstEntityDt = BusinessLayer.GetChargesTemplateDtList(String.Format("ChargesTemplateID = {0} AND IsDeleted = 0", entity.ChargesTemplateID));
                    if (lstEntityDt.Count <= 0)
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
                        errMessage = "Template " + entity.ChargesTemplateName + " tidak dapat dihapus karena memiliki Detail Item.";
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