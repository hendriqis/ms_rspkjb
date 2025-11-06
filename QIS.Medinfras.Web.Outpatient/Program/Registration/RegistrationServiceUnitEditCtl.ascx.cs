using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.Service;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Outpatient.Program
{
    public partial class RegistrationServiceUnitEditCtl : BaseEntryPopupCtl
    {
        protected string OnGetMotherRegistrationNoFilterExpression()
        {
            return string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}') AND DepartmentID = '{3}' AND IsParturition = 1", Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, hdnDepartmentID.Value);
        }

        public override void InitializeDataControl(string param)
        {
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param)).FirstOrDefault();
            txtRegistrationID.Text = entity.RegistrationNo;
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnMRNCtl.Value = entity.MRN.ToString();
            txtMRN.Text = string.Format("{0} - {1}", entity.MedicalNo, entity.PatientName);
            hdnRegistrationID.Value = param;
            hdnRegistrationNo.Value = entity.RegistrationNo;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnParamedicID.Value = entity.ParamedicID.ToString();
            txtPhysician.Text = entity.ParamedicName;
            IsAdd = false;

            GetSettingParameter();
        }

        private void GetSettingParameter()
        {
            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}', '{1}')",
                Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY, Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE));
            hdnIsBridgingToGateway.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).ParameterValue;
            hdnProviderGatewayService.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).ParameterValue;
        }

        protected string OnGetParamedicFilterExpression()
        {
            return string.Format("ParamedicID = {0} AND DepartmentID = '{1}' AND HealthcareServiceUnitID != {2}", hdnParamedicID.Value, Constant.Facility.OUTPATIENT, hdnHealthcareServiceUnitID.Value);
        }

        protected override void OnControlEntrySetting()
        {
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool isValid = true;
            bool result = true;
            int newQueue = 0;

            //if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
            //{
                //if (hdnIsBridgingToGateway.Value == "1")
                //{
                //    //Healthcare entityHSU = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
                //    if (Constant.HealthcareGatewayProvider.RSDOSOBA == hdnProviderGatewayService.Value)
                //    {
                //        vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1",hdnRegistrationID.Value)).FirstOrDefault();
                //        //string queue = BridgingToGatewayGetQueueNo(entity.MedicalNo, entity.VisitDate, entity.ParamedicCode, txtPhysicianCode.Text, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT), entity.QueueNo.ToString(), entity.BusinessPartnerCode, "DT", hdnHealthcareServiceUnitID.Value.ToString());
                //        //string queue = string.Empty;
                //        string queue = string.Empty;
                //        string[] queueSplit = queue.Split('|');
                //        if (queueSplit[0] == "1")
                //        {
                //            newQueue = Convert.ToInt16(queueSplit[1]);
                //            result = true;
                //            isValid = true;
                //        }
                //        else
                //        {
                //            errMessage = queueSplit[1];
                //            isValid = false;
                //        }
                //    }
                //}
            //}

            if (isValid)
            {
                IDbContext ctx = DbFactory.Configure(true);
                ConsultVisitDao entityConsultVisitDao = new ConsultVisitDao(ctx);
                RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
                ConsultVisitChangeHistoryDao entityVisitHistoryDao = new ConsultVisitChangeHistoryDao(ctx);

                try
                {
                    bool isHasAssessment = false;
                    Registration entityRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
                    ConsultVisit entity = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", hdnRegistrationID.Value)).FirstOrDefault();
                    ConsultVisitChangeHistory entityVisitHistory = new ConsultVisitChangeHistory();

                    #region Validation
                    List<ChiefComplaint> entityCC = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID), ctx);
                    List<NurseChiefComplaint> entityNCC = BusinessLayer.GetNurseChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID), ctx);
                    List<PatientVisitNote> entityPVN = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID), ctx);

                    if (entityCC.Count > 0 || entityNCC.Count > 0 || entityPVN.Count > 0)
                    {
                        isHasAssessment = true;
                    }
                    #endregion

                    if (!isHasAssessment)
                    {
                        if (entityRegistration.GCRegistrationStatus != Constant.VisitStatus.CLOSED && entityRegistration.GCRegistrationStatus != Constant.VisitStatus.CANCELLED)
                        {
                            if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                            {
                                if (newQueue != 0)
                                {
                                    entity.QueueNo = Convert.ToInt16(newQueue);
                                }
                                else
                                {
                                    //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), entity.VisitDate, Convert.ToInt32(entity.Session), 0));

                                    bool isBPJS = false;
                                    if (entityRegistration.GCCustomerType == Constant.CustomerType.BPJS)
                                    {
                                        isBPJS = true;
                                    }

                                    entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), entity.VisitDate, Convert.ToInt32(entity.Session), false, isBPJS, 0, ctx, 1));
                                }
                            }

                            string filterChargesHd = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}'", entity.VisitID, Constant.TransactionStatus.VOID);
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<PatientChargesHd> lstCharegesHd = BusinessLayer.GetPatientChargesHdList(filterChargesHd, ctx);

                            if (lstCharegesHd.Count == 0)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityRegistrationDao.Update(entityRegistration);

                                entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entity.LastUpdatedDate = DateTime.Now;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityConsultVisitDao.Update(entity);

                                //entityVisitHistory.VisitID = entity.VisitID;
                                //entityVisitHistory.FromHealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                                //entityVisitHistory.FromParamedicID = Convert.ToInt32(entity.ParamedicID);
                                //entityVisitHistory.FromRoomID = Convert.ToInt32(entity.RoomID);
                                //entityVisitHistory.FromBedID = Convert.ToInt32(entity.BedID);
                                //entityVisitHistory.FromClassID = Convert.ToInt32(entity.ClassID);
                                //entityVisitHistory.FromChargeClassID = Convert.ToInt32(entity.ChargeClassID);
                                //entityVisitHistory.ToHealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                                //entityVisitHistory.ToParamedicID = Convert.ToInt32(entity.ParamedicID);
                                //entityVisitHistory.ToRoomID = Convert.ToInt32(entity.RoomID);
                                //entityVisitHistory.ToBedID = Convert.ToInt32(entity.BedID);
                                //entityVisitHistory.ToClassID = Convert.ToInt32(entity.ClassID);
                                //entityVisitHistory.ToChargeClassID = Convert.ToInt32(entity.ChargeClassID);
                                //entityVisitHistory.Remarks = "Ubah Registrasi Poli";
                                //entityVisitHistory.CreatedBy = AppSession.UserLogin.UserID;
                                //entityVisitHistory.CreatedDate = DateTime.Now;
                                //entityVisitHistoryDao.Insert(entityVisitHistory);

                                ctx.CommitTransaction();
                            }
                            else
                            {
                                result = false;
                                errMessage = "Maaf, tidak dapat mengubah data registrasi karna sudah ada transaksi yang terbentuk.";
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                ctx.RollBackTransaction();
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = "Maaf, tidak dapat mengubah data registrasi karna status pendaftaran sudah ditutup (closed).";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf, tidak bisa ubah data registrasi ini karena sudah memiliki assessmen";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        protected string GetGenderFemale()
        {
            return Constant.Gender.FEMALE;
        }

        private string BridgingToGatewayGetQueueNo(string medicalNo, DateTime date, string oldParamedicCode, string newParamedicCode, string hour, string queueNo, string businessPartnerCode, string via, string healthcareServiceUnitID)
        {
            String queue = "";

            if (hdnIsBridgingToGateway.Value == "1")
            {
                GatewayService oService = new GatewayService();
                APIMessageLog entityAPILog = new APIMessageLog();
                entityAPILog.Sender = "MEDINFRAS";
                entityAPILog.Recipient = "QUEUE ENGINE";
                string apiResult = oService.OnRegistrationChangePhysician(medicalNo, date, oldParamedicCode, newParamedicCode, hour, queueNo, businessPartnerCode, via, healthcareServiceUnitID);
                string[] apiResultInfo = apiResult.Split('|');
                if (apiResultInfo[0] == "0")
                {
                    queue = string.Format("{0}|{1}", apiResultInfo[0], apiResultInfo[1]);
                    entityAPILog.MessageDateTime = DateTime.Now;
                    entityAPILog.IsSuccess = false;
                    entityAPILog.MessageText = apiResultInfo[2];
                    entityAPILog.Response = apiResultInfo[1];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    Exception ex = new Exception(apiResultInfo[1]);
                    Helper.InsertErrorLog(ex);
                }
                else
                {
                    queue = apiResult;
                    entityAPILog.MessageDateTime = DateTime.Now;
                    entityAPILog.MessageText = apiResultInfo[2];
                    entityAPILog.Response = apiResultInfo[1];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                }
            }

            return queue;
        }
    }
}