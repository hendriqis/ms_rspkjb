using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPersediaanBarangPerGudangPerLokasi : BaseDailyPortraitRpt
    {
        public LPersediaanBarangPerGudangPerLokasi()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblPeriod.Text = string.Format("Tanggal : {0}", Helper.YYYYMMDDToDate(param[0]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

        private void lblGCItemTypeItemGroup_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String ItemGroupID = GetCurrentColumnValue("ItemGroupID").ToString();
            String ItemGroupName1 = GetCurrentColumnValue("ItemGroupName1").ToString();
            String ItemType = GetCurrentColumnValue("ItemType").ToString();

            if (ItemGroupID == "0")
            {
                lblGCItemTypeItemGroup.Text = ItemType;
            }
            else
            {
                lblGCItemTypeItemGroup.Text = ItemType + " | " + ItemGroupName1;
            }
        }

        private void cSubTotalGCItemTypeItemGroup_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String ItemGroupID = GetCurrentColumnValue("ItemGroupID").ToString();
            String ItemGroupName1 = GetCurrentColumnValue("ItemGroupName1").ToString();
            String ItemType = GetCurrentColumnValue("ItemType").ToString();

            if (ItemGroupID == "0")
            {
                cSubTotalGCItemTypeItemGroup.Text = ItemType;
            }
            else
            {
                cSubTotalGCItemTypeItemGroup.Text = ItemType + " | " + ItemGroupName1;
            }
        }

    }
}
