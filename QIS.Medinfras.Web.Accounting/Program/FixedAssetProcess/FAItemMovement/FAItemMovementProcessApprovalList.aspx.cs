using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class FAItemMovementProcessApprovalList : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocationFrom = "";
        protected string filterExpressionLocationTo = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.FA_APPROVAL_COMBINED_ITEM_MOVEMENT;
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

            BindGridView(1, true, ref PageCount);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnFALocationIDFrom, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(lblFALocationFrom, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtFALocationCodeFrom, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFALocationNameFrom, new ControlEntrySetting(false, true, false));
            SetControlEntrySetting(hdnFALocationIDTo, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(lblFALocationTo, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtFALocationCodeTo, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtFALocationNameTo, new ControlEntrySetting(false, false, true));
        }

        protected string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;

            filterExpression = string.Format("GCTransactionStatus = '{0}'", Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            
            if (txtFALocationCodeFrom.Text != "" || txtFALocationCodeTo.Text != "")
            {
                if (txtFALocationCodeFrom.Text != "")
                    filterExpression += string.Format(" AND FromFALocationID = {0}", hdnFALocationIDFrom.Value);
                if (txtFALocationCodeTo.Text != "")
                    filterExpression += string.Format(" AND ToFALocationID = {0}", hdnFALocationIDTo.Value);
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFAItemMovementHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFAItemMovementHd> lstEntity = BusinessLayer.GetvFAItemMovementHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "MovementNo DESC");
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FAItemDao faItemDao = new FAItemDao(ctx);
            FAItemMovementHdDao itemMovementHdDao = new FAItemMovementHdDao(ctx);
            FAItemMovementDtDao itemMovementDtDao = new FAItemMovementDtDao(ctx);
            FAItemMovementDao movementDao = new FAItemMovementDao(ctx);

            if (type == "approve")
            {
                try
                {
                    string filterExpressionItemMovementHd = String.Format("MovementID IN ({0})", hdnParam.Value);
                    List<FAItemMovementHd> lstItemMovementHd = BusinessLayer.GetFAItemMovementHdList(filterExpressionItemMovementHd, ctx);
                    foreach (FAItemMovementHd itemMovementHd in lstItemMovementHd)
                    {
                        string filterExpressionItemMovementDt = String.Format("MovementID IN ({0}) AND IsDeleted = 0", itemMovementHd.MovementID);
                        List<FAItemMovementDt> lstItemMovementDt = BusinessLayer.GetFAItemMovementDtList(filterExpressionItemMovementDt, ctx);
                        foreach (FAItemMovementDt itemMovementDt in lstItemMovementDt)
                        {
                            itemMovementDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            itemMovementDtDao.Update(itemMovementDt);

                            FAItem faItem = faItemDao.Get(itemMovementDt.FixedAssetID);
                            faItem.DepreciationStartDate = DateTime.Now;
                            faItem.LastUpdatedBy = AppSession.UserLogin.UserID;
                            faItem.FALocationID = itemMovementHd.ToFALocationID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            faItemDao.Update(faItem);

                            FAItemMovement movement = new FAItemMovement();
                            movement.FixedAssetID = itemMovementDt.FixedAssetID;
                            movement.FromFALocationID = itemMovementHd.FromFALocationID;
                            movement.ToFALocationID = itemMovementHd.ToFALocationID;
                            movement.MovementDate = itemMovementHd.MovementDate;
                            movement.MovementNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.FIXED_ASSET_ITEM_MOVEMENT, movement.MovementDate, ctx);
                            movement.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                            movement.CreatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            movementDao.Insert(movement);
                        }

                        itemMovementHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        itemMovementHd.ApprovedBy = AppSession.UserLogin.UserID;
                        itemMovementHd.ApprovedDate = DateTime.Now;
                        itemMovementHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        itemMovementHdDao.Update(itemMovementHd);
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
            }
            else if (type == "decline")
            {
                try
                {
                    string[] listParam = hdnParam.Value.Split(',');
                    foreach (string param in listParam)
                    {
                        int MovementID = Convert.ToInt32(param);
                        FAItemMovementHd entity = itemMovementHdDao.Get(MovementID);

                        string filterExpressionItemMovementDt = String.Format("MovementID IN ({0}) AND IsDeleted = 0", entity.MovementID);
                        List<FAItemMovementDt> lstItemMovementDt = BusinessLayer.GetFAItemMovementDtList(filterExpressionItemMovementDt, ctx);
                        foreach (FAItemMovementDt itemMovementDt in lstItemMovementDt)
                        {
                            itemMovementDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            itemMovementDtDao.Update(itemMovementDt);
                        }

                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        itemMovementHdDao.Update(entity);
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
            }
            return result;
        }
    }
}