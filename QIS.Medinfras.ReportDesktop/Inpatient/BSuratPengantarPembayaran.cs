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
    public partial class BSuratPengantarPembayaran : BaseRpt
    {
        public BSuratPengantarPembayaran()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            if (oHealthcare != null)
            {
                xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
                cHealthcareName.Text = oHealthcare.HealthcareName;
                cHealthcareAddress.Text = oHealthcare.StreetName;
                cHealthcareCityZipCodes.Text = string.Format("{0} {1}", oHealthcare.City, oHealthcare.ZipCode);
                cHealthcarePhone.Text = oHealthcare.PhoneNo1;
                cHealthcareFax.Text = oHealthcare.FaxNo1;
            }
            #endregion

            #region Report : Title
            if (param[2] == "pl")
            {
                lblReportTitle1.Text = "SURAT PENGANTAR PEMBAYARAN";
            }
            else if (param[2] == "pdl")
            {
                lblReportTitle1.Text = "SURAT PENGANTAR PEMBAYARAN SELISIH";
            }
            #endregion

            #region Detail : PaymentLetter
            vRegistration entityR = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            List<TempData> lstTemp = new List<TempData>();
            TempData newEntity = new TempData();

            newEntity.RegistrationNo = entityR.RegistrationNo;
            newEntity.PatientName = entityR.PatientName;
            newEntity.ServiceUnitName = entityR.ServiceUnitName;
            if (entityR.RoomID != 0)
            {
                if (entityR.BedID != 0)
                {
                    newEntity.RoomName = entityR.RoomName + " | " + entityR.BedCode;
                }
                else
                {
                    newEntity.RoomName = entityR.RoomName;
                }
            }
            else
            {
                newEntity.RoomName = "";
            }
            newEntity.Address = entityR.StreetName;
            newEntity.PhoneNo = entityR.PhoneNo1;
            newEntity.City = entityR.City;
            decimal PaymentLetterAmount = Convert.ToDecimal(param[1]);
            newEntity.PaymentLetterAmount = string.Format("Rp {0}", PaymentLetterAmount.ToString(Constant.FormatString.NUMERIC_2));
            newEntity.Date = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
            newEntity.CityTTD = oHealthcare.City;
            newEntity.PrintBy = appSession.UserFullName;
            newEntity.PrintDateTime = DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss");

            lstTemp.Add(newEntity);

            this.DataSource = lstTemp;
            base.InitializeReport(param);
        }

        private class TempData
        {
            private string _RegistrationNo;

            public string RegistrationNo
            {
                get { return _RegistrationNo; }
                set { _RegistrationNo = value; }
            }
            
            private string _PatientName;

            public string PatientName
            {
                get { return _PatientName; }
                set { _PatientName = value; }
            }

            private String _ServiceUnitName;

            public String ServiceUnitName
            {
                get { return _ServiceUnitName; }
                set { _ServiceUnitName = value; }
            }

            private String _RoomName;

            public String RoomName
            {
                get { return _RoomName; }
                set { _RoomName = value; }
            }

            private String _Address;

            public String Address
            {
                get { return _Address; }
                set { _Address = value; }
            }

            private String _PhoneNo;

            public String PhoneNo
            {
                get { return _PhoneNo; }
                set { _PhoneNo = value; }
            }

            private String _City;

            public String City
            {
                get { return _City; }
                set { _City = value; }
            }

            private String _PaymentLetterAmount;

            public String PaymentLetterAmount
            {
                get { return _PaymentLetterAmount; }
                set { _PaymentLetterAmount = value; }
            }

            private String _Date;

            public String Date
            {
                get { return _Date; }
                set { _Date = value; }
            }

            private String _CityTTD;

            public String CityTTD
            {
                get { return _CityTTD; }
                set { _CityTTD = value; }
            }

            private String _PrintDateTime;

            public String PrintDateTime
            {
                get { return _PrintDateTime; }
                set { _PrintDateTime = value; }
            }

            private String _PrintBy;

            public String PrintBy
            {
                get { return _PrintBy; }
                set { _PrintBy = value; }
            }
            #endregion
        }
    }
}
