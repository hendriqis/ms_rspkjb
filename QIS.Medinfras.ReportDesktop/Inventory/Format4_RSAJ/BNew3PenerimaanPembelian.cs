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
    public partial class BNew3PenerimaanPembelian : BaseDailyPortraitRpt
    {
        public BNew3PenerimaanPembelian()
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
            
            lblDiskon.Text = entityHd.FinalDiscount.ToString("N2");
            if(entityHd.IsIncludeVAT)
            {
                lblPPN.Text = ((entityHd.VATPercentage * (entityHd.TransactionAmount - entityHd.FinalDiscount)) / 100).ToString("N2");
            }
            else 
            {
                lblPPN.Text = "0.00";
            }
            StandardCode sc = BusinessLayer.GetStandardCode(entityHd.GCChargesType);
            lblChargesType.Text = sc.StandardCodeName;
            lblCharges.Text = entityHd.ChargesAmount.ToString("N2");

            decimal netAmount = entityHd.TransactionAmount - entityHd.FinalDiscount
                                + ((entityHd.VATPercentage * (entityHd.TransactionAmount - entityHd.FinalDiscount)) / 100)
                                + entityHd.ChargesAmount;
            lblTotalPenerimaan.Text = netAmount.ToString("N2");

            lblCreatedByName.Text = entityHd.CreatedByName;
            
            string log = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC);
            string phr = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY);
            string sim = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_SIMIT);
            SettingParameterDt logistic = BusinessLayer.GetSettingParameterDtList(log).FirstOrDefault();
            SettingParameterDt pharmacy = BusinessLayer.GetSettingParameterDtList(phr).FirstOrDefault();
            SettingParameterDt simIT = BusinessLayer.GetSettingParameterDtList(sim).FirstOrDefault();

                Location locLogistik = BusinessLayer.GetLocationList(string.Format("LocationID = '{0}'", logistic.ParameterValue)).FirstOrDefault();
                Location locFarmasi = BusinessLayer.GetLocationList(string.Format("LocationID = '{0}'", pharmacy.ParameterValue)).FirstOrDefault();
                Location locSimTI = BusinessLayer.GetLocationList(string.Format("LocationID = '{0}'", simIT.ParameterValue)).FirstOrDefault();
                if (entityHd.LocationID == locLogistik.LocationID)
                {
                    string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Web.Common.Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_LOGISTIC);
                    SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
                    lblApprovedByName.Text = lstParam1.ParameterValue;
                }
                else if (entityHd.LocationID == locFarmasi.LocationID)
                {
                    string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Web.Common.Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_PHARMACY);
                    SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
                    lblApprovedByName.Text = lstParam1.ParameterValue;
                }
                else if (entityHd.LocationID == locSimTI.LocationID)
                {
                    string filterExpression = string.Format(" ParameterCode IN ('{0}')", Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_SIMIT);
                    SettingParameterDt lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression).FirstOrDefault();
                    lblApprovedByName.Text = lstParam.ParameterValue;
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
