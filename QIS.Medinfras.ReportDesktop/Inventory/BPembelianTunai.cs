using System;
using System.Data.OleDb;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.XtraReports.Extensions;
using System.Data;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPembelianTunai : BaseDailyPortraitRpt
    {
        public BPembelianTunai()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vDirectPurchaseHd entity = BusinessLayer.GetvDirectPurchaseHdList(String.Format("DirectPurchaseID IN (SELECT DirectPurchaseID FROM vDirectPurchaseDt WHERE {0})", param[0]))[0];
            lblCreatedByName.Text = entity.CreatedByName;

            string filterExpression = string.Format(" ParameterCode IN ('{0}')", "IM0009");
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;

        }
    }
}
