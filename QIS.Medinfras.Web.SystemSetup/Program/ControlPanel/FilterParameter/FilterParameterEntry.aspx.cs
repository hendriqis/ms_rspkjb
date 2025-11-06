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
    public partial class FilterParameterEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.FILTER_PARAMETER;
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
                SetControlProperties();
                FilterParameter entity = BusinessLayer.GetFilterParameter(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtFilterParameterCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(String.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.FILTER_PARAMETER_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboFilterParameterType, lst, "StandardCodeName", "StandardCodeID");
            cboFilterParameterType.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtFilterParameterCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFilterParameterName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFilterParameterCaption, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboFilterParameterType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFieldName, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(txtMethodName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFilterExpression, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtValueFieldName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTextFieldName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtClientInstanceName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsAllowSelectAll, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtSearchDialogType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSearchDialogNameField, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSearchDialogMethodName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSearchDialogIDField, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSearchDialogFilterExpression, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSearchDialogCodeField, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(txtListText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtListValue, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(txtMinusNYear, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtPlusNYear, new ControlEntrySetting(true, true, true, "0"));
        }

        private void EntityToControl(FilterParameter entity)
        {
            txtFilterParameterCode.Text = entity.FilterParameterCode;
            txtFilterParameterName.Text = entity.FilterParameterName;
            txtFilterParameterCaption.Text = entity.FilterParameterCaption;
            cboFilterParameterType.Value = entity.GCFilterParameterType;
            txtFieldName.Text = entity.FieldName;

            txtMethodName.Text = entity.MethodName;
            txtFilterExpression.Text = entity.FilterExpression;
            txtValueFieldName.Text = entity.ValueFieldName;
            txtTextFieldName.Text = entity.TextFieldName;
            txtClientInstanceName.Text = entity.ClientInstanceName;
            chkIsAllowSelectAll.Checked = entity.IsAllowSelectAll;

            txtSearchDialogCodeField.Text = entity.SearchDialogCodeField;
            txtSearchDialogFilterExpression.Text = entity.SearchDialogFilterExpression;
            txtSearchDialogIDField.Text = entity.SearchDialogIDField;
            txtSearchDialogMethodName.Text = entity.SearchDialogMethodName;
            txtSearchDialogNameField.Text = entity.SearchDialogNameField;
            txtSearchDialogType.Text = entity.SearchDialogType;

            txtListText.Text = entity.ListText;
            txtListValue.Text = entity.ListValue;

            txtPlusNYear.Text = entity.YearPlusNYear.ToString();
            txtMinusNYear.Text = entity.YearMinusNYear.ToString();

            txtCssClass.Text = entity.TxtCssClass;
            txtDefaultValue.Text = entity.DefaultValue;
        }

        private void ControlToEntity(FilterParameter entity)
        {
            entity.FilterParameterCode = txtFilterParameterCode.Text;
            entity.FilterParameterName = txtFilterParameterName.Text;
            entity.FilterParameterCaption = txtFilterParameterCaption.Text;
            entity.GCFilterParameterType = cboFilterParameterType.Value.ToString();
            entity.FieldName = txtFieldName.Text;

            entity.MethodName = txtMethodName.Text;
            entity.FilterExpression = txtFilterExpression.Text;
            entity.ValueFieldName = txtValueFieldName.Text;
            entity.TextFieldName = txtTextFieldName.Text;
            entity.ClientInstanceName = txtClientInstanceName.Text;
            entity.IsAllowSelectAll = chkIsAllowSelectAll.Checked;

            entity.SearchDialogCodeField = txtSearchDialogCodeField.Text;
            entity.SearchDialogFilterExpression = txtSearchDialogFilterExpression.Text;
            entity.SearchDialogIDField = txtSearchDialogIDField.Text;
            entity.SearchDialogMethodName = txtSearchDialogMethodName.Text;
            entity.SearchDialogNameField = txtSearchDialogNameField.Text;
            entity.SearchDialogType = txtSearchDialogType.Text;

            entity.ListText = txtListText.Text;
            entity.ListValue = txtListValue.Text;

            entity.TxtCssClass = txtCssClass.Text;
            entity.DefaultValue = txtDefaultValue.Text;

            if (txtPlusNYear.Text != "")
                entity.YearPlusNYear = Convert.ToByte(txtPlusNYear.Text);
            else
                entity.YearPlusNYear = 0;
            if (txtMinusNYear.Text != "")
                entity.YearMinusNYear = Convert.ToByte(txtMinusNYear.Text);
            else
                entity.YearMinusNYear = 0;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("FilterParameterCode = '{0}'", txtFilterParameterCode.Text);
            List<FilterParameter> lst = BusinessLayer.GetFilterParameterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Filter Parameter With Code " + txtFilterParameterCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("FilterParameterCode = '{0}' AND FilterParameterID != {1}", txtFilterParameterCode.Text, hdnID.Value);
            List<FilterParameter> lst = BusinessLayer.GetFilterParameterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Filter Parameter With Code " + txtFilterParameterCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            FilterParameterDao entityDao = new FilterParameterDao(ctx);
            bool result = false;
            try
            {
                FilterParameter entity = new FilterParameter();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetFilterParameterMaxID(ctx).ToString();
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
                FilterParameter entity = BusinessLayer.GetFilterParameter(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateFilterParameter(entity);
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