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
    public partial class CTSimulationRequestLogEntryCtl : BaseEntryPopupCtl3
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
                IsAdd = true;

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
                IsAdd = false;
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

            if (IsAdd)
            {
                txtProgramDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtProgramTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtProgramDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
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

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            if (IsValidToProceed(ref errMessage))
            {

                IDbContext ctx = DbFactory.Configure(true);
                CTSimulationRequestLogDao objDao = new CTSimulationRequestLogDao(ctx);

                try
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    int recordID = 0;

                    CTSimulationRequestLog entity = new CTSimulationRequestLog();
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
            CTSimulationRequestLogDao objDao = new CTSimulationRequestLogDao(ctx);

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
                    CTSimulationRequestLog entity = objDao.Get(Convert.ToInt32(hdnID.Value));
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
                errMsg.AppendLine(string.Format("Format Tanggal Catatan tidak sesuai dengan format", txtProgramDate.Text));
            }
;
            if (DateTime.Compare(Helper.GetDatePickerValue(txtProgramDate.Text), DateTime.Now.Date) > 0)
            {
                errMsg.AppendLine("Tanggal Catatan harus lebih kecil atau sama dengan tanggal saat ini.");
            }

            if (!string.IsNullOrEmpty(txtProgramTime.Text) || txtProgramTime.Text != "")
            {
                Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                if (!reg.IsMatch(txtProgramTime.Text))
                {
                    errMsg.AppendLine("Jam pengisian catatan harus sesuai dengan format jam (00:00 s/d 23:59)");
                }
            }

            if (cboGCOrientation.Value != null)
            {
                if (string.IsNullOrEmpty(cboGCOrientation.Value.ToString()))
                {
                    errMsg.AppendLine("Orientasi Pasien harus diisi atau tidak boleh kosong");
                }
            }
            else
            {
                errMsg.AppendLine("Orientasi Pasien harus diisi atau tidak boleh kosong");
            }

            //if (cboGCScanArea.Value != null)
            //{
            //    if (string.IsNullOrEmpty(cboGCScanArea.Value.ToString()))
            //    {
            //        errMsg.AppendLine("Scan Area harus diisi atau tidak boleh kosong");
            //    }
            //}
            //else
            //{
            //    errMsg.AppendLine("Scan Area harus diisi atau tidak boleh kosong");
            //}

            //if (string.IsNullOrEmpty(txtScanBorder.Text))
            //{
            //    errMsg.AppendLine("Batasan Scan harus diisi atau tidak boleh kosong");
            //}

            //if (string.IsNullOrEmpty(rblIsUsingContrast.SelectedValue))
            //{
            //    errMsg.AppendLine("Kontras atau Non-Kontras harus ditentukan");
            //}


            errMessage = errMsg.ToString().Replace(Environment.NewLine, "<br />");

            return (errMessage == string.Empty);
        }
    }
}