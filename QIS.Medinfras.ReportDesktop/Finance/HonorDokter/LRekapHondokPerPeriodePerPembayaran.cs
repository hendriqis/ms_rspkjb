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
    public partial class LRekapHondokPerPeriodePerPembayaran : BaseDailyPortraitRpt
    {
        public LRekapHondokPerPeriodePerPembayaran()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            
            //List<StandardCode> lstSt = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID != '{1}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REVENUE_SHARING_COMPONENT, Constant.RevenueSharingComponent.PARAMEDIC));
            //int count = lstSt.Count;
            //float width = 400 / count;
            //int idx = 0;
            //int max = 10;
            //XRLabel lbl;
            
            //// fill header column text
            //for ( idx = 0; idx < count; idx++)
            //{
            //    lbl = (XRLabel) FindControl(string.Format("hdCol{0}",idx+1),true);
            //    lbl.Text = lstSt[idx].StandardCodeName;
            //    lbl.WidthF = width;
            //    // detail column
            //    lbl = (XRLabel)FindControl(string.Format("Col{0}", idx + 1), true);
            // //   lbl.WidthF = width;
            //    // total column
            //    lbl = (XRLabel)FindControl(string.Format("tot{0}", idx + 1), true);
            //    lbl.WidthF = width;
            //}
            //// hide useless column
            //for ( ; idx < max; idx++)
            //{
            //    // header column
            //    lbl = (XRLabel)FindControl(string.Format("hdCol{0}", idx + 1), true);
            //    lbl.Text = "";
            //    lbl.WidthF = 2;
            //    // detail column
            //    lbl = (XRLabel)FindControl(string.Format("Col{0}", idx + 1), true);
            //    lbl.Visible = false;
            //    lbl.WidthF = 2;
            //    // total column
            //    lbl = (XRLabel)FindControl(string.Format("tot{0}", idx + 1), true);
            //    lbl.Visible = false;
            //    lbl.WidthF = 2;
            //}
            
            base.InitializeReport(param);
        }


    }
}
