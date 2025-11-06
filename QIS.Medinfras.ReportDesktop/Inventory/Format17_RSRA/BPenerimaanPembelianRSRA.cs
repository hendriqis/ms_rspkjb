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
    public partial class BPenerimaanPembelianRSRA : BaseDailyPortraitRpt
    {
        public BPenerimaanPembelianRSRA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseReceiveHd entityHd = BusinessLayer.GetvPurchaseReceiveHdList(param[0]).FirstOrDefault();

            lblSupplierCode.Text = String.Format("{0} - {1}", entityHd.SupplierCode, entityHd.SupplierName);
            lblNoFaktur.Text = entityHd.ReferenceNo;
            lblTanggalFaktur.Text = entityHd.ReferenceDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblProductLine.Text = entityHd.ProductLineName;
            lblRemarks.Text = entityHd.Remarks;
            
            lblDiskon.Text = entityHd.FinalDiscount.ToString(Constant.FormatString.NUMERIC_2);
            if(entityHd.IsIncludeVAT)
            {
                lblPPN.Text = ((entityHd.VATPercentage * (entityHd.TransactionAmount - entityHd.FinalDiscount)) / 100).ToString(Constant.FormatString.NUMERIC_2);
            }
            else 
            {
                lblPPN.Text = "0.00";
            }

            lblDP.Text = entityHd.DownPaymentAmount.ToString(Constant.FormatString.NUMERIC_2);
            lblStamp.Text = entityHd.StampAmount.ToString(Constant.FormatString.NUMERIC_2);
            lblCharges.Text = entityHd.ChargesAmount.ToString(Constant.FormatString.NUMERIC_2);

            lblTotalPenerimaan.Text = entityHd.NetTransactionAmount.ToString("N2");

            lblApproved.Text = entityHd.ApprovedByName;
            lblApprovedDate.Text = string.Format("{0} {1}", entityHd.ApprovedDate.ToString(Constant.FormatString.DATE_FORMAT), entityHd.ApprovedDate.ToString(Constant.FormatString.TIME_FORMAT));
            
            string log = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC);
            SettingParameterDt logistic = BusinessLayer.GetSettingParameterDtList(log).FirstOrDefault();
            if (logistic.ParameterValue != null && logistic.ParameterValue != "" && logistic.ParameterValue != "0")
            {
                Location loc = BusinessLayer.GetLocationList(string.Format("LocationID = '{0}'", logistic.ParameterValue)).FirstOrDefault();
                if (entityHd.LocationID == loc.LocationID)
                {
                    string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Web.Common.Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_LOGISTIC);
                    SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
                    lblApprovedByName.Visible = false;
                    lblCreatedByName.Visible = false;
                    lblPenerima.Text = "Penerima";
                    lblPengirim.Text = "Pengirim";
                }
                else
                {
                    string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Web.Common.Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_PHARMACY);
                    SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
                    lblCreatedByName.Text = entityHd.CreatedByName;
                    lblApprovedByName.Text = lstParam1.ParameterValue;
                    lblPenerima.Text = "Dibuat Oleh :";
                    lblPengirim.Text = "Disetujui Oleh :";
                }
            }
            else
            {
                string filterExpression = string.Format(" ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_UMUM);
                List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
                lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;
                lblPenerima.Text = "Dibuat Oleh :";
                lblPengirim.Text = "Disetujui Oleh :";
            }

            List<GetPurchaseOrderType> lstPurchaseOrderType = BusinessLayer.GetPurchaseOrderTypeList(entityHd.PurchaseReceiveID);

            #region PurchaseOrderDt
            subPurchaseOrderType.CanGrow = true;
            bPurchaseOrderTypeDt.InitializeReport(lstPurchaseOrderType);
            #endregion

            base.InitializeReport(param);
        }
    }
}
