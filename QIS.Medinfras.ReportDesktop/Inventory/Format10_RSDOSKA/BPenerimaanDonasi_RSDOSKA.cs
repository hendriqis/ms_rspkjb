using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.ReportDesktop;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Data.Service.Helper;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPenerimaanDonasi_RSDOSKA : BaseDailyPortraitRpt
    {
        public BPenerimaanDonasi_RSDOSKA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);
            vPurchaseReceiveHd entityHd = BusinessLayer.GetvPurchaseReceiveHdList(param[0])[0];
            lblCreatedByName.Text = entityHd.CreatedByName;

            string filterExpression = string.Format(" ParameterCode IN ('{0}')", "SA0020");
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "SA0020").FirstOrDefault().ParameterValue;

            lblPurchaseReceiveNo.Text = entityHd.PurchaseReceiveNo;
            lblPurchaseReceiveDate.Text = entityHd.ReceivedDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblSupplierCode.Text = String.Format("{0} - {1}", entityHd.SupplierCode, entityHd.SupplierName);
        }
            
    }
}
