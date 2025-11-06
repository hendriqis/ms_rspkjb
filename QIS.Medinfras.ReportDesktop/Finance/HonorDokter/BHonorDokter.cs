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
    public partial class BHonorDokter : BaseCustomDailyPotraitRpt
    {
        public BHonorDokter()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vTransRevenueSharingHd entity = BusinessLayer.GetvTransRevenueSharingHdList(param[0]).FirstOrDefault();
            lblParamedicName.Text = entity.ParamedicName;
            cTransactionNo.Text = entity.RevenueSharingNo;
            cTransactionDate.Text = entity.ProcessedDateInString;
            cReduction.Text = entity.Reduction;
            cPaymentMethod.Text = entity.PaymentMethod;
            cPeriodeType.Text = entity.PeriodeType;
            cPeriodeInfo.Text = entity.cfPeriodeText;

            cTransactionAmount.Text = entity.TotalTransactionAmount.ToString(Constant.FormatString.NUMERIC_2);
            cCreditCardAmount.Text = entity.TotalCreditCardFeeAmount.ToString(Constant.FormatString.NUMERIC_2);
            cDiscountAmount.Text = entity.TotalDiscountAmount.ToString(Constant.FormatString.NUMERIC_2);
            cAdjustmentAmount.Text = entity.TotalAdjustmentAmount.ToString(Constant.FormatString.NUMERIC_2);
            cRevenueAmount.Text = entity.TotalRevenueSharingAmount.ToString(Constant.FormatString.NUMERIC_2);
            cReductionAmount.Text = entity.ReductionAmount.ToString(Constant.FormatString.NUMERIC_2);
            cTotalRevenueAmount.Text = entity.TotalRevenueSharingReductionAmount.ToString(Constant.FormatString.NUMERIC_2);

            base.InitializeReport(param);
        }

    }
}
