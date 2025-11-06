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
    public partial class BRekapitulasiPembayaranJasaMedis : BaseDailyPortraitRpt
    {
        public BRekapitulasiPembayaranJasaMedis()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(',');
            string[] periode = temp[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(periode[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(periode[1]).ToString(Constant.FormatString.DATE_FORMAT));

            if(temp[1] != "" && temp[1] != "0")
            {
                ParamedicMaster oPM = BusinessLayer.GetParamedicMaster(Convert.ToInt32(temp[1]));
                lblParamedic.Text = string.Format("{0} ({1})", oPM.FullName, oPM.ParamedicCode);
            }

            base.InitializeReport(param);
        }


    }
}
