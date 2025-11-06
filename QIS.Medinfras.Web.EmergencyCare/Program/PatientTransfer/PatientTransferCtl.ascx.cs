using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Emergency.Program
{
    public partial class PatientTransferCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.PATIENT_TRANSFER_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboTransferType, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboTransferType.SelectedIndex = 0;

            Bed entityBed = BusinessLayer.GetBedList(string.Format("BedID = {0}", param)).FirstOrDefault();
            vRegistration3 entity = BusinessLayer.GetvRegistration3List(string.Format("RegistrationID = {0}", entityBed.RegistrationID))[0];
            EntityToControl(entity);

            hdnDefaultDate.Value = txtTransferDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransferTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            hdnRegistrationID.Value = Convert.ToString(entity.RegistrationID);
            txtMedicalNo.Text = entity.MedicalNo;
            txtPatientName.Text = entity.PatientName;

            SettingParameterDt entitySetPar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_DPJP)).FirstOrDefault();
            hdnSetvarParamedicRole.Value = entitySetPar.ParameterValue;
        }

        private void EntityToControl(vRegistration3 entity)
        {
            hdnFromParamedicID.Value = entity.ParamedicID.ToString();
            txtFromPhysicianCode.Text = entity.ParamedicCode;
            txtFromPhysicianName.Text = entity.ParamedicName;

            hdnFromClassID.Value = entity.ClassID.ToString();
            txtFromClassCode.Text = entity.ClassCode;
            txtFromClassName.Text = entity.ClassName;

            hdnToClassID.Value = entity.ClassID.ToString();
            txtToClassCode.Text = entity.ClassCode;
            txtToClassName.Text = entity.ClassName;

            hdnFromRoomID.Value = entity.RoomID.ToString();
            txtFromRoomCode.Text = entity.RoomCode;
            txtFromRoomName.Text = entity.RoomName;

            hdnFromChargeClassID.Value = entity.ChargeClassID.ToString();
            txtFromChargeClassCode.Text = entity.ChargeClassCode;
            txtFromChargeClassName.Text = entity.ChargeClassName;

            hdnToChargeClassID.Value = entity.ChargeClassID.ToString();
            txtToChargeClassCode.Text = entity.ChargeClassCode;
            txtToChargeClassName.Text = entity.ChargeClassName;

            hdnFromBedID.Value = entity.BedID.ToString();
            Bed entityBed = BusinessLayer.GetBed(entity.BedID);
            txtFromBedCode.Text = entityBed.BedCode;

            hdnFromServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            txtFromServiceUnitCode.Text = entity.ServiceUnitCode;
            txtFromServiceUnitName.Text = entity.ServiceUnitName;

            hdnToServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            txtToServiceUnitCode.Text = entity.ServiceUnitCode;
            txtToServiceUnitName.Text = entity.ServiceUnitName;

            hdnFromSpecialtyID.Value = entity.SpecialtyID;
            txtFromSpecialtyName.Text = entity.SpecialtyName;
        }

        private void ControlToEntity(PatientTransfer entity)
        {
            entity.ToBedID = Convert.ToInt32(hdnToBedID.Value);
            entity.ToClassID = Convert.ToInt32(hdnToClassID.Value);
            entity.ToChargeClassID = Convert.ToInt32(hdnToChargeClassID.Value);
            entity.ToHealthcareServiceUnitID = Convert.ToInt32(hdnToServiceUnitID.Value);
            entity.ToParamedicID = Convert.ToInt32(hdnFromParamedicID.Value);
            entity.ToRoomID = Convert.ToInt32(hdnToRoomID.Value);
            entity.ToSpecialtyID = hdnFromSpecialtyID.Value;
            entity.GCPatientTransferType = cboTransferType.Value.ToString();
            entity.TransferDate = Helper.GetDatePickerValue(Request.Form[txtTransferDate.UniqueID]);
            entity.TransferTime = txtTransferTime.Text;
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientTransferDao entityDao = new PatientTransferDao(ctx);
            BedDao entityBedDao = new BedDao(ctx);
            ConsultVisitDao entityConsultVisitDao = new ConsultVisitDao(ctx);
            ParamedicTeamDao entityParamedicTeamDao = new ParamedicTeamDao(ctx);
            try
            {
                Bed entityToBed = entityBedDao.Get(Convert.ToInt32(hdnToBedID.Value));

                PatientTransfer patientTransfer = BusinessLayer.GetPatientTransferList(String.Format("RegistrationID = {0} AND GCPatientTransferStatus = '{1}'", hdnRegistrationID.Value, Constant.PatientTransferStatus.OPEN), ctx).FirstOrDefault();
                if (patientTransfer == null)
                {
                    if (entityToBed.GCBedStatus == Constant.BedStatus.UNOCCUPIED)
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
                            entity.GCPatientTransferStatus = Constant.PatientTransferStatus.TRANSFERRED;

                            entity.CreatedBy = AppSession.UserLogin.UserID;

                            List<Bed> lstBed = BusinessLayer.GetBedList(string.Format("BedID IN ({0},{1})", entity.FromBedID, entity.ToBedID), ctx);
                            Bed fromBed = lstBed.FirstOrDefault(p => p.BedID == entity.FromBedID);
                            Bed toBed = lstBed.FirstOrDefault(p => p.BedID == entity.ToBedID);

                            fromBed.GCBedStatus = Constant.BedStatus.UNOCCUPIED;
                            fromBed.RegistrationID = null;
                            toBed.GCBedStatus = Constant.BedStatus.OCCUPIED;
                            toBed.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);

                            fromBed.LastUpdatedBy = AppSession.UserLogin.UserID;
                            toBed.LastUpdatedBy = AppSession.UserLogin.UserID;

                            entityDao.Insert(entity);
                            entityBedDao.Update(fromBed);
                            entityBedDao.Update(toBed);
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
                        }

                        ConsultVisit entityConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value), ctx).FirstOrDefault();
                        entityConsultVisit.ParamedicID = entity.ToParamedicID;
                        entityConsultVisit.RoomID = entity.ToRoomID;
                        entityConsultVisit.BedID = entity.ToBedID;
                        entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityConsultVisitDao.Update(entityConsultVisit);

                        List<ParamedicTeam> lstParamedicTeam = BusinessLayer.GetParamedicTeamList(string.Format("RegistrationID = {0} AND GCParamedicRole = '{1}'", hdnRegistrationID.Value, hdnSetvarParamedicRole.Value), ctx);
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
                            entityParamedicTeam.GCParamedicRole = hdnSetvarParamedicRole.Value;
                            entityParamedicTeam.CreatedBy = AppSession.UserLogin.UserID;
                            entityParamedicTeamDao.Insert(entityParamedicTeam);
                        }

                        retval = "transfer";

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Tempat tidur tidak bisa digunakan.";
                        ctx.RollBackTransaction();
                    }
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