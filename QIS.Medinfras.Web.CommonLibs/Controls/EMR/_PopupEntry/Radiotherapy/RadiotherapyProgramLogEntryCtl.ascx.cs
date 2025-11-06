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
    public partial class RadiotherapyProgramLogEntryCtl : BaseEntryPopupCtl3
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            if (paramInfo[0] != "0")
            {
                IsAdd = false;
                SetControlProperties(paramInfo);
                RadiotheraphyProgramLog entity = BusinessLayer.GetRadiotheraphyProgramLog(Convert.ToInt32(hdnProgramLogID.Value));
                EntityToControl(entity);
            }
            else
            {
                hdnProgramLogID.Value = "";
                IsAdd = true;
                SetControlProperties(paramInfo);
                if (paramInfo.Length >= 5)
                {
                    //copy mode
                    if (!string.IsNullOrEmpty(paramInfo[3]))
                    {
                        int copyRecordID = Convert.ToInt32(paramInfo[4]);
                        RadiotheraphyProgramLog entity = BusinessLayer.GetRadiotheraphyProgramLog(Convert.ToInt32(copyRecordID));
                        EntityToControl(entity);

                        txtLogDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtLogTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    }
                }
            }
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnProgramLogID.Value = paramInfo[0];
            hdnProgramID.Value = paramInfo[1];
            hdnPopupVisitID.Value = paramInfo[2];
            hdnTotalFraction.Value = paramInfo[3];
        }

        protected override void OnControlEntrySetting()
        {
            if (AppSession.RegisteredPatient.DepartmentID != Constant.Facility.INPATIENT)
            {
                SetControlEntrySetting(txtLogDate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
                SetControlEntrySetting(txtLogTime, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitTime));
            }
            else
            {
                SetControlEntrySetting(txtLogDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
                SetControlEntrySetting(txtLogTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            }

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

            SetControlEntrySetting(txtVRT, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtLNG, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtLAT, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtROT, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));

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

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            if (IsValidToProceed(ref errMessage))
            {

                IDbContext ctx = DbFactory.Configure(true);
                RadiotheraphyProgramLogDao objDao = new RadiotheraphyProgramLogDao(ctx);

                try
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    int recordID = 0;

                    RadiotheraphyProgramLog entity = new RadiotheraphyProgramLog();
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
            RadiotheraphyProgramLogDao objDao = new RadiotheraphyProgramLogDao(ctx);

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
                    RadiotheraphyProgramLog entity = objDao.Get(Convert.ToInt32(hdnProgramLogID.Value));
                    if (entity != null)
                    {
                        ControlToEntity(entity);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        objDao.Update(entity);

                        retVal = hdnProgramLogID.Value;

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Tidak ditemukan program radioterapi yang dilakukan perubahan";
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

        private bool IsValidToProceed(ref string errMessage)
        {
            StringBuilder errMsg = new StringBuilder();

            if (!string.IsNullOrEmpty(txtFractionNo.Text))
            {
                if (!Methods.IsNumeric(txtFractionNo.Text))
                {
                    errMsg.AppendLine("Nilai Fraksi Ke- harus berupa numerik/angka dan lebih besar dari 0");
                }
                else
                {
                    int fractionNo = Convert.ToInt32(txtFractionNo.Text);
                    if (fractionNo == 0)
                    {
                        errMsg.AppendLine("Nilai Fraksi Ke- harus berupa numerik/angka dan lebih besar dari 0");
                    }
                    else
                    {
                        if (fractionNo > Convert.ToInt32(hdnTotalFraction.Value))
                        {
                            errMsg.AppendLine(string.Format("Nilai Fraksi Ke- tidak boleh lebih besar dari jumlah Fraksi Program ({0})", hdnTotalFraction.Value));
                        }
                    }
                }
            }

            #region Report Date
            DateTime date;
            string format = Constant.FormatString.DATE_PICKER_FORMAT;

            try
            {
                date = DateTime.ParseExact(txtLogDate.Text, format, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                errMsg.AppendLine("Format Tanggal Catatan tidak sesuai dengan format");
            };
            if (DateTime.Compare(Helper.GetDatePickerValue(txtLogDate.Text), DateTime.Now.Date) > 0)
            {
                errMsg.AppendLine("Tanggal Catatan harus lebih kecil atau sama dengan tanggal saat ini.");
            }

            if (string.IsNullOrEmpty(txtLogTime.Text))
            {
                errMsg.AppendLine("Jam Catatan harus diisi");
            }
            else
            {
                if (!Methods.ValidateTimeFormat(txtLogTime.Text))
                    errMsg.AppendLine("Format Jam Catatan tidak sesuai format (HH:MM)");
            }
            #endregion

            if (cboGCEnergy.Value == null)
            {
                errMsg.AppendLine("Energi harus diisi");
            }
            if (cboGCPesawat.Value == null)
            {
                errMsg.AppendLine("Pesawat harus diisi");
            }
            if (cboGCBeamUnit.Value == null)
            {
                errMsg.AppendLine("Unit Rad harus diisi");
            }
            if (cboGCAccess.Value == null)
            {
                errMsg.AppendLine("Access harus diisi");
            }
            if (cboGCSetup.Value == null)
            {
                errMsg.AppendLine("Setup harus diisi");
            }

            if (!string.IsNullOrEmpty(txtVRT.Text))
            {
                if (!Methods.IsNumeric(txtVRT.Text))
                {
                    errMsg.AppendLine("Nilai VRT harus berupa numerik/angka");
                }
            }

            if (!string.IsNullOrEmpty(txtLNG.Text))
            {
                if (!Methods.IsNumeric(txtLNG.Text))
                {
                    errMsg.AppendLine("Nilai LNG harus berupa numerik/angka");
                }
            }

            if (!string.IsNullOrEmpty(txtLAT.Text))
            {
                if (!Methods.IsNumeric(txtLAT.Text))
                {
                    errMsg.AppendLine("Nilai LNG harus berupa numerik/angka");
                }
            }

            if (!string.IsNullOrEmpty(txtROT.Text))
            {
                if (!Methods.IsNumeric(txtROT.Text))
                {
                    errMsg.AppendLine("Nilai ROT harus berupa numerik/angka");
                }
            }

            if (!string.IsNullOrEmpty(txtNumberOfFields.Text))
            {
                if (!Methods.IsNumeric(txtNumberOfFields.Text))
                {
                    errMsg.AppendLine("Nilai Jumlah Lapangan harus berupa numerik/angka");
                }
            }

            if (!string.IsNullOrEmpty(txtMachineUnit.Text))
            {
                if (!Methods.IsNumeric(txtMachineUnit.Text))
                {
                    errMsg.AppendLine("Nilai Total MU harus berupa numerik/angka");
                }
            }

            if (!string.IsNullOrEmpty(txtIsoCenter.Text))
            {
                if (!Methods.IsNumeric(txtIsoCenter.Text))
                {
                    errMsg.AppendLine("Jumlah Isocenter harus berupa numerik/angka");
                }
            }

            errMessage = errMsg.ToString().Replace(Environment.NewLine, "<br />");

            return string.IsNullOrEmpty(errMessage);
        }
    }
}