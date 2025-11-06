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
    public partial class PatientBillDiscountDtCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            Int32 transactionID = Convert.ToInt32(param);
            PatientBill chargesHd = BusinessLayer.GetPatientBill(transactionID);
            txtBillingNo.Text = chargesHd.PatientBillingNo;
            txtBillingDate.Text = chargesHd.BillingDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtBillingTime.Text = chargesHd.BillingTime;

            BindGrid(transactionID);
        }

        private void BindGrid(int transactionID)
        {
            List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(string.Format("PatientBillingID = {0} AND IsDeleted = 0", transactionID));

            List<vPatientChargesDt8> lstService = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE).ToList();
            ((TransactionDtServiceViewCtl)ctlService).BindGrid(lstService);

            List<vPatientChargesDt8> lstDrugMS = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            ((TransactionDtProductViewCtl)ctlDrugMS).BindGrid(lstDrugMS);

            List<vPatientChargesDt8> lstLogistic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            ((TransactionDtProductViewCtl)ctlLogistic).BindGrid(lstLogistic);

            List<vPatientChargesDt8> lstLaboratory = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LABORATORY).ToList();
            ((TransactionDtServiceViewCtl)ctlLaboratory).BindGrid(lstLaboratory);

            List<vPatientChargesDt8> lstImaging = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY).ToList();
            ((TransactionDtServiceViewCtl)ctlImaging).BindGrid(lstImaging);

            List<vPatientChargesDt8> lstDiagnostic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            ((TransactionDtServiceViewCtl)ctlLogistic).BindGrid(lstDiagnostic);

            txtTotalPayer.Text = lst.Sum(p => p.PayerAmount).ToString("N");
            txtTotalPatient.Text = lst.Sum(p => p.PatientAmount).ToString("N");
            txtTotal.Text = lst.Sum(p => p.LineAmount).ToString("N");
        }
    }
}