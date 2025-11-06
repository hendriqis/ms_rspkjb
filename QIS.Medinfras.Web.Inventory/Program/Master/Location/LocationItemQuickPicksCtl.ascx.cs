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
    public partial class LocationItemQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private PurchaseRequest DetailPage
        {
            get { return (PurchaseRequest)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] temp = param.Split('|');
            //hdnTransactionID.Value = temp[0];
            hdnLocationIDCtl.Value = temp[0];
            //hdnLocationItemGroupID.Value = temp[1];
            hdnGCLocationGroupCtl.Value = temp[2];

            string filterExpression = string.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.TRANSACTION_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboTransactionTypeCtl, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboTransactionTypeCtl.SelectedIndex = 0;

            SettingParameterDt setvarDt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM0122);
            string[] paramValueSpit = setvarDt.ParameterValue.Split('|');

            hdnDefaultValueMin.Value = paramValueSpit[0];
            hdnDefaultValueMax.Value = paramValueSpit[1];

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

            if (hdnItemGroupDrugLogisticID.Value == "")
                filterExpression += string.Format("LocationID = '{0}' AND ItemName1 LIKE '%{1}%' AND IsDeleted = 0", hdnLocationIDCtl.Value, hdnFilterItem.Value);
            else
                filterExpression += string.Format("LocationID = '{0}' AND ItemName1 LIKE '%{1}%' AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath LIKE '%/{2}/%') AND IsDeleted = 0", hdnLocationIDCtl.Value, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value);

            return filterExpression;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vItemMaster entity = e.Row.DataItem as vItemMaster;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.ItemID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";
            if (hdnLocationIDCtl.Value != "0" && hdnLocationIDCtl.Value != "")
            {
                filterExpression += string.Format("ItemID NOT IN (SELECT ItemID FROM ItemBalance WHERE LocationID = {0} AND IsDeleted = 0)", hdnLocationIDCtl.Value);


                if (hdnGCLocationGroupCtl.Value == Constant.LocationGroup.DRUG_AND_MEDICAL_SUPPLIES) {
                    filterExpression += string.Format(" AND ItemName1 LIKE '%{0}%' AND GCItemType IN ('{1}', '{2}')",
                                            hdnFilterItem.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS);                
                }
                else if (hdnGCLocationGroupCtl.Value == Constant.LocationGroup.LOGISTIC)
                {
                    filterExpression += string.Format(" AND ItemName1 LIKE '%{0}%' AND GCItemType = '{1}'",
                                            hdnFilterItem.Value, Constant.ItemType.BARANG_UMUM);
                }
                else if (hdnGCLocationGroupCtl.Value == "" || hdnGCLocationGroupCtl.Value == null)
                {
                    filterExpression += string.Format(" AND ItemName1 LIKE '%{0}%' AND GCItemType IN ('{1}','{2}','{3}','{4}')",
                                            hdnFilterItem.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN);
                }

                if (hdnItemGroupDrugLogisticID.Value != "" && hdnItemGroupDrugLogisticID.Value != "0")
                {
                    filterExpression += String.Format(" AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath like '%{0}%')", hdnItemGroupDrugLogisticID.Value);
                }

                filterExpression += " AND IsDeleted = 0";
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemMasterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }
            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vItemMaster> lstEntity = BusinessLayer.GetvItemMasterList(filterExpression, 10, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemBalanceDao entityDao = new ItemBalanceDao(ctx);
            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                //string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                string[] lstSelectedMemberMin = hdnQuantityMin.Value.Split(',');
                string[] lstSelectedMemberMax = hdnQuantityMax.Value.Split(','); 

                int ct = 0;
                foreach (String itemID in lstSelectedMember)
                {
                    ItemBalance entity = new ItemBalance();
                    entity.ItemID = Convert.ToInt32(itemID);
                    entity.QuantityMIN = Convert.ToDecimal(lstSelectedMemberMin[ct]);
                    entity.QuantityMAX = Convert.ToDecimal(lstSelectedMemberMax[ct]);
                    entity.LocationID = Convert.ToInt32(hdnLocationIDCtl.Value);
                    entity.GCItemRequestType = cboTransactionTypeCtl.Value.ToString();
                    if (hdnBinLocationIDCtl.Value != "" && hdnBinLocationIDCtl.Value != "0")
                    {
                        entity.BinLocationID = Convert.ToInt32(hdnBinLocationIDCtl.Value);
                    }
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entityDao.Insert(entity);
                    ct++;
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