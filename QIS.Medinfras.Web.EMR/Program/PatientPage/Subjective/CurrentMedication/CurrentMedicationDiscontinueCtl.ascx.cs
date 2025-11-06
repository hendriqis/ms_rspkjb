using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.EMR.Program.PatientPage
{
    public partial class CurrentMedicationDiscontinueCtl : BasePagePatientPageEntryCtl
    {
        protected string startDateInString = "";
        public override void InitializeDataControl(string queryString)
        {
            IsAdd = true;
            hdnID.Value = queryString;
            vPastMedication entity = BusinessLayer.GetvPastMedicationList(string.Format("ID = {0}", queryString))[0];
            SetControlProperties();
            EntityToControl(entity);
        }

        protected void SetControlProperties()
        {

            string filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DISCONTINUE_MEDICATION_REASON);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboDiscontinueReason, lstSc, "StandardCodeName", "StandardCodeID");
            cboDiscontinueReason.SelectedIndex = 0;

            txtOtherDiscontinueReason.Text = "";
            txtDiscontinueDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtDiscontinueDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtOtherDiscontinueReason, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDiscontinueReason, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(vPastMedication entity)
        {
            startDateInString = entity.StartDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            divInformationLine.InnerHtml = entity.InformationLine1;
            spnDose.InnerHtml = string.Format("{0} {1} - {2} - {3}", entity.NumberOfDosage, entity.DosingUnit, entity.Route, entity.cfDoseFrequency);
            divDate.InnerHtml = entity.StartDateInString;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PastMedication obj = BusinessLayer.GetPastMedication(Convert.ToInt32(hdnID.Value));
                obj.DiscontinueDate = Helper.GetDatePickerValue(txtDiscontinueDate);
                obj.EndDate = obj.DiscontinueDate;
                obj.DosingDuration = Convert.ToDecimal((obj.EndDate - obj.StartDate).TotalDays);
                obj.GCDiscontinueReason = cboDiscontinueReason.Value.ToString();
                if (obj.GCDiscontinueReason == Constant.DiscontinueMedicationReason.OTHER)
                    obj.DiscontinueReason = txtOtherDiscontinueReason.Text;
                obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePastMedication(obj);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}