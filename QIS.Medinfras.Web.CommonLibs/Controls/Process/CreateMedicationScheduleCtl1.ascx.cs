using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using System.Net;
using Newtonsoft.Json;
using QIS.Medinfras.Web.Common.API.Model;
using System.IO;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class CreateMedicationScheduleCtl1 : BaseProcessPopupCtl
    {
        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            string[] paramInfo = param.Split('|');
            hdnID.Value = paramInfo[0];
            EntityToControl(paramInfo);
        }

        private void EntityToControl(string[] paramInfo)
        {
            if (paramInfo.Length >= 7)
            {
                txtMedicalNo.Text = paramInfo[1];
                txtPatientName.Text = paramInfo[2];
                
                hdnItemID.Value = paramInfo[3];
                txtDrugName.Text = paramInfo[4];
                txtSignaInfo.Text = paramInfo[5];
                txtStartDate.Text = paramInfo[6];
                txtDuration.Text = paramInfo[7];
            }
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            try
            {
                int recordID = Convert.ToInt32(hdnID.Value);
                string referenceNo = string.Empty;
                bool isError = false;

                string tmpResult = CreateMedicationSchedule();
                string[] tmpResultInfo = tmpResult.Split('|');

                isError = tmpResultInfo[0] == "0";
                errMessage = tmpResultInfo[3];
                result = !isError;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
            }
            return result;
        }

        private string CreateMedicationSchedule()
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PastMedicationDao pastMedicationDao = new PastMedicationDao(ctx);
            MedicationScheduleDao scheduleDao = new MedicationScheduleDao(ctx);
            try
            {
                PastMedication oMedication = BusinessLayer.GetPastMedication(Convert.ToInt32(hdnID.Value));

                if (oMedication != null)
                {
                    int frequency = oMedication.Frequency;

                    DateTime startDate = oMedication.LogDate;
                    Decimal duration = oMedication.DosingDuration;

                    DateTime date = startDate;

                    for (int i = 0; i < duration; i++)
                    {
                        DateTime medicationDate = startDate.AddDays(i);
                        for (int j = 1; j <= frequency; j++)
                        {
                            MedicationSchedule oSchedule = new MedicationSchedule();
                            oSchedule.VisitID = AppSession.RegisteredPatient.VisitID;
                            oSchedule.PastMedicationID = Convert.ToInt32(hdnID.Value);
                            oSchedule.ItemName = oMedication.DrugName;
                            oSchedule.MedicationDate = medicationDate;
                            oSchedule.SequenceNo = j.ToString();
                            switch (j)
                            {
                                case 1:
                                    oSchedule.MedicationTime = oMedication.Sequence1Time;
                                    break;
                                case 2:
                                    oSchedule.MedicationTime = oMedication.Sequence2Time;
                                    break;
                                case 3:
                                    oSchedule.MedicationTime = oMedication.Sequence3Time;
                                    break;
                                case 4:
                                    oSchedule.MedicationTime = oMedication.Sequence4Time;
                                    break;
                                case 5:
                                    oSchedule.MedicationTime = oMedication.Sequence5Time;
                                    break;
                                case 6:
                                    oSchedule.MedicationTime = oMedication.Sequence6Time;
                                    break;
                                default:
                                    oSchedule.MedicationTime = "00:00";
                                    break;
                            }

                            oSchedule.NumberOfDosage = oMedication.NumberOfDosage;
                            oSchedule.NumberOfDosageInString = oMedication.NumberOfDosage.ToString("G29");
                            oSchedule.GCDosingUnit = oMedication.GCDosingUnit;
                            oSchedule.ConversionFactor = 1;
                            oSchedule.ResultQuantity = 0;
                            oSchedule.ChargeQuantity = 0;
                            oSchedule.IsAsRequired = oMedication.IsAsRequired;
                            if (j == 0) oSchedule.IsMorning = true;
                            if (j == 1) oSchedule.IsNoon = true;
                            if (j == 2) oSchedule.IsEvening = true;
                            if (j == 3) oSchedule.IsNight = true;
                            oSchedule.GCRoute = oMedication.GCRoute;
                            if (oMedication.GCCoenamRule != null)
                                oSchedule.GCCoenamRule = oMedication.GCCoenamRule;
                            oSchedule.MedicationAdministration = oMedication.MedicationAdministration;
                            oSchedule.GCMedicationStatus = Constant.MedicationStatus.DIPROSES_FARMASI;
                            oSchedule.IsInternalMedication = false;
                            oSchedule.CreatedBy = AppSession.UserLogin.UserID;
                            scheduleDao.Insert(oSchedule);
                        }
                    }

                    result = string.Format("process|1|Medication schedule was created successfully for <b>{0}</b>||", txtDrugName.Text);
                    ctx.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|Error for Creating Medication Schedule for {0}|{1}", txtDrugName.Text, ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}