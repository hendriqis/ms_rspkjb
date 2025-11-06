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
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ChangeScheduleSequenceCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

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
                    txtItemName.Text = oSchedule.DrugName;
                    txtMedicationDate.Text = oSchedule.MedicationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtCurrentSequence.Text = oSchedule.SequenceNo;
                    int newSequence = Convert.ToInt16(oSchedule.SequenceNo) + 1;
                    txtSequenceNo.Text = newSequence > 6 ? "6" : newSequence.ToString();
                }
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

        private bool IsValid(ref string errMessage, IDbContext ctx)
        {
            if (string.IsNullOrEmpty(txtSequenceNo.Text) || txtSequenceNo.Text == "0")
            {
                errMessage = "New sequence number should be entried";
                return false;
            }
            else
            {
                int sequenceNo;
                if (!int.TryParse(txtSequenceNo.Text, out sequenceNo))
                {
                    errMessage = "Sequence number must be entried in correct format (numeric)";
                    return false;
                }
                else
                {
                    if (sequenceNo < 1 && sequenceNo > 6)
                    {
                        errMessage = "Sequence number must be entried in correct format (between 1 to 6)";
                        return false;                        
                    }
                }
            }

            string filterExpression = "1=0";
            if (hdnPrescriptionOrderDtID.Value != "" && hdnPrescriptionOrderDtID.Value != "0")
            {
                filterExpression = string.Format("VisitID = {0} AND PrescriptionOrderDetailID = {1}", AppSession.RegisteredPatient.VisitID, hdnPrescriptionOrderDtID.Value);
            }
            else if (hdnPastMedicationID.Value != "" && hdnPastMedicationID.Value != "0")
            {
                filterExpression = string.Format("VisitID = {0} AND PastMedicationID = {1}", AppSession.RegisteredPatient.VisitID, hdnPastMedicationID.Value);
            }
            filterExpression += string.Format(" AND SequenceNo = '{0}' AND IsDeleted = 0",txtSequenceNo.Text);
            if (!chkIsApplyToAllSchedule.Checked)
            {
                filterExpression += string.Format(" AND MedicationDate = '{0}'", Helper.GetDatePickerValue(Request.Form[txtMedicationDate.UniqueID]).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            List<vMedicationSchedule> lstSchedule = BusinessLayer.GetvMedicationScheduleList(filterExpression,ctx);
            if (lstSchedule.Count>0)
            {
                errMessage = "You should select empty sequence to switch.";
                return false;
            }

            return true;
        }

        private string UpdateMedicationSchedule(string recordID)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderDtDao prescOrderDtDao = new PrescriptionOrderDtDao(ctx);
            MedicationScheduleDao scheduleDao = new MedicationScheduleDao(ctx);
            MedicationScheduleLogDao scheduleLogDao = new MedicationScheduleLogDao(ctx);
            try
            {
                string validationErrMsg = string.Empty;
                if (IsValid(ref validationErrMsg, ctx))
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
                    filterExpression += string.Format(" AND SequenceNo = '{0}'", Request.Form[txtCurrentSequence.UniqueID]);
                    
                    if (!chkIsApplyToAllSchedule.Checked)
                    {
                        filterExpression += string.Format(" AND MedicationDate = '{0}'", Helper.GetDatePickerValue(Request.Form[txtMedicationDate.UniqueID]).ToString(Constant.FormatString.DATE_FORMAT_112));
                    }

                    string filterPrescOrderDt = string.Format("VisitID = {0} AND PrescriptionOrderDetailID = {1}", AppSession.RegisteredPatient.VisitID, hdnPrescriptionOrderDtID.Value);
                    PrescriptionOrderDt prescOrderDt = prescOrderDtDao.Get(Convert.ToInt32(hdnPrescriptionOrderDtID.Value));

                    string sequenceNoCurrent = Request.Form[txtCurrentSequence.UniqueID];
                    string sequenceNoNew = txtSequenceNo.Text;

                    if (Convert.ToInt32(hdnPrescriptionOrderDtID.Value) != 0)
                    {
                        if (sequenceNoCurrent == "1")
                        {
                            if (sequenceNoNew == "1")
                            {
                                prescOrderDt.Sequence1Time = prescOrderDt.Sequence1Time;
                            }
                            else if (sequenceNoNew == "2")
                            {
                                prescOrderDt.Sequence2Time = prescOrderDt.Sequence1Time;
                            }
                            else if (sequenceNoNew == "3")
                            {
                                prescOrderDt.Sequence3Time = prescOrderDt.Sequence1Time;
                            }
                            else if (sequenceNoNew == "4")
                            {
                                prescOrderDt.Sequence4Time = prescOrderDt.Sequence1Time;
                            }
                            else if (sequenceNoNew == "5")
                            {
                                prescOrderDt.Sequence5Time = prescOrderDt.Sequence1Time;
                            }
                            else if (sequenceNoNew == "6")
                            {
                                prescOrderDt.Sequence6Time = prescOrderDt.Sequence1Time;
                            }

                            prescOrderDt.Sequence1Time = "-";
                        }
                        else if (sequenceNoCurrent == "2")
                        {
                            if (sequenceNoNew == "1")
                            {
                                prescOrderDt.Sequence1Time = prescOrderDt.Sequence2Time;
                            }
                            else if (sequenceNoNew == "2")
                            {
                                prescOrderDt.Sequence2Time = prescOrderDt.Sequence2Time;
                            }
                            else if (sequenceNoNew == "3")
                            {
                                prescOrderDt.Sequence3Time = prescOrderDt.Sequence2Time;
                            }
                            else if (sequenceNoNew == "4")
                            {
                                prescOrderDt.Sequence4Time = prescOrderDt.Sequence2Time;
                            }
                            else if (sequenceNoNew == "5")
                            {
                                prescOrderDt.Sequence5Time = prescOrderDt.Sequence2Time;
                            }
                            else if (sequenceNoNew == "6")
                            {
                                prescOrderDt.Sequence6Time = prescOrderDt.Sequence2Time;
                            }

                            prescOrderDt.Sequence2Time = "-";
                        }
                        else if (sequenceNoCurrent == "3")
                        {
                            if (sequenceNoNew == "1")
                            {
                                prescOrderDt.Sequence1Time = prescOrderDt.Sequence3Time;
                            }
                            else if (sequenceNoNew == "2")
                            {
                                prescOrderDt.Sequence2Time = prescOrderDt.Sequence3Time;
                            }
                            else if (sequenceNoNew == "3")
                            {
                                prescOrderDt.Sequence3Time = prescOrderDt.Sequence3Time;
                            }
                            else if (sequenceNoNew == "4")
                            {
                                prescOrderDt.Sequence4Time = prescOrderDt.Sequence3Time;
                            }
                            else if (sequenceNoNew == "5")
                            {
                                prescOrderDt.Sequence5Time = prescOrderDt.Sequence3Time;
                            }
                            else if (sequenceNoNew == "6")
                            {
                                prescOrderDt.Sequence6Time = prescOrderDt.Sequence3Time;
                            }

                            prescOrderDt.Sequence3Time = "-";
                        }
                        else if (sequenceNoCurrent == "4")
                        {
                            if (sequenceNoNew == "1")
                            {
                                prescOrderDt.Sequence1Time = prescOrderDt.Sequence4Time;
                            }
                            else if (sequenceNoNew == "2")
                            {
                                prescOrderDt.Sequence2Time = prescOrderDt.Sequence4Time;
                            }
                            else if (sequenceNoNew == "3")
                            {
                                prescOrderDt.Sequence3Time = prescOrderDt.Sequence4Time;
                            }
                            else if (sequenceNoNew == "4")
                            {
                                prescOrderDt.Sequence4Time = prescOrderDt.Sequence4Time;
                            }
                            else if (sequenceNoNew == "5")
                            {
                                prescOrderDt.Sequence5Time = prescOrderDt.Sequence4Time;
                            }
                            else if (sequenceNoNew == "6")
                            {
                                prescOrderDt.Sequence6Time = prescOrderDt.Sequence4Time;
                            }

                            prescOrderDt.Sequence4Time = "-";
                        }
                        else if (sequenceNoCurrent == "5")
                        {
                            if (sequenceNoNew == "1")
                            {
                                prescOrderDt.Sequence1Time = prescOrderDt.Sequence5Time;
                            }
                            else if (sequenceNoNew == "2")
                            {
                                prescOrderDt.Sequence2Time = prescOrderDt.Sequence5Time;
                            }
                            else if (sequenceNoNew == "3")
                            {
                                prescOrderDt.Sequence3Time = prescOrderDt.Sequence5Time;
                            }
                            else if (sequenceNoNew == "4")
                            {
                                prescOrderDt.Sequence4Time = prescOrderDt.Sequence5Time;
                            }
                            else if (sequenceNoNew == "5")
                            {
                                prescOrderDt.Sequence5Time = prescOrderDt.Sequence5Time;
                            }
                            else if (sequenceNoNew == "6")
                            {
                                prescOrderDt.Sequence6Time = prescOrderDt.Sequence5Time;
                            }

                            prescOrderDt.Sequence5Time = "-";
                        }
                        else if (sequenceNoCurrent == "6")
                        {
                            if (sequenceNoNew == "1")
                            {
                                prescOrderDt.Sequence1Time = prescOrderDt.Sequence6Time;
                            }
                            else if (sequenceNoNew == "2")
                            {
                                prescOrderDt.Sequence2Time = prescOrderDt.Sequence6Time;
                            }
                            else if (sequenceNoNew == "3")
                            {
                                prescOrderDt.Sequence3Time = prescOrderDt.Sequence6Time;
                            }
                            else if (sequenceNoNew == "4")
                            {
                                prescOrderDt.Sequence4Time = prescOrderDt.Sequence6Time;
                            }
                            else if (sequenceNoNew == "5")
                            {
                                prescOrderDt.Sequence5Time = prescOrderDt.Sequence6Time;
                            }
                            else if (sequenceNoNew == "6")
                            {
                                prescOrderDt.Sequence6Time = prescOrderDt.Sequence6Time;
                            }

                            prescOrderDt.Sequence6Time = "-";
                        }
                        prescOrderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        prescOrderDtDao.Update(prescOrderDt);
                    }

                    List<MedicationSchedule> lstSchedule = BusinessLayer.GetMedicationScheduleList(filterExpression, ctx);
                    foreach (MedicationSchedule schedule in lstSchedule)
                    {
                        string oldSchedule = JsonConvert.SerializeObject(schedule);
                        schedule.SequenceNo = txtSequenceNo.Text;
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
                    string message = string.Format("Validation Error for Medication Schedule {0} : {1}", Request.Form[txtItemName.UniqueID], validationErrMsg);
                    result = string.Format("process|0|{0}||", message);
                    ctx.RollBackTransaction();
                }
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

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }
    }
}