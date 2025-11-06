using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class OutstandingPaymentInformation : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                hdnMRN.Value = param.ToString();
                Patient patient = BusinessLayer.GetPatient(Convert.ToInt32(hdnMRN.Value));                
                txtMRN.ReadOnly = true;
                txtPatientName.ReadOnly = true;
                txtMRN.Text = patient.MedicalNo;
                txtPatientName.Text = patient.FullName;
                BindGridView();
            } 
        }

        private void BindGridView()
        {
            string filter = string.Format("MRN = {0}", hdnMRN.Value);
            List<vPatientHasOutstandingInvoicePayment> lstOutstanding = BusinessLayer.GetvPatientHasOutstandingInvoicePaymentList(filter);
            List<vPatientPaymentHd> lstPayment = new List<vPatientPaymentHd>();

            foreach (vPatientHasOutstandingInvoicePayment e in lstOutstanding) {
                string filterExpression = string.Format("PaymentNo = '{0}'", e.PaymentNo);
                vPatientPaymentHd entity = BusinessLayer.GetvPatientPaymentHdList(filterExpression).FirstOrDefault();
                lstPayment.Add(entity);
            }

            grdView.DataSource = lstPayment;
            grdView.DataBind();
        }

        protected void cbpOutstandingPaymentInformation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}