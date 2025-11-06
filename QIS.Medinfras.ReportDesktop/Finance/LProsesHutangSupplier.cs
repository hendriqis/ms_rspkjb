using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LProsesHutangSupplier : BaseDailyPortraitRpt
    {
        public LProsesHutangSupplier()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            string filterExpression = String.Format("StandardCodeID = '{0}'",param[1].ToString());

            if (!String.IsNullOrEmpty(param[1].ToString()))
            {
                lblStatusTransaksi.Text = string.Format("Status Transaksi : {0}", BusinessLayer.GetStandardCodeList(filterExpression).FirstOrDefault().StandardCodeName);
            }
            else 
            {
                lblStatusTransaksi.Text = string.Format("Status Transaksi : ALL");            
            }
            base.InitializeReport(param);
        }

    }
}
