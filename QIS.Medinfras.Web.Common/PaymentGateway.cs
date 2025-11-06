using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Web.Common
{
    public class ApiResponse {
        public String Status { get; set; }
        public String Remarks { get; set; }
        public String Data { get; set; }
    }
    public class DokuBcaVaRequest
    {
        public DOKU_BCA_VA_Order order { get; set; }
        public DOKU_BCA_VA_VirtualAccountInfo virtual_account_info { get; set; }
        public DOKU_BCA_VA_Customer customer { get; set; }
    }
    public class DOKU_BCA_VA_Order
    {
        public string invoice_number { get; set; }
        public Decimal amount { get; set; }
    }

    public class DOKU_BCA_VA_VirtualAccountInfo
    {
        public long expired_time { get; set; }
        public bool reusable_status { get; set; }
        public string info1 { get; set; }
        public string info2 { get; set; }
        public string info3 { get; set; }
    }

    public class DOKU_BCA_VA_Customer
    {
        public string name { get; set; }
        public string email { get; set; }
    }

    public class DokuVaHisResponse {
        public string BillingNo { get; set; }
        public string BillingPage { get; set; }
        public string Billing_QRHash { get; set; }
        public string QRIS_Logo { get; set; }
        public string QRIS_NMID { get; set; }
    }

    #region request HIS
    public class DokuHISRequestVa
    {
        public string BillingNo { get; set; }
        public string CashierGroup { get; set; }
        public string CashierShift { get; set; }
        public string PaymentMethod { get; set; }
        public string ProviderMethod { get; set; }
        public string BankCode { get; set; }
        public bool IsCancel { get; set; }
        public string Channel { get; set; }
        public string JsonRequest { get; set; }
        public string JsonResponse { get; set; }
    }

    public class DokuHISVa {
        public string HealthcareID { get; set; }
        public string BillingNo { get; set; }
        public Decimal BillingAmount { get; set; }
        public string PatientEmail { get; set; }
        public string PatientName { get; set; }
        public string PatientPhoneNo { get; set; }
        
    }
    #endregion

    #region Response
    public class HISResponse {
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Data { get; set; }
    }
    public class DokuBcaVaResponse
    {
        public Order order { get; set; }
        public VirtualAccountInfo virtual_account_info { get; set; }
    }
    public class Order
    {
        public string invoice_number { get; set; }
    }
    public class VirtualAccountInfo
    {
        public string virtual_account_number { get; set; }
        public string how_to_pay_page { get; set; }
        public string how_to_pay_api { get; set; }
        public string created_date { get; set; }
        public string expired_date { get; set; }
        public DateTime created_date_utc { get; set; }
        public DateTime expired_date_utc { get; set; }
    }

    public class DokuResponsePaymentStatus
    {
        public Service service { get; set; }
        public Acquirer acquirer { get; set; }
        public Channel channel { get; set; }
        public Transaction transaction { get; set; }
        public Order order { get; set; }
        public VirtualAccountInfo virtual_account_info { get; set; }
        public VirtualAccountPayment virtual_account_payment { get; set; }
    }

    public class Service
    {
        public string id { get; set; }
    }

    public class Acquirer
    {
        public string id { get; set; }
    }

    public class Channel
    {
        public string id { get; set; }
    }

    public class Transaction
    {
        public string status { get; set; }
        public DateTime date { get; set; }
        public string original_request_id { get; set; }
    }

    public class Identifer
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class VirtualAccountPayment
    {
        public List<Identifer> identifer { get; set; }
    }

    public class QrisJsonResponse {
        public string qrCode { get; set; }
        public string transactionId { get; set; }
        public string responseCode { get; set; }
    }


    #endregion
 

    #region Response DOKU

    public class DOKULogResponseLog
    {
        public string[] message { get; set; }
        public DOKULog_Payment_Response_Checkout response { get; set; }

        public DOKULog_Payment_Response_Client client { get; set; }
        public DOKULog_Payment_Response_Order order { get; set; }
        public DOKULog_Payment_Response_Transaction transaction { get; set; }
        public DOKULog_Payment_Response_VirtualInfo virtual_account_info { get; set; }

        //SHOPEE
        public DOKULog_Payment_Response_ShopeePay_Config shopeepay_configuration { get; set; }
        public DOKULog_Payment_Response_ShopeePay_Payment shopeepay_payment { get; set; }

        //OVO
        public DOKULog_Payment_Response_Ovo_Info ovo_info { get; set; }
        public DOKULog_Payment_Response_Ovo_Config ovo_configuration { get; set; }
        public DOKULog_Payment_Response_Ovo_Payment ovo_payment { get; set; }
    }

    public class DOKULog_Payment_Response_Checkout
    {
        public DOKULog_Payment_Response_Order order { get; set; }
        public DOKULog_Payment_Response_Payment payment { get; set; }
    }

    public class DOKULog_Payment_Response_Client
    {
        public string id { get; set; }
    }
    public class DOKULog_Payment_Response_Order
    {
        public string invoice_number { get; set; }
        public long amount { get; set; }
        public string currency { get; set; }
        public string session_id { get; set; }
    }
    public class DOKULog_Payment_Response_Transaction
    {
        public string status { get; set; }
        public string original_request_id { get; set; }
    }
    public class DOKULog_Payment_Response_VirtualInfo
    {
        public string virtual_account_number { get; set; }
        public string how_to_pay_page { get; set; }
        public string how_to_pay_api { get; set; }
        public string created_date { get; set; }
        public string expired_date { get; set; }
        public string created_date_utc { get; set; }
        public string expired_date_utc { get; set; }
    }
    public class DOKULog_Payment_Response_ShopeePay_Config
    {
        public string merchant_ext_id { get; set; }
        public string store_ext_id { get; set; }
    }

    public class DOKULog_Payment_Response_ShopeePay_Payment
    {
        public string redirect_url_http { get; set; }
        public string status { get; set; }
    }

    public class DOKULog_Payment_Response_Ovo_Info
    {
        public string ovo_id { get; set; }
        public string ovo_account_name { get; set; }
    }

    public class DOKULog_Payment_Response_Ovo_Config
    {
        public string merchant_id { get; set; }
        public string store_code { get; set; }
        public string mid { get; set; }
        public string tid { get; set; }
    }

    public class DOKULog_Payment_Response_Ovo_Payment
    {
        public string date { get; set; }
        public int batch_number { get; set; }
        public int trace_number { get; set; }
        public int reference_number { get; set; }
        public string approval_code { get; set; }
        public string response_code { get; set; }
        public long cash_used { get; set; }
        public long cash_balance { get; set; }
        public long ovo_points_used { get; set; }
        public long ovo_points_balance { get; set; }
        public long ovo_points_earned { get; set; }
        public string status { get; set; }

    }
    public class DOKULog_Payment_Response_Payment
    {
        public string[] payment_method_type { get; set; }
        public int payment_due_date { get; set; }
        public string token_id { get; set; }
        public string url { get; set; }
        public string expired_date { get; set; }
    }

    #endregion
}
