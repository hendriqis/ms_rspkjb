using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class CloseBilling : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_CLOSE_BILLING;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_CLOSE_BILLING;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_CLOSE_BILLING;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_SUMMARY_CLOSE_BILLING;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.BILL_SUMMARY_CLOSE_BILLING;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_SUMMARY_CLOSE_BILLING;
                    return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_CLOSE_BILLING;
                default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_CLOSE_BILLING;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            vRegistration8 entity = BusinessLayer.GetvRegistration8List(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            
            txtLockStatus.Text = entity.IsLockDownInText;
            txtLockBy.Text = entity.LockDownByName;
            txtLockDate.Text = entity.cfLockDownDateInString;
            txtBillingStatus.Text = entity.IsBillingClosedInText;
            txtBillingClosedBy.Text = entity.BillingClosedByName;
            txtBillingClosedDate.Text = entity.cfBillingClosedDateInString;

            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnRegistrationNo.Value = entity.RegistrationNo;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            hdnDepartmentID.Value = entity.DepartmentID;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao registrationDao = new RegistrationDao(ctx);

            try
            {
                Registration entityReg = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                if (!entityReg.IsBillingClosed)
                {
                    entityReg.IsBillingClosed = true;
                    entityReg.BillingClosedBy = AppSession.UserLogin.UserID;
                    entityReg.BillingClosedDate = DateTime.Now;
                    entityReg.LastUpdatedBy = AppSession.UserLogin.UserID;
                    registrationDao.Update(entityReg);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, registrasi " + hdnRegistrationNo.Value + " ini proses tagihannya sudah TUTUP.";
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