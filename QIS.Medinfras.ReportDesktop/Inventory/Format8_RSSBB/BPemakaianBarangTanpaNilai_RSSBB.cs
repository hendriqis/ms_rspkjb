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
    public partial class BPemakaianBarangTanpaNilai_RSSBB : BaseDailyPortraitRpt
    {
        public BPemakaianBarangTanpaNilai_RSSBB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vItemTransactionHd entity = BusinessLayer.GetvItemTransactionHdList(param[0])[0];
            lblTipePemakaian.Text = entity.ConsumptionType;
            lblUnit.Text = entity.HealthcareUnit;
            lblKeterangan.Text = entity.Remarks;
            if (entity.GCTransactionStatus != Constant.StandardCode.TransactionStatus.OPEN)
            {
                lblLastUpdatedBy.Text = String.Format("{0} {1}", entity.LastUpdatedByName, entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_FORMAT));
            }
            else
            {
                lblLastUpdatedBy.Text = "";
            }
            base.InitializeReport(param);
        }
    }
}
