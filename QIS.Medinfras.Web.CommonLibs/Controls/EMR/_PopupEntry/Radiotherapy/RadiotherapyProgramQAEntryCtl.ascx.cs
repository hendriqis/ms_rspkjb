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
    public partial class RadiotherapyProgramQAEntryCtl : BaseEntryPopupCtl3
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            if (paramInfo[0] != "0")
            {
                IsAdd = false;
                SetControlProperties(paramInfo);
                RadiotherapyProgramQA entity = BusinessLayer.GetRadiotherapyProgramQA(Convert.ToInt32(hdnProgramQAID.Value));
                EntityToControl(entity);
            }
            else
            {
                hdnProgramQAID.Value = "";
                IsAdd = true;
                SetControlProperties(paramInfo);
                if (paramInfo.Length >= 4)
                {
                    //copy mode
                    if (!string.IsNullOrEmpty(paramInfo[3]))
                    {
                        int copyRecordID = Convert.ToInt32(paramInfo[3]);
                        RadiotherapyProgramQA entity = BusinessLayer.GetRadiotherapyProgramQA(Convert.ToInt32(copyRecordID));
                        EntityToControl(entity);

                        txtQADate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtQATime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    }
                }
            }
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnProgramQAID.Value = paramInfo[0];
            hdnProgramID.Value = paramInfo[1];
            hdnPopupVisitID.Value = paramInfo[2];
        }

        protected override void OnControlEntrySetting()
        {
            if (AppSession.RegisteredPatient.DepartmentID != Constant.Facility.INPATIENT)
            {
                SetControlEntrySetting(txtQADate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
                SetControlEntrySetting(txtQATime, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitTime));
            }
            else
            {
                SetControlEntrySetting(txtQADate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
                SetControlEntrySetting(txtQATime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            }

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0",
Constant.StandardCode.BEAM_PESAWAT, Constant.StandardCode.RADIOTHERAPY_VERIFICATION, Constant.StandardCode.ENERGY));

            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BEAM_PESAWAT).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCPesawat, lstCode1, "StandardCodeName", "StandardCodeID");
            List<StandardCode> lstCode4 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.RADIOTHERAPY_VERIFICATION).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCVerificationType, lstCode4, "StandardCodeName", "StandardCodeID");
            List<StandardCode> lstCode5 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.ENERGY).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCEnergy, lstCode5, "StandardCodeName", "StandardCodeID");

            SetControlEntrySetting(cboGCPesawat, new ControlEntrySetting(true, true, true, string.Empty));
            SetControlEntrySetting(cboGCEnergy, new ControlEntrySetting(true, true, true, string.Empty));
            SetControlEntrySetting(cboGCVerificationType, new ControlEntrySetting(true, true, true, string.Empty));
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

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            if (IsValidToProceed(ref errMessage))
            {

                IDbContext ctx = DbFactory.Configure(true);
                RadiotherapyProgramQADao objDao = new RadiotherapyProgramQADao(ctx);

                try
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    int recordID = 0;

                    RadiotherapyProgramQA entity = new RadiotherapyProgramQA();
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
            RadiotherapyProgramQADao objDao = new RadiotherapyProgramQADao(ctx);

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
                    RadiotherapyProgramQA entity = objDao.Get(Convert.ToInt32(hdnProgramQAID.Value));
                    if (entity != null)
                    {
                        ControlToEntity(entity);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        objDao.Update(entity);

                        retVal = hdnProgramQAID.Value;

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

            if (cboGCEnergy.Value == null)
            {
                errMsg.AppendLine("Energi harus diisi");
            }
            if (cboGCPesawat.Value == null)
            {
                errMsg.AppendLine("Pesawat harus diisi");                
            }
            if (cboGCVerificationType.Value == null)
            {
                errMsg.AppendLine("Jenis Verifikasi harus diisi");
            }

            if (!string.IsNullOrEmpty(txtTotalFraction.Text))
            {
                if (!Methods.IsNumeric(txtTotalFraction.Text))
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

            if (!string.IsNullOrEmpty(txtTotalDosage.Text))
            {
                if (!Methods.IsNumeric(txtTotalDosage.Text))
                {
                    errMsg.AppendLine("Total Dosis harus berupa numerik/angka");
                }
            }

            errMessage = errMsg.ToString().Replace(Environment.NewLine, "<br />");

            return string.IsNullOrEmpty(errMessage);
        }
    }
}