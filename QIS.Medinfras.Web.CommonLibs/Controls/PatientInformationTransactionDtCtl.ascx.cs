using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class PatientInformationTransactionDtCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] varParam = param.Split('|');
            hdnType.Value = varParam[1];
            Int32 transactionID = Convert.ToInt32(varParam[0]);
            PatientChargesHd chargesHd = BusinessLayer.GetPatientChargesHd(transactionID);
            txtTransactionNo.Text = chargesHd.TransactionNo;
            txtReferenceNo.Text = chargesHd.ReferenceNo;
            txtTransactionDate.Text = chargesHd.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = chargesHd.TransactionTime;

            BindGrid(transactionID);
        }

        private void BindGrid(int transactionID)
        {
            List<vPatientChargesDt> lst = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", transactionID));

            List<vPatientChargesDt> lstService = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE ||
                                                                p.GCItemType == Constant.ItemGroupMaster.LABORATORY || 
                                                                p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY || 
                                                                p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            ((InformationDtServiceCtl)ctlService).BindGrid(lstService);
            
            List<vPatientChargesDt> lstDrug = lst.Where(p => p.ChargedQuantity > -1 &&
                                                                   (p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES)
                                                             ).ToList();
            ((InformationDtDrugCtl)ctlDrug).BindGrid(lstDrug);

            List<vPatientChargesDt> lstDrugReturn = lst.Where(p => p.ChargedQuantity < 0 &&
                                                                   (p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES)
                                                             ).ToList();
            ((InformationDtDrugReturnCtl)ctlDrugReturn).BindGrid(lstDrugReturn);

            List<vPatientChargesDt> lstLogistic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            ((InformationDtLogisticCtl)ctlLogistic).BindGrid(lstLogistic);

            txtTotalPayer.Text = lst.Sum(p => p.PayerAmount).ToString("N");
            txtTotalPatient.Text = lst.Sum(p => p.PatientAmount).ToString("N");
            txtTotal.Text = lst.Sum(p => p.LineAmount).ToString("N");
        }
    }
}