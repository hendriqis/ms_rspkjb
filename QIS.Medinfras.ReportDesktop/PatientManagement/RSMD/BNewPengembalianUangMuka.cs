using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNewPengembalianUangMuka : BaseDailyPortraitRpt
    {
        public BNewPengembalianUangMuka()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            vPatientPaymentHd entityPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("{0}", param[0]))[0];
            lblLastUpdatedDate.Text = entityHealthcare.City + ", " + entityPayment.PaymentDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblLastUpdatedBy.Text = appSession.UserFullName;
            lblTotalAmountString.Text = "# " + entityPayment.TotalPaymentAmountOUTInString + " #";

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityPayment.RegistrationID))[0];
            lblUnitPelayanan.Text = entityReg.ServiceUnitName;

            List<vPatientPaymentDt> ListPaymentDt = BusinessLayer.GetvPatientPaymentDtList(string.Format("PaymentID = {0} AND GCPaymentMethod != '{1}'", entityPayment.PaymentID, Constant.PaymentMethod.DOWN_PAYMENT));

            String NoKartu = "";
            String JenisKartu = "";
            String EDC = "";
            String PaymentMethod = "";
            foreach (vPatientPaymentDt e in ListPaymentDt)
            {
                if (e.GCPaymentMethod == Constant.PaymentMethod.DEBIT_CARD || e.PaymentMethod == Constant.PaymentMethod.CREDIT_CARD)
                {
                    if (NoKartu != "")
                    {
                        NoKartu = NoKartu + ", " + e.CardNumber + "(" + e.PaymentMethod + ")";
                    }
                    else
                    {
                        NoKartu = e.CardNumber + "(" + e.PaymentMethod + ")";
                    }

                    if (JenisKartu != "")
                    {
                        JenisKartu = JenisKartu + ", " + e.CardType;
                    }
                    else
                    {
                        JenisKartu = e.CardType;
                    }

                    if (EDC != "")
                    {
                        EDC = EDC + ", " + e.EDCMachineName;
                    }
                    else
                    {
                        EDC = e.EDCMachineName;
                    }
                }

                if (e.GCPaymentMethod == Constant.PaymentMethod.BANK_TRANSFER)
                {
                    PaymentMethod += e.PaymentMethod + "(" + e.BankName + ")" + ", ";
                }
                else
                {
                    PaymentMethod += e.PaymentMethod + ", ";
                }
            }
            lblPaymentMethod.Text = PaymentMethod.Substring(0, PaymentMethod.Length - 2);
            lblCardNo.Text = NoKartu;
            lblCardType.Text = JenisKartu;
            lblEDC.Text = EDC;
            base.InitializeReport(param);
        }

    }
}
