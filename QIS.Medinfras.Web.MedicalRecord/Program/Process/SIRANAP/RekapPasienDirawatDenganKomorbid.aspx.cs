using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class RekapPasienDirawatDenganKomorbid : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.BRIDGING_SIRANAP_REKAPPASIENDIRAWATDENGANKOMORBID;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        private GetUserMenuAccess menu;
        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            txtObservasiDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        private void LoadParameterValues()
        {
            String observasiDate = Helper.GetDatePickerValue(txtObservasiDate).ToString(Constant.FormatString.DATE_FORMAT_112);

            GetRekapPasienDirawatdenganKomorbidSiranap entity = BusinessLayer.GetRekapPasienDirawatdenganKomorbidSiranapList(observasiDate).FirstOrDefault();
            if (entity == null)
            {
                txtICUdenganVentilatorSuspectL.Text = "0";
                txtICUdenganVentilatorSuspectP.Text = "0";
                txtICUdenganVentilatorConfirmL.Text = "0";
                txtICUdenganVentilatorConfirmP.Text = "0";
                txtICUtanpaVentilatorSuspectL.Text = "0";
                txtICUtanpaVentilatorSuspectP.Text = "0";
                txtICUtanpaVentilatorConfirmL.Text = "0";
                txtICUtanpaVentilatorConfirmP.Text = "0";
                txtICUTekananNegatifdenganVentilatorSuspectL.Text = "0";
                txtICUTekananNegatifdenganVentilatorSuspectP.Text = "0";
                txtICUTekananNegatifdenganVentilatorConfirmL.Text = "0";
                txtICUTekananNegatifdenganVentilatorConfirmP.Text = "0";
                txtICUTekananNegatiftanpaVentilatorSuspectL.Text = "0";
                txtICUTekananNegatiftanpaVentilatorSuspectP.Text = "0";
                txtICUTekananNegatiftanpaVentilatorConfirmL.Text = "0";
                txtICUTekananNegatiftanpaVentilatorConfirmP.Text = "0";
                txtIsolasiTekananNegatifSuspectL.Text = "0";
                txtIsolasiTekananNegatifSuspectP.Text = "0";
                txtIsolasiTekananNegatifConfirmL.Text = "0";
                txtIsolasitanpaTekananNegatifSuspectL.Text = "0";
                txtIsolasitanpaTekananNegatifSuspectP.Text = "0";
                txtIsolasitanpaTekananNegatifConfirmL.Text = "0";
                txtIsolasitanpaTekananNegatifConfirmP.Text = "0";
                txtNICUKhususCovidSuspectL.Text = "0";
                txtNICUKhususCovidSuspectP.Text = "0";
                txtNICUKhususCovidConfirmL.Text = "0";
                txtNICUKhususCovidConfirmP.Text = "0";
                txtPICUKhususCovidSuspectL.Text = "0";
                txtPICUKhususCovidSuspectP.Text = "0";
                txtPICUKhususCovidConfirmL.Text = "0";
                txtPICUKhususCovidConfirmP.Text = "0";

            }
            else
            {
                txtICUdenganVentilatorSuspectL.Text = entity.icu_dengan_ventilator_suspect_l.ToString("G29");
                txtICUdenganVentilatorSuspectP.Text = entity.icu_dengan_ventilator_suspect_p.ToString("G29");
                txtICUdenganVentilatorConfirmL.Text = entity.icu_dengan_ventilator_confirm_l.ToString("G29");
                txtICUdenganVentilatorConfirmP.Text = entity.icu_dengan_ventilator_confirm_p.ToString("G29");
                txtICUtanpaVentilatorSuspectL.Text = entity.icu_tanpa_ventilator_suspect_l.ToString("G29");
                txtICUtanpaVentilatorSuspectP.Text = entity.icu_tanpa_ventilator_suspect_p.ToString("G29");
                txtICUtanpaVentilatorConfirmL.Text = entity.icu_tanpa_ventilator_confirm_l.ToString("G29");
                txtICUtanpaVentilatorConfirmP.Text = entity.icu_tanpa_ventilator_confirm_p.ToString("G29");
                txtICUTekananNegatifdenganVentilatorSuspectL.Text = entity.icu_tekanan_negatif_dengan_ventilator_suspect_l.ToString("G29");
                txtICUTekananNegatifdenganVentilatorSuspectP.Text = entity.icu_tekanan_negatif_dengan_ventilator_suspect_p.ToString("G29");
                txtICUTekananNegatifdenganVentilatorConfirmL.Text = entity.icu_tekanan_negatif_dengan_ventilator_confirm_l.ToString("G29");
                txtICUTekananNegatifdenganVentilatorConfirmP.Text = entity.icu_tekanan_negatif_dengan_ventilator_confirm_p.ToString("G29");
                txtICUTekananNegatiftanpaVentilatorSuspectL.Text = entity.icu_tekanan_negatif_tanpa_ventilator_suspect_l.ToString("G29");
                txtICUTekananNegatiftanpaVentilatorSuspectP.Text = entity.icu_tekanan_negatif_tanpa_ventilator_suspect_p.ToString("G29");
                txtICUTekananNegatiftanpaVentilatorConfirmL.Text = entity.icu_tekanan_negatif_tanpa_ventilator_confirm_l.ToString("G29");
                txtICUTekananNegatiftanpaVentilatorConfirmP.Text = entity.icu_tekanan_negatif_tanpa_ventilator_confirm_p.ToString("G29");
                txtIsolasiTekananNegatifSuspectL.Text = entity.isolasi_tekanan_negatif_suspect_l.ToString("G29");
                txtIsolasiTekananNegatifSuspectP.Text = entity.isolasi_tekanan_negatif_suspect_p.ToString("G29");
                txtIsolasiTekananNegatifConfirmL.Text = entity.isolasi_tekanan_negatif_confirm_l.ToString("G29");
                txtIsolasitanpaTekananNegatifSuspectL.Text = entity.isolasi_tanpa_tekanan_negatif_suspect_l.ToString("G29");
                txtIsolasitanpaTekananNegatifSuspectP.Text = entity.isolasi_tanpa_tekanan_negatif_suspect_p.ToString("G29");
                txtIsolasitanpaTekananNegatifConfirmL.Text = entity.isolasi_tanpa_tekanan_negatif_confirm_l.ToString("G29");
                txtIsolasitanpaTekananNegatifConfirmP.Text = entity.isolasi_tanpa_tekanan_negatif_confirm_p.ToString("G29");
                txtNICUKhususCovidSuspectL.Text = entity.nicu_khusus_covid_suspect_l.ToString("G29");
                txtNICUKhususCovidSuspectP.Text = entity.nicu_khusus_covid_suspect_p.ToString("G29");
                txtNICUKhususCovidConfirmL.Text = entity.nicu_khusus_covid_confirm_l.ToString("G29");
                txtNICUKhususCovidConfirmP.Text = entity.nicu_khusus_covid_confirm_p.ToString("G29");
                txtPICUKhususCovidSuspectL.Text = entity.picu_khusus_covid_suspect_l.ToString("G29");
                txtPICUKhususCovidSuspectP.Text = entity.picu_khusus_covid_suspect_p.ToString("G29");
                txtPICUKhususCovidConfirmL.Text = entity.picu_khusus_covid_confirm_l.ToString("G29");
                txtPICUKhususCovidConfirmP.Text = entity.picu_khusus_covid_confirm_p.ToString("G29");
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtICUdenganVentilatorSuspectL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtICUdenganVentilatorSuspectP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtICUdenganVentilatorConfirmL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtICUdenganVentilatorConfirmP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtICUtanpaVentilatorSuspectL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtICUtanpaVentilatorSuspectP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtICUtanpaVentilatorConfirmL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtICUtanpaVentilatorConfirmP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtICUTekananNegatifdenganVentilatorSuspectL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtICUTekananNegatifdenganVentilatorSuspectP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtICUTekananNegatifdenganVentilatorConfirmL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtICUTekananNegatifdenganVentilatorConfirmP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtICUTekananNegatiftanpaVentilatorSuspectL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtICUTekananNegatiftanpaVentilatorSuspectP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtICUTekananNegatiftanpaVentilatorConfirmL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtICUTekananNegatiftanpaVentilatorConfirmP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtIsolasiTekananNegatifSuspectL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtIsolasiTekananNegatifSuspectP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtIsolasiTekananNegatifConfirmL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtIsolasitanpaTekananNegatifSuspectL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtIsolasitanpaTekananNegatifSuspectP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtIsolasitanpaTekananNegatifConfirmL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtIsolasitanpaTekananNegatifConfirmP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtNICUKhususCovidSuspectL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtNICUKhususCovidSuspectP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtNICUKhususCovidConfirmL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtNICUKhususCovidConfirmP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtPICUKhususCovidSuspectL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtPICUKhususCovidSuspectP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtPICUKhususCovidConfirmL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtPICUKhususCovidConfirmP, new ControlEntrySetting(true, true, true, "0"));

            LoadParameterValues();
            IsLoadFirstRecord = true;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;

            if (type == "refresh")
            {
                #region refresh
                String observasiDate = Helper.GetDatePickerValue(txtObservasiDate).ToString(Constant.FormatString.DATE_FORMAT_112);

                GetRekapPasienDirawatdenganKomorbidSiranap entity = BusinessLayer.GetRekapPasienDirawatdenganKomorbidSiranapList(observasiDate).FirstOrDefault();
                if (entity == null)
                {
                    txtICUdenganVentilatorSuspectL.Text = "0";
                    txtICUdenganVentilatorSuspectP.Text = "0";
                    txtICUdenganVentilatorConfirmL.Text = "0";
                    txtICUdenganVentilatorConfirmP.Text = "0";
                    txtICUtanpaVentilatorSuspectL.Text = "0";
                    txtICUtanpaVentilatorSuspectP.Text = "0";
                    txtICUtanpaVentilatorConfirmL.Text = "0";
                    txtICUtanpaVentilatorConfirmP.Text = "0";
                    txtICUTekananNegatifdenganVentilatorSuspectL.Text = "0";
                    txtICUTekananNegatifdenganVentilatorSuspectP.Text = "0";
                    txtICUTekananNegatifdenganVentilatorConfirmL.Text = "0";
                    txtICUTekananNegatifdenganVentilatorConfirmP.Text = "0";
                    txtICUTekananNegatiftanpaVentilatorSuspectL.Text = "0";
                    txtICUTekananNegatiftanpaVentilatorSuspectP.Text = "0";
                    txtICUTekananNegatiftanpaVentilatorConfirmL.Text = "0";
                    txtICUTekananNegatiftanpaVentilatorConfirmP.Text = "0";
                    txtIsolasiTekananNegatifSuspectL.Text = "0";
                    txtIsolasiTekananNegatifSuspectP.Text = "0";
                    txtIsolasiTekananNegatifConfirmL.Text = "0";
                    txtIsolasitanpaTekananNegatifSuspectL.Text = "0";
                    txtIsolasitanpaTekananNegatifSuspectP.Text = "0";
                    txtIsolasitanpaTekananNegatifConfirmL.Text = "0";
                    txtIsolasitanpaTekananNegatifConfirmP.Text = "0";
                    txtNICUKhususCovidSuspectL.Text = "0";
                    txtNICUKhususCovidSuspectP.Text = "0";
                    txtNICUKhususCovidConfirmL.Text = "0";
                    txtNICUKhususCovidConfirmP.Text = "0";
                    txtPICUKhususCovidSuspectL.Text = "0";
                    txtPICUKhususCovidSuspectP.Text = "0";
                    txtPICUKhususCovidConfirmL.Text = "0";
                    txtPICUKhususCovidConfirmP.Text = "0";
                }
                else
                {
                    txtICUdenganVentilatorSuspectL.Text = entity.icu_dengan_ventilator_suspect_l.ToString("G29");
                    txtICUdenganVentilatorSuspectP.Text = entity.icu_dengan_ventilator_suspect_p.ToString("G29");
                    txtICUdenganVentilatorConfirmL.Text = entity.icu_dengan_ventilator_confirm_l.ToString("G29");
                    txtICUdenganVentilatorConfirmP.Text = entity.icu_dengan_ventilator_confirm_p.ToString("G29");
                    txtICUtanpaVentilatorSuspectL.Text = entity.icu_tanpa_ventilator_suspect_l.ToString("G29");
                    txtICUtanpaVentilatorSuspectP.Text = entity.icu_tanpa_ventilator_suspect_p.ToString("G29");
                    txtICUtanpaVentilatorConfirmL.Text = entity.icu_tanpa_ventilator_confirm_l.ToString("G29");
                    txtICUtanpaVentilatorConfirmP.Text = entity.icu_tanpa_ventilator_confirm_p.ToString("G29");
                    txtICUTekananNegatifdenganVentilatorSuspectL.Text = entity.icu_tekanan_negatif_dengan_ventilator_suspect_l.ToString("G29");
                    txtICUTekananNegatifdenganVentilatorSuspectP.Text = entity.icu_tekanan_negatif_dengan_ventilator_suspect_p.ToString("G29");
                    txtICUTekananNegatifdenganVentilatorConfirmL.Text = entity.icu_tekanan_negatif_dengan_ventilator_confirm_l.ToString("G29");
                    txtICUTekananNegatifdenganVentilatorConfirmP.Text = entity.icu_tekanan_negatif_dengan_ventilator_confirm_p.ToString("G29");
                    txtICUTekananNegatiftanpaVentilatorSuspectL.Text = entity.icu_tekanan_negatif_tanpa_ventilator_suspect_l.ToString("G29");
                    txtICUTekananNegatiftanpaVentilatorSuspectP.Text = entity.icu_tekanan_negatif_tanpa_ventilator_suspect_p.ToString("G29");
                    txtICUTekananNegatiftanpaVentilatorConfirmL.Text = entity.icu_tekanan_negatif_tanpa_ventilator_confirm_l.ToString("G29");
                    txtICUTekananNegatiftanpaVentilatorConfirmP.Text = entity.icu_tekanan_negatif_tanpa_ventilator_confirm_p.ToString("G29");
                    txtIsolasiTekananNegatifSuspectL.Text = entity.isolasi_tekanan_negatif_suspect_l.ToString("G29");
                    txtIsolasiTekananNegatifSuspectP.Text = entity.isolasi_tekanan_negatif_suspect_p.ToString("G29");
                    txtIsolasiTekananNegatifConfirmL.Text = entity.isolasi_tekanan_negatif_confirm_l.ToString("G29");
                    txtIsolasitanpaTekananNegatifSuspectL.Text = entity.isolasi_tanpa_tekanan_negatif_suspect_l.ToString("G29");
                    txtIsolasitanpaTekananNegatifSuspectP.Text = entity.isolasi_tanpa_tekanan_negatif_suspect_p.ToString("G29");
                    txtIsolasitanpaTekananNegatifConfirmL.Text = entity.isolasi_tanpa_tekanan_negatif_confirm_l.ToString("G29");
                    txtIsolasitanpaTekananNegatifConfirmP.Text = entity.isolasi_tanpa_tekanan_negatif_confirm_p.ToString("G29");
                    txtNICUKhususCovidSuspectL.Text = entity.nicu_khusus_covid_suspect_l.ToString("G29");
                    txtNICUKhususCovidSuspectP.Text = entity.nicu_khusus_covid_suspect_p.ToString("G29");
                    txtNICUKhususCovidConfirmL.Text = entity.nicu_khusus_covid_confirm_l.ToString("G29");
                    txtNICUKhususCovidConfirmP.Text = entity.nicu_khusus_covid_confirm_p.ToString("G29");
                    txtPICUKhususCovidSuspectL.Text = entity.picu_khusus_covid_suspect_l.ToString("G29");
                    txtPICUKhususCovidSuspectP.Text = entity.picu_khusus_covid_suspect_p.ToString("G29");
                    txtPICUKhususCovidConfirmL.Text = entity.picu_khusus_covid_confirm_l.ToString("G29");
                    txtPICUKhususCovidConfirmP.Text = entity.picu_khusus_covid_confirm_p.ToString("G29");
                }
                #endregion
            }
            return result;
        }
    }
}