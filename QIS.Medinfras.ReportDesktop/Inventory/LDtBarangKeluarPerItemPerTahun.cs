using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
//using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LDtBarangKeluarPerItemPerTahun : BaseCustomDailyLandscapeA3Rpt
    {
        public LDtBarangKeluarPerItemPerTahun()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {           
            string FromLocationID = param[0].ToString();
            string ToLocationID = param[1].ToString();

            if (!String.IsNullOrEmpty(ToLocationID))
            {
                List<Location> lstLocation = BusinessLayer.GetLocationList(string.Format("LocationID IN ({0},{1})", FromLocationID, ToLocationID));
                lblFromLoction.Text = lstLocation.Where(t => t.LocationID == Convert.ToInt32(FromLocationID)).FirstOrDefault().LocationName;
                lblToLocation.Text = lstLocation.Where(t => t.LocationID == Convert.ToInt32(ToLocationID)).FirstOrDefault().LocationName;
            }
            else {
                Location location = BusinessLayer.GetLocationList(string.Format("LocationID IN ({0})", FromLocationID)).FirstOrDefault();
                lblFromLoction.Text = location.LocationName;
                lblToLocation.Text = "Semua Lokasi";            
            }

            base.InitializeReport(param);
        }
    }
}
