using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientPaymentCashList : BasePageTrx
    {
        private string refreshGridInterval = "";
        private string departmentID = "";

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.IP_PROCESS_BRIDING_MASPION;
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.PROCESS_BRIDING_MASPION;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.ER_PROCESS_BRIDING_MASPION;
                case Constant.Facility.IMAGING: return Constant.MenuCode.Imaging.IS_PROCESS_BRIDING_MASPION;
                case Constant.Facility.LABORATORY: return Constant.MenuCode.Laboratory.LB_PROCESS_BRIDING_MASPION;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.MC_PROCESS_BRIDING_MASPION;
                case Constant.Facility.DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.MD_PROCESS_BRIDING_MASPION;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.PH_PROCESS_BRIDING_MASPION; 
                default: return Constant.MenuCode.Outpatient.PROCESS_BRIDING_MASPION;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

            departmentID = Page.Request.QueryString["id"];
            hdnParam.Value = departmentID;
            
            //hdnIsBridgingToAplicares.Value = AppSession.IsBridgingToAPLICARES ? "1" : "0";
            //List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
            //                                                                "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
            //                                                                AppSession.UserLogin.HealthcareID,
            //                                                                Constant.SettingParameter.IP_ICU_SERVICE_UNIT_ID,
            //                                                                Constant.SettingParameter.IP_NICU_SERVICE_UNIT_ID,
            //                                                                Constant.SettingParameter.IP_PICU_SERVICE_UNIT_ID,
            //                                                                Constant.SettingParameter.IS_PATIENT_DISCHARGE_DEAD
            //                                                            )
            //                                                        );
            //hdnHealthcareServiceUnitICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_ICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
            //hdnHealthcareServiceUnitNICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_NICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
            //hdnHealthcareServiceUnitPICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_PICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
            //hdnIsAllowCancelDischargeForDischargeDead.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_PATIENT_DISCHARGE_DEAD).FirstOrDefault().ParameterValue;

            txtPaymentDate.Text  =  DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            string filterExpression = string.Format("UserID = '{0}'", AppSession.UserLogin.UserID);
            List<vUser> lstUserCashier = BusinessLayer.GetvUserList(filterExpression);
            Methods.SetComboBoxField<vUser>(cboCashier, lstUserCashier, "FullName", "UserID");
            cboCashier.SelectedIndex = 0;
            BindGridView(); 
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {

            string Filterexpression = string.Format("CreatedBy={0} AND GCTransactionStatus NOT IN ('{1}') AND GCPaymentType NOT IN('{2}', '{3}') AND GCBridgingStatus = '{4}' AND PaymentReceiptID > 0  AND DepartmentID='{5}'", AppSession.UserLogin.UserID,
                Constant.TransactionStatus.VOID, Constant.PaymentType.AR_PATIENT, Constant.PaymentType.AR_PAYER, Constant.MaspionProcessStatus.OPEN, hdnParam.Value);
            List<vPatientPaymentBridgingMaspion> lstData = BusinessLayer.GetvPatientPaymentBridgingMaspionList(Filterexpression);
            grdView.DataSource = lstData;
            grdView.DataBind();
        }

        protected void cbpProcessPayment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            IDbContext ctx = DbFactory.Configure(true);
            string result = "success|";
            bool flagResult = true;
            try {
               string lstPayment =  hdnSelectedPaymentID.Value;
               bool oResult = BusinessLayer.InsertPaymentIDToMaspionDbLink(lstPayment, ctx);
               if (oResult) {
                   ctx.CommitTransaction();
               }
            }
            catch (Exception ex)
            {
                flagResult = false;
                result = string.Format("fail|{0}", ex.Message);
                Helper.InsertErrorLog(ex);
                 ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

      
    }
}