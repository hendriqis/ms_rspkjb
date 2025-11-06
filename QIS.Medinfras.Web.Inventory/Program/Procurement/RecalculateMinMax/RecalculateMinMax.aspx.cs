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
    public partial class RecalculateMinMax : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";
        private string[] lstSelectedMember = null;
        private string[] lstQtyMinStock = null;
        private string[] lstQtyMaxStock = null;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.RECALCULATE_MIN_MAX;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = false;
        }

        protected override void InitializeDataControl()
        {
            filterExpressionLocation = string.Format("{0};{1};{2};{3}", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.ITEM_REQUEST, " IsMinMaxReadonly = 1");

            GetSettingParameter();
            BindGridView();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void GetSettingParameter()
        {
            string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}', '{3}','{4}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_FAKTOR_X_ROP_MIN, Constant.SettingParameter.IM_FAKTOR_X_ROP_MAX, Constant.SettingParameter.IM_PERCENTAGE_MAXIMUM_STOCK_FORMULA, Constant.SettingParameter.IM0134);
            List<SettingParameterDt> lstParameter = BusinessLayer.GetSettingParameterDtList(filterExpression);
            hdnFactorXMin.Value = lstParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_FAKTOR_X_ROP_MIN).FirstOrDefault().ParameterValue;
            hdnFactorXMax.Value = lstParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_FAKTOR_X_ROP_MAX).FirstOrDefault().ParameterValue;
            hdnPercentageForMaximumStock.Value = lstParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_PERCENTAGE_MAXIMUM_STOCK_FORMULA).FirstOrDefault().ParameterValue;
            hdnIM0134.Value = lstParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM0134).FirstOrDefault().ParameterValue;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnLocationIDFrom, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true));
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vItemBalance1 entity = e.Item.DataItem as vItemBalance1;
                TextBox txtAverageQty = e.Item.FindControl("txtAverageQty") as TextBox;
                TextBox txtCurrentMinStock = e.Item.FindControl("txtCurrentMinStock") as TextBox;
                TextBox txtCurrentMaxStock = e.Item.FindControl("txtCurrentMaxStock") as TextBox;
                TextBox txtSystemMinStock = e.Item.FindControl("txtSystemMinStock") as TextBox;
                TextBox txtSystemMaxStock = e.Item.FindControl("txtSystemMaxStock") as TextBox;
                TextBox txtMinStock = e.Item.FindControl("txtMinStock") as TextBox;
                TextBox txtMaxStock = e.Item.FindControl("txtMaxStock") as TextBox;
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsSelected");
                Decimal averageQty = entity.CustomAverageQty;
                Decimal percentageStock = Convert.ToDecimal(hdnPercentageForMaximumStock.Value);                
                
                Decimal minStock = (averageQty * entity.LeadTime * Convert.ToDecimal(hdnFactorXMin.Value));
                Decimal maxStock = (percentageStock * averageQty * entity.LeadTime * Convert.ToDecimal(hdnFactorXMax.Value)) / (100);

                if (hdnIM0134.Value == "1")
                {
                    minStock = (entity.CustomAverageQtyOut * entity.LeadTime * Convert.ToDecimal(hdnFactorXMin.Value));
                    maxStock = (entity.CustomAverageQtyOut * entity.LeadTime * Convert.ToDecimal(hdnFactorXMax.Value));
                }

                minStock = Math.Ceiling(minStock);
                maxStock = Math.Ceiling(maxStock);
                minStock = minStock < 0 ? 0 : minStock;
                maxStock = maxStock < 0 ? 0 : maxStock;
                txtAverageQty.Text = averageQty.ToString("N");
                txtCurrentMinStock.Text = entity.QuantityMIN.ToString("N");
                txtCurrentMaxStock.Text = entity.QuantityMAX.ToString("N");
                txtSystemMinStock.Text = minStock.ToString("N");
                txtSystemMaxStock.Text = maxStock.ToString("N");
                txtMinStock.Text = minStock.ToString("N");
                txtMaxStock.Text = maxStock.ToString("N");
                if (lstSelectedMember.Contains(entity.ID.ToString()))
                {
                    int idx = Array.IndexOf(lstSelectedMember, entity.ID.ToString());
                    chkIsSelected.Checked = true;
                    txtMinStock.ReadOnly = false;
                    txtMinStock.Text = lstQtyMinStock[idx];
                    txtMaxStock.ReadOnly = false;
                    txtMaxStock.Text = lstQtyMaxStock[idx];
                }
            }
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";

            if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression = hdnFilterExpressionQuickSearch.Value;
            }
            
            if (hdnLocationIDFrom.Value != "")
            {
                if (filterExpression != "1 = 0")
                {
                    filterExpression += string.Format(" AND LocationID = {0} AND IsDeleted = 0", hdnLocationIDFrom.Value);
                }
                else
                {
                    filterExpression = string.Format("LocationID = {0} AND IsDeleted = 0", hdnLocationIDFrom.Value);
                }
            }

            if (rblItemType.SelectedValue != "0")
            {
                switch (rblItemType.SelectedValue)
                {
                    case "2":
                        filterExpression += string.Format(" AND GCItemType = '{0}'", Constant.ItemType.OBAT_OBATAN);
                        break;
                    case "3":
                        filterExpression += string.Format(" AND GCItemType = '{0}'", Constant.ItemType.BARANG_MEDIS);
                        break;
                    case "8":
                        filterExpression += string.Format(" AND GCItemType = '{0}'", Constant.ItemType.BARANG_UMUM);
                        break;
                    default:
                        break;
                }
            }

            if (chkIsConsignmentItem.Checked)
                filterExpression += " AND IsConsigmentItem = 1";             
            else
                filterExpression += " AND IsConsigmentItem = 0";

            lstSelectedMember = hdnSelectedMember.Value.Split('|');
            lstQtyMinStock = hdnMinStock.Value.Split('|');
            lstQtyMaxStock = hdnMaxStock.Value.Split('|');

            List<vItemBalance1> lstEntity = BusinessLayer.GetvItemBalance1List(filterExpression, int.MaxValue, 1, "ItemName1 ASC");
            if (rblDisplay.SelectedValue.ToLower().Equals("1"))
            {
                lstEntity = lstEntity.Where(t => t.CustomAverageQty > 0).ToList();
            }
            else if (rblDisplay.SelectedValue.ToLower().Equals("2"))
            {
                lstEntity = lstEntity.Where(t => t.CustomAverageQty == 0).ToList();
            }

            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            String[] paramID = hdnSelectedMember.Value.Substring(1).Split('|');
            String[] paramAverageQty = hdnAverageQty.Value.Substring(1).Split('|');
            String[] paramSystemMinStock = hdnSystemMinStock.Value.Substring(1).Split('|');
            String[] paramSystemMaxStock = hdnSystemMaxStock.Value.Substring(1).Split('|');
            String[] paramMinStock = hdnMinStock.Value.Substring(1).Split('|');
            String[] paramMaxStock = hdnMaxStock.Value.Substring(1).Split('|');
            IDbContext ctx = DbFactory.Configure(true);
            ItemBalanceDao entityItemBalanceDao = new ItemBalanceDao(ctx);
            ItemMinMaxLogDao entityLogDao = new ItemMinMaxLogDao(ctx);
            try
            {
                for (int ct = 0; ct < paramID.Length; ct++)
                {
                    ItemBalance entityItemBalance = BusinessLayer.GetItemBalanceList(string.Format("ID = {0}", paramID[ct]),ctx)[0];
                    decimal oldMin = entityItemBalance.QuantityMIN;
                    decimal oldMax = entityItemBalance.QuantityMAX;

                    entityItemBalance.QuantityMIN = Convert.ToDecimal(paramMinStock[ct]);
                    entityItemBalance.QuantityMAX = Convert.ToDecimal(paramMaxStock[ct]);
                    entityItemBalance.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityItemBalanceDao.Update(entityItemBalance);

                    //Insert to Log
                    ItemMinMaxLog entityLog = new ItemMinMaxLog();
                    entityLog.LocationID = entityItemBalance.LocationID;
                    entityLog.ItemID = entityItemBalance.ItemID;
                    entityLog.AverageQuantity = Convert.ToDecimal(paramAverageQty[ct]);
                    entityLog.OldQuantityMIN = oldMin;
                    entityLog.OldQuantityMAX = oldMax;
                    entityLog.SystemQuantityMIN = Convert.ToDecimal(paramSystemMinStock[ct]);
                    entityLog.SystemQuantityMAX = Convert.ToDecimal(paramSystemMaxStock[ct]);
                    entityLog.NewQuantityMIN = Convert.ToDecimal(paramMinStock[ct]);
                    entityLog.NewQuantityMAX = Convert.ToDecimal(paramMaxStock[ct]);
                    entityLog.CreatedBy = AppSession.UserLogin.UserID;
                    entityLogDao.Insert(entityLog);
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