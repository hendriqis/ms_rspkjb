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

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class BedReservationEntry : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.BED_RESERVATION;
        }

        protected string maxNextDate = "";
        protected int serviceUnitUserCount = 0;
        private bool isAdd = false;

        protected override void InitializeDataControl()
        {
            isAdd = true;
            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param[0] != "0")
                {
                    string ReservasiID = param[0];
                    hdnReservationID.Value = ReservasiID;
                    BedReservation oBedReservation = BusinessLayer.GetBedReservation(Convert.ToInt32(ReservasiID));
                    hdnReservationNo.Value = oBedReservation.ReservationNo;
                }
                else
                {
                    string RegistrationID = param[1];
                    hdnRegistrationID.Value = RegistrationID;
                    String filterExpression = string.Format("RegistrationID = {0}", RegistrationID);
                    vConsultVisit7 oVisit = BusinessLayer.GetvConsultVisit7List(filterExpression).FirstOrDefault();
                    hdnRegistrationNo.Value = oVisit.RegistrationNo;
                }
            }
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", Constant.Facility.INPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));

            txtPlanningRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            trIsHasMRN.Attributes.Add("style", "display:none");
            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

            GetSettingParameter();
        }

        protected override void SetControlProperties()
        {

        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void OnControlEntrySetting()
        {
            rblDataSource.SelectedIndex = 0;
            SetControlEntrySetting(hdnGCReservationStatus, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtReservationNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtReservationDate, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtReservationHour, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.TIME_NOW));
            SetControlEntrySetting(txtRegistrationNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPlanningRegistrationDate, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtPlanningRegistrationHour, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.TIME_NOW));
            SetControlEntrySetting(hdnIsAdd, new ControlEntrySetting(false, false, false, "0"));

            SetControlEntrySetting(txtMRN, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtPatientName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPreferredName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtGender, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDOB, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAgeInDay, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAgeInMonth, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAgeInYear, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAddress, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtClassCode, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtClassName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRoomCode, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtRoomName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtBedCode, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(chkIsParturition, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsNewBorn, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtChargeClassCode, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtChargeClassName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, false, true));
        }

        private void GetSettingParameter()
        {

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                                                                            AppSession.UserLogin.HealthcareID,
                                                                            Constant.SettingParameter.SA_TIPE_SEARCH_DIALOG_PASIEN,
                                                                            Constant.SettingParameter.RM_DEFAULT_PATIENT_WALKIN,
                                                                            Constant.SettingParameter.MAX_NEXT_DATE,
                                                                            Constant.SettingParameter.RM_REGISTRASI_SELAIN_RAJAL_MEMPERHATIKAN_CUTI_DOKTER
                                                                        ));
            maxNextDate = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MAX_NEXT_DATE).ParameterValue;
            hdnPatientSearchDialogType.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_TIPE_SEARCH_DIALOG_PASIEN).ParameterValue;
            hdnRMPatientWalkin.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.RM_DEFAULT_PATIENT_WALKIN).ParameterValue;
            hdnRegistrasiSelainRajalMemperhatikanCutiDokter.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.RM_REGISTRASI_SELAIN_RAJAL_MEMPERHATIKAN_CUTI_DOKTER).ParameterValue;
        }

        protected string GetServiceUnitUserParameter()
        {
            return string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.INPATIENT);
        }

        protected string GetGenderFemale()
        {
            return Constant.Gender.FEMALE;
        }

        protected string OnGetRegistrationNoFilterExpression()
        {
            string filterExpression = string.Format("DepartmentID = '{0}' AND GCRegistrationStatus IN ('{1}', '{2}')", Constant.Facility.INPATIENT, Constant.VisitStatus.OPEN, Constant.VisitStatus.CHECKED_IN);
            return filterExpression;
        }

        protected string OnGetPatientFilterExpression()
        {
            string filterExpression = string.Format("MRN NOT IN (SELECT r.MRN FROM Registration r WHERE r.GCRegistrationStatus = '{0}' AND r.RegistrationID IN (SELECT cv.RegistrationID FROM ConsultVisit cv INNER JOIN vHealthcareServiceUnit vsu ON cv.HealthcareServiceUnitID = vsu.HealthcareServiceUnitID AND vsu.DepartmentID = '{1}' WHERE cv.GCVisitStatus = '{0}'))", Constant.VisitStatus.CHECKED_IN, Constant.Facility.INPATIENT);
            filterExpression += string.Format(" AND MRN NOT IN (SELECT MRN FROM BedReservation WHERE GCReservationStatus = '{0}' AND IsFromRegistration = 0 AND MRN IS NOT NULL)", Constant.Bed_Reservation_Status.OPEN);
            return filterExpression;
        }

        public override void OnAddRecord()
        {
            isAdd = true;
        }

        #region Load Entity
        public override int OnGetRowCount()
        {
            string filterExpression = "";
            return BusinessLayer.GetvBedReservationRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            vBedReservation entity = BusinessLayer.GetvBedReservation("1=1", PageIndex, "ReservationID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = String.Format("ReservationID = {0}", Convert.ToInt32(hdnReservationID.Value));
            PageIndex = BusinessLayer.GetvBedReservationRowIndex(filterExpression, keyValue, "ReservationID DESC");
            vBedReservation entity = BusinessLayer.GetvBedReservation(filterExpression, PageIndex, "ReservationID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vBedReservation entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCReservationStatus != Constant.Bed_Reservation_Status.OPEN)
            {
                hdnIsEditable.Value = "0";
                isShowWatermark = true;
                watermarkText = entity.ReservationStatus;
            }

            trBedQuickPicks.Style.Add("display", "none");
            rblDataSource.Enabled = false;

            if (entity.IsFromRegistration)
            {
                rblDataSource.SelectedIndex = 0;
            }
            else
            {
                rblDataSource.SelectedIndex = 1;
            }

            hdnReservationID.Value = entity.ReservationID.ToString();
            txtReservationNo.Text = entity.ReservationNo;
            txtReservationDate.Text = entity.cfReservationDateInString;
            txtReservationHour.Text = entity.ReservationTime;
            txtPlanningRegistrationDate.Text = entity.cfPlanningRegistrationDateInString;
            txtPlanningRegistrationHour.Text = entity.PlanningRegistrationTime;
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtMRN.Text = entity.MedicalNo;
            txtPatientName.Text = entity.PatientName;
            txtPreferredName.Text = entity.PreferredName;
            txtGender.Text = entity.Gender;
            txtDOB.Text = entity.cfDateOfBirthInString;
            txtAgeInYear.Text = entity.cfAgeInYear.ToString();
            txtAgeInMonth.Text = entity.cfAgeInMonth.ToString();
            txtAgeInDay.Text = entity.cfAgeInDay.ToString();
            txtAddress.Text = entity.PatientAddress;
            txtClassCode.Text = entity.ClassCode;
            txtClassName.Text = entity.ClassName;
            txtServiceUnitCode.Text = entity.ServiceUnitCode;
            txtServiceUnitName.Text = entity.ServiceUnitName;
            txtRoomCode.Text = entity.RoomCode;
            txtRoomName.Text = entity.RoomName;
            txtBedCode.Text = entity.BedCode;
            txtChargeClassCode.Text = entity.ChargeClassCode;
            txtChargeClassName.Text = entity.ChargeClassName;
            txtRemarks.Text = entity.Remarks;
            hdnGCReservationStatus.Value = entity.GCReservationStatus;

            if (entity.ParamedicID != 0 || entity.ParamedicID != null)
            {
                hdnParamedicID.Value = entity.ParamedicID.ToString();
                txtPhysicianCode.Text = entity.ParamedicCode;
                txtPhysicianName.Text = entity.ParamedicName;
            }

            chkIsNewBorn.Checked = entity.IsNewBorn;
            chkIsPregnant.Checked = entity.IsPregnant;
            chkIsParturition.Checked = entity.IsParturition;
        }
        #endregion

        private void ControlToEntity(Guest entityGuest, Patient entityPatient, BedReservation entityBedReservation)
        {
            #region Patient Data
            entityGuest.GCSalutation = hdnGuestGCSalutation.Value;
            entityGuest.GCTitle = hdnGuestGCTitle.Value;
            entityGuest.FirstName = hdnGuestFirstName.Value;
            entityGuest.MiddleName = hdnGuestMiddleName.Value;
            entityGuest.LastName = hdnGuestLastName.Value;
            entityGuest.GCSuffix = hdnGuestGCSuffix.Value;
            entityGuest.GCGender = hdnGuestGCGender.Value;
            entityGuest.DateOfBirth = Helper.GetDatePickerValue(hdnGuestDateOfBirth.Value);
            #endregion

            #region Patient Address
            entityGuest.StreetName = hdnGuestStreetName.Value;
            entityGuest.County = hdnGuestCounty.Value; // Desa
            entityGuest.District = hdnGuestDistrict.Value; //Kabupaten
            entityGuest.City = hdnGuestCity.Value;
            #endregion

            #region Patient Contact
            entityGuest.PhoneNo = hdnGuestPhoneNo.Value;
            entityGuest.MobilePhoneNo = hdnGuestMobilePhoneNo.Value;
            entityGuest.EmailAddress = hdnGuestEmailAddress.Value;
            entityGuest.SSN = hdnGuestSSN.Value;
            entityGuest.GCIdentityNoType = hdnGuestGCIdentityNoType.Value;
            #endregion

            #region Patient Name
            entityGuest.Name = Helper.GenerateName(entityGuest.LastName, entityGuest.MiddleName, entityGuest.FirstName);
            entityGuest.FullName = Helper.GenerateFullName(entityGuest.Name, hdnGuestTitle.Value, hdnGuestSuffix.Value);
            #endregion

            #region BedReservation
            entityBedReservation.TransactionCode = Constant.TransactionCode.BED_RESERVATION;
            entityBedReservation.ReservationDate = Helper.GetDatePickerValue(txtReservationDate);
            entityBedReservation.ReservationTime = txtReservationHour.Text;
            if (hdnRoomID.Value != "")
            {
                entityBedReservation.RoomID = Convert.ToInt32(hdnRoomID.Value);
            }
            if (hdnBedID.Value != "")
            {
                entityBedReservation.BedID = Convert.ToInt32(hdnBedID.Value);
            }
            if (hdnParamedicID.Value != "")
            {
                entityBedReservation.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            }
            entityBedReservation.PlanningRegistrationDate = Helper.GetDatePickerValue(txtPlanningRegistrationDate);
            entityBedReservation.PlanningRegistrationTime = txtPlanningRegistrationHour.Text;
            entityBedReservation.ClassID = Convert.ToInt32(hdnClassID.Value);
            entityBedReservation.ChargeClassID = Convert.ToInt32(hdnChargeClassID.Value);
            entityBedReservation.GCReservationStatus = Constant.Bed_Reservation_Status.OPEN;
            entityBedReservation.CreatedBy = AppSession.UserLogin.UserID;
            entityBedReservation.Remarks = txtRemarks.Text;
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            GuestDao entityGuestDao = new GuestDao(ctx);
            BedReservationDao entityDao = new BedReservationDao(ctx);

            try
            {
                Registration entityRegistration = null;
                Patient entityPatient = new Patient();
                Guest entityGuest = new Guest();
                BedReservation bedReservation = new BedReservation();

                ControlToEntity(entityGuest, entityPatient, bedReservation);

                Int32 QueueNumber = BusinessLayer.GetBedReservationList(String.Format("BedID = '{0}' AND GCReservationStatus = '{1}'", bedReservation.BedID, Constant.Bed_Reservation_Status.OPEN)).Count;
                QueueNumber = QueueNumber + 1;
                bedReservation.ReservationNo = BusinessLayer.GenerateTransactionNo(bedReservation.TransactionCode, bedReservation.ReservationDate, ctx);

                if (hdnIsFromRegistration.Value == "1")
                {
                    entityRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
                    bedReservation.RegistrationID = entityRegistration.RegistrationID;
                    bedReservation.MRN = entityRegistration.MRN;
                    bedReservation.IsNewPatient = entityRegistration.IsNewPatient;
                    bedReservation.IsFromRegistration = true;
                    bedReservation.QueueNo = Convert.ToInt16(QueueNumber);
                }
                else if (hdnIsFromRegistration.Value == "0")
                {
                    if (String.IsNullOrEmpty(hdnMRN.Value))
                    {
                        entityGuest.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityGuestDao.Insert(entityGuest);
                        bedReservation.GuestID = BusinessLayer.GetGuestMaxID(ctx);
                        bedReservation.IsFromRegistration = false;
                        bedReservation.IsNewPatient = true;
                        bedReservation.QueueNo = Convert.ToInt16(QueueNumber);
                    }
                    else
                    {
                        bedReservation.MRN = Convert.ToInt32(hdnMRN.Value);
                        bedReservation.IsFromRegistration = false;
                        bedReservation.IsNewPatient = true;
                        bedReservation.QueueNo = Convert.ToInt16(QueueNumber);
                    }
                }
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                int newReservationID = entityDao.InsertReturnPrimaryKeyID(bedReservation);
                hdnReservationID.Value = newReservationID.ToString();
                ctx.CommitTransaction();
                retval = bedReservation.ReservationNo;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            string[] param = type.Split(';');
            bool result = true;
            if (param[0] == "void")
            {
                string gcDeleteReason = param[1];
                string reason = param[2];
                IDbContext ctx = DbFactory.Configure(true);
                BedReservationDao entityDao = new BedReservationDao(ctx);
                try
                {
                    BedReservation entity = entityDao.Get(Convert.ToInt32(hdnReservationID.Value));
                    if (entity.GCReservationStatus == Constant.Bed_Reservation_Status.OPEN)
                    {
                        entity.GCDeleteReason = gcDeleteReason;
                        if (gcDeleteReason == Constant.DeleteReason.OTHER)
                        {
                            entity.DeleteReason = reason;
                        }
                        entity.DeletedBy = AppSession.UserLogin.UserID;
                        entity.DeletedDate = DateTime.Now;
                        entity.GCReservationStatus = Constant.Bed_Reservation_Status.CANCELLED;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        errMessage = "Reservasi tidak dapat diubah. Harap refresh halaman ini.";
                        result = false;
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
            return result;
        }

        #region Getter Guest Data
        public String GuestGCSalutation { get { return Request.Form[hdnGuestGCSalutation.UniqueID]; } }
        public String GuestGCTitle { get { return Request.Form[hdnGuestGCTitle.UniqueID]; } }
        public String GuestFirstName { get { return Request.Form[hdnGuestFirstName.UniqueID]; } }
        public String GuestMiddleName { get { return Request.Form[hdnGuestMiddleName.UniqueID]; } }
        public String GuestLastName { get { return Request.Form[hdnGuestLastName.UniqueID]; } }
        public String GuestGCSuffix { get { return Request.Form[hdnGuestGCSuffix.UniqueID]; } }
        public String GuestGCGender { get { return Request.Form[hdnGuestGCGender.UniqueID]; } }
        public String GuestDateOfBirth { get { return Request.Form[hdnGuestDateOfBirth.UniqueID]; } }
        public String GuestStreetName { get { return Request.Form[hdnGuestStreetName.UniqueID]; } }
        public String GuestCounty { get { return Request.Form[hdnGuestCounty.UniqueID]; } }
        public String GuestCity { get { return Request.Form[hdnGuestCity.UniqueID]; } }
        public String GuestDistrict { get { return Request.Form[hdnGuestDistrict.UniqueID]; } }
        public String GuestPhoneNo { get { return Request.Form[hdnGuestPhoneNo.UniqueID]; } }
        public String GuestMobilePhoneNo { get { return Request.Form[hdnGuestMobilePhoneNo.UniqueID]; } }
        public String GuestEmailAddress { get { return Request.Form[hdnGuestEmailAddress.UniqueID]; } }
        public String GuestGCIdentityNoType { get { return Request.Form[hdnGuestGCIdentityNoType.UniqueID]; } }
        public String GuestSSN { get { return Request.Form[hdnGuestSSN.UniqueID]; } }
        #endregion
    }
}