using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class PatientDischargePlanEntryCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                IsAdd = false;
                hdnVisitID.Value = param;
                vConsultVisit9 entity = BusinessLayer.GetvConsultVisit9List(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();

                List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PLAN_DISCHARGE_NOTES_TYPE));
                Methods.SetComboBoxField<StandardCode>(cboPlanDischargeNotesType, lstSc, "StandardCodeName", "StandardCodeID");
                cboPlanDischargeNotesType.SelectedIndex = 0;
                
                EntityToControl(entity);
            }
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(chkIsPlanDischarge, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPlanDischargeDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPlanDischargeTime1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPlanDischargeTime2, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboPlanDischargeNotesType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPlanDischargeNotes, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(vConsultVisit9 entity)
        {
            hdnRegistrationDate.Value = entity.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            hdnRegistrationTime.Value = Convert.ToDateTime(entity.ActualVisitTime).ToString(Constant.FormatString.TIME_FORMAT_FULL);
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtRegistrationDateTime.Text = entity.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT) + " " + entity.ActualVisitTime;
            txtPatientInfo.Text = "(" + entity.MedicalNo + ") " + entity.PatientName;
            txtServiceUnitName.Text = entity.ServiceUnitName;
            Room entityRoom = BusinessLayer.GetRoom(entity.RoomID);
            txtRoomName.Text = entityRoom.RoomName;
            txtBedCode.Text = entity.BedCode;

            chkIsPlanDischarge.Checked = entity.IsPlanDischarge;
            if (string.IsNullOrEmpty(entity.PlanDischargeTime))
            {
                txtPlanDischargeDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtPlanDischargeTime1.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(0, 2);
                txtPlanDischargeTime2.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(3, 2);
                txtPlanDischargeNotes.Text = "";
            }
            else
            {
                txtPlanDischargeDate.Text = entity.PlanDischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtPlanDischargeTime1.Text = entity.PlanDischargeTime.Substring(0, 2);
                txtPlanDischargeTime2.Text = entity.PlanDischargeTime.Substring(3, 2);
                                cboPlanDischargeNotesType.Value = entity.GCPlanDischargeNotesType;
                txtPlanDischargeNotes.Text = entity.PlanDischargeNotes;
                String DischargePlanUpdatedDate = entity.DischargePlanUpdatedDate.ToString(Constant.FormatString.DATE_FORMAT) + " " + entity.DischargePlanUpdatedDate.ToString(Constant.FormatString.TIME_FORMAT);
                txtDischargePlanUpdatedDate.Text = DischargePlanUpdatedDate;
                UserAttribute entityUA = BusinessLayer.GetUserAttributeList(String.Format("UserID = {0}", entity.DischargePlanUpdatedBy)).FirstOrDefault();
                txtDischargePlanUpdatedBy.Text = entityUA.FullName;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                DateTime registrationDate = Helper.YYYYMMDDHourToDate(hdnRegistrationDate.Value + " " + hdnRegistrationTime.Value);
                DateTime dischargeDateSelected = Helper.YYYYMMDDHourToDate(Helper.GetDatePickerValue(txtPlanDischargeDate).ToString(Constant.FormatString.DATE_FORMAT_112) + " " + Convert.ToDateTime(string.Format("{0}:{1}", txtPlanDischargeTime1.Text, txtPlanDischargeTime2.Text)).ToString(Constant.FormatString.TIME_FORMAT_FULL));
                
                if (dischargeDateSelected > registrationDate)
                {
                    ConsultVisit entity = BusinessLayer.GetConsultVisit(Convert.ToInt32(hdnVisitID.Value));
                    if (chkIsPlanDischarge.Checked)
                    {
                        entity.PlanDischargeDate = Helper.GetDatePickerValue(txtPlanDischargeDate);
                        entity.PlanDischargeTime = string.Format("{0}:{1}", txtPlanDischargeTime1.Text, txtPlanDischargeTime2.Text);
                        entity.PlanDischargeNotes = txtPlanDischargeNotes.Text;
                        entity.GCPlanDischargeNotesType = cboPlanDischargeNotesType.Value.ToString();
                        entity.DischargePlanUpdatedDate = DateTime.Now;
                        entity.DischargePlanUpdatedBy = AppSession.UserLogin.UserID;
                    }
                    else
                    {
                        entity.PlanDischargeDate = null;
                        entity.PlanDischargeTime = null;
                        entity.PlanDischargeNotes = null;
                        entity.GCPlanDischargeNotesType = null;
                        entity.DischargePlanUpdatedDate = null;
                        entity.DischargePlanUpdatedBy = null;
                    }
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateConsultVisit(entity);
                    return true;
                }
                else
                {
                    errMessage = "Tanggal Rencana Pulang tidak boleh kurang dari Tanggal Pendaftaran !";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}