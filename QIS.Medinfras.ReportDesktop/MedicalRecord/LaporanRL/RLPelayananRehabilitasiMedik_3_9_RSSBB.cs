using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class RLPelayananRehabilitasiMedik_3_9_RSSBB : BaseCustomDailyPotraitRpt
    {
        public RLPelayananRehabilitasiMedik_3_9_RSSBB()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            Healthcare entityHC = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];
            List<GetPatientPhysiotherapy> lstPhy = BusinessLayer.GetPatientPhysiotherapyList(Convert.ToInt32(param[0]), Convert.ToInt32(param[1]), Convert.ToInt32(param[2]));
            #region Medis
            #region Gait Analyzer
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^210") != null)
            {
                lblGaitAnalyzer.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^210")).Sum(a => a.Qty));
            }
            #endregion
            #region E M G
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^211") != null)
            {
                lblEMG.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^211")).Sum(a => a.Qty));
            }
            #endregion
            #region Uro Dinamic
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^212") != null)
            {
                lblUroDinamic.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^212")).Sum(a => a.Qty));
            }
            #endregion
            #region Side Back
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^213") != null)
            {
                lblSideBack.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^213")).Sum(a => a.Qty));
            }
            #endregion
            #region E N Tree
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^214") != null)
            {
                lblENTree.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^214")).Sum(a => a.Qty));
            }
            #endregion
            #region Spyrometer
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^215") != null)
            {
                lblSpyrometer.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^215")).Sum(a => a.Qty));
            }
            #endregion
            #region Static Bicycle
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^216") != null)
            {
                lblStaticBicycle.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^216")).Sum(a => a.Qty));
            }
            #endregion
            #region Tread Mill
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^217") != null)
            {
                lblTreadMill.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^217")).Sum(a => a.Qty));
            }
            #endregion
            #region Body Platysmograf
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^218") != null)
            {
                lblBodyPlatysmograf.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^218")).Sum(a => a.Qty));
            }
            #endregion
            #region Medis Lain-lain
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^274") != null)
            {
                lblMedisLL.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^274")).Sum(a => a.Qty));
            }
            #endregion
            #endregion

            #region Fisioterapi
            #region Latihan Fisik
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^219") != null)
            {
                lblLatihanFisik.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^219")).Sum(a => a.Qty));
            }
            #endregion
            #region Aktinoterapi
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^220") != null)
            {
                lblAktinoterapi.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^220")).Sum(a => a.Qty));
            }
            #endregion
            #region Elektroterapi
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^221") != null)
            {
                lblElektroterapi.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^221")).Sum(a => a.Qty));
            }
            #endregion
            #region Hidroterapi
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^222") != null)
            {
                lblHidroterapi.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^222")).Sum(a => a.Qty));
            }
            #endregion
            #region Traksi Lumbal & Cervical
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^223") != null)
            {
                lblTraksiLumbal.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^223")).Sum(a => a.Qty));
            }
            #endregion
            #region Fisioterapi Lain-lain
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^275") != null)
            {
                lblFisioLL.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^275")).Sum(a => a.Qty));
            }
            #endregion
            #endregion

            #region Okupasiterapi
            #region Snoosien Room
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^224") != null)
            {
                lblSnoosienRoom.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^224")).Sum(a => a.Qty));
            }
            #endregion
            #region Sensori Integrasi
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^225") != null)
            {
                lblSensoriIntegrasi.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^225")).Sum(a => a.Qty));
            }
            #endregion
            #region Latihan Aktivitas kehidupan sehari-hari
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^226") != null)
            {
                lblLatihanAktivitas.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^226")).Sum(a => a.Qty));
            }
            #endregion
            #region Proper Body Mekanik
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^227") != null)
            {
                lblProperBody.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^227")).Sum(a => a.Qty));
            }
            #endregion
            #region Pembuatan Alat Lontar & Adaptasi Alat
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^228") != null)
            {
                lblPembuatanAlat.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^228")).Sum(a => a.Qty));
            }
            #endregion
            #region Analisa Persiapan Kerja
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^229") != null)
            {
                lblAnalisaPK.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^229")).Sum(a => a.Qty));
            }
            #endregion
            #region Latihan Relaksasi
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^230") != null)
            {
                lblLatihanRelaksasi.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^230")).Sum(a => a.Qty));
            }
            #endregion
            #region Analisa & Intervensi, Persepsi, Kognitif, Psikomotor
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^231") != null)
            {
                lblAnalisaIntervensi.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^231")).Sum(a => a.Qty));
            }
            #endregion
            #region Okupasiterapi Lain-lain
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^278") != null)
            {
                lblOkupasiLL.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^278")).Sum(a => a.Qty));
            }
            #endregion
            #endregion

            #region Terapi Wicara
            #region Fungsi Bicara
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^232") != null)
            {
                lblFungsiBicara.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^232")).Sum(a => a.Qty));
            }
            #endregion
            #region Fungsi Bahasa / Laku
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^233") != null)
            {
                lblFungsiBahasa.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^233")).Sum(a => a.Qty));
            }
            #endregion
            #region Fungsi Menelan
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^234") != null)
            {
                lblFungsiMenelan.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^234")).Sum(a => a.Qty));
            }
            #endregion
            #region Terapi Wicara Lain-lain
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^277") != null)
            {
                lblTerapiLL.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^277")).Sum(a => a.Qty));
            }
            #endregion
            #endregion

            #region Psikologi
            #region Psikolog Anak
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^235") != null)
            {
                lblPsikologiAnak.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^235")).Sum(a => a.Qty));
            }
            #endregion
            #region Psikolog Dewasa
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^236") != null)
            {
                lblPsikologiDewasa.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^236")).Sum(a => a.Qty));
            }
            #endregion
            #region Psikologi Lain-lain
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^278") != null)
            {
                lblPsikologiLL.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^278")).Sum(a => a.Qty));
            }
            #endregion
            #endregion

            #region Sosial Medis
            #region Evaluasi Lingkungan Rumah
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^237") != null)
            {
                lblEvaluasiLingkungan.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^237")).Sum(a => a.Qty));
            }
            #endregion
            #region Evaluasi Ekonomi
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^238") != null)
            {
                lblEvaluasiEkonomi.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^238")).Sum(a => a.Qty));
            }
            #endregion
            #region Evaluasi Pekerjaan
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^239") != null)
            {
                lblEvaluasiPekerjaan.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^239")).Sum(a => a.Qty));
            }
            #endregion
            #region Sosial Medis Lain-lain
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^279") != null)
            {
                lblSosialLL.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^279")).Sum(a => a.Qty));
            }
            #endregion
            #endregion

            #region Ortotik Prostetik
            #region Pembuatan Alat Bantu
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^240") != null)
            {
                lblPembuatanAlatB.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^240")).Sum(a => a.Qty));
            }
            #endregion
            #region Pembuatan Alat Anggota Tiruan
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^241") != null)
            {
                lblPembuatanAlatA.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^241")).Sum(a => a.Qty));
            }
            #endregion
            #region Ortotik Prostetik Lain-lain
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^280") != null)
            {
                lblOrtotikLL.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^280")).Sum(a => a.Qty));
            }
            #endregion
            #endregion

            #region Kunjungan Rumah
            #region Kunjungan Rumah
            if (lstPhy.Where(a => a.GCRLReportGroup == "X378^281") != null)
            {
               lblKunjunganRumah.Text = string.Format("{0}", lstPhy.Where(a => a.GCRLReportGroup == ("X378^281")).Sum(a => a.Qty));
            }
            #endregion
            #endregion

            lblTotal.Text = string.Format("{0}", lstPhy.Sum(a => a.Qty));
            lblNamaRS.Text = entityHC.HealthcareName;
            base.InitializeReport(param);
        }
    }
}
