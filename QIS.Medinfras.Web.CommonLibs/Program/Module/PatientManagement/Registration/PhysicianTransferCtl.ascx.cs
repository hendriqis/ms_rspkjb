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
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PhysicianTransferCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param)).FirstOrDefault();
            txtRegistrationID.Text = entity.RegistrationNo;
            hdnMRNCtl.Value = entity.MRN.ToString();
            txtMRN.Text = string.Format("{0} - {1}", entity.MedicalNo, entity.PatientName);
            txtServiceUnit.Text = entity.ServiceUnitName;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            hdnRegistrationID.Value = param;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            hdnDepartmentID.Value = entity.DepartmentID;
            IsAdd = false;

            string filterExpression = string.Format("ParentID IN ('{0}')", Constant.StandardCode.PATIENT_TRANSFER_REASON);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboTransferReason, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.PATIENT_TRANSFER_REASON).ToList(), "StandardCodeName", "StandardCodeID");

            List<vParamedicTeam> lstParamedic = BusinessLayer.GetvParamedicTeamList(string.Format("RegistrationID = {0} AND ParamedicID != {1} AND GCParamedicRole = '{2}' AND IsDeleted = 0", hdnRegistrationID.Value, hdnPhysicianID.Value, Constant.ParamedicRole.DPJP_KONSUL));
            Methods.SetComboBoxField<vParamedicTeam>(cboPhysician2, lstParamedic, "ParamedicName", "ParamedicID", DevExpress.Web.ASPxEditors.DropDownStyle.DropDownList);
        }

        protected string OnGetParamedicFilterExpression()
        {
            return string.Format("ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND IsDeleted = 0", hdnHealthcareServiceUnitID.Value);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboPhysician2, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboTransferReason, new ControlEntrySetting(true, true, true));
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = true;
            try
            {
                int visitID = AppSession.RegisteredPatient.VisitID;
                int fromParamedicID = Convert.ToInt32(hdnPhysicianID.Value.ToString());
                int toParamedicID = Convert.ToInt32(cboPhysician2.Value.ToString());
                bool isChangeMedicalRecord = false; //chkMedicalRecord.Checked;
                bool isChangeTransaction = false;
                string gcTransferReason = cboTransferReason.Value.ToString();
                string otherReason = string.IsNullOrEmpty(txtOtherReason.Text) ? string.Empty : txtOtherReason.Text;

                BusinessLayer.TransferPatientToPhysician(visitID, fromParamedicID, toParamedicID, isChangeMedicalRecord, isChangeTransaction, gcTransferReason, otherReason, AppSession.UserLogin.UserID, ctx);
                result = true;
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = "Untuk Alih DPJP, Spesialisasi Dokter Pengganti perlu ditambahkan dulu di Pengaturan Data Dokter";
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected string GetGenderFemale()
        {
            return Constant.Gender.FEMALE;
        }
    }
}