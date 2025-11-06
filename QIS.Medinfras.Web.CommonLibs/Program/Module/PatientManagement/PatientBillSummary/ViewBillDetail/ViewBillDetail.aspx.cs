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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ViewBillDetail : BasePageTrx
    {
        private string pageTitle = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void InitializeDataControl()
        {
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).First();
            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            hdnLinkedRegistrationID.Value = entityVisit.LinkedRegistrationID.ToString();
            hdnDepartmentID.Value = entityVisit.DepartmentID;
            pageTitle = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            //vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value)).FirstOrDefault();
            string filterExpression = string.Empty;
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filterExpression = string.Format("(RegistrationID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnRegistrationID.Value, hdnLinkedRegistrationID.Value);
            else
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);

            List<vHealthcareServiceUnit> lstHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vPatientChargesHd WHERE {0}) AND IsDeleted = 0", filterExpression));
            lstHSU.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;
            BindGrid();

        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGrid();
        }


        private string GetFilterExpression()
        {
            string filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filterExpression = string.Format("(RegistrationID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnRegistrationID.Value, hdnLinkedRegistrationID.Value);
            else
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}') ", Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            if (rblFilterDate.SelectedValue.ToLower() == "true")
            {
                if (!txtFilterOrderDate.Text.Equals(string.Empty))
                {
                    filterExpression += string.Format(" AND datediff(day, TransactionDate, '{0}') = 0 ", Helper.GetDatePickerValue(txtFilterOrderDate.Text).ToString("yyyyMMdd"));
                }
            }
            return filterExpression;
        }

        private void BindGrid()
        {
            string filterExpression = GetFilterExpression() + "AND IsDeleted = 0";
            List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(filterExpression);
            List<vPatientChargesDt8> lstFilterByComboBox = lst;
            if (Convert.ToInt32(cboServiceUnit.Value) != 0)
            {
                lstFilterByComboBox = lstFilterByComboBox.Where(p => p.HealthcareServiceUnitID == Convert.ToInt32(cboServiceUnit.Value)).ToList();
            }
            lstFilterByComboBox.OrderBy(t => t.ItemName1).ThenBy(n=> n.TransactionDate).ThenBy(z => z.TransactionTime);

            List<vPatientChargesDt8> lstService = lstFilterByComboBox.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE || p.GCItemType == Constant.ItemGroupMaster.LABORATORY
                                                            || p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY || p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            //ctlService.HideCheckBox();
            ctlService.BindGrid(lstService);

            List<vPatientChargesDt8> lstDrugMS = lstFilterByComboBox.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            //ctlDrugMS.HideCheckBox();
            ctlDrugMS.BindGrid(lstDrugMS);

            List<vPatientChargesDt8> lstLogistic = lstFilterByComboBox.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            //ctlLogistic.HideCheckBox();
            ctlLogistic.BindGrid(lstLogistic);

            //List<vPatientChargesDt> lstLaboratory = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LABORATORY).ToList();
            //ctlLaboratory.HideCheckBox();
            //ctlLaboratory.BindGrid(lstLaboratory);

            //List<vPatientChargesDt> lstImaging = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY).ToList();
            //ctlImaging.HideCheckBox();
            //ctlImaging.BindGrid(lstImaging);

            //List<vPatientChargesDt> lstMedicalDiagnostic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            //ctlMedicalDiagnostic.HideCheckBox();
            //ctlMedicalDiagnostic.BindGrid(lstMedicalDiagnostic);
        }

        protected string GetPageTitle()
        {
            //return hdnPageTitle.Value;
            return pageTitle;
        }

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_DETAIL;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_DETAIL;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_DETAIL;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_SUMMARY_DETAIL;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.BILL_SUMMARY_DETAIL;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_SUMMARY_DETAIL;
                    return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_DETAIL;
                default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_DETAIL; ;
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtDao entityPatientChargesDtDao = new PatientChargesDtDao(ctx);


            if (type == "generatebill")
            {
                try
                {
                    string[] param = hdnLstSelectedValue.Value.Split(',');
                    for (int i = 0; i < param.Length; i++)
                    {
                        PatientChargesDt entity = entityPatientChargesDtDao.Get(Convert.ToInt32(param[i]));
                        if (hdnVerifyCancel.Value.ToLower().Equals("verify"))
                        {
                            entity.IsReviewed = true;
                            entity.ReviewedBy = AppSession.UserLogin.UserID;
                            entity.ReviewedDate = DateTime.Now;
                        }
                        else
                        {
                            entity.IsReviewed = false;
                            entity.ReviewedBy = null;
                            entity.ReviewedDate = null;
                        }

                        entityPatientChargesDtDao.Update(entity);
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                    ctx.RollBackTransaction();
                }

            }
            return result;
        }
    }
}