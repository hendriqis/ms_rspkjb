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
    public partial class ViewCTSimulationRequestCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            hdnID.Value = paramInfo[0];
            hdnPopupVisitID.Value = paramInfo[1];
            hdnPopupMRN.Value = paramInfo[2];
            hdnPopupMedicalNo.Value = paramInfo[3];
            hdnPopupPatientName.Value = paramInfo[4];
            hdnPopupParamedicID.Value = paramInfo[5];
            
            SetControlProperties(); 
        }

        private void SetControlProperties()
        {
            txtMedicalNo.Text = hdnPopupMedicalNo.Value;
            txtPatientName.Text = hdnPopupPatientName.Value;

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
        Constant.StandardCode.SIMULATION_REQUEST_TYPE,
        Constant.StandardCode.SIMULATION_SCAN_AREA));

            List<StandardCode> lstCode3 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.SIMULATION_REQUEST_TYPE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCSimulationRequestType, lstCode3, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode4 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.SIMULATION_SCAN_AREA && lst.StandardCodeID != "X572^999").ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCScanArea, lstCode4, "StandardCodeName", "StandardCodeID");

            VisitInfoForRadiotherapyProgram oInfo = BusinessLayer.GetVisitInfoForRadiotherapyProgram(Convert.ToInt32(hdnPopupVisitID.Value)).FirstOrDefault();
            if (oInfo != null)
            {
                txtDiagnosisInfo.Text = oInfo.PatientDiagnosis.Replace("+", Environment.NewLine);
                txtStagingInfo.Text = string.Format("{0} : T{1} N{2} M{3}", oInfo.CancerStaging, oInfo.TValue, oInfo.NValue, oInfo.MValue);
            }

            string filter = string.Format("SimulationRequestID = '{0}'", hdnID.Value);
            vCTSimulationRequest entity = BusinessLayer.GetvCTSimulationRequestList(filter).FirstOrDefault();
            EntityToControl(entity);
        }

        private void ControlToEntity(CTSimulationRequest oRecord)
        {
            oRecord.RequestDate = Helper.GetDatePickerValue(txtProgramDate);
            oRecord.RequestTime = txtProgramTime.Text;
            oRecord.VisitID = Convert.ToInt32(hdnPopupVisitID.Value);
            oRecord.ParamedicID = Convert.ToInt32(hdnPopupParamedicID.Value);
            oRecord.GCSimulationRequestType = cboGCSimulationRequestType.Value.ToString();
            oRecord.GCScanArea = cboGCScanArea.Value.ToString();
            oRecord.ScanBorder = txtScanBorder.Text;
            oRecord.IsUsingContrast = rblIsUsingContrast.SelectedValue == "1" ? true : false;

            if (!string.IsNullOrEmpty(txtRemarks.Text))
                oRecord.Remarks = txtRemarks.Text;

            oRecord.GCRequestStatus = Constant.TransactionStatus.OPEN;
        }

        private void EntityToControl(vCTSimulationRequest oRecord)
        {
            txtProgramDate.Text = oRecord.RequestDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtProgramTime.Text = oRecord.RequestTime;
            cboGCSimulationRequestType.Value = oRecord.GCSimulationRequestType;
            cboGCScanArea.Value = oRecord.GCScanArea;
            txtScanBorder.Text = oRecord.ScanBorder;
            rblIsUsingContrast.SelectedValue = oRecord.IsUsingContrast ? "1" : "0";
            txtRemarks.Text = oRecord.Remarks;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}