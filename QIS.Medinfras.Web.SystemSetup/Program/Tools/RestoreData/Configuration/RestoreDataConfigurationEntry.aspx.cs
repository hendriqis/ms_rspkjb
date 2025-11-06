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

namespace QIS.Medinfras.Web.SystemSetup.Tools
{
    public partial class RestoreDataConfigurationEntry : BasePageEntry
    {
        private string[] lstGridColumns;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.RESTORE_DATA_CONFIGURATION;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                RestoreDataConfiguration entity = BusinessLayer.GetRestoreDataConfigurationList(string.Format("ID = {0}", Convert.ToInt32(ID)))[0];
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
                SetControlProperties();
            }
            cboTableName.Focus();
        }

        protected override void SetControlProperties()
        {
            List<SysObjects> lstTable = BusinessLayer.GetSysObjectsList("type = 'U' ORDER BY name ASC");
            Methods.SetComboBoxField<SysObjects>(cboTableName, lstTable, "Name", "ObjectID");
        }

        protected void cbpColumnList_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (cboTableName.Value != null && cboTableName.Value.ToString() != "")
            {
                List<SysColumns> lstColumns = BusinessLayer.GetSysColumnsList("OBJECT_ID = " + cboTableName.Value);
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
            SetControlEntrySetting(cboTableName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAlias, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFilterExpression, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(RestoreDataConfiguration entity)
        {
            cboTableName.Text = entity.TableName;
            txtAlias.Text = entity.TableAlias;
            txtFilterExpression.Text = entity.FilterExpression;
            hdnGridColumn.Value = entity.GridColumns;

            if (cboTableName.Value != null && cboTableName.Value.ToString() != "")
            {
                lstGridColumns = entity.GridColumns.Split('|');
                List<SysColumns> lstColumns = BusinessLayer.GetSysColumnsList("OBJECT_ID = " + cboTableName.Value);
                rptColumnList.DataSource = lstColumns;
                rptColumnList.DataBind();
            }
        }

        private void ControlToEntity(RestoreDataConfiguration entity)
        {
            entity.TableName = cboTableName.Text;
            entity.TableAlias = txtAlias.Text;
            entity.FilterExpression = txtFilterExpression.Text;
            entity.GridColumns = hdnGridColumn.Value;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                RestoreDataConfiguration entity = new RestoreDataConfiguration();
                ControlToEntity(entity);
                BusinessLayer.InsertRestoreDataConfiguration(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                RestoreDataConfiguration entity = BusinessLayer.GetRestoreDataConfigurationList(string.Format("ID = {0}", hdnID.Value))[0];
                ControlToEntity(entity);
                BusinessLayer.UpdateRestoreDataConfiguration(entity);
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