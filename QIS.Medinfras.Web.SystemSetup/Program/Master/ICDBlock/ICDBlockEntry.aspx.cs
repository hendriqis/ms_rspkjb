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
    public partial class ICDBlockEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.ICD_BLOCK;
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

        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtICDBlockID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtICDBlockName, new ControlEntrySetting(true, true, true));
            
            SetControlEntrySetting(txtFromICDNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFromICDName, new ControlEntrySetting(false, false, false));
            
            SetControlEntrySetting(txtToICDNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtToICDName, new ControlEntrySetting(false, false, false));
        }

        private void EntityToControl(ICDBlock entity)
        {
            txtICDBlockID.Text = entity.ICDBlockID;
            txtICDBlockName.Text = entity.ICDBlockName;
            txtFromICDNo.Text = entity.FromICD;
            txtToICDNo.Text = entity.ToICD;
        }

        private void ControlToEntity(ICDBlock entity)
        {
            entity.ICDBlockID = txtICDBlockID.Text;
            entity.ICDBlockName = txtICDBlockName.Text;
            entity.FromICD = txtFromICDNo.Text;
            entity.ToICD = txtToICDNo.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ICDBlockID = '{0}'", txtICDBlockID.Text);
            List<ICDBlock> lst = BusinessLayer.GetICDBlockList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " ICDBlock with code " + txtICDBlockID.Text + " is already exist!";

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