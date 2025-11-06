using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TemporaryBedChangeCtl : BaseEntryPopupCtl
    {
        protected int serviceUnitUserCount = 0;
        protected string filterExpressionOtherMedicalDiagnostic = "";

        protected string GetServiceUnitUserParameter()
        {
            return string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, hdnDepartmentID.Value);
        }

        public override void InitializeDataControl(string param)
        {
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param))[0];
            EntityToControl(entity);

            hdnRegistrationID.Value = Convert.ToString(entity.RegistrationID);
            hdnRegistrationNo.Value = entity.RegistrationNo;
            hdnDepartmentID.Value = entity.DepartmentID;
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtMedicalNo.Text = entity.MedicalNo;
            txtPatientName.Text = entity.PatientName;
            chkIsTemporaryLocation.Checked = entity.IsTemporaryLocation;
            if (entity.IsTemporaryLocation)
            {
                trClassRequest.Attributes.Remove("style");
                txtClassRequestCode.Text = entity.RequestClassCode;
                txtClassRequestName.Text = entity.RequestClassName;
            }
            else
            {
                trClassRequest.Attributes.Add("style", "display:none");
                txtClassRequestCode.Text = "";
                txtClassRequestName.Text = "";
            }

            serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", hdnDepartmentID.Value, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));

            if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                string departmentID = Page.Request.QueryString["id"];
                if (departmentID != Constant.Facility.LABORATORY && departmentID != Constant.Facility.IMAGING)
                {
                    filterExpressionOtherMedicalDiagnostic = string.Format("HealthcareServiceUnitID NOT IN ({0},{1}) AND IsLaboratoryUnit = 0", AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
                }
            }
        }

        private void EntityToControl(vRegistration entity)
        {
            hdnClassID.Value = entity.ClassID.ToString();
            txtClassCode.Text = entity.ClassCode;
            txtClassName.Text = entity.ClassName;

            hdnServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            txtServiceUnitCode.Text = entity.ServiceUnitCode;
            txtServiceUnitName.Text = entity.ServiceUnitName;

            hdnRoomID.Value = entity.RoomID.ToString();
            txtRoomCode.Text = entity.RoomCode;
            txtRoomName.Text = entity.RoomName;

            hdnBedID.Value = entity.BedID.ToString();
            txtBedCode.Text = entity.BedCode;

            hdnChargeClassID.Value = entity.ChargeClassID.ToString();
            txtChargeClassCode.Text = entity.ChargeClassCode;
            txtChargeClassName.Text = entity.ChargeClassName;

        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BedDao entityBedDao = new BedDao(ctx);
            ConsultVisitDao entityConsultVisitDao = new ConsultVisitDao(ctx);
            try
            {
                ConsultVisit entityConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value), ctx).FirstOrDefault();

                entityConsultVisit.IsTemporaryLocation = chkIsTemporaryLocation.Checked;
                if (entityConsultVisit.IsTemporaryLocation == false)
                {
                    entityConsultVisit.RequestClassID = null;
                }
                else
                {
                    entityConsultVisit.RequestClassID = Convert.ToInt32(hdnClassRequestID.Value);
                }
                entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityConsultVisitDao.Update(entityConsultVisit);

                retval = hdnRegistrationNo.Value;

                ctx.CommitTransaction();
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