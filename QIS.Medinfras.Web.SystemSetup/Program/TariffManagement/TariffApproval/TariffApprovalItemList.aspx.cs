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
using System.Text;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxTreeList;
using System.Data;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class TariffApprovalItemList : BasePageListEntry
    {
        protected int PageCount = 1;
        protected int ClassCount = 0;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.TARIFF_APPROVAL;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = false;
        }

        public override string OnGetCustomMenuCaption()
        {
            return "Tariff Approval Item";
        }

        public override string OnGetCustomBreadcrumbs(string breadCrumbs)
        {
            return string.Format("{0} > Tariff Approval Item", breadCrumbs);
        }

        #region List
        List<ClassCare> ListClassCare = null;
        protected override void OnLoad(EventArgs e)
        {
            if (hdnListClassID.Value == "")
            {
                hdnBookID.Value = Page.Request.QueryString["id"];
                vTariffBookHd entity = BusinessLayer.GetvTariffBookHdList(string.Format("BookID = {0}", hdnBookID.Value))[0];

                txtHealthcare.Text = entity.HealthcareName;
                txtTariffScheme.Text = entity.TariffScheme;
                txtEffectiveDate.Text = entity.StartingDateInString;
                txtDocumentNo.Text = entity.DocumentNo;
                txtDocumentDate.Text = entity.DocumentDateInString;
                hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
                txtRevisionNo.Text = entity.RevisionNo.ToString();

                //hdnGCItemType.Value = entity.GCItemType;

                List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0");
                StringBuilder sbListClassID = new StringBuilder();
                StringBuilder sbListClassName = new StringBuilder();
                StringBuilder sbListClassMargin = new StringBuilder();

                foreach (ClassCare classCare in lstClassCare)
                {
                    if (sbListClassID.ToString() != "")
                    {
                        sbListClassID.Append("|");
                        sbListClassName.Append("|");
                        sbListClassMargin.Append("|");
                    }
                    sbListClassID.Append(classCare.ClassID);
                    sbListClassName.Append(classCare.ClassName);
                    int marginPercentage = 0;
                    if (hdnGCItemType.Value == Constant.ItemGroupMaster.SERVICE || hdnGCItemType.Value == Constant.ItemGroupMaster.RADIOLOGY || hdnGCItemType.Value == Constant.ItemGroupMaster.LABORATORY || hdnGCItemType.Value == Constant.ItemGroupMaster.DIAGNOSTIC)
                        marginPercentage = (int)classCare.MarginPercentage1;
                    else if (hdnGCItemType.Value == Constant.ItemGroupMaster.DRUGS || hdnGCItemType.Value == Constant.ItemGroupMaster.SUPPLIES)
                        marginPercentage = (int)classCare.MarginPercentage2;
                    else
                        marginPercentage = (int)classCare.MarginPercentage3;
                    sbListClassMargin.Append(marginPercentage);
                }

                hdnListClassID.Value = sbListClassID.ToString();
                hdnListClassName.Value = sbListClassName.ToString();
                hdnListClassMargin.Value = sbListClassMargin.ToString();

                ASPxTreeList treeList = (ASPxTreeList)ddeItemGroup.FindControl("treeList");
                //Session["filterExpressionTariffBookItemGroup"] = String.Format("GCItemType = '{0}' AND IsDeleted = 0", entity.GCItemType);

                treeList.DataBind();
                treeList.UnselectAll();
                ddeItemGroup.Text = "";
            }

            BindingRptClass();
            base.OnLoad(e);
        }

        private void BindingRptClass()
        {
            string[] lstClassID = hdnListClassID.Value.Split('|');
            string[] lstClassName = hdnListClassName.Value.Split('|');
            string[] lstClassMargin = hdnListClassMargin.Value.Split('|');
            ListClassCare = new List<ClassCare>();
            for (int i = 0; i < lstClassID.Length; ++i)
            {
                ClassCare entity = new ClassCare();
                entity.ClassID = Convert.ToInt32(lstClassID[i]);
                entity.ClassName = lstClassName[i];
                entity.MarginPercentage1 = Convert.ToDecimal(lstClassMargin[i]);
                ListClassCare.Add(entity);
            }

            ClassCount = ListClassCare.Count;

            rptClassCareProposed.DataSource = ListClassCare;
            rptClassCareProposed.DataBind();

            rptClassCareApproved.DataSource = ListClassCare;
            rptClassCareApproved.DataBind();

            rptTariffBookClassHeader.DataSource = ListClassCare;
            rptTariffBookClassHeader.DataBind();
        }

        protected override void InitializeDataControl()
        {
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            if (hdnListItemGroupID.Value != "")
                filterExpression += string.Format("ItemGroupID IN ({0}) AND ", hdnListItemGroupID.Value);
            
            filterExpression += string.Format("BookID = {0}", hdnBookID.Value);
            if (chkDisplayUpapprovedOnly.Checked)
                filterExpression += " AND IsApproved = 0";

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTariffBookDtCustomRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTariffBookDtCustom> lstEntity = BusinessLayer.GetvTariffBookDtCustomList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vTariffBookDtCustom obj = (vTariffBookDtCustom)e.Item.DataItem;
                Repeater rptTariffBookClass = (Repeater)e.Item.FindControl("rptTariffBookClass");
                List<decimal> lstTariff = null;
                if (obj.IsApproved)
                    lstTariff = obj.ApprovedTariff.Split(';').ToList().Select(decimal.Parse).ToList();
                else
                    lstTariff = obj.ProposedTariff.Split(';').ToList().Select(decimal.Parse).ToList();

                List<int> lstClassID = obj.ClassID.Split(';').ToList().Select(int.Parse).ToList();
                List<decimal> lstOrderedTariff = new List<decimal>();
                for (int i = 0; i < ListClassCare.Count; ++i)
                {
                    int idx = lstClassID.IndexOf(ListClassCare[i].ClassID);
                    if (idx > -1)
                        lstOrderedTariff.Add(lstTariff[idx]);
                    else
                        lstOrderedTariff.Add(0);
                }

                rptTariffBookClass.DataSource = lstOrderedTariff;
                rptTariffBookClass.DataBind();

                CheckBox chkIsApproved = (CheckBox)e.Item.FindControl("chkIsApproved");
                chkIsApproved.Checked = obj.IsApproved;
            }
        }

        protected void cbpGetTreeListSelectedValue_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            ASPxTreeList treeList = (ASPxTreeList)ddeItemGroup.FindControl("treeList");
            if (treeList.GetSelectedNodes().Count > 0)
            {
                foreach (TreeListNode node in treeList.GetSelectedNodes())
                {
                    if (result != "")
                        result += ", ";
                    result += ((ItemGroupMaster)node.DataItem).ItemGroupID;
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
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

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                TariffBookDtDao entityDao = new TariffBookDtDao(ctx);
                try
                {
                    int bookID = Convert.ToInt32(hdnBookID.Value);
                    int itemID = Convert.ToInt32(hdnID.Value);
                    List<TariffBookDt> lstTariffBookDt = BusinessLayer.GetTariffBookDtList(string.Format("BookID = {0} AND ItemID = {1}", bookID, itemID), ctx);
                    foreach (TariffBookDt entity in lstTariffBookDt)
                    {
                        entityDao.Delete(bookID, itemID, entity.ClassID);
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    ctx.RollBackTransaction();
                    result = false;
                }
                finally
                {
                    ctx.Close();
                }
                return result;
            }
            return false;
        }
        #endregion

        #region Entry
        protected void treeList_CustomDataCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomDataCallbackEventArgs e)
        {
            ASPxTreeList treeList = sender as ASPxTreeList;
            string result = "";
            foreach (TreeListNode node in treeList.GetSelectedNodes())
            {
                if (result != "")
                    result += ";";
                result += ((ItemGroupMaster)node.DataItem).ItemGroupName1;
            }
            e.Result = result;
        }

        protected override void SetControlProperties()
        {
            
        }

        protected override void OnControlEntrySetting()
        {
            //SetControlEntrySetting(ledDrugName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSuggestedTariff, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtBaseTariff, new ControlEntrySetting(true, true, true));
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TariffBookDtDao entityDao = new TariffBookDtDao(ctx);
            TariffBookDtCostDao entityCostDao = new TariffBookDtCostDao(ctx);
            try
            {
                int bookID = Convert.ToInt32(hdnBookID.Value);
                int itemID = Convert.ToInt32(hdnItemID.Value);
                List<TariffBookDt> lstTariffBookDt = BusinessLayer.GetTariffBookDtList(string.Format("BookID = {0} AND ItemID = {1}", bookID, itemID), ctx);
                List<TariffBookDtCost> lstTariffBookDtCost = BusinessLayer.GetTariffBookDtCostList(string.Format("BookID = {0} AND ItemID = {1}", bookID, itemID), ctx);

                decimal approvedBaseTariff = Convert.ToDecimal(txtApprovedBaseTariff.Text);
                foreach (RepeaterItem item in rptClassCareApproved.Items)
                {
                    TextBox txtClass = (TextBox)item.FindControl("txtClass");
                    HtmlInputHidden hdnClassID = (HtmlInputHidden)item.FindControl("hdnClassID");

                    HtmlInputHidden hdnPrevBurden = (HtmlInputHidden)item.FindControl("hdnPrevBurden");
                    HtmlInputHidden hdnPrevLabor = (HtmlInputHidden)item.FindControl("hdnPrevLabor");
                    HtmlInputHidden hdnPrevMaterial = (HtmlInputHidden)item.FindControl("hdnPrevMaterial");
                    HtmlInputHidden hdnPrevOverhead = (HtmlInputHidden)item.FindControl("hdnPrevOverhead");
                    HtmlInputHidden hdnPrevSubContract = (HtmlInputHidden)item.FindControl("hdnPrevSubContract");

                    HtmlInputHidden hdnCurrentBurden = (HtmlInputHidden)item.FindControl("hdnCurrentBurden");
                    HtmlInputHidden hdnCurrentLabor = (HtmlInputHidden)item.FindControl("hdnCurrentLabor");
                    HtmlInputHidden hdnCurrentMaterial = (HtmlInputHidden)item.FindControl("hdnCurrentMaterial");
                    HtmlInputHidden hdnCurrentOverhead = (HtmlInputHidden)item.FindControl("hdnCurrentOverhead");
                    HtmlInputHidden hdnCurrentSubContract = (HtmlInputHidden)item.FindControl("hdnCurrentSubContract");

                    HtmlInputHidden hdnTotalBurden = (HtmlInputHidden)item.FindControl("hdnTotalBurden");
                    HtmlInputHidden hdnTotalLabor = (HtmlInputHidden)item.FindControl("hdnTotalLabor");
                    HtmlInputHidden hdnTotalMaterial = (HtmlInputHidden)item.FindControl("hdnTotalMaterial");
                    HtmlInputHidden hdnTotalOverhead = (HtmlInputHidden)item.FindControl("hdnTotalOverhead");
                    HtmlInputHidden hdnTotalSubContract = (HtmlInputHidden)item.FindControl("hdnTotalSubContract");

                    int classID = Convert.ToInt32(hdnClassID.Value);
                    TariffBookDt entity = lstTariffBookDt.FirstOrDefault(p => p.ClassID == classID);
                    if (entity != null)
                    {
                        entity.ApprovedBaseTariff = approvedBaseTariff;
                        entity.ApprovedTariff = Convert.ToDecimal(Request.Form[txtClass.UniqueID]);
                        entity.IsApproved = chkIsApproved.Checked;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);
                    }

                    TariffBookDtCost entityCost = lstTariffBookDtCost.FirstOrDefault(p => p.ClassID == classID);
                    if (entityCost != null)
                    {
                        entityCost.PreviousBurden = Convert.ToDecimal(Request.Form[hdnPrevBurden.UniqueID]);
                        entityCost.PreviousLabor = Convert.ToDecimal(Request.Form[hdnPrevLabor.UniqueID]);
                        entityCost.PreviousMaterial = Convert.ToDecimal(Request.Form[hdnPrevMaterial.UniqueID]);
                        entityCost.PreviousOverhead = Convert.ToDecimal(Request.Form[hdnPrevOverhead.UniqueID]);
                        entityCost.PreviousSubContract = Convert.ToDecimal(Request.Form[hdnPrevSubContract.UniqueID]);

                        entityCost.CurrentBurden = Convert.ToDecimal(Request.Form[hdnCurrentBurden.UniqueID]);
                        entityCost.CurrentLabor = Convert.ToDecimal(Request.Form[hdnCurrentLabor.UniqueID]);
                        entityCost.CurrentMaterial = Convert.ToDecimal(Request.Form[hdnCurrentMaterial.UniqueID]);
                        entityCost.CurrentOverhead = Convert.ToDecimal(Request.Form[hdnCurrentOverhead.UniqueID]);
                        entityCost.CurrentSubContract = Convert.ToDecimal(Request.Form[hdnCurrentSubContract.UniqueID]);

                        entityCost.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityCostDao.Update(entityCost);
                    }
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
    }
}