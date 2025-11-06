using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfinitResultCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnTransactionID.Value = param;
            string filterExpression = string.Format("TransactionID = {0}", hdnTransactionID.Value);
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
            if (entity != null)
            {
                EntityToControl(entity);
                BindGridView();
            }
        }

        private void BindGridView()
        {
            //string filterExpression = string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionID.Value);
            //List<vPatientChargesDt> lstDetail = BusinessLayer.GetvPatientChargesDtList(filterExpression);
            string filterExpression = string.Format("TransactionID = {0} AND  Sender='INFINIT' ", hdnTransactionID.Value);
            List<HL7Message> lstDetail = BusinessLayer.GetHL7MessageList(filterExpression);
            grdView.DataSource = lstDetail;
            grdView.DataBind();
        }

        private void EntityToControl(vPatientChargesHd entity)
        {
            if (entity != null)
            {
                txtTransactionNo.Text = entity.TransactionNo;
                txtTransactionDate.Text = entity.TransactionDateInString;
                txtMedicalNo.Text = entity.MedicalNo;
                txtPatientName.Text = entity.PatientName;
                txtOrderPhysician.Text = string.Format("{0}", entity.TestOrderPhysician);
                hdnTestOrderID.Value = entity.TestOrderID.ToString();
                hdnBridgingStatus.Value = entity.GCLISBridgingStatus;
            }
        }
    }
}