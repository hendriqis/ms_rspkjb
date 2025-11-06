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
    public partial class PatientBillSummaryGenerateBillDtCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            Int32 transactionID = Convert.ToInt32(param);
            PatientChargesHd chargesHd = BusinessLayer.GetPatientChargesHd(transactionID);
            txtTransactionNo.Text = chargesHd.TransactionNo;
            txtReferenceNo.Text = chargesHd.ReferenceNo;
            txtTransactionDate.Text = chargesHd.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = chargesHd.TransactionTime;

            BindGrid(transactionID, chargesHd.TransactionCode);
        }

        private void BindGrid(int transactionID, string transactionCode)
        {
            List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(string.Format("TransactionID = {0} AND IsDeleted = 0", transactionID));

            //string itemType = "";
            ///if (transactionCode == Constant.TransactionCode.IMAGING_CHARGES)
            //    itemType = Constant.ItemGroupMaster.RADIOLOGY;
            //else if (transactionCode == Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES)
            //    itemType = Constant.ItemGroupMaster.DIAGNOSTIC;
            //else
            //    itemType = Constant.ItemGroupMaster.SERVICE;
            List<vPatientChargesDt8> lstService = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE || p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY || p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            ((TransactionDtServiceViewCtl)ctlService).BindGrid(lstService);

            List<vPatientChargesDt8> lstDrugMS = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            ((TransactionDtProductViewCtl)ctlDrugMS).BindGrid(lstDrugMS);

            List<vPatientChargesDt8> lstLogistic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            ((TransactionDtProductViewCtl)ctlLogistic).BindGrid(lstLogistic);

            txtTotalPayer.Text = lst.Sum(p => p.PayerAmount).ToString("N");
            txtTotalPatient.Text = lst.Sum(p => p.PatientAmount).ToString("N");
            txtTotal.Text = lst.Sum(p => p.LineAmount).ToString("N");
        }
    }
}