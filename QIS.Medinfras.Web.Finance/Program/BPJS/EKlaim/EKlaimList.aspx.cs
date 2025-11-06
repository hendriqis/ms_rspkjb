using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class EKlaimList : BasePageRegisteredPatient
    {
        private GetUserMenuAccess menu;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.BPJS_EKLAIM_ENTRY;
        }

        public override bool IsShowRightPanel()
        {
            return false;
        }

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                List<SettingParameterDt> lstSetVar = BusinessLayer.GetSettingParameterDtList(string.Format(
                    "HealthcareID = '{0}' AND Parametercode IN ('{1}')",
                    AppSession.UserLogin.HealthcareID, //0
                    Constant.SettingParameter.FN_IS_EKLAIM_LIST_INCLUDE_REGWITHLINKEDTO //1
                ));
                hdnIsShowRegistrationWithLinkedTo.Value = lstSetVar.Where(p => p.ParameterCode == Constant.SettingParameter.FN_IS_EKLAIM_LIST_INCLUDE_REGWITHLINKEDTO).FirstOrDefault().ParameterValue;
            
                List<Variable> lstJenisRawat = new List<Variable>();
                lstJenisRawat.Add(new Variable { Code = "0", Value = "Semua" });
                lstJenisRawat.Add(new Variable { Code = "1", Value = "Rawat Inap" });
                lstJenisRawat.Add(new Variable { Code = "2", Value = "Rawat Jalan" });
                Methods.SetComboBoxField<Variable>(cboJenisRawat, lstJenisRawat, "Value", "Code");
                cboJenisRawat.Value = "0";

                List<Variable> lstFilterNoSEP = new List<Variable>();
                lstFilterNoSEP.Add(new Variable { Code = "0", Value = "Semua" });
                lstFilterNoSEP.Add(new Variable { Code = "1", Value = "Ada No SEP" });
                lstFilterNoSEP.Add(new Variable { Code = "2", Value = "Tidak Ada No SEP" });
                Methods.SetComboBoxField<Variable>(cboFilterNoSEP, lstFilterNoSEP, "Value", "Code");
                cboFilterNoSEP.Value = "1";

                List<Variable> lstKelasSEP = new List<Variable>();
                lstKelasSEP.Add(new Variable { Code = "0", Value = "Semua" });
                lstKelasSEP.Add(new Variable { Code = "1", Value = "Kelas 1" });
                lstKelasSEP.Add(new Variable { Code = "2", Value = "Kelas 2" });
                lstKelasSEP.Add(new Variable { Code = "3", Value = "Kelas 3" });
                Methods.SetComboBoxField<Variable>(cboKelasSEP, lstKelasSEP, "Value", "Code");
                cboKelasSEP.Value = "0";

                txtSearchTanggalSEPFrom.Text = DateTime.Today.AddDays(-14).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtSearchTanggalSEPTo.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                List<Variable> lstJenisTanggalPulang = new List<Variable>();
                lstJenisTanggalPulang.Add(new Variable { Code = "1", Value = "Tgl Pulang" });
                lstJenisTanggalPulang.Add(new Variable { Code = "2", Value = "Tgl Pulang SEP" });
                Methods.SetComboBoxField<Variable>(cboJenisTanggalPulang, lstJenisTanggalPulang, "Value", "Code");
                cboJenisTanggalPulang.Value = "1";

                txtSearchTanggalPulangFrom.Text = DateTime.Today.AddDays(-14).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtSearchTanggalPulangTo.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                List<Variable> lstSortByCustom = new List<Variable>();
                lstSortByCustom.Add(new Variable { Code = "1", Value = "Tanggal + No Registrasi ASC" });
                lstSortByCustom.Add(new Variable { Code = "2", Value = "Tanggal + No Registrasi DESC" });
                lstSortByCustom.Add(new Variable { Code = "3", Value = "Tanggal + No SEP ASC" });
                lstSortByCustom.Add(new Variable { Code = "4", Value = "Tanggal + No SEP DESC" });
                Methods.SetComboBoxField<Variable>(cboSortByCustom, lstSortByCustom, "Value", "Code");
                cboSortByCustom.Value = "1";

                SettingControlProperties();

                grdEKlaimPatient.InitializeControl();
            }
        }

        private void SettingControlProperties()
        {
            if (AppSession.LastContentEKlaimList != null)
            {
                LastContentEKlaimList lc = AppSession.LastContentEKlaimList;
                cboFilterNoSEP.Value = lc.FilterNoSEP;
                cboJenisRawat.Value = lc.JenisRawat;
                chkAbaikanTanggalSEP.Checked = lc.AbaikanTanggalSEP;
                txtSearchTanggalSEPFrom.Text = lc.TanggalSEP_dari;
                txtSearchTanggalSEPTo.Text = lc.TanggalSEP_sampai;
                if (lc.AbaikanTanggalSEP)
                {
                    txtSearchTanggalSEPFrom.Attributes.Add("disabled", "disabled");
                    txtSearchTanggalSEPTo.Attributes.Add("disabled", "disabled");
                }
                else
                {
                    txtSearchTanggalSEPFrom.Attributes.Remove("disabled");
                    txtSearchTanggalSEPTo.Attributes.Remove("disabled");
                }
                chkAbaikanTanggalPulang.Checked = lc.AbaikanTanggalPulang;
                txtSearchTanggalPulangFrom.Text = lc.TanggalPulang_dari;
                txtSearchTanggalPulangTo.Text = lc.TanggalPulang_sampai;
                if (lc.AbaikanTanggalPulang)
                {
                    txtSearchTanggalPulangFrom.Attributes.Add("disabled", "disabled");
                    txtSearchTanggalPulangTo.Attributes.Add("disabled", "disabled");
                }
                else
                {
                    txtSearchTanggalPulangFrom.Attributes.Remove("disabled");
                    txtSearchTanggalPulangTo.Attributes.Remove("disabled");
                }
                hdnQuickText.Value = lc.QuickText.Equals("Search") ? "" : lc.QuickText;
                hdnFilterExpressionQuickSearch.Value = lc.QuickFilterExpression;
            }
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("GCRegistrationStatus != '{0}' AND GCCustomerType = '{1}'", Constant.VisitStatus.CANCELLED, Constant.CustomerType.BPJS);

            if (cboFilterNoSEP.Value.ToString() == "1")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("(NoSEP != '' AND NoSEP IS NOT NULL)");
            }
            else if (cboFilterNoSEP.Value.ToString() == "2")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("(NoSEP = '' OR NoSEP IS NULL)");
            }

            if (cboJenisRawat.Value.ToString() == "1")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("DepartmentID = '{0}'", Constant.Facility.INPATIENT);
            }
            else if (cboJenisRawat.Value.ToString() == "2")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("DepartmentID NOT IN('{0}')", Constant.Facility.INPATIENT);
            }

            if (!chkAbaikanTanggalSEP.Checked)
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("TanggalSEP BETWEEN '{0}' AND '{1}'",
                                                    Helper.GetDatePickerValue(txtSearchTanggalSEPFrom).ToString(Constant.FormatString.DATE_FORMAT_112),
                                                    Helper.GetDatePickerValue(txtSearchTanggalSEPTo).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (!chkAbaikanTanggalPulang.Checked)
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                if (cboJenisTanggalPulang.Value.ToString() == "2")
                {
                    filterExpression += string.Format("TanggalPulang BETWEEN '{0}' AND '{1}'",
                                                        Helper.GetDatePickerValue(txtSearchTanggalPulangFrom).ToString(Constant.FormatString.DATE_FORMAT_112),
                                                        Helper.GetDatePickerValue(txtSearchTanggalPulangTo).ToString(Constant.FormatString.DATE_FORMAT_112));
                }
                else
                {
                    filterExpression += string.Format("DischargeDate BETWEEN '{0}' AND '{1}'",
                                                        Helper.GetDatePickerValue(txtSearchTanggalPulangFrom).ToString(Constant.FormatString.DATE_FORMAT_112),
                                                        Helper.GetDatePickerValue(txtSearchTanggalPulangTo).ToString(Constant.FormatString.DATE_FORMAT_112));
                }
            }

            if (cboKelasSEP.Value.ToString() != "0")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("KelasSEP = '{0}'", cboKelasSEP.Value.ToString());

            }

            if (hdnIsShowRegistrationWithLinkedTo.Value == "1")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("LinkedToRegistrationID IS NULL");
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("{0}", hdnFilterExpressionQuickSearch.Value);
            }

            if (cboSortByCustom.Value.ToString() == "1")
            {
                filterExpression += "|RegistrationDate, RegistrationNo";
            }
            else if (cboSortByCustom.Value.ToString() == "2")
            {
                filterExpression += "|RegistrationDate DESC, RegistrationNo DESC";
            }
            else if (cboSortByCustom.Value.ToString() == "3")
            {
                filterExpression += "|TanggalSEP, NoSEP";
            }
            else if (cboSortByCustom.Value.ToString() == "4")
            {
                filterExpression += "|TanggalSEP DESC, NoSEP DESC";
            }

            return filterExpression;
        }


        public override void OnGrdRowClick(string registrationID)
        {
            RegistrationBPJS regBPJS = BusinessLayer.GetRegistrationBPJS(Convert.ToInt32(registrationID));
            if (regBPJS.NoSEP != null && regBPJS.NoSEP != "")
            {
                string id = Page.Request.QueryString["id"];

                LastContentEKlaimList lc = new LastContentEKlaimList()
                {
                    FilterNoSEP = cboFilterNoSEP.Value.ToString(),
                    JenisRawat = cboJenisRawat.Value.ToString(),
                    AbaikanTanggalSEP = chkAbaikanTanggalSEP.Checked,
                    TanggalSEP_dari = txtSearchTanggalSEPFrom.Text,
                    TanggalSEP_sampai = txtSearchTanggalSEPTo.Text,
                    AbaikanTanggalPulang = chkAbaikanTanggalPulang.Checked,
                    TanggalPulang_dari = txtSearchTanggalPulangFrom.Text,
                    TanggalPulang_sampai = txtSearchTanggalPulangTo.Text,
                    QuickText = txtSearchView.Text,
                    QuickFilterExpression = hdnFilterExpressionQuickSearch.Value
                };
                AppSession.LastContentEKlaimList = lc;

                vConsultVisit9 entity = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0}", regBPJS.RegistrationID)).FirstOrDefault();
                RegisteredPatient pt = new RegisteredPatient();
                pt.MRN = entity.MRN;
                pt.MedicalNo = entity.MedicalNo;
                pt.GCGender = entity.GCGender;
                pt.GCSex = entity.GCSex;
                pt.DateOfBirth = entity.DateOfBirth;
                pt.RegistrationID = entity.RegistrationID;
                pt.RegistrationNo = entity.RegistrationNo;
                pt.RegistrationDate = entity.RegistrationDate;
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

                string url = string.Format("~/Program/BPJS/EKlaim/EKlaimEntry.aspx?id={0}", registrationID);
                Response.Redirect(url);
            }
        }

    }
}