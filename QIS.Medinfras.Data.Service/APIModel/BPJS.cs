using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Data.Service
{
    public class BPJSTxt
    {
        public string KodeRS { get; set; }
        public string Kelas { get; set; }
        public string NoRM { get; set; }
        public string NoSEP { get; set; }
        public string KelasRawat { get; set; }
        public string TariffRS { get; set; }
        public string JenisRawat { get; set; }
        public string TglMasuk { get; set; }
        public string TglKeluar { get; set; }
        public string LOS { get; set; }
        public string TglLahir { get; set; }
        public string UmurThn { get; set; }
        public string UmurHari { get; set; }
        public string JK { get; set; }
        public string CaraPulang { get; set; }
        public string Berat { get; set; }
        public string DiagnosaUtama { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
        public string D3 { get; set; }
        public string D4 { get; set; }
        public string D5 { get; set; }
        public string D6 { get; set; }
        public string D7 { get; set; }
        public string D8 { get; set; }
        public string D9 { get; set; }
        public string D10 { get; set; }
        public string D11 { get; set; }
        public string D12 { get; set; }
        public string D13 { get; set; }
        public string D14 { get; set; }
        public string D15 { get; set; }
        public string D16 { get; set; }
        public string D17 { get; set; }
        public string D18 { get; set; }
        public string D19 { get; set; }
        public string D20 { get; set; }
        public string D21 { get; set; }
        public string D22 { get; set; }
        public string D23 { get; set; }
        public string D24 { get; set; }
        public string D25 { get; set; }
        public string D26 { get; set; }
        public string D27 { get; set; }
        public string D28 { get; set; }
        public string D29 { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string P3 { get; set; }
        public string P4 { get; set; }
        public string P5 { get; set; }
        public string P6 { get; set; }
        public string P7 { get; set; }
        public string P8 { get; set; }
        public string P9 { get; set; }
        public string P10 { get; set; }
        public string P11 { get; set; }
        public string P12 { get; set; }
        public string P13 { get; set; }
        public string P14 { get; set; }
        public string P15 { get; set; }
        public string P16 { get; set; }
        public string P17 { get; set; }
        public string P18 { get; set; }
        public string P19 { get; set; }
        public string P20 { get; set; }
        public string P21 { get; set; }
        public string P22 { get; set; }
        public string P23 { get; set; }
        public string P24 { get; set; }
        public string P25 { get; set; }
        public string P26 { get; set; }
        public string P27 { get; set; }
        public string P28 { get; set; }
        public string P29 { get; set; }
        public string P30 { get; set; }
        public string ADL { get; set; }
        public string RecordID { get; set; }
        public string INACBG { get; set; }
        public string Deskripsi { get; set; }
        public string Tarif { get; set; }
        public string SA { get; set; }
        public string TarifSA { get; set; }
        public string SP { get; set; }
        public string DescSP { get; set; }
        public string TarifSP { get; set; }
        public string SR { get; set; }
        public string DescSR { get; set; }
        public string TarifSR { get; set; }
        public string SI { get; set; }
        public string DescSI { get; set; }
        public string TarifSI { get; set; }
        public string SD { get; set; }
        public string DescSD { get; set; }
        public string TarifSD { get; set; }
        public string TotalTarif { get; set; }
        public string NamaPasien { get; set; }
        public string DPJP { get; set; }
        public string SEP { get; set; }
        public string Rujukan { get; set; }
        public string PengesahanSL3 { get; set; }
        public string VersiINACBG { get; set; }
        public string C1 { get; set; }
        public string C2 { get; set; }
        public string C3 { get; set; }
    }

    public class RujukanDTO
    {
        public string tglKunjungan { get; set; }
        public string noKunjungan { get; set; }
        public string noKartu { get; set; }
        public string namaPeserta { get; set; }
        public string kodePPK { get; set; }
        public string namaPPK { get; set; }
        public string kodeKelas { get; set; }
        public string namaKelas { get; set; }
        public string kodeJenisPeserta { get; set; }
        public string namaJenisPeserta { get; set; }
        public string nik { get; set; }
        public string tglLahir { get; set; }
        public string kodeSex { get; set; }
        public string kodeStatusPeserta { get; set; }
        public string namaStatusPeserta { get; set; }
        public string kodeDiagnosa { get; set; }
        public string namaDiagnosa { get; set; }
        public string kodePerujuk { get; set; }
        public string namaPerujuk { get; set; }
        public string kodePoli { get; set; }
        public string namaPoli { get; set; }
        public string kodePelayanan { get; set; }
        public string namaPelayanan { get; set; }
        public string keluhan { get; set; }

        public string cfPoli
        {
            get { return string.Format("{0} - {1}", kodePoli, namaPoli); }
        }
        public string cfDiagnosa
        {
            get { return string.Format("{0} - {1}", kodeDiagnosa, namaDiagnosa); }
        }
    }

    public class HistoryKunjungan
    {
        public string diagnosa { get; set; }
        public string jnsPelayanan { get; set; }
        public string kelasRawat { get; set; }
        public string namaPeserta { get; set; }
        public string noKartu { get; set; }
        public string noSep { get; set; }
        public string noRujukan { get; set; }
        public string poli { get; set; }
        public string ppkPelayanan { get; set; }
        public string tglPlgSep { get; set; }
        public string tglSep { get; set; }
    }
}
