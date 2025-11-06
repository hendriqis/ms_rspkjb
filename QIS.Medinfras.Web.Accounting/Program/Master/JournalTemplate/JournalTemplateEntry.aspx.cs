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

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class JournalTemplateEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.JOURNAL_TEMPLATE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                JournalTemplateHd entity = BusinessLayer.GetJournalTemplateHd(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList("IsDeleted = 0");
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboJournalTemplateType, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.JOURNAL_TEMPLATE_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            txtTemplateCode.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTemplateCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTemplateName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboJournalTemplateType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(JournalTemplateHd entity)
        {
            txtTemplateCode.Text = entity.TemplateCode;
            txtTemplateName.Text = entity.TemplateName;
            cboJournalTemplateType.Value = entity.GCJournalTemplateType;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(JournalTemplateHd entity)
        {
            entity.TemplateCode = txtTemplateCode.Text;
            entity.TemplateName = txtTemplateName.Text;
            if (cboJournalTemplateType.Value != null && cboJournalTemplateType.Value.ToString() != "")
                entity.GCJournalTemplateType = cboJournalTemplateType.Value.ToString();
            else
                entity.GCJournalTemplateType = null;
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("TemplateCode = '{0}'", txtTemplateCode.Text);
            List<JournalTemplateHd> lst = BusinessLayer.GetJournalTemplateHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Template With Code " + txtTemplateCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("TemplateCode = '{0}' AND TemplateID != {1}", txtTemplateCode.Text, hdnID.Value);
            List<JournalTemplateHd> lst = BusinessLayer.GetJournalTemplateHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Template With Code " + txtTemplateCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            JournalTemplateHdDao entityDao = new JournalTemplateHdDao(ctx);
            bool result = false;
            try
            {
                JournalTemplateHd entity = new JournalTemplateHd();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetJournalTemplateHdMaxID(ctx).ToString();
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
                JournalTemplateHd entity = BusinessLayer.GetJournalTemplateHd(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateJournalTemplateHd(entity);
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