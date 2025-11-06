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
    public partial class NursingObjectiveEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.NURSING_OBJECTIVE;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                Int32 ObjectiveID = Convert.ToInt32(Request.QueryString["id"]);
                hdnID.Value = ObjectiveID.ToString();
                SetControlProperties();
                NursingObjective entity = BusinessLayer.GetNursingObjective(Convert.ToInt32(ObjectiveID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
                SetControlProperties();
            }
            txtObjectiveCode.Focus();
        }
        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.OBJECTIVE_DATA_SOURCE);
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboGCObjectiveDataSource, lst, "StandardCodeName", "StandardCodeID");
            cboGCObjectiveDataSource.SelectedIndex = 0;
        }
        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObjectiveCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtObjectiveText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCObjectiveDataSource, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsEditableByUser, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(NursingObjective entity)
        {
            txtObjectiveCode.Text = entity.ObjectiveCode;
            txtObjectiveText.Text = entity.ObjectiveText;
            cboGCObjectiveDataSource.Value = entity.GCObjectiveDataSource;
            chkIsEditableByUser.Checked = entity.IsEditableByUser;
        }

        private void ControlToEntity(NursingObjective entity)
        {
            entity.ObjectiveCode = txtObjectiveCode.Text;
            entity.ObjectiveText = txtObjectiveText.Text;
            entity.GCObjectiveDataSource = cboGCObjectiveDataSource.Value.ToString();
            entity.IsEditableByUser = chkIsEditableByUser.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ObjectiveCode = '{0}'", txtObjectiveCode.Text);
            List<NursingObjective> lst = BusinessLayer.GetNursingObjectiveList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Objective Text with Code " + txtObjectiveCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }
        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 ObjectiveID = Convert.ToInt32(hdnID.Value);

            string FilterExpression = string.Format("ObjectiveCode = '{0}' AND ObjectiveID != {1}", txtObjectiveCode.Text, ObjectiveID);
            List<NursingObjective> lst = BusinessLayer.GetNursingObjectiveList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Objective Text with Code " + txtObjectiveCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            NursingObjectiveDao entityDao = new NursingObjectiveDao(ctx);
            bool result = false;
            try
            {
                NursingObjective entity = new NursingObjective();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetNursingObjectiveMaxID(ctx).ToString();
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
                NursingObjective entity = BusinessLayer.GetNursingObjective(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNursingObjective(entity);
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