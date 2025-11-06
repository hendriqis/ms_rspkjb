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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ViewBill : BasePageTrx
    {
        private string pageTitle = string.Empty;

        protected string GetPageTitle()
        {
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
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_SUMMARY_DETAIL;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.BILL_SUMMARY_DETAIL;
                    return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_DETAIL;
                default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_DETAIL; ;
            }
        }

        protected override void InitializeDataControl()
        {
            int hsuID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM));
            string serviceUntIDLab = lstSetParDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            string serviceUntIDRadiologi = lstSetParDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;

            if (hsuID == Convert.ToInt32(serviceUntIDRadiologi))
            {
                AppSession.MedicalDiagnostic.MedicalDiagnosticType = MedicalDiagnosticType.Imaging;
            }
            else if (hsuID == Convert.ToInt32(serviceUntIDLab))
            {
                AppSession.MedicalDiagnostic.MedicalDiagnosticType = MedicalDiagnosticType.Laboratory;
            }

            pageTitle = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnLinkedRegistrationID.Value = AppSession.RegisteredPatient.LinkedRegistrationID.ToString();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;

            string filterExpression = string.Empty;

            //if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
            //    filterExpression = string.Format("(RegistrationID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnRegistrationID.Value, hdnLinkedRegistrationID.Value);
            //else
            //    filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);

            filterExpression = string.Format("(RegistrationID = {0} OR (LinkedToRegistrationID = {0} AND IsChargesTransfered = 1))", hdnRegistrationID.Value);

            string filterHSU = string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM PatientChargesHd WHERE VisitID IN (SELECT VisitID FROM ConsultVisit WHERE RegistrationID IN (SELECT RegistrationID FROM Registration WHERE RegistrationID = {0})) AND GCTransactionStatus != '{1}' UNION ALL SELECT HealthcareServiceUnitID FROM PatientChargesHd WHERE IsChargesTransfered = 1 AND VisitID IN (SELECT VisitID FROM ConsultVisit WHERE RegistrationID IN (SELECT RegistrationID FROM Registration WHERE LinkedToRegistrationID = {0})) AND GCTransactionStatus != '{1}') AND IsDeleted = 0", hdnRegistrationID.Value, Constant.TransactionStatus.VOID);
            List<vHealthcareServiceUnit> lstHSU = BusinessLayer.GetvHealthcareServiceUnitList(filterHSU);
            lstHSU.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

            txtFilterTransactionDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            SettingParameter setPar = BusinessLayer.GetSettingParameter(Constant.SettingParameter.FILTER_PREVIOUS_TRANSACTION_DATE_INTERVAL);
            hdnIntervalFilterDate.Value = setPar.ParameterValue;
            txtFilterTransactionDateFrom.Text = DateTime.Now.AddDays(Convert.ToInt32(hdnIntervalFilterDate.Value) * -1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            //BindGrid(); 
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGrid();
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";

            //if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
            //    filterExpression = string.Format("(RegistrationID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnRegistrationID.Value, hdnLinkedRegistrationID.Value);
            //else
            //    filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);

            filterExpression = string.Format("(RegistrationID = {0} OR (LinkedToRegistrationID = {0} AND IsChargesTransfered = 1))", hdnRegistrationID.Value);

            filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}') ", Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            
            if (rblFilterDate.SelectedValue.ToLower() == "true")
            {
                if (!txtFilterTransactionDateTo.Text.Equals(string.Empty))
                {
                    filterExpression += string.Format(" AND TransactionDate >= '{0}' and TransactionDate <= '{1}' ", Helper.GetDatePickerValue(txtFilterTransactionDateFrom.Text).ToString("yyyyMMdd"), Helper.GetDatePickerValue(txtFilterTransactionDateTo.Text).ToString("yyyyMMdd"));
                }
            }

            if (Convert.ToInt32(cboServiceUnit.Value) != 0)
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
            }

            hdnFilterParameter.Value = filterExpression;
            return filterExpression;
        }

        private void BindGrid()
        {
            string filterExpression = GetFilterExpression() + " AND IsDeleted = 0 ORDER BY TransactionID DESC, ItemName1 ASC";
            List<vPatientChargesDt12> lstEntity = BusinessLayer.GetvPatientChargesDt12List(filterExpression);
            //.OrderBy(t => t.ItemName1).ThenBy(n=> n.TransactionDate).ThenBy(z => z.TransactionTime).ToList();

            #region services
            List<vPatientChargesDt12> lstService = lstEntity.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE || p.GCItemType == Constant.ItemGroupMaster.LABORATORY
                                                                                || p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY || p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            lvwService.DataSource = lstService;
            lvwService.DataBind();
            if (lstService.Count > 0)
            {
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPayer")).InnerHtml = lstService.Sum(p => p.PayerAmount).ToString("N");
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotalPatient")).InnerHtml = lstService.Sum(p => p.PatientAmount).ToString("N");
                ((HtmlTableCell)lvwService.FindControl("tdServiceTotal")).InnerHtml = lstService.Sum(p => p.LineAmount).ToString("N");
            } 
            #endregion

            #region drugms
            List<vPatientChargesDt12> lstDrugMS = lstEntity.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            lvwDrugMS.DataSource = lstDrugMS;
            lvwDrugMS.DataBind();
            if (lstDrugMS.Count > 0)
            {
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotalPayer")).InnerHtml = lstDrugMS.Sum(p => p.PayerAmount).ToString("N");
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotalPatient")).InnerHtml = lstDrugMS.Sum(p => p.PatientAmount).ToString("N");
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotal")).InnerHtml = lstDrugMS.Sum(p => p.LineAmount).ToString("N");
            }
            #endregion

            #region logistics
            List<vPatientChargesDt12> lstLogistics = lstEntity.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            lvwLogistic.DataSource = lstLogistics;
            lvwLogistic.DataBind();
            if (lstLogistics.Count > 0)
            {
                ((HtmlTableCell)lvwLogistic.FindControl("tdLogisticTotalPayer")).InnerHtml = lstLogistics.Sum(p => p.PayerAmount).ToString("N");
                ((HtmlTableCell)lvwLogistic.FindControl("tdLogisticTotalPatient")).InnerHtml = lstLogistics.Sum(p => p.PatientAmount).ToString("N");
                ((HtmlTableCell)lvwLogistic.FindControl("tdLogisticTotal")).InnerHtml = lstLogistics.Sum(p => p.LineAmount).ToString("N");
            }
            #endregion
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

                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPatientChargesDtDao.Update(entity);
                    }
                    ctx.CommitTransaction();
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

            }
            return result;
        }
    }
}