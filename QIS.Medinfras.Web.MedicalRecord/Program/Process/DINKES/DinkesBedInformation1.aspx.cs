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
    public partial class DinkesBedInformation1 : BasePageTrx
    {
        private static bool _isNewRecordInfoTT;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.BRIDGING_DINKES_TEMPAT_TIDUR;
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
        }

        private void LoadParameterValues()
        {
            DinkesInfoTT infoTT = BusinessLayer.GetDinkesInfoTT(AppSession.UserLogin.HealthcareID);
            if (infoTT == null)
            {               
                txtKapasitasVIP.Text = "0";
                txtKapasitasKelas1.Text = "0";
                txtKapasitasKelas2.Text = "0";
                txtKapasitasKelas3.Text = "0";
                txtKapasitasKelas1L.Text = "0";
                txtKapasitasKelas1P.Text = "0";
                txtKapasitasKelas2L.Text = "0";
                txtKapasitasKelas2P.Text = "0";
                txtKapasitasKelas3L.Text = "0";
                txtKapasitasKelas3P.Text = "0";
                txtKapasitasHCU.Text = "0";
                txtKapasitasICCU.Text = "0";
                txtKapasitasICUNegatifVentilator.Text = "0";
                txtKapasitasICUNegatifTanpaVentilator.Text = "0";
                txtKapasitasICUTanpaNegatifVentilator.Text = "0";
                txtKapasitasICUTanpaNegatifTanpaVentilator.Text = "0";
                txtKapasitasICUCovidNegatifVentilator.Text = "0";
                txtKapasitasICUCovidNegatifTanpaVentilator.Text = "0";
                txtKapasitasICUCovidTanpaNegatifVentilator.Text = "0";
                txtKapasitasICUCovidTanpaNegatifTanpaVentilator.Text = "0";
                txtKapasitasIsolasiNegatif.Text = "0";
                txtKapasitasIsolasiTanpaNegatif.Text = "0";
                txtKapasitasNICUCovid.Text = "0";
                txtKapasitasPerinaCovid.Text = "0";
                txtKapasitasPICUCovid.Text = "0";
                txtKapasitasOKCovid.Text = "0";
                txtKapasitasHDCovid.Text = "0";
                txtKosongVIP.Text = "0";
                txtKosongKelas1.Text = "0";
                txtKosongKelas2.Text = "0";
                txtKosongKelas3.Text = "0";
                txtKosongKelas1L.Text = "0";
                txtKosongKelas1P.Text = "0";
                txtKosongKelas2L.Text = "0";
                txtKosongKelas2P.Text = "0";
                txtKosongKelas3L.Text = "0";
                txtKosongKelas3P.Text = "0";
                txtKosongHCU.Text = "0";
                txtKosongICCU.Text = "0";
                txtKosongICUNegatifVentilator.Text = "0";
                txtKosongICUNegatifTanpaVentilator.Text = "0";
                txtKosongICUTanpaNegatifVentilator.Text = "0";
                txtKosongICUTanpaNegatifTanpaVentilator.Text = "0";
                txtKosongICUCovidNegatifVentilator.Text = "0";
                txtKosongICUCovidNegatifTanpaVentilator.Text = "0";
                txtKosongICUCovidTanpaNegatifVentilator.Text = "0";
                txtKosongICUCovidTanpaNegatifTanpaVentilator.Text = "0";
                txtKosongIsolasiNegatif.Text = "0";
                txtKosongIsolasiTanpaNegatif.Text = "0";
                txtKosongNICUCovid.Text = "0";
                txtKosongPerinaCovid.Text = "0";
                txtKosongPICUCovid.Text = "0";
                txtKosongOKCovid.Text = "0";
                txtKosongHDCovid.Text = "0";

                lblLastUpdatedDateTime.InnerText = string.Empty;
            }
            else
            {
                txtKapasitasVIP.Text = infoTT.kapasitas_vip.ToString("G29");
                txtKapasitasKelas1.Text = infoTT.kapasitas_kelas_1.ToString("G29");
                txtKapasitasKelas2.Text = infoTT.kapasitas_kelas_2.ToString("G29");
                txtKapasitasKelas3.Text = infoTT.kapasitas_kelas_3.ToString("G29");
                txtKapasitasKelas1L.Text = infoTT.kapasitas_kelas_1_l.ToString("G29");
                txtKapasitasKelas1P.Text = infoTT.kapasitas_kelas_1_p.ToString("G29");
                txtKapasitasKelas2L.Text = infoTT.kapasitas_kelas_2_l.ToString("G29");
                txtKapasitasKelas2P.Text = infoTT.kapasitas_kelas_2_p.ToString("G29"); ;
                txtKapasitasKelas3L.Text = infoTT.kapasitas_kelas_3_l.ToString("G29"); ;
                txtKapasitasKelas3P.Text = infoTT.kapasitas_kelas_3_p.ToString("G29"); ;
                txtKapasitasHCU.Text = infoTT.kapasitas_hcu.ToString("G29");
                txtKapasitasICCU.Text = infoTT.kapasitas_iccu.ToString("G29");
                txtKapasitasICUNegatifVentilator.Text = infoTT.kapasitas_icu_negatif_ventilator.ToString("G29");
                txtKapasitasICUNegatifTanpaVentilator.Text = infoTT.kapasitas_icu_negatif_tanpa_ventilator.ToString("G29");
                txtKapasitasICUTanpaNegatifVentilator.Text = infoTT.kapasitas_icu_tanpa_negatif_ventilator.ToString("G29");
                txtKapasitasICUTanpaNegatifTanpaVentilator.Text = infoTT.kapasitas_icu_tanpa_negatif_tanpa_ventilator.ToString("G29");
                txtKapasitasICUCovidNegatifVentilator.Text = infoTT.kapasitas_icu_covid_negatif_ventilator.ToString("G29");
                txtKapasitasICUCovidNegatifTanpaVentilator.Text = infoTT.kapasitas_icu_covid_negatif_tanpa_ventilator.ToString("G29");
                txtKapasitasICUCovidTanpaNegatifVentilator.Text = infoTT.kapasitas_icu_covid_tanpa_negatif_ventilator.ToString("G29");
                txtKapasitasICUCovidTanpaNegatifTanpaVentilator.Text = infoTT.kapasitas_icu_covid_tanpa_negatif_tanpa_ventilator.ToString("G29");
                txtKapasitasIsolasiNegatif.Text = infoTT.kapasitas_isolasi_negatif.ToString("G29");
                txtKapasitasIsolasiTanpaNegatif.Text = infoTT.kapasitas_isolasi_tanpa_negatif.ToString("G29");
                txtKapasitasNICUCovid.Text = infoTT.kapasitas_nicu_covid.ToString("G29");
                txtKapasitasPerinaCovid.Text = infoTT.kapasitas_perina_covid.ToString("G29");
                txtKapasitasPICUCovid.Text = infoTT.kapasitas_picu_covid.ToString("G29");
                txtKapasitasOKCovid.Text = infoTT.kapasitas_ok_covid.ToString("G29");
                txtKapasitasHDCovid.Text = infoTT.kapasitas_hd_covid.ToString("G29");
                txtKosongVIP.Text = infoTT.kosong_vip.ToString("G29");
                txtKosongKelas1.Text = infoTT.kosong_kelas_1.ToString("G29");
                txtKosongKelas2.Text = infoTT.kosong_kelas_2.ToString("G29");
                txtKosongKelas3.Text = infoTT.kosong_kelas_3.ToString("G29");
                txtKosongKelas1L.Text = infoTT.kosong_kelas_1_l.ToString("G29");
                txtKosongKelas1P.Text = infoTT.kosong_kelas_1_p.ToString("G29");
                txtKosongKelas2L.Text = infoTT.kosong_kelas_2_l.ToString("G29");
                txtKosongKelas2P.Text = infoTT.kosong_kelas_2_p.ToString("G29");
                txtKosongKelas3L.Text = infoTT.kosong_kelas_3_l.ToString("G29");
                txtKosongKelas3P.Text = infoTT.kosong_kelas_3_p.ToString("G29");
                txtKosongHCU.Text = infoTT.kosong_hcu.ToString("G29");
                txtKosongICCU.Text = infoTT.kosong_iccu.ToString("G29");
                txtKosongICUNegatifVentilator.Text = infoTT.kosong_icu_negatif_ventilator.ToString("G29");
                txtKosongICUNegatifTanpaVentilator.Text = infoTT.kosong_icu_negatif_tanpa_ventilator.ToString("G29");
                txtKosongICUTanpaNegatifVentilator.Text = infoTT.kosong_icu_tanpa_negatif_ventilator.ToString("G29");
                txtKosongICUTanpaNegatifTanpaVentilator.Text = infoTT.kosong_icu_tanpa_negatif_tanpa_ventilator.ToString("G29");
                txtKosongICUCovidNegatifVentilator.Text = infoTT.kosong_icu_covid_negatif_ventilator.ToString("G29");
                txtKosongICUCovidNegatifTanpaVentilator.Text = infoTT.kosong_icu_covid_negatif_tanpa_ventilator.ToString("G29");
                txtKosongICUCovidTanpaNegatifVentilator.Text = infoTT.kosong_icu_covid_tanpa_negatif_ventilator.ToString("G29");
                txtKosongICUCovidTanpaNegatifTanpaVentilator.Text = infoTT.kosong_icu_covid_negatif_tanpa_ventilator.ToString("G29");
                txtKosongIsolasiNegatif.Text = infoTT.kosong_isolasi_negatif.ToString("G29");
                txtKosongIsolasiTanpaNegatif.Text = infoTT.kosong_isolasi_tanpa_negatif.ToString("G29");
                txtKosongNICUCovid.Text = infoTT.kosong_nicu_covid.ToString("G29");
                txtKosongPerinaCovid.Text = infoTT.kosong_perina_covid.ToString("G29");
                txtKosongPICUCovid.Text = infoTT.kosong_picu_covid.ToString("G29");
                txtKosongOKCovid.Text = infoTT.kosong_ok_covid.ToString("G29");
                txtKosongHDCovid.Text = infoTT.kosong_hd_covid.ToString("G29");

                lblLastUpdatedDateTime.InnerText = string.Format("{0}", infoTT.updated_time);
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtKapasitasVIP, new ControlEntrySetting(true, true, true,"0"));
            SetControlEntrySetting(txtKapasitasKelas1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasKelas2, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasKelas3, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasKelas1L, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasKelas1P, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasKelas2L, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasKelas2P, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasKelas3L, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasKelas3P, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasKelas3P, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasHCU, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasICCU, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasICUNegatifVentilator, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasICUNegatifTanpaVentilator, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasICUTanpaNegatifVentilator, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasICUTanpaNegatifTanpaVentilator, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasICUCovidNegatifVentilator, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasICUCovidNegatifTanpaVentilator, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasICUCovidTanpaNegatifVentilator, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasICUCovidTanpaNegatifTanpaVentilator, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasIsolasiNegatif, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasIsolasiTanpaNegatif, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasNICUCovid, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasPerinaCovid, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasPICUCovid, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasOKCovid, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKapasitasHDCovid, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongVIP, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongKelas1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongKelas1L, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongKelas1P, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongKelas2, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongKelas2L, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongKelas2P, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongKelas3, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongKelas3L, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongKelas3P, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongHCU, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongICCU, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongICUNegatifVentilator, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongICUNegatifTanpaVentilator, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongICUTanpaNegatifVentilator, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongICUTanpaNegatifTanpaVentilator, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongICUCovidNegatifVentilator, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongICUCovidNegatifTanpaVentilator, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongICUCovidTanpaNegatifVentilator, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongICUCovidTanpaNegatifTanpaVentilator, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongIsolasiNegatif, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongIsolasiTanpaNegatif, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongNICUCovid, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongPerinaCovid, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongPICUCovid, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongOKCovid, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKosongHDCovid, new ControlEntrySetting(true, true, true));

            LoadParameterValues();
            IsLoadFirstRecord = true;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;

            if (type=="send")
            {
                if (!string.IsNullOrEmpty(AppSession.SA0111) && !string.IsNullOrEmpty(AppSession.SA0112) && !string.IsNullOrEmpty(AppSession.SA0113))
                {
                    string paramData = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}|{26}|{27}|{28}|{29}|{30}|{31}|{32}|{33}|{34}|{35}|{36}|{37}|{38}|{39}|{40}|{41}|{42}|{43}|{44}|{45}|{46}|{47}|{48}|{49}|{50}|{51}|{52}|{53}", 
                        txtKapasitasVIP.Text,
                        txtKapasitasKelas1.Text,
                        txtKapasitasKelas1L.Text,
                        txtKapasitasKelas1P.Text,
                        txtKapasitasKelas2.Text,
                        txtKapasitasKelas2L.Text,
                        txtKapasitasKelas2P.Text,
                        txtKapasitasKelas3.Text,
                        txtKapasitasKelas3L.Text,
                        txtKapasitasKelas3P.Text,
                        txtKapasitasHCU.Text,
                        txtKapasitasICCU.Text,
                        txtKapasitasICUNegatifVentilator.Text,
                        txtKapasitasICUNegatifTanpaVentilator.Text,
                        txtKapasitasICUTanpaNegatifVentilator.Text,
                        txtKapasitasICUTanpaNegatifTanpaVentilator.Text,
                        txtKapasitasICUCovidNegatifVentilator.Text,
                        txtKapasitasICUCovidNegatifTanpaVentilator.Text,
                        txtKapasitasICUCovidTanpaNegatifVentilator.Text,
                        txtKapasitasICUCovidTanpaNegatifTanpaVentilator.Text,
                        txtKapasitasIsolasiNegatif.Text,
                        txtKapasitasIsolasiTanpaNegatif.Text,
                        txtKapasitasNICUCovid.Text,
                        txtKapasitasPerinaCovid.Text,
                        txtKapasitasPICUCovid.Text,
                        txtKapasitasOKCovid.Text,
                        txtKapasitasHDCovid.Text,
                        txtKosongVIP.Text,
                        txtKosongKelas1.Text,
                        txtKosongKelas1L.Text,
                        txtKosongKelas1P.Text,
                        txtKosongKelas2.Text,
                        txtKosongKelas2L.Text,
                        txtKosongKelas2P.Text,
                        txtKosongKelas3.Text,
                        txtKosongKelas3L.Text,
                        txtKosongKelas3P.Text,
                        txtKosongHCU.Text,
                        txtKosongICCU.Text,
                        txtKosongICUNegatifVentilator.Text,
                        txtKosongICUNegatifTanpaVentilator.Text,
                        txtKosongICUTanpaNegatifVentilator.Text,
                        txtKosongICUTanpaNegatifTanpaVentilator.Text,
                        txtKosongICUCovidNegatifVentilator.Text,
                        txtKosongICUCovidNegatifTanpaVentilator.Text,
                        txtKosongICUCovidTanpaNegatifVentilator.Text,
                        txtKosongICUCovidTanpaNegatifTanpaVentilator.Text,
                        txtKosongIsolasiNegatif.Text,
                        txtKosongIsolasiTanpaNegatif.Text,
                        txtKosongNICUCovid.Text,
                        txtKosongPerinaCovid.Text,
                        txtKosongPICUCovid.Text,
                        txtKosongOKCovid.Text,
                        txtKosongHDCovid.Text
                        );

                    //IDbContext ctx = DbFactory.Configure(true);
                    
                    try
                    {
                        DinKesService oService = new DinKesService();

                        string apiResult = oService.SendInformasiTT(paramData);
                        if (!string.IsNullOrEmpty(apiResult))
                        {
                            string[] apiResultInfo = apiResult.Split('|');
                            if (apiResultInfo[0] == "1")
                            {
                                DinkesInfoTT infoTT = BusinessLayer.GetDinkesInfoTT(AppSession.UserLogin.HealthcareID);
                                bool isAdd = infoTT == null;

                                if (infoTT == null)
                                    infoTT = new DinkesInfoTT();

                                infoTT.HealthcareID = AppSession.UserLogin.HealthcareID;
                                infoTT.kapasitas_vip = Convert.ToInt16(txtKapasitasVIP.Text);
                                infoTT.kapasitas_kelas_1 = Convert.ToInt16(txtKapasitasKelas1.Text);
                                infoTT.kapasitas_kelas_2 = Convert.ToInt16(txtKapasitasKelas2.Text);
                                infoTT.kapasitas_kelas_3 = Convert.ToInt16(txtKapasitasKelas3.Text);
                                infoTT.kapasitas_kelas_1_l = Convert.ToInt16(txtKapasitasKelas1L.Text);
                                infoTT.kapasitas_kelas_1_p = Convert.ToInt16(txtKapasitasKelas1P.Text);
                                infoTT.kapasitas_kelas_2_l = Convert.ToInt16(txtKapasitasKelas2L.Text);
                                infoTT.kapasitas_kelas_2_p = Convert.ToInt16(txtKapasitasKelas2P.Text);
                                infoTT.kapasitas_kelas_3_l = Convert.ToInt16(txtKapasitasKelas3L.Text);
                                infoTT.kapasitas_kelas_3_p = Convert.ToInt16(txtKapasitasKelas3P.Text);
                                infoTT.kapasitas_hcu = Convert.ToInt16(txtKapasitasHCU.Text);
                                infoTT.kapasitas_iccu = Convert.ToInt16(txtKapasitasICCU.Text);
                                infoTT.kapasitas_icu_negatif_ventilator = Convert.ToInt16(txtKapasitasICUNegatifVentilator.Text);
                                infoTT.kapasitas_icu_negatif_tanpa_ventilator = Convert.ToInt16(txtKapasitasICUNegatifTanpaVentilator.Text);
                                infoTT.kapasitas_icu_tanpa_negatif_ventilator = Convert.ToInt16(txtKapasitasICUTanpaNegatifVentilator.Text);
                                infoTT.kapasitas_icu_tanpa_negatif_tanpa_ventilator = Convert.ToInt16(txtKapasitasICUTanpaNegatifTanpaVentilator.Text);
                                infoTT.kapasitas_icu_covid_negatif_ventilator = Convert.ToInt16(txtKapasitasICUCovidNegatifVentilator.Text);
                                infoTT.kapasitas_icu_covid_negatif_tanpa_ventilator = Convert.ToInt16(txtKapasitasICUCovidNegatifTanpaVentilator.Text);
                                infoTT.kapasitas_icu_covid_tanpa_negatif_ventilator = Convert.ToInt16(txtKapasitasICUCovidTanpaNegatifVentilator.Text);
                                infoTT.kapasitas_icu_covid_tanpa_negatif_tanpa_ventilator = Convert.ToInt16(txtKapasitasICUCovidTanpaNegatifTanpaVentilator.Text);
                                infoTT.kapasitas_isolasi_negatif = Convert.ToInt16(txtKapasitasIsolasiNegatif.Text);
                                infoTT.kapasitas_isolasi_tanpa_negatif = Convert.ToInt16(txtKapasitasIsolasiTanpaNegatif.Text);
                                infoTT.kapasitas_nicu_covid = Convert.ToInt16(txtKapasitasNICUCovid.Text);
                                infoTT.kapasitas_perina_covid = Convert.ToInt16(txtKapasitasPerinaCovid.Text);
                                infoTT.kapasitas_picu_covid = Convert.ToInt16(txtKapasitasPICUCovid.Text);
                                infoTT.kapasitas_ok_covid = Convert.ToInt16(txtKapasitasOKCovid.Text);
                                infoTT.kapasitas_hd_covid = Convert.ToInt16(txtKapasitasHDCovid.Text);
                                infoTT.kosong_vip = Convert.ToInt16(txtKosongVIP.Text);
                                infoTT.kosong_kelas_1 = Convert.ToInt16(txtKosongKelas1.Text);
                                infoTT.kosong_kelas_2 = Convert.ToInt16(txtKosongKelas2.Text);
                                infoTT.kosong_kelas_3 = Convert.ToInt16(txtKosongKelas3.Text);
                                infoTT.kosong_kelas_1_l = Convert.ToInt16(txtKosongKelas1L.Text);
                                infoTT.kosong_kelas_1_p = Convert.ToInt16(txtKosongKelas1P.Text);
                                infoTT.kosong_kelas_2_l = Convert.ToInt16(txtKosongKelas2L.Text);
                                infoTT.kosong_kelas_2_p = Convert.ToInt16(txtKosongKelas2P.Text);
                                infoTT.kosong_kelas_3_l = Convert.ToInt16(txtKosongKelas3L.Text);
                                infoTT.kosong_kelas_3_p = Convert.ToInt16(txtKosongKelas3P.Text);
                                infoTT.kosong_hcu = Convert.ToInt16(txtKosongHCU.Text);
                                infoTT.kosong_iccu = Convert.ToInt16(txtKosongICCU.Text);
                                infoTT.kosong_icu_negatif_ventilator = Convert.ToInt16(txtKosongICUNegatifVentilator.Text);
                                infoTT.kosong_icu_negatif_tanpa_ventilator = Convert.ToInt16(txtKosongICUNegatifTanpaVentilator.Text);
                                infoTT.kosong_icu_tanpa_negatif_ventilator = Convert.ToInt16(txtKosongICUTanpaNegatifVentilator.Text);
                                infoTT.kosong_icu_tanpa_negatif_tanpa_ventilator = Convert.ToInt16(txtKosongICUTanpaNegatifTanpaVentilator.Text);
                                infoTT.kosong_icu_covid_negatif_ventilator = Convert.ToInt16(txtKosongICUCovidNegatifVentilator.Text);
                                infoTT.kosong_icu_covid_negatif_tanpa_ventilator = Convert.ToInt16(txtKosongICUCovidNegatifTanpaVentilator.Text);
                                infoTT.kosong_icu_covid_tanpa_negatif_ventilator = Convert.ToInt16(txtKosongICUCovidTanpaNegatifVentilator.Text);
                                infoTT.kosong_icu_covid_negatif_tanpa_ventilator = Convert.ToInt16(txtKosongICUCovidTanpaNegatifTanpaVentilator.Text);
                                infoTT.kosong_isolasi_negatif = Convert.ToInt16(txtKosongIsolasiNegatif.Text);
                                infoTT.kosong_isolasi_tanpa_negatif = Convert.ToInt16(txtKosongIsolasiTanpaNegatif.Text);
                                infoTT.kosong_nicu_covid = Convert.ToInt16(txtKosongNICUCovid.Text);
                                infoTT.kosong_perina_covid = Convert.ToInt16(txtKosongPerinaCovid.Text);
                                infoTT.kosong_picu_covid = Convert.ToInt16(txtKosongPICUCovid.Text);
                                infoTT.kosong_ok_covid = Convert.ToInt16(txtKosongOKCovid.Text);
                                infoTT.kosong_hd_covid = Convert.ToInt16(txtKosongHDCovid.Text);
                                infoTT.updated_time = DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT_2);
                                infoTT.LastUpdatedBy = AppSession.UserLogin.UserID;
                                lblLastUpdatedDateTime.InnerText = infoTT.updated_time;

                                if (isAdd)
                                    BusinessLayer.InsertDinkesInfoTT(infoTT);
                                else
                                    BusinessLayer.UpdateDinkesInfoTT(infoTT);

                                result = true;
                                //ctx.CommitTransaction();
                            }
                            else
                            {
                                result = false;
                                errMessage = apiResultInfo[2];
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = "Proses Pengiriman Data Informasi tempat tidur gagal dilakukan. (Response Kosong)";
                        }
                    }
                    catch (Exception ex)
                    {
                        //ctx.RollBackTransaction();
                        errMessage = ex.Message;
                        result = false;
                    }
                    finally
                    {
                        //ctx.Close();
                    }
                }
                else
                {
                    errMessage = "Parameter System yang diperlukan untuk proses Bridging belum dikonfigurasi!";
                    result = false;
                } 
            }
            return result;    
        }
    }
}