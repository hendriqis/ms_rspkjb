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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ViewItemPackageDetail : BaseViewPopupCtl
    {
        private int ItemServiceID = 0;

        public override void InitializeDataControl(string param)
        {
            vItemService vItemService = BusinessLayer.GetvItemServiceList(String.Format("ItemID = {0} AND IsDeleted = 0", Convert.ToInt32(param))).FirstOrDefault();
            ItemServiceID = vItemService.ItemID;
            txtItemServiceName.Text = string.Format("{0} - {1}", vItemService.ItemCode, vItemService.ItemName1);
            txtItemServiceName2.Text = string.Format("{0} - {1}", vItemService.ItemCode, vItemService.ItemName1);
            txtItemServiceName3.Text = string.Format("{0} - {1}", vItemService.ItemCode, vItemService.ItemName1);

            BindGridView();
        }


        private void BindGridView()
        {
            List<vItemServiceDt> lstItemService = BusinessLayer.GetvItemServiceDtList(string.Format("ItemID = {0} AND IsDeleted = 0 AND GCItemType IN ('{1}','{2}','{3}') ORDER BY ItemID ASC", ItemServiceID, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM));
            grdView.DataSource = from i in lstItemService
                                 where i.GCItemType != Constant.ItemType.OBAT_OBATAN && i.GCItemType != Constant.ItemType.BARANG_MEDIS && i.GCItemType != Constant.ItemType.BARANG_UMUM && i.GCItemType != Constant.ItemType.BAHAN_MAKANAN
                                 select i;
            grdView.DataBind();
            grdViewObat.DataSource = from i in lstItemService
                                     where i.GCItemType == Constant.ItemType.OBAT_OBATAN || i.GCItemType == Constant.ItemType.BARANG_MEDIS
                                     select i;
            grdViewObat.DataBind();
            grdViewBarang.DataSource = from i in lstItemService
                                       where i.GCItemType == Constant.ItemType.BARANG_UMUM || i.GCItemType == Constant.ItemType.BAHAN_MAKANAN
                                       select i;
            grdViewBarang.DataBind();
        }


        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }

        }

        protected void grdViewObat_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }

        }

        protected void grdViewBarang_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
        }
    }
}