using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxEditors;
using System.Reflection;
using System.Collections;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLSupplierLineEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_SUPPLIER_LINE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            String[] param = Request.QueryString["id"].Split('|');
            if (param[0] == "edit")
            {
                IsAdd = false;
                String ID = param[1];
                hdnID.Value = ID;
                SupplierLine entity = BusinessLayer.GetSupplierLineList(String.Format("SupplierLineID = {0}", hdnID.Value))[0];
                vSupplierLineDt entityDt = BusinessLayer.GetvSupplierLineDtList(String.Format("SupplierLineID = {0}", entity.SupplierLineID))[0];

                SetControlProperties();
                EntityToControl(entity, entityDt);
                hdnGCSupplierType.Value = entity.GCSupplierType;
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
                hdnGCSupplierType.Value = param[1];
            }

            txtSupplierLineCode.Focus();
        }

        protected override void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtSupplierLineCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtSupplierLineName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));

            #region Pengaturan Perkiraan
            SetControlEntrySetting(hdnAPID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAPSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAPSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAPGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAPGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblAPSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnAPSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAPSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAPSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnAPInProcessID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAPInProcessSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAPInProcessSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAPInProcessGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAPInProcessGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblAPInProcessSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnAPInProcessSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAPInProcessSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAPInProcessSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnAPDiscountID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAPDiscountSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAPDiscountSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAPDiscountGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAPDiscountGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblAPDiscountSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnAPDiscountSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAPDiscountSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAPDiscountSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnAPStampID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAPStampSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAPStampSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAPStampGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAPStampGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblAPStampSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnAPStampSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAPStampSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAPStampSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnAPDownPaymentID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAPDownPaymentSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAPDownPaymentSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAPDownPaymentGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAPDownPaymentGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblAPDownPaymentSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnAPDownPaymentSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAPDownPaymentSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAPDownPaymentSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnAPChargeID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAPChargeSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAPChargeSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAPChargeGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAPChargeGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblAPChargeSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnAPChargeSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAPChargeSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAPChargeSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnAPRoundingID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAPRoundingSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAPRoundingSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAPRoundingGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAPRoundingGLAccountName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnARPurchaseReturnID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnARPurchaseReturnSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnARPurchaseReturnSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtARPurchaseReturnGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtARPurchaseReturnGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblARPurchaseReturnSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnARPurchaseReturnSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtARPurchaseReturnSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtARPurchaseReturnSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnARCreditNoteID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnARCreditNoteSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnARCreditNoteSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtARCreditNoteGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtARCreditNoteGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblARCreditNoteSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnARCreditNoteSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtARCreditNoteSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtARCreditNoteSubLedgerName, new ControlEntrySetting(false, false, false));
            #endregion
        }

        private void EntityToControl(SupplierLine entity, vSupplierLineDt entityDt)
        {
            txtSupplierLineCode.Text = entity.SupplierLineCode;
            txtSupplierLineName.Text = entity.SupplierLineName;
            txtRemarks.Text = entity.Remarks;

            #region Pengaturan Perkiraan
            #region AP
            hdnAPID.Value = entityDt.AP.ToString();
            txtAPGLAccountNo.Text = entityDt.APGLAccountNo;
            txtAPGLAccountName.Text = entityDt.APGLAccountName;
            hdnAPSubLedgerID.Value = entityDt.APSubLedgerID.ToString();
            hdnAPSearchDialogTypeName.Value = entityDt.APSearchDialogTypeName;
            hdnAPIDFieldName.Value = entityDt.APIDFieldName;
            hdnAPCodeFieldName.Value = entityDt.APCodeFieldName;
            hdnAPDisplayFieldName.Value = entityDt.APDisplayFieldName;
            hdnAPMethodName.Value = entityDt.APMethodName;
            hdnAPFilterExpression.Value = entityDt.APFilterExpression;

            hdnAPSubLedger.Value = entityDt.APSubLedger.ToString();
            txtAPSubLedgerCode.Text = entityDt.APSubLedgerCode.ToString();
            txtAPSubLedgerName.Text = entityDt.APSubLedgerName.ToString();
            #endregion

            #region APInProcess
            hdnAPInProcessID.Value = entityDt.APInProcess.ToString();
            txtAPInProcessGLAccountNo.Text = entityDt.APInProcessGLAccountNo;
            txtAPInProcessGLAccountName.Text = entityDt.APInProcessGLAccountName;
            hdnAPInProcessSubLedgerID.Value = entityDt.APInProcessSubLedgerID.ToString();
            hdnAPInProcessSearchDialogTypeName.Value = entityDt.APInProcessSearchDialogTypeName;
            hdnAPInProcessIDFieldName.Value = entityDt.APInProcessIDFieldName;
            hdnAPInProcessCodeFieldName.Value = entityDt.APInProcessCodeFieldName;
            hdnAPInProcessDisplayFieldName.Value = entityDt.APInProcessDisplayFieldName;
            hdnAPInProcessMethodName.Value = entityDt.APInProcessMethodName;
            hdnAPInProcessFilterExpression.Value = entityDt.APInProcessFilterExpression;

            hdnAPInProcessSubLedger.Value = entityDt.APInProcessSubLedger.ToString();
            txtAPInProcessSubLedgerCode.Text = entityDt.APInProcessSubLedgerCode.ToString();
            txtAPInProcessSubLedgerName.Text = entityDt.APInProcessSubLedgerName.ToString();
            #endregion

            #region APDiscount
            hdnAPDiscountID.Value = entityDt.APDiscount.ToString();
            txtAPDiscountGLAccountNo.Text = entityDt.APDiscountGLAccountNo;
            txtAPDiscountGLAccountName.Text = entityDt.APDiscountGLAccountName;
            hdnAPDiscountSubLedgerID.Value = entityDt.APDiscountSubLedgerID.ToString();
            hdnAPDiscountSearchDialogTypeName.Value = entityDt.APDiscountSearchDialogTypeName;
            hdnAPDiscountIDFieldName.Value = entityDt.APDiscountIDFieldName;
            hdnAPDiscountCodeFieldName.Value = entityDt.APDiscountCodeFieldName;
            hdnAPDiscountDisplayFieldName.Value = entityDt.APDiscountDisplayFieldName;
            hdnAPDiscountMethodName.Value = entityDt.APDiscountMethodName;
            hdnAPDiscountFilterExpression.Value = entityDt.APDiscountFilterExpression;

            hdnAPDiscountSubLedger.Value = entityDt.APDiscountSubLedger.ToString();
            txtAPDiscountSubLedgerCode.Text = entityDt.APDiscountSubLedgerCode.ToString();
            txtAPDiscountSubLedgerName.Text = entityDt.APDiscountSubLedgerName.ToString();
            #endregion

            #region APStamp
            hdnAPStampID.Value = entityDt.APStamp.ToString();
            txtAPStampGLAccountNo.Text = entityDt.APStampGLAccountNo;
            txtAPStampGLAccountName.Text = entityDt.APStampGLAccountName;
            hdnAPStampSubLedgerID.Value = entityDt.APStampSubLedgerID.ToString();
            hdnAPStampSearchDialogTypeName.Value = entityDt.APStampSearchDialogTypeName;
            hdnAPStampIDFieldName.Value = entityDt.APStampIDFieldName;
            hdnAPStampCodeFieldName.Value = entityDt.APStampCodeFieldName;
            hdnAPStampDisplayFieldName.Value = entityDt.APStampDisplayFieldName;
            hdnAPStampMethodName.Value = entityDt.APStampMethodName;
            hdnAPStampFilterExpression.Value = entityDt.APStampFilterExpression;

            hdnAPStampSubLedger.Value = entityDt.APStampSubLedger.ToString();
            txtAPStampSubLedgerCode.Text = entityDt.APStampSubLedgerCode.ToString();
            txtAPStampSubLedgerName.Text = entityDt.APStampSubLedgerName.ToString();
            #endregion

            #region APDownPayment
            hdnAPDownPaymentID.Value = entityDt.APDownPayment.ToString();
            txtAPDownPaymentGLAccountNo.Text = entityDt.APDownPaymentGLAccountNo;
            txtAPDownPaymentGLAccountName.Text = entityDt.APDownPaymentGLAccountName;
            hdnAPDownPaymentSubLedgerID.Value = entityDt.APDownPaymentSubLedgerID.ToString();
            hdnAPDownPaymentSearchDialogTypeName.Value = entityDt.APDownPaymentSearchDialogTypeName;
            hdnAPDownPaymentIDFieldName.Value = entityDt.APDownPaymentIDFieldName;
            hdnAPDownPaymentCodeFieldName.Value = entityDt.APDownPaymentCodeFieldName;
            hdnAPDownPaymentDisplayFieldName.Value = entityDt.APDownPaymentDisplayFieldName;
            hdnAPDownPaymentMethodName.Value = entityDt.APDownPaymentMethodName;
            hdnAPDownPaymentFilterExpression.Value = entityDt.APDownPaymentFilterExpression;

            hdnAPDownPaymentSubLedger.Value = entityDt.APDownPaymentSubLedger.ToString();
            txtAPDownPaymentSubLedgerCode.Text = entityDt.APDownPaymentSubLedgerCode.ToString();
            txtAPDownPaymentSubLedgerName.Text = entityDt.APDownPaymentSubLedgerName.ToString();
            #endregion

            #region APCharge
            hdnAPChargeID.Value = entityDt.APCharge.ToString();
            txtAPChargeGLAccountNo.Text = entityDt.APChargeGLAccountNo;
            txtAPChargeGLAccountName.Text = entityDt.APChargeGLAccountName;
            hdnAPChargeSubLedgerID.Value = entityDt.APChargeSubLedgerID.ToString();
            hdnAPChargeSearchDialogTypeName.Value = entityDt.APChargeSearchDialogTypeName;
            hdnAPChargeIDFieldName.Value = entityDt.APChargeIDFieldName;
            hdnAPChargeCodeFieldName.Value = entityDt.APChargeCodeFieldName;
            hdnAPChargeDisplayFieldName.Value = entityDt.APChargeDisplayFieldName;
            hdnAPChargeMethodName.Value = entityDt.APChargeMethodName;
            hdnAPChargeFilterExpression.Value = entityDt.APChargeFilterExpression;

            hdnAPChargeSubLedger.Value = entityDt.APChargeSubLedger.ToString();
            txtAPChargeSubLedgerCode.Text = entityDt.APChargeSubLedgerCode.ToString();
            txtAPChargeSubLedgerName.Text = entityDt.APChargeSubLedgerName.ToString();
            #endregion

            #region APRounding
            hdnAPRoundingID.Value = entityDt.APRounding.ToString();
            txtAPRoundingGLAccountNo.Text = entityDt.APRoundingGLAccountNo;
            txtAPRoundingGLAccountName.Text = entityDt.APRoundingGLAccountName;
            hdnAPRoundingSubLedgerID.Value = entityDt.APRoundingSubLedgerID.ToString();
            hdnAPRoundingSearchDialogTypeName.Value = entityDt.APRoundingSearchDialogTypeName;
            hdnAPRoundingIDFieldName.Value = entityDt.APRoundingIDFieldName;
            hdnAPRoundingCodeFieldName.Value = entityDt.APRoundingCodeFieldName;
            hdnAPRoundingDisplayFieldName.Value = entityDt.APRoundingDisplayFieldName;
            hdnAPRoundingMethodName.Value = entityDt.APRoundingMethodName;
            hdnAPRoundingFilterExpression.Value = entityDt.APRoundingFilterExpression;
            #endregion

            #region ARPurchaseReturn
            hdnARPurchaseReturnID.Value = entityDt.ARPurchaseReturn.ToString();
            txtARPurchaseReturnGLAccountNo.Text = entityDt.ARPurchaseReturnGLAccountNo;
            txtARPurchaseReturnGLAccountName.Text = entityDt.ARPurchaseReturnGLAccountName;
            hdnARPurchaseReturnSubLedgerID.Value = entityDt.ARPurchaseReturnSubLedgerID.ToString();
            hdnARPurchaseReturnSearchDialogTypeName.Value = entityDt.ARPurchaseReturnSearchDialogTypeName;
            hdnARPurchaseReturnIDFieldName.Value = entityDt.ARPurchaseReturnIDFieldName;
            hdnARPurchaseReturnCodeFieldName.Value = entityDt.ARPurchaseReturnCodeFieldName;
            hdnARPurchaseReturnDisplayFieldName.Value = entityDt.ARPurchaseReturnDisplayFieldName;
            hdnARPurchaseReturnMethodName.Value = entityDt.ARPurchaseReturnMethodName;
            hdnARPurchaseReturnFilterExpression.Value = entityDt.ARPurchaseReturnFilterExpression;

            hdnARPurchaseReturnSubLedger.Value = entityDt.ARPurchaseReturnSubLedger.ToString();
            txtARPurchaseReturnSubLedgerCode.Text = entityDt.ARPurchaseReturnSubLedgerCode.ToString();
            txtARPurchaseReturnSubLedgerName.Text = entityDt.ARPurchaseReturnSubLedgerName.ToString();
            #endregion

            #region ARCreditNote
            hdnARCreditNoteID.Value = entityDt.ARCreditNote.ToString();
            txtARCreditNoteGLAccountNo.Text = entityDt.ARCreditNoteGLAccountNo;
            txtARCreditNoteGLAccountName.Text = entityDt.ARCreditNoteGLAccountName;
            hdnARCreditNoteSubLedgerID.Value = entityDt.ARCreditNoteSubLedgerID.ToString();
            hdnARCreditNoteSearchDialogTypeName.Value = entityDt.ARCreditNoteSearchDialogTypeName;
            hdnARCreditNoteIDFieldName.Value = entityDt.ARCreditNoteIDFieldName;
            hdnARCreditNoteCodeFieldName.Value = entityDt.ARCreditNoteCodeFieldName;
            hdnARCreditNoteDisplayFieldName.Value = entityDt.ARCreditNoteDisplayFieldName;
            hdnARCreditNoteMethodName.Value = entityDt.ARCreditNoteMethodName;
            hdnARCreditNoteFilterExpression.Value = entityDt.ARCreditNoteFilterExpression;

            hdnARCreditNoteSubLedger.Value = entityDt.ARCreditNoteSubLedger.ToString();
            txtARCreditNoteSubLedgerCode.Text = entityDt.ARCreditNoteSubLedgerCode.ToString();
            txtARCreditNoteSubLedgerName.Text = entityDt.ARCreditNoteSubLedgerName.ToString();
            #endregion
            #endregion
        }

        private void ControlToEntity(SupplierLine entity, SupplierLineDt entityDt)
        {
            entity.SupplierLineCode = txtSupplierLineCode.Text;
            entity.SupplierLineName = txtSupplierLineName.Text;
            entity.Remarks = txtRemarks.Text;

            entityDt.HealthcareID = AppSession.UserLogin.HealthcareID;

            #region Pengaturan Perkiraan
            #region AP
            if (hdnAPID.Value != "" && hdnAPID.Value != "0")
                entityDt.AP = Convert.ToInt32(hdnAPID.Value);
            else
                entityDt.AP = null;
            if (hdnAPSubLedger.Value != "" && hdnAPSubLedger.Value != "0")
                entityDt.APSubLedger= Convert.ToInt32(hdnAPSubLedger.Value);
            else
                entityDt.APSubLedger = null;
            #endregion
            
            #region APInProcess
            if (hdnAPInProcessID.Value != "" && hdnAPInProcessID.Value != "0")
                entityDt.APInProcess = Convert.ToInt32(hdnAPInProcessID.Value);
            else
                entityDt.APInProcess = null;
            if (hdnAPInProcessSubLedger.Value != "" && hdnAPInProcessSubLedger.Value != "0")
                entityDt.APInProcessSubLedger= Convert.ToInt32(hdnAPInProcessSubLedger.Value);
            else
                entityDt.APInProcessSubLedger = null;
            #endregion

            #region APDiscount
            if (hdnAPDiscountID.Value != "" && hdnAPDiscountID.Value != "0")
                entityDt.APDiscount = Convert.ToInt32(hdnAPDiscountID.Value);
            else
                entityDt.APDiscount = null;
            if (hdnAPDiscountSubLedger.Value != "" && hdnAPDiscountSubLedger.Value != "0")
                entityDt.APDiscountSubLedger = Convert.ToInt32(hdnAPDiscountSubLedger.Value);
            else
                entityDt.APDiscountSubLedger = null;
            #endregion

            #region APStamp
            if (hdnAPStampID.Value != "" && hdnAPStampID.Value != "0")
                entityDt.APStamp = Convert.ToInt32(hdnAPStampID.Value);
            else
                entityDt.APStamp = null;
            if (hdnAPStampSubLedger.Value != "" && hdnAPStampSubLedger.Value != "0")
                entityDt.APStampSubLedger = Convert.ToInt32(hdnAPStampSubLedger.Value);
            else
                entityDt.APStampSubLedger = null;
            #endregion

            #region APDownPayment
            if (hdnAPDownPaymentID.Value != "" && hdnAPDownPaymentID.Value != "0")
                entityDt.APDownPayment = Convert.ToInt32(hdnAPDownPaymentID.Value);
            else
                entityDt.APDownPayment = null;
            if (hdnAPDownPaymentSubLedger.Value != "" && hdnAPDownPaymentSubLedger.Value != "0")
                entityDt.APDownPaymentSubLedger = Convert.ToInt32(hdnAPDownPaymentSubLedger.Value);
            else
                entityDt.APDownPaymentSubLedger = null;
            #endregion

            #region APCharge
            if (hdnAPChargeID.Value != "" && hdnAPChargeID.Value != "0")
                entityDt.APCharge = Convert.ToInt32(hdnAPChargeID.Value);
            else
                entityDt.APCharge = null;
            if (hdnAPChargeSubLedger.Value != "" && hdnAPChargeSubLedger.Value != "0")
                entityDt.APChargeSubLedger = Convert.ToInt32(hdnAPChargeSubLedger.Value);
            else
                entityDt.APChargeSubLedger = null;
            #endregion

            #region APRounding
            if (hdnAPRoundingID.Value != "" && hdnAPRoundingID.Value != "0")
                entityDt.APRounding = Convert.ToInt32(hdnAPRoundingID.Value);
            else
                entityDt.APRounding = null;
            #endregion

            #region ARPurchaseReturn
            if (hdnARPurchaseReturnID.Value != "" && hdnARPurchaseReturnID.Value != "0")
                entityDt.ARPurchaseReturn = Convert.ToInt32(hdnARPurchaseReturnID.Value);
            else
                entityDt.ARPurchaseReturn = null;
            if (hdnARPurchaseReturnSubLedger.Value != "" && hdnARPurchaseReturnSubLedger.Value != "0")
                entityDt.ARPurchaseReturnSubLedger = Convert.ToInt32(hdnARPurchaseReturnSubLedger.Value);
            else
                entityDt.ARPurchaseReturnSubLedger = null;
            #endregion

            #region ARCreditNote
            if (hdnARCreditNoteID.Value != "" && hdnARCreditNoteID.Value != "0")
                entityDt.ARCreditNote = Convert.ToInt32(hdnARCreditNoteID.Value);
            else
                entityDt.ARCreditNote = null;
            if (hdnARCreditNoteSubLedger.Value != "" && hdnARCreditNoteSubLedger.Value != "0")
                entityDt.ARCreditNoteSubLedger = Convert.ToInt32(hdnARCreditNoteSubLedger.Value);
            else
                entityDt.ARCreditNoteSubLedger = null;
            #endregion
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            SupplierLineDao supplierLineDao = new SupplierLineDao(ctx);
            SupplierLineDtDao supplierLineDtDao = new SupplierLineDtDao(ctx);
            bool result = true;
            try
            {
                SupplierLine entity = new SupplierLine();
                SupplierLineDt entityDt = new SupplierLineDt();
                ControlToEntity(entity, entityDt);

                entity.GCSupplierType = hdnGCSupplierType.Value;
                entityDt.CreatedBy = entity.CreatedBy = AppSession.UserLogin.UserID;
                int oSupplierLineID = supplierLineDao.InsertReturnPrimaryKeyID(entity);

                entityDt.SupplierLineID = oSupplierLineID;
                supplierLineDtDao.Insert(entityDt);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally 
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            SupplierLineDao supplierLineDao = new SupplierLineDao(ctx);
            SupplierLineDtDao supplierLineDtDao = new SupplierLineDtDao(ctx);
            bool result = true;
            try
            {
                SupplierLine entity = supplierLineDao.Get(Convert.ToInt32(hdnID.Value));
                SupplierLineDt entityDt = supplierLineDtDao.Get(entity.SupplierLineID, AppSession.UserLogin.HealthcareID);
                ControlToEntity(entity, entityDt);
                entityDt.LastUpdatedBy = entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                supplierLineDao.Update(entity);
                supplierLineDtDao.Update(entityDt);
                
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally 
            {
                ctx.Close();
            }
            return result;
        }
    }
}