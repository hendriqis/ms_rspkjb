using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;



namespace QIS.Medinfras.ReportDesktop
{
    public partial class LHutangPerUsahaPersuplier : BaseCustomDailyPotraitRpt
    {
        public LHutangPerUsahaPersuplier()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

            vBusinessPartners bp = BusinessLayer.GetvBusinessPartnersList(string.Format("BusinessPartnersID = {0}", param[1])).FirstOrDefault();
            txtSupplier.Text = string.Format("Suppplier : {0}", bp.BusinessPartnersName);  


            base.InitializeReport(param);
        }

    }
}
