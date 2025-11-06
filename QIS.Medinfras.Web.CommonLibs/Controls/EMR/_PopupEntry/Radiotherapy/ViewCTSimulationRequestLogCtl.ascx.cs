using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using System.Text;
using System.IO;
using DevExpress.Web.ASPxCallbackPanel;
using System.Net;
using System.Globalization;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ViewCTSimulationRequestLogCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            hdnID.Value = paramInfo[0];
            hdnPopupRequestID.Value = paramInfo[1];
            hdnPopupVisitID.Value = paramInfo[2];
            hdnPopupMRN.Value = paramInfo[3];
            hdnPopupMedicalNo.Value = paramInfo[4];
            hdnPopupPatientName.Value = paramInfo[5];
            hdnPopupParamedicID.Value = paramInfo[6];

            SetControlProperties(); 

            if (hdnID.Value == "0" || hdnID.Value == string.Empty)
            {
                if (paramInfo.Length >= 8)
                {
                    //copy mode
                    if (!string.IsNullOrEmpty(paramInfo[7]))
                    {
                        int copyRecordID = Convert.ToInt32(paramInfo[7]);
                        string filter = string.Format("SimulationRequestLogID = '{0}'", copyRecordID);
                        vCTSimulationRequestLog entity = BusinessLayer.GetvCTSimulationRequestLogList(filter).FirstOrDefault();
                        EntityToControl(entity);

                        txtProgramDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtProgramTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    }
                }
            }
            else
            {
                string filter = string.Format("SimulationRequestLogID = '{0}'", hdnID.Value);
                vCTSimulationRequestLog entity = BusinessLayer.GetvCTSimulationRequestLogList(filter).FirstOrDefault();
                EntityToControl(entity);
            }
        }

        private void SetControlProperties()
        {
            txtMedicalNo.Text = hdnPopupMedicalNo.Value;
            txtPatientName.Text = hdnPopupPatientName.Value;

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsActive = 1 AND IsDeleted = 0",
        Constant.StandardCode.SIMULATION_ORIENTASI_POSISI,
        Constant.StandardCode.SIMULATION_POSISI_PASIEN,
        Constant.StandardCode.SIMULATION_POSISI_TANGAN,
        Constant.StandardCode.SIMULATION_ALAT_BANTU));

            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.SIMULATION_ORIENTASI_POSISI).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCOrientation, lstCode1, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode2 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.SIMULATION_POSISI_PASIEN).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCPatientPosition, lstCode2, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode3 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.SIMULATION_POSISI_TANGAN).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCHandPosition, lstCode3, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode4 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.SIMULATION_ALAT_BANTU).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCImmobilizationTool, lstCode4, "StandardCodeName", "StandardCodeID");
        }

        private void ControlToEntity(CTSimulationRequestLog oRecord)
        {
            oRecord.LogDate = Helper.GetDatePickerValue(txtProgramDate);
            oRecord.LogTime = txtProgramTime.Text;
            oRecord.SimulationRequestID = Convert.ToInt32(hdnPopupRequestID.Value);
            oRecord.ParamedicID = Convert.ToInt32(hdnPopupParamedicID.Value);
            oRecord.GCOrientation = cboGCOrientation.Value.ToString();
            oRecord.GCPatientPosition = cboGCPatientPosition.Value.ToString();
            oRecord.GCHandPosition = cboGCHandPosition.Value.ToString();
            oRecord.GCImmobilizationTool = cboGCImmobilizationTool.Value.ToString();
            oRecord.IsHasPatientPositionImage = rblIsHasPatientPositionImage.SelectedValue == "1" ? true : false;

            if (!string.IsNullOrEmpty(txtRemarks.Text))
                oRecord.Remarks = txtRemarks.Text;
        }

        private void EntityToControl(vCTSimulationRequestLog oRecord)
        {
            txtProgramDate.Text = oRecord.LogDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtProgramTime.Text = oRecord.LogTime;
            cboGCOrientation.Value = oRecord.GCOrientation;
            cboGCPatientPosition.Value = oRecord.GCPatientPosition;
            cboGCHandPosition.Value = oRecord.GCHandPosition;
            cboGCImmobilizationTool.Value = oRecord.GCImmobilizationTool;
            rblIsHasPatientPositionImage.SelectedValue = oRecord.IsHasPatientPositionImage ? "1" : "0";
            txtRemarks.Text = oRecord.Remarks;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}