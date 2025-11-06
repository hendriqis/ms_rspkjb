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
    public partial class PatientBillSummaryGenerateBillDtPrescriptionCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            Int32 PrescriptionOrderID = Convert.ToInt32(param);
            vPrescriptionOrderHd entity = BusinessLayer.GetvPrescriptionOrderHdList(string.Format("PrescriptionOrderID = {0}", PrescriptionOrderID)).FirstOrDefault();

            txtTransactionNo.Text = entity.ChargesTransactionNo;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtTransactionDate.Text = entity.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = entity.PrescriptionTime;

            BindGrid(PrescriptionOrderID);
        }

        public void BindGrid(Int32 PrescriptionOrderID) 
        {
            String filterExpression = String.Format("PrescriptionOrderID = {0} AND ID IS NOT NULL AND IsDeleted = 0", PrescriptionOrderID);
            List<vPrescriptionOrderDtCustom> lstPrescriptionDt = BusinessLayer.GetvPrescriptionOrderDtCustomList(filterExpression);
            lvwPrescription.DataSource = lstPrescriptionDt;
            lvwPrescription.DataBind();

            txtTotal.Text = lstPrescriptionDt.Sum(x => x.LineAmount).ToString("N");
            txtTotalPatient.Text = lstPrescriptionDt.Sum(x => x.PatientAmount).ToString("N");
            txtTotalPayer.Text = lstPrescriptionDt.Sum(x => x.PayerAmount).ToString("N");
        }
    }
}