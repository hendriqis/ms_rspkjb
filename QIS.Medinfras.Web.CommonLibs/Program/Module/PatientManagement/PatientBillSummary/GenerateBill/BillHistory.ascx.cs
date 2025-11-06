using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class BillHistory : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] registrationID = param.Split('|');
            string paramRegistration = registrationID[0];
            if (registrationID[1] != "") paramRegistration += "," + registrationID[1];
            BindGrid(paramRegistration);
        }

        private void BindGrid(string paramRegistration)
        {
            List<vPatientBill> lstPatientBill = BusinessLayer.GetvPatientBillList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus <> '{1}'", paramRegistration, Constant.TransactionStatus.VOID));
            lvwView.DataSource = lstPatientBill;
            lvwView.DataBind();
            decimal totalAdminPatientAmount = lstPatientBill.Sum(p => p.PatientAdminFeeAmount);
            decimal totalAdminPayerAmount = lstPatientBill.Sum(p => p.AdministrationFeeAmount);
            decimal totalServicePatientAmount = lstPatientBill.Sum(p => p.PatientAdminFeeAmount);
            decimal totalServicePayerAmount = lstPatientBill.Sum(p => p.AdministrationFeeAmount);

            if (lstPatientBill.Count > 0)
            {
                ((HtmlGenericControl)lvwView.FindControl("tdTotalAdminPayer")).InnerHtml = totalAdminPayerAmount.ToString("N");
                ((HtmlGenericControl)lvwView.FindControl("tdTotalAdminPatient")).InnerHtml = totalAdminPatientAmount.ToString("N");
                ((HtmlGenericControl)lvwView.FindControl("tdTotalServicePayer")).InnerHtml = totalServicePayerAmount.ToString("N");
                ((HtmlGenericControl)lvwView.FindControl("tdTotalServicePatient")).InnerHtml = totalServicePatientAmount.ToString("N");
            }
        }
    }
}