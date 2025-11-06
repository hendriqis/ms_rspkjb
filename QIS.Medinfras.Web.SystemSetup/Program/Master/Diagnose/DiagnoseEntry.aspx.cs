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
    public partial class DiagnoseEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.DIAGNOSE;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                Diagnose entity = BusinessLayer.GetDiagnose(ID);
                EntityToControl(entity);
                if (entity.DTDNo != null && entity.DTDNo != "")
                {
                    DTD entityDTD = BusinessLayer.GetDTD(entity.DTDNo);
                    txtDTDName.Text = entityDTD.DTDName;
                }
            }
            else
            {
                IsAdd = true;
            }
            
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtDiagnoseID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDiagnoseName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDTDNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDTDName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsChronicDisease, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsDisease, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(Diagnose entity)
        {
            txtDiagnoseID.Text = entity.DiagnoseID;
            txtDiagnoseName.Text = entity.DiagnoseName;
            txtDTDNo.Text = entity.DTDNo;
            chkIsDisease.Checked = entity.IsDisease;
            chkIsChronicDisease.Checked = entity.IsChronicDisease;
        }

        private void ControlToEntity(Diagnose entity)
        {
            entity.DiagnoseID = txtDiagnoseID.Text;
            entity.DiagnoseName = txtDiagnoseName.Text;
            entity.DTDNo = txtDTDNo.Text;
            entity.IsDisease = chkIsDisease.Checked;
            entity.IsChronicDisease = chkIsChronicDisease.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("DiagnoseID = '{0}'", txtDiagnoseID.Text);
            List<Diagnose> lst = BusinessLayer.GetDiagnoseList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Diagnose with code " + txtDiagnoseID.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                Diagnose entity = new Diagnose();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entity.DiagnoseID;
                BusinessLayer.InsertDiagnose(entity);
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
                Diagnose entity = BusinessLayer.GetDiagnose(hdnID.Value);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateDiagnose(entity);
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