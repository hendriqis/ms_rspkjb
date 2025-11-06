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
    public partial class LRekapPenerimaanBarang : BaseDailyLandscapeRpt
    {
        public LRekapPenerimaanBarang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriode.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            
            ItemMaster entity = BusinessLayer.GetItemMaster(Convert.ToInt32(param[1]));
            lblItem.Text = String.Format("Item : {0} {1}",entity.ItemCode, entity.ItemName1);

            if (param[2] != "")
            {
                vSupplier entitySup = BusinessLayer.GetvSupplierList(String.Format("BusinessPartnerID = {0}", param[2]))[0];
                lblSupplier.Text = String.Format("Supplier : {0}",entitySup.BusinessPartnerName);
            }
            else 
            {
                lblSupplier.Text = String.Format("Supplier : Semua");
            }
            
            base.InitializeReport(param);
        }
    }
}
