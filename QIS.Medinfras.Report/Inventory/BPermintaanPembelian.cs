using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Text;
using System.Reflection;

namespace QIS.Medinfras.Report
{
    public partial class BPermintaanPembelian : BaseDailyPortraitRpt
    {
        public BPermintaanPembelian()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseRequestHd entity = BusinessLayer.GetvPurchaseRequestHdList(param[0])[0];
            lblPurchaseRequestNo.Text = entity.PurchaseRequestNo;
            lblLocation.Text = string.Format("{0} ({1})", entity.LocationName, entity.LocationCode);
            //lblCreatedBy.Text = entity.CreatedByName;
            //lblApprovedByName.Text = entity.ApprovedByName;
            base.InitializeReport(param);
        }

        private void lblGroupSubTotal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblGroupSubTotal.Text = string.Format("Sub Total per Supplier {0}", this.lblGroupSubTotal.Text);
        }
    }
}
