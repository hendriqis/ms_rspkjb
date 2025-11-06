using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Data;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BReturPembelianTunaiTanpaNilai: BaseDailyPortraitRpt
    {
        public BReturPembelianTunaiTanpaNilai()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {

            vDirectPurchaseReturnHd entityHd = BusinessLayer.GetvDirectPurchaseReturnHdList(param[0])[0];

            lblCreatedByName.Text = entityHd.CreatedByName;

            string filterExpression = string.Format(" ParameterCode IN ('{0}')", "IM0009");
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;

            base.InitializeReport(param);
        }

    }
}
