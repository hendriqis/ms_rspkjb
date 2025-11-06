using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Common
{
    public class AppointmentRequestMCU
    {
        public string No { get; set; }
        public string IsNewPatient { get; set; }
        public string MedicalNo { get; set; }
        public string PatientName { get; set; }
        public string CityOfBirth { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Religion { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNo { get; set; }
        public string MobilePhoneNo { get; set; }
        public string EmailAddress { get; set; }
        public string IdentityCardType { get; set; }
        public string IdentityCardNo { get; set; }
        public int MCUItemID { get; set; }
        public string MCUItem { get; set; }
        public string BusinessPartnerCode { get; set; }
        public string AppointmentDate { get; set; }
        public string CorporateAccountNo { get; set; }
        public string CorporateAccountName { get; set; }
        public string CorporateAccountDepartment { get; set; }
        public bool IsHasMedicalRecord { get; set; }
        public string PatientIdentity
        {
            get
            {
                string result = string.Empty;

                if (!string.IsNullOrEmpty(MedicalNo))
                {
                    result = string.Format(@"{2}{1}({0})", MedicalNo, Environment.NewLine, PatientName);
                }
                else
                {
                    result = PatientName;
                }

                return result;
            }
        }
        public string DateOfBirthInString
        {
            get
            {
                int year = Convert.ToInt32(DateOfBirth.Substring(0, 4));
                int month = Convert.ToInt32(DateOfBirth.Substring(4, 2));
                int day = Convert.ToInt32(DateOfBirth.Substring(6, 2));

                DateTime date = new DateTime(year, month, day, 0, 0, 0);
                return date.ToString(Constant.FormatString.DATE_FORMAT);
            }
        }
        public string CorporateAccount
        {
            get
            {
                return string.Format(@"{0}{1}{2}", CorporateAccountNo, Environment.NewLine, CorporateAccountName);
            }
        }
        public string IdentityCard
        {
            get
            {
                string result = string.Empty;
                result = string.Format(@"{0}{1}{2}", !string.IsNullOrEmpty(IdentityCardType) ? IdentityCardType : string.Empty, Environment.NewLine, !string.IsNullOrEmpty(IdentityCardNo) ? IdentityCardNo : string.Empty);
                return result;
            }
        }
        public string MCUItemDetail
        {
            get
            {
                string result = string.Empty;

                ItemMaster entity = BusinessLayer.GetItemMasterList(string.Format("ItemCode = '{0}' AND IsDeleted = 0", MCUItem)).FirstOrDefault();
                if (entity != null)
                {
                    result = string.Format(@"({0}){1}{2}", entity.ItemCode, Environment.NewLine, entity.ItemName1);
                }

                return result;
            }
        }
        public string AppointmentDateInString
        {
            get
            {
                int year = Convert.ToInt32(AppointmentDate.Substring(0, 4));
                int month = Convert.ToInt32(AppointmentDate.Substring(4, 2));
                int day = Convert.ToInt32(AppointmentDate.Substring(6, 2));

                DateTime date = new DateTime(year, month, day, 0, 0, 0);
                return date.ToString(Constant.FormatString.DATE_FORMAT);
            }
        }
    }

    public class ZendeskAppountmentConfirmUpload
    {
        public string TicketId { get; set; }
        public string RequesterName { get; set; }
        public string RequesterWANumber { get; set; }
        public string AppointmentReason { get; set; }
        public string NamaPasien { get; set; }
        public string OPA { get; set; } 

    }
}
