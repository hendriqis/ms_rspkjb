using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPurchaseReceivePerItemRpt : BaseDailyPortraitRpt
    {
        public LPurchaseReceivePerItemRpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] date = param[1].Split(';');
            Location entity = BusinessLayer.GetLocation(Convert.ToInt32(param[0]));
            lblLocation.Text = entity.LocationName;
            lblDate.Text = string.Format("{0} s.d {1}", Helper.YYYYMMDDToDate(date[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(date[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }
    }
}
