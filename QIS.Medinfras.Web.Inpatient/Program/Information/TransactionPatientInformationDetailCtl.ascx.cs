using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class TransactionPatientInformationDetailCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnRegistrationID.Value = param;

            string filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            vRegistration entityRegistration = BusinessLayer.GetvRegistrationList(filterExpression).FirstOrDefault();
            txtRegistraionNo.Text = entityRegistration.RegistrationNo;
            txtPatientName.Text = entityRegistration.PatientName;
            
            bindGrdTestOrder(filterExpression);
            bindGrdCharges(filterExpression);
            bindGrdBilling(filterExpression);
        }

        private void bindGrdTestOrder(String filterExpression) 
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}','{1}')", Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.VOID);
            List<vTestOrderHd> lstTestOrderHd = BusinessLayer.GetvTestOrderHdList(filterExpression);
            grdView.DataSource = lstTestOrderHd;
            grdView.DataBind();
        }

        private void bindGrdCharges(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}','{1}')", Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vPatientChargesHd> lstPatientChargesHd = BusinessLayer.GetvPatientChargesHdList(filterExpression);
            grdView2.DataSource = lstPatientChargesHd;
            grdView2.DataBind();
        }

        private void bindGrdBilling(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}','{1}')", Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<PatientBill> lstPatientBillHd = BusinessLayer.GetPatientBillList(filterExpression);
            grdView3.DataSource = lstPatientBillHd;
            grdView3.DataBind();
        }
    }
}