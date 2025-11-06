using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;


namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class PatientDischarge : BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.INACBGS_PATIENT_DISCHARGE;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = IsAllowEdit = false;
        }

        protected string RegistrationDateTime = "";
        protected override void InitializeDataControl()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.PATIENT_OUTCOME, Constant.StandardCode.DISCHARGE_ROUTINE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            Methods.SetComboBoxField<StandardCode>(cboPatientOutcome, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PATIENT_OUTCOME).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischageRoutine, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_ROUTINE).ToList(), "StandardCodeName", "StandardCodeID");

            RegistrationDateTime = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            RegistrationDateTime += AppSession.RegisteredPatient.VisitTime.Replace(":", "");

            Helper.SetControlEntrySetting(txtDischargeDate, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(txtDischargeTime, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboDischageRoutine, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboPatientOutcome, new ControlEntrySetting(true, true, true), "mpPatientDischarge");

            ConsultVisit entity = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);
            if (entity.GCDischargeCondition != "" && entity.GCDischargeMethod != "")
            {
                EntityToControl(entity);
            }
            else
            {
                txtDischargeDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }
        }

        private void EntityToControl(ConsultVisit entity)
        {
            cboPatientOutcome.Value = entity.GCDischargeCondition;
            cboDischageRoutine.Value = entity.GCDischargeMethod;
            txtDischargeDate.Text = entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDischargeTime.Text = entity.DischargeTime;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "process")
            {
                try
                {
                    ConsultVisit entity = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);
                    //entity.GCVisitStatus = Constant.RegistrationStatus.PHYSICIAN_DISCHARGE;
                    entity.GCDischargeCondition = cboPatientOutcome.Value.ToString();
                    entity.GCDischargeMethod = cboDischageRoutine.Value.ToString();
                    entity.DischargeDate = Helper.GetDatePickerValue(txtDischargeDate);
                    entity.DischargeTime = txtDischargeTime.Text;
                    entity.LOSInDay = Convert.ToDecimal(hdnLOSInDay.Value);
                    entity.LOSInHour = Convert.ToDecimal(hdnLOSInHour.Value);
                    entity.LOSInMinute = Convert.ToDecimal(hdnLOSInMinute.Value);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateConsultVisit(entity);
                    return true;
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    return false;
                }
            }
            return false;
        }
    }
}