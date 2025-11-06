using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.ReportDesktop;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Data.Service.Helper;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPenerimaanDonasiRSSC : BaseDailyPortraitRpt
    {
        public BPenerimaanDonasiRSSC()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);
            vPurchaseReceiveHd entityHd = BusinessLayer.GetvPurchaseReceiveHdList(param[0])[0];
            vPurchaseReceiveDt entityDt = BusinessLayer.GetvPurchaseReceiveDtList(param[0])[0];
            lblCreatedByName.Text = entityHd.CreatedByName;

            if (entityHd.GCItemType == "X001^002")
            {
                string filterExpression = string.Format(" ParameterCode IN ('{0}')", "PH0080");
                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExpression);
                lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "PH0080").FirstOrDefault().ParameterValue;
                lblApproved.Text = lstParam.Where(lst => lst.ParameterCode == "PH0080").FirstOrDefault().ParameterName;
            }
            else
            {
                string filterExpression = string.Format(" ParameterCode IN ('{0}')", "IM0046");
                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExpression);
                lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "IM0046").FirstOrDefault().ParameterValue;
                lblApproved.Text = lstParam.Where(lst => lst.ParameterCode == "IM0046").FirstOrDefault().ParameterName;
            }
            lblPurchaseReceiveNo.Text = entityHd.PurchaseReceiveNo;
            lblPurchaseReceiveDate.Text = entityHd.ReceivedDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblSupplierCode.Text = String.Format("{0} - {1}", entityHd.SupplierCode, entityHd.SupplierName);
            lblDiscount1.Text = String.Format("{0}%" + Environment.NewLine + "{1}", entityDt.DiscountPercentage1, entityDt.DiscountAmount1.ToString(Constant.FormatString.NUMERIC_2));
            lblDiscount2.Text = String.Format("{0}%" + Environment.NewLine + "{1}", entityDt.DiscountPercentage2, entityDt.DiscountAmount2.ToString(Constant.FormatString.NUMERIC_2));
        }
            
    }
}
