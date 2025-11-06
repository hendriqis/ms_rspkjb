using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Tools
{
    public partial class DataMigrationConfigurationColumnMapEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.DATA_MIGRATION_CONFIGURATION;
        }

        protected override void InitializeDataControl()
        {
            String[] param = Request.QueryString["id"].Split('|');
            if (param[0] == "edit")
            {
                IsAdd = false;
                hdnID.Value = param[1];

                MigrationConfigurationDt entity = BusinessLayer.GetMigrationConfigurationDt(Convert.ToInt32(hdnID.Value));
                hdnHeaderID.Value = entity.HeaderID.ToString();
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                hdnHeaderID.Value = param[1];
                IsAdd = true;
                SetControlProperties();
            }
            txtTableName.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTableName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtLinkColumn, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtColumnName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtColumnCaption, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsVisible, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(txtFromColumn, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDefaultValue, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboInputType, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(txtMethodName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtValueField, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTextField, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFilterExpression, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtValueChecked, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtValueUnchecked, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkOtherValue, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtFormatDate, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(txtSearchDialogType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSearchDialogNameField, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSearchDialogMethodName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSearchDialogIDField, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSearchDialogFilterExpression, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSearchDialogCodeField, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(txtIDColumn, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFormatCode, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(MigrationConfigurationDt entity)
        {
            txtTableName.Text = entity.TableName;
            txtColumnName.Text = entity.ColumnName;
            txtLinkColumn.Text = entity.LinkColumn;
            txtFromColumn.Text = entity.FromColumn;
            txtDefaultValue.Text = entity.DefaultValue;
            chkIsVisible.Checked = entity.IsVisible;
            txtColumnCaption.Text = entity.ColumnCaption;
            cboInputType.Value = entity.Type;
            if (entity.IsVisible)
            {
                trColumnDescription.Attributes.Remove("style");
                if (entity.Type == "2")
                {
                    trComboBox.Attributes.Remove("style");
                    txtMethodName.Text = entity.MethodName;
                    txtValueField.Text = entity.ValueField;
                    txtTextField.Text = entity.TextField;
                    txtFilterExpression.Text = entity.FilterExpression;
                }
                else if (entity.Type == "3")
                {
                    trCheckBox.Attributes.Remove("style");
                    txtValueChecked.Text = entity.ValueChecked;
                    txtValueUnchecked.Text = entity.ValueUnchecked;
                    chkOtherValue.Checked = entity.OtherValue;
                }
                else if (entity.Type == "4")
                {
                    trDateEdit.Attributes.Remove("style");
                    txtFormatDate.Text = entity.FormatDate;
                }
                else if (entity.Type == "5")
                {
                    trSearchDialog.Attributes.Remove("style");
                    txtSearchDialogCodeField.Text = entity.SearchDialogCodeField;
                    txtSearchDialogFilterExpression.Text = entity.SearchDialogFilterExpression;
                    txtSearchDialogIDField.Text = entity.SearchDialogIDField;
                    txtSearchDialogMethodName.Text = entity.SearchDialogMethodName;
                    txtSearchDialogNameField.Text = entity.SearchDialogNameField;
                    txtSearchDialogType.Text = entity.SearchDialogType;
                }
                else if (entity.Type == "6")
                {
                    trDateEdit.Attributes.Remove("style");
                    txtIDColumn.Text = entity.IDColumn;
                    txtFormatCode.Text = entity.FormatCode;
                }
            }
            else
            {
                trColumnDescription.Attributes.Add("style", "display:none");
            }
        }

        private void ControlToEntity(MigrationConfigurationDt entity)
        {
            entity.TableName = txtTableName.Text;
            entity.ColumnName = txtColumnName.Text;
            entity.LinkColumn = txtLinkColumn.Text;
            entity.FromColumn = txtFromColumn.Text;
            entity.DefaultValue = txtDefaultValue.Text;
            entity.IsVisible = chkIsVisible.Checked;
            entity.Type = cboInputType.Value.ToString();
            entity.ColumnCaption = txtColumnCaption.Text;

            if (entity.IsVisible)
            {
                if (entity.Type == "2")
                {
                    entity.MethodName = txtMethodName.Text;
                    entity.ValueField = txtValueField.Text;
                    entity.TextField = txtTextField.Text;
                    entity.FilterExpression = txtFilterExpression.Text;
                }
                else if (entity.Type == "3")
                {
                    entity.ValueChecked = txtValueChecked.Text;
                    entity.ValueUnchecked = txtValueUnchecked.Text;
                    entity.OtherValue = chkOtherValue.Checked;
                }
                else if (entity.Type == "4")
                {
                    entity.FormatDate = txtFormatDate.Text;
                }
                else if (entity.Type == "5")
                {
                    entity.SearchDialogCodeField = txtSearchDialogCodeField.Text;
                    entity.SearchDialogFilterExpression = txtSearchDialogFilterExpression.Text;
                    entity.SearchDialogIDField = txtSearchDialogIDField.Text;
                    entity.SearchDialogMethodName = txtSearchDialogMethodName.Text;
                    entity.SearchDialogNameField = txtSearchDialogNameField.Text;
                    entity.SearchDialogType = txtSearchDialogType.Text;
                }
                else if (entity.Type == "6")
                {
                    entity.IDColumn = txtIDColumn.Text;
                    entity.FormatCode = txtFormatCode.Text;
                }
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            MigrationConfigurationDtDao entityDao = new MigrationConfigurationDtDao(ctx);
            bool result = false;
            try
            {
                MigrationConfigurationDt entity = new MigrationConfigurationDt();
                ControlToEntity(entity);
                entity.HeaderID = Convert.ToInt32(hdnHeaderID.Value);
                entityDao.Insert(entity);
                retval = BusinessLayer.GetMigrationConfigurationDtMaxID(ctx).ToString();
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
                MigrationConfigurationDt entity = BusinessLayer.GetMigrationConfigurationDt(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                BusinessLayer.UpdateMigrationConfigurationDt(entity);
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