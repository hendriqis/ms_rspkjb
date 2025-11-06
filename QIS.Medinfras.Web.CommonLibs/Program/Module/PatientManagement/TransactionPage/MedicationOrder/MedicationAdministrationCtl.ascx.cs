using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MedicationAdministrationCtl : BaseViewPopupCtl
    {

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnDispensaryServiceUnitID.Value = paramInfo[0];
            hdnSelectedMRN.Value = paramInfo[1];
            GetPatientInformation1();
            SetControlProperties();
        }

        private void GetPatientInformation1()
        {
            string filterExpression = "";
            filterExpression = string.Format("MRN = {0}", hdnSelectedMRN.Value);
            if (!string.IsNullOrEmpty(filterExpression))
            {
                vPatient entity = BusinessLayer.GetvPatientList(filterExpression).FirstOrDefault();

                if (entity != null)
                {
                    txtMRN1.Text = entity.MedicalNo;
                    txtPatientName1.Text = entity.PatientName;
                    txtPreferredName1.Text = entity.PreferredName;
                    txtBirthPlace1.Text = entity.CityOfBirth;
                    txtDOB1.Text = entity.DateOfBirthInString;
                    txtGender1.Text = entity.Gender;
                    txtAddress1.Text = entity.HomeAddress;
                }
                else
                {
                    txtMRN1.Text = string.Empty;
                    txtPatientName1.Text = string.Empty;
                    txtPreferredName1.Text = string.Empty;
                    txtBirthPlace1.Text = string.Empty;
                    txtDOB1.Text = string.Empty;
                    txtGender1.Text = string.Empty;
                    txtAddress1.Text = string.Empty;
                }
            }
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            string lstRecordID = hdnSelectedID.Value;
            string lstDate = hdnSelectedDate.Value;
            string lstStatus = hdnSelectedStatus.Value;
            string lstOtherStatus = hdnSelectedOtherStatus.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            if (IsValidated(lstDate, ref result))
                result = UpdateMedicationStatus(lstRecordID, lstDate, lstStatus, lstOtherStatus);

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private bool IsValidated(string lstTime, ref string result)
        {
            StringBuilder oMsg = new StringBuilder();

            string message = string.Empty;

            if (string.IsNullOrEmpty(cboParamedic.Value.ToString()))
                oMsg.AppendLine("Paramedic field should not leave blanks");

            if (chkIsNeedConfirmation.Checked)
            {
                if (cboParamedic2.Value == null)
                {
                    oMsg.AppendLine("Perawat yang diminta melakukan konfirmasi harus diisi.");
                }
                else
                {
                    if (String.IsNullOrEmpty(cboParamedic2.Value.ToString()))
                    {
                        oMsg.AppendLine("Perawat yang diminta melakukan konfirmasi harus diisi.");
                    }
                    else
                    {
                        if (cboParamedic.Value == cboParamedic2.Value)
                        {
                            oMsg.AppendLine("Perawat yang diminta melakukan konfirmasi harus berbeda dengan Perawat yang melakukan pemberian obat.");
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(message))
            {
                #region Validate Discontinue Date
                string[] selectedTime = lstTime.Split(',');
                foreach (string time in selectedTime)
                {
                    if (string.IsNullOrEmpty(time) || time == "__:__")
                    {
                        oMsg.AppendLine("There is medication with empty medication time.");
                        break;
                    }
                    else
                    {
                        Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                        if (!reg.IsMatch(txtMedicationTime.Text))
                        {
                            oMsg.AppendLine("Medication time must be entried in correct format (hh:mm)");
                            break;
                        }
                    } 
                }
                #endregion
            }

            if (!string.IsNullOrEmpty(message))
            {
                result = string.Format("process|0|{0}", message);
            }
            return message == string.Empty;
        }

        private string UpdateMedicationStatus(string lstRecordID, string lstTime, string lstStatus, string lstOtherStatus)
        {
            string result = string.Empty;
            String[] selectedID = lstRecordID.Split(',');
            String[] selectedTime = lstTime.Split(',');
            String[] selectedStatus = lstStatus.Split(',');
            String[] selectedOtherStatus = lstOtherStatus.Split(',');

            IDbContext ctx = DbFactory.Configure(true);
            MedicationScheduleDao scheduleDao = new MedicationScheduleDao(ctx);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);

            try
            {
                for (int ct = 0; ct < selectedID.Length; ct++)
                {
                    //DateTime date = Helper.GetDatePickerValue(selectedDate[ct]);

                    MedicationSchedule oSchedule = BusinessLayer.GetMedicationSchedule(Convert.ToInt32(selectedID[ct]));
                    if (oSchedule != null)
                    {
                        oSchedule.ProceedTime = selectedTime[ct];
                        oSchedule.GCMedicationStatus = selectedStatus[ct];
                        oSchedule.OtherMedicationStatus = selectedOtherStatus[ct];
                        oSchedule.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);

                        oSchedule.IsNeedConfirmation = chkIsNeedConfirmation.Checked;
                        if (chkIsNeedConfirmation.Checked)
                        {
                            if (cboParamedic2.Value != null)
                            {
                                oSchedule.NeedConfirmationParamedicID = Convert.ToInt32(cboParamedic2.Value);
                            }
                        }

                        BusinessLayer.UpdateMedicationSchedule(oSchedule);
                    }
                }

                ctx.CommitTransaction();

                string message = string.Format("Medication Administration was processed successfully");
                result = string.Format("process|1|{0}", message);
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        private void SetControlProperties()
        {
            //Load Physician
            string filterExpression = string.Empty;

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType IN ('{0}') AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1})",
                                                                                            Constant.ParamedicType.Nurse, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedic, lstParamedic, "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedic2, lstParamedic, "ParamedicName", "ParamedicID");
            if (AppSession.UserLogin.ParamedicID != null)
            {
                hdnParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                ParamedicMaster oParamedic = BusinessLayer.GetParamedicMaster(Convert.ToInt32(hdnParamedicID.Value));
                cboParamedic.Value = oParamedic.FullName;
                cboParamedic.Enabled = false;
            }

            filterExpression = string.Format("ParentID = '{0}' AND StandardCodeID NOT IN ('{1}','{2}','{3}')", Constant.StandardCode.UDD_MEDICATION_STATUS, Constant.MedicationStatus.OPEN, Constant.MedicationStatus.DIPROSES_FARMASI, Constant.MedicationStatus.DISCONTINUE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField(cboDefaultMedicationStatus, lstStandardCode, "StandardCodeName", "StandardCodeID");

            BindGridView();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vMedicationSchedule item = (vMedicationSchedule)e.Item.DataItem;
                List<StandardCode> lstStatusNotes = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID NOT IN ('{1}','{2}','{3}') ORDER BY StandardCodeID", Constant.StandardCode.UDD_MEDICATION_STATUS, Constant.MedicationStatus.OPEN, Constant.MedicationStatus.DIPROSES_FARMASI, Constant.MedicationStatus.DISCONTINUE));
                DropDownList cboMedicationStatus = e.Item.FindControl("cboMedicationStatus") as DropDownList;
                TextBox txtTime = e.Item.FindControl("txtTime") as TextBox;
                cboMedicationStatus.DataValueField = "StandardCodeID";
                cboMedicationStatus.DataTextField = "StandardCodeName";
                cboMedicationStatus.DataSource = lstStatusNotes;
                cboMedicationStatus.DataBind();
                txtTime.Text = item.MedicationTime;
            }
        }

        private void BindGridView()
        {
            string recordID = hdnRecordIDList.Value;
            string medicationDate = hdnMedicationDate.Value;
            string filterExpression = string.Format("VisitID = {0} AND MedicationDate = '{1}' AND GCMedicationStatus = '{2}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(medicationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.MedicationStatus.DIPROSES_FARMASI);
            if (!string.IsNullOrEmpty(recordID))
            {
                filterExpression += string.Format(" AND ID IN ({0})", recordID);
            }
            List<vMedicationSchedule> lstEntity = BusinessLayer.GetvMedicationScheduleList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected void cbpBarcodeEntry1Process_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExpression = "";
            string param = txtBarcodeEntry1.Text;
            string result = "1";

            filterExpression = Methods.GenerateFilterExpFromPatientListBarcode(filterExpression, param);
            if (!string.IsNullOrEmpty(filterExpression))
            {
                vConsultVisit entity = BusinessLayer.GetvConsultVisitList(filterExpression).FirstOrDefault();

                if (entity != null)
                {
                    txtMRN1.Text = entity.MedicalNo;
                    txtPatientName1.Text = entity.PatientName;
                    txtPreferredName1.Text = entity.PreferredName;
                    txtDOB1.Text = entity.DateOfBirthInString;
                    txtAddress1.Text = entity.HomeAddress;
                }
                else
                {
                    txtMRN1.Text = string.Empty;
                    txtPatientName1.Text = string.Empty;
                    txtPreferredName1.Text = string.Empty;
                    txtDOB1.Text = string.Empty;
                    txtAddress1.Text = string.Empty;
                    result = "0";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpValue"] = txtBarcodeEntry1.Text;
            panel.JSProperties["cpIsFound"] = result;
        }

        protected void cbpBarcodeEntry2Process_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExpression = "";
            string param = txtBarcodeEntry2.Text;
            string[] paramInfo = param.Split('|');
            string result = "1";
            string patientInfo = string.Empty;
            string medicalNo = string.Empty;

            filterExpression = Methods.GenerateFilterExpFromUDDQRCode(filterExpression, param);
            if (!string.IsNullOrEmpty(filterExpression))
            {
                vConsultVisit entity = BusinessLayer.GetvConsultVisitList(filterExpression).FirstOrDefault();
                if (entity != null)
                {
                    medicalNo = entity.MedicalNo;
                    patientInfo = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", entity.MedicalNo, entity.PatientName, entity.PreferredName, entity.CityOfBirth, entity.DateOfBirthInString, entity.Gender, entity.HomeAddress);
                }
                else
                {
                    result = "0";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpPatientInfo"] = patientInfo;
            panel.JSProperties["cpValue"] = paramInfo[3];
            panel.JSProperties["cpIsFound"] = result;
            if (medicalNo == Request.Form[txtMRN1.UniqueID])
                panel.JSProperties["cpIsMatch"] = "1";
            else
                panel.JSProperties["cpIsMatch"] = "0";
        }
    }
}