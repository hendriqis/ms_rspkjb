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
    public partial class DataMigrationConfigurationEntry : BasePageEntry
    {
        private string[] lstGridColumns;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.DATA_MIGRATION_CONFIGURATION;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                MigrationConfigurationHd entity = BusinessLayer.GetMigrationConfigurationHdList(string.Format("ID = {0}", Convert.ToInt32(ID)))[0];
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
                SetControlProperties();
            }
            cboFromTable.Focus();
        }

        protected override void SetControlProperties()
        {
            List<SysObjects> lstTable = BusinessLayer.GetSysObjectsList("type = 'U' ORDER BY name ASC");
            Methods.SetComboBoxField<SysObjects>(cboToTable, lstTable, "Name", "ObjectID");
            Methods.SetComboBoxField<SysObjects>(cboFromTable, lstTable, "Name", "ObjectID");
        }

        protected void cbpColumnList_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (cboFromTable.Value != null && cboFromTable.Value.ToString() != "")
            {
                List<SysColumns> lstColumns = BusinessLayer.GetSysColumnsList("OBJECT_ID = " + cboFromTable.Value);
                rptColumnList.DataSource = lstColumns;
                rptColumnList.DataBind();
            }
        }

        protected void rptColumnList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (hdnGridColumn.Value != "")
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    SysColumns obj = (SysColumns)e.Item.DataItem;

                    if (Array.IndexOf(lstGridColumns, obj.Name) > -1)
                    {
                        HtmlInputCheckBox chkColumn = (HtmlInputCheckBox)e.Item.FindControl("chkColumn");
                        chkColumn.Checked = true;
                    }

                }
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboToTable, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboFromTable, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(MigrationConfigurationHd entity)
        {
            cboFromTable.Text = entity.FromTable;
            cboToTable.Text = entity.ToTable;
            hdnGridColumn.Value = entity.GridColumns;

            if (cboFromTable.Value != null && cboFromTable.Value.ToString() != "")
            {
                lstGridColumns = entity.GridColumns.Split('|');
                List<SysColumns> lstColumns = BusinessLayer.GetSysColumnsList("OBJECT_ID = " + cboFromTable.Value);
                rptColumnList.DataSource = lstColumns;
                rptColumnList.DataBind();
            }
        }

        private void ControlToEntity(MigrationConfigurationHd entity)
        {
            entity.FromTable = cboFromTable.Text;
            entity.ToTable = cboToTable.Text;
            entity.GridColumns = hdnGridColumn.Value;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            MigrationConfigurationHdDao entityDao = new MigrationConfigurationHdDao(ctx);
            bool result = false;
            try
            {
                MigrationConfigurationHd entity = new MigrationConfigurationHd();
                ControlToEntity(entity);
                entityDao.Insert(entity);
                retval = BusinessLayer.GetMigrationConfigurationHdMaxID(ctx).ToString();
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
                MigrationConfigurationHd entity = BusinessLayer.GetMigrationConfigurationHdList(string.Format("ID = {0}", hdnID.Value))[0];
                ControlToEntity(entity);
                BusinessLayer.UpdateMigrationConfigurationHd(entity);
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