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

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class ProcessBedReservationList : BasePagePatientOrder
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.PROCESS_BED_RESERVATION_LIST;
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
            refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LB_INTERVAL_AUTO_REFRESH).ParameterValue;

            String filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}'", AppSession.UserLogin.HealthcareID, Constant.Facility.INPATIENT);
            List<vHealthcareServiceUnit> lstHServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            List<ClassCare> lstCC = BusinessLayer.GetClassCareList(string.Format("IsDeleted = 0"));
            //lstHServiceUnit.Insert(0, new vHealthcareServiceUnit { ServiceUnitName = string.Format("{0}", GetLabel(" ")) });
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnitTitipan, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            lstCC.Insert(0, new ClassCare { ClassName = string.Format("{0}", GetLabel("")) });
            Methods.SetComboBoxField<ClassCare>(cboClassRequest, lstCC, "ClassName", "ClassID");
            cboServiceUnitTitipan.SelectedIndex = 0;
            cboClassRequest.SelectedIndex = 0;

            List<Variable> lstSortBy = new List<Variable>();
            lstSortBy.Add(new Variable { Code = "0", Value = " " });
            lstSortBy.Add(new Variable { Code = "1", Value = "Tgl. Reservasi" });
            lstSortBy.Add(new Variable { Code = "2", Value = "Kelas Permintaan" });
            lstSortBy.Add(new Variable { Code = "3", Value = "Penjamin Bayar" });
            lstSortBy.Add(new Variable { Code = "4", Value = "Asal Pasien" });
            Methods.SetComboBoxField<Variable>(cboSortBy, lstSortBy, "Value", "Code");
            //cboSortBy.Value = "0";
            cboSortBy.SelectedIndex = 0;

            CtlGrdPatientTitipan.InitializeControl();
            CtlGrdPatientReservasi.InitializeControl();
        }

        public override string GetFilterExpression()
        {
            string filterExpression = "";
            string cboServiceUnitTitipan = hdncboServiceUnitTitipan.Value;
            if (cboServiceUnitTitipan == "")
            {
                filterExpression = string.Format("RequestClassID IS NOT NULL AND GCVisitStatus IN ('{0}', '{1}')", Constant.VisitStatus.OPEN, Constant.VisitStatus.CHECKED_IN);
            }else{
                filterExpression = string.Format("RequestClassID IS NOT NULL AND GCVisitStatus IN ('{0}', '{1}') AND HealthcareServiceUnitID = {2}", Constant.VisitStatus.OPEN, Constant.VisitStatus.CHECKED_IN, cboServiceUnitTitipan);
            }
            
            if (hdnFilterExpressionQuickSearchTitipan.Value != "")
            {
                filterExpression += String.Format(" AND {0}", hdnFilterExpressionQuickSearchTitipan.Value);
            }
            return filterExpression;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string ReservasiID)
        {
            string url = "";
            url = string.Format("~/Program/BedReservation/BedReservationEntry.aspx?id={0}", ReservasiID);
            Response.Redirect(url);
        }

        public override void OnGrdRowClickTestOrder(string ReservasiID, string RegistrationID)
        {
            string url = "";
            url = string.Format("~/Program/BedReservation/BedReservationEntry.aspx?id={0}|{1}", ReservasiID, RegistrationID);
            Response.Redirect(url);
        }

        public override string GetFilterExpressionTestOrder()
        {
            string filterExpression = "";
            string cboClassRequestReservasi = hdncboClassRequest.Value;
            if (cboClassRequestReservasi == "" || cboClassRequestReservasi == "0")
            {
                filterExpression = string.Format("GCReservationStatus IN ('{0}')", Constant.Bed_Reservation_Status.PROPOSED);
            }
            else
            {
                filterExpression = string.Format("GCReservationStatus IN ('{0}') AND ClassID = {1}", Constant.Bed_Reservation_Status.PROPOSED, cboClassRequestReservasi);
            }

            filterExpression += string.Format(" AND RegistrationID NOT IN (SELECT RegistrationID FROM PatientTransfer WHERE GCPatientTransferStatus != '{0}')", Constant.PatientTransferStatus.TRANSFERRED);

            if (hdnFilterExpressionQuickSearchReservasi.Value != "")
            {
                filterExpression += String.Format(" AND {0}", hdnFilterExpressionQuickSearchReservasi.Value);
            }
            return filterExpression;
        }

        public override string GetSortingTestOrder()
        {
            string sortBy = "";

            if (hdnSortBy.Value == "1")
            {
                sortBy += "ReservationDate ASC";
            }
            else if (hdnSortBy.Value  == "2")
            {
                sortBy += "ClassName ASC";
            }
            else if (hdnSortBy.Value == "3")
            {
                sortBy += "BusinessPartnerName ASC";
            }
            else if (hdnSortBy.Value == "4")
            {
                sortBy += "PatientOrigin ASC";
            }
            else
            {
                sortBy += "ReservationID ASC";
            }

            return sortBy;
        }
    }
}