using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BHonorDokterPerDateRSPBT : BaseCustomDailyPotraitRpt
    {
        public BHonorDokterPerDateRSPBT()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string filterExpression = string.Format("ProcessedDate = '{0}' AND ParamedicID = {1}", param[0], param[1]);
            vTransRevenueSharingHd entity = BusinessLayer.GetvTransRevenueSharingHdList(filterExpression).FirstOrDefault();
            List<vTransRevenueSharingHd> lstEntity = BusinessLayer.GetvTransRevenueSharingHdList(filterExpression);

            string revenueSharingNo = "";
            string reduction = "";
            string paymentMethod = "";
            string periodeType = "";
            string periodeInfo = "";
            foreach (vTransRevenueSharingHd ts in lstEntity)
            {
                if (revenueSharingNo != "")
                {
                    revenueSharingNo += ", " + ts.RevenueSharingNo;
                }
                else
                {
                    revenueSharingNo = ts.RevenueSharingNo;
                }
            }

            lblParamedicName.Text = entity.ParamedicName;
            cTransactionNo.Text = revenueSharingNo;
            cTransactionDate.Text = entity.ProcessedDateInString;
            cReduction.Text = entity.Reduction;
            cPaymentMethod.Text = entity.PaymentMethod;
            cPeriodeType.Text = entity.PeriodeType;
            cPeriodeInfo.Text = entity.cfPeriodeText;

            decimal transactionAmount = lstEntity.Sum(p => p.TotalTransactionAmount);
            decimal creditCardAmount = lstEntity.Sum(p => p.TotalCreditCardFeeAmount);
            decimal discountAmount = lstEntity.Sum(p => p.TotalDiscountAmount);
            decimal adjustmentAmount = lstEntity.Sum(p => p.TotalAdjustmentAmount);
            decimal revenueAmount = lstEntity.Sum(p => p.TotalRevenueSharingAmount);
            decimal reductionAmount = lstEntity.Sum(p => p.ReductionAmount);
            decimal totalRevenueAmount = lstEntity.Sum(p => p.TotalRevenueSharingReductionAmount);

            cTransactionAmount.Text = transactionAmount.ToString(Constant.FormatString.NUMERIC_2);
            cCreditCardAmount.Text = creditCardAmount.ToString(Constant.FormatString.NUMERIC_2);
            cDiscountAmount.Text = discountAmount.ToString(Constant.FormatString.NUMERIC_2);
            cAdjustmentAmount.Text = adjustmentAmount.ToString(Constant.FormatString.NUMERIC_2);
            cRevenueAmount.Text = revenueAmount.ToString(Constant.FormatString.NUMERIC_2);
            cReductionAmount.Text = reductionAmount.ToString(Constant.FormatString.NUMERIC_2);
            cTotalRevenueAmount.Text = totalRevenueAmount.ToString(Constant.FormatString.NUMERIC_2);

            base.InitializeReport(param);
        }

    }
}
