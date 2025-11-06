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
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class LocationItemList : BasePageTrx
    {
        protected string filterExpressionItemProduct = "";
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.LOCATION_ITEM;
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            filterExpressionItemProduct = string.Format("GCItemType IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC, Constant.ItemGroupMaster.NUTRITION);

            string filterExpression = string.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.TRANSACTION_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboTransactionType, lstStandardCode, "StandardCodeName", "StandardCodeID");

            SettingParameterDt setvarDt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM0125);
            hdnIsAllowEditMinMax.Value = setvarDt.ParameterValue;

            BindGridView(1, true, ref PageCount);
        }

        protected string OnGetLocationFilterExpression()
        {
            return string.Format("{0};{1};;", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
        }

        protected string OnGetItemGroupFilterExpression()
        {
            return string.Format("GCItemType IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC, Constant.ItemGroupMaster.NUTRITION);
        }

        protected string OnGetItemProductFilterExpression()
        {
            return string.Format("GCItemType IN ('{1}','{2}','{3}','{4}') AND IsDeleted = 0", hdnLocationID.Value, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.LOGISTIC, Constant.ItemGroupMaster.NUTRITION);
        }   

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";
            if (hdnLocationID.Value != null && hdnLocationID.Value.ToString() != "")
            {
                filterExpression = String.Format("LocationID = {0}", hdnLocationID.Value);

                if (hdnItemGroupID.Value != null && hdnItemGroupID.Value.ToString() != "")
                {
                    //filterExpression += String.Format("AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath like '%/{0}/%') ", hdnItemGroupID.Value);
                    filterExpression += String.Format(" AND DisplayPath like '%/{0}/%' ", hdnItemGroupID.Value);
                }
                if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
                {
                    filterExpression += String.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
                }
                filterExpression += " AND IsDeleted = 0";
            }
            else
                filterExpression = "1 = 0";

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemBalance3RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vItemBalance3> lstEntity = BusinessLayer.GetvItemBalance3List(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "ItemName1");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vItemBalance3 entity = e.Item.DataItem as vItemBalance3;
                HtmlGenericControl lblExpiredDate = e.Item.FindControl("lblExpiredDate") as HtmlGenericControl;

                if (!entity.IsControlExpired)
                    lblExpiredDate.Attributes.Add("class", "lblDisabled");
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            int pageCount = 1;

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = string.Format("refresh|{0}", pageCount);
            }
            else
            {
                if (param[0] == "save")
                {
                    if (hdnID.Value.ToString() != "")
                    {
                        if (OnSaveEditRecord(ref errMessage))
                            result += "success";
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
                    else
                    {
                        if (OnSaveAddRecord(ref errMessage))
                            result += "success";
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
                }
                else if (param[0] == "delete")
                {
                    if (OnDeleteRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }

                BindGridView(1, true, ref pageCount);
                result += "|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(ItemBalance entity)
        {
            entity.ItemID = Convert.ToInt32(hdnItemID.Value);

            if (hdnIsAllowEditMinMax.Value == "1")
            {
                entity.QuantityMIN = Convert.ToDecimal(txtMinimum.Text);
                entity.QuantityMAX = Convert.ToDecimal(txtMaximum.Text);
            }

            if (string.IsNullOrEmpty(hdnBinLocationID.Value) || hdnBinLocationID.Value == "0")
            {
                entity.BinLocationID = null;
            }
            else
            {
                entity.BinLocationID = Convert.ToInt32(hdnBinLocationID.Value);
            }

            entity.Remarks = txtRemarks.Text;

            entity.GCItemRequestType = cboTransactionType.Value.ToString();
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemBalanceDao entityDao = new ItemBalanceDao(ctx);

            try
            {
                ItemBalance entity = new ItemBalance();
                ControlToEntity(entity);
                entity.LocationID = Convert.ToInt32(hdnLocationID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);

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
            ItemBalanceDao entityDao = new ItemBalanceDao(ctx);

            try
            {
                ItemBalance entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

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

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemBalanceDao entityDao = new ItemBalanceDao(ctx);

            try
            {
                ItemBalance entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                if (entity.QuantityEND <= 0)
                {
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Item yang masih memiliki Balance Ending (stok di lokasi ini) tidak bisa dihapus.";
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