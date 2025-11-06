using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class BudgetMasterPerMonthTemplateCtl : BaseEntryPopupCtl
    {
        private BudgetingMasterPerMonthEntry DetailPage
        {
            get { return (BudgetingMasterPerMonthEntry)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            BindGridView();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (grdView.Rows.Count < 1)
                BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnBudgetTemplateHdID.Value != "")
            {
                filterExpression = string.Format("BudgetTemplateID = {0} AND IsDeletedDt = 0", hdnBudgetTemplateHdID.Value);
            }

            List<vBudgetTemplateDtInfo> lstEntity = BusinessLayer.GetvBudgetTemplateDtInfoList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vBudgetTemplateDtInfo entity = e.Row.DataItem as vBudgetTemplateDtInfo;
                TextBox txtBudgetAmount = e.Row.FindControl("txtBudgetAmount") as TextBox;

                txtBudgetAmount.Text = entity.BudgetAmount.ToString();
            }
        }

        private void ControlToEntity(IDbContext ctx, int BudgetTransactionID)
        {
            BudgetTransactionDtDao entityDtDao = new BudgetTransactionDtDao(ctx);
            BudgetTransactionDtRevenueCostCenterDao entityDtRCCDao = new BudgetTransactionDtRevenueCostCenterDao(ctx);

            int BudgetTemplateDtID = 0;
            
            int dtID = 0;

            foreach (GridViewRow row in grdView.Rows)
            {
                CheckBox chkIsSelected = row.FindControl("chkIsSelected") as CheckBox;

                if (chkIsSelected.Checked)
                {
                    HtmlInputHidden hdnBudgetTemplateDtRCCID = (HtmlInputHidden)row.FindControl("keyField");
                    HtmlInputHidden hdnBudgetTemplateDtID = (HtmlInputHidden)row.FindControl("hdnBudgetTemplateDtID");
                    HtmlInputHidden hdnGLAccountID = (HtmlInputHidden)row.FindControl("hdnGLAccountID");
                    HtmlInputHidden hdnDepartmentID = (HtmlInputHidden)row.FindControl("hdnDepartmentID");
                    HtmlInputHidden hdnRevenueCostCenterID = (HtmlInputHidden)row.FindControl("hdnRevenueCostCenterID");
                    TextBox txtRemaksRCC = (TextBox)row.FindControl("txtRemaksRCC");
                    TextBox txtBudgetAmount = (TextBox)row.FindControl("txtBudgetAmount");

                    BudgetTransactionDtRevenueCostCenter en = entityDtRCCDao.Get(Convert.ToInt32(hdnBudgetTemplateDtRCCID.Value));

                    BudgetTransactionDt entityDt = new BudgetTransactionDt();
                    if (BudgetTemplateDtID != Convert.ToInt32(hdnBudgetTemplateDtID.Value))
                    {
                        entityDt.GLAccountID = Convert.ToInt32(hdnGLAccountID.Value);
                        entityDt.BudgetTransactionID = BudgetTransactionID;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;

                        dtID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);

                        String filterBudgetTemplateRCC = string.Format("BudgetTemplateDtID = {0} AND IsDeleted = 0", hdnBudgetTemplateDtID.Value);
                        List<BudgetTemplateDtRevenueCostCenter> lstentityBudgetTemplateRCC = BusinessLayer.GetBudgetTemplateDtRevenueCostCenterList(filterBudgetTemplateRCC, ctx);

                        foreach (BudgetTemplateDtRevenueCostCenter e in lstentityBudgetTemplateRCC)
                        {
                            if (e.ID.ToString() == hdnBudgetTemplateDtRCCID.Value)
                            {
                                BudgetTransactionDtRevenueCostCenter entityDtRCC = new BudgetTransactionDtRevenueCostCenter();
                                entityDtRCC.BudgetTransactionDtID = dtID;
                                entityDtRCC.DepartmentID = hdnDepartmentID.Value;
                                entityDtRCC.RevenueCostCenterID = Convert.ToInt32(hdnRevenueCostCenterID.Value);
                                entityDtRCC.BudgetAmount = Convert.ToDecimal(Request.Form[txtBudgetAmount.UniqueID]);
                                entityDtRCC.Remarks = Request.Form[txtRemaksRCC.UniqueID];
                                entityDtRCCDao.Insert(entityDtRCC);
                            }
                        }
                        BudgetTemplateDtID = Convert.ToInt32(hdnBudgetTemplateDtID.Value);
                    }
                    else
                    {
                        String filterBudgetTemplateRCC = string.Format("BudgetTemplateDtID = {0} AND IsDeleted = 0", hdnBudgetTemplateDtID.Value);
                        List<BudgetTemplateDtRevenueCostCenter> lstentityBudgetTemplateRCC = BusinessLayer.GetBudgetTemplateDtRevenueCostCenterList(filterBudgetTemplateRCC, ctx);

                        foreach (BudgetTemplateDtRevenueCostCenter e in lstentityBudgetTemplateRCC)
                        {
                            if (e.ID.ToString() == hdnBudgetTemplateDtRCCID.Value)
                            {
                                BudgetTransactionDtRevenueCostCenter entityDtRCC = new BudgetTransactionDtRevenueCostCenter();
                                entityDtRCC.BudgetTransactionDtID = dtID;
                                entityDtRCC.DepartmentID = hdnDepartmentID.Value;
                                entityDtRCC.RevenueCostCenterID = Convert.ToInt32(hdnRevenueCostCenterID.Value);
                                entityDtRCC.BudgetAmount = Convert.ToDecimal(Request.Form[txtBudgetAmount.UniqueID]);
                                entityDtRCC.Remarks = Request.Form[txtRemaksRCC.UniqueID];
                                entityDtRCCDao.Insert(entityDtRCC);
                            }
                        }
                        BudgetTemplateDtID = Convert.ToInt32(hdnBudgetTemplateDtID.Value);
                    }
                }
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BudgetTransactionHdDao entityHdDao = new BudgetTransactionHdDao(ctx);
            BudgetTransactionDtDao entityDtDao = new BudgetTransactionDtDao(ctx);

            int BudgetTransactionID = 0;
            try
            {
                DetailPage.SaveBudgetHd(ctx, ref BudgetTransactionID);

                ControlToEntity(ctx, BudgetTransactionID);

                retval = BudgetTransactionID.ToString();
                ctx.CommitTransaction();
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