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
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.EMR.MasterPage;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransactionPageVisitOrderTemp : BasePageTrx
    {
        private List<HealthcareServiceUnit> lstHealthcareServiceUnit;

        public override string OnGetMenuCode()
        {
           return Constant.MenuCode.Outpatient.PATIENT_TRANSACTION_VISIT_ORDER; 
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            //IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.RegistrationStatus.CLOSED);
        }

        protected override void InitializeDataControl()
        {
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = (SELECT RegistrationID FROM vConsultVisit WHERE VisitID = '{0}')", AppSession.RegisteredPatient.VisitID))[0];
            hdnClassID.Value = entity.ClassID.ToString();
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            lstHealthcareServiceUnit = BusinessLayer.GetHealthcareServiceUnitList("");
            
        }

        protected override void OnControlEntrySetting()
        {
            List<Specialty> lstEntity = BusinessLayer.GetSpecialtyList("IsDeleted = 0");
            Methods.SetComboBoxField(cboSpecialty,lstEntity,"SpecialtyName","SpecialtyID");
        }

        private void ControlToEntity(ConsultVisit entity)
        {
            entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            entity.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
            entity.SpecialtyID = cboSpecialty.Value.ToString();
            entity.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);
            entity.GCVisitReason = Constant.VisitReason.OTHER;
            entity.VisitReason = txtVisitReason.Text;
            entity.ClassID = entity.ChargeClassID = Convert.ToInt32(hdnClassID.Value);
            entity.RoomID = null;
            entity.BedID = null;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            ConsultVisitDao entityVisitDao = new ConsultVisitDao(ctx);
            SettingParameterDao entitySettingParameterDao = new SettingParameterDao(ctx);
            try
            {
                Registration reg = entityRegistrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                        
                ConsultVisit entityVisit = new ConsultVisit();
                ControlToEntity(entityVisit);
                entityVisit.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                entityVisit.VisitDate = entityVisit.ActualVisitDate = DateTime.Today;
                entityVisit.VisitTime = entityVisit.ActualVisitTime = DateTime.Now.ToString("HH:mm");

                string registrationStatus = "";
                if (entitySettingParameterDao.Get(Constant.SettingParameter.IS_OUTPATIENT_REGISTRATION_AUTOMATICALLY_CHECKED_IN).ParameterValue == "1")
                    registrationStatus = Constant.VisitStatus.CHECKED_IN;
                else
                    registrationStatus = Constant.VisitStatus.OPEN;

                entityVisit.ClassID = entityVisit.ChargeClassID = Convert.ToInt32(hdnClassID.Value);
                entityVisit.GCVisitStatus = registrationStatus;
                entityVisit.CreatedBy = AppSession.UserLogin.UserID;
                entityVisit.IsMainVisit = false;
                entityVisit.QueueNo = BusinessLayer.GetConsultVisitMaxQueueNo(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND VisitDate = '{2}'", entityVisit.HealthcareServiceUnitID, entityVisit.ParamedicID, entityVisit.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112)), ctx);
                entityVisit.QueueNo++;
                entityVisitDao.Insert(entityVisit);

                if (registrationStatus == Constant.VisitStatus.CHECKED_IN)
                {
                    entityVisit.VisitID = BusinessLayer.GetConsultVisitMaxID(ctx);
                    Helper.InsertAutoBillItem(ctx, entityVisit, Constant.Facility.OUTPATIENT, Convert.ToInt32(hdnClassID.Value), reg.GCCustomerType, false, 0);
                }

                ctx.CommitTransaction();
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                ConsultVisit entity = BusinessLayer.GetConsultVisit(Convert.ToInt32(hdnVisitID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateConsultVisit(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}