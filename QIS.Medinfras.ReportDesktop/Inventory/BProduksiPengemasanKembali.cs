using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BProduksiPengemasanKembali : BaseDailyPortraitRpt
    {
        public BProduksiPengemasanKembali()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vItemProductionHd entityHd = BusinessLayer.GetvItemProductionHdList(param[0]).FirstOrDefault();
            lblWarehouse.Text = String.Format("{0} - {1}",entityHd.FromLocationCode,entityHd.FromLocationName);
            lblRemarksHd.Text = entityHd.Remarks;

            lblCreatedByName.Text = entityHd.CreatedByName;

            string filterExpression = string.Format(" ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_UMUM);
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.KEPALA_LOGISTIK_UMUM).FirstOrDefault().ParameterValue;

            base.InitializeReport(param);
        }

    }
}
