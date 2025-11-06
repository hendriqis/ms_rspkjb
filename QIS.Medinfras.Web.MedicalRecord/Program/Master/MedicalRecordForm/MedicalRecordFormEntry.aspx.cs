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

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class MedicalRecordFormEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.MEDICAL_RECORD_FORM;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                MedicalRecordForm entity = BusinessLayer.GetMedicalRecordForm(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtFormCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtFormCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFormName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(MedicalRecordForm entity)
        {
            txtFormCode.Text = entity.FormCode;
            txtFormName.Text = entity.FormName;
            txtRemarks.Text = entity.Remarks;
            chkIsGeneratedForm.Checked = entity.IsGeneratedForm;
        }

        private void ControlToEntity(MedicalRecordForm entity)
        {
            entity.FormCode = txtFormCode.Text;
            entity.FormName = txtFormName.Text;
            entity.Remarks = txtRemarks.Text;
            entity.IsGeneratedForm = chkIsGeneratedForm.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("FormCode = '{0}'", txtFormCode.Text);
            List<MedicalRecordForm> lst = BusinessLayer.GetMedicalRecordFormList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Medical Record Form with Code " + txtFormCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            MedicalRecordFormDao entityDao = new MedicalRecordFormDao(ctx);
            bool result = false;
            try
            {
                MedicalRecordForm entity = new MedicalRecordForm();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetMedicalRecordFormMaxID(ctx).ToString();
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
                MedicalRecordForm entity = BusinessLayer.GetMedicalRecordForm(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMedicalRecordForm(entity);
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