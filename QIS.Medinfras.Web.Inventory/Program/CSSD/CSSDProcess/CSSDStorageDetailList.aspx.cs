using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class CSSDStorageDetailList : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CSSD_STORAGING;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            #region MDServiceRequest

            hdnServiceRequestID.Value = Page.Request.QueryString["id"];
            vMDServiceRequestHd entity = BusinessLayer.GetvMDServiceRequestHdList(string.Format("RequestID = {0}", hdnServiceRequestID.Value)).FirstOrDefault();
            
            EntityToControl(entity);

            #endregion
        }

        #region MDServiceRequest

        private void EntityToControl(vMDServiceRequestHd entity)
        {
            hdnServiceRequestID.Value = entity.RequestID.ToString();
            txtRequestNo.Text = entity.RequestNo;
            txtRequestDate.Text = entity.RequestDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRequestTime.Text = entity.RequestTime;
            txtLastUpdatedByName.Text = entity.LastUpdatedByName;
            txtLastUpdatedDate.Text = entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtLastUpdatedTime.Text = entity.LastUpdatedDate.ToString(Constant.FormatString.TIME_FORMAT);
            hdnLocationIDFrom.Value = entity.FromLocationID.ToString();
            txtLocationCode.Text = entity.FromLocationCode;
            txtLocationName.Text = entity.FromLocationName;
            hdnLocationIDTo.Value = entity.ToLocationID.ToString();
            txtLocationCodeTo.Text = entity.ToLocationCode;
            txtLocationNameTo.Text = entity.ToLocationName;
            txtNotes.Text = entity.Remarks;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";

            if (hdnServiceRequestID.Value != "")
            {
                filterExpression = string.Format("RequestID = {0} AND GCServiceDetailStatus IN ('{1}') AND IsDeleted = 0", hdnServiceRequestID.Value, Constant.ServiceStatus.PROCESSED);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvMDServiceRequestDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_FIVE);
            }

            List<vMDServiceRequestDt> lstEntity = BusinessLayer.GetvMDServiceRequestDtList(filterExpression, Constant.GridViewPageSize.GRID_FIVE, pageIndex, "ItemName1 ASC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
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

        #endregion

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            MDServiceRequestHdDao reqHdDao = new MDServiceRequestHdDao(ctx);
            MDServiceRequestDtDao reqDtDao = new MDServiceRequestDtDao(ctx);
            ItemDistributionHdDao distHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionDtDao distDtDao = new ItemDistributionDtDao(ctx);

            try
            {
                if (type == "processed")
                {
                    MDServiceRequestHd reqHd = reqHdDao.Get(Convert.ToInt32(hdnServiceRequestID.Value));

                    ItemDistributionHd distHd = new ItemDistributionHd();
                    distHd.TransactionCode = Constant.TransactionCode.SERVICE_RETURN;
                    distHd.ServiceRequestID = reqHd.RequestID;
                    distHd.FromLocationID = reqHd.ToLocationID;
                    distHd.ToLocationID = reqHd.FromLocationID;
                    distHd.DeliveryDate = DateTime.Now;
                    distHd.DeliveryTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    distHd.DeliveryRemarks = "Distribusi pengembalian dari permintaan CSSD di nomor " + reqHd.RequestNo;
                    distHd.DeliveredBy = BusinessLayer.GetUser(AppSession.UserLogin.UserID).UserName;
                    distHd.DeliveryDate = DateTime.Now;
                    distHd.DeliveryTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    distHd.GCDistributionStatus = Constant.DistributionStatus.OPEN;
                    distHd.DistributionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.SERVICE_DISTRIBUTION, distHd.DeliveryDate, ctx);
                    distHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    distHdDao.Insert(distHd);
                    int oDistributionID = BusinessLayer.GetItemDistributionHdMaxID(ctx);

                    string filterlstReqDt = string.Format("RequestID = {0} AND IsDeleted = 0 AND GCServiceDetailStatus = '{1}'", reqHd.RequestID, Constant.ServiceStatus.PROCESSED);
                    List<MDServiceRequestDt> lstReqDt = BusinessLayer.GetMDServiceRequestDtList(filterlstReqDt);

                    if (reqHd.GCServiceStatus == Constant.ServiceStatus.PROCESSED)
                    {
                        foreach (MDServiceRequestDt reqDt in lstReqDt)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            reqDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            reqDtDao.Update(reqDt);

                            ItemDistributionDt distDt = new ItemDistributionDt();
                            distDt.DistributionID = oDistributionID;
                            distDt.ItemID = reqDt.ItemID;
                            distDt.Quantity = reqDt.Quantity;
                            distDt.GCItemUnit = reqDt.GCItemUnit;
                            distDt.GCBaseUnit = reqDt.GCBaseUnit;
                            distDt.ConversionFactor = reqDt.ConversionFactor;
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
                        }

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        reqHd.GCServiceStatus = Constant.ServiceStatus.ON_RETURN;
                        reqHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        reqHdDao.Update(reqHd);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Proses CSSD dengan nomor " + reqHd.RequestNo + " tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);  
                        ctx.RollBackTransaction();
                    }
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