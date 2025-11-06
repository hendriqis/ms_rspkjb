using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LObatDeadStockRSSES : BaseCustomDailyLandscapeRpt
    {
        List<GetMedicineDeadStockRSSES> lstEntityMedicine;
        public LObatDeadStockRSSES()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lstEntityMedicine = BusinessLayer.GetMedicineDeadStockRSSESList(param[0], param[1], param[2],param[3]);
           
            base.InitializeReport(param);
        }
        private void GroupFooter1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            decimal totalItem = 0;
            String LocationName = GetCurrentColumnValue("LocationName").ToString();
            String GCItemType = GetCurrentColumnValue("GCItemType").ToString();
            decimal totalItemDS = lstEntityMedicine.Count(a => a.LocationName == LocationName && a.GCItemType == GCItemType);
            if(GCItemType == Constant.ItemType.OBAT_OBATAN)
            {
                totalItem = lstEntityMedicine.Where(a => a.LocationName == LocationName).FirstOrDefault().TotalObat;
            }
            else
            {
                totalItem = lstEntityMedicine.Where(a => a.LocationName == LocationName).FirstOrDefault().TotalAlkes;
            }
             decimal cftotal = 0;
             cftotal = (totalItemDS / totalItem) * 1;
             lblDSPerLokasi.Text = string.Format("{0}", cftotal.ToString("P"));
             lblAllItem.Text = string.Format("{0}", totalItem.ToString("#.#"));
        }
    }
}
