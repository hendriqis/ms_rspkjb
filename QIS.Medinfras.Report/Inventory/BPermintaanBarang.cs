using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Report
{
    public partial class BPermintaanBarang : BaseDailyPortraitRpt
    {
        public BPermintaanBarang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vItemRequestHd entity = BusinessLayer.GetvItemRequestHdList(param[0])[0];
            lblRequestNo.Text = entity.ItemRequestNo;
            lblRequestDate.Text = entity.TransactionDateInString;
            lblLocationCodeFrom.Text = entity.FromLocationCode;
            lblLocationNameFrom.Text = entity.FromLocationName;
            lblLocationCodeTo.Text = entity.ToLocationCode;
            lblLocationNameTo.Text = entity.ToLocationName;
            base.InitializeReport(param);
        }

    }
}
