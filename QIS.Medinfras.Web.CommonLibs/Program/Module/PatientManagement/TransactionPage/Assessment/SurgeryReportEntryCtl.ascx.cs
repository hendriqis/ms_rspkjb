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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SurgeryReportEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] temp = param.Split('|');
            if (temp[0] == "add")
            {
                hdnVisitID.Value = temp[1];
                hdnMRN.Value = temp[2];
                hdnIsAdd.Value = "1";
                Helper.SetControlEntrySetting(cboSurgeryMark, new ControlEntrySetting(true, true, true), "mpEntryPopup");
                Helper.SetControlEntrySetting(cboWoundType, new ControlEntrySetting(true, true, true), "mpEntryPopup");
                Helper.SetControlEntrySetting(cboAnestasionType, new ControlEntrySetting(true, true, true), "mpEntryPopup");
                Helper.SetControlEntrySetting(cboSurgeryDelayCause, new ControlEntrySetting(true, true, true), "mpEntryPopup");

                txtStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                Helper.SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true), "mpEntryPopup");
                Helper.SetControlEntrySetting(txtStartHour, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.TIME_NOW), "mpEntryPopup");
                txtEndDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                Helper.SetControlEntrySetting(txtEndDate, new ControlEntrySetting(false, false, true), "mpEntryPopup");
                Helper.SetControlEntrySetting(txtEndHour, new ControlEntrySetting(false, false, true, Constant.DefaultValueEntry.TIME_NOW), "mpEntryPopup");

                SetControlProperties();
                GetSettingParameter();
            }
            else
            {
                hdnPatientSuregncyID.Value = temp[1];
                hdnIsAdd.Value = "0";
                vPatientSurgery entity = BusinessLayer.GetvPatientSurgeryList(String.Format("PatientSurgeryID = {0}", hdnPatientSuregncyID.Value)).FirstOrDefault();
                Helper.SetControlEntrySetting(cboSurgeryMark, new ControlEntrySetting(true, true, true), "mpEntryPopup");
                Helper.SetControlEntrySetting(cboWoundType, new ControlEntrySetting(true, true, true), "mpEntryPopup");
                Helper.SetControlEntrySetting(cboAnestasionType, new ControlEntrySetting(true, true, true), "mpEntryPopup");
                Helper.SetControlEntrySetting(cboSurgeryDelayCause, new ControlEntrySetting(true, true, true), "mpEntryPopup");
                Helper.SetControlEntrySetting(txtStartDate, new ControlEntrySetting(false, false, false), "mpEntryPopup");
                Helper.SetControlEntrySetting(txtEndDate, new ControlEntrySetting(false, false, false), "mpEntryPopup");

                SetControlProperties();
                GetSettingParameter();

                EntityToControl(entity);
            }
        }

        private void SetControlProperties()
        {
            txtStartDate.Attributes.Add("validationgroup", "mpEntryPopup");
            txtStartHour.Attributes.Add("validationgroup", "mpEntryPopup");
            txtEndDate.Attributes.Add("validationgroup", "mpEntryPopup");
            txtEndHour.Attributes.Add("validationgroup", "mpEntryPopup");
            txtItemTransaction.Attributes.Add("validationgroup", "mpEntryPopup");
            txtAntibiotikHour.Attributes.Add("validationgroup", "mpEntryPopup");

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID LIKE '%{0}%' AND IsDeleted = 0", Constant.StandardCode.OPERATION_LOCATION_SIGN));
            Methods.SetComboBoxField<StandardCode>(cboSurgeryMark, lstSc, "StandardCodeName", "StandardCodeID");
            cboSurgeryMark.SelectedIndex = 0;

            List<StandardCode> lstSc1 = BusinessLayer.GetStandardCodeList(string.Format("ParentID LIKE '%{0}%' AND IsDeleted = 0", Constant.StandardCode.WOUND_CLASIFICATION));
            Methods.SetComboBoxField<StandardCode>(cboWoundType, lstSc1, "StandardCodeName", "StandardCodeID");
            cboWoundType.SelectedIndex = 0;

            List<StandardCode> lstSc2 = BusinessLayer.GetStandardCodeList(string.Format("ParentID LIKE '%{0}%' AND IsDeleted = 0", Constant.StandardCode.ANESTHESIA_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboAnestasionType, lstSc2, "StandardCodeName", "StandardCodeID");
            cboAnestasionType.SelectedIndex = 0;

            List<StandardCode> lstSc3 = BusinessLayer.GetStandardCodeList(string.Format("ParentID LIKE '%{0}%' AND IsDeleted = 0", Constant.StandardCode.ANESTHESIA_COMPLICATION));
            Methods.SetComboBoxField<StandardCode>(cboAnestasionComplication, lstSc3, "StandardCodeName", "StandardCodeID");
            cboAnestasionComplication.SelectedIndex = 0;

            List<StandardCode> lstSc4 = BusinessLayer.GetStandardCodeList(string.Format("ParentID LIKE '%{0}%' AND IsDeleted = 0", Constant.StandardCode.DELAY_OPERATION_REASON));
            Methods.SetComboBoxField<StandardCode>(cboSurgeryDelayCause, lstSc4, "StandardCodeName", "StandardCodeID");
            cboSurgeryDelayCause.SelectedIndex = 0;

            List<Specimen> lstSpecimen = BusinessLayer.GetSpecimenList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<Specimen>(cboSpecimenType, lstSpecimen, "SpecimenName", "SpecimenID");
            cboSpecimenType.SelectedIndex = 0;
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                AppSession.UserLogin.HealthcareID,
                Constant.SettingParameter.MD_SERVICE_UNIT_OPERATING_THEATRE));
            hdnHealthcvareServiceUnitOK.Value = BusinessLayer.GetvHealthcareServiceUnitList(String.Format("ServiceUnitID = {0} AND IsUsingRegistration = 1", lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD_SERVICE_UNIT_OPERATING_THEATRE).ParameterValue)).FirstOrDefault().HealthcareServiceUnitID.ToString();
        }

        protected string OnGetFilterExpression()
        {
            string filterExpression = String.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND GCTransactionStatus != '{2}' AND GCTransactionDetailStatus != '{2}' AND IsDeleted = 0 AND GCItemType = '{3}' AND ID NOT IN (SELECT TransactionDtID FROM PatientSurgery WHERE VisitID = {0} AND IsDeleted = 0)", hdnVisitID.Value, hdnHealthcvareServiceUnitOK.Value, Constant.TransactionStatus.VOID, Constant.ItemType.PELAYANAN);
            if (!String.IsNullOrEmpty(Convert.ToString(hdnPatientSuregncyID.Value))) {
                filterExpression += String.Format(" OR ID = (SELECT TransactionDtID FROM PatientSurgery WHERE VisitID = {0} AND IsDeleted = 0 AND PatientSurgeryID = {1})", hdnVisitID.Value, hdnPatientSuregncyID.Value);
            }
            return filterExpression;
        }

        protected string OnGetFilterExpressionEdit()
        {
            string filterExpression = String.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND GCTransactionStatus != '{2}' AND GCTransactionDetailStatus != '{2}' AND IsDeleted = 0 AND GCItemType = '{3}' AND ID NOT IN (SELECT TransactionDtID FROM PatientSurgery WHERE VisitID = {0} AND IsDeleted = 0)", hdnVisitID.Value, hdnHealthcvareServiceUnitOK.Value, Constant.TransactionStatus.VOID, Constant.ItemType.PELAYANAN);
             return filterExpression;
        }

        #region Load Entity
        private void EntityToControl(vPatientSurgery entity)
        {
            hdnMRN.Value = Convert.ToString(entity.MRN);
            hdnVisitID.Value = Convert.ToString(entity.VisitID);
            txtStartDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartHour.Text = entity.StartTime;
            txtEndDate.Text = entity.EndDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtEndHour.Text = entity.EndTime;

            if (entity.IsEmergency)
            {
                rblEmergency.SelectedIndex = 1;
            }
            else
            {
                rblEmergency.SelectedIndex = 0;
            }

            cboSurgeryMark.Value = entity.GCSurgeryMark;
            cboWoundType.Value = entity.GCWoundType;
            cboAnestasionType.Value = entity.GCAnesthesiaType;
            cboAnestasionComplication.Value = entity.GCAnesthesiaComplication;
            cboSurgeryDelayCause.Value = entity.GCSurgeryDelayCause;
            chkIsTableWheelLocked.Checked = entity.IsTableWheelLocked;
            chkIsUseBedBound.Checked = entity.IsUseBedBound;

            hdnTransactionDtID.Value = Convert.ToString(entity.TransactionDtID);
            txtItemTransaction.Text = entity.ItemName1;

            if (entity.IsBloodDrain)
            {
                rblBloodDrain.SelectedIndex = 0;
                txtOtherBloodDrainType.Text = entity.OtherBloodDrainType;
            }
            else
            {
                rblBloodDrain.SelectedIndex = 1;
                txtOtherBloodDrainType.Attributes.Add("readonly", "readonly");
            }

            txtHemorrhage.Text = Convert.ToString(entity.Hemorrhage);

            if (entity.IsUsingTampon)
            {
                rblUsingTampon.SelectedIndex = 0;
                txtTamponType.Text = entity.TamponType;
            }
            else
            {
                rblUsingTampon.SelectedIndex = 1;
                txtTamponType.Attributes.Add("readonly", "readonly");
            }

            if (entity.IsUsingCatheter)
            {
                rblUsingCatheter.SelectedIndex = 0;
                txtIsCatherterType.Text = entity.IsCatheterType;
            }
            else
            {
                rblUsingCatheter.SelectedIndex = 1;
                txtIsCatherterType.Attributes.Add("readonly", "readonly");
            }

            if (entity.IsCatheterFromWard)
            {
                rblUsingCatheterFromWard.SelectedIndex = 1;
            }
            else
            {
                rblUsingCatheterFromWard.SelectedIndex = 0;
            }

            if (entity.IsSpecimenTest)
            {
                rblSpecimen.SelectedIndex = 0;
                cboSpecimenType.Value = entity.SpecimenID.ToString();
            }
            else
            {
                rblSpecimen.SelectedIndex = 1;
            }

            if (entity.IsTestPA)
            {
                rblTestPA.SelectedIndex = 0;
            }
            else
            {
                rblTestPA.SelectedIndex = 1;
            }

            if (entity.IsTestKultur)
            {
                rblTestKultur.SelectedIndex = 0;
            }
            else
            {
                rblTestKultur.SelectedIndex = 1;
            }

            if (entity.IsUsingImplant)
            {
                rblUsingImplant.SelectedIndex = 0;
                txtImplantType.Text = entity.ImplantType;
            }
            else
            {
                rblUsingImplant.SelectedIndex = 1;
                txtImplantType.Attributes.Add("readonly", "readonly");
            }

            if (entity.IsUsingAntibiotics)
            {
                rblUsingAntibiotics.SelectedIndex = 0;
                txtAntibioticsType.Text = entity.AntibioticsType;
                txtAntibiotikHour.Text = entity.AntibioticsTime;
            }
            else
            {
                rblUsingAntibiotics.SelectedIndex = 1;
                txtAntibioticsType.Attributes.Add("readonly", "readonly");
                txtAntibiotikHour.Attributes.Add("readonly", "readonly");
            }

            if (entity.IsTransferedToICU)
            {
                rblTransferedToICU.SelectedIndex = 0;
            }
            else
            {
                rblTransferedToICU.SelectedIndex = 1;
            }

            if (entity.IsSchedulleTransferToICU)
            {
                rblSchedulleTransferToICU.SelectedIndex = 0;
            }
            else
            {
                rblSchedulleTransferToICU.SelectedIndex = 1;
            }

            #region pre diagnose
            if (!String.IsNullOrEmpty(entity.PreOperativeDiagnosisID))
            {
                txtDiagnosePreCode.Text = entity.PreOperativeDiagnosisID;
                txtDiagnosePreName.Text = entity.PreOperativeDiagnosisText;
                hdnDiagnosePreName.Value = entity.PreOperativeDiagnosisText;
                txtRemarksPre.Text = entity.PreOperativeRemarks;
            }

            #endregion

            #region post diagnose
            if (!String.IsNullOrEmpty(entity.PostOperativeDiagnosisID)) {
                txtDiagnosePostCode.Text = entity.PostOperativeDiagnosisID;
                txtDiagnosePostName.Text = entity.PostOperativeDiagnosisText;
                hdnDiagnosePostName.Value = entity.PostOperativeDiagnosisText;
                txtRemarksPost.Text = entity.PostOperativeDiagnosisRemarks;
            }
            #endregion

        }
        #endregion

        protected void cbpProcessSave_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "save")
            {
                if (OnSaveAddRecord(ref errMessage))
                {
                    result += "success";
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "edit")
            {
                if (OnEditRecord(ref errMessage))
                {
                    result += "success";
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Save Entity
        private void ControlToEntity(PatientSurgery entity)
        {
            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entity.MRN = Convert.ToInt32(hdnMRN.Value);
            entity.TransactionDtID = Convert.ToInt32(hdnTransactionDtID.Value);
            entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            entity.StartTime = txtStartHour.Text;
            entity.EndDate = Helper.GetDatePickerValue(txtEndDate);
            entity.EndTime = txtEndHour.Text;

            if (rblEmergency.SelectedValue == "1")
            {
                entity.IsEmergency = false;
            }
            else
            {
                entity.IsEmergency = true;
            }

            entity.GCSurgeryMark = cboSurgeryMark.Value.ToString();
            entity.GCWoundType = cboWoundType.Value.ToString();
            entity.GCAnesthesiaType = cboAnestasionType.Value.ToString();
            entity.GCAnesthesiaComplication = cboAnestasionComplication.Value.ToString();
            entity.GCSurgeryDelayCause = cboSurgeryDelayCause.Value.ToString();
            entity.IsTableWheelLocked = chkIsTableWheelLocked.Checked;
            entity.IsUseBedBound = chkIsUseBedBound.Checked;

            if (rblBloodDrain.SelectedValue == "1")
            {
                entity.IsBloodDrain = true;
                entity.OtherBloodDrainType = txtOtherBloodDrainType.Text;
            }
            else
            {
                entity.IsBloodDrain = false;
            }

            if (!String.IsNullOrEmpty(txtHemorrhage.Text))
            {
                entity.Hemorrhage = Convert.ToDecimal(txtHemorrhage.Text);
            }

            if (rblUsingTampon.SelectedValue == "1")
            {
                entity.IsUsingTampon = true;
                entity.TamponType = txtTamponType.Text;
            }
            else
            {
                entity.IsUsingTampon = false;
            }

            if (rblUsingCatheter.SelectedValue == "1")
            {
                entity.IsUsingCatheter = true;
                entity.IsCatheterType = txtIsCatherterType.Text;
            }
            else
            {
                entity.IsUsingCatheter = false;
            }

            if (rblUsingCatheterFromWard.SelectedValue == "1")
            {
                entity.IsCatheterFromWard = false;
            }
            else
            {
                entity.IsCatheterFromWard = true;
            }

            if (rblSpecimen.SelectedValue == "1")
            {
                entity.IsSpecimenTest = true;
                entity.SpecimenID = Convert.ToInt32(cboSpecimenType.Value);
            }
            else
            {
                entity.IsSpecimenTest = false;
            }

            if (rblTestPA.SelectedValue == "1")
            {
                entity.IsTestPA = true;
            }
            else
            {
                entity.IsTestPA = false;
            }

            if (rblTestKultur.SelectedValue == "1")
            {
                entity.IsTestKultur = true;
            }
            else
            {
                entity.IsTestKultur = false;
            }

            if (rblUsingImplant.SelectedValue == "1")
            {
                entity.IsUsingImplant = true;
                entity.ImplantType = txtImplantType.Text;
            }
            else
            {
                entity.IsUsingImplant = false;
            }

            if (rblUsingAntibiotics.SelectedValue == "1")
            {
                entity.IsUsingAntibiotics = true;
                entity.AntibioticsType = txtAntibioticsType.Text;
                entity.AntibioticsTime = txtAntibiotikHour.Text;
            }
            else
            {
                entity.IsUsingAntibiotics = false;
            }

            if (rblTransferedToICU.SelectedValue == "1")
            {
                entity.IsTransferedToICU = true;
            }
            else
            {
                entity.IsTransferedToICU = false;
            }

            if (rblSchedulleTransferToICU.SelectedValue == "1")
            {
                entity.IsSchedulleTransferToICU = true;
            }
            else
            {
                entity.IsSchedulleTransferToICU = false;
            }

            if (!String.IsNullOrEmpty(txtDiagnosePreCode.Text))
            {
                entity.PreOperativeDiagnosisID = txtDiagnosePreCode.Text;
                entity.PreOperativeDiagnosisText = hdnDiagnosePreName.Value;
                entity.PreOperativeRemarks = txtRemarksPre.Text;
            }

            if (!String.IsNullOrEmpty(txtDiagnosePostCode.Text))
            {
                entity.PostOperativeDiagnosisID = txtDiagnosePostCode.Text;
                entity.PostOperativeDiagnosisText = hdnDiagnosePostName.Value;
                entity.PostOperativeDiagnosisRemarks = txtRemarksPost.Text;
            }
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientSurgeryDao entityDao = new PatientSurgeryDao(ctx);
            PatientSurgery entity = new PatientSurgery();
            try
            {
                ControlToEntity(entity);

                if (String.IsNullOrEmpty(entity.StartTime))
                {
                    entity.StartTime = "00:00";
                }

                if (String.IsNullOrEmpty(entity.EndTime))
                {
                    entity.EndTime = "00:00";
                }

                String duration = "";
                String awal = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) + " " + entity.StartTime;
                String akhir = entity.EndDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) + " " + entity.EndTime;
                duration = Function.DifferentDateTimeInTime(awal, akhir);
                entity.Duration = (entity.EndDate - entity.StartDate).Minutes;

                DateTime date1 = DateTime.ParseExact(awal, "dd-MM-yyyy HH:mm",
                                           System.Globalization.CultureInfo.InvariantCulture);
                DateTime date2 = DateTime.ParseExact(akhir, "dd-MM-yyyy HH:mm",
                                           System.Globalization.CultureInfo.InvariantCulture);
                TimeSpan durationTemp = date2 - date1;
                double totalMin = durationTemp.TotalMinutes;

                if (totalMin >= 0)
                {

                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entityDao.Insert(entity);
                    ctx.CommitTransaction();
                }
                else {
                    errMessage = "Maaf, Waktu selesai melewati waktu mulai";
                    result = false;
                    ctx.RollBackTransaction();                
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientSurgeryDao entityDao = new PatientSurgeryDao(ctx);
            PatientSurgery entity = new PatientSurgery();
            try
            {
                entity = entityDao.Get(Convert.ToInt32(hdnPatientSuregncyID.Value));
                ControlToEntity(entity);

                if (String.IsNullOrEmpty(entity.StartTime))
                {
                    entity.StartTime = "00:00";
                }

                if (String.IsNullOrEmpty(entity.EndTime))
                {
                    entity.EndTime = "00:00";
                }

                String duration = "";
                String awal = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) + " " + entity.StartTime;
                String akhir = entity.EndDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) + " " + entity.EndTime;
                duration = Function.DifferentDateTimeInTime(awal, akhir);
                entity.Duration = (entity.EndDate - entity.StartDate).Minutes;

                DateTime date1 = DateTime.ParseExact(awal, "dd-MM-yyyy HH:mm",
                                           System.Globalization.CultureInfo.InvariantCulture);
                DateTime date2 = DateTime.ParseExact(akhir, "dd-MM-yyyy HH:mm",
                                           System.Globalization.CultureInfo.InvariantCulture);
                TimeSpan durationTemp = date2 - date1;
                double totalMin = durationTemp.TotalMinutes;

                if (totalMin >= 0)
                {
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else 
                {
                    errMessage = "Maaf, Waktu selesai melewati waktu mulai";
                    result = false;
                    ctx.RollBackTransaction();                   
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
    }
}