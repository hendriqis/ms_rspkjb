using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPenggantianReturPembelian : BaseDailyPortraitRpt
    {
        public BPenggantianReturPembelian()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseReplacementHd entityHd = BusinessLayer.GetvPurchaseReplacementHdList(param[0])[0];
            lblExchangeNo.Text = entityHd.PurchaseReplacementNo;
            lblExchangeDate.Text = entityHd.ReplacementDateInString;
            lblReturnNo.Text = entityHd.PurchaseReturnNo;
            lblSupplier.Text = String.Format("{0} - {1}", entityHd.BusinessPartnerCode, entityHd.BusinessPartnerName);
            lblWarehouseCode.Text = String.Format("{0} - {1}",entityHd.LocationCode,entityHd.LocationName);
            
            lblCreatedByName.Text = entityHd.CreatedByName;

            string filterExpression = string.Format(" ParameterCode IN ('{0}')", "IM0009");
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;
            base.InitializeReport(param);
        }
    }
}
