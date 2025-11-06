using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class ICDBlockEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.ICDBLOCK;
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
                ICDBlock entity = BusinessLayer.GetICDBlock(ID);
                EntityToControl(entity);
                if (entity.FromICD != null && entity.FromICD != "")
                {
                    Diagnose entityFromDiagnose = BusinessLayer.GetDiagnose(entity.FromICD);
                    txtFromICDName.Text = entityFromDiagnose.DiagnoseName;

                    Diagnose entityToDiagnose = BusinessLayer.GetDiagnose(entity.ToICD);
                    txtToICDName.Text = entityToDiagnose.DiagnoseName;
                }
            }
            else
            {
                IsAdd = true;
            }
            txtICDCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtICDCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtICDName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFromICDCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFromICDName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtToICDCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtToICDName, new ControlEntrySetting(false, false, false));
        }

        private void EntityToControl(ICDBlock entity)
        {
            txtICDCode.Text = entity.ICDBlockID;
            txtICDName.Text = entity.ICDBlockName;
            txtFromICDCode.Text = entity.FromICD;
            txtToICDCode.Text = entity.ToICD;
        }
        
        private void ControlToEntity(ICDBlock entity)
        {
            entity.ICDBlockID = txtICDCode.Text;
            entity.ICDBlockName = txtICDName.Text;
            entity.FromICD = txtFromICDCode.Text;
            entity.ToICD = txtToICDCode.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ICDBlockID = '{0}'", txtICDCode.Text);
            List<ICDBlock> lst = BusinessLayer.GetICDBlockList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Diagnose Group with Code " + txtICDCode.Text + " is already exist!";
            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                ICDBlock entity = new ICDBlock();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertICDBlock(entity);
                retval = entity.ICDBlockID;
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
                ICDBlock entity = BusinessLayer.GetICDBlock(hdnID.Value);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateICDBlock(entity);
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