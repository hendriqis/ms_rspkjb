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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ViewQACtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            SetControlProperties(paramInfo);
            RadiotherapyProgramQA entity = BusinessLayer.GetRadiotherapyProgramQA(Convert.ToInt32(hdnProgramQAID.Value));
            EntityToControl(entity);
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnProgramQAID.Value = paramInfo[0];
            hdnProgramID.Value = paramInfo[1];
            hdnPopupVisitID.Value = paramInfo[2];

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0",
Constant.StandardCode.BEAM_PESAWAT, Constant.StandardCode.RADIOTHERAPY_VERIFICATION, Constant.StandardCode.ENERGY));

            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BEAM_PESAWAT).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCPesawat, lstCode1, "StandardCodeName", "StandardCodeID");
            List<StandardCode> lstCode4 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.RADIOTHERAPY_VERIFICATION).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCVerificationType, lstCode4, "StandardCodeName", "StandardCodeID");
            List<StandardCode> lstCode5 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.ENERGY).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCEnergy, lstCode5, "StandardCodeName", "StandardCodeID");
        }

        private void EntityToControl(RadiotherapyProgramQA entity)
        {
           txtQADate.Text = entity.QADate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
           txtQATime.Text = entity.QATime;
           cboGCPesawat.Value = entity.GCPesawat;
           cboGCEnergy.Value = entity.GCEnergy;
           cboGCVerificationType.Value = entity.GCVerificationType;
           txtTotalDosage.Text = entity.TotalDosage.ToString();
           txtTotalFraction.Text = entity.TotalFraction.ToString();
           txtMachineUnit.Text = entity.MachineUnit.ToString();
           txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(RadiotherapyProgramQA entity)
        {
            entity.QADate = Helper.GetDatePickerValue(txtQADate);
            entity.QATime = txtQATime.Text;
            entity.ProgramID = Convert.ToInt32(hdnProgramID.Value);
            entity.VisitID = Convert.ToInt32(hdnPopupVisitID.Value);
            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            entity.GCPesawat = cboGCPesawat.Value.ToString();
            entity.GCEnergy = cboGCEnergy.Value.ToString();
            entity.TotalDosage = Convert.ToDecimal(txtTotalDosage.Text);
            entity.TotalFraction = Convert.ToInt32(txtTotalFraction.Text);
            entity.MachineUnit = Convert.ToDecimal(txtMachineUnit.Text);
            entity.GCVerificationType = cboGCVerificationType.Value.ToString();
            entity.Remarks = txtRemarks.Text;
        }
    }
}