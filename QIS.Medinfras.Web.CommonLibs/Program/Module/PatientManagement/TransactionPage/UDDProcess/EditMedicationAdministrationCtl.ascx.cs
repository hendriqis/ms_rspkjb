using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class EditMedicationAdministrationCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            this.PopupTitle = "Edit Medication Schedule";

            string[] paramInfo = param.Split('|');
            hdnDispensaryServiceUnitID.Value = paramInfo[0];

            SetControlProperties();

            if (paramInfo.Length > 1)
            {
                hdnSelectedID.Value = paramInfo[1];
                int scheduleID = Convert.ToInt32(paramInfo[1]);
                string filterExp = string.Format("ID = {0}", scheduleID);
                vMedicationSchedule oSchedule = BusinessLayer.GetvMedicationScheduleList(filterExp).FirstOrDefault();
                if (oSchedule != null)
                {
                    //Serialize old object 
                    hdnSelectedItem.Value = JsonConvert.SerializeObject(oSchedule);

                    txtMedicationDate.Text = oSchedule.MedicationDateInString;
                    txtSequenceNo.Text = oSchedule.SequenceNo;
                    txtItemName.Text = oSchedule.DrugName;
                    txtMedicationTime.Text = oSchedule.MedicationTime;
                    hdnMedicationTime.Value = oSchedule.MedicationTime;

                    if (oSchedule.GCMedicationStatus != Constant.MedicationStatus.OPEN && oSchedule.GCMedicationStatus != Constant.MedicationStatus.DIPROSES_FARMASI)
                    {
                        txtProceedTime.Text = oSchedule.ProceedTime;
                        cboMedicationStatus.Value = oSchedule.GCMedicationStatus.ToString();
                        txtOtherMedicationStatus.Text = oSchedule.OtherMedicationStatus;
                    }

                    txtNumberOfDosage.Text = oSchedule.cfNumberOfDosage;
                    cboDosingUnit.Value = oSchedule.GCDosingUnit;
                    txtNumberOfDosage.Enabled = oSchedule.SequenceNo == "0";
                    cboDosingUnit.Enabled = oSchedule.SequenceNo == "0";
                }
            }
        }

        private void SetControlProperties()
        {
            //Load Physician
            string filterExpression = string.Format("ParentID IN ('{0}','{4}','{5}') AND StandardCodeID NOT IN ('{1}','{2}','{3}')", Constant.StandardCode.UDD_MEDICATION_STATUS, Constant.MedicationStatus.OPEN, Constant.MedicationStatus.DIPROSES_FARMASI, Constant.MedicationStatus.DISCONTINUE, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.FAMILY_RELATION);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboMedicationStatus, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.UDD_MEDICATION_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFamilyRelation, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.FAMILY_RELATION).ToList(), "StandardCodeName", "StandardCodeID");

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType IN ('{0}','{1}') AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {2})",
    Constant.ParamedicType.Nurse, Constant.ParamedicType.Bidan, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedic2, lstParamedic, "ParamedicName", "ParamedicID");
            if (AppSession.UserLogin.ParamedicID != null)
            {
                cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                cboParamedicID.Enabled = false;
            }
            else
            {
                cboParamedicID.Enabled = true;
            }
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            string recordID = hdnSelectedID.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            result = UpdateMedicationSchedule(recordID);

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string UpdateMedicationSchedule(string scheduleID)
        {
            string result = string.Empty;

            IDbContext ctx = DbFactory.Configure(true);
            MedicationScheduleDao scheduleDao = new MedicationScheduleDao(ctx);
            MedicationScheduleLogDao scheduleLogDao = new MedicationScheduleLogDao(ctx);
            try
            {
                string validationErrMsg = string.Empty;
                if (IsValid(ref validationErrMsg))
                {
                    MedicationSchedule oSchedule = BusinessLayer.GetMedicationSchedule(Convert.ToInt32(scheduleID));
                    if (oSchedule != null)
                    {
                        oSchedule.MedicationTime = hdnMedicationTime.Value;
                        if (cboMedicationStatus.Value != null)
                        {
                            oSchedule.ProceedTime = txtProceedTime.Text;
                            if (oSchedule.SequenceNo == "0") //As Required Medication
                            {
                                oSchedule.MedicationTime = txtProceedTime.Text;

                                string dosageInText = txtNumberOfDosage.Text.Replace(',', '.');
                                decimal numberOfDosage = 0;
                                if (dosageInText.Contains('/'))
                                {
                                    string[] dosageInTextInfo = dosageInText.Split('/');
                                    decimal num1 = Convert.ToDecimal(dosageInTextInfo[0]);
                                    decimal num2 = Convert.ToDecimal(dosageInTextInfo[1]);
                                    numberOfDosage = Math.Round(num1 / num2, 2);
                                }
                                else
                                {
                                    numberOfDosage = Convert.ToDecimal(dosageInText);
                                }

                                oSchedule.NumberOfDosage = numberOfDosage;
                                oSchedule.NumberOfDosageInString = dosageInText;
                            }
                            oSchedule.GCMedicationStatus = cboMedicationStatus.Value.ToString();
                            oSchedule.OtherMedicationStatus = txtOtherMedicationStatus.Text;
                            oSchedule.ParamedicID = Convert.ToInt32(cboParamedicID.Value);

                            oSchedule.IsNeedConfirmation = chkIsNeedConfirmation.Checked;
                            if (chkIsNeedConfirmation.Checked)
                            {
                                if (cboParamedic2.Value != null)
                                {
                                    oSchedule.NeedConfirmationParamedicID = Convert.ToInt32(cboParamedic2.Value);
                                }  
                            }

                            if (rblIsPatientFamily.SelectedValue == "1")
                            {
                                oSchedule.IsPatientFamily = true;
                                if (cboFamilyRelation.Value != null)
                                {
                                    if (!String.IsNullOrEmpty(cboFamilyRelation.Value.ToString()))
                                    {
                                        oSchedule.GCFamilyRelation = cboFamilyRelation.Value.ToString();
                                        oSchedule.PatientFamilyName = txtPatientFamilyName.Text;
                                    }
                                }
                            }
                        }
                        scheduleDao.Update(oSchedule);

                        #region Log Changes
                        MedicationScheduleLog oLog = new MedicationScheduleLog();
                        oLog.LogDate = DateTime.Now;
                        oLog.MedicationScheduleID = oSchedule.ID;
                        oLog.OldValues = hdnSelectedItem.Value;
                        oLog.NewValues = JsonConvert.SerializeObject(oSchedule);
                        oLog.UserID = AppSession.UserLogin.UserID;
                        scheduleLogDao.Insert(oLog);
                        #endregion
                    }
                    else
                    {
                        string message = string.Format("Invalid Medication Schedule ID {0}", scheduleID);
                        result = string.Format("process|0|{0}||", message);
                    }
                    result = string.Format("process|1|||");
                    ctx.CommitTransaction();
                }
                else
                {
                    string message = string.Format("Validation Error for Medication Schedule ID {0} : {1}", scheduleID, validationErrMsg);
                    result = string.Format("process|0|{0}||", message);
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}||", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool IsValid(ref string errMessage)
        {
            if (string.IsNullOrEmpty(hdnMedicationTime.Value) || hdnMedicationTime.Value == "__:__")
            {
                errMessage = "Medication Time must be entried";
                return false;
            }
            else
            {
                Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                if (!reg.IsMatch(hdnMedicationTime.Value))
                {
                    errMessage = "Medication time must be entried in correct format (hh:mm)";
                    return false;
                }
            }

            if (cboMedicationStatus.Value != null)
            {
                if (cboParamedicID.Value == null)
                {
                    errMessage = "You must select the paramedic (nurse) for update medication status";
                    return false;
                }

                if (string.IsNullOrEmpty(txtProceedTime.Text) || txtProceedTime.Text == "__:__")
                {
                    errMessage = "Medication proceed time must be entried";
                    return false;
                }
                else
                {
                    Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                    if (!reg.IsMatch(txtProceedTime.Text))
                    {
                        errMessage = "Medication proceed time must be entried in correct format (hh:mm)";
                        return false;
                    }
                }

                return true;
            }

            if (chkIsNeedConfirmation.Checked)
            {
                if (cboParamedic2.Value == null)
                {
                    errMessage = "Perawat yang diminta melakukan konfirmasi harus diisi.";
                    return false;
                }
                else
                {
                    if (String.IsNullOrEmpty(cboParamedic2.Value.ToString()))
                    {
                        errMessage = "Perawat yang diminta melakukan konfirmasi harus diisi.";
                        return false;
                    }
                    else 
                    {
                        if (cboParamedicID.Value == cboParamedic2.Value)
                        {
                            errMessage = "Perawat yang diminta melakukan konfirmasi harus berbeda dengan Perawat yang melakukan pemberian obat.";
                            return false;                           
                        }
                    }
                }
            }

            if (rblIsPatientFamily.SelectedValue == "1")
            {
                if (cboFamilyRelation.Value == null)
                {
                    errMessage = "Hubungan penerima informasi pemberian obat dengan pasien harus diisi";
                    return false;
                }
                else
                {
                    if (String.IsNullOrEmpty(cboFamilyRelation.Value.ToString()))
                    {
                        errMessage = "Hubungan penerima informasi pemberian obat dengan pasien harus diisi";
                        return false;
                    }
                }
            }
            return true;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }
    }
}