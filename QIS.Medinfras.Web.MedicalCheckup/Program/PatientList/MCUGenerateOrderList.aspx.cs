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
    public partial class MCUGenerateOrderList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalCheckup.GENERATE_ORDER_NEW;
        }

        private GetUserMenuAccess menu;
        private string refreshGridInterval = "";

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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string id = Page.Request.QueryString["id"];

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                List<StandardCode> lstVariable = new List<StandardCode>();
                lstVariable.Insert(0, new StandardCode { StandardCodeName = "", StandardCodeID = "0" });
                lstVariable.Insert(1, new StandardCode { StandardCodeName = "CLOSED", StandardCodeID = "1" });
                lstVariable.Insert(2, new StandardCode { StandardCodeName = "NON-CLOSED", StandardCodeID = "2" });
                Methods.SetComboBoxField<StandardCode>(cboResultType, lstVariable, "StandardCodeName", "StandardCodeID");
                cboResultType.SelectedIndex = 2;

                if (id == "resultform")
                {
                    IsShowRightPanel();
                }
                grdRegisteredPatient.InitializeControl();

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                AppSession.LastPatientVisitMCUForm = null;
            }
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Empty;

            filterExpression = string.Format("DepartmentID = '{0}' AND GCVisitStatus IN ('{1}','{2}','{3}') AND GCItemDetailStatus = '{4}'",
                                                    Constant.Facility.MEDICAL_CHECKUP,
                                                    Constant.VisitStatus.CHECKED_IN,
                                                    Constant.VisitStatus.RECEIVING_TREATMENT,
                                                    Constant.VisitStatus.PHYSICIAN_DISCHARGE,
                                                    Constant.TransactionStatus.OPEN
                                                );

            if (!chkIsPreviousEpisodePatient.Checked)
            {
                filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            return filterExpression;
        }

        public override void OnGrdRowClick(string param)
        {
            string[] paramList = param.Split('|');
            string oVisitID = paramList[0];
            string oConsultVisitItemPackageID = paramList[1];

            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", oVisitID)).FirstOrDefault();
            Response.Redirect(GetResponseRedirectUrl(entity, oConsultVisitItemPackageID));
        }

        private string GetResponseRedirectUrl(vConsultVisit4 entity, string ConsultVisitItemPackageID)
        {
            string paramValue = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_TRANSAKSI_HANYA_DAPAT_DIUBAH_UNIT_ASAL).ParameterValue;
            AppSession.IsAdminCanCancelAllTransaction = paramValue == "0" ? true : false;

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

            string url = string.Format("~/Program/Worklist/GenerateOrder/MCUOrderDetail.aspx?id={0}|{1}", entity.VisitID, ConsultVisitItemPackageID);

            return url;
        }

    }
}