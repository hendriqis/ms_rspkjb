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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class EditCoverageAmount : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_EDIT_COVERAGE_AMOUNT;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_EDIT_COVERAGE_AMOUNT;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_EDIT_COVERAGE_AMOUNT;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_SUMMARY_EDIT_COVERAGE_AMOUNT;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.BILL_SUMMARY_EDIT_COVERAGE_AMOUNT;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_SUMMARY_EDIT_COVERAGE_AMOUNT;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.BILL_SUMMARY_EDIT_COVERAGE_AMOUNT;
                    return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_EDIT_COVERAGE_AMOUNT;
                default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_EDIT_COVERAGE_AMOUNT;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            
            vRegistration4 entity = BusinessLayer.GetvRegistration4List(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID))[0];
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
            
            BindGridDetail();
        }

        private void BindGridDetail()
        {
            string filter = string.Format("((RegistrationID = {0} OR (LinkedToRegistrationID = {0} AND IsChargesTransfered = 1)) AND GCTransactionStatus IN ('{1}','{2}') AND GCTransactionDetailStatus IN ('{1}','{2}') AND IsDeleted = 0)",
                                                hdnRegistrationID.Value, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);

            filter += " ORDER BY TransactionDate, TransactionID, ID";

            List<vPatientChargesDt10> lst = BusinessLayer.GetvPatientChargesDt10List(filter);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem || e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPatientChargesDt10 entity = e.Item.DataItem as vPatientChargesDt10;

                CheckBox chkIsSelected = e.Item.FindControl("chkIsSelected") as CheckBox;
                TextBox txtPayerAmount = e.Item.FindControl("txtPayerAmount") as TextBox;
                TextBox txtPatientAmount = e.Item.FindControl("txtPatientAmount") as TextBox;

                chkIsSelected.Checked = false;
                txtPayerAmount.Text = entity.PayerAmount.ToString();
                txtPatientAmount.Text = entity.PatientAmount.ToString();
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');

            BindGridDetail();
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "editcoverage")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);

                try
                {
                    if (hdnSelectedTransactionDtID.Value != "")
                    {
                        List<String> lstChargesDtID = hdnSelectedTransactionDtID.Value.Split(',').ToList();
                        List<String> lstPatientAmount = hdnSelectedPatientAmount.Value.Split(',').ToList();
                        List<String> lstPayerAmount = hdnSelectedPayerAmount.Value.Split(',').ToList();

                        lstChargesDtID.RemoveAt(0);
                        lstPatientAmount.RemoveAt(0);
                        lstPayerAmount.RemoveAt(0);

                        for (int i = 0; i < lstChargesDtID.Count(); i++)
                        {
                            PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(lstChargesDtID[i]));
                            entityDt.PayerAmount = Convert.ToDecimal(lstPayerAmount[i]);
                            entityDt.PatientAmount = Convert.ToDecimal(lstPatientAmount[i]);
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
                        }

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = GetErrorMsgSelectTransactionFirst();
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
            return true;
        }
    }
}