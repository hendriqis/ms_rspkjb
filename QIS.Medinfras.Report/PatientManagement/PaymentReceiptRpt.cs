using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Report
{
    public partial class PaymentReceiptRpt : BaseDailyPortraitRpt
    {
        public PaymentReceiptRpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);
            IDbContext ctx = DbFactory.Configure(true);
            string errMessage = "";
            
            try
            {
                vHealthcare entity = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];
                lblReceiptDate.Text = entity.City + ", [ReceiptDateInString]";
                
                int id = BusinessLayer.GetPaymentReceiptMaxID(ctx);
                string name = BusinessLayer.GetPaymentReceipt(id).PrintAsName;
                lblPrintAsName.Text = name;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
            }
            finally
            {
                ctx.Close();
            }
        }

        private void xrLabel19_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Int64 total = Convert.ToInt64(GetCurrentColumnValue("ReceiptAmount"));
            ((XRLabel)sender).Text = Helper.NumberInWords(total, true);
        }

        private void xrLabel28_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String nama = lblPrintAsName.Text;
            Decimal amount = Convert.ToDecimal(GetCurrentColumnValue("ReceiptAmount"));
            ((XRLabel)sender).Text = string.Format("Sudah terima dari {0} sebesar Rp {1}",nama,amount);
        }

    }
}
