using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LDaftarAkun : BaseDailyPortraitRpt
    {
        public LDaftarAkun()
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

        
    }
}
