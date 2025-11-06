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
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryRecalculationBillMDProcessCtl : BasePatientManagementRecalculationBillPage
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                IsAdd = false;
                hdnRegistrationID.Value = param;
                vRegistration registration = BusinessLayer.GetvRegistrationList(String.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
                hdnLinkedRegistrationID.Value = registration.LinkedRegistrationID.ToString();
                ListPatientChargesDt = BusinessLayer.GetvPatientChargesDt8List(string.Format("RegistrationID = {0} AND GCTransactionStatus IN ('{1}','{2}') AND IsDeleted = 0", param, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL));
                BindGrid();
            }
        }
    
        protected override void BindGrid()
        {
            string itemType = "";
            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                itemType = Constant.ItemGroupMaster.RADIOLOGY;
            else
                itemType = Constant.ItemGroupMaster.DIAGNOSTIC;

            List<vPatientChargesDt8> lstService = ListPatientChargesDt.Where(p => p.GCItemType == itemType).ToList();
            ((TransactionDtServiceViewCtl)ctlService).BindGrid(lstService);

            List<vPatientChargesDt8> lstDrugMS = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            ((TransactionDtProductViewCtl)ctlDrugMS).BindGrid(lstDrugMS);

            List<vPatientChargesDt8> lstLogistic = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            ((TransactionDtProductViewCtl)ctlLogistic).BindGrid(lstLogistic);

            txtTotalPayer.Text = ListPatientChargesDt.Sum(p => p.PayerAmount).ToString("N");
            txtTotalPatient.Text = ListPatientChargesDt.Sum(p => p.PatientAmount).ToString("N");
            txtTotal.Text = ListPatientChargesDt.Sum(p => p.LineAmount).ToString("N");
        }

        protected void cbpRecalculationPatientBillProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
            if (hdnParam.Value != "")
            {
                string[] listParam = hdnParam.Value.Split('|');
                int[] listParamTemp = Array.ConvertAll(listParam, int.Parse);
                //OnProcessRecalculation(registrationID, chkIsIncludeVariableTariff.Checked, chkIsResetItemTariff.Checked, listParamTemp);
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);

            if (hdnParam.Value != "")
            {
                string[] listParam = hdnParam.Value.Split('|');
                int[] listParamTemp = Array.ConvertAll(listParam, int.Parse);

                return OnSaveRecord(ref errMessage, ref retval, registrationID, Convert.ToInt32(hdnLinkedRegistrationID.Value), listParamTemp);
            }
            else
            {
                int[] i = new int[0];
                return OnSaveRecord(ref errMessage, ref retval, registrationID, Convert.ToInt32(hdnLinkedRegistrationID.Value), i);
            }
        }
    }
}