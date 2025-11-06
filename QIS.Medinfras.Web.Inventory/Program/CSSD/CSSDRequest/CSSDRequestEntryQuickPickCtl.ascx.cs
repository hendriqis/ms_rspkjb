using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class CSSDRequestEntryQuickPickCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private CSSDRequestEntry DetailPage
        {
            get { return (CSSDRequestEntry)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] temp = param.Split('|');
            hdnRequestIDCtl.Value = temp[0];
            hdnLocationIDFromCtl.Value = temp[1];
            hdnLocationIDToCtl.Value = temp[2];
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

            filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}', '{2}') AND ItemName1 LIKE '%{3}%' AND IsDeleted = 0",
                                                hdnLocationIDFromCtl.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, hdnFilterItem.Value);

            filterExpression += string.Format(" AND GCItemStatus != '{0}'", Constant.ItemStatus.IN_ACTIVE);

            filterExpression += string.Format(" AND ItemID IN (SELECT ItemID FROM DrugInfo WHERE IsCSSD = 1)");

            return filterExpression;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vItemBalance entity = e.Row.DataItem as vItemBalance;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.ItemID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (hdnRequestIDCtl.Value != "0" && hdnRequestIDCtl.Value != "")
            {
                List<vMDServiceRequestDt> lstItemID = BusinessLayer.GetvMDServiceRequestDtList(string.Format("RequestID = {0} AND IsDeleted = 0", hdnRequestIDCtl.Value));
                string lstSelectedID = "";
                if (lstItemID.Count > 0)
                {
                    foreach (vMDServiceRequestDt itm in lstItemID)
                    {
                        lstSelectedID += "," + itm.ItemID;
                    }
                    filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));
                }
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemBalanceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }
            lstSelectedMember = hdnSelectedMember.Value.Split(',');

            List<vItemBalance> lstEntity = BusinessLayer.GetvItemBalanceList(filterExpression, 8, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            MDServiceRequestHdDao entityHdDao = new MDServiceRequestHdDao(ctx);
            MDServiceRequestDtDao entityDtDao = new MDServiceRequestDtDao(ctx);
            ItemDistributionHdDao distHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionDtDao distDtDao = new ItemDistributionDtDao(ctx);

            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                int RequestID = 0;
                DetailPage.SaveMDServiceRequestHd(ctx, ref RequestID);

                int oDistributionID = 0;
                string filterDistHd = string.Format("ServiceRequestID = {0} AND TransactionCode = '{1}'", RequestID, Constant.TransactionCode.SERVICE_DISTRIBUTION);
                List<ItemDistributionHd> distHdLst = BusinessLayer.GetItemDistributionHdList(filterDistHd, ctx);

                if (distHdLst.Count == 0)
                {
                    ItemDistributionHd distHd = new ItemDistributionHd();
                    distHd.TransactionCode = Constant.TransactionCode.SERVICE_DISTRIBUTION;
                    distHd.ServiceRequestID = RequestID;
                    distHd.FromLocationID = Convert.ToInt32(hdnLocationIDFromCtl.Value);
                    distHd.ToLocationID = Convert.ToInt32(hdnLocationIDToCtl.Value);
                    distHd.DeliveryDate = DateTime.Now;
                    distHd.DeliveryTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    distHd.DeliveryRemarks = "Distribusi dari permintaan CSSD di nomor " + entityHdDao.Get(RequestID).RequestNo;
                    distHd.GCDistributionStatus = Constant.DistributionStatus.OPEN;
                    distHd.DistributionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.SERVICE_DISTRIBUTION, distHd.DeliveryDate, ctx);
                    distHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    oDistributionID = distHdDao.InsertReturnPrimaryKeyID(distHd);
                }
                else
                {
                    oDistributionID = distHdLst.FirstOrDefault().DistributionID;
                }

                if (entityHdDao.Get(RequestID).GCServiceStatus == Constant.ServiceStatus.OPEN)
                {
                    List<ItemMaster> lstItemMaster = BusinessLayer.GetItemMasterList(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);

                    List<ItemBalance> lstItemBalance = BusinessLayer.GetItemBalanceList(string.Format("ItemID IN ({0}) AND LocationID = {1}", hdnSelectedMember.Value, entityHdDao.Get(RequestID).FromLocationID), ctx);

                    int ct = 0;
                    foreach (String itemID in lstSelectedMember)
                    {
                        ItemMaster itemMaster = lstItemMaster.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));
                        ItemBalance itemBalance = lstItemBalance.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));

                        MDServiceRequestDt entityDt = new MDServiceRequestDt();
                        entityDt.RequestID = RequestID;
                        entityDt.ItemID = itemMaster.ItemID;
                        entityDt.Quantity = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                        entityDt.GCItemUnit = itemMaster.GCItemUnit;
                        entityDt.BaseQuantity = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                        entityDt.GCBaseUnit = itemMaster.GCItemUnit;
                        entityDt.ConversionFactor = 1;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Insert(entityDt);

                        ItemDistributionDt distDt = new ItemDistributionDt();
                        distDt.DistributionID = oDistributionID;
                        distDt.ItemID = entityDt.ItemID;
                        distDt.Quantity = entityDt.Quantity;
                        distDt.GCItemUnit = entityDt.GCItemUnit;
                        distDt.GCBaseUnit = entityDt.GCBaseUnit;
                        distDt.ConversionFactor = entityDt.ConversionFactor;
                        distDt.GCItemDetailStatus = Constant.DistributionStatus.OPEN;
                        distDt.CreatedBy = AppSession.UserLogin.UserID;

                        ItemPlanning entityPlanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0}", distDt.ItemID), ctx).FirstOrDefault();
                        if (entityPlanning != null)
                        {
                            distDt.AveragePrice = entityPlanning.AveragePrice;
                        }
                        else
                        {
                            distDt.AveragePrice = 0;
                        }

                        distDtDao.Insert(distDt);

                        ct++;
                    }
                    retval = RequestID.ToString();
                    ctx.CommitTransaction();
                }
                else 
                {
                    result = false;
                    errMessage = "Permintaan CSSD tidak dapat diubah. Harap refresh halaman ini.";
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