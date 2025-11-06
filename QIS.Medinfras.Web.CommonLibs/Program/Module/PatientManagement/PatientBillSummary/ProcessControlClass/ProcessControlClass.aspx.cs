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
    public partial class ProcessControlClass : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_PROCESS_CONTROL_CLASS;
                default: return Constant.MenuCode.Inpatient.BILL_SUMMARY_PROCESS_CONTROL_CLASS;
            }
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            vConsultVisit2 entityVisit = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();

            //vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];

            hdnLinkedRegistrationID.Value = entityVisit.LinkedRegistrationID.ToString();
            hdnDepartmentID.Value = entityVisit.DepartmentID;
                
            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);

            String filterExpression = GetFilterExpression() + "AND IsDeleted = 0";
            List<vPatientChargesClassCoverageDt> lst = BusinessLayer.GetvPatientChargesClassCoverageDtList(filterExpression);
            BindGrid(lst);
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filterExpression = string.Format("(RegistrationID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnRegistrationID.Value, hdnLinkedRegistrationID.Value);
            else
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);

            filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}')", Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            return filterExpression;
        }

        private void BindGrid(List<vPatientChargesClassCoverageDt> lst)
        {
            List<vPatientChargesClassCoverageDt> lstService = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE).ToList();
            ctlService.HideCheckBox();
            ctlService.BindGrid(lstService);

            List<vPatientChargesClassCoverageDt> lstDrugMS = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            ctlService.HideCheckBox();
            ctlDrugMS.BindGrid(lstDrugMS);

            List<vPatientChargesClassCoverageDt> lstLogistic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            ctlService.HideCheckBox();
            ctlLogistic.BindGrid(lstLogistic);

            List<vPatientChargesClassCoverageDt> lstLaboratory = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LABORATORY).ToList();
            ctlService.HideCheckBox();
            ctlLaboratory.BindGrid(lstLaboratory);

            List<vPatientChargesClassCoverageDt> lstImaging = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY).ToList();
            ctlService.HideCheckBox();
            ctlImaging.BindGrid(lstImaging);

            List<vPatientChargesClassCoverageDt> lstMedicalDiagnostic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            ctlMedicalDiagnostic.BindGrid(lstMedicalDiagnostic);

            txtTotalPayer.Text = lst.Sum(p => p.PayerAmount).ToString("N");
            txtTotalPatient.Text = lst.Sum(p => p.PatientAmount).ToString("N");
            txtTotal.Text = lst.Sum(p => p.LineAmount).ToString("N");

            divWarningPendingRecalculated.Attributes.Remove("style");
            string filterExpression = GetFilterExpression() + " AND IsPendingRecalculated = 1";
            int countPendingRecalculated = BusinessLayer.GetvPatientChargesHdRowCount(filterExpression);
            if (countPendingRecalculated < 1)
                divWarningPendingRecalculated.Attributes.Add("style", "display:none");
        }

        protected void cbpProcessControlClass_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
            String filterExpression = GetFilterExpression() + "AND IsDeleted = 0";
            List<vPatientChargesClassCoverageDt> lst = BusinessLayer.GetvPatientChargesClassCoverageDtList(filterExpression);
            BindGrid(lst);
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}