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
    public partial class CTSimulationRequestEntryCtl : BaseEntryPopupCtl3
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
            
            if (hdnID.Value == "0" || hdnID.Value == string.Empty)
            {
                IsAdd = true;
            }
            else
            {
                IsAdd = false;
            }

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

            if (!IsAdd)
            {
                string filter = string.Format("SimulationRequestID = '{0}'", hdnID.Value);
                vCTSimulationRequest entity = BusinessLayer.GetvCTSimulationRequestList(filter).FirstOrDefault();
                EntityToControl(entity);
            }
            else
            {
                txtProgramDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtProgramTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtProgramDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
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

            if (IsAdd)
            {
                oRecord.GCRequestStatus = Constant.TransactionStatus.OPEN;
            }
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

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            if (IsValidToProceed(ref errMessage))
            {

                IDbContext ctx = DbFactory.Configure(true);
                CTSimulationRequestDao objDao = new CTSimulationRequestDao(ctx);

                try
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    int recordID = 0;

                    CTSimulationRequest entity = new CTSimulationRequest();
                    ControlToEntity(entity);
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    recordID = objDao.InsertReturnPrimaryKeyID(entity);

                    retVal = recordID.ToString();

                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    result = false;
                    retVal = "0";
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            bool isError = false;
            IDbContext ctx = DbFactory.Configure(true);
            CTSimulationRequestDao objDao = new CTSimulationRequestDao(ctx);

            if (!IsValidToProceed(ref errMessage))
            {
                isError = true;
                result = false;
            }

            if (!isError)
            {
                try
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    CTSimulationRequest entity = objDao.Get(Convert.ToInt32(hdnID.Value));
                    if (entity != null)
                    {
                        ControlToEntity(entity);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        objDao.Update(entity);

                        retVal = hdnID.Value;

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Tidak ditemukan program permintaan simulasi yang dilakukan perubahan";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                } 
            }
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        private bool IsValidToProceed(ref string errMessage)
        {
            StringBuilder errMsg = new StringBuilder();

            DateTime date;
            string format = Constant.FormatString.DATE_PICKER_FORMAT;
 
            try
            {
                date = DateTime.ParseExact(txtProgramDate.Text, format, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                errMsg.AppendLine(string.Format("Format Tanggal Permintaan tidak sesuai dengan format", txtProgramDate.Text));
            }
;
            if (DateTime.Compare(Helper.GetDatePickerValue(txtProgramDate.Text), DateTime.Now.Date) > 0)
            {
                errMsg.AppendLine("Tanggal Permintaan harus lebih kecil atau sama dengan tanggal saat ini.");
            }

            if (!string.IsNullOrEmpty(txtProgramTime.Text) || txtProgramTime.Text != "")
            {
                Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                if (!reg.IsMatch(txtProgramTime.Text))
                {
                    errMsg.AppendLine("Jam pengisian permintaan harus sesuai dengan format jam (00:00 s/d 23:59)");
                }
            }

            if (cboGCSimulationRequestType.Value != null)
            {
                if (string.IsNullOrEmpty(cboGCSimulationRequestType.Value.ToString()))
                {
                    errMsg.AppendLine("Jenis Permintaan Simulasi harus diisi atau tidak boleh kosong");
                }
            }
            else
            {
                errMsg.AppendLine("Jenis Permintaan Simulasi harus diisi atau tidak boleh kosong");
            }

            if (cboGCScanArea.Value != null)
            {
                if (string.IsNullOrEmpty(cboGCScanArea.Value.ToString()))
                {
                    errMsg.AppendLine("Scan Area harus diisi atau tidak boleh kosong");
                }
            }
            else
            {
                errMsg.AppendLine("Scan Area harus diisi atau tidak boleh kosong");
            }

            if (string.IsNullOrEmpty(txtScanBorder.Text))
            {
                errMsg.AppendLine("Batasan Scan harus diisi atau tidak boleh kosong");
            }

            if (string.IsNullOrEmpty(rblIsUsingContrast.SelectedValue))
            {
                errMsg.AppendLine("Kontras atau Non-Kontras harus ditentukan");
            }


            errMessage = errMsg.ToString().Replace(Environment.NewLine, "<br />");

            return (errMessage == string.Empty);
        }
    }
}