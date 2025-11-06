using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Data.Service.Helper;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BDistribusiBarangRSFM : BaseDailyPortrait2Rpt
    {
        public BDistribusiBarangRSFM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param) 
        {
            vItemDistributionHd entityHd = BusinessLayer.GetvItemDistributionHdList(param[0])[0];
            lblDistributionNo.Text = entityHd.DistributionNo;
            lblTransactionDate.Text = entityHd.TransactionDateTimeInString;
            lblDistributionDate.Text = entityHd.DeliveryDateTimeInString;
            lblWarehouseCode.Text = String.Format("{0} - {1}", entityHd.FromLocationCode, entityHd.FromLocationName);
            lblOtherWarehouseCode.Text = String.Format("{0} - {1}", entityHd.ToLocationCode, entityHd.ToLocationName);
            if (entityHd.DeliveryDate.ToString(Constant.FormatString.DATE_TIME_FORMAT) != "01-Jan-1900 00:00:00")
            {
                lblCreatedByName.Text = entityHd.DeliveredByName;
            }
            else
            {
                lblCreatedByName.Text = entityHd.CreatedByName;
            }

            if (entityHd.ReceivedDate.ToString(Constant.FormatString.DATE_TIME_FORMAT) != "01-Jan-1900 00:00:00")
            {
                lblApprovedByName.Text = entityHd.ReceivedByName;
            }
            else
            {
                lblApprovedByName.Text = "";
            }

            base.InitializeReport(param);
        }
    }
}
