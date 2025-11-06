using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class Main : BasePage
    {
        private const string STATUS_IMAGE_PATH = "~/libs/Images/Dashboard/";

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private void GetHealthcareSettingParameter(string healthcareID)
        {
            //Healthcare Setting Parameter
            string filterExp = string.Format(
                "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}','{46}','{47}','{48}','{49}','{50}','{51}','{52}','{53}','{54}','{55}','{56}','{57}','{58}','{59}','{60}','{61}','{62}','{63}','{64}','{65}','{66}','{67}','{68}','{69}','{70}','{71}','{72}','{73}','{74}','{75}','{76}','{77}','{78}','{79}','{80}','{81}','{82}','{83}','{84}','{85}','{86}','{87}','{88}','{89}','{90}','{91}','{92}','{93}','{94}','{95}','{96}','{97}','{98}','{99}','{100}','{101}','{102}','{103}','{104}','{105}','{106}','{107}','{108}','{109}','{110}','{111}','{112}','{113}','{114}','{115}','{116}','{117}','{118}','{119}','{120}','{121}','{122}','{123}','{124}','{125}','{126}','{127}','{128}','{129}','{130}','{131}','{132}','{133}','{134}','{135}','{136}','{137}','{138}','{139}','{140}','{141}','{142}','{143}','{144}','{145}','{146}','{147}','{148}','{149}','{150}','{151}','{152}','{153}','{154}','{155}','{156}','{157}','{158}','{159}','{160}','{161}','{162}','{163}','{164}','{165}','{166}')",
                healthcareID, //0
                Constant.SettingParameter.IS_BPJS_BRIDGING, //1
                Constant.SettingParameter.BPJS_CODE, //2
                Constant.SettingParameter.BPJS_CONSUMER_ID, //3
                Constant.SettingParameter.BPJS_CONSUMER_PASSWORD, //4
                Constant.SettingParameter.BPJS_SEP_WS_URL, //5
                Constant.SettingParameter.IS_RIS_BRIDGING, //6
                Constant.SettingParameter.IS_RIS_CONSUMER_ID, //7
                Constant.SettingParameter.IS_RIS_CONSUMER_PASSWORD, //8
                Constant.SettingParameter.IS_RIS_WEB_API_URL, //9
                Constant.SettingParameter.IS_RIS_WEB_VIEW_URL, //10
                Constant.SettingParameter.IS_RIS_BRIDGING_PROTOCOL, //11
                Constant.SettingParameter.IS_BRIDGING_TOOLS, //12
                Constant.SettingParameter.IS_RESULT_SEND_TO_BRIDGING, //13
                Constant.SettingParameter.IS_HL7_MESSAGE_FORMAT, //14
                Constant.SettingParameter.SA_BRIDGING_SISTEM_ANTRIAN, //15
                Constant.SettingParameter.SA_PROTOKOL_BRIDGING_SISTEM_ANTRIAN, //16
                Constant.SettingParameter.SA_CONSID_BRIDGING_SISTEM_ANTRIAN, //17
                Constant.SettingParameter.SA_CONSPWD_BRIDGING_SISTEM_ANTRIAN, //18
                Constant.SettingParameter.SA_ALAMAT_WEBAPI_SISTEM_ANTRIAN, //19
                Constant.SettingParameter.SA_LOKASI_PRINTER_IP_ADDR, //20
                Constant.SettingParameter.IS_APLICARES_BRIDGING, //21
                Constant.SettingParameter.APLICARES_CONSUMER_ID, //22
                Constant.SettingParameter.APLICARES_CONSUMER_PASSWORD, //23
                Constant.SettingParameter.APLICARES_SEP_WS_URL, //24
                Constant.SettingParameter.RM_MULTI_LOKASI_PENDAFTARAN, //25
                Constant.SettingParameter.EMR_PATIENT_PAGE_BY_DEPARTMENT, //26
                Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE, //27
                Constant.SettingParameter.FN_PEMBATASAN_CPOE_BPJS, //28
                Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN, //29
                Constant.SettingParameter.IS_BRIDGING_TO_EKLAIM, //30
                Constant.SettingParameter.EKLAIM_WEB_SERVICE_URL, //31
                Constant.SettingParameter.EKLAIM_HOSPITAL_CODE, //32
                Constant.SettingParameter.EKLAIM_ENCRYPTION_KEY, //33
                Constant.SettingParameter.INHEALTH_WEB_SERVICE_URL, //34
                Constant.SettingParameter.INHEALTH_ACCESS_TOKEN, //35
                Constant.SettingParameter.INHEALTH_PROVIDER_CODE, //36
                Constant.SettingParameter.IS_BRIDGING_TO_INHEALTH, //37
                Constant.SettingParameter.LB_BRIDGING_LIS, //38
                Constant.SettingParameter.LB_LIS_BRIDGING_PROTOCOL, //39
                Constant.SettingParameter.LB_LIS_CONSUMER_ID, //40
                Constant.SettingParameter.LB_LIS_CONSUMER_PASSWORD, //41
                Constant.SettingParameter.LB_LIS_WEB_API_URL, //42
                Constant.SettingParameter.LB_LIS_PROVIDER, //43
                Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, //44
                Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, //45
                Constant.SettingParameter.FN_IS_AP_CONSIGNMENT_FROM_ORDER, //46
                Constant.SettingParameter.PH_IS_AUTO_RELOCATE_DISPENSARY, //47
                Constant.SettingParameter.PH_AUTO_RELOCATE_DISPENSARY_TIME, //48
                Constant.SettingParameter.PH_AUTO_RELOCATE_DISPENSARY_ID, //49
                Constant.SettingParameter.PH_USE_BPJS_ORDER_ENTRY, //50
                Constant.SettingParameter.PH_BPJS_TAKEN_QTY_FORMULA, //51
                Constant.SettingParameter.PH_AUTO_VOID_DISCHARGE_PRESCRIPTION_ORDER, //52
                Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL, //53
                Constant.SettingParameter.IS_USED_PATIENT_OWNER_STATUS, //54
                Constant.SettingParameter.FN_IS_USED_CALCULATE_COVERAGE_PER_BILLING_GROUP, //55
                Constant.SettingParameter.SA_USE_VERIFY_ALL_BUTTON, //56
                Constant.SettingParameter.SA_IS_USED_REVENUE_COST_CENTER, //57
                Constant.SettingParameter.FN_IS_USED_REOPEN_BILLING, //58
                Constant.SettingParameter.FN_IS_USED_CLAIM_FINAL, //59
                Constant.SettingParameter.FN_IS_CLAIM_FINAL_AFTER_AR_INVOICE, //60
                Constant.SettingParameter.SA_REGISTRATION_BUTTON_TRANSACTION_DIRECT_MENU, //61,
                Constant.SettingParameter.PH_UDD_MEDICATION_RECEIPT, //62
                Constant.SettingParameter.FM_USE_UDD_DRUG_DISPENSE_LABEL, //63
                Constant.SettingParameter.FM_FORMAT_CETAKAN_LABEL_UDD, //64
                Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS, //65
                Constant.SettingParameter.FN_PENJAMIN_INHEALTH, //66
                Constant.SettingParameter.EM_PEMBATASAN_CPOE_INHEALTH, //67
                Constant.SettingParameter.FM_AKTIFASI_FILTER_JENIS_RESEP_RAWAT_INAP, //68
                Constant.SettingParameter.FM_FILTER_VALUE_JENIS_RESEP_RAWAT_INAP, //69
                Constant.SettingParameter.DEFAULT_DAYS_PRMRJ, //70
                Constant.SettingParameter.IM_POR_WITH_PRICE_INFORMATION, //71
                Constant.SettingParameter.EM_PRINT_TRACER_FARMASI_KETIKA_SEND_ORDER, //72
                Constant.SettingParameter.OP0027, //73
                Constant.SettingParameter.IP0024, //74
                Constant.SettingParameter.IP0025, //75
                Constant.SettingParameter.IP0026, //76
                Constant.SettingParameter.IP0027, //77
                Constant.SettingParameter.OP0028, //78
                Constant.SettingParameter.OP0029, //79
                Constant.SettingParameter.OP0030, //80
                Constant.SettingParameter.MD0012, //81
                Constant.SettingParameter.MD0013, //82
                Constant.SettingParameter.MD0014, //83
                Constant.SettingParameter.FM_KONTROL_ADVERSE_REACTION, //84
                Constant.SettingParameter.FM_MAKSIMUM_DURASI_NARKOTIKA, //85
                Constant.SettingParameter.FM_KONTROL_DUPLIKASI_TERAPI, //86
                Constant.SettingParameter.FM_KONTROL_ERROR_ALERGI,  //87
                Constant.SettingParameter.SA0111,  //88
                Constant.SettingParameter.SA0112,  //89
                Constant.SettingParameter.SA0113,   //90
                Constant.SettingParameter.FN_IS_CLAIM_FINAL_BEFORE_AR_INVOICE_AND_SKIP_CLAIM,   //91
                Constant.SettingParameter.MD0006,  //92
                Constant.SettingParameter.EM0034,  //93
                Constant.SettingParameter.IS_USED_PATIENT_OWNER_IN_INPATIENT_REGISTRATION,  //94,
                Constant.SettingParameter.EM0036, //95
                Constant.SettingParameter.MD0016, //96
                Constant.SettingParameter.MD0017, //97     
                Constant.SettingParameter.SA0119, //98     
                Constant.SettingParameter.SA0120, //99
                Constant.SettingParameter.SA0121, //100
                Constant.SettingParameter.SA0126, //101 
                Constant.SettingParameter.SA0127, //102 
                Constant.SettingParameter.SA0128, //103
                Constant.SettingParameter.SA0129, //104
                Constant.SettingParameter.SA0130, //105 
                Constant.SettingParameter.SA0131, //106
                Constant.SettingParameter.SA0132, //107
                Constant.SettingParameter.SA0133, //108
                Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS, //109
                Constant.SettingParameter.SA0137, //110
                Constant.SettingParameter.SA0138, //111
                Constant.SettingParameter.SA0139, //112
                Constant.SettingParameter.SA0140, //113
                Constant.SettingParameter.SA0141,  //114
                Constant.SettingParameter.IS_USED_PATIENT_OWNER_IN_OUTPATIENT_REGISTRATION, //115
                Constant.SettingParameter.IS_USED_PATIENT_OWNER_IN_EMERGENCY_REGISTRATION, //116
                Constant.SettingParameter.IS_USED_PATIENT_OWNER_IN_MCU_REGISTRATION, //117
                Constant.SettingParameter.IS_USED_PATIENT_OWNER_IN_LABORATORY_REGISTRATION, //118
                Constant.SettingParameter.IS_USED_PATIENT_OWNER_IN_IMAGING_REGISTRATION, //119
                Constant.SettingParameter.IS_USED_PATIENT_OWNER_IN_DIAGNOSTIC_REGISTRATION, //120
                Constant.SettingParameter.IS_USED_PATIENT_OWNER_IN_PHARMACY_REGISTRATION, //121
                Constant.SettingParameter.IS_USED_INPUT_AIO_PACKAGE_IN_OUTPATIENT_REGISTRATION, //122
                Constant.SettingParameter.IS_USED_INPUT_AIO_PACKAGE_IN_LABORATORY_REGISTRATION, //123
                Constant.SettingParameter.IS_USED_INPUT_AIO_PACKAGE_IN_IMAGING_REGISTRATION, //124
                Constant.SettingParameter.IS_USED_INPUT_AIO_PACKAGE_IN_DIAGNOSTIC_REGISTRATION, //125
                Constant.SettingParameter.IS_USED_INPUT_AIO_PACKAGE_IN_INPATIENT_REGISTRATION, //126
                Constant.SettingParameter.SA0167, //127
                Constant.SettingParameter.EM_IS_VALIDATION_INPUT_SURGERY_ASSESSMENT_FIRST, //128
                Constant.SettingParameter.NR0001, //129
                Constant.SettingParameter.SA0175, //130
                Constant.SettingParameter.EM0049, //131
                Constant.SettingParameter.EM0050, //132
                Constant.SettingParameter.EM0051, //133
                Constant.SettingParameter.SA_IS_BRIDGING_TO_IPTV, //134
                Constant.SettingParameter.MD0018, //135
                Constant.SettingParameter.LB0034, //136
                Constant.SettingParameter.LB0035, //137
                Constant.SettingParameter.PH0070, //138
                Constant.SettingParameter.SA0197, //139
                Constant.SettingParameter.SA0194, //140
                Constant.SettingParameter.SA0195, //141
                Constant.SettingParameter.SA0196, //142,
                Constant.SettingParameter.SA0198, //143
                Constant.SettingParameter.SA0199, //144
                Constant.SettingParameter.CONSUMER_CONS_ID, //145
                Constant.SettingParameter.CONSUMER_PASS_ID, //146
                Constant.SettingParameter.SA0200, //147
                Constant.SettingParameter.SA0201, //148
                Constant.SettingParameter.SA_REPORT_FOOTER_PRINTEDBY_USERNAME_FULLNAME, //149
                Constant.SettingParameter.EM0058, //150
                Constant.SettingParameter.EM0010, //151
                Constant.SettingParameter.EM0072,  //152
                Constant.SettingParameter.ER_REGISTRATION_ORDER_BY,  //153
                Constant.SettingParameter.MC_REGISTRATION_ORDER_BY,  //154
                Constant.SettingParameter.MD_REGISTRATION_ORDER_BY,  //155
                Constant.SettingParameter.PH_REGISTRATION_ORDER_BY,  //156
                Constant.SettingParameter.EM0079,   //157
                Constant.SettingParameter.RT0001,   //158
                Constant.SettingParameter.EM0088,   //159
                Constant.SettingParameter.IP0038,   //160
                Constant.SettingParameter.OP0043,   //161
                Constant.SettingParameter.ER0011,   //162
                Constant.SettingParameter.MD0027,   //163
                Constant.SettingParameter.PH0082,   //164
                Constant.SettingParameter.MC0014,   //165
                Constant.SettingParameter.EM0090   //166
                );

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(filterExp);

            if (lstSettingParameterDt != null)
            {
                AppSession.IsBridgingToBPJS = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_BPJS_BRIDGING)).FirstOrDefault().ParameterValue == "1" ? true : false;
                AppSession.BPJS_WS_URL = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.BPJS_SEP_WS_URL)).FirstOrDefault().ParameterValue;
                AppSession.BPJS_Code = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.BPJS_CODE)).FirstOrDefault().ParameterValue;
                AppSession.BPJS_Consumer_ID = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.BPJS_CONSUMER_ID)).FirstOrDefault().ParameterValue;
                AppSession.BPJS_Consumer_Pwd = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.BPJS_CONSUMER_PASSWORD)).FirstOrDefault().ParameterValue;

                AppSession.IsBridgingToRIS = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_RIS_BRIDGING)).FirstOrDefault().ParameterValue == "1" ? true : false;
                AppSession.RIS_BRIDGING_PROTOCOL = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_RIS_BRIDGING_PROTOCOL)).FirstOrDefault().ParameterValue;
                AppSession.RIS_WEB_API_URL = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_RIS_WEB_API_URL)).FirstOrDefault().ParameterValue;
                AppSession.RIS_Consumer_ID = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_RIS_CONSUMER_ID)).FirstOrDefault().ParameterValue;
                AppSession.RIS_Consumer_Pwd = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_RIS_CONSUMER_PASSWORD)).FirstOrDefault().ParameterValue;
                AppSession.RIS_WEB_VIEW_URL = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_RIS_WEB_VIEW_URL)).FirstOrDefault().ParameterValue;
                AppSession.BRIDGING_TOOLS = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_BRIDGING_TOOLS)).FirstOrDefault().ParameterValue;
                AppSession.RIS_HL7_MESSAGE_FORMAT = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_HL7_MESSAGE_FORMAT)).FirstOrDefault().ParameterValue;
                AppSession.RESULT_SEND_TO_BRIDGING = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_RESULT_SEND_TO_BRIDGING)).FirstOrDefault().ParameterValue;

                AppSession.IsBridgingToQueue = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.SA_BRIDGING_SISTEM_ANTRIAN)).FirstOrDefault().ParameterValue == "1" ? true : false;
                AppSession.QUEUE_WEB_API_URL = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.SA_ALAMAT_WEBAPI_SISTEM_ANTRIAN)).FirstOrDefault().ParameterValue;
                AppSession.QUEUE_Consumer_ID = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.SA_CONSID_BRIDGING_SISTEM_ANTRIAN)).FirstOrDefault().ParameterValue;
                AppSession.QUEUE_Consumer_Pwd = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.SA_CONSPWD_BRIDGING_SISTEM_ANTRIAN)).FirstOrDefault().ParameterValue;

                AppSession.IsPrinterLocationBasedOnIP = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.SA_LOKASI_PRINTER_IP_ADDR)).FirstOrDefault().ParameterValue == "1" ? true : false;

                AppSession.PreviewReportInPDF = AppConfigManager.PreviewReportInPDF == null ? "1" : AppConfigManager.PreviewReportInPDF;

                AppSession.IsBridgingToAPLICARES = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_APLICARES_BRIDGING)).FirstOrDefault().ParameterValue == "1" ? true : false;
                AppSession.APLICARES_WS_URL = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.APLICARES_SEP_WS_URL)).FirstOrDefault().ParameterValue;
                AppSession.APLICARES_Consumer_ID = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.APLICARES_CONSUMER_ID)).FirstOrDefault().ParameterValue;
                AppSession.APLICARES_Consumer_Pwd = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.APLICARES_CONSUMER_PASSWORD)).FirstOrDefault().ParameterValue;

                AppSession.IsUsingMultiRegistrationPrinterLocation = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_MULTI_LOKASI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                AppSession.IsPatientPageByDepartment = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.EMR_PATIENT_PAGE_BY_DEPARTMENT)).FirstOrDefault().ParameterValue == "1" ? true : false;

                AppSession.IsUsedProductLine = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE)).FirstOrDefault().ParameterValue;

                AppSession.IsLimitedCPOEItemForBPJS = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FN_PEMBATASAN_CPOE_BPJS)).FirstOrDefault().ParameterValue == "1" ? true : false;

                SettingParameterDt oParam = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault();

                AppSession.BusinessPartnerIDBPJS = oParam != null ? Convert.ToInt32(oParam.ParameterValue) : 0;

                AppSession.IsBridgingToEKlaim = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_BRIDGING_TO_EKLAIM)).FirstOrDefault().ParameterValue == "1" ? true : false;
                AppSession.EKlaim_WebService_URL = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.EKLAIM_WEB_SERVICE_URL)).FirstOrDefault().ParameterValue;
                AppSession.EKlaim_HospitalCode = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.EKLAIM_HOSPITAL_CODE)).FirstOrDefault().ParameterValue;
                AppSession.EKlaim_EncryptionKey = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.EKLAIM_ENCRYPTION_KEY)).FirstOrDefault().ParameterValue;

                AppSession.IsBridgingToInhealth = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_BRIDGING_TO_INHEALTH)).FirstOrDefault().ParameterValue == "1" ? true : false;
                AppSession.Inhealth_WebService_URL = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.INHEALTH_WEB_SERVICE_URL)).FirstOrDefault().ParameterValue;
                AppSession.Inheatlh_Access_Token = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.INHEALTH_ACCESS_TOKEN)).FirstOrDefault().ParameterValue;
                AppSession.Inhealth_Provider_Code = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.INHEALTH_PROVIDER_CODE)).FirstOrDefault().ParameterValue;

                AppSession.IsBridgingToLIS = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.LB_BRIDGING_LIS)).FirstOrDefault().ParameterValue == "1" ? true : false;
                AppSession.LIS_BRIDGING_PROTOCOL = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.LB_LIS_BRIDGING_PROTOCOL)).FirstOrDefault().ParameterValue;
                AppSession.LIS_WEB_API_URL = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.LB_LIS_WEB_API_URL)).FirstOrDefault().ParameterValue;
                AppSession.LIS_Consumer_ID = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.LB_LIS_CONSUMER_ID)).FirstOrDefault().ParameterValue;
                AppSession.LIS_Consumer_Pwd = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.LB_LIS_CONSUMER_PASSWORD)).FirstOrDefault().ParameterValue;
                AppSession.LIS_PROVIDER = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.LB_LIS_PROVIDER)).FirstOrDefault().ParameterValue;

                AppSession.ImagingServiceUnitID = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
                AppSession.LaboratoryServiceUnitID = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
                AppSession.IsAPConsignmentFromOrder = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_IS_AP_CONSIGNMENT_FROM_ORDER).ParameterValue;

                AppSession.IsAutoRelocateDispensary = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_IS_AUTO_RELOCATE_DISPENSARY).ParameterValue;
                AppSession.AutoRelocateDispensaryTime = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_AUTO_RELOCATE_DISPENSARY_TIME).ParameterValue;
                AppSession.AutoRelocateDispensaryID = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_AUTO_RELOCATE_DISPENSARY_ID).ParameterValue;

                AppSession.UseBPJSPrescriptionEntryMode = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_USE_BPJS_ORDER_ENTRY).ParameterValue;
                AppSession.BPJSTakenQtyFormula = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_BPJS_TAKEN_QTY_FORMULA).ParameterValue;
                AppSession.IsAutoVoidPrescriptionOrderCreatedBySystem = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_AUTO_VOID_DISCHARGE_PRESCRIPTION_ORDER).ParameterValue;

                AppSession.RefreshGridInterval = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                AppSession.IsUsedPatientOwnerStatus = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_USED_PATIENT_OWNER_STATUS).ParameterValue;
                AppSession.IsUsedPatientOwnerStatusInInpatientRegistration = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_USED_PATIENT_OWNER_IN_INPATIENT_REGISTRATION).ParameterValue;
                AppSession.IsUsedPatientOwnerStatusInOutpatientRegistration = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_USED_PATIENT_OWNER_IN_OUTPATIENT_REGISTRATION).ParameterValue;
                AppSession.IsUsedPatientOwnerStatusInEmergencyRegistration = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_USED_PATIENT_OWNER_IN_EMERGENCY_REGISTRATION).ParameterValue;
                AppSession.IsUsedPatientOwnerStatusInMCURegistration = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_USED_PATIENT_OWNER_IN_MCU_REGISTRATION).ParameterValue;
                AppSession.IsUsedPatientOwnerStatusInLaboratoryRegistration = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_USED_PATIENT_OWNER_IN_LABORATORY_REGISTRATION).ParameterValue;
                AppSession.IsUsedPatientOwnerStatusInImagingRegistration = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_USED_PATIENT_OWNER_IN_IMAGING_REGISTRATION).ParameterValue;
                AppSession.IsUsedPatientOwnerStatusInDiagnosticRegistration = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_USED_PATIENT_OWNER_IN_DIAGNOSTIC_REGISTRATION).ParameterValue;
                AppSession.IsUsedPatientOwnerStatusInPharmacyRegistration = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_USED_PATIENT_OWNER_IN_PHARMACY_REGISTRATION).ParameterValue;

                AppSession.IsUsedInputItemAIOInOutpatientRegistration = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_USED_INPUT_AIO_PACKAGE_IN_OUTPATIENT_REGISTRATION).ParameterValue;
                AppSession.IsUsedInputItemAIOInLaboratoryRegistration = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_USED_INPUT_AIO_PACKAGE_IN_LABORATORY_REGISTRATION).ParameterValue;
                AppSession.IsUsedInputItemAIOInImagingRegistration = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_USED_INPUT_AIO_PACKAGE_IN_IMAGING_REGISTRATION).ParameterValue;
                AppSession.IsUsedInputItemAIOInDiagnosticRegistration = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_USED_INPUT_AIO_PACKAGE_IN_DIAGNOSTIC_REGISTRATION).ParameterValue;
                AppSession.IsUsedInputItemAIOInInpatientRegistration = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_USED_INPUT_AIO_PACKAGE_IN_INPATIENT_REGISTRATION).ParameterValue;

                AppSession.IsUsedCalculateCoveragePerBillingGroup = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_IS_USED_CALCULATE_COVERAGE_PER_BILLING_GROUP).ParameterValue;

                AppSession.IsUseVerifyAllButton = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.SA_USE_VERIFY_ALL_BUTTON)).FirstOrDefault().ParameterValue == "1" ? true : false;

                AppSession.IsUsedRevenueCostCenter = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.SA_IS_USED_REVENUE_COST_CENTER)).FirstOrDefault().ParameterValue;

                AppSession.IsUsedReopenBilling = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_IS_USED_REOPEN_BILLING).ParameterValue == "1" ? true : false;

                AppSession.IsUsedClaimFinal = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_IS_USED_CLAIM_FINAL).ParameterValue == "1" ? true : false;
                AppSession.IsClaimFinalAfterARInvoice = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_IS_CLAIM_FINAL_AFTER_AR_INVOICE).ParameterValue == "1" ? true : false;
                AppSession.IsClaimFinalBeforeARInvoiceAndSkipProcessClaim = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_IS_CLAIM_FINAL_BEFORE_AR_INVOICE_AND_SKIP_CLAIM).ParameterValue == "1" ? true : false;

                AppSession.RegistrationButtonTransactionDirectMenu = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.SA_REGISTRATION_BUTTON_TRANSACTION_DIRECT_MENU)).FirstOrDefault().ParameterValue;

                AppSession.UDDReceiptPrinterType = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.PH_UDD_MEDICATION_RECEIPT)).FirstOrDefault().ParameterValue;
                AppSession.IsUsedUDDDrugLabel = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FM_USE_UDD_DRUG_DISPENSE_LABEL).ParameterValue == "1" ? true : false;

                AppSession.UDDDrugLabelType = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FM_FORMAT_CETAKAN_LABEL_UDD)).FirstOrDefault().ParameterValue;

                AppSession.IsLimitedCPOEItemForBPJS = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS).ParameterValue == "1" ? true : false;

                AppSession.IsLimitedCPOEItemForInhealth = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM_PEMBATASAN_CPOE_INHEALTH).ParameterValue == "1" ? true : false;
                AppSession.IsLimitedCPOEItemForInhealthIP = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IP0038).ParameterValue == "1" ? true : false;
                AppSession.IsLimitedCPOEItemForInhealthOP = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP0043).ParameterValue == "1" ? true : false;
                AppSession.IsLimitedCPOEItemForInhealthER = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.ER0011).ParameterValue == "1" ? true : false;
                AppSession.IsLimitedCPOEItemForInhealthMD = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD0027).ParameterValue == "1" ? true : false;
                AppSession.IsLimitedCPOEItemForInhealthPH = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH0082).ParameterValue == "1" ? true : false;
                AppSession.IsLimitedCPOEItemForInhealthMC = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MC0014).ParameterValue == "1" ? true : false;

                AppSession.IsUsedInpatientPrescriptionTypeFilter = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FM_AKTIFASI_FILTER_JENIS_RESEP_RAWAT_INAP).ParameterValue == "1" ? true : false;

                oParam = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_INHEALTH).FirstOrDefault();
                AppSession.BusinessPartnerIDInhealth = oParam != null ? oParam.ParameterValue : "-";

                oParam = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).FirstOrDefault();
                AppSession.TipeCustomerBPJS = oParam != null ? oParam.ParameterValue : "-";

                oParam = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FM_FILTER_VALUE_JENIS_RESEP_RAWAT_INAP)).FirstOrDefault();
                AppSession.InpatientPrescriptionTypeFilter = oParam != null ? oParam.ParameterValue : "-";

                AppSession.DefaultDaysPRMRJ = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.DEFAULT_DAYS_PRMRJ).ParameterValue;

                AppSession.IsPORWithPriceInformation = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IM_POR_WITH_PRICE_INFORMATION)).FirstOrDefault().ParameterValue;

                AppSession.IsPrintPHOrderTracerFromEMR = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM_PRINT_TRACER_FARMASI_KETIKA_SEND_ORDER).ParameterValue == "1" ? true : false;

                AppSession.OP0027 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP0027).ParameterValue;

                AppSession.IP0024 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IP0024).ParameterValue;
                AppSession.IP0025 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IP0025).ParameterValue;
                AppSession.IP0026 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IP0026).ParameterValue;
                AppSession.IP0027 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IP0027).ParameterValue;
                AppSession.OP0028 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP0028).ParameterValue;
                AppSession.OP0029 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP0029).ParameterValue;
                AppSession.OP0030 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP0030).ParameterValue;

                AppSession.MD0006 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD0006).ParameterValue;
                AppSession.MD0012 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD0012).ParameterValue;
                AppSession.MD0013 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD0013).ParameterValue;
                AppSession.MD0014 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD0014).ParameterValue;
                AppSession.MD0016 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD0016).ParameterValue;
                AppSession.MD0017 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD0017).ParameterValue;
                AppSession.MD0018 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD0018).ParameterValue;

                AppSession.PH0006 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FM_KONTROL_ADVERSE_REACTION).ParameterValue;
                AppSession.PH0007 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FM_MAKSIMUM_DURASI_NARKOTIKA).ParameterValue;
                AppSession.PH0008 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FM_KONTROL_DUPLIKASI_TERAPI).ParameterValue;
                AppSession.PH0033 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FM_KONTROL_ERROR_ALERGI).ParameterValue;

                AppSession.SA0111 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0111).ParameterValue;
                AppSession.SA0112 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0112).ParameterValue;
                AppSession.SA0113 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0113).ParameterValue;

                AppSession.EM0010 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM0010).ParameterValue;
                AppSession.EM0034 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM0034).ParameterValue;
                AppSession.EM0036 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM0036).ParameterValue;
                AppSession.EM0049 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM0049).ParameterValue;
                AppSession.EM0050 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM0050).ParameterValue;
                AppSession.EM0058 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM0058).ParameterValue;

                AppSession.SA0119 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0119).ParameterValue;
                AppSession.SA0120 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0120).ParameterValue;
                AppSession.SA0121 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0121).ParameterValue;
                AppSession.SA0126 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0126).ParameterValue;
                AppSession.SA0127 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0127).ParameterValue;
                AppSession.SA0128 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0128).ParameterValue;
                AppSession.SA0129 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0129).ParameterValue;
                AppSession.SA0130 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0130).ParameterValue;
                AppSession.SA0131 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0131).ParameterValue;
                AppSession.SA0132 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0132).ParameterValue;
                AppSession.SA0133 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0133).ParameterValue;
                AppSession.SA0137 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0137).ParameterValue;

                AppSession.SA0138 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0138).ParameterValue;
                AppSession.SA0139 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0139).ParameterValue;
                AppSession.SA0140 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0140).ParameterValue;
                AppSession.SA0141 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0141).ParameterValue;

                AppSession.SA0167 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0167).ParameterValue;
                AppSession.SA0175 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0175).ParameterValue;

                AppSession.SA0197 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0197).ParameterValue;
                AppSession.SA0194 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0194).ParameterValue;
                AppSession.SA0195 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0195).ParameterValue;
                AppSession.SA0196 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0196).ParameterValue;
                AppSession.SA0198 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0198).ParameterValue == "1" ? true : false;
                AppSession.SA0199 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0199).ParameterValue;

                AppSession.EM0046 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM_IS_VALIDATION_INPUT_SURGERY_ASSESSMENT_FIRST).ParameterValue;
                AppSession.NR0001 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.NR0001).ParameterValue;
                AppSession.Is_Bridging_To_IPTV = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_IS_BRIDGING_TO_IPTV).ParameterValue == "1" ? true : false;

                AppSession.LB0034 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB0034).ParameterValue;
                AppSession.LB0035 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB0035).ParameterValue;

                AppSession.PH0070 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH0070).ParameterValue;

                AppSession.SA0058 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.CONSUMER_CONS_ID).ParameterValue;
                AppSession.SA0059 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.CONSUMER_PASS_ID).ParameterValue;
                AppSession.SA0200 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0200).ParameterValue;

                AppSession.EM0072 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM0072).ParameterValue;
                AppSession.EM0079 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM0079).ParameterValue;


                AppSession.SA0201 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0201).ParameterValue;

                AppSession.ReportFooterPrintedByInfo = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_REPORT_FOOTER_PRINTEDBY_USERNAME_FULLNAME).ParameterValue;

                AppSession.RegistrationOrderBy_ER = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.ER_REGISTRATION_ORDER_BY).ParameterValue;
                AppSession.RegistrationOrderBy_MC = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MC_REGISTRATION_ORDER_BY).ParameterValue;
                AppSession.RegistrationOrderBy_MD = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD_REGISTRATION_ORDER_BY).ParameterValue;
                AppSession.RegistrationOrderBy_PH = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_REGISTRATION_ORDER_BY).ParameterValue;

                string rt0001Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.RT0001).ParameterValue;
                if (rt0001Value != null && rt0001Value != "")
                {
                    AppSession.RT0001 = rt0001Value;
                }
                else
                {
                    AppSession.RT0001 = "0";
                }

                AppSession.EM0088 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM0088).ParameterValue;
                AppSession.EM0090 = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM0090).ParameterValue;
            }
        }

    }
}