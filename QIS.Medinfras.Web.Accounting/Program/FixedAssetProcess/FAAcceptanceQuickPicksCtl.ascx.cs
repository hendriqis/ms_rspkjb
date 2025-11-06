using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class FAAcceptanceQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private FAAcceptanceProcessEntry DetailPage
        {
            get { return (FAAcceptanceProcessEntry)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            string[] lstParam = param.Split('|');

            hdnFAAcceptanceIDCtl.Value = lstParam[0];
            hdnProductLineIDCtl.Value = lstParam[1];
            hdnFAGroupIDCtl.Value = lstParam[2];
            hdnFALocationIDCtl.Value = lstParam[3];

            BindGridView(1, true, ref PageCount);
        }

        protected void cbpPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        private string GetFilterExpression()
        {
            string filterExpression = "";

            filterExpression = string.Format("GCItemStatus != '{0}' AND IsDeleted = 0", Constant.ItemStatus.IN_ACTIVE);

            filterExpression += string.Format(" AND FixedAssetID NOT IN (SELECT FixedAssetID FROM FAAcceptanceDt WHERE FAAcceptanceID IN (SELECT FAAcceptanceID FROM FAAcceptanceHd WHERE GCTransactionStatus NOT IN ('{0}')) AND IsDeleted = 0)", Constant.TransactionStatus.VOID);

            if (hdnFAGroupIDCtl.Value != "" && hdnFAGroupIDCtl.Value != "0")
            {
                filterExpression += string.Format(" AND FAGroupID = {0}", hdnFAGroupIDCtl.Value);
            }

            if (hdnFALocationIDCtl.Value != "" && hdnFALocationIDCtl.Value != "0")
            {
                filterExpression += string.Format(" AND FALocationID = {0}", hdnFALocationIDCtl.Value);
            }

            if (hdnFilterItemCode.Value != "")
            {
                filterExpression += string.Format(" AND FixedAssetCode LIKE '%{0}%'", hdnFilterItemCode.Value);
            }

            if (hdnFilterItemName.Value != "")
            {
                filterExpression += string.Format(" AND FixedAssetName LIKE '%{0}%'", hdnFilterItemName.Value);
            }

            if (hdnFilterSerialNumber.Value != "")
            {
                filterExpression += string.Format(" AND SerialNumber LIKE '%{0}%'", hdnFilterSerialNumber.Value);
            }

            return filterExpression;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vFAItem entity = e.Row.DataItem as vFAItem;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.ItemID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFAItemRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }
            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vFAItem> lstEntity = BusinessLayer.GetvFAItemList(filterExpression, 10, pageIndex, "FixedAssetCode ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FAAcceptanceHdDao entityHdDao = new FAAcceptanceHdDao(ctx);
            FAAcceptanceDtDao entityDtDao = new FAAcceptanceDtDao(ctx);
            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                int FAAcceptanceID = 0;
                string FAAcceptanceNo = "";
                DetailPage.SaveFAAcceptanceHd(ctx, ref FAAcceptanceID, ref FAAcceptanceNo);
                if (entityHdDao.Get(FAAcceptanceID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    List<FAItem> lstItemMaster = BusinessLayer.GetFAItemList(string.Format("FixedAssetID IN ({0})", hdnSelectedMember.Value), ctx);
                    int ct = 0;
                    foreach (String itemID in lstSelectedMember)
                    {
                        FAItem itemMaster = lstItemMaster.FirstOrDefault(p => p.FixedAssetID == Convert.ToInt32(itemID));

                        FAAcceptanceDt entityDt = new FAAcceptanceDt();
                        entityDt.FAAcceptanceID = FAAcceptanceID;
                        entityDt.FixedAssetID = itemMaster.FixedAssetID;
                        entityDt.IsDeleted = false;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;

                        entityDtDao.Insert(entityDt);
                        ct++;
                    }
                    retval = FAAcceptanceID.ToString();
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "";
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