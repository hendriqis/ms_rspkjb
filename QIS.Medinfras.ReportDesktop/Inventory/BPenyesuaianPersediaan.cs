using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPenyesuaianPersediaan : BaseDailyPortraitRpt
    {
        public BPenyesuaianPersediaan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            ItemTransactionHd entityHD = BusinessLayer.GetItemTransactionHdList(param[0])[0];
            lblKeterangan.Text = entityHD.Remarks;

            base.InitializeReport(param);
        }

    }
}
