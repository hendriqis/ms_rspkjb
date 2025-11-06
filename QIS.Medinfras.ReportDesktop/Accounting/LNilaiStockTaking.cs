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
    public partial class LNilaiStockTaking : BaseDailyPortraitRpt
    {
        public LNilaiStockTaking()
        {
            InitializeComponent();
        }

        private void xrTableCell7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        { 
            XRControl control = (XRControl)sender;
            //control.LocationF = new PointF(15F, 15F);
            int level = Convert.ToInt32(GetCurrentColumnValue("Level"));
            control.Padding = new DevExpress.XtraPrinting.PaddingInfo(level * 10,0,0,0);
            //control.Styles.Style = this.StyleSheet[0];
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            String idLocation = param[1];
            Location entity = BusinessLayer.GetLocation(Convert.ToInt32(idLocation));
            lblLocation.Text = "Lokasi : " + entity.LocationName;
            base.InitializeReport(param);
        }
        
    }
}
