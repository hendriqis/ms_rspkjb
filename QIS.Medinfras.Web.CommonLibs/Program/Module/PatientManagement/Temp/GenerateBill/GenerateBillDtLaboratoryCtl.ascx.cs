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
    public partial class GenerateBillDtLaboratoryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            Int32 transactionID = Convert.ToInt32(param);
            PatientChargesHd chargesHd = BusinessLayer.GetPatientChargesHd(transactionID);
            txtTransactionNo.Text = chargesHd.TransactionNo;
            txtReferenceNo.Text = chargesHd.ReferenceNo;
            txtTransactionDate.Text = chargesHd.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = chargesHd.TransactionTime;

            BindGrid(transactionID);
        }

        private void BindGrid(int transactionID)
        {
            List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(string.Format("TransactionID = {0} AND IsDeleted = 0", transactionID));
            List<vPatientChargesDt8> lstService = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LABORATORY).ToList();
            ((TransactionDtServiceViewCtl)ctlService).BindGrid(lstService);

            txtTotalPayer.Text = lst.Sum(p => p.PayerAmount).ToString("N");
            txtTotalPatient.Text = lst.Sum(p => p.PatientAmount).ToString("N");
            txtTotal.Text = lst.Sum(p => p.LineAmount).ToString("N");
        }
    }
}