using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class VisitList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case "ptp": return Constant.MenuCode.MedicalCheckup.GENERATE_ORDER_NEW;
                case "hsl": return Constant.MenuCode.MedicalCheckup.MCU_RESULT_EXTRENAL;
                case "resultform": return Constant.MenuCode.MedicalCheckup.MCU_RESULT_FORM;
                default: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY;
            }
        }
        private GetUserMenuAccess menu;
        public override bool IsShowRightPanel()
        {
            return true;
        }
        protected String GetMenuCaption()
        {
            return menu.MenuCaption;
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
                string id = Page.Request.QueryString["id"];

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                hdnIsUsedReopenBilling.Value = AppSession.IsUsedReopenBilling ? "1" : "0";
                loadSetpar();

                List<StandardCode> lstVariable = new List<StandardCode>();
                lstVariable.Insert(0, new StandardCode { StandardCodeName = "Semua", StandardCodeID = "0" });
                lstVariable.Insert(1, new StandardCode { StandardCodeName = "Closed", StandardCodeID = "1" });
                lstVariable.Insert(2, new StandardCode { StandardCodeName = "Non Closed", StandardCodeID = "2" });
                Methods.SetComboBoxField<StandardCode>(cboResultType, lstVariable, "StandardCodeName", "StandardCodeID");
                cboResultType.SelectedIndex = 2;

                if (id == "bs")
                {
                    trFilterReopenBilling.Visible = AppSession.IsUsedReopenBilling;
                    trIsShowRegistrationStatus.Visible = false;
                }
                else
                {
                    trFilterReopenBilling.Visible = false;
                }

                if (id == "resultform")
                {
                    IsShowRightPanel();
                }
                grdRegisteredPatient.InitializeControl();

                Helper.SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                txtBarcodeEntry.Focus();

                AppSession.LastPatientVisitMCUForm = null;       
            }
        }
        private void loadSetpar()
        {
            List<SettingParameterDt> setPar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN('{0}') AND HealthcareID='001' ", Constant.SettingParameter.MC_MENGGUNAKAN_DEKSTOP_MCU));
            if (setPar.Count > 0)
            {
                string MCUDesktop = setPar.Where(p => p.ParameterCode == Constant.SettingParameter.MC_MENGGUNAKAN_DEKSTOP_MCU).FirstOrDefault().ParameterValue;
                if (MCUDesktop == "1") {
                    IsHasBillingNotes.Style.Remove("display");
                }

            }
        }
        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Empty;
            if (Page.Request.QueryString["id"] == "hsl" || Page.Request.QueryString["id"] == "resultform")
            {
                filterExpression = string.Format("DepartmentID = '{0}'", Constant.Facility.MEDICAL_CHECKUP);

                if (hdnIsUsedReopenBilling.Value == "1")
                {
                    if (chkIsIncludeReopenBilling.Checked)
                    {
                        filterExpression += string.Format(" AND GCVisitStatus IN ('{0}') AND IsBillingReopen = 1",
                                                                Constant.VisitStatus.CLOSED);
                    }
                }
                else
                {
                    if (Page.Request.QueryString["id"] == "resultform")
                    {
                        if (cboResultType.Value.ToString() == "0")
                        {
                            filterExpression += string.Format(" AND GCVisitStatus NOT IN ('{0}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.TRANSFERRED);
                        }
                        else if (cboResultType.Value.ToString() == "1")
                        {
                            filterExpression += string.Format(" AND GCVisitStatus IN ('{0}')", Constant.VisitStatus.CLOSED);
                        }
                        else if (cboResultType.Value.ToString() == "2")
                        {
                            filterExpression += string.Format(" AND GCVisitStatus NOT IN ('{0}','{1}')", Constant.VisitStatus.CLOSED, Constant.VisitStatus.CANCELLED);
                        }
                    }
                    else
                    {
                        filterExpression += string.Format(" AND GCVisitStatus NOT IN ('{0}','{1}','{2}')",
                                                                Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN);
                    }
                }
            }
            else if (Page.Request.QueryString["id"] == "bs")
            {
                filterExpression = string.Format("DepartmentID = '{0}' AND GCVisitStatus NOT IN ('{1}','{2}','{3}') AND VisitID IN (SELECT VisitID FROM ConsultVisitItemPackage)",
                                        Constant.Facility.MEDICAL_CHECKUP,
                                        Constant.VisitStatus.CANCELLED,
                                        Constant.VisitStatus.CLOSED,
                                        Constant.VisitStatus.OPEN
                                    );
            }
            else
            {
                filterExpression = string.Format("DepartmentID = '{0}' AND GCVisitStatus IN ('{1}','{2}','{3}') AND VisitID IN (SELECT VisitID FROM ConsultVisitItemPackage WHERE GCItemDetailStatus = '{4}')",
                                                        Constant.Facility.MEDICAL_CHECKUP,
                                                        Constant.VisitStatus.CHECKED_IN,
                                                        Constant.VisitStatus.RECEIVING_TREATMENT,
                                                        Constant.VisitStatus.PHYSICIAN_DISCHARGE,
                                                        Constant.TransactionStatus.OPEN
                                                    );
            }

            if (!chkIsPreviousEpisodePatient.Checked)
            {
                filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            if (hdnSearchBarcodeNoRM.Value != "" && hdnSearchBarcodeNoRM.Value != null && hdnSearchBarcodeNoRM.Value != "00-00-00-00")
            {
                filterExpression += string.Format(" AND MedicalNo = '{0}'", hdnSearchBarcodeNoRM.Value);
            }

            if (chkIsIncludeHasBillingNotes.Checked)
            {
                filterExpression += string.Format(" AND IsHasBillingNotes = 1");
            }

            return filterExpression;
        }

        public override void OnGrdRowClick(string oVisitID)
        {
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", oVisitID)).FirstOrDefault();
            Response.Redirect(GetResponseRedirectUrl(entity));
        }

        private string GetResponseRedirectUrl(vConsultVisit4 entity)
        {
            string paramValue = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_TRANSAKSI_HANYA_DAPAT_DIUBAH_UNIT_ASAL).ParameterValue;
            AppSession.IsAdminCanCancelAllTransaction = paramValue == "0" ? true : false;

            string url = "";
            string id = Page.Request.QueryString["id"];
            if (id == "bs" || id == "ptp" || id == "hsl" || id == "resultform")
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
                AppSession.RegisteredPatient = pt;

                string parentCode = "";
                if (id == "bs")
                {
                    parentCode = Constant.MenuCode.MedicalCheckup.BILL_SUMMARY;
                    string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
                    List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.MEDICAL_CHECKUP, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                    int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

                    filterExpression = string.Format("ParentID = {0}", parentID);
                    lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.MEDICAL_CHECKUP, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                    GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
                    url = Page.ResolveUrl(menu.MenuUrl);
                }
                else if (id == "hsl")
                {
                    url = string.Format("~/Program/ResultList/MCUResultDetail.aspx?id={0}", entity.VisitID);
                }
                else if (id == "resultform")
                {

                        LastPatientVisitMCUForm oLastVisit = new LastPatientVisitMCUForm()
                        {
                             VisitID = entity.VisitID,
                        };
                        AppSession.LastPatientVisitMCUForm = oLastVisit;
                    
                    url = string.Format("~/Program/MCUResultForm/MCUResultFormEntry.aspx");
                }
                else
                {
                    url = string.Format("~/Program/Worklist/GenerateOrder/MCUOrderDetail.aspx?id={0}", entity.VisitID);
                }
            }
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
    }
}