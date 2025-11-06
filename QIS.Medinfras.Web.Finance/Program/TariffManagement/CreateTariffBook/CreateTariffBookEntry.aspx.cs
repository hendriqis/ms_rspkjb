using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class CreateTariffBookEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.CREATE_TARIFF_BOOK;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowSaveAndNew, ref bool IsAllowSaveAndClose)
        {
            if (hdnGCTransactionStatus.Value == "" || hdnGCTransactionStatus.Value == Constant.TransactionStatus.OPEN)
                IsAllowSaveAndNew = IsAllowSaveAndClose = true;
            else
                IsAllowSaveAndNew = IsAllowSaveAndClose = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                SetControlProperties();
                TariffBookHd entity = BusinessLayer.GetTariffBookHd(Convert.ToInt32(ID));
                hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
                EntityToControl(entity);
            }
            else
            {
                hdnGCTransactionStatus.Value = "";
                SetControlProperties();
                IsAdd = true;
            }

            if (hdnGCTransactionStatus.Value == "" || hdnGCTransactionStatus.Value == Constant.TransactionStatus.OPEN)
                pnlDocumentSummary.Visible = false;
            else
                txtDocumentSummary.Visible = false;

            cboHealthcare.Focus();
        }

        protected override void SetControlProperties()
        {
            List<Healthcare> lstHealthcare = BusinessLayer.GetHealthcareList("");
            Methods.SetComboBoxField<Healthcare>(cboHealthcare, lstHealthcare, "HealthcareName", "HealthcareID");

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.TARIFF_SCHEME, Constant.StandardCode.ITEM_TYPE));

            Methods.SetComboBoxField<StandardCode>(cboTariffScheme, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.TARIFF_SCHEME).ToList(), "StandardCodeName", "StandardCodeID");

            cboHealthcare.SelectedIndex = 0;
            cboTariffScheme.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboHealthcare, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboTariffScheme, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtEffectiveDate, new ControlEntrySetting(true, true, true, DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtDocumentNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDocumentDate, new ControlEntrySetting(true, true, true, DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtRevisionNo, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtDocumentSummary, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(TariffBookHd entity)
        {
            cboHealthcare.Value = entity.HealthcareID;
            cboTariffScheme.Value = entity.GCTariffScheme;
            txtEffectiveDate.Text = entity.StartingDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDocumentNo.Text = entity.DocumentNo;
            txtDocumentDate.Text = entity.DocumentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRevisionNo.Text = entity.RevisionNo.ToString();
            txtDocumentSummary.Text = entity.DocumentSummary;
            pnlDocumentSummary.InnerHtml = entity.DocumentSummary;
        }

        private void ControlToEntity(TariffBookHd entity)
        {
            entity.HealthcareID = cboHealthcare.Value.ToString();
            entity.GCTariffScheme = cboTariffScheme.Value.ToString();
            entity.StartingDate = Helper.GetDatePickerValue(txtEffectiveDate);
            entity.DocumentNo = txtDocumentNo.Text;
            entity.DocumentDate = Helper.GetDatePickerValue(txtDocumentDate);
            entity.RevisionNo = Byte.Parse(txtRevisionNo.Text);
            entity.DocumentSummary = Helper.GetHTMLEditorText(txtDocumentSummary);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("DocumentNo = '{0}' AND IsDeleted = 0", txtDocumentNo.Text);
            List<TariffBookHd> lst = BusinessLayer.GetTariffBookHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Tariff Book with DocumentNo " + txtDocumentNo.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("DocumentNo = '{0}' AND BookID != {1} AND IsDeleted = 0", txtDocumentNo.Text, hdnID.Value);
            List<TariffBookHd> lst = BusinessLayer.GetTariffBookHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Tariff Book with DocumentNo " + txtDocumentNo.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            TariffBookHdDao entityDao = new TariffBookHdDao(ctx);
            bool result = false;
            try
            {
                TariffBookHd entity = new TariffBookHd();
                ControlToEntity(entity);
                entity.PreparedBy = AppSession.UserLogin.UserID;
                entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetTariffBookHdMaxID(ctx).ToString();
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                if (hdnGCTransactionStatus.Value == "" || hdnGCTransactionStatus.Value == Constant.TransactionStatus.OPEN)
                {
                    TariffBookHd entity = BusinessLayer.GetTariffBookHd(Convert.ToInt32(hdnID.Value));
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateTariffBookHd(entity);
                }
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}