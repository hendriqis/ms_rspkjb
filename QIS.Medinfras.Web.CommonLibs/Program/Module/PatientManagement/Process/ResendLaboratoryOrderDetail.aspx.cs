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
    public partial class ResendLaboratoryOrderDetail : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.RESEND_ORDER;
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
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            string[] param = Page.Request.QueryString["id"].Split('|');
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[3])).FirstOrDefault();

            hdnVisitID.Value = entity.VisitID.ToString();
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();

            string filterSetvar = string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM);
            List<SettingParameterDt> lstsetvarDt = BusinessLayer.GetSettingParameterDtList(filterSetvar);

            hdnHsuImaging.Value = lstsetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
            hdnHsuLaboratory.Value = lstsetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;

            string testOrderID = "";
            if (param[0] == "to")
            {
                testOrderID = param[2];
            }
            else
            {
                testOrderID = "0";
            }

            string filterCh1 = string.Format("TestOrderID = '{0}' AND GCTransactionStatus <> '{1}'", testOrderID, Constant.TransactionStatus.VOID);
            PatientChargesHd entityPatientChargesHd = BusinessLayer.GetPatientChargesHdList(filterCh1).FirstOrDefault();
            if (entityPatientChargesHd != null)
            {
                IsLoadFirstRecord = true;
                string filterCh2 = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1}", hdnVisitID.Value, hdnHsuLaboratory.Value);
                pageIndexFirstLoad = BusinessLayer.GetvPatientChargesHd2RowIndex(filterCh2, entityPatientChargesHd.TransactionNo, "TransactionID DESC");
            }
            else
            {
                if (param[0] != "to") IsLoadFirstRecord = (OnGetRowCount() > 0);
            }

            //BindGridDetail();
        }

        public override int OnGetRowCount()
        {
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1}", hdnVisitID.Value, hdnHsuLaboratory.Value);
            return BusinessLayer.GetvPatientChargesHd2RowCount(filterExpression);
        }

        protected string GetFilterExpression()
        {
            string filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
            else
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            filterExpression += string.Format(" AND GCTransactionStatus != '{0}' AND HealthcareServiceUnitID = {1}", Constant.TransactionStatus.VOID, hdnHsuLaboratory.Value);
            return filterExpression;
        }

        #region Load Entity
        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vPatientChargesHd2 entity = BusinessLayer.GetvPatientChargesHd2(filterExpression, PageIndex, " TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvPatientChargesHd2RowIndex(filterExpression, keyValue, "TransactionID DESC");
            vPatientChargesHd2 entity = BusinessLayer.GetvPatientChargesHd2(filterExpression, PageIndex, "TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPatientChargesHd2 entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }
            hdnTransactionID.Value = entity.TransactionID.ToString();
            txtTransactionNo.Text = entity.TransactionNo;
            txtTransactionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = entity.TransactionTime;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtTestOrderInfo.Text = entity.TestOrderInfo;
            txtProcedureOrderInfo.Text = string.Format("{0} - {1}", entity.ProcedureGroupCode, entity.ProcedureGroupName);
            txtRemarks.Text = entity.Remarks;

            BindGridDetail();
        }
        #endregion

        private void BindGridDetail()
        {
            string filterExpression = string.Format("TransactionID = {0} AND GCItemType IN ('{1}') AND IsDeleted = 0 ORDER BY ID", hdnTransactionID.Value, Constant.ItemGroupMaster.LABORATORY);
            List<vPatientChargesDt1> lst = BusinessLayer.GetvPatientChargesDt1List(filterExpression);
            lvwService.DataSource = lst;
            lvwService.DataBind();

            decimal totalPatientAmount = lst.Sum(p => p.PatientAmount);
            decimal totalPayerAmount = lst.Sum(p => p.PayerAmount);
            decimal totalLineAmount = lst.Sum(p => p.LineAmount);
            if (lst.Count > 0)
            {
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPayer")).InnerHtml = totalPayerAmount.ToString("N");
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPatient")).InnerHtml = totalPatientAmount.ToString("N");
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotal")).InnerHtml = totalLineAmount.ToString("N");
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