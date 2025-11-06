using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class CancelPatientRegistrationCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnVisitID.Value = param;
            List<StandardCode> lstReopenReason = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DELETE_REASON));
            Methods.SetComboBoxField<StandardCode>(cboCancelRegistrationReason, lstReopenReason, "StandardCodeName", "StandardCodeID");
            cboCancelRegistrationReason.SelectedIndex = 0;

            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                            "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}')",
                                                            AppSession.UserLogin.HealthcareID, //0
                                                            Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY, //1
                                                            Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE, //2
                                                            Constant.SettingParameter.RM_VOID_REGISTRASI_VALIDASI_DATA_CHIEF_COMPLAINT, //3
                                                            Constant.SettingParameter.RM_IS_VOID_REG_DELETE_LINKEDREG, //4
                                                            Constant.SettingParameter.SA0138, //5
                                                            Constant.SettingParameter.SA0175 //6
                                                        ));

            hdnIsBridgingToGateway.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).FirstOrDefault().ParameterValue;
            hdnProviderGatewayService.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).FirstOrDefault().ParameterValue;
            hdnMenggunakanValidasiChiefComplaint.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.RM_VOID_REGISTRASI_VALIDASI_DATA_CHIEF_COMPLAINT).FirstOrDefault().ParameterValue;
            hdnIsVoidRegistrationDeleteLinkedReg.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.RM_IS_VOID_REG_DELETE_LINKEDREG).FirstOrDefault().ParameterValue;
            hdnIsBridgingToMobileJKN.Value = lstSetParDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.SA0138).FirstOrDefault().ParameterValue;
            hdnIsSendNotifToMobileJKN.Value = lstSetParDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.SA0175).FirstOrDefault().ParameterValue;
            
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
            hdnRegID.Value = entityVisit.RegistrationID.ToString();
            hdnDeptID.Value = entityVisit.DepartmentID;
        }

        protected void cbpCancelRegistration_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnCancelRegistration(ref errMessage))
                result += "success";
            else
                result = "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnCancelRegistration(ref string errMessage)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityDao = new RegistrationDao(ctx);
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientPaymentHdDao patientPaymentHdDao = new PatientPaymentHdDao(ctx);
            AppointmentDao appointmentDao = new AppointmentDao(ctx);
            ChiefComplaintDao chiefComplaintDao = new ChiefComplaintDao(ctx);
            NurseChiefComplaintDao nurseChiefComplaintDao = new NurseChiefComplaintDao(ctx);
            PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
            BedDao entityBedDao = new BedDao(ctx);
            try
            {
                // cek apa ada outstanding pasien pindah
                string filterTransfer = string.Format("RegistrationID = {0} AND GCPatientTransferStatus = '{1}'", hdnRegID.Value, Constant.PatientTransferStatus.OPEN);
                List<PatientTransfer> lstTransfer = BusinessLayer.GetPatientTransferList(filterTransfer, ctx);
                if (lstTransfer.Count == 0)
                {
                    #region Tidak Ada Outstanding Pasien Pindah

                    // cek apa ada outstanding charges, order, payment
                    string filterOutstanding = string.Format("RegistrationID = {0} AND GCVisitStatus != '{1}'", hdnRegID.Value, Constant.VisitStatus.CANCELLED);
                    List<vRegistrationAllInfo> lstInfo = BusinessLayer.GetvRegistrationAllInfoList(filterOutstanding, ctx);
                    int oCharges = 0, oTestOrder = 0, oPrescriptionOrder = 0, oPrescriptionReturnOrder = 0, oServiceOrder = 0, oPayment = 0;
                    foreach (vRegistrationAllInfo info in lstInfo)
                    {
                        oCharges += info.Charges;
                        oTestOrder += info.TestOrder;
                        oPrescriptionOrder += info.PrescriptionOrder;
                        oPrescriptionReturnOrder += info.PrescriptionReturnOrder;
                        oServiceOrder += info.ServiceOrder;
                        oPayment += info.Payment;
                    }
                    bool outstanding = (oCharges + oTestOrder + oPrescriptionOrder + oPrescriptionReturnOrder + oServiceOrder + oPayment > 0);

                    if (!outstanding)
                    {
                        #region Tidak Ada Outstanding Charges / Order / Payment

                        Registration entity = entityDao.Get(Convert.ToInt32(hdnRegID.Value));
                        if (entity.SourceAmount == 0)
                        {
                            #region Tidak Ada Proses Transfer Tagihan

                            // list transaksi
                            string filterExpression = string.Format("RegistrationID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}' AND IsDeleted = 0", hdnRegID.Value, Constant.TransactionStatus.VOID);
                            List<vPatientChargesDt3> lstPatientChargesDt = BusinessLayer.GetvPatientChargesDt3List(filterExpression, ctx);
                            List<vPatientChargesDt3> lstPatientChargesDtTemp = lstPatientChargesDt.Where(t => !t.IsCreatedBySystem || (t.PatientBillingID != 0)).ToList();

                            // list chief complaint
                            string filterCC = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);
                            List<ChiefComplaint> lstCC = BusinessLayer.GetChiefComplaintList(filterCC, ctx);

                            // list nurse chief complaint
                            string filterNCC = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);
                            List<NurseChiefComplaint> lstNCC = BusinessLayer.GetNurseChiefComplaintList(filterCC, ctx);

                            // list patient visit note
                            string filterPVN = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);
                            List<PatientVisitNote> listPVN = BusinessLayer.GetPatientVisitNoteList(filterCC, ctx);

                            foreach (ChiefComplaint a in lstCC)
                            {
                                a.IsDeleted = true;
                                a.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                chiefComplaintDao.Update(a);
                            }

                            foreach (NurseChiefComplaint b in lstNCC)
                            {
                                b.IsDeleted = true;
                                b.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                nurseChiefComplaintDao.Update(b);
                            }

                            foreach (PatientVisitNote c in listPVN)
                            {
                                c.IsDeleted = true;
                                c.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                patientVisitNoteDao.Update(c);
                            }

                            #region Update Data Linked To Registration

                            if (entity.LinkedRegistrationID != null)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                Registration entityLinkedFrom = entityDao.Get(Convert.ToInt32(entity.LinkedRegistrationID));

                                entityLinkedFrom.LinkedToRegistrationID = null;
                                entityLinkedFrom.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityDao.Update(entityLinkedFrom);
                            }

                            #endregion

                            #region Update Data Auto Bill Item (PatientCharges)

                            lstPatientChargesDtTemp = lstPatientChargesDt.Where(t => t.IsCreatedBySystem).ToList();
                            List<string> lstTransactionID = new List<string>();
                            foreach (vPatientChargesDt3 entityDt in lstPatientChargesDtTemp)
                            {
                                PatientChargesDt entityPatientChargesDt = patientChargesDtDao.Get(entityDt.ID);
                                entityPatientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                entityPatientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                patientChargesDtDao.Update(entityPatientChargesDt);
                                if (!lstTransactionID.Any(t => t == entityDt.TransactionID.ToString()))
                                {
                                    lstTransactionID.Add(entityDt.TransactionID.ToString());
                                }
                            }

                            foreach (string entityTransactionID in lstTransactionID)
                            {
                                List<PatientChargesDt> lstChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = '{0}'", entityTransactionID), ctx).ToList();
                                if (lstChargesDt.Where(t => t.GCTransactionDetailStatus != Constant.TransactionStatus.VOID).Count() == 0)
                                {
                                    PatientChargesHd entityPatientChargesHd = patientChargesHdDao.Get(Convert.ToInt32(entityTransactionID));
                                    entityPatientChargesHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    entityPatientChargesHd.VoidBy = AppSession.UserLogin.UserID;
                                    entityPatientChargesHd.VoidDate = DateTime.Now;
                                    entityPatientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    patientChargesHdDao.Update(entityPatientChargesHd);
                                }
                            }

                            #endregion

                            #region Update Data Status Registrasi (Registration)

                            if (hdnIsVoidRegistrationDeleteLinkedReg.Value == "1")
                            {
                                entity.LinkedRegistrationID = null;
                            }

                            entity.GCRegistrationStatus = Constant.VisitStatus.CANCELLED;
                            entity.GCVoidReason = cboCancelRegistrationReason.Value.ToString();
                            if (cboCancelRegistrationReason.Value.ToString() == Constant.DeleteReason.OTHER)
                            {
                                entity.VoidReason = txtReason.Text;
                            }
                            entity.VoidBy = AppSession.UserLogin.UserID;
                            entity.VoidDate = DateTime.Now;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityDao.Update(entity);

                            #endregion

                            #region Update Data Penunggu Pasien (PatientAccompany)

                            if (hdnDeptID.Value == Constant.Facility.INPATIENT)
                            {
                                List<PatientAccompany> lstPatientAccompany = BusinessLayer.GetPatientAccompanyList(String.Format("RegistrationID = {0}", hdnRegID.Value), ctx);
                                if (lstPatientAccompany.Count > 0)
                                {
                                    foreach (PatientAccompany e in lstPatientAccompany)
                                    {
                                        Bed bedPA = entityBedDao.Get(Convert.ToInt32(e.BedID));
                                        bedPA.RegistrationID = null;
                                        bedPA.GCBedStatus = Constant.BedStatus.UNOCCUPIED;
                                        bedPA.IsPatientAccompany = false;
                                        bedPA.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityBedDao.Update(bedPA);
                                    }
                                }
                            }

                            #endregion

                            #region Update Data Status Visit (ConsultVisit + Bed)

                            List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", hdnRegID.Value), ctx);
                            foreach (ConsultVisit consultVisit in lstConsultVisit)
                            {
                                consultVisit.GCVisitStatus = Constant.VisitStatus.CANCELLED;
                                consultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                consultVisitDao.Update(consultVisit);

                                if (hdnDeptID.Value == Constant.Facility.INPATIENT)
                                {
                                    //Bed entityBed = entityBedDao.Get((int)consultVisit.BedID);
                                    Bed entityBed = BusinessLayer.GetBedList(String.Format("BedID = {0} AND IsPatientAccompany = 0", (int)consultVisit.BedID), ctx).FirstOrDefault();
                                    if (entityBed.RegistrationID == entity.RegistrationID)
                                    {
                                        entityBed.GCBedStatus = Constant.BedStatus.UNOCCUPIED;
                                        entityBed.RegistrationID = null;
                                        entityBed.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityBedDao.Update(entityBed);
                                    }
                                }
                                else if (hdnDeptID.Value == Constant.Facility.EMERGENCY)
                                {
                                    String BedID = Convert.ToString(consultVisit.BedID);
                                    if (BedID != "")
                                    {
                                        Bed entityBed = BusinessLayer.GetBedList(String.Format("BedID = {0} AND IsPatientAccompany = 0", (int)consultVisit.BedID), ctx).FirstOrDefault();
                                        entityBed.GCBedStatus = Constant.BedStatus.UNOCCUPIED;
                                        entityBed.RegistrationID = null;
                                        entityBed.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityBedDao.Update(entityBed);
                                    }
                                }
                            }

                            #endregion

                            #region Update Data Appointment

                            if (entity.AppointmentID != null)
                            {
                                Appointment appointment = BusinessLayer.GetAppointment(Convert.ToInt32(entity.AppointmentID));
                                if (!appointment.IsAutoAppointment)
                                {
                                    appointment.GCAppointmentStatus = Constant.AppointmentStatus.STARTED;
                                    appointment.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    appointmentDao.Update(appointment);
                                }
                                else
                                {
                                    appointment.GCAppointmentStatus = Constant.AppointmentStatus.DELETED;
                                    appointment.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    appointmentDao.Update(appointment);
                                }
                            }

                            #endregion

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            ctx.CommitTransaction();

                            #region Bridging Gateway

                            if (hdnIsBridgingToGateway.Value == "1")
                            {
                                if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA)
                                {
                                    string queue = string.Empty;
                                    GatewayService oService = new GatewayService();
                                    APIMessageLog entityAPILog = new APIMessageLog();
                                    entityAPILog.Sender = "MEDINFRAS";
                                    entityAPILog.Recipient = "QUEUE ENGINE";
                                    vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
                                    if (entityReg != null)
                                    {
                                        string apiResult = oService.GetQueueNoByVoidRegistration(entityReg.MedicalNo, entityReg.RegistrationDate, entityReg.ParamedicCode, entityReg.ServiceUnitCode, entityReg.RegistrationTime, Convert.ToString(entityReg.QueueNo), entityReg.HealthcareServiceUnitID.ToString());
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
                                    else
                                    {

                                    }
                                }
                            }

                            #endregion

                            if (result)
                            {
                                if (hdnIsBridgingToMobileJKN.Value == "1" && hdnIsSendNotifToMobileJKN.Value == "1")
                                {
                                    BusinessLayer.OnInsertBPJSTaskLog(entity.RegistrationID, 99, AppSession.UserLogin.UserID, DateTime.Now);
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            result = false;
                            errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_REGISTRATION_CANNOT_VOID_BECAUSE_HAS_SOURCE_AMOUNT);
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }

                        #endregion
                    }
                    else
                    {
                        result = false;
                        errMessage = "Kunjungan tidak dapat dibatalkan karena sudah memiliki order / transaksi / pembayaran.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }

                    #endregion
                }
                else
                {
                    result = false;
                    errMessage = "Pendaftaran tidak dapat dibatalkan karena masih ada outstanding pasien pindah.";
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
            return result;
        }
    }
}