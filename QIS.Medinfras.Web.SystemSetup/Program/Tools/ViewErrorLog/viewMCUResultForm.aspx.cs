using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class viewMCUResultForm : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                 
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            string value = txtResult.Text;
            ///"1^Penyakit Yang pernah diderita=Tidak|3^Pernah Dirawat Di Rumah Sakit=Tidak|5^Berapa lama=-|6^Pernah Operasi Apa=Tidak|8^Pernah Kecelakaan=Tidak|9^Kecelakaan dimana=Tidak Ada|11^Merokok=Tidak|13^Alkohol=Tidak|15^Kopi=Tidak|17^Olah Raga=Tidak|19^Hypertensi=Tidak|21^Diabetes=Tidak|23^Sakit Jantung=Tidak|25^Ginjal=Tidak|27^Ganguan Mental=Tidak|29^Lain2/ALergi=Tidak|31^Apakah Sedang Menderita Penyakit=Tidak|33^Apakah Sedang Menjalani Pengobatan=Tidak|39^Status Gizi=Normal|41^Kulit=Normal|43^Rambut=Normal|45^Visus Kanan=Abnormal|46^Visus Kiri=Abnormal|59^Konjungtiva=Tidak Hiperemis|60^Skelera=Normal|61^Pupil=Isokor|62^Buta Warna=Total|63^Bola Mata=Simetris|64^Cornea=Keruh|72^Hidung=Normal|73^Lidah=Normal|74^Gigi Atas=Baik|78^Gigi Bawah=Baik|82^Pharing=Normal|83^Pharing Abnormal=-|84^Tonsil=Tidak Hypertropi|86^Tiroid=Normal|87^Tiroid Abnormal=-|88^Frekuensi Pernapasan=Normal|90^Paru paru=Normal|100^Vesikuer=Normal|105^Telinga Kanan - Telinga Luar=Serumen Propt (-)|106^Telinga Luar - Membran Tympani=Utuh|107^Telinga Luar - Membran Timpany tidak Utuh=-|108^Telinga Kiri - Telinga Luar=Serumen Propt (-)|109^Telinga Kiri - Membran Tympani=Utuh|110^Telinga Kiri - Membran Timpany tidak Utuh=-|113^Irama=Reguler|115^Hasil Tensi=Normal|116^Iktrus Kordis=Tidak Teraba|117^Auskultasi=BJ Murni|118^Kesan Batas Jantung=Melebar|119^Inspeksi=Normal|120^Nyeri Tekan=Ada|121^Nyeri Lepas=Ada|122^Hati=Tidak Teraba|123^Limpa=Tidak Teraba|124^Hernia=Tidak|125^Rectal Touche=Tidak Dilakukan|126^null=-|127^Ginjal=Normal|128^Ballotement=Normal|129^Nyeri Ketok Kanan=Negatif|130^Nyeri Ketok Kiri=Negatif|131^Genital=1|132^Refleks Fisiologis=Positif|133^Refleks Patologis=Negatif|134^Fungsi Motorik=Normal|135^Fungsi Sensorik=Normal|136^Tonus Otot=Eutoni|137^Ket Abnormal Tonus Otot=Ischialgia|140^Tulang Belakang=Normal|142^Anggota Gerak Atas=Normal|144^Anggota Gerak Atas=Normal|146^Leher=Tidak Membesar|147^Axila=Tidak Membesar|148^Inguinal=Tidak Membesar|151^Radiologi=Tidak Dilakukan|153^EKG=Tidak Dilakukan|159^Penyakit akibat kerja=Tidak|163^Hasil Hipertensi=Tidak|164^Hasil Hipertensi Derajat=-|165^Penyakit Diabetes=-|2^Penyakit apa dan sejak kapan=|4^Pernah Dirawat Penyakit apa dan sejak kapan=|7^Operasi Apa dan kapan=|10^Kecelakan Apa dan Kapan=|12^Jumlah Rokok(Batang/hari)=0|14^Alkohol(kali/Minggu)=0|16^Minum Kopi (Gelas/hari)=0|18^Olahraga(Jam/Satu Minggu)=0|20^Klg yg Hypertensi=|22^Klg yg Diabetes=|24^Klg yg Sakit Jantung=|26^Klg yg Sakit Ginjal=|28^Klg yg Sakit Ganguan Jiwa=|30^Klg yg Sakit Lain2/ALergi=|32^Jika Ya, Penyakit Apa=|34^Sudah Berapa Lama=|35^Pengobatan Apa Dan Apakah Terkontrol=|36^Tinggi Badan=180|37^Berat Badan=80|38^Anjuran Berat Badan=80|40^Body Mass Index=25.00|42^null=|44^keterangan Abnormal=|47^Visus + Koreksi Kanan=|48^Visus + Koreksi Kiri=|49^Add Visus Kanan=|50^Add Visus Kiri=|65^Lain-lain=|66^AutoRef Kanan=|67^AutoRef Kiri=|68^NCT / Tonometri Kanan=|69^NCT / Tonometri Kiri=|70^Kacamata Kanan=|71^Kacamata Kanan=|75^Gigi Atas - Keterangan=|79^Gigi Bawah - Keterangan=|85^Keterangan Tonsil=|89^Nilai Pernapasan=|111^Tekanan Darah (mm/Hg)=|112^Frequensi Nadi=|114^Irama Kardiovaskular=|139^Ket Abnormal Tengkorak=|141^Ket Abnormal Tengkorak=|143^Ket Abnormal Anggota Gerak Atas=|145^Ket Abnormal Anggota Gerak Atas=|149^Anamnesa=|150^Pemeriksaan Fisik=|152^Radiologi Abnormal=|154^Elektro Kardiografi Description=|155^Audiometri=|156^Laboratorium=|157^Lain-lain=|158^Saran=|160^Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|161^Kesimpulan=|166^Penyakit Diabetes Keterangan=|167^Spirometri=|168^Pap Smear=|169^Nama Perusahaan 1=|170^Nama Perusahaan 2=|171^Jenis Pekerjaan 1=|172^Jenis Pekerjaan 2=|173^Faktor Fisika 1=|174^Faktor Fisika 2=|175^Faktor Kimia 1=|176^Faktor Kimia 1=|177^Faktor Biologi 1=|178^Faktor Biologi 2=|179^Faktor Psikologi 1=|180^Faktor Psikologi 2=|181^Faktor Ergonomi 1=|182^Faktor Ergonomi 2=|183^Lama tahun bekerja 1=0|184^Lama Bulan bekerja 1=0|185^Lama tahun bekerja 2=0|186^Lama bulan bekerja 2=0|51^Mata Kanan - Myopia=Ya|52^Mata Kanan - Hypermetrop=Ya|53^Mata Kanan - Presbiop=Ya|54^Mata Kanan - Cilindris=Ya|55^Mata Kiri - Myopia=|56^Mata Kiri - Hypermetrop=|57^Mata Kiri - Presbiop=|58^Mata Kiri - Cilindris=|76^Gigi Atas - Radix=Ya|76_01^Gigi Atas - Caries=|76_02^Gigi Atas - Missing=|77^Gigi Atas - Abrasi=|77_01^Gigi Atas - Impacted=Ya|77_02^Gigi Atas - Kalkulus=Ya|80^Gigi Bawah - Radix=|80_01^Gigi Bawah - Caries=|80_02^Gigi Bawah - Missing=|81^Gigi Bawah - Abrasi=Ya|80_02^Gigi Bawah - Impacted=|80_03^Gigi Bawah - Kalkulus=|187^Fisika Pencahayaan=|188^Fisika Bising=|189^Fisika Getaran=|190^Fisika Suhu=|191^Kimia Partike (Debu/Asap)=|192^Kimia Cairan=|193^Kimia Gas/Uap=|194^Biologi Bakteri=|195^Biologi Virus=|196^Biologi Jamur=|197^Ergonomi Gerak Repetitif/ Berulang=|198^Ergonomi Angkat Beban Berat=|199^Ergonomi Akward Posture=|200^Ergonomi Mata Lelah=|201^Psikologi Stress=|202^Psikologi Shift=|101^Ronchi=|101^Ronchi=Ya|101^Ronchi=|101^Ronchi=|102^Wheezing=|102^Wheezing=|102^Wheezing=|102^Wheezing=|103^Perkusi Kanan=Ya|103^Perkusi Kanan=|103^Perkusi Kanan=|104^Perkusi Kiri=Ya|104^Perkusi Kiri=|104^Perkusi Kiri=|162^Kriteria Hiperkes=|162^Kriteria Hiperkes=Ya|162^Kriteria Hiperkes=|162^Kriteria Hiperkes=|162^Kriteria Hiperkes=|162^Kriteria Hiperkes=";
            string[] data = value.Split('|');
            if (data.Length > 0)
            {
                MCUFormResultFieldReporting oData = new MCUFormResultFieldReporting();
                oData.TransactionNo = string.Empty;
                oData.RegistrasiNo = string.Empty;
                oData.TanggalPemeriksaan = oData.TglMasuk = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_1);

                oData.TglLahir = string.Empty;

                oData.JnsKelamin = oData.TglLahir = string.Empty;
                oData.Lokasi = oData.TglLahir = string.Empty;
                oData.NamaPegawai = oData.TglLahir = string.Empty;
                oData.NoPegawai = oData.TglLahir = string.Empty;
                oData.ParamedicCode = oData.TglLahir = string.Empty;
                oData.ParamedicName = oData.TglLahir = string.Empty;
                oData.Penjamin = oData.TglLahir = string.Empty;
                oData.Posisi = oData.TglLahir = string.Empty;

                for (int i = 0; i < data.Length; i++)
                {

                    string[] field = data[i].Split('^');
                    if (field.Length > 1)
                    {
                        string[] dataField = field[1].Split('=');
                        string dataFieldValue = dataField[1];
                        if (dataField.Length > 2)
                        {
                            for (int indexField = 0; indexField < dataField.Length; indexField++)
                            {
                                if (i > 0)
                                {
                                    dataFieldValue += string.Format("{0}=", dataField[indexField]);
                                }
                            }
                            if (!string.IsNullOrEmpty(dataFieldValue))
                            {
                                dataFieldValue = dataFieldValue.Remove(dataFieldValue.Length - 1);
                            }
                        }

                        #region filter
                        switch (field[0])
                        {
                            case "1":
                                oData.Col1 = dataFieldValue;
                                break;
                            case "2":
                                oData.Col2 = dataFieldValue;
                                break;
                            case "3":
                                oData.Col3 = dataFieldValue;
                                break;
                            case "4":
                                oData.Col4 = dataFieldValue;
                                break;
                            case "5":
                                oData.Col5 = dataFieldValue;
                                break;
                            case "6":
                                oData.Col6 = dataFieldValue;
                                break;
                            case "7":
                                oData.Col7 = dataFieldValue;
                                break;
                            case "8":
                                oData.Col8 = dataFieldValue;
                                break;
                            case "9":
                                oData.Col9 = dataFieldValue;
                                break;
                            case "10":
                                oData.Col10 = dataFieldValue;
                                break;
                            case "11":
                                oData.Col11 = dataFieldValue;
                                break;
                            case "12":
                                oData.Col12 = dataFieldValue;
                                break;
                            case "13":
                                oData.Col13 = dataFieldValue;
                                break;
                            case "14":
                                oData.Col14 = dataFieldValue;
                                break;
                            case "15":
                                oData.Col15 = dataFieldValue;
                                break;
                            case "16":
                                oData.Col16 = dataFieldValue;
                                break;
                            case "17":
                                oData.Col17 = dataFieldValue;
                                break;
                            case "18":
                                oData.Col18 = dataFieldValue;
                                break;
                            case "19":
                                oData.Col19 = dataFieldValue;
                                break;
                            case "20":
                                oData.Col20 = dataFieldValue;
                                break;
                            case "21":
                                oData.Col21 = dataFieldValue;
                                break;
                            case "22":
                                oData.Col22 = dataFieldValue;
                                break;
                            case "23":
                                oData.Col23 = dataFieldValue;
                                break;
                            case "24":
                                oData.Col24 = dataFieldValue;
                                break;
                            case "25":
                                oData.Col25 = dataFieldValue;
                                break;
                            case "26":
                                oData.Col26 = dataFieldValue;
                                break;
                            case "27":
                                oData.Col27 = dataFieldValue;
                                break;
                            case "28":
                                oData.Col28 = dataFieldValue;
                                break;
                            case "29":
                                oData.Col29 = dataFieldValue;
                                break;
                            case "30":
                                oData.Col30 = dataFieldValue;
                                break;
                            case "31":
                                oData.Col31 = dataFieldValue;
                                break;
                            case "32":
                                oData.Col32 = dataFieldValue;
                                break;
                            case "33":
                                oData.Col33 = dataFieldValue;
                                break;
                            case "34":
                                oData.Col34 = dataFieldValue;
                                break;
                            case "35":
                                oData.Col35 = dataFieldValue;
                                break;
                            case "36":
                                oData.Col36 = dataFieldValue;
                                break;
                            case "37":
                                oData.Col37 = dataFieldValue;
                                break;
                            case "38":
                                oData.Col38 = dataFieldValue;
                                break;
                            case "39":
                                oData.Col39 = dataFieldValue;
                                break;
                            case "40":
                                oData.Col40 = dataFieldValue;
                                break;
                            case "41":
                                oData.Col41 = dataFieldValue;
                                break;
                            case "42":
                                oData.Col42 = dataFieldValue;
                                break;
                            case "43":
                                oData.Col43 = dataFieldValue;
                                break;
                            case "44":
                                oData.Col44 = dataFieldValue;
                                break;
                            case "45":
                                oData.Col45 = dataFieldValue;
                                break;
                            case "46":
                                oData.Col46 = dataFieldValue;
                                break;
                            case "47":
                                oData.Col47 = dataFieldValue;
                                break;
                            case "48":
                                oData.Col48 = dataFieldValue;
                                break;
                            case "49":
                                oData.Col49 = dataFieldValue;
                                break;
                            case "50":
                                oData.Col50 = dataFieldValue;
                                break;
                            case "51":
                                oData.Col51 = dataFieldValue;
                                break;
                            case "52":
                                oData.Col52 = dataFieldValue;
                                break;
                            case "53":
                                oData.Col53 = dataFieldValue;
                                break;
                            case "54":
                                oData.Col54 = dataFieldValue;
                                break;
                            case "55":
                                oData.Col55 = dataFieldValue;
                                break;
                            case "56":
                                oData.Col56 = dataFieldValue;
                                break;
                            case "57":
                                oData.Col57 = dataFieldValue;
                                break;
                            case "58":
                                oData.Col58 = dataFieldValue;
                                break;
                            case "59":
                                oData.Col59 = dataFieldValue;
                                break;
                            case "60":
                                oData.Col60 = dataFieldValue;
                                break;
                            case "61":
                                oData.Col61 = dataFieldValue;
                                break;
                            case "62":
                                oData.Col62 = dataFieldValue;
                                break;
                            case "63":
                                oData.Col63 = dataFieldValue;
                                break;
                            case "64":
                                oData.Col64 = dataFieldValue;
                                break;
                            case "65":
                                oData.Col65 = dataFieldValue;
                                break;
                            case "66":
                                oData.Col66 = dataFieldValue;
                                break;
                            case "67":
                                oData.Col67 = dataFieldValue;
                                break;
                            case "68":
                                oData.Col68 = dataFieldValue;
                                break;
                            case "69":
                                oData.Col69 = dataFieldValue;
                                break;
                            case "70":
                                oData.Col70 = dataFieldValue;
                                break;
                            case "71":
                                oData.Col71 = dataFieldValue;
                                break;
                            case "72":
                                oData.Col72 = dataFieldValue;
                                break;
                            case "73":
                                oData.Col73 = dataFieldValue;
                                break;
                            case "74":
                                oData.Col74 = dataFieldValue;
                                break;
                            case "75":
                                oData.Col75 = dataFieldValue;
                                break;
                            case "76":
                                oData.Col76 = dataFieldValue;
                                break;
                            case "77":
                                oData.Col77 = dataFieldValue;
                                break;
                            case "78":
                                oData.Col78 = dataFieldValue;
                                break;
                            case "79":
                                oData.Col79 = dataFieldValue;
                                break;
                            case "80":
                                oData.Col80 = dataFieldValue;
                                break;
                            case "81":
                                oData.Col81 = dataFieldValue;
                                break;
                            case "82":
                                oData.Col82 = dataFieldValue;
                                break;
                            case "83":
                                oData.Col83 = dataFieldValue;
                                break;
                            case "84":
                                oData.Col84 = dataFieldValue;
                                break;
                            case "85":
                                oData.Col85 = dataFieldValue;
                                break;
                            case "86":
                                oData.Col86 = dataFieldValue;
                                break;
                            case "87":
                                oData.Col87 = dataFieldValue;
                                break;
                            case "88":
                                oData.Col88 = dataFieldValue;
                                break;
                            case "89":
                                oData.Col89 = dataFieldValue;
                                break;
                            case "90":
                                oData.Col90 = dataFieldValue;
                                break;
                            case "91":
                                oData.Col91 = dataFieldValue;
                                break;
                            case "92":
                                oData.Col92 = dataFieldValue;
                                break;
                            case "93":
                                oData.Col93 = dataFieldValue;
                                break;
                            case "94":
                                oData.Col94 = dataFieldValue;
                                break;
                            case "95":
                                oData.Col95 = dataFieldValue;
                                break;
                            case "96":
                                oData.Col96 = dataFieldValue;
                                break;
                            case "97":
                                oData.Col97 = dataFieldValue;
                                break;
                            case "98":
                                oData.Col98 = dataFieldValue;
                                break;
                            case "99":
                                oData.Col99 = dataFieldValue;
                                break;
                            case "100":
                                oData.Col100 = dataFieldValue;
                                break;
                            case "101":
                                oData.Col101 = dataFieldValue;
                                break;
                            case "102":
                                oData.Col102 = dataFieldValue;
                                break;
                            case "103":
                                oData.Col103 = dataFieldValue;
                                break;
                            case "104":
                                oData.Col104 = dataFieldValue;
                                break;
                            case "105":
                                oData.Col105 = dataFieldValue;
                                break;
                            case "106":
                                oData.Col106 = dataFieldValue;
                                break;
                            case "107":
                                oData.Col107 = dataFieldValue;
                                break;
                            case "108":
                                oData.Col108 = dataFieldValue;
                                break;
                            case "109":
                                oData.Col109 = dataFieldValue;
                                break;
                            case "110":
                                oData.Col110 = dataFieldValue;
                                break;
                            case "111":
                                oData.Col111 = dataFieldValue;
                                break;
                            case "112":
                                oData.Col112 = dataFieldValue;
                                break;
                            case "113":
                                oData.Col113 = dataFieldValue;
                                break;
                            case "114":
                                oData.Col114 = dataFieldValue;
                                break;
                            case "115":
                                oData.Col115 = dataFieldValue;
                                break;
                            case "116":
                                oData.Col116 = dataFieldValue;
                                break;
                            case "117":
                                oData.Col117 = dataFieldValue;
                                break;
                            case "118":
                                oData.Col118 = dataFieldValue;
                                break;
                            case "119":
                                oData.Col119 = dataFieldValue;
                                break;
                            case "120":
                                oData.Col120 = dataFieldValue;
                                break;
                            case "121":
                                oData.Col121 = dataFieldValue;
                                break;
                            case "122":
                                oData.Col122 = dataFieldValue;
                                break;
                            case "123":
                                oData.Col123 = dataFieldValue;
                                break;
                            case "124":
                                oData.Col124 = dataFieldValue;
                                break;
                            case "125":
                                oData.Col125 = dataFieldValue;
                                break;
                            case "126":
                                oData.Col126 = dataFieldValue;
                                break;
                            case "127":
                                oData.Col127 = dataFieldValue;
                                break;
                            case "128":
                                oData.Col128 = dataFieldValue;
                                break;
                            case "129":
                                oData.Col129 = dataFieldValue;
                                break;
                            case "130":
                                oData.Col130 = dataFieldValue;
                                break;
                            case "131":
                                oData.Col131 = dataFieldValue;
                                break;
                            case "132":
                                oData.Col132 = dataFieldValue;
                                break;
                            case "133":
                                oData.Col133 = dataFieldValue;
                                break;
                            case "134":
                                oData.Col134 = dataFieldValue;
                                break;
                            case "135":
                                oData.Col135 = dataFieldValue;
                                break;
                            case "136":
                                oData.Col136 = dataFieldValue;
                                break;
                            case "137":
                                oData.Col137 = dataFieldValue;
                                break;
                            case "138":
                                oData.Col138 = dataFieldValue;
                                break;
                            case "139":
                                oData.Col139 = dataFieldValue;
                                break;
                            case "140":
                                oData.Col140 = dataFieldValue;
                                break;
                            case "141":
                                oData.Col141 = dataFieldValue;
                                break;
                            case "142":
                                oData.Col142 = dataFieldValue;
                                break;
                            case "143":
                                oData.Col143 = dataFieldValue;
                                break;
                            case "144":
                                oData.Col144 = dataFieldValue;
                                break;
                            case "145":
                                oData.Col145 = dataFieldValue;
                                break;
                            case "146":
                                oData.Col146 = dataFieldValue;
                                break;
                            case "147":
                                oData.Col147 = dataFieldValue;
                                break;
                            case "148":
                                oData.Col148 = dataFieldValue;
                                break;
                            case "149":
                                oData.Col149 = dataFieldValue;
                                break;
                            case "150":
                                oData.Col150 = dataFieldValue;
                                break;
                            case "151":
                                oData.Col151 = dataFieldValue;
                                break;
                            case "152":
                                oData.Col152 = dataFieldValue;
                                break;
                            case "153":
                                oData.Col153 = dataFieldValue;
                                break;
                            case "154":
                                oData.Col154 = dataFieldValue;
                                break;
                            case "155":
                                oData.Col155 = dataFieldValue;
                                break;
                            case "156":
                                oData.Col156 = dataFieldValue;
                                break;
                            case "157":
                                oData.Col157 = dataFieldValue;
                                break;
                            case "158":
                                oData.Col158 = dataFieldValue;
                                break;
                            case "159":
                                oData.Col159 = dataFieldValue;
                                break;
                            case "160":
                                oData.Col160 = dataFieldValue;
                                break;
                            case "161":
                                oData.Col161 = dataFieldValue;
                                break;
                            case "162":
                                oData.Col162 = dataFieldValue;
                                break;
                            case "163":
                                oData.Col163 = dataFieldValue;
                                break;
                            case "164":
                                oData.Col164 = dataFieldValue;
                                break;
                            case "165":
                                oData.Col165 = dataFieldValue;
                                break;
                            case "166":
                                oData.Col166 = dataFieldValue;
                                break;
                            case "167":
                                oData.Col168 = dataFieldValue;
                                break;
                            case "168":
                                oData.Col168 = dataFieldValue;
                                break;
                            case "169":
                                oData.Col169 = dataFieldValue;
                                break;
                            case "170":
                                oData.Col170 = dataFieldValue;
                                break;
                            case "171":
                                oData.Col171 = dataFieldValue;
                                break;
                            case "172":
                                oData.Col172 = dataFieldValue;
                                break;
                            case "173":
                                oData.Col173 = dataFieldValue;
                                break;
                            case "174":
                                oData.Col174 = dataFieldValue;
                                break;
                            case "175":
                                oData.Col175 = dataFieldValue;
                                break;
                            case "176":
                                oData.Col176 = dataFieldValue;
                                break;
                            case "177":
                                oData.Col177 = dataFieldValue;
                                break;
                            case "178":
                                oData.Col178 = dataFieldValue;
                                break;
                            case "179":
                                oData.Col179 = dataFieldValue;
                                break;
                            case "180":
                                oData.Col180 = dataFieldValue;
                                break;
                            case "181":
                                oData.Col181 = dataFieldValue;
                                break;
                            case "182":
                                oData.Col182 = dataFieldValue;
                                break;
                            case "183":
                                oData.Col183 = dataFieldValue;
                                break;
                            case "184":
                                oData.Col184 = dataFieldValue;
                                break;
                            case "185":
                                oData.Col185 = dataFieldValue;
                                break;
                            case "186":
                                oData.Col186 = dataFieldValue;
                                break;
                            case "187":
                                oData.Col187 = dataFieldValue;
                                break;
                            case "188":
                                oData.Col188 = dataFieldValue;
                                break;
                            case "189":
                                oData.Col189 = dataFieldValue;
                                break;
                            case "190":
                                oData.Col190 = dataFieldValue;
                                break;
                            case "191":
                                oData.Col191 = dataFieldValue;
                                break;
                            case "192":
                                oData.Col192 = dataFieldValue;
                                break;
                            case "193":
                                oData.Col193 = dataFieldValue;
                                break;
                            case "194":
                                oData.Col194 = dataFieldValue;
                                break;
                            case "195":
                                oData.Col195 = dataFieldValue;
                                break;
                            case "196":
                                oData.Col196 = dataFieldValue;
                                break;
                            case "197":
                                oData.Col197 = dataFieldValue;
                                break;
                            case "198":
                                oData.Col198 = dataFieldValue;
                                break;
                            case "199":
                                oData.Col199 = dataFieldValue;
                                break;
                            case "200":
                                oData.Col200 = dataFieldValue;
                                break;
                            case "201":
                                oData.Col201 = dataFieldValue;
                                break;
                            case "202":
                                oData.Col202 = dataFieldValue;
                                break;
                            case "203":
                                oData.Col203 = dataFieldValue;
                                break;
                            case "204":
                                oData.Col204 = dataFieldValue;
                                break;
                            case "205":
                                oData.Col205 = dataFieldValue;
                                break;
                            case "206":
                                oData.Col206 = dataFieldValue;
                                break;
                            case "207":
                                oData.Col207 = dataFieldValue;
                                break;
                            case "208":
                                oData.Col208 = dataFieldValue;
                                break;
                            case "209":
                                oData.Col209 = dataFieldValue;
                                break;
                            case "210":
                                oData.Col210 = dataFieldValue;
                                break;
                            case "211":
                                oData.Col211 = dataFieldValue;
                                break;
                            case "212":
                                oData.Col212 = dataFieldValue;
                                break;
                            case "213":
                                oData.Col213 = dataFieldValue;
                                break;
                            case "214":
                                oData.Col214 = dataFieldValue;
                                break;
                            case "215":
                                oData.Col215 = dataFieldValue;
                                break;
                        }
                        #endregion
                    }
                }

                txtParsing.Text = JsonConvert.SerializeObject(oData);
            }
        }

        protected void btnGenerateView_Click(object sender, EventArgs e)
        {
             
        }
    }
}