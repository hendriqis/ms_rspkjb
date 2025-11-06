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
    public partial class PatientBillSummaryGenerateBillDtPrescriptionReturnCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            Int32 PrescriptionReturnOrderID = Convert.ToInt32(param);
            vPrescriptionReturnOrderHd entity = BusinessLayer.GetvPrescriptionReturnOrderHdList(string.Format("PrescriptionReturnOrderID = {0}", PrescriptionReturnOrderID)).FirstOrDefault();

            txtTransactionNo.Text = entity.TransactionNo;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtPrescriptionReturnType.Text = entity.PrescriptionReturnType;
            txtTransactionDate.Text = entity.OrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = entity.OrderTime;

            BindGrid(PrescriptionReturnOrderID);
        }

        public void BindGrid(Int32 PrescriptionReturnOrderID) 
        {
            String filterExpression = String.Format("PrescriptionReturnOrderID = {0} AND IsDeleted = 0", PrescriptionReturnOrderID);
            List<vPrescriptionReturnOrderDt> lstPrescriptionDt = BusinessLayer.GetvPrescriptionReturnOrderDtList(filterExpression);
            lvwPrescription.DataSource = lstPrescriptionDt;
            lvwPrescription.DataBind();

            txtTotal.Text = lstPrescriptionDt.Sum(x => x.LineAmount).ToString("N");
            txtTotalPatient.Text = lstPrescriptionDt.Sum(x => x.PatientAmount).ToString("N");
            txtTotalPayer.Text = lstPrescriptionDt.Sum(x => x.PayerAmount).ToString("N");
        }
    }
}