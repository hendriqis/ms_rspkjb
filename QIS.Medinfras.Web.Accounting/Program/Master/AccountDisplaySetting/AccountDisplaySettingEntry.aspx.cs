using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class AccountDisplaySettingEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.ACCOUNT_DISPLAY_SETTING;
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
                hdnID.Value = Request.QueryString["id"];
                vAccountDisplaySetting entity = BusinessLayer.GetvAccountDisplaySettingList(string.Format("ID = {0}", hdnID.Value)).FirstOrDefault();
                hdnTableName.Value = entity.TableName;
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }

            cboTableName.Focus();
        }

        protected override void SetControlProperties()
        {
            List<vInformationSchemaTables> lstT = BusinessLayer.GetvInformationSchemaTablesList("", int.MaxValue, 1, "TABLE_NAME");
            Methods.SetComboBoxField<vInformationSchemaTables>(cboTableName, lstT, "TABLE_NAME", "TABLE_NAME");
            cboTableName.SelectedIndex = 0;
            if (hdnTableName.Value == "")
            {
                hdnTableName.Value = cboTableName.Value.ToString();
            }

            List<vInformationSchemaColumns> lstF = BusinessLayer.GetvInformationSchemaColumnsList(string.Format("TABLE_NAME = '{0}'", hdnTableName.Value), int.MaxValue, 1, "ORDINAL_POSITION");
            Methods.SetComboBoxField<vInformationSchemaColumns>(cboFieldName, lstF, "COLUMN_NAME", "COLUMN_NAME");
            cboFieldName.SelectedIndex = 0;

            List<Variable> lstDisplay = new List<Variable>();
            lstDisplay.Add(new Variable { Code = "1", Value = GetLabel("1") });
            lstDisplay.Add(new Variable { Code = "2", Value = GetLabel("2") });
            lstDisplay.Add(new Variable { Code = "3", Value = GetLabel("3") });
            lstDisplay.Add(new Variable { Code = "4", Value = GetLabel("4") });
            lstDisplay.Add(new Variable { Code = "5", Value = GetLabel("5") });
            lstDisplay.Add(new Variable { Code = "6", Value = GetLabel("6") });
            Methods.SetComboBoxField<Variable>(cboDisplayOrder, lstDisplay, "Value", "Code");
            cboDisplayOrder.SelectedIndex = 0;
        }

        protected void cboFieldName_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string tabName = hdnTableName.Value;
            List<vInformationSchemaColumns> lstF = BusinessLayer.GetvInformationSchemaColumnsList(string.Format("TABLE_NAME = '{0}'", hdnTableName.Value), int.MaxValue, 1, "ORDINAL_POSITION");
            Methods.SetComboBoxField<vInformationSchemaColumns>(cboFieldName, lstF, "COLUMN_NAME", "COLUMN_NAME");
            cboFieldName.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboTableName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboFieldName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDisplayOrder, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(vAccountDisplaySetting entity)
        {
            cboTableName.Value = entity.TableName;
            hdnTableName.Value = entity.TableName;
            cboFieldName.Value = entity.FieldName;
            hdnFieldName.Value = entity.FieldName;
            cboDisplayOrder.Value = entity.DisplayOrder.ToString();
        }

        private void ControlToEntity(AccountDisplaySetting entity)
        {
            entity.TableName = hdnTableName.Value;
            entity.FieldName = hdnFieldName.Value;
            entity.DisplayOrder = Convert.ToInt32(cboDisplayOrder.Value);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string filter2 = string.Format("DisplayOrder = '{0}' AND IsDeleted = 0", cboDisplayOrder.Value);
            List<AccountDisplaySetting> lst2 = BusinessLayer.GetAccountDisplaySettingList(filter2);

            if (lst2.Count > 0)
            {
                errMessage = "AccountDisplaySetting with DisplayOrder <b>" + cboDisplayOrder.Value + "</b> is already exist!";
            }
            else
            {
                string filter1 = string.Format("TableName = '{0}' AND FieldName = '{1}' AND IsDeleted = 0", cboTableName.Value, cboFieldName.Value);
                List<AccountDisplaySetting> lst1 = BusinessLayer.GetAccountDisplaySettingList(filter1);

                if (lst1.Count > 0)
                {
                    errMessage = "AccountDisplaySetting with Table <b>" + cboTableName.Value + "</b> AND Field <b>" + cboFieldName.Value + "</b> is already exist!";
                }
            }

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string filter2 = string.Format("DisplayOrder = '{0}' AND ID != {1} AND IsDeleted = 0", cboDisplayOrder.Value, hdnID.Value);
            List<AccountDisplaySetting> lst2 = BusinessLayer.GetAccountDisplaySettingList(filter2);

            if (lst2.Count > 0)
            {
                errMessage = "AccountDisplaySetting with DisplayOrder <b>" + cboDisplayOrder.Value + "</b> is already exist!";
            }
            else
            {
                string filter1 = string.Format("TableName = '{0}' AND FieldName = '{1}' AND ID != {2} AND IsDeleted = 0", cboTableName.Value, cboFieldName.Value, hdnID.Value);
                List<AccountDisplaySetting> lst1 = BusinessLayer.GetAccountDisplaySettingList(filter1);

                if (lst1.Count > 0)
                {
                    errMessage = "AccountDisplaySetting with Table <b>" + cboTableName.Value + "</b> AND Field <b>" + cboFieldName.Value + "</b> is already exist!";
                }
            }

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            AccountDisplaySettingDao entityDao = new AccountDisplaySettingDao(ctx);
            bool result = true;
            try
            {
                AccountDisplaySetting entity = new AccountDisplaySetting();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetAccountDisplaySettingMaxID(ctx).ToString();
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
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
            AccountDisplaySettingDao entityDao = new AccountDisplaySettingDao(ctx);
            bool result = true;
            try
            {
                AccountDisplaySetting entity = BusinessLayer.GetAccountDisplaySetting(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
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