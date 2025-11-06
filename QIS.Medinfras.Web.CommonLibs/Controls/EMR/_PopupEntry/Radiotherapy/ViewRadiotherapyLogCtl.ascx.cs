using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;
using System.Data;
using System.Text;
using System.Globalization;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ViewRadiotherapyLogCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            if (paramInfo[0] != "0")
            {
                SetControlProperties(paramInfo);
                RadiotheraphyProgramLog entity = BusinessLayer.GetRadiotheraphyProgramLog(Convert.ToInt32(hdnProgramLogID.Value));
                EntityToControl(entity);
            }
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnProgramLogID.Value = paramInfo[0];
            hdnProgramID.Value = paramInfo[1];
            hdnPopupVisitID.Value = paramInfo[2];
            hdnTotalFraction.Value = paramInfo[3];

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}') AND IsActive = 1 AND IsDeleted = 0",
Constant.StandardCode.BEAM_PESAWAT, Constant.StandardCode.BEAM_RAD, Constant.StandardCode.BEAM_SETUP, Constant.StandardCode.BEAM_ACCESS, Constant.StandardCode.ENERGY));

            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BEAM_PESAWAT).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCPesawat, lstCode1, "StandardCodeName", "StandardCodeID");
            List<StandardCode> lstCode2 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BEAM_RAD).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCBeamUnit, lstCode2, "StandardCodeName", "StandardCodeID");
            List<StandardCode> lstCode3 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BEAM_ACCESS).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCAccess, lstCode3, "StandardCodeName", "StandardCodeID");
            List<StandardCode> lstCode4 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BEAM_SETUP).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCSetup, lstCode4, "StandardCodeName", "StandardCodeID");
            List<StandardCode> lstCode5 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.ENERGY).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCEnergy, lstCode5, "StandardCodeName", "StandardCodeID");
        }

        private void EntityToControl(RadiotheraphyProgramLog entity)
        {
            txtFractionNo.Text = entity.FractionNo.ToString();
            txtLogDate.Text = entity.LogDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtLogTime.Text = entity.LogTime;
            cboGCPesawat.Value = entity.GCPesawat;
            cboGCBeamUnit.Value = entity.GCBeamUnit;
            cboGCAccess.Value = entity.GCAccess;
            cboGCSetup.Value = entity.GCSetup;
            cboGCEnergy.Value = entity.GCEnergy;
            txtVRT.Text = entity.VRT.ToString();
            txtLAT.Text = entity.LAT.ToString();
            txtLNG.Text = entity.LNG.ToString();
            txtROT.Text = entity.ROT.ToString();
            txtDuration.Text = entity.Duration.ToString();
            txtNumberOfFields.Text = entity.NumberOfFields.ToString();
            txtMachineUnit.Text = entity.MachineUnit.ToString();
            txtIsoCenter.Text = entity.IsoCenter.ToString();
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(RadiotheraphyProgramLog entity)
        {
            entity.FractionNo = Convert.ToInt32(txtFractionNo.Text);
            entity.LogDate = Helper.GetDatePickerValue(txtLogDate);
            entity.LogTime = txtLogTime.Text;
            entity.ProgramID = Convert.ToInt32(hdnProgramID.Value);
            entity.VisitID = Convert.ToInt32(hdnPopupVisitID.Value);
            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            entity.GCPesawat = cboGCPesawat.Value.ToString();
            entity.GCBeamUnit = cboGCBeamUnit.Value.ToString();
            entity.GCAccess = cboGCAccess.Value.ToString();
            entity.GCSetup = cboGCSetup.Value.ToString();
            entity.GCEnergy = cboGCEnergy.Value.ToString();
            entity.VRT = Convert.ToDecimal(txtVRT.Text);
            entity.LNG = Convert.ToDecimal(txtLNG.Text);
            entity.LAT = Convert.ToDecimal(txtLAT.Text);
            entity.ROT = Convert.ToDecimal(txtROT.Text);
            entity.Duration = Convert.ToDecimal(txtDuration.Text);
            entity.NumberOfFields = Convert.ToInt32(txtNumberOfFields.Text);
            entity.MachineUnit = Convert.ToDecimal(txtMachineUnit.Text);
            entity.IsoCenter = Convert.ToDecimal(txtIsoCenter.Text);
            entity.Remarks = txtRemarks.Text;
        }
    }
}