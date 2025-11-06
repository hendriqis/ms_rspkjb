using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Inpatient.Controls
{
    public partial class PaymentLetterCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            String[] lstParam = param.Split('|');
            hdnRegistrationID.Value = lstParam[0];
            hdnID.Value = lstParam[1];

            PatientBill entityPB = BusinessLayer.GetPatientBillList(String.Format("RegistrationID = {0} AND GCTransactionStatus = 'X121^001'", Convert.ToInt32(lstParam[0]))).FirstOrDefault();

            if (hdnID.Value == "pl")
            {
                txtPayerLetterAmount.Text = "0.00";
            }
            else if (hdnID.Value == "pdl")
            {
                if (entityPB == null)
                {
                    txtPayerLetterAmount.Text = "0.00";
                }
                else
                {
                    txtPayerLetterAmount.Text = Convert.ToString(entityPB.TotalPatientAmount.ToString("N2"));
                }
            }

            txtPayerLetterAmount.Attributes.Add("validationgroup", "mpTrxPopup");
        }

        protected void cbpPaymentLetter_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}