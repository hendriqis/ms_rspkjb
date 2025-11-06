using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LDaftarPemeriksaanPasienRadiologi : BaseCustomDailyLandscapeA3Rpt
    {
        public LDaftarPemeriksaanPasienRadiologi()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string fromItemID = param[2];
            string toItemID = param[3];

            if (fromItemID != "")
            {
                ItemMaster entItemFrom = BusinessLayer.GetItemMasterList(string.Format("ItemID = {0}", fromItemID))[0];
                lblFromItem.Text = string.Format("Dari item : {0}", entItemFrom.ItemName1);
            }
            else
            {
                lblFromItem.Text = string.Format("Dari item :");
            }

            if (toItemID != "") 
            {
                ItemMaster entItemTo = BusinessLayer.GetItemMasterList(string.Format("ItemID = {0}", toItemID))[0];
                lblToItem.Text = string.Format("Ke item : {0}", entItemTo.ItemName1);
            }
            else 
            {
                lblToItem.Text = string.Format("Ke item :");
            }

            base.InitializeReport(param);
        }

    }
}
