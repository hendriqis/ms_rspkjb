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
    public partial class BVerifikasiPenerimaanPembelian_RSDOSOBA : BaseDailyPortraitRpt
    {
        public BVerifikasiPenerimaanPembelian_RSDOSOBA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseReceiveHd entityHd = BusinessLayer.GetvPurchaseReceiveHdList(param[0])[0];

            lblSupplierCode.Text = String.Format("{0} - {1}", entityHd.SupplierCode, entityHd.SupplierName);
            lblNoFaktur.Text = entityHd.ReferenceNo;
            lblTanggalFaktur.Text = entityHd.ReferenceDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblTanggalJatuhTempo.Text = entityHd.PaymentDueDate.ToString(Constant.FormatString.DATE_FORMAT);
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
            //StandardCode sc = BusinessLayer.GetStandardCode(entityHd.GCChargesType);
            //lblChargesType.Text = sc.StandardCodeName;
            //lblCharges.Text = entityHd.ChargesAmount.ToString("N2");
            lblDP.Text = entityHd.DownPaymentAmount.ToString("N2");
            lblCharges.Text = entityHd.ChargesAmount.ToString("N2");
            lblStamp.Text = entityHd.StampAmount.ToString("N2");

            //decimal netAmount = entityHd.TransactionAmount - entityHd.FinalDiscount
            //                    + ((entityHd.VATPercentage * (entityHd.TransactionAmount - entityHd.FinalDiscount)) / 100)
            //                    + entityHd.ChargesAmount;
            //lblTotalPenerimaan.Text = netAmount.ToString("N2");
            lblTotalPenerimaan.Text = entityHd.NetTransactionAmount.ToString("N2");

            lblCreatedByName.Text = entityHd.CreatedByName;

            if (entityHd.ApprovedBy == null)
            {
                lblApproved.Visible = false;
                lblApprovedDate.Visible = false;
            }
            else
            {
                lblApproved.Text = entityHd.ApprovedByName;
                lblApprovedDate.Text = string.Format("{0} {1}", entityHd.ApprovedDate.ToString(Constant.FormatString.DATE_FORMAT), entityHd.ApprovedDate.ToString(Constant.FormatString.TIME_FORMAT));
            }

            string log = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC);
            string nut = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_NUTRITION);
            SettingParameterDt logistic = BusinessLayer.GetSettingParameterDtList(log).FirstOrDefault();
            SettingParameterDt nutrition = BusinessLayer.GetSettingParameterDtList(nut).FirstOrDefault();
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

            List<GetPurchaseOrderType> lstPurchaseOrderType = BusinessLayer.GetPurchaseOrderTypeList(entityHd.PurchaseReceiveID);

            #region PurchaseOrderDt
            subPurchaseOrderType.CanGrow = true;
            bPurchaseOrderTypeDt.InitializeReport(lstPurchaseOrderType);
            #endregion

            lblFooterInfo.Text = String.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), AppSession.UserLogin.UserName);

            base.InitializeReport(param);
        }
    }
}
