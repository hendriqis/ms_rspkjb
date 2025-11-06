using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Data.Service.Helper;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNew4PemakaianBarang : BaseDailyPortraitRpt
    {
        public BNew4PemakaianBarang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vItemTransactionHd entity = BusinessLayer.GetvItemTransactionHdList(param[0])[0];
            lblCreatedByName.Text = entity.CreatedByName;
            lblTipePemakaian.Text = entity.ConsumptionType;
            lblUnit.Text = entity.HealthcareUnit;
            lblKeterangan.Text = entity.Remarks;
            string filterExpression = string.Format(" ParameterCode IN ('{0}')", "IM0046");
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "IM0046").FirstOrDefault().ParameterValue;

            base.InitializeReport(param);
        }

    }
}
