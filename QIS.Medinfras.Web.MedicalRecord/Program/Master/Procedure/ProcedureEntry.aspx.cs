using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class ProcedureEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PROCEDURES;
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
                vProcedures entity = BusinessLayer.GetvProceduresList(string.Format("ProcedureID = '{0}'", hdnID.Value)).FirstOrDefault();
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtProcedureID.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
         }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtProcedureID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtProcedureName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtEKlaimINAProcedureName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtEKlaimProcedureCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEKlaimProcedureName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtINACBGLabel, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBPJSReferenceInfo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBPJSReferenceInfo1Code, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBPJSReferenceInfo1Name, new ControlEntrySetting(false, false, false));
        }

        private void EntityToControl(vProcedures entity)
        {
            txtProcedureID.Text = entity.ProcedureID;
            txtProcedureName.Text = entity.ProcedureName;
            txtEKlaimProcedureCode.Text = entity.INACBGLabel;
            txtEKlaimProcedureName.Text = entity.INACBGText;
            txtINACBGLabel.Text = entity.INACBGLabel;
            txtBPJSReferenceInfo1Code.Text = entity.BPJSReferenceInfo;
            txtBPJSReferenceInfo.Text = entity.BPJSReferenceInfo;
            txtKeyword.Text = entity.Keyword;
            txtEKlaimINAProcedureCode.Text = entity.INACBGINALabel;
            txtEKlaimINAProcedureName.Text = entity.INACBGINALabelName; 
            if (!string.IsNullOrEmpty(entity.BPJSReferenceInfo))
            {
                if (entity.BPJSReferenceInfo.Contains('|'))
                {
                    string[] bpjsInfo = entity.BPJSReferenceInfo.Split('|');
                    txtBPJSReferenceInfo1Code.Text = bpjsInfo[0];
                    txtBPJSReferenceInfo1Name.Text = bpjsInfo[1];
                }
                else
                {
                    txtBPJSReferenceInfo1Code.Text = entity.BPJSReferenceInfo;
                    txtBPJSReferenceInfo1Name.Text = string.Empty;
                }
            }
            else
            {
                txtBPJSReferenceInfo1Code.Text = string.Empty;
                txtBPJSReferenceInfo1Name.Text = string.Empty;
            }
        }

        private void ControlToEntity(Procedures entity)
        {
            entity.ProcedureName = txtProcedureName.Text;
            entity.INACBGLabel = txtEKlaimProcedureCode.Text;
            entity.BPJSReferenceInfo = txtBPJSReferenceInfo1Code.Text;
            entity.Keyword = txtKeyword.Text;
            entity.INACBGINALabel = txtEKlaimINAProcedureCode.Text; 

            if (!string.IsNullOrEmpty(txtBPJSReferenceInfo1Code.Text))
            {
                entity.BPJSReferenceInfo = string.Format("{0}|{1};{0}|{1}", txtBPJSReferenceInfo1Code.Text, txtBPJSReferenceInfo1Name.Text);
            }
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ProcedureID = '{0}'", txtProcedureID.Text);
            List<Procedures> lst = BusinessLayer.GetProceduresList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Procedure with Code " + txtProcedureID.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                Procedures entity = new Procedures();
                ControlToEntity(entity);
                entity.ProcedureID = txtProcedureID.Text;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertProcedures(entity);
                retval = entity.ProcedureID;
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
                Procedures entity = BusinessLayer.GetProcedures(hdnID.Value);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateProcedures(entity);
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