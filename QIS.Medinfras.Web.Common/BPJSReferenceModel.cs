using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Common
{
    public class BPJSReferenceModel
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class BPJSReferenceModel_MedinfrasAPI
    {
        public string Type { get; set; }
        public string Parameter { get; set; }
    }

    #region 01. Diagnosa

    public class ReferenceDiagnoseResponse
    {
        public BPJSReferenceModel metadata { get; set; }
        public ReferenceDiagnoseResponseData response { get; set; }
    }

    public class ReferenceDiagnoseResponseData
    {
        public ReferenceDiagnoseResponseDataDetail[] diagnosa { get; set; }
    }

    public class ReferenceDiagnoseResponseDataDetail
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    #endregion

    #region 02. Poli

    public class ReferencePoliResponse
    {
        public BPJSReferenceModel metadata { get; set; }
        public ReferencePoliResponseData response { get; set; }
    }

    public class ReferencePoliResponseData
    {
        public ReferencePoliResponseDataDetail[] poli { get; set; }
    }

    public class ReferencePoliResponseDataDetail
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    #endregion

    #region 03. FasilitasKesehatan

    public class ReferenceFasilitasKesehatanResponse
    {
        public BPJSReferenceModel metadata { get; set; }
        public ReferenceFasilitasKesehatanResponseData response { get; set; }
    }

    public class ReferenceFasilitasKesehatanResponseData
    {
        public ReferenceFasilitasKesehatanResponseDataDetail[] faskes { get; set; }
    }

    public class ReferenceFasilitasKesehatanResponseDataDetail
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    #endregion

    #region 04. Kabupaten

    public class ReferenceKabupatenResponse
    {
        public BPJSReferenceModel metadata { get; set; }
        public ReferenceKabupatenResponseData response { get; set; }
    }

    public class ReferenceKabupatenResponseData
    {
        public ReferenceKabupatenResponseDataDetail[] list { get; set; }
    }

    public class ReferenceKabupatenResponseDataDetail
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    #endregion

    #region 05. Kecamatan

    public class ReferenceKecamatanResponse
    {
        public BPJSReferenceModel metadata { get; set; }
        public ReferenceKecamatanResponseData response { get; set; }
    }

    public class ReferenceKecamatanResponseData
    {
        public ReferenceKecamatanResponseDataDetail[] list { get; set; }
    }

    public class ReferenceKecamatanResponseDataDetail
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    #endregion

    #region 06. Propinsi

    public class ReferencePropinsiResponse
    {
        public BPJSReferenceModel metadata { get; set; }
        public ReferencePropinsiResponseData response { get; set; }
    }

    public class ReferencePropinsiResponseData
    {
        public ReferencePropinsiResponseDataDetail[] list { get; set; }
    }

    public class ReferencePropinsiResponseDataDetail
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    #endregion

    #region 07. DokterDPJP

    public class ReferenceDokterDPJPResponse
    {
        public BPJSReferenceModel metadata { get; set; }
        public ReferenceDokterDPJPResponseData response { get; set; }
    }

    public class ReferenceDokterDPJPResponseData
    {
        public ReferenceDokterDPJPResponseDataDetail[] list { get; set; }
    }

    public class ReferenceDokterDPJPResponseDataDetail
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    #endregion

    #region 08. Procedure

    public class ReferenceProcedureResponse
    {
        public BPJSReferenceModel metadata { get; set; }
        public ReferenceProcedureResponseData response { get; set; }
    }

    public class ReferenceProcedureResponseData
    {
        public ReferenceProcedureResponseDataDetail[] procedure { get; set; }
    }

    public class ReferenceProcedureResponseDataDetail
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    #endregion

    #region 09. ClassCare

    public class ReferenceClassCareResponse
    {
        public BPJSReferenceModel metadata { get; set; }
        public ReferenceClassCareResponseData response { get; set; }
    }

    public class ReferenceClassCareResponseData
    {
        public ReferenceClassCareResponseDataDetail[] list { get; set; }
    }

    public class ReferenceClassCareResponseDataDetail
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    #endregion

    #region 10. Dokter

    public class ReferenceParamedicResponse
    {
        public BPJSReferenceModel metadata { get; set; }
        public ReferenceParamedicResponseData response { get; set; }
    }

    public class ReferenceParamedicResponseData
    {
        public ReferenceParamedicResponseDataDetail[] list { get; set; }
    }

    public class ReferenceParamedicResponseDataDetail
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    #endregion

    #region 11. Spesialistik

    public class ReferenceSpecialityResponse
    {
        public BPJSReferenceModel metadata { get; set; }
        public ReferenceSpecialityResponseData response { get; set; }
    }

    public class ReferenceSpecialityResponseData
    {
        public ReferenceSpecialityResponseDataDetail[] list { get; set; }
    }

    public class ReferenceSpecialityResponseDataDetail
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    #endregion

    #region 12. Ruang Rawat

    public class ReferenceRuangRawatResponse
    {
        public BPJSReferenceModel metadata { get; set; }
        public ReferenceRuangRawatResponseData response { get; set; }
    }

    public class ReferenceRuangRawatResponseData
    {
        public ReferenceRuangRawatResponseDataDetail[] list { get; set; }
    }

    public class ReferenceRuangRawatResponseDataDetail
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    #endregion

    #region 13. Cara Keluar

    public class ReferenceCaraKeluarResponse
    {
        public BPJSReferenceModel metadata { get; set; }
        public ReferenceCaraKeluarResponseData response { get; set; }
    }

    public class ReferenceCaraKeluarResponseData
    {
        public ReferenceCaraKeluarResponseDataDetail[] list { get; set; }
    }

    public class ReferenceCaraKeluarResponseDataDetail
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    #endregion

    #region 14. Pasca Pulang

    public class ReferencePascaPulangResponse
    {
        public BPJSReferenceModel metadata { get; set; }
        public ReferencePascaPulangResponseData response { get; set; }
    }

    public class ReferencePascaPulangResponseData
    {
        public ReferencePascaPulangResponseDataDetail[] list { get; set; }
    }

    public class ReferencePascaPulangResponseDataDetail
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    #endregion

    #region MOBILE JKN
    public class UpdateJadwalDokter
    {
        public string ParamedicCode { get; set; }
        public string ServiceUnitCode { get; set; }
    }
    public class TambahAntrianJKN
    {
        public string AppointmentNo { get; set; }
        public string RegistrationNo { get; set; }
    }
    public class BatalAntrianJKN
    {
        public string IsFromAppointment { get; set; }
        public string AppointmentNo { get; set; }
        public string DeleteReason { get; set; }
    }
    public class UpdateAntrianWaktuJKN
    {
        public string AppointmentNo { get; set; }
        public string TaskID { get; set; }
    }

    public class DashboardAntrianOnline
    {
        public MetadataAntrol metadata { get; set; }
        public ResponseAntrol response { get; set; }
    }

    public class MetadataAntrol
    {
        public int code { get; set; }
        public string message { get; set; }
    }

    public class ResponseAntrol
    {
        public List<ListAntrianOnline> list { get; set; }
    }

    public class ListAntrianOnline
    {
        public string kdppk { get; set; }
        public string nmppk { get; set; }
        public string kodepoli { get; set; }
        public string namapoli { get; set; }
        public string tanggal { get; set; }
        public int jumlah_antrean { get; set; }
        public int waktu_task1 { get; set; }
        public int waktu_task2 { get; set; }
        public int waktu_task3 { get; set; }
        public int waktu_task4 { get; set; }
        public int waktu_task5 { get; set; }
        public int waktu_task6 { get; set; }
        public float avg_waktu_task1 { get; set; }
        public float avg_waktu_task2 { get; set; }
        public float avg_waktu_task3 { get; set; }
        public float avg_waktu_task4 { get; set; }
        public float avg_waktu_task5 { get; set; }
        public float avg_waktu_task6 { get; set; }
        public long insertdate { get; set; }

        public string waktu_task1_string
        {
            get
            {
                return waktu_task1.ToString();
            }
        }
        public string waktu_task2_string
        {
            get
            {
                return waktu_task2.ToString();
            }
        }
        public string waktu_task3_string
        {
            get
            {
                return waktu_task3.ToString();
            }
        }
        public string waktu_task4_string
        {
            get
            {
                return waktu_task4.ToString();
            }
        }
        public string waktu_task5_string
        {
            get
            {
                return waktu_task5.ToString();
            }
        }
        public string avg_waktu_task1_string
        {
            get
            {
                return avg_waktu_task1.ToString();
            }
        }
        public string avg_waktu_task2_string
        {
            get
            {
                return avg_waktu_task2.ToString();
            }
        }
        public string avg_waktu_task3_string
        {
            get
            {
                return avg_waktu_task3.ToString();
            }
        }
        public string avg_waktu_task4_string
        {
            get
            {
                return avg_waktu_task4.ToString();
            }
        }
        public string avg_waktu_task5_string
        {
            get
            {
                return avg_waktu_task5.ToString();
            }
        }
    }
    #endregion
}