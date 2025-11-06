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
    public partial class EditMedicationScheduleCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            this.PopupTitle = "Edit Medication Schedule";

            string[] paramInfo = param.Split('|');
            SetControlProperties();

            if (paramInfo.Length > 1)
            {
                int scheduleID = Convert.ToInt32(paramInfo[0]);
                hdnSelectedID.Value = paramInfo[0];
                hdnPrescriptionOrderDtID.Value = paramInfo[1];
                hdnPastMedicationID.Value = paramInfo[2];

                string filterExp = string.Format("ID = {0}", scheduleID);
                vMedicationSchedule oSchedule = BusinessLayer.GetvMedicationScheduleList(filterExp).FirstOrDefault();
                if (oSchedule != null)
                {
                    //Serialize old object 
                    hdnSelectedItem.Value = JsonConvert.SerializeObject(oSchedule);
                    hdnMedicationDate.Value = oSchedule.MedicationDate.ToString(Constant.FormatString.DATE_FORMAT_112);

                    txtMedicationDate.Text = oSchedule.MedicationDateInString;
                    txtSequenceNo.Text = oSchedule.SequenceNo;
                    txtItemName.Text = oSchedule.DrugName;
                    txtMedicationTime.Text = oSchedule.MedicationTime;
                }
            }
        }

        private void SetControlProperties()
        {
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
                    string filterExpression = "1=0";
                    if (hdnPrescriptionOrderDtID.Value != "" && hdnPrescriptionOrderDtID.Value != "0")
                    {
                        filterExpression = string.Format("VisitID = {0} AND PrescriptionOrderDetailID = {1}", AppSession.RegisteredPatient.VisitID, hdnPrescriptionOrderDtID.Value);
                    }
                    else if (hdnPastMedicationID.Value != "" && hdnPastMedicationID.Value != "0")
                    {
                        filterExpression = string.Format("VisitID = {0} AND PastMedicationID = {1}", AppSession.RegisteredPatient.VisitID, hdnPastMedicationID.Value);
                    }
                    filterExpression += string.Format(" AND SequenceNo = '{0}'", Request.Form[txtSequenceNo.UniqueID]);

                    if (chkIsApplyToAllSchedule.Checked)
                    {
                        filterExpression += string.Format(" AND MedicationDate >= '{0}'", hdnMedicationDate.Value);
                    }
                    else
                    {
                        filterExpression += string.Format(" AND ID = {0}", hdnSelectedID.Value);
                    }

                    List<MedicationSchedule> lstSchedule = BusinessLayer.GetMedicationScheduleList(filterExpression, ctx);
                    foreach (MedicationSchedule schedule in lstSchedule)
                    {
                        string oldSchedule = JsonConvert.SerializeObject(schedule);
                        schedule.MedicationTime = txtMedicationTime.Text;
                        schedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                        schedule.LastUpdatedDate = DateTime.Now;
                        scheduleDao.Update(schedule);

                        #region Log Changes
                        MedicationScheduleLog oLog = new MedicationScheduleLog();
                        oLog.LogDate = DateTime.Now;
                        oLog.MedicationScheduleID = schedule.ID;
                        oLog.OldValues = oldSchedule;
                        oLog.NewValues = JsonConvert.SerializeObject(schedule);
                        oLog.UserID = AppSession.UserLogin.UserID;
                        oLog.Remarks = txtRemarks.Text;
                        scheduleLogDao.Insert(oLog);
                        #endregion
                    }

                    ctx.CommitTransaction();

                    string message = string.Format("Medication schedule was changed successfully");
                    result = string.Format("process|1|{0}", message);
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
            if (string.IsNullOrEmpty(txtMedicationTime.Text) || txtMedicationTime.Text == "__:__")
            {
                errMessage = "Medication Time must be entried";
                return false;
            }
            else
            {
                Regex reg = new Regex(@"^(?:[01][0-9]|2[0-4]):[0-5][0-9]$");
                if (!reg.IsMatch(txtMedicationTime.Text))
                {
                    errMessage = "Medication time must be entried in correct format (hh:mm)";
                    return false;
                }
            }
            return true;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }
    }
}