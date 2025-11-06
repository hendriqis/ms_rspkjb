using System;
using System.Collections.Generic;
using System.Linq;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PhysicianTransferEntry : BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PHYSICIAN_TRANSFER;
        }

        #region List
        protected override void InitializeDataControl()
        {
            hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();
            hdnFromParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
        }

        #endregion

        #region Entry
        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}')", Constant.StandardCode.PATIENT_TRANSFER_REASON);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboTransferReason, lstStandardCode.Where(lst=>lst.ParentID == Constant.StandardCode.PATIENT_TRANSFER_REASON).ToList(), "StandardCodeName", "StandardCodeID");

            List<vParamedicTeam> lstParamedic = BusinessLayer.GetvParamedicTeamList(string.Format("RegistrationID = {0} AND ParamedicID != {1} AND GCParamedicRole = '{2}' AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID, AppSession.UserLogin.ParamedicID, Constant.ParamedicRole.DPJP_KONSUL));
            Methods.SetComboBoxField<vParamedicTeam>(cboPhysician2, lstParamedic, "ParamedicName", "ParamedicID", DevExpress.Web.ASPxEditors.DropDownStyle.DropDownList);
        }
        #endregion

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;

            if (cboPhysician2.Value == null)
            {
                errMessage = "Dokter Tujuan harus dipilih";
                return false;
            }
            else
            {
                if (AppSession.RegisteredPatient.ParamedicID != AppSession.UserLogin.ParamedicID)
                {
                    errMessage = "Maaf, proses ini hanya bisa dilakukan oleh DPJP Utama";
                    return false;
                }
            }

            IDbContext ctx = DbFactory.Configure(true);
            PatientReferralDao referralDao = new PatientReferralDao(ctx);

            try
            {
                int visitID = AppSession.RegisteredPatient.VisitID;
                int fromParamedicID = AppSession.RegisteredPatient.ParamedicID;
                int toParamedicID = Convert.ToInt32(cboPhysician2.Value.ToString());
                bool isChangeMedicalRecord = false; //chkMedicalRecord.Checked;
                bool isChangeTransaction = false;
                string gcTransferReason = cboTransferReason.Value.ToString();
                string otherReason = string.IsNullOrEmpty(txtOtherReason.Text) ? string.Empty : txtOtherReason.Text;

                BusinessLayer.TransferPatientToPhysician(visitID, fromParamedicID, toParamedicID,isChangeMedicalRecord,isChangeTransaction,gcTransferReason,otherReason, AppSession.UserLogin.UserID, ctx);

                result = true;
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = "Untuk Alih DPJP, Spesialisasi Dokter Pengganti perlu ditambahkan dulu di Pengaturan Data Dokter";
                result = false;
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }
    }
}