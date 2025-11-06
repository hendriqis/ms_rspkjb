using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.ReportDesktop;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BFormCheckListPermintaanBarang : BaseDailyPortraitRpt
    {
        public BFormCheckListPermintaanBarang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseReceiveHd entityHd = BusinessLayer.GetvPurchaseReceiveHdList(param[0])[0];

            lblSupplierCode.Text = String.Format("{0} - {1}", entityHd.SupplierCode, entityHd.SupplierName);
            lblNoFaktur.Text = entityHd.ReferenceNo;
            lblTanggalFaktur.Text = entityHd.ReferenceDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblRemarks.Text = entityHd.Remarks;
            

            lblCreatedByName.Text = entityHd.CreatedByName;
            
            string log = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC);
            SettingParameterDt logistic = BusinessLayer.GetSettingParameterDtList(log).FirstOrDefault();
            if (logistic.ParameterValue != null && logistic.ParameterValue != "" && logistic.ParameterValue != "0")
            {
                vLocation loc = BusinessLayer.GetvLocationList(string.Format("LocationID = '{0}'", logistic.ParameterValue)).FirstOrDefault();
                if (entityHd.LocationID == loc.LocationID)
                {
                    string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Web.Common.Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_LOGISTIC);
                    SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
                    lblApprovedByName.Text = lstParam1.ParameterValue;
                }
                else
                {
                    string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Web.Common.Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_PHARMACY);
                    SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
                    lblApprovedByName.Text = lstParam1.ParameterValue;
                }
            }
            else
            {
                string filterExpression = string.Format(" ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_UMUM);
                List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
                lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;
            }

            base.InitializeReport(param);
        }
    }
}
