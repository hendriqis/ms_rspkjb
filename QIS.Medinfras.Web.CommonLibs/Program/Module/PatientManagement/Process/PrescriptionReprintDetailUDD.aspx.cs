using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PrescriptionReprintDetailUDD : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.PRESCRIPTION_REPRINT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        protected override void InitializeDataControl()
        {
            String filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PRESCRIPTION_TYPE, Constant.StandardCode.TIPE_TRANSAKSI_BPJS);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            List<StandardCode> lstPrescriptionType = lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstPrescriptionType, "StandardCodeName", "StandardCodeID");
            cboPrescriptionType.Enabled = false;

            List<StandardCode> lstTipeTransaksiBPJS = lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.TIPE_TRANSAKSI_BPJS).ToList();
            Methods.SetComboBoxField<StandardCode>(cboBPJSTransType, lstTipeTransaksiBPJS, "StandardCodeName", "StandardCodeID");
            cboBPJSTransType.Enabled = false;

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            string[] param = Page.Request.QueryString["id"].Split('|');

            string transactionNo = string.Empty;
            if (param[0] == "to")
            {
                    hdnVisitID.Value = param[1];
                    hdnDefaultPrescriptionOrderID.Value = param[2];
                    PrescriptionOrderHd entityHd = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnDefaultPrescriptionOrderID.Value));
                    hdnDispensaryServiceUnitID.Value = entityHd.DispensaryServiceUnitID.ToString();
                    cboPrescriptionType.Value = entityHd.GCPrescriptionType;
                    PatientChargesHd entityPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PrescriptionOrderID = '{0}' AND GCTransactionStatus <> '{1}'", hdnDefaultPrescriptionOrderID.Value, Constant.TransactionStatus.VOID)).FirstOrDefault();
                    if (entityPatientChargesHd != null) transactionNo = entityPatientChargesHd.TransactionNo;
            }
            else
            {
                hdnDispensaryServiceUnitID.Value = param[2];
            }

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[3])).FirstOrDefault();
            hdnVisitID.Value = entityReg.VisitID.ToString();
            hdnRegistrationID.Value = entityReg.RegistrationID.ToString();
            hdnLinkedRegistrationID.Value = entityReg.LinkedRegistrationID.ToString();
            hdnDepartmentID.Value = entityReg.DepartmentID;
            hdnHealthcareServiceUnitID.Value = entityReg.HealthcareServiceUnitID.ToString();

            if (!string.IsNullOrEmpty(transactionNo))
            {
                IsLoadFirstRecord = true;
                filterExpression = GetFilterExpression();
                pageIndexFirstLoad = BusinessLayer.GetvPatientChargesHdRowIndex(filterExpression, transactionNo, "TransactionID DESC");
            }
            else
            {
                if (param[0] != "to") IsLoadFirstRecord = (OnGetRowCount() > 0);
            }
        }

        protected string GetFilterExpression()
        {
            String filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
            else
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            filterExpression += string.Format(" AND PrescriptionOrderID IS NOT NULL");
            return filterExpression;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvPatientChargesHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHd(filterExpression, PageIndex, " TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvPatientChargesHdRowIndex(filterExpression, keyValue, "TransactionID DESC");
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHd(filterExpression, PageIndex, "TransactionID DESC");

            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPatientChargesHd entityCharges, ref bool isShowWatermark, ref string watermarkText)
        {
            vPrescriptionOrderHd entity = BusinessLayer.GetvPrescriptionOrderHdList(string.Format("PrescriptionOrderID = {0}", entityCharges.PrescriptionOrderID))[0];
            if (entityCharges.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entityCharges.TransactionStatusWatermark;
            }
            hdnPrescriptionOrderID.Value = entity.PrescriptionOrderID.ToString();
            hdnTransactionID.Value = entityCharges.TransactionID.ToString();
            txtTransactionNo.Text = entityCharges.TransactionNo;
            txtPrescriptionDate.Text = entityCharges.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPrescriptionTime.Text = entityCharges.TransactionTime;
            hdnGCPrescriptionType.Value = entity.GCPrescriptionType;
            cboPrescriptionType.Value = entity.GCPrescriptionType;
            if (!string.IsNullOrEmpty(entityCharges.GCBPJSTransactionType))
            {
                cboBPJSTransType.Value = entityCharges.GCBPJSTransactionType;
            }
            txtReferenceNo.Text = entityCharges.ReferenceNo;
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            txtPrescriptionOrderInfo.Text = string.Format("{0}|{1}|{2}", entity.PrescriptionOrderNo, entity.LastUpdatedDateInString, entity.CreatedByName);
            txtNotes.Text = entity.Remarks;

            BindGridDetail();
        }

        private void BindGridDetail()
        {
            List<GetPrescriptionPrice> lstEntity = new List<GetPrescriptionPrice>();
            if (hdnPrescriptionOrderID.Value != "" && hdnPrescriptionOrderID.Value != "0" && hdnTransactionID.Value != "0")
            {
                lstEntity = BusinessLayer.GetPrescriptionPrice(Convert.ToInt32(hdnTransactionID.Value), Convert.ToInt32(hdnPrescriptionOrderID.Value));
            }
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
            if (lstEntity.Count > 0)
            {
                ((HtmlGenericControl)lvwView.FindControl("tdTotalAllPayer")).InnerHtml = lstEntity.Sum(x => x.PayerAmount).ToString("N");
                ((HtmlGenericControl)lvwView.FindControl("tdTotalAllPatient")).InnerHtml = lstEntity.Sum(x => x.PatientAmount).ToString("N");
                ((HtmlGenericControl)lvwView.FindControl("tdTotalAll")).InnerHtml = lstEntity.Sum(x => x.LineAmount).ToString("N");
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }
    }
}