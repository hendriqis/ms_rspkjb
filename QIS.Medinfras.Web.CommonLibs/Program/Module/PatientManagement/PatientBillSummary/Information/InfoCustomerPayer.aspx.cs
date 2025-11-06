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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfoCustomerPayer : BasePageTrx
    {
        String filterExpression = "";
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.INFORMATION_CUSTOMER_PAYER_LABORATORY;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.INFORMATION_CUSTOMER_PAYER_IMAGING;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.INFORMATION_CUSTOMER_PAYER;
                    return Constant.MenuCode.MedicalDiagnostic.INFORMATION_CUSTOMER_PAYER_MEDICALDIAGNOSTIC;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.INFORMATION_CUSTOMER_PAYER_MEDICALCHECKUP;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.INFORMATION_CUSTOMER_PAYER_INPATIENT;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.INFORMATION_CUSTOMER_PAYER_EMERGENCY;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.INFORMATION_CUSTOMER_PAYER_EMERGENCY;
                default: return Constant.MenuCode.Outpatient.INFORMATION_CUSTOMER_PAYER_OUTPATIENT;
            }
        }

        //protected string GetPageTitle()
        //{
        //    return hdnPageTitle.Value;
        //}

        protected override void InitializeDataControl()
        {
            //hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID))[0];
            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            hdnVisitID.Value = entityVisit.VisitID.ToString();

            //vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
            EntityToControl(entityVisit);

            //BindGridDetail();
        }

        //private void BindGridDetail()
        //{
        //    filterExpression = (string.Format("VisitID = {0}", hdnVisitID.Value));
        //    List<vPatientDiagnosis1> lst = BusinessLayer.GetvPatientDiagnosis1List(filterExpression);
        //    lvwView.DataSource = lst;
        //    lvwView.DataBind();
        //}

        //protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        //{
        //    BindGridDetail();
        //}

        private void EntityToControl(vConsultVisit entityVisit)
        {
            hdnDepartmentID.Value = entityVisit.DepartmentID;
            hdnHealthcareServiceUnitID.Value = entityVisit.HealthcareServiceUnitID.ToString();
            hdnPayerID.Value = entityVisit.BusinessPartnerID.ToString();

            if (hdnPayerID.Value != "1")
            {
                CustomerContract entityCC = BusinessLayer.GetCustomerContractList(string.Format("BusinessPartnerID = {0}", hdnPayerID.Value)).FirstOrDefault();
                hdnContractID.Value = entityCC.ContractID.ToString();
            }

        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

    }
}