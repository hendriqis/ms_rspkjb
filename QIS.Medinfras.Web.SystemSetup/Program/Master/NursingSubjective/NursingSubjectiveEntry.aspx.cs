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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class NursingSubjectiveEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.NURSING_SUBJECTIVE;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                Int32 subjectiveID = Convert.ToInt32(Request.QueryString["id"]);
                hdnID.Value = subjectiveID.ToString();
                NursingSubjective entity = BusinessLayer.GetNursingSubjective(Convert.ToInt32(subjectiveID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtSubjectiveCode.Focus();
        }
       
        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtSubjectiveCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSubjectiveText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsEditableByUser, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(NursingSubjective entity)
        {
            txtSubjectiveCode.Text = entity.SubjectiveCode;
            txtSubjectiveText.Text = entity.SubjectiveText;
            chkIsEditableByUser.Checked = entity.IsEditableByUser;
        }

        private void ControlToEntity(NursingSubjective entity)
        {
            entity.SubjectiveCode = txtSubjectiveCode.Text;
            entity.SubjectiveText = txtSubjectiveText.Text;
            entity.IsEditableByUser = chkIsEditableByUser.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("SubjectiveCode = '{0}'", txtSubjectiveCode.Text);
            List<NursingSubjective> lst = BusinessLayer.GetNursingSubjectiveList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Nurse Subjective with Code " + txtSubjectiveCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }
        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 SubjectiveID = Convert.ToInt32(hdnID.Value);

            string FilterExpression = string.Format("SubjectiveCode = '{0}' AND SubjectiveID != {1}", txtSubjectiveCode.Text, SubjectiveID);
            List<NursingSubjective> lst = BusinessLayer.GetNursingSubjectiveList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Nurse Subjective with Code " + txtSubjectiveCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            NursingSubjectiveDao entityDao = new NursingSubjectiveDao(ctx);
            bool result = false;
            try
            {
                NursingSubjective entity = new NursingSubjective();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetNursingSubjectiveMaxID(ctx).ToString();
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
                NursingSubjective entity = BusinessLayer.GetNursingSubjective(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNursingSubjective(entity);
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