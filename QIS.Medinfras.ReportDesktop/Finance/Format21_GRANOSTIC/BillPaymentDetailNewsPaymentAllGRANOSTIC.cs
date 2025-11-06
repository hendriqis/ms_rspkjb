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
    public partial class BillPaymentDetailNewsPaymentAllGRANOSTIC : BaseDailyPortraitRpt
    {
        public BillPaymentDetailNewsPaymentAllGRANOSTIC()
        {
            InitializeComponent();
        }
        protected decimal cash = 0;
        protected decimal banktransfer = 0;
        protected decimal creditCard = 0;
        protected decimal debitCard = 0;
        protected decimal piutang = 0;
        protected decimal piutangIns = 0;
        public void InitializeReport(List<GetNewsAttachmentPerShift> lst)
        {
            foreach (GetNewsAttachmentPerShift atc in lst)
            {
                if (atc.PaymentMethod == "Bank Transfer")
                {
                    banktransfer = lst.Where(a => a.PaymentMethod == ("Bank Transfer")).Sum(a => a.NettSales);
                }
                else if (atc.PaymentMethod == "Cash")
                {
                    cash = lst.Where(a => a.PaymentMethod == ("Cash")).Sum(a => a.NettSales);
                }
                else if (atc.PaymentMethod == "Credit Card")
                {
                    creditCard = lst.Where(a => a.PaymentMethod == ("Credit Card")).Sum(a => a.NettSales);
                }
                else if (atc.PaymentMethod == "Debit Card")
                {
                    debitCard = lst.Where(a => a.PaymentMethod == ("Debit Card")).Sum(a => a.NettSales);
                }
                else if (atc.PaymentMethod == "Piutang")
                {
                    piutang = lst.Where(a => a.PaymentMethod == ("Piutang")).Sum(a => a.NettSales);
                }
                #region Payment Column
                #region Bank Transfer
                if (lst.Where(a => a.PaymentMethod == ("Bank Transfer")) != null)
                {
                    lblBank.Text = string.Format("{0}", banktransfer.ToString(Constant.FormatString.NUMERIC_2));
                }
                else
                {
                    lblBank.Text = string.Format(" ");
                }
                #endregion
                #region Cash
                if (lst.Where(a => a.PaymentMethod == ("Cash")) != null)
                {
                    lblCash.Text = string.Format("{0}", cash.ToString(Constant.FormatString.NUMERIC_2));
                }
                else 
                {
                    lblCash.Text = string.Format(" ");
                }
                #endregion
                #region Card
                if (lst.Where(a => a.PaymentMethod == ("Credit Card") || a.PaymentMethod == ("Debit Card")) != null)
                {
                    lblCard2.Text = string.Format("{0}", (creditCard + debitCard).ToString(Constant.FormatString.NUMERIC_2));
                }
                else
                {
                    lblCard2.Text = string.Format(" ");
                }
                #endregion
                #region Credit Card
                if (lst.Where(a => a.PaymentMethod == ("Credit Card")) != null)
                {
                    lblCredit.Text = string.Format("{0}", creditCard.ToString(Constant.FormatString.NUMERIC_2));
                }
                else
                {
                    lblCredit.Text = string.Format(" ");
                }
                #endregion
                #region Debit Card
                if (lst.Where(a => a.PaymentMethod == ("Debit Card")) != null)
                {
                    lblDebit.Text = string.Format("{0}", debitCard.ToString(Constant.FormatString.NUMERIC_2));
                }
                else
                {
                    lblDebit.Text = string.Format(" ");
                }
                #endregion
                #region Piutang
                if (lst.Where(a => a.PaymentMethod == ("Piutang")) != null)
                {
                    //foreach (GetNewsAttachmentPerShift atc2 in lst.Where(a => a.PaymentMethod == "Piutang"))
                    //{
                    //    string instansi = atc2.BusinessPartnerName;
                    //    piutangIns = lst.Where(a => a.PaymentMethod == "Piutang").Sum(a => a.NettSales);
                    //    lblBusinesspartner.Text = string.Format("{0} ", instansi);
                    //    lblPiutangIns.Text = string.Format("{0} ", piutangIns.ToString(Constant.FormatString.NUMERIC_2));
                    //}
                    lblPiutang2.Text = string.Format("{0}", piutang.ToString(Constant.FormatString.NUMERIC_2));
                }
                else
                {
                    lblPiutang.Text = string.Format(" ");
                    lblPiutang2.Text = string.Format(" ");
                }
                #endregion
                #endregion

                #region Total & Subtotal
                if (atc.PaymentMethod == "Credit Card" || atc.PaymentMethod == "Debit Card" || atc.PaymentMethod == "Piutang")
                {
                    lblSubTotal.Text = (lst.Where(a => a.PaymentMethod == ("Credit Card")).Sum(a => a.NettSales) + lst.Where(a => a.PaymentMethod == ("Debit Card")).Sum(a => a.NettSales) + lst.Where(a => a.PaymentMethod == ("Piutang")).Sum(a => a.NettSales)).ToString(Constant.FormatString.NUMERIC_2);
                }
                if (atc.PaymentMethod == "Bank Transfer" || atc.PaymentMethod == "Cash" || atc.PaymentMethod == "Credit Card" || atc.PaymentMethod == "Debit Card" || atc.PaymentMethod == "Piutang")
                {
                    lblTotal.Text = (lst.Where(a => a.PaymentMethod == ("Bank Transfer")).Sum(a => a.NettSales) + lst.Where(a => a.PaymentMethod == ("Cash")).Sum(a => a.NettSales) + lst.Where(a => a.PaymentMethod == ("Credit Card")).Sum(a => a.NettSales) + lst.Where(a => a.PaymentMethod == ("Debit Card")).Sum(a => a.NettSales) + lst.Where(a => a.PaymentMethod == ("Piutang")).Sum(a => a.NettSales)).ToString(Constant.FormatString.NUMERIC_2); 
                }
                #endregion
            }
            this.DataSource = lst;
        }
    }
}
