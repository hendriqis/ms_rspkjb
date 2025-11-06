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
    public partial class BNew4DistribusiBarang : BaseDailyPortraitRpt
    {
        public BNew4DistribusiBarang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param) 
        {
            vItemDistributionHd entityHd = BusinessLayer.GetvItemDistributionHdList(param[0])[0];
            lblDistributionNo.Text = entityHd.DistributionNo;
            lblDistributionDate.Text = entityHd.DeliveryDate.ToString(Constant.FormatString.DATE_FORMAT) + " / " + entityHd.DeliveryTime;
            lblWarehouseCode.Text = String.Format("{0} - {1}", entityHd.FromLocationCode, entityHd.FromLocationName);
            lblOtherWarehouseCode.Text = String.Format("{0} - {1}", entityHd.ToLocationCode, entityHd.ToLocationName);
            lblDeliveryRemarks.Text = entityHd.DeliveryRemarks;

            //string filterExpression = string.Format(" ParameterCode IN ('{0}')", "IM0009");
            //List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            //lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;

            //lblCreatedByName.Text = entityHd.CreatedByName;

            base.InitializeReport(param);
        }
    }
}
