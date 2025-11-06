using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ItemDistributionConfirmedList : BasePageTrx
    {
        protected int PageCount = 1;
        protected String filterExpressionLocation = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_DISTRIBUTION_CONFIRMED;
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.ITEM_REQUEST);
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += String.Format("ServiceRequestID IS NULL AND GCDistributionStatus = '{0}' AND ToLocationID = {1}", Constant.DistributionStatus.ON_DELIVERY, hdnLocationIDFrom.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemDistributionHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vItemDistributionHd> lstEntity = BusinessLayer.GetvItemDistributionHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "DistributionNo DESC");
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

        public void SaveItemDistributionHd(IDbContext ctx, ref int distributionID, ref string distributionNo)
        {
            ItemDistributionHdDao entityHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionHd entityHd = new ItemDistributionHd();
            //entityHd.ItemRequestID = Convert.ToInt32(hdnOrderID.Value);
            //entityHd.FromLocationID = Convert.ToInt32(hdnLocationIDTo.Value);
            //entityHd.ToLocationID = Convert.ToInt32(hdnLocationIDFrom.Value);
            entityHd.DeliveredBy = BusinessLayer.GetUser(AppSession.UserLogin.UserID).UserName;
            entityHd.DeliveryDate = DateTime.Now;
            entityHd.DeliveryTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            //entityHd.DeliveryRemarks = string.Format("Distribusi untuk permintaan Nomor {0} dari {1}", Request.Form[txtOrderNo.UniqueID], Request.Form[txtLocationName.UniqueID]);
            entityHd.DistributionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.ITEM_DISTRIBUTION, entityHd.DeliveryDate, ctx);
            entityHd.GCDistributionStatus = Constant.DistributionStatus.OPEN;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            distributionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            distributionNo = entityHd.DistributionNo;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "approve")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                ItemDistributionHdDao distributionHdDao = new ItemDistributionHdDao(ctx);
                ItemDistributionDtDao distributionDtDao = new ItemDistributionDtDao(ctx);
                ItemRequestDtDao entityItemRequestDtDao = new ItemRequestDtDao(ctx);
                ItemRequestHdDao entityItemRequestHdDao = new ItemRequestHdDao(ctx);

                try
                {
                    string filterExpressionDistributionHd = String.Format("DistributionID IN ({0})", hdnParam.Value);

                    List<ItemDistributionDt> lstItemDistributionDt = BusinessLayer.GetItemDistributionDtList(filterExpressionDistributionHd);
                    foreach (ItemDistributionDt distributionDt in lstItemDistributionDt)
                    {
                        if (distributionHdDao.Get(distributionDt.DistributionID).GCDistributionStatus == Constant.DistributionStatus.ON_DELIVERY)
                        {
                            distributionDt.GCItemDetailStatus = Constant.DistributionStatus.RECEIVED;
                            distributionDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            distributionDtDao.Update(distributionDt);
                        }
                    }

                    List<ItemDistributionHd> lstItemDistributionHd = BusinessLayer.GetItemDistributionHdList(filterExpressionDistributionHd); 
                    foreach (ItemDistributionHd distributionHd in lstItemDistributionHd)
                    {
                        if (distributionHd.GCDistributionStatus == Constant.DistributionStatus.ON_DELIVERY)
                        {
                            distributionHd.GCDistributionStatus = Constant.DistributionStatus.RECEIVED;
                            distributionHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            distributionHd.ReceivedDate = DateTime.Now;
                            distributionHd.ReceivedTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                            distributionHd.ReceivedBy = BusinessLayer.GetUser(AppSession.UserLogin.UserID).UserName;
                            distributionHdDao.Update(distributionHd);

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            if (distributionHd.ItemRequestID != 0 && distributionHd.ItemRequestID != null)
                            {
                                string filterIRHD = String.Format("ItemRequestID = (SELECT ItemRequestID FROM ItemRequestHd WHERE ItemRequestNo = (SELECT ReferenceNo FROM ItemRequestHd WHERE ItemRequestID = {0} AND GCTransactionStatus != '{1}') AND GCTransactionStatus = '{2}')", distributionHd.ItemRequestID, Constant.TransactionStatus.VOID, Constant.TransactionStatus.APPROVED);
                                vItemRequestHd irh = BusinessLayer.GetvItemRequestHdList(filterIRHD, ctx).FirstOrDefault();
                                if (irh != null)
                                {
                                    int distributionID = 0;
                                    ItemDistributionHd distHdCopy = new ItemDistributionHd();
                                    distHdCopy.ItemRequestID = irh.ItemRequestID;
                                    distHdCopy.FromLocationID = irh.ToLocationID;
                                    distHdCopy.ToLocationID = irh.FromLocationID;
                                    distHdCopy.DeliveredBy = BusinessLayer.GetUser(AppSession.UserLogin.UserID).UserName;
                                    distHdCopy.DeliveryDate = DateTime.Now;
                                    distHdCopy.DeliveryTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                                    distHdCopy.DeliveryRemarks = string.Format("Distribusi untuk permintaan Nomor {0} dari {1}", irh.ItemRequestNo, irh.FromLocationName);
                                    distHdCopy.DistributionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.ITEM_DISTRIBUTION, distHdCopy.DeliveryDate, ctx);
                                    distHdCopy.GCDistributionStatus = Constant.DistributionStatus.OPEN;
                                    distHdCopy.CreatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    distributionID = distributionHdDao.InsertReturnPrimaryKeyID(distHdCopy);

                                    ItemRequestHd reqHdCopy = entityItemRequestHdDao.Get(irh.ItemRequestID);
                                    reqHdCopy.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                    reqHdCopy.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityItemRequestHdDao.Update(reqHdCopy);

                                    string filterReqDtCopy = String.Format("ItemRequestID IN ({0})", irh.ItemRequestID);
                                    List<ItemRequestDt> lstReqDtCopy = BusinessLayer.GetItemRequestDtList(filterReqDtCopy, ctx);
                                    foreach (ItemRequestDt reqDtCopy in lstReqDtCopy)
                                    {
                                        ItemDistributionDt itemDt = new ItemDistributionDt();

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

                                        itemDt.DistributionID = distributionID;
                                        itemDt.ItemID = reqDtCopy.ItemID;
                                        itemDt.Quantity = reqDtCopy.Quantity;
                                        itemDt.ConversionFactor = reqDtCopy.ConversionFactor;
                                        itemDt.GCItemUnit = reqDtCopy.GCItemUnit;
                                        itemDt.GCBaseUnit = reqDtCopy.GCBaseUnit;
                                        itemDt.GCItemDetailStatus = Constant.DistributionStatus.OPEN;
                                        itemDt.CreatedBy = AppSession.UserLogin.UserID;

                                        ItemPlanning entityPlanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0}", Convert.ToInt32(reqDtCopy.ItemID)), ctx).FirstOrDefault();
                                        if (entityPlanning != null)
                                        {
                                            itemDt.AveragePrice = entityPlanning.AveragePrice;
                                        }
                                        else
                                        {
                                            itemDt.AveragePrice = 0;
                                        }

                                        reqDtCopy.DistributionQty += itemDt.Quantity;
                                        reqDtCopy.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                                        reqDtCopy.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityItemRequestDtDao.Update(reqDtCopy);

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        distributionDtDao.Insert(itemDt);
                                    }
                                }
                            }
                        }
                    }

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
            else
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                ItemDistributionHdDao distributionHdDao = new ItemDistributionHdDao(ctx);
                ItemDistributionDtDao distributionDtDao = new ItemDistributionDtDao(ctx);
                ItemRequestHdDao itemRequestHdDao = new ItemRequestHdDao(ctx);
                ItemRequestDtDao itemRequestDtDao = new ItemRequestDtDao(ctx);
                try
                {
                    string filterExpressionDistributionHd = String.Format("DistributionID IN ({0})", hdnParam.Value);

                    List<ItemDistributionDt> lstItemDistributionDt = BusinessLayer.GetItemDistributionDtList(filterExpressionDistributionHd);
                    foreach (ItemDistributionDt distributionDt in lstItemDistributionDt)
                    {
                        if (distributionHdDao.Get(distributionDt.DistributionID).GCDistributionStatus == Constant.DistributionStatus.ON_DELIVERY)
                        {
                            distributionDt.GCItemDetailStatus = Constant.DistributionStatus.OPEN;
                            distributionDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            distributionDtDao.Update(distributionDt);
                        }
                    }

                    List<ItemDistributionHd> lstItemDistributionHd = BusinessLayer.GetItemDistributionHdList(filterExpressionDistributionHd);
                    foreach (ItemDistributionHd distributionHd in lstItemDistributionHd)
                    {
                        if (distributionHd.GCDistributionStatus == Constant.DistributionStatus.ON_DELIVERY)
                        {
                            distributionHd.GCDistributionStatus = Constant.DistributionStatus.OPEN;
                            distributionHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            distributionHdDao.Update(distributionHd);
                        }

                        foreach (ItemDistributionDt distributionDt in lstItemDistributionDt)
                        {
                            if (distributionHd.ItemRequestID != null && distributionHd.ItemRequestID != 0)
                            {
                                string filterItemRequestDt = string.Format("ItemRequestID = {0} AND ItemID = {1} AND GCItemDetailStatus IN ('{2}','{3}')",
                                                                        distributionHd.ItemRequestID, distributionDt.ItemID, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                                ItemRequestDt irequestDt = BusinessLayer.GetItemRequestDtList(filterItemRequestDt, ctx).FirstOrDefault();
                                if (irequestDt != null)
                                {
                                    irequestDt.ApprovedDistributionQty = irequestDt.DistributionQty - distributionDt.Quantity;
                                    irequestDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    itemRequestDtDao.Update(irequestDt);
                                }
                            }
                        }
                    }

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
}