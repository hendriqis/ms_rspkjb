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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ConfirmMedicationAdministrationPHCtl1 : BaseViewPopupCtl
    {

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnSelectedMRN.Value = paramInfo[0];
            hdnParamedicID.Value = paramInfo[1];
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
                    txtDOB1.Text = entity.DateOfBirthInString;
                    txtGender1.Text = entity.Gender;
                    txtAddress1.Text = entity.HomeAddress;
                }
                else
                {
                    txtMRN1.Text = string.Empty;
                    txtPatientName1.Text = string.Empty;
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
            string message = string.Empty;

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
                    MedicationSchedule oSchedule = BusinessLayer.GetMedicationSchedule(Convert.ToInt32(selectedID[ct]));
                    if (oSchedule != null)
                    {

                        oSchedule.IsConfirmed = true;
                        oSchedule.NeedConfirmationDateTime = DateTime.Now;
                        BusinessLayer.UpdateMedicationSchedule(oSchedule);
                    }
                }

                ctx.CommitTransaction();

                string message = string.Format("Konfirmasi Pemberian Obat berhasil dilakukan.");
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

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType IN ('{0}','{1}')",
    Constant.ParamedicType.Nurse, Constant.ParamedicType.Bidan));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedic, lstParamedic, "ParamedicName", "ParamedicID");
            if (AppSession.UserLogin.ParamedicID != null)
            {
                cboParamedic.Value = AppSession.UserLogin.ParamedicID.ToString();
                cboParamedic.Enabled = false;
            }

            List<StandardCode> lstSC = new List<StandardCode>();
            for (int a = 1; a <= 6; a++)
            {
                StandardCode sc = new StandardCode()
                {
                    StandardCodeID = a.ToString(),
                    StandardCodeName = a.ToString()
                };
                lstSC.Add(sc);
            }
            Methods.SetComboBoxField<StandardCode>(cboSequence, lstSC, "StandardCodeName", "StandardCodeID");   
            cboSequence.SelectedIndex = 0;

            txtMedicationDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
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
            string filterExpression = string.Format("VisitID = {0} AND MedicationDate = '{1}' AND GCMedicationStatus IN ('{2}','{3}','{4}','{5}','{6}','{7}') AND IsDeleted = 0 AND (IsNeedConfirmation = 1 AND (IsConfirmed = 0 OR IsConfirmed IS NULL) AND NeedConfirmationParamedicID = {8})", AppSession.RegisteredPatient.VisitID,
    Helper.GetDatePickerValue(txtMedicationDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.MedicationStatus.TELAH_DIBERIKAN, Constant.MedicationStatus.DI_TUNDA, Constant.MedicationStatus.DISCONTINUE, Constant.MedicationStatus.PASIEN_ABSEN, Constant.MedicationStatus.PASIEN_MENOLAK, Constant.MedicationStatus.PASIEN_PUASA, hdnParamedicID.Value);
            if (cboSequence.Value != null)
            {
                filterExpression += string.Format(" AND SequenceNo = '{0}'", cboSequence.Value);
            }
            filterExpression += " ORDER BY ID";

            List<vMedicationSchedule> lstEntity = BusinessLayer.GetvMedicationScheduleList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpMedicalChartProcessView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }
    }
}