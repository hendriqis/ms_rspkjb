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
    }
}