using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ReservationTransferEntry : BaseViewPopupCtl
    {
        protected string GetSCProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }

        protected int serviceUnitUserCount = 0;

        public override void InitializeDataControl(string param)
        {
            hdnReservationID.Value = param;
            BedReservation reservation = BusinessLayer.GetBedReservation(Convert.ToInt32(hdnReservationID.Value));
            hdnRegistrationID.Value = Convert.ToString(reservation.RegistrationID);

            vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
            vBedReservation entityBedReservation = BusinessLayer.GetvBedReservationList(string.Format("ReservationID = {0}", hdnReservationID.Value))[0];
            SetControlProperties();
            EntityToControl(entity, entityBedReservation);
            int count = BusinessLayer.GetvPatientTransferRowCount(string.Format("RegistrationID = {0} AND GCPatientTransferStatus = '{1}'", hdnRegistrationID.Value, Constant.PatientTransferStatus.OPEN));
            hdnIsAllowAddRecord.Value = (count > 0) ? "0" : "1";
        }

        private void EntityToControl(vConsultVisit2 entity, vBedReservation entityBedReservation)
        {
            #region From
            hdnFromParamedicID.Value = entity.ParamedicID.ToString();
            txtFromPhysicianCode.Text = entity.ParamedicCode;
            txtFromPhysicianName.Text = entity.ParamedicName;

            hdnFromClassID.Value = entity.ClassID.ToString();
            txtFromClassCode.Text = entity.ClassCode;
            txtFromClassName.Text = entity.ClassName;

            hdnFromRoomID.Value = entity.RoomID.ToString();
            txtFromRoomCode.Text = entity.RoomCode;
            txtFromRoomName.Text = entity.RoomName;

            hdnFromChargeClassID.Value = entity.ChargeClassID.ToString();
            txtFromChargeClassCode.Text = entity.ChargeClassCode;
            txtFromChargeClassName.Text = entity.ChargeClassName;

            hdnFromBedID.Value = entity.BedID.ToString();
            Bed entityBed = BusinessLayer.GetBed(entity.BedID);
            txtFromBedCode.Text = entityBed.BedCode;

            hdnFromServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            txtFromServiceUnitCode.Text = entity.ServiceUnitCode;
            txtFromServiceUnitName.Text = entity.ServiceUnitName;

            hdnFromSpecialtyID.Value = entity.SpecialtyID;
            txtFromSpecialtyName.Text = entity.SpecialtyName;

            hdnToParamedicID.Value = entity.ParamedicID.ToString();
            txtToPhysicianCode.Text = entity.ParamedicCode;
            txtToPhysicianName.Text = entity.ParamedicName;
            #endregion

            #region To
            hdnToClassID.Value = entityBedReservation.ClassID.ToString();
            txtToClassCode.Text = entityBedReservation.ClassCode;
            txtToClassName.Text = entityBedReservation.ClassName;

            if (entityBedReservation.RoomID.ToString() != "0")
            {
                hdnToRoomID.Value = entityBedReservation.RoomID.ToString();
                txtToRoomCode.Text = entityBedReservation.RoomCode;
                txtToRoomName.Text = entityBedReservation.RoomName;
            }
            else
            {
                hdnToRoomID.Value = "";
                txtToRoomCode.Text = "";
                txtToRoomName.Text = "";
            }

            hdnToChargeClassID.Value = entityBedReservation.ChargeClassID.ToString();
            txtToChargeClassCode.Text = entityBedReservation.ChargeClassCode;
            txtToChargeClassName.Text = entityBedReservation.ChargeClassName;

            if (entityBedReservation.BedID.ToString() != "0")
            {
                hdnToBedID.Value = entityBedReservation.BedID.ToString();
                Bed entityBedReservation1 = BusinessLayer.GetBed(entityBedReservation.BedID);
                txtToBedCode.Text = entityBedReservation1.BedCode;
            }
            else
            {
                hdnToBedID.Value = "";
                txtToBedCode.Text = "";
            }

            if (entityBedReservation.HealthcareServiceUnitID.ToString() != "0")
            {
                hdnToServiceUnitID.Value = entityBedReservation.HealthcareServiceUnitID.ToString();
                txtToServiceUnitCode.Text = entityBedReservation.ServiceUnitCode;
                txtToServiceUnitName.Text = entityBedReservation.ServiceUnitName;
            }
            else
            {
                hdnToServiceUnitID.Value = "";
                txtToServiceUnitCode.Text = "";
                txtToServiceUnitName.Text = "";
            }
            #endregion
        }

        private void ControlToEntity(PatientTransfer entity)
        {
            entity.ToBedID = Convert.ToInt32(hdnToBedID.Value);
            entity.ToClassID = Convert.ToInt32(hdnToClassID.Value);
            entity.ToChargeClassID = Convert.ToInt32(hdnToChargeClassID.Value);
            entity.ToHealthcareServiceUnitID = Convert.ToInt32(hdnToServiceUnitID.Value);
            entity.ToParamedicID = Convert.ToInt32(hdnToParamedicID.Value);
            entity.ToRoomID = Convert.ToInt32(hdnToRoomID.Value);
            entity.ToSpecialtyID = cboToSpecialty.Value.ToString();
            entity.GCPatientTransferType = cboTransferType.Value.ToString();
            entity.TransferDate = Helper.GetDatePickerValue(Request.Form[txtTransferDate.UniqueID]);
            entity.TransferTime = txtTransferTime.Text;
            entity.Remarks = txtRemarks.Text;
        }

        private void SetControlProperties()
        {
            List<Specialty> lstSpecialty = BusinessLayer.GetSpecialtyList("IsDeleted = 0");
            Methods.SetComboBoxField<Specialty>(cboToSpecialty, lstSpecialty, "SpecialtyName", "SpecialtyID");
            cboToSpecialty.SelectedIndex = 0;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.PATIENT_TRANSFER_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboTransferType, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboTransferType.SelectedIndex = 0;

            hdnDefaultDate.Value = txtTransferDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransferTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int RegistrationID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0].Contains("process"))
            {
                if (OnSaveAddRecord(ref errMessage, RegistrationID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveAddRecord(ref string errMessage, int RegistrationID)
        {
            List<Bed> lstBedCheck = BusinessLayer.GetBedList(string.Format("BedID IN ({0},{1})", hdnFromBedID.Value, hdnToBedID.Value));
            Bed fromBedCheck = lstBedCheck.FirstOrDefault(p => p.BedID == Convert.ToInt32(hdnFromBedID.Value));
            Bed toBedCheck = lstBedCheck.FirstOrDefault(p => p.BedID == Convert.ToInt32(hdnToBedID.Value));
            int currRegistrationID = hdnRegistrationID.Value == "" ? 0 : Convert.ToInt32(hdnRegistrationID.Value);
            if (((fromBedCheck.GCBedStatus == Constant.BedStatus.OCCUPIED || fromBedCheck.GCBedStatus == Constant.BedStatus.WAIT_TO_BE_TRANSFERRED) && fromBedCheck.RegistrationID == currRegistrationID
                && toBedCheck.RegistrationID == null && toBedCheck.GCBedStatus == Constant.BedStatus.UNOCCUPIED)
                || toBedCheck.RegistrationID == currRegistrationID)
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PatientTransferDao entityDao = new PatientTransferDao(ctx);
                BedDao entityBedDao = new BedDao(ctx);
                ConsultVisitDao entityConsultVisitDao = new ConsultVisitDao(ctx);
                ParamedicTeamDao entityParamedicTeamDao = new ParamedicTeamDao(ctx);
                BedReservationDao entityReservationDao = new BedReservationDao(ctx);
                try
                {
                    PatientTransfer patientTransfer = BusinessLayer.GetPatientTransferList(String.Format("RegistrationID = {0} AND GCPatientTransferStatus = '{1}'", Convert.ToInt32(hdnRegistrationID.Value), Constant.PatientTransferStatus.OPEN)).FirstOrDefault();
                    if (patientTransfer == null)
                    {
                        PatientTransfer entity = new PatientTransfer();
                        if (hdnFromBedID.Value != hdnToBedID.Value)
                        {
                            ControlToEntity(entity);

                            entity.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                            entity.FromBedID = Convert.ToInt32(hdnFromBedID.Value);
                            entity.FromClassID = Convert.ToInt32(hdnFromClassID.Value);
                            entity.FromChargeClassID = Convert.ToInt32(hdnFromChargeClassID.Value);
                            entity.FromHealthcareServiceUnitID = Convert.ToInt32(hdnFromServiceUnitID.Value);
                            entity.FromParamedicID = Convert.ToInt32(hdnFromParamedicID.Value);
                            entity.FromRoomID = Convert.ToInt32(hdnFromRoomID.Value);
                            entity.FromSpecialtyID = hdnFromSpecialtyID.Value;
                            entity.GCPatientTransferStatus = Constant.PatientTransferStatus.OPEN;

                            entity.CreatedBy = AppSession.UserLogin.UserID;

                            List<Bed> lstBed = BusinessLayer.GetBedList(string.Format("BedID IN ({0},{1})", entity.FromBedID, entity.ToBedID), ctx);
                            Bed fromBed = lstBed.FirstOrDefault(p => p.BedID == entity.FromBedID);
                            Bed toBed = lstBed.FirstOrDefault(p => p.BedID == entity.ToBedID);

                            fromBed.GCBedStatus = Constant.BedStatus.WAIT_TO_BE_TRANSFERRED;
                            toBed.GCBedStatus = Constant.BedStatus.BOOKED;
                            toBed.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);

                            fromBed.LastUpdatedBy = AppSession.UserLogin.UserID;
                            toBed.LastUpdatedBy = AppSession.UserLogin.UserID;

                            entityDao.Insert(entity);
                            entityBedDao.Update(fromBed);
                            entityBedDao.Update(toBed);

                            ConsultVisit entityConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value), ctx).FirstOrDefault();
                            entityConsultVisit.ParamedicID = entity.ToParamedicID;
                            entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityConsultVisitDao.Update(entityConsultVisit);
                        }
                        else
                        {
                            ControlToEntity(entity);

                            entity.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                            entity.FromBedID = Convert.ToInt32(hdnFromBedID.Value);
                            entity.FromClassID = Convert.ToInt32(hdnFromClassID.Value);
                            entity.FromChargeClassID = Convert.ToInt32(hdnFromChargeClassID.Value);
                            entity.FromHealthcareServiceUnitID = Convert.ToInt32(hdnFromServiceUnitID.Value);
                            entity.FromParamedicID = Convert.ToInt32(hdnFromParamedicID.Value);
                            entity.FromRoomID = Convert.ToInt32(hdnFromRoomID.Value);
                            entity.FromSpecialtyID = hdnFromSpecialtyID.Value;
                            entity.GCPatientTransferStatus = Constant.PatientTransferStatus.TRANSFERRED;
                            entity.CreatedBy = AppSession.UserLogin.UserID;
                            entityDao.Insert(entity);

                            ConsultVisit entityConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value), ctx).FirstOrDefault();
                            entityConsultVisit.ParamedicID = entity.ToParamedicID;
                            entityConsultVisit.ClassID = entity.ToClassID;
                            entityConsultVisit.ChargeClassID = entity.ToChargeClassID;
                            entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityConsultVisitDao.Update(entityConsultVisit);
                        }
                        SettingParameterDt entitySetPar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_DPJP))[0];
                        List<ParamedicTeam> lstParamedicTeam = BusinessLayer.GetParamedicTeamList(string.Format("RegistrationID = {0} AND GCParamedicRole = '{1}'", hdnRegistrationID.Value, entitySetPar.ParameterValue.ToString()));
                        if (lstParamedicTeam.Count > 0)
                        {
                            ParamedicTeam entityParamedicTeam = lstParamedicTeam[0];
                            entityParamedicTeam.ParamedicID = entity.ToParamedicID;
                            entityParamedicTeam.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityParamedicTeamDao.Update(entityParamedicTeam);
                        }
                        else
                        {
                            ParamedicTeam entityParamedicTeam = new ParamedicTeam();
                            entityParamedicTeam.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                            entityParamedicTeam.ParamedicID = entity.ToParamedicID;
                            entityParamedicTeam.GCParamedicRole = entitySetPar.ParameterValue.ToString();
                            entityParamedicTeam.CreatedBy = AppSession.UserLogin.UserID;
                            entityParamedicTeamDao.Insert(entityParamedicTeam);
                        }

                        BedReservation entityReservation = BusinessLayer.GetBedReservation(Convert.ToInt32(hdnReservationID.Value));
                        entityReservation.GCReservationStatus = Constant.Bed_Reservation_Status.COMPLETE;
                        entityReservationDao.Update(entityReservation);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Please Transfer / Cancel All Opened Patient Transfer For This Registration First.";
                        ctx.RollBackTransaction();
                    }

                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = ex.Message;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
                return result;
            }
            errMessage = "This Bed Cannot Be Used";
            return false;
        }
    }
}