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
using QIS.Data.Core.Dal;
using System.Data;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class PatientTransferEntry : BasePageListEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.PATIENT_TRANSFER;
        }

        #region List
        protected int PageCount = 1;
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hdnIsBridgingToAplicares.Value = AppSession.IsBridgingToAPLICARES ? "1" : "0";

                hdnRegistrationID.Value = Page.Request.QueryString["id"];
                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
                SetControlProperties();
                EntityToControl(entity);
                int count = BusinessLayer.GetvPatientTransferRowCount(string.Format("RegistrationID = {0} AND GCPatientTransferStatus = '{1}'", hdnRegistrationID.Value, Constant.PatientTransferStatus.OPEN));
                hdnIsAllowAddRecord.Value = (count > 0) ? "0" : "1";

                BindGridView(1, true, ref PageCount);
            }
            base.OnLoad(e);
        }

        private void EntityToControl(vConsultVisit2 entity)
        {
            ctlPatientBanner.InitializePatientBanner(entity);

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

            if (BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IP_CODE_TYPE_IS_MOVE_CLASS_TRANSFER).ParameterValue == "1" && BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IP_ICU_CLASS_CODE).ParameterValue == Convert.ToString(hdnToClassID.Value))
            {
                hdnToChargeClassID.Value = entity.ChargeClassID.ToString();
                txtToChargeClassCode.Text = entity.ChargeClassCode;
                txtToChargeClassName.Text = entity.ChargeClassName;
            }
        }

        protected override void InitializeDataControl()
        {
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IP0036, Constant.SettingParameter.IP0040));

            hdnIsAutoConfirm.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IP0036).ParameterValue;
            hdnIsAutomaticInputChargeClass.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.IP0040).FirstOrDefault().ParameterValue;

            BindGridView(1, true, ref PageCount);
        }

        protected Boolean GetIsMoveClassTransfer()
        {
            Boolean result = false;
            if (BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID ,Constant.SettingParameter.IP_CODE_TYPE_IS_MOVE_CLASS_TRANSFER).ParameterValue == "1")
            {
                result = true;
            }
            return result;
        }

        protected String GetIsICUClass()
        {
            return BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IP_ICU_CLASS_CODE).ParameterValue;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("RegistrationID = {0} AND GCPatientTransferStatus != '{1}'", hdnRegistrationID.Value, Constant.PatientTransferStatus.CANCELLED);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientTransferRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vPatientTransfer> lstEntity = BusinessLayer.GetvPatientTransferList(filterExpression, 8, pageIndex, "ID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PatientTransferDao entityDao = new PatientTransferDao(ctx);
                BedDao entityBedDao = new BedDao(ctx);
                try
                {
                    PatientTransfer entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                    if (entity.GCPatientTransferStatus == Constant.PatientTransferStatus.OPEN)
                    {
                        entity.GCPatientTransferStatus = Constant.PatientTransferStatus.CANCELLED;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);

                        Bed entityBed = entityBedDao.Get(entity.ToBedID);
                        entityBed.RegistrationID = null;
                        entityBed.GCBedStatus = Constant.BedStatus.UNOCCUPIED;
                        entityBed.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityBedDao.Update(entityBed);

                        Bed entityFromBed = entityBedDao.Get(entity.FromBedID);
                        entityFromBed.GCBedStatus = Constant.BedStatus.OCCUPIED;
                        entityBed.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityBedDao.Update(entityFromBed);

                        ctx.CommitTransaction();
                    }
                    else 
                    {
                        ctx.RollBackTransaction();
                        errMessage = "This Data Has Been Approved / Cancel.";
                        result = false;                    
                    }
                }
                catch (Exception ex)
                {
                    ctx.RollBackTransaction();
                    errMessage = ex.Message;
                    result = false;
                }
                finally
                {
                    ctx.Close();
                }
                return result;
            }
            return false;
        }
        #endregion

        #region Entry
        protected override void SetControlProperties()
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

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtToBedCode, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtToChargeClassCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtToClassCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtToPhysicianCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtToRoomCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtToServiceUnitCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTransferDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTransferTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboToSpecialty, new ControlEntrySetting(true, true, true));
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

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            List<Bed> lstBed = BusinessLayer.GetBedList(string.Format("BedID IN ({0},{1})", hdnFromBedID.Value, hdnToBedID.Value));
            Bed fromBed = lstBed.FirstOrDefault(p => p.BedID == Convert.ToInt32(hdnFromBedID.Value));
            Bed toBed = lstBed.FirstOrDefault(p => p.BedID == Convert.ToInt32(hdnToBedID.Value));
            int currRegistrationID = hdnRegistrationID.Value == "" ? 0 : Convert.ToInt32(hdnRegistrationID.Value);
            if (((fromBed.GCBedStatus == Constant.BedStatus.OCCUPIED || fromBed.GCBedStatus == Constant.BedStatus.WAIT_TO_BE_TRANSFERRED) && fromBed.RegistrationID == currRegistrationID 
                &&  toBed.RegistrationID == null && toBed.GCBedStatus == Constant.BedStatus.UNOCCUPIED)
                || toBed.RegistrationID == currRegistrationID)
                return true;

            errMessage = "This Bed Cannot Be Used";
            return false;
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            return OnBeforeSaveAddRecord(ref errMessage);
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientTransferDao entityDao = new PatientTransferDao(ctx);
            BedDao entityBedDao = new BedDao(ctx);
            ConsultVisitDao entityConsultVisitDao = new ConsultVisitDao(ctx);
            ParamedicTeamDao entityParamedicTeamDao = new ParamedicTeamDao(ctx);

            BedReservationDao bedReservationDao = new BedReservationDao(ctx);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            PatientTransferDao entityPatientTransferDao = new PatientTransferDao(ctx);
            NutritionOrderHdDao entityNutritionOrderDao = new NutritionOrderHdDao(ctx);

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
                    if (hdnIsAutoConfirm.Value == "1")
                    {
                        OnConfirm(true, ref  errMessage, ctx);
                    }
                    else {
                        ctx.CommitTransaction();
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
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            if (hdnEntryID.Value != "")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PatientTransferDao entityDao = new PatientTransferDao(ctx);
                BedDao entityBedDao = new BedDao(ctx);
                ConsultVisitDao entityConsultVisitDao = new ConsultVisitDao(ctx);
                ParamedicTeamDao entityParamedicTeamDao = new ParamedicTeamDao(ctx);
                try
                {
                    PatientTransfer entity = entityDao.Get(Convert.ToInt32(hdnEntryID.Value));
                    if (entity.GCPatientTransferStatus == Constant.PatientTransferStatus.OPEN)
                    {
                        List<Bed> lstBed = BusinessLayer.GetBedList(string.Format("BedID IN ({0},{1},{2})", entity.FromBedID, entity.ToBedID, hdnToBedID.Value), ctx);
                        if (entity.ToBedID != Convert.ToInt32(hdnToBedID.Value))
                        {
                            Bed oldToBed = lstBed.FirstOrDefault(p => p.BedID == entity.ToBedID);
                            oldToBed.RegistrationID = null;
                            oldToBed.GCBedStatus = Constant.BedStatus.UNOCCUPIED;
                            oldToBed.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityBedDao.Update(oldToBed);

                            ConsultVisit entityConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value), ctx).FirstOrDefault();
                            entityConsultVisit.ParamedicID = entity.ToParamedicID;
                            entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityConsultVisitDao.Update(entityConsultVisit);
                        }
                        else
                        {
                            ConsultVisit entityConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value), ctx).FirstOrDefault();
                            entityConsultVisit.ParamedicID = entity.ToParamedicID;
                            entityConsultVisit.ClassID = entity.ToClassID;
                            entityConsultVisit.ChargeClassID = entity.ToChargeClassID;
                            entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityConsultVisitDao.Update(entityConsultVisit);
                        }

                        ControlToEntity(entity);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);

                        Bed fromBed = lstBed.FirstOrDefault(p => p.BedID == entity.FromBedID);
                        Bed toBed = lstBed.FirstOrDefault(p => p.BedID == entity.ToBedID);

                        fromBed.GCBedStatus = Constant.BedStatus.WAIT_TO_BE_TRANSFERRED;
                        toBed.GCBedStatus = Constant.BedStatus.BOOKED;
                        toBed.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);

                        fromBed.LastUpdatedBy = AppSession.UserLogin.UserID;
                        toBed.LastUpdatedBy = AppSession.UserLogin.UserID;

                        entityBedDao.Update(fromBed);
                        entityBedDao.Update(toBed);

                        SettingParameterDt entitySetPar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_DPJP))[0];
                        ParamedicTeam entityParamedicTeam = BusinessLayer.GetParamedicTeamList(string.Format("RegistrationID = {0} AND GCParamedicRole = '{1}'", hdnRegistrationID.Value, entitySetPar.ParameterValue.ToString()))[0];
                        entityParamedicTeam.ParamedicID = entity.ToParamedicID;
                        entityParamedicTeam.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityParamedicTeamDao.Update(entityParamedicTeam);

                        ctx.CommitTransaction();
                    }
                    else 
                    {
                        ctx.RollBackTransaction();
                        errMessage = "This Data Has Been Approved / Cancel.";
                        result = false;                               
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
            return false;
        }
        #endregion
        private bool OnConfirm(bool transfer, ref string errMessage , IDbContext ctx)
        {
            List<CenterbackBedTransferDTO> lstCenterbackDTO = new List<CenterbackBedTransferDTO>();
            ConsultVisitDao entityConsultVisitDao = new ConsultVisitDao(ctx);
            BedDao entityBedDao = new BedDao(ctx);
            BedReservationDao entityDao = new BedReservationDao(ctx);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            PatientTransferDao entityPatientTransferDao = new PatientTransferDao(ctx);
            NutritionOrderHdDao entityNutritionOrderDao = new NutritionOrderHdDao(ctx);
            try
            {
                List<Registration> lstRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID IN ({0})", hdnRegistrationID.Value), ctx);
                List<Bed> lstBed = BusinessLayer.GetBedList(string.Format("RegistrationID IN ({0})", hdnRegistrationID.Value), ctx);
                List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID IN ({0})", hdnRegistrationID.Value), ctx);
                List<PatientTransfer> lstPatientTransfer = BusinessLayer.GetPatientTransferList(string.Format("RegistrationID IN ({0}) AND GCPatientTransferStatus = '{1}'", hdnRegistrationID.Value, Constant.PatientTransferStatus.OPEN), ctx);

                CenterbackBedTranferType cbProcessType = CenterbackBedTranferType.checkin;

                foreach (Registration registration in lstRegistration)
                {
                    registration.GCRegistrationStatus = Constant.VisitStatus.CHECKED_IN;
                    registration.LastUpdatedBy = AppSession.UserLogin.UserID;

                    ConsultVisit consultVisit = lstConsultVisit.FirstOrDefault(p => p.RegistrationID == registration.RegistrationID);
                    consultVisit.GCVisitStatus = Constant.VisitStatus.CHECKED_IN;
                    consultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;

                    List<Bed> lstUpdatedBed = lstBed.Where(p => p.RegistrationID == registration.RegistrationID).ToList();
                    foreach (Bed bed in lstUpdatedBed)
                    {
                        if (bed.GCBedStatus == Constant.BedStatus.BOOKED)
                        {
                            bed.GCBedStatus = Constant.BedStatus.OCCUPIED;
                            string username = AppSession.UserLogin.UserName;
                            string confirmDate = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
                            string confirmTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                            registration.TagProperty = username + ";" + confirmDate + ";" + confirmTime + "|";
                        }
                        else
                        {
                            if (bed.GCBedStatus == Constant.BedStatus.WAIT_TO_BE_TRANSFERRED)
                            {
                                bed.GCBedStatus = Constant.BedStatus.UNOCCUPIED;
                                bed.RegistrationID = null;
                            }
                        }
                        bed.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityBedDao.Update(bed);
                    }

                    PatientTransfer entityPatientTransfer = lstPatientTransfer.FirstOrDefault(p => p.RegistrationID == registration.RegistrationID);
                    if (entityPatientTransfer != null)
                    {
                        consultVisit.ParamedicID = entityPatientTransfer.ToParamedicID;
                        consultVisit.ClassID = entityPatientTransfer.ToClassID;
                        consultVisit.ChargeClassID = entityPatientTransfer.ToChargeClassID;
                        consultVisit.SpecialtyID = entityPatientTransfer.ToSpecialtyID;
                        consultVisit.RoomID = entityPatientTransfer.ToRoomID;
                        consultVisit.BedID = entityPatientTransfer.ToBedID;
                        consultVisit.HealthcareServiceUnitID = entityPatientTransfer.ToHealthcareServiceUnitID;

                        entityPatientTransfer.GCPatientTransferStatus = Constant.PatientTransferStatus.TRANSFERRED;
                        entityPatientTransfer.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPatientTransferDao.Update(entityPatientTransfer);

                        if (transfer)
                        {
                            List<NutritionOrderHd> lstNutritionOrderHd = BusinessLayer.GetNutritionOrderHdList(string.Format("HealthcareServiceUnitID = {0} AND GCTransactionStatus IN ('{1}', '{2}') AND VisitID IN (SELECT VisitID FROM ConsultVisit WHERE RegistrationID = {3})", entityPatientTransfer.FromHealthcareServiceUnitID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, entityPatientTransfer.RegistrationID), ctx);
                            if (lstNutritionOrderHd.Count > 0)
                            {
                                foreach (NutritionOrderHd order in lstNutritionOrderHd)
                                {
                                    order.HealthcareServiceUnitID = entityPatientTransfer.ToHealthcareServiceUnitID;
                                    entityNutritionOrderDao.Update(order);
                                }
                            }
                        }

                        if (entityPatientTransfer.ReservationID != null)
                        {
                            BedReservation entity = entityDao.Get(Convert.ToInt32(entityPatientTransfer.ReservationID));
                            entity.GCReservationStatus = Constant.Bed_Reservation_Status.COMPLETE;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
                            ctx.CommitTransaction();
                        }

                        if (AppSession.Is_Bridging_To_IPTV)
                        {
                            cbProcessType = CenterbackBedTranferType.move;
                        }

                        if (AppSession.IsBridgingToQueue)
                        {
                            try
                            {
                                List<vBed> lst = BusinessLayer.GetvBedList(string.Format("BedID IN ({0},{1})", entityPatientTransfer.FromBedID, entityPatientTransfer.ToBedID), ctx);
                                if (lst.Count > 0)
                                {
                                    vBed fromBed = lst.Where(list => list.BedID == entityPatientTransfer.FromBedID).FirstOrDefault();
                                    vBed toBed = lst.Where(list => list.BedID == entityPatientTransfer.ToBedID).FirstOrDefault();
                                    VisitInfo visitInfo = new VisitInfo();
                                    visitInfo = ConvertVisitToDTO(consultVisit.VisitID, ctx);

                                    PatientTranferInfo transferInfo = new PatientTranferInfo();
                                    transferInfo.fromRoomID = fromBed.RoomID;
                                    transferInfo.fromRoomCode = fromBed.RoomCode;
                                    transferInfo.fromRoomName = fromBed.RoomName;
                                    transferInfo.fromBedID = fromBed.BedID;
                                    transferInfo.fromBedCode = fromBed.BedCode;
                                    transferInfo.fromExtensionNo = fromBed.ExtensionNo;

                                    transferInfo.toRoomID = toBed.RoomID;
                                    transferInfo.toRoomCode = toBed.RoomCode;
                                    transferInfo.toRoomName = toBed.RoomName;
                                    transferInfo.toBedID = toBed.BedID;
                                    transferInfo.toBedCode = toBed.BedCode;
                                    transferInfo.toExtensionNo = toBed.ExtensionNo;

                                    QueueService oService = new QueueService();
                                    APIMessageLog entityAPILog = new APIMessageLog()
                                    {
                                        MessageDateTime = DateTime.Now,
                                        Recipient = Constant.BridgingVendor.QUEUE,
                                        Sender = Constant.BridgingVendor.HIS,
                                        IsSuccess = true
                                    };
                                    string apiResult = oService.SendPatientTransferInformation(registration.RegistrationID, registration.RegistrationNo, (int)registration.MRN, visitInfo, transferInfo);
                                    string[] apiResultInfo = apiResult.Split('|');
                                    if (apiResultInfo[0] == "0")
                                    {
                                        entityAPILog.IsSuccess = false;
                                        entityAPILog.Response = apiResultInfo[1];
                                        Exception ex = new Exception(apiResultInfo[1]);
                                        Helper.InsertErrorLog(ex);
                                    }
                                    else
                                        entityAPILog.MessageText = apiResultInfo[1];

                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                }
                            }
                            catch (Exception ex)
                            {
                                errMessage = ex.Message;
                                Helper.InsertErrorLog(ex);
                            }
                        }
                    }
                    else
                    {
                        consultVisit.StartServiceDate = DateTime.Now;
                        consultVisit.StartServiceTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        Helper.InsertAutoBillItem(ctx, consultVisit, Constant.Facility.INPATIENT, (int)consultVisit.ChargeClassID, registration.GCCustomerType, registration.IsPrintingPatientCard, 0);
                    }
                    entityRegistrationDao.Update(registration);
                    entityConsultVisitDao.Update(consultVisit);

                    if (AppSession.Is_Bridging_To_IPTV)
                    {
                        string processType = string.Empty;
                        switch (cbProcessType)
                        {
                            case CenterbackBedTranferType.checkin:
                                processType = "checkin";
                                break;
                            case CenterbackBedTranferType.move:
                                processType = "move";
                                break;
                        }
                        CenterbackBedTransferDTO cbObj = new CenterbackBedTransferDTO()
                        {
                            HealthcareID = AppSession.UserLogin.HealthcareID,
                            ProcessType = processType,
                            RegistrationID = registration.RegistrationID
                        };
                        lstCenterbackDTO.Add(cbObj);
                    }
                }

                ctx.CommitTransaction();

                if (AppSession.Is_Bridging_To_IPTV)
                {
                    GatewayService oService = new GatewayService();
                    APIMessageLog entityAPILog = new APIMessageLog()
                    {
                        MessageDateTime = DateTime.Now,
                        Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                        Sender = Constant.BridgingVendor.HIS,
                        IsSuccess = true
                    };
                    string apiResult = oService.IPTV_BedTransfer(lstCenterbackDTO);
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.Response = apiResultInfo[1];
                        entityAPILog.MessageText = apiResultInfo[2];
                        Exception ex = new Exception(apiResultInfo[1]);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                        entityAPILog.MessageText = apiResultInfo[2];

                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                }

                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                return false;
            }
            finally
            {
                ctx.Close();
            }
        }

        #region Bridging to Queue - Methods
        private VisitInfo ConvertVisitToDTO(int visitID, IDbContext ctx)
        {
            vConsultVisit oConsultVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", visitID), ctx).FirstOrDefault();
            VisitInfo visitInfo = new VisitInfo();
            visitInfo.VisitID = oConsultVisit.VisitID;
            visitInfo.VisitDate = oConsultVisit.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            visitInfo.VisitTime = oConsultVisit.VisitTime;
            visitInfo.DepartmentCode = Constant.Facility.INPATIENT;
            visitInfo.ServiceUnitCode = oConsultVisit.ServiceUnitCode;
            visitInfo.ServiceUnitName = oConsultVisit.ServiceUnitName;
            visitInfo.RoomID = oConsultVisit.RoomID;
            visitInfo.RoomCode = oConsultVisit.RoomCode;
            visitInfo.RoomName = oConsultVisit.RoomName;
            visitInfo.PhysicianID = oConsultVisit.ParamedicID;
            visitInfo.PhysicianCode = oConsultVisit.ParamedicCode;
            visitInfo.PhysicianName = oConsultVisit.ParamedicName;
            visitInfo.SpecialtyName = oConsultVisit.SpecialtyName;
            visitInfo.BedCode = oConsultVisit.BedCode;
            visitInfo.DischargeDate = "";
            visitInfo.DischargeTime = "";
            return visitInfo;
        }
        #endregion

    }

}