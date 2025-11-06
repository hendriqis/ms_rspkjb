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
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class CSSDStorageList : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";
        protected string filterExpressionLocationTo = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CSSD_STORAGING;
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        public override bool IsShowRightPanel()
        {
            return false;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = false;
            IsAllowSave = false;
        }
        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            filterExpressionLocation = string.Format("{0};0;{1};", AppSession.UserLogin.HealthcareID, Constant.TransactionCode.SERVICE_REQUEST);
            
            int count = BusinessLayer.GetLocationUserRowCount(string.Format("UserID = {0} AND IsDeleted = 0", AppSession.UserLogin.UserID));
            if (count > 0)
                hdnRecordFilterExpression.Value = string.Format("ToLocationID IN (SELECT LocationID FROM LocationUser WHERE UserID = {0} AND IsDeleted = 0)", AppSession.UserLogin.UserID);
            else
            {
                count = BusinessLayer.GetLocationUserRoleRowCount(string.Format("RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID));
                if (count > 0)
                    hdnRecordFilterExpression.Value = string.Format("ToLocationID IN (SELECT LocationID FROM LocationUserRole WHERE RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0)", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID);
                else
                    hdnRecordFilterExpression.Value = "";
            }

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;

            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }

            filterExpression += String.Format("GCServiceStatus IN ('{0}')", Constant.ServiceStatus.QUALITY_CONTROL);

            int count = BusinessLayer.GetLocationUserRowCount(string.Format("UserID = {0} AND IsDeleted = 0", AppSession.UserLogin.UserID));

            if (count > 0)
            {
                filterExpression += string.Format(" AND ToLocationID IN (SELECT LocationID FROM LocationUser WHERE UserID = {0} AND IsDeleted = 0)", AppSession.UserLogin.UserID);
            }
            else
            {
                count = BusinessLayer.GetLocationUserRoleRowCount(string.Format("RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID));
                if (count > 0)
                {
                    filterExpression += string.Format(" AND ToLocationID IN (SELECT LocationID FROM LocationUserRole WHERE RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0)", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID);
                    if (hdnLocationIDFrom.Value != null && hdnLocationIDFrom.Value.ToString() != "")
                    {
                        filterExpression += string.Format(" AND FromLocationID = {0}", hdnLocationIDFrom.Value);
                    }
                }
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvMDServiceRequestHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            List<vMDServiceRequestHd> lstEntity = BusinessLayer.GetvMDServiceRequestHdList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, "RequestID DESC");
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
                    MDServiceRequestHd reqHd = reqHdDao.Get(Convert.ToInt32(hdnID.Value));

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

                    string filterlstReqDt = string.Format("RequestID = {0} AND IsDeleted = 0", reqHd.RequestID);
                    List<MDServiceRequestDt> lstReqDt = BusinessLayer.GetMDServiceRequestDtList(filterlstReqDt);

                    if (reqHd.GCServiceStatus == Constant.ServiceStatus.QUALITY_CONTROL)
                    {
                        foreach (MDServiceRequestDt reqDt in lstReqDt)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            reqDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            reqDtDao.Update(reqDt);

                            if (!reqDt.IsConsumption)
                            {
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
                        }

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        reqHd.GCServiceStatus = Constant.ServiceStatus.ON_STORAGE;
                        reqHd.StorageBy = AppSession.UserLogin.UserID;
                        reqHd.StorageDate = DateTime.Now;
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

        #region MD Service Request Dt
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnID.Value != "")
            {
                filterExpression = string.Format("RequestID = {0} AND IsDeleted = 0", hdnID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvMDServiceRequestDtRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vMDServiceRequestDt> lstEntity = BusinessLayer.GetvMDServiceRequestDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "IsConsumption, ItemName1 ASC");
            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind();
        }

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}