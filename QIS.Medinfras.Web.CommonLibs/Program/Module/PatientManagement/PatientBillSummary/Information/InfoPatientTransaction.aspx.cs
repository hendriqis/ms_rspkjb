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
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfoPatientTransaction : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PENDING_TRANSACTION;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.PENDING_TRANSACTION;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PENDING_TRANSACTION;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.PENDING_TRANSACTION;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.PENDING_TRANSACTION;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.PENDING_TRANSACTION;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.PENDING_TRANSACTION;
                    return Constant.MenuCode.MedicalDiagnostic.PENDING_TRANSACTION;
                default: return Constant.MenuCode.Outpatient.PENDING_TRANSACTION;
            }
        }

        protected override void InitializeDataControl()
        {
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID))[0];

            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();

            string filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);

            bindGrdTestOrder(filterExpression);
            bindGrdServiceOrder(filterExpression);
            bindGrdPharmacy(filterExpression);
            bindGrdPrescriptionReturn(filterExpression);
            //bindGrdCharges(filterExpression);
            //bindGrdBilling(filterExpression);
        }

        private void bindGrdPharmacy(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}','{1}','{2}') ORDER BY PrescriptionOrderID", Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vPrescriptionOrderHd> PrescriptionOrderHd = BusinessLayer.GetvPrescriptionOrderHdList(filterExpression);
            grdViewOrderFarmasi.DataSource = PrescriptionOrderHd;
            grdViewOrderFarmasi.DataBind();
        }

        private void bindGrdPrescriptionReturn(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}','{1}','{2}') ORDER BY PrescriptionReturnOrderID", Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vPrescriptionReturnOrderHd> PrescriptionReturnOrderHd = BusinessLayer.GetvPrescriptionReturnOrderHdList(filterExpression);
            grdViewOrderReturResep.DataSource = PrescriptionReturnOrderHd;
            grdViewOrderReturResep.DataBind();
        }

        private void bindGrdTestOrder(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}','{1}') ORDER BY TestOrderID", Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.VOID);
            List<vTestOrderHd> lstTestOrderHd = BusinessLayer.GetvTestOrderHdList(filterExpression);
            grdViewOrderPenunjang.DataSource = lstTestOrderHd;
            grdViewOrderPenunjang.DataBind();
        }

        private void bindGrdServiceOrder(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}','{1}') ORDER BY ServiceOrderID", Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.VOID);
            List<vServiceOrderHd> lstServiceOrderHd = BusinessLayer.GetvServiceOrderHdList(filterExpression);
            grdServiceOrder.DataSource = lstServiceOrderHd;
            grdServiceOrder.DataBind();
        }

        private void bindGrdCharges(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}','{1}') ORDER BY TransactionID", Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vPatientChargesHd> lstPatientChargesHd = BusinessLayer.GetvPatientChargesHdList(filterExpression);
            grdViewCharges.DataSource = lstPatientChargesHd;
            grdViewCharges.DataBind();
        }

        private void bindGrdBilling(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}','{1}','{2}') ORDER BY PatientBillingID", Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<PatientBill> lstPatientBillHd = BusinessLayer.GetPatientBillList(filterExpression);
            grdViewBilling.DataSource = lstPatientBillHd;
            grdViewBilling.DataBind();
        }
    }
}