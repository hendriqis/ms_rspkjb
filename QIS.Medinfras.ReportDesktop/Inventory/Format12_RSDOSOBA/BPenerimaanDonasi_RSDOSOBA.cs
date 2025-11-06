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
    public partial class BPenerimaanDonasi_RSDOSOBA : BaseDailyPortraitRpt
    {
        public BPenerimaanDonasi_RSDOSOBA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseReceiveHd entityHd = BusinessLayer.GetvPurchaseReceiveHdList(param[0])[0];
            lblCreatedByName.Text = entityHd.CreatedByName;

            //lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == lstParam).FirstOrDefault().ParameterValue
            string log = string.Format("ParameterCode IN ('{0}')", Web.Common.Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC);
            string nut = string.Format("ParameterCode IN ('{0}')", Web.Common.Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_NUTRITION);
            string med = string.Format("ParameterCode IN ('{0}')", Web.Common.Constant.SettingParameter.IM_DEFAULT_LOCATION_CSSD);
            SettingParameterDt logistic = BusinessLayer.GetSettingParameterDtList(log).FirstOrDefault();
            SettingParameterDt nutrition = BusinessLayer.GetSettingParameterDtList(nut).FirstOrDefault();
            SettingParameterDt medic = BusinessLayer.GetSettingParameterDtList(med).FirstOrDefault();
            if (logistic.ParameterValue != null && logistic.ParameterValue != "" && logistic.ParameterValue != "0")
            {
                Location loc = BusinessLayer.GetLocationList(string.Format("LocationID = '{0}'", logistic.ParameterValue)).FirstOrDefault();
                if (entityHd.LocationID == loc.LocationID)
                {
                    string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Web.Common.Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_LOGISTIC);
                    SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
                    lblApprovedByName.Text = lstParam1.ParameterValue;
                }
                else if (entityHd.LocationID == Convert.ToInt32(nutrition.ParameterValue))
                {
                    string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Web.Common.Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_NUTRITION);
                    SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
                    lblApprovedByName.Text = lstParam1.ParameterValue;
                }
                else if (entityHd.LocationID == Convert.ToInt32(medic.ParameterValue))
                {
                    string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Web.Common.Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_MEDIK);
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
                string filterExpression = string.Format(" ParameterCode IN ('{0}')", Web.Common.Constant.SettingParameter.PHARMACIST);
                List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
                lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "PH0004").FirstOrDefault().ParameterValue;
            }

            lblPurchaseReceiveNo.Text = entityHd.PurchaseReceiveNo;
            lblPurchaseReceiveDate.Text = entityHd.ReceivedDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblSupplierCode.Text = String.Format("{0} - {1}", entityHd.SupplierCode, entityHd.SupplierName);

            base.InitializeReport(param);
        }
            
    }
}
