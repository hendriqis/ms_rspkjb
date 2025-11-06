using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Tools
{
    public partial class DataMigrationTableLinkCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnID.Value = param;
            MigrationConfigurationHd entity = BusinessLayer.GetMigrationConfigurationHd(Convert.ToInt32(hdnID.Value));
            txtHeaderText.Text = string.Format("{0} - {1}", entity.FromTable, entity.ToTable);

            BindGridView();

            txtTableName.Attributes.Add("validationgroup", "mpEntryPopup");
            txtColumnName.Attributes.Add("validationgroup", "mpEntryPopup");
            txtLinkTableName.Attributes.Add("validationgroup", "mpEntryPopup");
            txtLinkColumnName.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetMigrationConfigurationTableLinkList(string.Format("HeaderID = {0} ORDER BY TableName, ColumnName", hdnID.Value));
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }

        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnIsAdd.Value == "0")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(MigrationConfigurationTableLink entity)
        {
            entity.TableName = txtTableName.Text;
            entity.ColumnName = txtColumnName.Text;
            entity.LinkTableColumn = txtLinkColumnName.Text;
            entity.LinkTableName = txtLinkTableName.Text;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                MigrationConfigurationTableLink entity = new MigrationConfigurationTableLink();
                ControlToEntity(entity);
                entity.HeaderID = Convert.ToInt32(hdnID.Value);
                BusinessLayer.InsertMigrationConfigurationTableLink(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                MigrationConfigurationTableLink entity = BusinessLayer.GetMigrationConfigurationTableLink(Convert.ToInt32(hdnID.Value), txtTableName.Text, txtColumnName.Text);
                ControlToEntity(entity);
                BusinessLayer.UpdateMigrationConfigurationTableLink(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                BusinessLayer.DeleteMigrationConfigurationTableLink(Convert.ToInt32(hdnID.Value), txtTableName.Text, txtColumnName.Text);
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