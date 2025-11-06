using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BBuktiKasdanBankDraftRSFM : BaseCustomDailyPotraitRpt
    {
        public BBuktiKasdanBankDraftRSFM()
        {
            InitializeComponent();
        }

        private void lblJudul_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String TransactionCode = GetCurrentColumnValue("TransactionCode").ToString();
            String GCTreasuryType = GetCurrentColumnValue("GCTreasuryType").ToString();

            if (TransactionCode == "7282" || TransactionCode == "7283")
            {
                if (GCTreasuryType == Constant.TreasuryType.PENERIMAAN)
                {
                    lblJudul.Text = "KAS PENERIMAAN";

                    lblDirektur1.Visible = false;
                    lblJabatanDirektur1.Visible = false;

                    lblKabagKeu1.Visible = false;
                    lblJabatanKabagKeu1.Visible = false;

                    lblBendahara1.Visible = false;
                    lblJabatanBendahara1.Visible = false;

                    lblDirekturPT.Visible = false;
                    lblJabatanDirekturPT.Visible = false;

                    lblDirektur.Visible = false;
                    lblJabatanDirektur.Visible = false;

                    lblKabagKeu.Visible = false;
                    lblJabatanKabagKeu.Visible = false;

                    lblDisetujui3.Visible = false;
                    lblJabatanDisetujui3.Visible = false;
                }
                else if (GCTreasuryType == Constant.TreasuryType.PENGELUARAN)
                {
                    string filterExpressionApprover = string.Format("ParameterCode IN ('{0}', '{1}', '{2}', '{3}')", Constant.SettingParameter.DIREKTUR_YANMED, Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR, Constant.SettingParameter.MANAGER_KEUANGAN,
                        Constant.SettingParameter.FN_BAGIAN_KEUANGAN);

                    List<SettingParameter> lstSettingParameter1 = BusinessLayer.GetSettingParameterList(filterExpressionApprover);

                    SettingParameter direktur = lstSettingParameter1.Where(p => p.ParameterCode == Constant.SettingParameter.DIREKTUR_YANMED).FirstOrDefault();
                    SettingParameter jabatanDirektur = lstSettingParameter1.Where(p => p.ParameterCode == Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR).FirstOrDefault();

                    SettingParameter kepalaKeuanganAkuntansi = lstSettingParameter1.Where(p => p.ParameterCode == Constant.SettingParameter.MANAGER_KEUANGAN).FirstOrDefault();
                    SettingParameter jabatanKepalaKeuanganAkuntansi = lstSettingParameter1.Where(p => p.ParameterCode == Constant.SettingParameter.MANAGER_KEUANGAN).FirstOrDefault();

                    SettingParameter bendahara = lstSettingParameter1.Where(p => p.ParameterCode == Constant.SettingParameter.FN_BAGIAN_KEUANGAN).FirstOrDefault();
                    SettingParameter jabatanBendahara = lstSettingParameter1.Where(p => p.ParameterCode == Constant.SettingParameter.FN_BAGIAN_KEUANGAN).FirstOrDefault();

                    lblJudul.Text = "BUKTI PENGELUARAN KAS";

                    lblDirektur1.Text = direktur.ParameterValue;
                    lblJabatanDirektur1.Text = jabatanDirektur.ParameterValue;

                    lblKabagKeu1.Text = kepalaKeuanganAkuntansi.ParameterValue;
                    lblJabatanKabagKeu1.Text = kepalaKeuanganAkuntansi.Notes;

                    lblBendahara1.Text = bendahara.ParameterValue;
                    lblJabatanBendahara1.Text = bendahara.Notes;

                    lblDirekturPT.Visible = false;
                    lblJabatanDirekturPT.Visible = false;

                    lblDirektur.Visible = false;
                    lblJabatanDirektur.Visible = false;

                    lblKabagKeu.Visible = false;
                    lblJabatanKabagKeu.Visible = false;

                    lblDisetujui3.Visible = false;
                    lblJabatanDisetujui3.Visible = false;
                }
                else if (GCTreasuryType == Constant.TreasuryType.PINDAH_BUKU)
                {
                    lblJudul.Text = "KAS PINDAH BUKU";

                    lblDirektur1.Visible = false;
                    lblJabatanDirektur1.Visible = false;

                    lblKabagKeu1.Visible = false;
                    lblJabatanKabagKeu1.Visible = false;

                    lblBendahara1.Visible = false;
                    lblJabatanBendahara1.Visible = false;

                    lblDirekturPT.Visible = false;
                    lblJabatanDirekturPT.Visible = false;

                    lblDirektur.Visible = false;
                    lblJabatanDirektur.Visible = false;

                    lblKabagKeu.Visible = false;
                    lblJabatanKabagKeu.Visible = false;

                    lblDisetujui3.Visible = false;
                    lblJabatanDisetujui3.Visible = false;
                }
                else
                {
                    lblJudul.Text = "";
                }
            }
            else if (TransactionCode == "7284" || TransactionCode == "7285")
            {
                if (GCTreasuryType == Constant.TreasuryType.PENERIMAAN)
                {
                    lblJudul.Text = "BANK PENERIMAAN";

                    lblDirektur1.Visible = false;
                    lblJabatanDirektur1.Visible = false;

                    lblKabagKeu1.Visible = false;
                    lblJabatanKabagKeu1.Visible = false;

                    lblBendahara1.Visible = false;
                    lblJabatanBendahara1.Visible = false;

                    lblDirekturPT.Visible = false;
                    lblJabatanDirekturPT.Visible = false;

                    lblDirektur.Visible = false;
                    lblJabatanDirektur.Visible = false;

                    lblKabagKeu.Visible = false;
                    lblJabatanKabagKeu.Visible = false;

                    lblDisetujui3.Visible = false;
                    lblJabatanDisetujui3.Visible = false;
                }
                else if (GCTreasuryType == Constant.TreasuryType.PENGELUARAN)
                {
                    string filterExpressionApprover = string.Format("ParameterCode IN ('{0}', '{1}', '{2}', '{3}', '{4}')", Constant.SettingParameter.DIREKTUR_YANMED, Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR, Constant.SettingParameter.MANAGER_KEUANGAN,
                        Constant.SettingParameter.FN_BAGIAN_KEUANGAN, Constant.SettingParameter.PRESIDEN_DIREKTUR);

                    List<SettingParameter> lstSettingParameter1 = BusinessLayer.GetSettingParameterList(filterExpressionApprover);

                    SettingParameter direkturPT = lstSettingParameter1.Where(p => p.ParameterCode == Constant.SettingParameter.PRESIDEN_DIREKTUR).FirstOrDefault();
                    SettingParameter jabatanDirekturPT = lstSettingParameter1.Where(p => p.ParameterCode == Constant.SettingParameter.PRESIDEN_DIREKTUR).FirstOrDefault();

                    SettingParameter direktur = lstSettingParameter1.Where(p => p.ParameterCode == Constant.SettingParameter.DIREKTUR_YANMED).FirstOrDefault();
                    SettingParameter jabatanDirektur = lstSettingParameter1.Where(p => p.ParameterCode == Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR).FirstOrDefault();

                    SettingParameter kepalaKeuanganAkuntansi = lstSettingParameter1.Where(p => p.ParameterCode == Constant.SettingParameter.MANAGER_KEUANGAN).FirstOrDefault();
                    SettingParameter jabatanKepalaKeuanganAkuntansi = lstSettingParameter1.Where(p => p.ParameterCode == Constant.SettingParameter.MANAGER_KEUANGAN).FirstOrDefault();

                    SettingParameter kepalaPenunjangMedis = lstSettingParameter1.Where(p => p.ParameterCode == Constant.SettingParameter.FN_BAGIAN_KEUANGAN).FirstOrDefault();
                    SettingParameter jabatanKepalaPenunjangMedis = lstSettingParameter1.Where(p => p.ParameterCode == Constant.SettingParameter.FN_BAGIAN_KEUANGAN).FirstOrDefault();


                    lblJudul.Text = "BUKTI PENGELUARAN BANK";

                    lblDirekturPT.Text = direkturPT.ParameterValue;
                    lblJabatanDirekturPT.Text = jabatanDirekturPT.Notes;

                    lblDirektur.Text = direktur.ParameterValue;
                    lblJabatanDirektur.Text = jabatanDirektur.ParameterValue;

                    lblKabagKeu.Text = kepalaKeuanganAkuntansi.ParameterValue;
                    lblJabatanKabagKeu.Text = jabatanKepalaKeuanganAkuntansi.Notes;

                    lblDisetujui3.Text = kepalaPenunjangMedis.ParameterValue;
                    lblJabatanDisetujui3.Text = jabatanKepalaPenunjangMedis.Notes;

                    lblDirektur1.Visible = false;
                    lblJabatanDirektur1.Visible = false;

                    lblKabagKeu1.Visible = false;
                    lblJabatanKabagKeu1.Visible = false;

                    lblBendahara1.Visible = false;
                    lblJabatanBendahara1.Visible = false;
                }
                else if (GCTreasuryType == Constant.TreasuryType.PINDAH_BUKU)
                {
                    lblJudul.Text = "BANK PINDAH BUKU";

                    lblDirektur1.Visible = false;
                    lblJabatanDirektur1.Visible = false;

                    lblKabagKeu1.Visible = false;
                    lblJabatanKabagKeu1.Visible = false;

                    lblBendahara1.Visible = false;
                    lblJabatanBendahara1.Visible = false;

                    lblDirekturPT.Visible = false;
                    lblJabatanDirekturPT.Visible = false;

                    lblDirektur.Visible = false;
                    lblJabatanDirektur.Visible = false;

                    lblKabagKeu.Visible = false;
                    lblJabatanKabagKeu.Visible = false;

                    lblDisetujui3.Visible = false;
                    lblJabatanDisetujui3.Visible = false;
                }
                else
                {
                    lblJudul.Text = "";

                    lblDirektur1.Visible = false;
                    lblJabatanDirektur1.Visible = false;

                    lblKabagKeu1.Visible = false;
                    lblJabatanKabagKeu1.Visible = false;

                    lblBendahara1.Visible = false;
                    lblJabatanBendahara1.Visible = false;

                    lblDirekturPT.Visible = false;
                    lblJabatanDirekturPT.Visible = false;

                    lblDirektur.Visible = false;
                    lblJabatanDirektur.Visible = false;

                    lblKabagKeu.Visible = false;
                    lblJabatanKabagKeu.Visible = false;

                    lblDisetujui3.Visible = false;
                    lblJabatanDisetujui3.Visible = false;
                }
            }
            else
            {
                lblJudul.Text = "";

                lblDirektur1.Visible = false;
                lblJabatanDirektur1.Visible = false;

                lblKabagKeu1.Visible = false;
                lblJabatanKabagKeu1.Visible = false;

                lblBendahara1.Visible = false;
                lblJabatanBendahara1.Visible = false;

                lblDirekturPT.Visible = false;
                lblJabatanDirekturPT.Visible = false;

                lblDirektur.Visible = false;
                lblJabatanDirektur.Visible = false;

                lblKabagKeu.Visible = false;
                lblJabatanKabagKeu.Visible = false;

                lblDisetujui3.Visible = false;
                lblJabatanDisetujui3.Visible = false;
            }

            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')",
            Constant.SettingParameter.SA_VERIFICATION_TREASURY, Constant.SettingParameter.SA_APPROVAL_TREASURY));
        }

    }
}
