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

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class FAItemMovementEntry : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.FA_ITEM_MOVEMENT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnFixedAssetID.Value = AppSession.FixedAssetID.ToString();

            txtMovementDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowProposed = IsAllowNextPrev = false;
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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount) 
        {
            string filterExpression = String.Format("GCTransactionStatus NOT IN ('{0}','{1}') AND FixedAssetID = {2}",
                                                        Constant.TransactionStatus.VOID, Constant.TransactionStatus.CLOSED, hdnFixedAssetID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFAItemMovementRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFAItemMovement> lstEntity = BusinessLayer.GetvFAItemMovementList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex,"MovementNo DESC");
            if (lstEntity.Count > 0 && pageIndex == 1) lstEntity[0].IsEditable = true;
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpViewProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                if (e.Parameter == "process")
                {
                    if(hdnIsAdd.Value == "1")
                        OnSaveAddRecord(ref result);
                    else
                        OnSaveEditRecord(ref result);
                }
                else // delete
                {
                    OnSaveDeleteRecord(ref result);
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(FAItemMovement entity) 
        {
            entity.FixedAssetID = Convert.ToInt32(hdnFixedAssetID.Value);
            entity.ToFALocationID = Convert.ToInt32(hdnToLocationID.Value);
            entity.MovementDate = Helper.GetDatePickerValue(txtMovementDate.Text);
            entity.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
            entity.Remarks = txtRemarks.Text;
        }

        private bool OnSaveAddRecord(ref string errMessage) 
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FAItemMovementDao faItemMovementDao = new FAItemMovementDao(ctx);
            FAItemDao faItemDao = new FAItemDao(ctx);
            
            try
            {
                FAItem faItem = faItemDao.Get(Convert.ToInt32(hdnFixedAssetID.Value));
                FAItemMovement faItemMovement = new FAItemMovement();
                
                faItemMovement.FromFALocationID = faItem.FALocationID;
                ControlToEntity(faItemMovement);
                faItemMovement.MovementNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.FIXED_ASSET_ITEM_MOVEMENT, faItemMovement.MovementDate, ctx);
                ctx.CommandType = System.Data.CommandType.Text;
                ctx.Command.Parameters.Clear();
                faItemMovement.CreatedBy = AppSession.UserLogin.UserID;
                faItemMovementDao.Insert(faItemMovement);

                faItem.FALocationID = faItemMovement.ToFALocationID;
                faItem.LastUpdatedBy = AppSession.UserLogin.UserID;
                faItemDao.Update(faItem);
                

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

        private bool OnSaveEditRecord(ref string errMessage) 
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FAItemMovementDao faItemMovementDao = new FAItemMovementDao(ctx);
            FAItemDao faItemDao = new FAItemDao(ctx);

            try
            {
                string filterExpression = String.Format("MovementID = {0}", hdnMovementID.Value);
                FAItemMovement faItemMovement = BusinessLayer.GetFAItemMovementList(filterExpression, ctx)[0];
                FAItem faItem = faItemDao.Get(faItemMovement.FixedAssetID);

                ControlToEntity(faItemMovement);
                faItemMovement.LastUpdatedBy = AppSession.UserLogin.UserID;

                faItem.FALocationID = faItemMovement.ToFALocationID;
                faItem.LastUpdatedBy = AppSession.UserLogin.UserID;

                faItemDao.Update(faItem);
                faItemMovementDao.Update(faItemMovement);
                
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

        private bool OnSaveDeleteRecord(ref string errMessage) 
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FAItemMovementDao faItemMovementDao = new FAItemMovementDao(ctx);
            FAItemDao faItemDao = new FAItemDao(ctx);
            try
            {
                string filterExpression = String.Format("MovementID = {0}", hdnMovementID.Value);
                FAItemMovement faItemMovement = BusinessLayer.GetFAItemMovementList(filterExpression, ctx)[0];
                faItemMovement.GCTransactionStatus = Constant.TransactionStatus.VOID;
                faItemMovement.LastUpdatedBy = AppSession.UserLogin.UserID;

                FAItem faItem = faItemDao.Get(AppSession.FixedAssetID);
                faItem.FALocationID = faItemMovement.FromFALocationID;
                faItem.LastUpdatedBy = AppSession.UserLogin.UserID;

                faItemDao.Update(faItem);
                faItemMovementDao.Update(faItemMovement);
                
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