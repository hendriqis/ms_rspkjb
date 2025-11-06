using System;
using System.Collections.Generic;
using System.Linq;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientTransferEntry : BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PATIENT_TRANSFER;
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
            Helper.SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, true), "mpProcessPatient");
        }
        #endregion

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int visitID = AppSession.RegisteredPatient.VisitID;
                int fromParamedicID = AppSession.RegisteredPatient.ParamedicID;
                int toParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                bool isChangeMedicalRecord = false; //chkMedicalRecord.Checked;
                bool isChangeTransaction = chkPatientTransaction.Checked;
                string gcTransferReason = cboTransferReason.Value.ToString();
                string otherReason = string.IsNullOrEmpty(txtOtherReason.Text) ? string.Empty : txtOtherReason.Text;

                BusinessLayer.TransferPatientToPhysician(visitID, fromParamedicID, toParamedicID,isChangeMedicalRecord,isChangeTransaction,gcTransferReason,otherReason, AppSession.UserLogin.UserID, ctx);
                result = true;
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = "Untuk Proses Transfer, Spesialisasi Dokter Pengganti perlu ditambahkan dulu di Pengaturan Data Dokter / Pasien harus memiliki transaksi"; ;
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