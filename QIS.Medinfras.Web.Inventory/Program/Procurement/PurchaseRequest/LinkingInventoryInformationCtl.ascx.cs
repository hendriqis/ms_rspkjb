using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class LinkingInventoryInformationCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override void InitializeDataControl(string param)
        {
            txtValueDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtValueDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<StandardCode> lstItemType = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID IN ('{0}','{1}','{2}','{3}')", Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN));
            lstItemType.Insert(0, new StandardCode() { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboItemType, lstItemType, "StandardCodeName", "StandardCodeID");

            BindGridView(1, true, ref PageCount);
        }

        #region Linking Inventory
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string[] tempFrom = txtValueDateFrom.Text.Split('-');
            string[] tempTo = txtValueDateTo.Text.Split('-');

            string dateFrom = string.Format("{0}{1}{2}", tempFrom[2], tempFrom[1], tempFrom[0]);
            string dateTo = string.Format("{0}{1}{2}", tempTo[2], tempTo[1], tempTo[0]);
            string date = string.Format("{0};{1}", dateFrom, dateTo);
            Int32 ItemID = 0;

            string ItemType = Convert.ToString(cboItemType.Value);

            if (hdnItemID.Value != null && hdnItemID.Value != "")
            {
                ItemID = Convert.ToInt32(hdnItemID.Value);
            }

            List<GetPurchaseRequestOrderReceivePerRequestDate> lstEntity = BusinessLayer.GetPurchaseRequestOrderReceivePerRequestDateList(date, ItemType, ItemID);

            grdPopupView.DataSource = lstEntity;
            grdPopupView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(1, true, ref pageCount);
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
    }
        #endregion
}
