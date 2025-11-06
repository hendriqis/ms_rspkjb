using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class StandardCodeEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.STANDARD_CODE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 1)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                StandardCode entity = BusinessLayer.GetStandardCode(ID);
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtStandardCodeName.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtStandardCodeParentID, new ControlEntrySetting(false, false, true, Request.QueryString["par"]));
            SetControlEntrySetting(txtStandardCodeChildID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtStandardCodeName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTagProperty, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFHIRReferenceCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFHIRReferenceName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsMapping, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(StandardCode entity)
        {
            txtStandardCodeChildID.Text = entity.StandardCodeID.Split('^')[1];
            txtStandardCodeName.Text = entity.StandardCodeName;
            txtTagProperty.Text = entity.TagProperty;
            txtNotes.Text = entity.Notes;
            string[] info = entity.FHIRReferenceInfo.Split('|');
            if (info.Length>=2)
            {
                txtFHIRReferenceCode.Text = info[0];
                txtFHIRReferenceName.Text = info[1]; 
            }

            chkIsMapping.Checked = entity.IsMapping;
        }

        private void ControlToEntity(StandardCode entity)
        {
            entity.StandardCodeID = String.Format("{0}^{1}", txtStandardCodeParentID.Text, txtStandardCodeChildID.Text);
            entity.StandardCodeName = txtStandardCodeName.Text;
            entity.TagProperty = txtTagProperty.Text;
            entity.Notes = txtNotes.Text;
            entity.IsMapping = chkIsMapping.Checked;
            entity.ParentID = txtStandardCodeParentID.Text;
            entity.FHIRReferenceInfo = string.Format("{0}|{1}", txtFHIRReferenceCode.Text, txtFHIRReferenceName.Text);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("StandardCodeID = '{0}^{1}'", txtStandardCodeParentID.Text, txtStandardCodeChildID.Text);
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Standard Code with ID " + String.Format("{0}^{1}", txtStandardCodeParentID.Text, txtStandardCodeChildID.Text) + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                StandardCode entity = new StandardCode();
                ControlToEntity(entity);
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
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                StandardCode entity = BusinessLayer.GetStandardCode(hdnID.Value);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateStandardCode(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}