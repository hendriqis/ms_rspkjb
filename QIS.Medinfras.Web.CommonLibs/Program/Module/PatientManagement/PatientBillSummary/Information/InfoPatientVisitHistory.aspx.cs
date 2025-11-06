using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class InfoPatientVisitHistory : BasePageTrx
    {
        String filterExpression = "";
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.INFORMATION_VISIT_HISTORY_LABORATORY;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.INFORMATION_VISIT_HISTORY_IMAGING;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.INFORMATION_VISIT_HISTORY;
                    return Constant.MenuCode.MedicalDiagnostic.INFORMATION_VISIT_HISTORY_MEDICALDIAGNOSTIC;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.INFORMATION_VISIT_HISTORY_MEDICALCHECKUP;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.INFORMATION_VISIT_HISTORY_INPATIENT;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.INFORMATION_VISIT_HISTORY_EMERGENCY;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.INFORMATION_VISIT_HISTORY_EMERGENCY;
                default: return Constant.MenuCode.Outpatient.INFORMATION_VISIT_HISTORY_OUTPATIENT;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.Day);
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            vConsultVisit4 entityVisit = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            hdnVisitID.Value = entityVisit.VisitID.ToString();

            EntityToControl(entityVisit);

            BindGridDetail();
        }

        private void BindGridDetail()
        {
            filterExpression = string.Format("MRN = {0} ORDER BY VisitID DESC", AppSession.RegisteredPatient.MRN);
            List<vConsultVisit> lst = BusinessLayer.GetvConsultVisitList(filterExpression);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }
        
        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vConsultVisit entity = e.Item.DataItem as vConsultVisit;

                Registration entityReg = BusinessLayer.GetRegistration(entity.RegistrationID);

                HtmlGenericControl divTotalAmount = e.Item.FindControl("divTotalAmount") as HtmlGenericControl;
                HtmlGenericControl divPaymentAmount = e.Item.FindControl("divPaymentAmount") as HtmlGenericControl;
                HtmlGenericControl divRemainingAmount = e.Item.FindControl("divRemainingAmount") as HtmlGenericControl;

                decimal paymentAmount = (entityReg.PaymentAmount);
                decimal totalAmount = (entityReg.ChargesAmount + entityReg.SourceAmount + entityReg.AdminAmount - entityReg.DiscountAmount - entityReg.TransferAmount);
                decimal remainingAmount = paymentAmount - totalAmount;

                divPaymentAmount.InnerHtml = string.Format("Payment Amount = {0}", paymentAmount.ToString(Constant.FormatString.NUMERIC_2));
                divTotalAmount.InnerHtml = string.Format("Total Amount = {0}", totalAmount.ToString(Constant.FormatString.NUMERIC_2));
                divRemainingAmount.InnerHtml = string.Format("Remaining Amount = {0}", remainingAmount.ToString(Constant.FormatString.NUMERIC_2));

                if (remainingAmount != 0)
                {
                    divRemainingAmount.Attributes.Add("style", "color:Red");
                }
                else
                {
                    divRemainingAmount.Attributes.Add("style", "color:Blue");
                }
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void EntityToControl(vConsultVisit4 entity)
        {
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

    }
}