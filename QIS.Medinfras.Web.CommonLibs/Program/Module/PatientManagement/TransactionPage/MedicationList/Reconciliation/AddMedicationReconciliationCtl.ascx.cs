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
using Newtonsoft.Json;
using System.Text;
using System.Globalization;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AddMedicationReconciliationCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnPrescriptionOrderDetailID.Value = paramInfo[0];

            if (paramInfo[0] != "" && paramInfo[0] != "0")
            {
                IsAdd = false;
                hdnPopupID.Value = paramInfo[0];
                SetControlProperties();
                vPastMedication entity = BusinessLayer.GetvPastMedicationList(string.Format("ID = {0}",Convert.ToInt32(hdnPopupID.Value))).FirstOrDefault();
                EntityToControl(entity);
            }
            else
            {
                hdnPopupID.Value = "0";
                IsAdd = true;
                chkIsMasterItem.Checked = true;
                if (chkIsMasterItem.Checked)
                {
                    trItemInfo.Style.Add("display", "table-row");
                    trExternalItemInfo.Style.Add("display", "none");
                }
                else
                {
                    trItemInfo.Style.Add("display", "none");
                    trExternalItemInfo.Style.Add("display", "table-row");
                }
                SetControlProperties();
            }
        }

        private void EntityToControl(vPastMedication entity)
        {
            txtLogDate.Text = entity.LogDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            chkIsMasterItem.Checked = (entity.ItemID != null && entity.ItemID != 0);
            if (entity.ItemID != 0)
            {
                hdnDrugID.Value = entity.ItemID.ToString();
                hdnDrugName.Value = entity.DrugName;
                ledItem.Value = entity.DrugName.ToString();
                txtItemName.Text = entity.DrugName;
            }
            else
            {
                txtItemName.Text = entity.DrugName;
                hdnDrugName.Value = entity.DrugName;
                //ledItem.Value = entity.DrugName.ToString();
            }

            if (chkIsMasterItem.Checked)
            {
                trItemInfo.Style.Add("display", "table-row");
                trExternalItemInfo.Style.Add("display", "none");
            }
            else
            {
                trItemInfo.Style.Add("display", "none");
                trExternalItemInfo.Style.Add("display", "table-row");
            }

            hdnGenericName.Value = entity.GenericName;
            cboDrugForm.Value = entity.GCDrugForm;
            txtStartDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            chkIsAsRequired.Checked = entity.IsAsRequired;
            txtFrequencyNumber.Text = entity.Frequency.ToString();
            cboFrequencyTimeline.Value = entity.GCDosingFrequency;
            txtDosingDose.Text = entity.NumberOfDosage.ToString();
            cboDosingUnit.Value = entity.GCDosingUnit;
            cboCoenamRule.Value = entity.GCCoenamRule;
            cboMedicationRoute.Value = entity.GCRoute;
            cboMedicationStorage.Value = entity.GCMedicationStorage;
            chkIsExternalMedication.Checked = entity.IsExternalMedication;
            txtMedicationAdministration.Text = entity.MedicationAdministration;
            txtMedicationPurpose.Text = entity.MedicationPurpose;
            txtLastTakenDate.Text = entity.LastTakenDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtLastTakenTime.Text = entity.LastTakenTime;
            chkIsContinueInpatientMedication.Checked = entity.IsContinueInpatientMedication;

            if (chkIsContinueInpatientMedication.Checked)
            {
                txtDosingDuration.ReadOnly = false;
                trMedicationTime.Style.Add("display", "table-row");
            }
            else
            {
                txtDosingDuration.ReadOnly = true;
                trMedicationTime.Style.Add("display", "none");
            }

            txtDosingDuration.Text = entity.DosingDuration.ToString();

            txtStartTime1.Text = entity.Sequence1Time;
            txtStartTime2.Text = entity.Sequence2Time;
            txtStartTime3.Text = entity.Sequence3Time;
            txtStartTime4.Text = entity.Sequence4Time;
            txtStartTime5.Text = entity.Sequence5Time;
            txtStartTime6.Text = entity.Sequence6Time;
            txtRemarks.Text = entity.Remarks;
        }

        private void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.COENAM_RULE, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.DRUG_FORM, Constant.StandardCode.MEDICATION_STORAGE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            txtStartTime1.Attributes.Add("validationgroup", "mpAEM");
            txtStartTime2.Attributes.Add("validationgroup", "mpAEM");
            txtStartTime3.Attributes.Add("validationgroup", "mpAEM");
            txtStartTime4.Attributes.Add("validationgroup", "mpAEM");
            txtStartTime5.Attributes.Add("validationgroup", "mpAEM");
            txtStartTime6.Attributes.Add("validationgroup", "mpAEM");
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboDrugForm, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DRUG_FORM).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboMedicationStorage, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.MEDICATION_STORAGE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            cboFrequencyTimeline.SelectedIndex = 1;
            cboMedicationRoute.SelectedIndex = 0;

            txtLogDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtLastTakenDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtLastTakenTime.Text = "00:00";
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PastMedicationDao pastMedicationDao = new PastMedicationDao(ctx);

            try
            {
                if (IsValid(ref errMessage))
                {
                    PastMedication entity = new PastMedication();
                    ControlToEntity(entity);

                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entity.CreatedDate = DateTime.Now;
                    int id = pastMedicationDao.InsertReturnPrimaryKeyID(entity);

                    ctx.CommitTransaction();
                    retVal = id.ToString();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                retVal = "0";
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PastMedicationDao pastMedicationDao = new PastMedicationDao(ctx);

            try
            {
                if (IsValid(ref errMessage))
                {
                    PastMedication entity = BusinessLayer.GetPastMedication(Convert.ToInt32(hdnPopupID.Value));
                    ControlToEntity(entity);

                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                    pastMedicationDao.Update(entity);
                    int id = entity.ID;

                    ctx.CommitTransaction();
                    retVal = id.ToString();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                retVal = "0";
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private void ControlToEntity(PastMedication oMedication)
        {
            int frequency = Convert.ToInt16(txtFrequencyNumber.Text);

            DateTime startDate = Helper.GetDatePickerValue(txtStartDate.Text);
            int duration = 0;
            decimal dose = Convert.ToDecimal(txtDosingDose.Text);

            if (chkIsContinueInpatientMedication.Checked)
            {
                duration = Convert.ToInt16(Request.Form[txtDosingDuration.UniqueID].ToString());
            }

            oMedication.LogDate = Helper.GetDatePickerValue(txtLogDate.Text);
            oMedication.VisitID = AppSession.RegisteredPatient.VisitID;
            oMedication.MRN = AppSession.RegisteredPatient.MRN;
            if (chkIsMasterItem.Checked)
            {
                if (!string.IsNullOrEmpty(hdnDrugID.Value))
                {
                    oMedication.ItemID = Convert.ToInt32(hdnDrugID.Value);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(oMedication.ItemID.ToString()))
                {
                    oMedication.ItemID = null;
                }
            }
            oMedication.DrugName = txtItemName.Text;
            oMedication.GenericName = hdnGenericName.Value;
            oMedication.GCDrugForm = cboDrugForm.Value.ToString();
            oMedication.Frequency = Convert.ToInt16(frequency);
            oMedication.GCDosingFrequency = cboFrequencyTimeline.Value.ToString();
            oMedication.NumberOfDosage = dose;
            oMedication.GCDosingUnit = cboDosingUnit.Value.ToString();

            if (cboCoenamRule.Value != null)
            {
                if (!string.IsNullOrEmpty(cboCoenamRule.Value.ToString()))
                {
                    oMedication.GCCoenamRule = cboCoenamRule.Value.ToString();
                } 
            }

            oMedication.GCRoute = cboMedicationRoute.Value.ToString();
            oMedication.IsAsRequired = chkIsAsRequired.Checked;
            oMedication.IsExternalMedication = chkIsExternalMedication.Checked;
            oMedication.MedicationPurpose = txtMedicationPurpose.Text;
            oMedication.MedicationAdministration = txtMedicationAdministration.Text;

            oMedication.StartDate = startDate;
            oMedication.LastTakenDate = Helper.GetDatePickerValue(txtLastTakenDate.Text);
            oMedication.LastTakenTime = txtLastTakenTime.Text;
            if (chkIsContinueInpatientMedication.Checked)
            {
                oMedication.IsContinueInpatientMedication = chkIsContinueInpatientMedication.Checked;
                oMedication.EndDate = startDate.AddDays(duration);
                oMedication.Sequence1Time = txtStartTime1.Text;
                oMedication.Sequence2Time = txtStartTime2.Text;
                oMedication.Sequence3Time = txtStartTime3.Text;
                oMedication.Sequence4Time = txtStartTime4.Text;
                oMedication.Sequence5Time = txtStartTime5.Text;
                oMedication.Sequence6Time = txtStartTime6.Text;
            }
            else
            {
                oMedication.IsContinueInpatientMedication = false;
                oMedication.EndDate = startDate.AddDays(duration);
                string medicationTime = "00:00";
                oMedication.Sequence1Time = medicationTime;
                oMedication.Sequence2Time = medicationTime;
                oMedication.Sequence3Time = medicationTime;
                oMedication.Sequence4Time = medicationTime;
                oMedication.Sequence5Time = medicationTime;
                oMedication.Sequence6Time = medicationTime;
            }
            oMedication.DosingDuration = duration;

            oMedication.IsMedicationReconciliation = true;
            oMedication.Remarks = txtRemarks.Text;

            if (cboMedicationStorage.Value != null)
            {
                if (!string.IsNullOrEmpty(cboMedicationStorage.Value.ToString()))
                {
                    oMedication.GCMedicationStorage = cboMedicationStorage.Value.ToString();
                }
            }
            else
            {
                oMedication.GCMedicationStorage = null;
            }
        }

        private string CreateMedicationSchedule(string scheduleID)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PastMedicationDao pastMedicationDao = new PastMedicationDao(ctx);
            MedicationScheduleDao scheduleDao = new MedicationScheduleDao(ctx);
            try
            {
                string validationErrMsg = string.Empty;
                if (IsValid(ref validationErrMsg))
                {
                    int frequency = Convert.ToInt16(txtFrequencyNumber.Text);

                    DateTime startDate = Helper.GetDatePickerValue(txtStartDate.Text);
                    int duration = Convert.ToInt16(Request.Form[txtDosingDuration.UniqueID].ToString());

                    decimal dose = Convert.ToDecimal(txtDosingDose.Text);
                    bool isPRN = chkIsAsRequired.Checked;
                    bool isMorning = chkIsMorning.Checked;
                    bool isNoon = chkIsNoon.Checked;
                    bool isEvening = chkIsEvening.Checked;
                    bool isNight = chkIsNight.Checked;

                    #region PastMedication
                    PastMedication oMedication = new PastMedication();
                    oMedication.VisitID = AppSession.RegisteredPatient.VisitID;
                    oMedication.MRN = AppSession.RegisteredPatient.MRN;
                    oMedication.ItemID = null;
                    oMedication.GenericName = string.Empty;
                    oMedication.GCDrugForm = "X122^999";
                    oMedication.DrugName = txtItemName.Text;
                    oMedication.Frequency = Convert.ToInt16(frequency);
                    oMedication.GCDosingFrequency = cboFrequencyTimeline.Value.ToString();
                    oMedication.NumberOfDosage = dose;
                    oMedication.GCDosingUnit = cboDosingUnit.Value.ToString();
                    oMedication.DosingDuration = duration;
                    oMedication.GCRoute = cboMedicationRoute.Value.ToString();
                    oMedication.StartDate = startDate;
                    oMedication.EndDate = startDate.AddDays(duration);
                    oMedication.IsAsRequired = chkIsAsRequired.Checked;
                    oMedication.IsExternalMedication = chkIsExternalMedication.Checked;
                    oMedication.MedicationPurpose = txtMedicationPurpose.Text;
                    oMedication.MedicationAdministration = txtMedicationAdministration.Text;

                    oMedication.CreatedBy = AppSession.UserLogin.UserID;
                    oMedication.CreatedDate = DateTime.Now;
                    int pastMedicationID = pastMedicationDao.InsertReturnPrimaryKeyID(oMedication);
                    #endregion

                    DateTime date = startDate;

                    for (int i = 0; i < duration; i++)
                    {
                        DateTime medicationDate = startDate.AddDays(i);
                        for (int j = 1; j <= frequency; j++)
                        {
                            MedicationSchedule oSchedule = new MedicationSchedule();
                            oSchedule.VisitID = AppSession.RegisteredPatient.VisitID;
                            oSchedule.PastMedicationID = pastMedicationID;
                            oSchedule.ItemName = txtItemName.Text;
                            oSchedule.MedicationDate = medicationDate;
                            oSchedule.SequenceNo = j.ToString();
                            switch (j)
                            {
                                case 1:
                                    oSchedule.MedicationTime = txtStartTime1.Text;
                                    break;
                                case 2:
                                    oSchedule.MedicationTime = txtStartTime2.Text;
                                    break;
                                case 3:
                                    oSchedule.MedicationTime = txtStartTime3.Text;
                                    break;
                                case 4:
                                    oSchedule.MedicationTime = txtStartTime4.Text;
                                    break;
                                case 5:
                                    oSchedule.MedicationTime = txtStartTime5.Text;
                                    break;
                                case 6:
                                    oSchedule.MedicationTime = txtStartTime6.Text;
                                    break;
                                default:
                                    oSchedule.MedicationTime = "00:00";
                                    break;
                            }

                            oSchedule.NumberOfDosage = dose;
                            oSchedule.NumberOfDosageInString = dose.ToString("G29");
                            oSchedule.GCDosingUnit = cboDosingUnit.Value.ToString();
                            oSchedule.ConversionFactor = 1;
                            oSchedule.ResultQuantity = 0;
                            oSchedule.ChargeQuantity = 0;
                            oSchedule.IsAsRequired = isPRN;
                            if (j == 0) oSchedule.IsMorning = isMorning;
                            if (j == 1) oSchedule.IsNoon = isNoon;
                            if (j == 2) oSchedule.IsEvening = isEvening;
                            if (j == 3) oSchedule.IsNight = isNight;
                            oSchedule.GCRoute = cboMedicationRoute.Value.ToString();
                            if (cboCoenamRule.Value != null)
                                oSchedule.GCCoenamRule = cboCoenamRule.Value.ToString();
                            oSchedule.MedicationAdministration = txtMedicationAdministration.Text;
                            oSchedule.GCMedicationStatus = Constant.MedicationStatus.DIPROSES_FARMASI;
                            oSchedule.IsInternalMedication = false;
                            oSchedule.CreatedBy = AppSession.UserLogin.UserID;
                            scheduleDao.Insert(oSchedule);
                        }
                    }

                    result = string.Format("process|1|Medication schedule was created successfully for <b>{0}</b>||", txtItemName.Text);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = string.Format("process|0|Validation Error for Creating Medication Schedule for {0} : {1}||", txtItemName.Text, validationErrMsg);
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
            StringBuilder message = new StringBuilder();

            string itemName = hdnDrugName.Value;

            if (chkIsExternalMedication.Checked)
            {
                if (string.IsNullOrEmpty(itemName))
                {
                    message.AppendLine("Nama obat harus diisi / tidak boleh kosong|");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtItemName.Text))
                {
                    message.AppendLine("Nama obat harus diisi / tidak boleh kosong|");
                }
            }

            if (cboDosingUnit.Value != null)
            {
                if (string.IsNullOrEmpty(cboDosingUnit.Value.ToString()))
                    message.AppendLine("Satuan Dosis pemberian harus diisi / tidak boleh kosong|");
            }
            else
            {
                message.AppendLine("Satuan Dosis pemberian harus diisi / tidak boleh kosong|"); 
            }

            if (!Methods.IsNumeric(txtFrequencyNumber.Text) || txtFrequencyNumber.Text == "0")
                message.AppendLine("Frekuensi pemberian harus berupa angka dan lebih besar dari nol|");

            if (!Methods.IsNumeric(txtDosingDose.Text) || txtDosingDose.Text == "0")
                message.AppendLine("Dosis pemberian harus berupa angka dan lebih besar dari nol|");

            DateTime startDateTime = DateTime.ParseExact(string.Format("{0} {1}", txtStartDate.Text, "00:00"), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);

            if (startDateTime.Date > DateTime.Now.Date)
            {
                message.AppendLine("Tanggal Mulai Pemberian harus lebih kecil atau sama dengan tanggal hari ini.|");
            }

            DateTime lastTakenDateTime = DateTime.ParseExact(string.Format("{0} {1}", txtLastTakenDate.Text, txtLastTakenTime.Text), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);

            if (lastTakenDateTime.Date > DateTime.Now.Date)
            {
                message.AppendLine("Tanggal dan Jam Terakhir Pemberian harus lebih kecil atau sama dengan tanggal hari ini.|");
            }

            if (chkIsContinueInpatientMedication.Checked)
            {
                if (!Methods.IsNumeric(Request.Form[txtDosingDuration.UniqueID].ToString()))
                {
                    message.AppendLine("Durasi pemberian harus dalam format angka|");
                }
            }

            errMessage = message.ToString().Replace(@"|", "<br />");

            return string.IsNullOrEmpty(errMessage);
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }
    }
}