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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class RegistrationEditChargeClassCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param)).FirstOrDefault();
            txtRegistrationID.Text = entity.RegistrationNo;
            txtMRN.Text = string.Format("{0} - {1}", entity.MedicalNo, entity.PatientName);
            txtServiceUnit.Text = entity.ServiceUnitName;
            txtParamedicName.Text = entity.ParamedicName;
            txtSpecialty.Text = entity.SpecialtyName;
            hdnRegistrationID.Value = param;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();

            IsAdd = false;

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsUsedInChargeClass = 1 AND IsDeleted = 0");
            Methods.SetComboBoxField<ClassCare>(cboRegistrationEditChargeClass, lstClassCare, "ClassName", "ClassID");
            cboRegistrationEditChargeClass.Value = entity.ChargeClassID.ToString();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboRegistrationEditChargeClass, new ControlEntrySetting(true, true, true));
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true; ;
            ConsultVisit entity = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", hdnRegistrationID.Value)).FirstOrDefault();

            IDbContext ctx = DbFactory.Configure(true);
            ConsultVisitDao entityDao = new ConsultVisitDao(ctx);
            try
            {
                entity.ChargeClassID = Convert.ToInt32(cboRegistrationEditChargeClass.Value);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                //BusinessLayer.UpdateConsultVisit(entity);
                entityDao.Update(entity);
                ctx.CommitTransaction();
                retval = hdnRegistrationNo.Value;
                result = true;
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
    }
}