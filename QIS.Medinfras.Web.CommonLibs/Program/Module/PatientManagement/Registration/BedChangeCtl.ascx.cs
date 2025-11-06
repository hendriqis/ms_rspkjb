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
    public partial class BedChangeCtl : BaseEntryPopupCtl
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
            hdnParamedicID.Value = entity.ParamedicID.ToString();
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtMedicalNo.Text = entity.MedicalNo;
            txtPatientName.Text = entity.PatientName;

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

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtToServiceUnitCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtToRoomCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtToBedCode, new ControlEntrySetting(true, false, true));
        }

        private void EntityToControl(vRegistration entity)
        {
            hdnFromClassID.Value = entity.ClassID.ToString();
            txtFromClassCode.Text = entity.ClassCode;
            txtFromClassName.Text = entity.ClassName;

            hdnFromServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            txtFromServiceUnitCode.Text = entity.ServiceUnitCode;
            txtFromServiceUnitName.Text = entity.ServiceUnitName;

            hdnFromRoomID.Value = entity.RoomID.ToString();
            txtFromRoomCode.Text = entity.RoomCode;
            txtFromRoomName.Text = entity.RoomName;

            hdnFromBedID.Value = entity.BedID.ToString();
            txtFromBedCode.Text = entity.BedCode;

            hdnFromChargeClassID.Value = entity.ChargeClassID.ToString();
            txtFromChargeClassCode.Text = entity.ChargeClassCode;
            txtFromChargeClassName.Text = entity.ChargeClassName;
            
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
                Bed entityToBed = BusinessLayer.GetBed(Convert.ToInt32(hdnToBedID.Value));

                if (entityToBed.GCBedStatus == Constant.BedStatus.UNOCCUPIED)
                {
                    if (hdnFromBedID.Value != hdnToBedID.Value)
                    {
                        List<Bed> lstBed = BusinessLayer.GetBedList(string.Format("BedID IN ({0},{1})", hdnFromBedID.Value, hdnToBedID.Value), ctx);
                        Bed fromBed = lstBed.FirstOrDefault(p => p.BedID == Convert.ToInt32(hdnFromBedID.Value));
                        Bed toBed = lstBed.FirstOrDefault(p => p.BedID == Convert.ToInt32(hdnToBedID.Value));

                        fromBed.GCBedStatus = Constant.BedStatus.UNOCCUPIED;
                        fromBed.RegistrationID = null;
                        fromBed.LastUpdatedBy = AppSession.UserLogin.UserID;

                        toBed.GCBedStatus = Constant.BedStatus.BOOKED;
                        toBed.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                        toBed.LastUpdatedBy = AppSession.UserLogin.UserID;

                        entityBedDao.Update(fromBed);
                        entityBedDao.Update(toBed);
                    }

                    entityConsultVisit.HealthcareServiceUnitID = Convert.ToInt32(hdnToServiceUnitID.Value);
                    entityConsultVisit.RoomID = Convert.ToInt32(hdnToRoomID.Value);
                    entityConsultVisit.BedID = Convert.ToInt32(hdnToBedID.Value);
                    entityConsultVisit.ClassID = Convert.ToInt32(hdnToClassID.Value);
                    entityConsultVisit.ChargeClassID = Convert.ToInt32(hdnToChargeClassID.Value);
                    entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityConsultVisitDao.Update(entityConsultVisit);

                    retval = hdnRegistrationNo.Value;

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Tempat tidur dengan kode " + entityToBed.BedCode + " tidak bisa digunakan.";
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