using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using System.Text;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Outpatient.Program
{
    public partial class VisitList : BasePageRegisteredPatient
    {
        string id = string.Empty;
        protected int PageCountDate = 1;

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"].Split('|')[0];
            hdnQueryString.Value = id;
            switch (id)
            {
                case "ct": return Constant.MenuCode.Outpatient.CHANGE_PATIENT_TRANSACTION_STATUS;
                case "ptp": return Constant.MenuCode.Outpatient.PATIENT_TRANSACTION_PAGE;
                //case "pe": return Constant.MenuCode.Outpatient.PATIENT_PAGE;
                default: return Constant.MenuCode.Outpatient.BILL_SUMMARY;
            }
        }

        private GetUserMenuAccess menu;
        public override bool IsShowRightPanel()
        {
            return true;
        }
        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }
        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.OUTPATIENT, "IsUsingRegistration = 1");
                id = Page.Request.QueryString["id"].Split('|')[0];
                if (id == "bs")
                {
                    StringBuilder sbLstHealthcareServiceUnitID = new StringBuilder();
                    foreach (GetServiceUnitUserList serviceUnit in lstServiceUnit)
                    {
                        if (sbLstHealthcareServiceUnitID.ToString() != "")
                            sbLstHealthcareServiceUnitID.Append(",");
                        sbLstHealthcareServiceUnitID.Append(serviceUnit.HealthcareServiceUnitID.ToString());
                    }
                    hdnLstHealthcareServiceUnitID.Value = sbLstHealthcareServiceUnitID.ToString();
                    lstServiceUnit.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                }
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboClinic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;
                cboClinic.SelectedIndex = 0;

                hdnIsUsedReopenBilling.Value = AppSession.IsUsedReopenBilling ? "1" : "0";
                trFilterReopenBilling.Visible = AppSession.IsUsedReopenBilling;

                txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtRegistrationDatePatientCall.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                SettingControlProperties();
                grdRegisteredOutpatientPatient.InitializeControl();

                Helper.SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");
                Helper.SetControlEntrySetting(txtRegistrationDatePatientCall, new ControlEntrySetting(true, true, true), "mpPatientList");
                Helper.SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true, true, true), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                txtBarcodeEntry.Focus();

                txtScheduleDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                GetSettingParameter();

                //BindGridViewDate(1, true, ref PageCountDate);

                //BindingViewPatientCall();

                if (hdnQueryString.Value == "bs")
                {
                    clinicServiceStatus.Visible = false;
                    patientCall.Visible = false;
                }
                else
                {
                    if (hdnIsAutomaticallyCheckedIn.Value != "1")
                    {
                        if (hdnIsUsingPatientCall.Value == "1")
                        {
                            patientCall.Visible = true;
                        }
                        else
                        {
                            patientCall.Visible = false;
                        }
                    }
                    else
                    {
                        patientCall.Visible = false;
                    }

                    if (hdnIsUsingClinicService.Value != "1")
                    {
                        clinicServiceStatus.Visible = false;
                    }
                    else
                    {
                        clinicServiceStatus.Visible = true;
                    }
                }
            }
        }

        private void SettingControlProperties()
        {
            if (AppSession.LastContentVisitListOP != null)
            {
                LastContentVisitListOP lc = AppSession.LastContentVisitListOP;
                txtRegistrationDate.Text = lc.RegistrationDate;
                txtRegistrationDatePatientCall.Text = lc.RegistrationDate2;
                hdnPhysicianID.Value = lc.ParamedicID.ToString();
                hdnPhysicianIDPatientCall.Value = lc.ParamedicIDPatientCall.ToString();
                if (id == "bs")
                {
                    cboServiceUnit.Value = lc.HealthcareServiceUnitID.ToString();
                    cboClinic.Value = lc.ClinicHealthcareServiceUnitID.ToString();
                }
                else
                {
                    if (lc.HealthcareServiceUnitID == 0)
                    {
                        cboServiceUnit.SelectedIndex = 0;
                        cboClinic.SelectedIndex = 0;
                    }
                    else
                    {
                        cboServiceUnit.Value = lc.HealthcareServiceUnitID.ToString();
                        cboClinic.Value = lc.ClinicHealthcareServiceUnitID.ToString();
                    }
                }
                ParamedicMaster pm = BusinessLayer.GetParamedicMaster(lc.ParamedicID);
                if (pm != null)
                {
                    txtPhysicianCode.Text = pm.ParamedicCode;
                    txtPhysicianName.Text = pm.FullName;
                }

                ParamedicMaster pm2 = BusinessLayer.GetParamedicMaster(lc.ParamedicIDPatientCall);
                if (pm2 != null)
                {
                    txtPhysicianCodePatientCall.Text = pm2.ParamedicCode;
                    txtPhysicianNamePatientCall.Text = pm2.FullName;
                }
                txtBarcodeEntry.Text = lc.MedicalNo;
                hdnQuickText.Value = lc.QuickText.Equals("Search") ? "" : lc.QuickText;
                hdnQuickTextPatientCall.Value = lc.QuickTextPatientCall.Equals("Search") ? "" : lc.QuickTextPatientCall;
                hdnFilterExpressionQuickSearchPatientCall.Value = lc.QuickFilterExpressionPatientCall;
            }
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string id = Page.Request.QueryString["id"];
            string filterExpression = string.Empty;

            if (id == "pe")
            {
                filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}')",
                        Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED);
            }
            else
            {
                if (hdnIsUsedReopenBilling.Value == "1")
                {
                    if (chkIsIncludeReopenBilling.Checked)
                    {
                        filterExpression += string.Format("GCVisitStatus IN ('{0}') AND IsBillingReopen = 1",
                                Constant.VisitStatus.CLOSED);
                    }
                    else
                    {
                        if (id == "ptp")
                        {
                            filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}')",
                                    Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED);
                        }
                        else
                        {
                            filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}')",
                                    Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);
                        }
                    }
                }
                else
                {
                    if (id == "ptp")
                    {
                        filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}')",
                                Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED);
                    }
                    else
                    {
                        filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}')",
                                Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);
                    }
                }
            }

            if (!chkIsPreviousEpisodePatient.Checked)
            {
                filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (cboServiceUnit.Value.ToString() != "0")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
            else
                filterExpression += string.Format(" AND HealthcareServiceUnitID IN ({0}) AND IsMainVisit = 1", hdnLstHealthcareServiceUnitID.Value);

            if (txtPhysicianCode.Text != "")
                filterExpression += string.Format(" AND ParamedicID = {0}", hdnPhysicianID.Value);

            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            if (hdnSearchBarcodeNoRM.Value != "" && hdnSearchBarcodeNoRM.Value != null && hdnSearchBarcodeNoRM.Value != "00-00-00-00")
            {
                filterExpression += string.Format(" AND MedicalNo = '{0}'", hdnSearchBarcodeNoRM.Value);
            }

            return filterExpression;
        }

        private void GetSettingParameter()
        {
            List<SettingParameter> lstSetPar = BusinessLayer.GetSettingParameterList(string.Format(
                                                                    "ParameterCode IN ('{0}')",
                                                                    Constant.SettingParameter.IS_OUTPATIENT_REGISTRATION_AUTOMATICALLY_CHECKED_IN //0
                                                                ));
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')",
                    AppSession.UserLogin.HealthcareID, //0
                    Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY, //1
                    Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE, //2
                    Constant.SettingParameter.OP_IS_USING_CALL_PATIENT_FEATURE, //3
                    Constant.SettingParameter.OP_IS_USING_CLINIC_SERVICE_FEATURE, //4
                    Constant.SettingParameter.FN_KONTROL_KODE_BIAYA_ADMINISTRASI_RJ, //5
                    Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI_RJ_INSTANSI, //6
                    Constant.SettingParameter.FN_KONTROL_BIAYA_ADM_PASIEN_RJ, //7
                    Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI, //8
                    Constant.SettingParameter.FN_KONTROL_BIAYA_KARTU, //9
                    Constant.SettingParameter.FN_KODE_PELAYANAN_KARTU, //10
                    Constant.SettingParameter.SA_BRIDGING_WITH_MEDINLINK, //11
                    Constant.SettingParameter.SA0138 //12
                ));
            
            hdnIsBridgingToGateway.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).FirstOrDefault().ParameterValue;
            hdnProviderGatewayService.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).FirstOrDefault().ParameterValue;
            hdnIsAutomaticallyCheckedIn.Value = lstSetPar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_OUTPATIENT_REGISTRATION_AUTOMATICALLY_CHECKED_IN).FirstOrDefault().ParameterValue;
            hdnIsUsingPatientCall.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.OP_IS_USING_CALL_PATIENT_FEATURE).FirstOrDefault().ParameterValue;
            hdnIsUsingClinicService.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.OP_IS_USING_CLINIC_SERVICE_FEATURE).FirstOrDefault().ParameterValue;
            hdnIsControlAdministrationCharges.Value = lstSetParDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KONTROL_KODE_BIAYA_ADMINISTRASI_RJ).ParameterValue;
            hdnChargeCodeAdministrationForInstansi.Value = lstSetParDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI_RJ_INSTANSI).ParameterValue;
            hdnIsControlAdmCost.Value = lstSetParDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KONTROL_BIAYA_ADM_PASIEN_RJ).ParameterValue;
            hdnAdminID.Value = lstSetParDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI).ParameterValue;
            hdnIsControlPatientCardPayment.Value = lstSetParDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KONTROL_BIAYA_KARTU).ParameterValue;
            hdnItemCardFee.Value = lstSetParDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KODE_PELAYANAN_KARTU).ParameterValue;
            hdnIsBridgingWithMedinlink.Value = lstSetParDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_BRIDGING_WITH_MEDINLINK).ParameterValue;
            hdnIsBridgingToMobileJKN.Value = lstSetParDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.SA0138).FirstOrDefault().ParameterValue;
            
        }

        public override void OnGrdRowClick(string oVisitID)
        {
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", oVisitID)).FirstOrDefault();
            Response.Redirect(GetResponseRedirectUrl(entity));
        }

        private string GetResponseRedirectUrl(vConsultVisit4 entity)
        {
            string url = "";
            string id = Page.Request.QueryString["id"];

            LastContentVisitListOP lc = new LastContentVisitListOP()
            {
                HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value),
                ClinicHealthcareServiceUnitID = Convert.ToInt32(cboClinic.Value),
                RegistrationDate = txtRegistrationDate.Text,
                RegistrationDate2 = txtRegistrationDatePatientCall.Text,
                ParamedicID = hdnPhysicianID.Value == string.Empty ? 0 : Convert.ToInt32(hdnPhysicianID.Value),
                ParamedicIDPatientCall = hdnPhysicianIDPatientCall.Value == string.Empty ? 0 : Convert.ToInt32(hdnPhysicianIDPatientCall.Value),
                QuickText = txtSearchView.Text,
                QuickTextPatientCall = txtSearchViewPatientCall.Text,
                QuickFilterExpression = hdnFilterExpressionQuickSearch.Value,
                QuickFilterExpressionPatientCall = hdnFilterExpressionQuickSearchPatientCall.Value,
                MedicalNo = txtBarcodeEntry.Text
            };
            AppSession.LastContentVisitListOP = lc;

            string paramValue = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_TRANSAKSI_HANYA_DAPAT_DIUBAH_UNIT_ASAL).ParameterValue;
            AppSession.IsAdminCanCancelAllTransaction = paramValue == "0" ? true : false;

            if (id == "bs" || id == "ptp" || id == "pe")
            {
                RegisteredPatient pt = new RegisteredPatient();
                pt.MRN = entity.MRN;
                pt.MedicalNo = entity.MedicalNo;
                pt.GCGender = entity.GCGender;
                pt.GCSex = entity.GCSex;
                pt.DateOfBirth = entity.DateOfBirth;
                pt.RegistrationID = entity.RegistrationID;
                pt.RegistrationNo = entity.RegistrationNo;
                pt.RegistrationDate = entity.RegistrationDate;
                pt.RegistrationTime = entity.RegistrationTime;
                pt.VisitID = entity.VisitID;
                pt.VisitDate = entity.VisitDate;
                pt.VisitTime = entity.VisitTime;
                pt.StartServiceDate = entity.StartServiceDate;
                pt.StartServiceTime = entity.StartServiceTime;
                pt.DischargeDate = entity.DischargeDate;
                pt.DischargeTime = entity.DischargeTime;
                pt.GCCustomerType = entity.GCCustomerType;
                pt.BusinessPartnerID = entity.BusinessPartnerID;
                pt.ParamedicID = entity.ParamedicID;
                pt.ParamedicCode = entity.ParamedicCode;
                pt.ParamedicName = entity.ParamedicName;
                pt.SpecialtyID = entity.SpecialtyID;
                pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                pt.DepartmentID = entity.DepartmentID;
                pt.ServiceUnitName = entity.ServiceUnitName;
                pt.RoomCode = entity.RoomCode;
                pt.BedCode = entity.BedCode;
                pt.DepartmentID = entity.DepartmentID;
                pt.ChargeClassID = entity.ChargeClassID;
                pt.ClassID = entity.ClassID;
                pt.GCRegistrationStatus = entity.GCVisitStatus;
                pt.IsLockDown = entity.IsLockDown;
                pt.IsBillingReopen = entity.IsBillingReopen;
                pt.LinkedRegistrationID = entity.LinkedRegistrationID;
                pt.LinkedToRegistrationID = entity.LinkedToRegistrationID;
                pt.LastAcuteInitialAssessmentDate = entity.LastAcuteInitialAssessmentDate;
                pt.LastChronicInitialAssessmentDate = entity.LastChronicInitialAssessmentDate;
                pt.IsNeedRenewalAcuteInitialAssessment = Helper.GetIsNeedRenewalOPInitialAssessment(pt.LastAcuteInitialAssessmentDate, Convert.ToInt16(AppSession.OP0027));
                pt.IsNeedRenewalChronicInitialAssessment = Helper.GetIsNeedRenewalOPInitialAssessment(pt.LastChronicInitialAssessmentDate, Convert.ToInt16(AppSession.OP0027));
                
                AppSession.RegisteredPatient = pt;

                AppSession.HealthcareServiceUnitID = cboServiceUnit.Value.ToString();

                //if (id == "bs" || id == "ptp")
                //{
                string parentCode = "";
                if (id == "bs")
                {
                    parentCode = Constant.MenuCode.Outpatient.BILL_SUMMARY;
                    string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
                    List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                    int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

                    filterExpression = string.Format("ParentID = {0}", parentID);
                    lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                    GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
                    url = Page.ResolveUrl(menu.MenuUrl);
                }
                else if (id == "ptp")
                {
                    parentCode = Constant.MenuCode.Outpatient.PATIENT_TRANSACTION_PAGE;
                    string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
                    List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                    int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

                    filterExpression = string.Format("ParentID = {0}", parentID);
                    lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                    GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
                    url = Page.ResolveUrl(menu.MenuUrl);
                }
                //else
                //{
                //    parentCode = Constant.MenuCode.Outpatient.PATIENT_PAGE;

                //    List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("ParentCode = '{0}'", parentCode)).OrderBy(p => p.MenuIndex).ToList();
                //    GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
                //    url = Page.ResolveUrl(menu.MenuUrl);
                //}
                //}
                //else
                //{
                //    url = "~/Libs/Program/Module/PatientManagement/PatientAssessment/PatientStatus.aspx";
                //}
            }
            else
                url = string.Format("~/Libs/Program/Module/PatientManagement/ChangePatientTransactionStatus/ChangePatientTransactionStatusEntry.aspx?id={0}", entity.VisitID);
            return url;
        }

        protected void cbpBarcodeEntryProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExpression = GetFilterExpression();
            filterExpression += string.Format(" AND MedicalNo = '{0}'", txtBarcodeEntry.Text);
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(filterExpression).FirstOrDefault();

            string url = "";
            if (entity != null)
                url = GetResponseRedirectUrl(entity);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpUrl"] = url;
        }

        protected void cbpPatientCheckedIn_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            bool isError = false;
            if (param[0] == "checkin")
            {
                int regID = Convert.ToInt32(param[1]);
                int visitID = Convert.ToInt32(param[2]);

                IDbContext ctx = DbFactory.Configure(true);
                RegistrationDao entityRegDao = new RegistrationDao(ctx);
                ConsultVisitDao entityVisitDao = new ConsultVisitDao(ctx);
                try
                {
                    Registration entityReg = BusinessLayer.GetRegistration(regID);
                    ConsultVisit entityVisit = BusinessLayer.GetConsultVisit(visitID);

                    entityReg.GCRegistrationStatus = Constant.VisitStatus.CHECKED_IN;
                    entityReg.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityReg.LastUpdatedDate = DateTime.Now;
                    entityRegDao.Update(entityReg);

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();

                    if (!string.IsNullOrEmpty(hdnRoomID.Value))
                    {
                        entityVisit.RoomID = Convert.ToInt32(hdnRoomID.Value);
                    }
                    entityVisit.GCVisitStatus = Constant.VisitStatus.CHECKED_IN;
                    entityVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityVisit.LastUpdatedDate = DateTime.Now;
                    entityVisitDao.Update(entityVisit);

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();

                    String ChargeCodeAdministration = string.Empty;
                    bool flagSkipAdm = false;
                    if (hdnIsControlAdministrationCharges.Value == "1" && entityReg.GCCustomerType != Constant.CustomerType.PERSONAL) ChargeCodeAdministration = hdnChargeCodeAdministrationForInstansi.Value;
                    int itemMCUID = 0;
                    if (hdnIsControlAdmCost.Value == "1")
                    {
                        if (entityReg.MRN != 0 && entityReg.MRN != null)
                        {
                            int flagAdmCount = BusinessLayer.GetvPatientChargesDtList(string.Format("MRN = {0} AND ItemID = '{1}' AND datediff(day, RegistrationDate, '{2}') = 0  AND GCTransactionStatus = '{3}'", entityReg.MRN, hdnAdminID.Value, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.CLOSED), ctx).ToList().Count;
                            if (flagAdmCount > 0) flagSkipAdm = true;
                        }
                    }
                    //Check : Kontrol Pembayaran Kartu
                    if (hdnIsControlPatientCardPayment.Value != "1")
                    {
                        Helper.InsertAutoBillItem(ctx, entityVisit, Constant.Facility.OUTPATIENT, (int)entityVisit.ChargeClassID, entityReg.GCCustomerType, entityReg.IsPrintingPatientCard, itemMCUID, ChargeCodeAdministration, flagSkipAdm);
                    }
                    else
                    {
                        if (entityReg.IsPrintingPatientCard && hdnItemCardFee.Value != "")
                        {
                            Helper.InsertPatientCardBillItem(ctx, entityVisit, Constant.Facility.OUTPATIENT, (int)entityVisit.ChargeClassID);
                        }
                        Helper.InsertAutoBillItem(ctx, entityVisit, Constant.Facility.OUTPATIENT, (int)entityVisit.ChargeClassID, entityReg.GCCustomerType, false, itemMCUID, ChargeCodeAdministration, flagSkipAdm);
                    }

                    ctx.CommitTransaction();

                    if (hdnIsBridgingToMobileJKN.Value == "1")
                    {
                        BusinessLayer.OnInsertBPJSTaskLog(entityReg.RegistrationID, 4, AppSession.UserLogin.UserID, DateTime.Now);
                    }
                }
                catch
                {
                    ctx.RollBackTransaction();
                    ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
                    panel.JSProperties["cpUrl"] = string.Empty;
                }
                finally
                {
                    ctx.Close();

                    if (param[0] == "checkin" && !isError)
                    {
                        vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", visitID))[0];
                        string url = GetResponseRedirectUrl(entity);

                        ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
                        panel.JSProperties["cpUrl"] = url;
                    }
                }
            }
        }

        protected void cbpInfoParamedicScheduleDateView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCountDate = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDate(Convert.ToInt32(param[1]), false, ref pageCountDate);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDate(1, true, ref pageCountDate);
                    result = "refresh|" + pageCountDate;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewDate(int pageIndexDate, bool isCountPageCount, ref int pageCountDate)
        {
            DateTime ScheduleDate = Helper.GetDatePickerValue(txtScheduleDate.Text);
            int daynumber = (int)ScheduleDate.DayOfWeek;

            if (daynumber == 0)
            {
                daynumber = 7;
            }

            string filterExpression = String.Format("IsUsingRegistration = 1 AND IsDeleted = 0 AND DepartmentID = '{0}' AND ServiceUnitID IN (SELECT ServiceUnitID FROM vHealthcareServiceUnit WHERE HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM ParamedicSchedule WHERE DayNumber = '{1}'))", Constant.Facility.OUTPATIENT, daynumber);
            string filterExpressionDate = String.Format("IsUsingRegistration = 1 AND IsDeleted = 0 AND DepartmentID = '{0}' AND ServiceUnitID IN (SELECT ServiceUnitID FROM vHealthcareServiceUnit WHERE HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM ParamedicScheduleDate WHERE ScheduleDate = '{1}'))", Constant.Facility.OUTPATIENT, ScheduleDate);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetServiceUnitMasterRowCount(filterExpression) + BusinessLayer.GetServiceUnitMasterRowCount(filterExpressionDate);
                pageCountDate = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            filterExpression += String.Format(" ORDER BY ServiceUnitName ASC");
            filterExpressionDate += String.Format("ORDER BY ServiceUnitName ASC");

            List<ServiceUnitMaster> lstEntity = BusinessLayer.GetServiceUnitMasterList(filterExpression);
            List<ServiceUnitMaster> lstEntityDate = BusinessLayer.GetServiceUnitMasterList(filterExpressionDate);
            lstEntity.AddRange(lstEntityDate);

            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            DateTime ScheduleDate = Helper.GetDatePickerValue(txtScheduleDate.Text);
            int daynumber = (int)ScheduleDate.DayOfWeek;
            string filterExpression = string.Format("HealthcareServiceUnitID = {0}", hdnCboClinicValue.Value);
            int healthcareServiceUnitID = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression).FirstOrDefault().HealthcareServiceUnitID;
            if (daynumber == 0)
            {
                daynumber = 7;
            }

            String FilterScheduleDate = Helper.GetDatePickerValue(txtScheduleDate).ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
            string filterExpressionDate = string.Format("TempDate IN ('{0}','{1}') AND HealthcareServiceUnitID = {2}", daynumber, FilterScheduleDate, hdnID.Value);

            List<GetParamedicScheduleClinicStatus> lstEntityDate = null;
            lstEntityDate = BusinessLayer.GetParamedicScheduleClinicStatusList(daynumber, Helper.GetDatePickerValue(txtScheduleDate).ToString(Constant.FormatString.DATE_FORMAT_112), healthcareServiceUnitID);
            lvwView.DataSource = lstEntityDate;
            lvwView.DataBind();

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    result = "changepage";
                }
                else if (param[0] == "start")
                {
                    int paramedicID = Convert.ToInt32(param[1]);
                    int hsuID = Convert.ToInt32(param[2]);
                    string departmentID = param[3];
                    int tempDate = Convert.ToInt32(param[4]);
                    int operationalTimeID = Convert.ToInt32(param[5]);
                    OnStartClinicRecord(paramedicID, hsuID, departmentID, tempDate, operationalTimeID);

                    lstEntityDate = BusinessLayer.GetParamedicScheduleClinicStatusList(daynumber, Helper.GetDatePickerValue(txtScheduleDate).ToString(Constant.FormatString.DATE_FORMAT_112), healthcareServiceUnitID);
                    lvwView.DataSource = lstEntityDate;
                    lvwView.DataBind(); lvwView.DataBind();
                }
                else if (param[0] == "stop")
                {
                    int paramedicID = Convert.ToInt32(param[1]);
                    int hsuID = Convert.ToInt32(param[2]);
                    string departmentID = param[3];
                    int tempDate = Convert.ToInt32(param[4]);
                    int operationalTimeID = Convert.ToInt32(param[5]);
                    OnStopClinicRecord(paramedicID, hsuID, departmentID, tempDate, operationalTimeID);

                    lstEntityDate = BusinessLayer.GetParamedicScheduleClinicStatusList(daynumber, Helper.GetDatePickerValue(txtScheduleDate).ToString(Constant.FormatString.DATE_FORMAT_112), healthcareServiceUnitID);
                    lvwView.DataSource = lstEntityDate;
                    lvwView.DataBind();
                }
                else
                {
                    lstEntityDate = BusinessLayer.GetParamedicScheduleClinicStatusList(daynumber, Helper.GetDatePickerValue(txtScheduleDate).ToString(Constant.FormatString.DATE_FORMAT_112), healthcareServiceUnitID);
                    lvwView.DataSource = lstEntityDate;
                    lvwView.DataBind();
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewPatientCall_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            bool isError = false;
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "refresh")
                {
                    BindingViewPatientCall();
                    result = "refresh|" + pageCount;
                }
                else if (param[0] == "call")
                {
                    int regID = Convert.ToInt32(param[1]);
                    int visitID = Convert.ToInt32(param[2]);
                    string roomCode = param[3];
                    if (hdnIsBridgingWithMedinlink.Value == "1")
                    {
                        NotificationRegistrationTicketNo(regID, visitID, roomCode);
                    }
                    BindingViewPatientCall();
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindingViewPatientCall()
        {
            string filterExpression = "";

            filterExpression = string.Format("DepartmentID = '{0}' AND GCVisitStatus = '{1}'", Constant.Facility.OUTPATIENT, Constant.VisitStatus.OPEN);


            if (!chkIsPreviousEpisodePatientCall.Checked)
            {
                filterExpression += string.Format("  AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtRegistrationDatePatientCall).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (cboClinic.Value.ToString() != "0")
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboClinic.Value);
            }
            else
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID IN ({0}) AND IsMainVisit = 1", hdnLstHealthcareServiceUnitID.Value);
            }

            if (txtPhysicianCodePatientCall.Text != "")
            {
                filterExpression += string.Format(" AND ParamedicID = {0}", hdnPhysicianIDPatientCall.Value);
            }

            if (hdnFilterExpressionQuickSearchPatientCall.Value.ToString() != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchPatientCall.Value);
            }

            List<vConsultVisit9> lstSum = BusinessLayer.GetvConsultVisit9List(filterExpression, Constant.GridViewPageSize.GRID_TEMP_MAX_FOR_ORDERING_2, 1, "QueueNo, Session ASC");
            lstViewPatientCall.DataSource = lstSum;
            lstViewPatientCall.DataBind();
        }

        private void OnStartClinicRecord(int paramedicID, int hsuID, string departmentID, int tempDate, int operationalTimeID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ClinicServiceStatusDao entityDao = new ClinicServiceStatusDao(ctx);

            try
            {
                ClinicServiceStatus entity = new ClinicServiceStatus();
                ClinicServiceStatus entityClinic = BusinessLayer.GetClinicServiceStatusList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1}", hsuID, paramedicID)).FirstOrDefault();
                if (entityClinic == null)
                {
                    entity.HealthcareServiceunitID = hsuID;
                    //entity.RoomID = 
                    entity.ParamedicID = paramedicID;
                    entity.GCClinicStatus = Constant.ClinicStatus.STARTED;
                    entity.StartDate = DateTime.Now;
                    entity.StartTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    entity.StartBy = AppSession.UserLogin.UserID;
                    entity.GCPausedReason = string.Empty;
                    entity.OtherPausedReason = string.Empty;
                    entityDao.Insert(entity);

                    if (hdnIsBridgingToGateway.Value == "1")
                    {
                        if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                        {
                            string filterExpression = string.Format("GCVisitStatus = '{0}' AND HealthcareServiceUnitID = {1} AND ParamedicID = {2} AND VisitDate = '{3}' ORDER BY VisitID", Constant.VisitStatus.CHECKED_IN, hsuID, paramedicID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
                            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(filterExpression).FirstOrDefault();
                            if (entityCV != null)
                            {
                                SendNotification("1", entityCV.QueueNo.ToString(), entityCV.HealthcareServiceUnitID.ToString(), entityCV.ParamedicID.ToString());
                            }
                            else
                            {
                                SendNotification("1", "0", hsuID.ToString(), paramedicID.ToString());
                            }
                        }
                    }
                }
                else
                {
                    entityClinic.GCClinicStatus = Constant.ClinicStatus.STARTED;
                    entityClinic.StartDate = DateTime.Now;
                    entityClinic.StartTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    entityClinic.StartBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entityClinic);

                    if (hdnIsBridgingToGateway.Value == "1")
                    {
                        if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                        {
                            string filterExpression = string.Format("GCVisitStatus = '{0}' AND HealthcareServiceUnitID = {1} AND ParamedicID = {2} AND VisitDate = '{3}' ORDER BY VisitID DESC", Constant.VisitStatus.RECEIVING_TREATMENT, hsuID, paramedicID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
                            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(filterExpression).FirstOrDefault();
                            if (entityCV != null)
                            {
                                SendNotification("3", entityCV.QueueNo.ToString(), entityCV.HealthcareServiceUnitID.ToString(), entityCV.ParamedicID.ToString());
                            }
                        }
                    }
                }

                ctx.CommitTransaction();
            }
            catch
            {
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
        }

        private void OnStopClinicRecord(int paramedicID, int hsuID, string departmentID, int tempDate, int operationalTimeID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ClinicServiceStatusDao entityDao = new ClinicServiceStatusDao(ctx);

            try
            {
                ClinicServiceStatus entity = BusinessLayer.GetClinicServiceStatusList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1}", hsuID, paramedicID)).FirstOrDefault();
                if (entity != null)
                {
                    entity.GCClinicStatus = Constant.ClinicStatus.STOPPED;
                    entity.StopDate = DateTime.Now;
                    entity.StopTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    entity.StopBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);
                    ctx.CommitTransaction();
                    result = true;

                    if (hdnIsBridgingToGateway.Value == "1")
                    {
                        if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                        {
                            string filterExpression = string.Format("GCVisitStatus = '{0}' AND HealthcareServiceUnitID = {1} AND ParamedicID = {2} AND VisitDate = '{3}' ORDER BY VisitID DESC", Constant.VisitStatus.RECEIVING_TREATMENT, hsuID, paramedicID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
                            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(filterExpression).FirstOrDefault();
                            if (entityCV != null)
                            {
                                SendNotification("4", entityCV.QueueNo.ToString(), entityCV.HealthcareServiceUnitID.ToString(), entityCV.ParamedicID.ToString());
                            }
                        }
                    }
                }
                else
                {
                    ctx.RollBackTransaction();
                    result = false;
                }
            }
            catch
            {
                ctx.RollBackTransaction();
                result = false;
            }
            finally
            {
                ctx.Close();
            }
        }

        private void SendNotification(string jenisNotif, string queueNo, string hsuID, string paramedicID)
        {
            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
            {
                GatewayService oService = new GatewayService();
                APIMessageLog entityAPILog = new APIMessageLog();
                string apiResult = oService.OnChangeClinicStatus(jenisNotif, queueNo, hsuID, paramedicID, string.Empty);
                string[] apiResultInfo = apiResult.Split('|');
                if (apiResultInfo[0] == "0")
                {
                    entityAPILog.IsSuccess = false;
                    entityAPILog.MessageText = apiResultInfo[1];
                    entityAPILog.Response = apiResultInfo[1];
                    Exception ex = new Exception(apiResultInfo[1]);
                    Helper.InsertErrorLog(ex);
                }
                else
                {
                    entityAPILog.MessageText = apiResultInfo[0];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                }
            }
        }

        private void NotificationRegistrationTicketNo(int registrationID, int visitID, string roomCode)
        {
            vConsultVisit9 entity = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0} AND VisitID = {1}", registrationID, visitID)).FirstOrDefault();
            if (entity != null)
            {
                try
                {
                    APIMessageLog entityAPILog = new APIMessageLog()
                    {
                        MessageDateTime = DateTime.Now,
                        Recipient = Constant.BridgingVendor.MEDINLINK,
                        Sender = Constant.BridgingVendor.HIS,
                        IsSuccess = true
                    };
                    QueueService oService = new QueueService();
                    string apiResult = oService.MedinlinkPatientCallNotificationViaAPIMedin(entity, roomCode); ////oService.MedinlinkPatientCallNotification(entity, roomCode);
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.Response = apiResultInfo[1];
                        Exception ex = new Exception(apiResultInfo[1]);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                    {
                        entityAPILog.MessageText = apiResultInfo[1];
                    }

                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                }
                catch (Exception ex)
                {
                    Helper.InsertErrorLog(ex);
                }
            }
        }
    }
}