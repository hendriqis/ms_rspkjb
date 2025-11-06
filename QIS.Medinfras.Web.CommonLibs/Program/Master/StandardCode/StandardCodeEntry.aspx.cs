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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class StandardCodeEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_UNIT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            String[] param = Request.QueryString["id"].Split('|');
            if (param[0] == "edit")
            {
                IsAdd = false;
                string ID = param[1];
                txtStandardCodeID.Text = ID;
                StandardCode entity = BusinessLayer.GetStandardCode(ID);
                hdnParentID.Value = entity.ParentID;
                EntityToControl(entity);
            }
            else
            {
                hdnParentID.Value = param[1];
                IsAdd = true;
            }
            txtStandardCodeName.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtStandardCodeID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtStandardCodeName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTagProperty, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(StandardCode entity)
        {
            txtStandardCodeID.Text = entity.StandardCodeID.Split('^')[1];
            txtStandardCodeName.Text = entity.StandardCodeName;
            txtTagProperty.Text = entity.TagProperty;
            txtNotes.Text = entity.Notes;
        }

        private void ControlToEntity(StandardCode entity)
        {
            entity.StandardCodeName = txtStandardCodeName.Text;
            entity.TagProperty = txtTagProperty.Text;
            entity.Notes = txtNotes.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("StandardCodeID = '{0}^{1}'", hdnParentID.Value, txtStandardCodeID.Text);
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Standard Code with ID " + txtStandardCodeID.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                StandardCode entity = new StandardCode();
                ControlToEntity(entity);
                entity.StandardCodeID = String.Format("{0}^{1}", hdnParentID.Value, txtStandardCodeID.Text);
                entity.ParentID = hdnParentID.Value;
                entity.IsEditableByUser = true;
                entity.IsHeader = false;
                entity.IsDefault = false;
                entity.IsActive = true;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertStandardCode(entity);
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
                string StandardCodeID = string.Format("{0}^{1}", hdnParentID.Value, txtStandardCodeID.Text);
                StandardCode entity = BusinessLayer.GetStandardCode(StandardCodeID);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateStandardCode(entity);
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