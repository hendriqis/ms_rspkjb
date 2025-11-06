using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class KartuPasien9Rpt : BaseRpt
    {
   
        public KartuPasien9Rpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string medicalNo = param[0];
            string filterExpression = string.Format("MRN ='{0}'", medicalNo);
            Patient entity = BusinessLayer.GetPatientList(filterExpression).FirstOrDefault();
            Address entityAddress = BusinessLayer.GetAddressList(string.Format("AddressID = {0}", Convert.ToInt32(entity.HomeAddressID))).ToList().FirstOrDefault();
            string name = entity.FullName.ToLower();
            string text = UpperFirst(name);
            string medicalNumber = entity.MedicalNo ;
            string Address = entityAddress.StreetName + entityAddress.RT + entityAddress.RW;
            xrBarCode1.Text = entity.MedicalNo;
            lblPatientName.Text = text;
            lblAddress.Text = Address;
            lblMedicalNo.Text = medicalNumber;
        }

        //Regex Untuk kapitalisasi Karakter Pertama tiap kata
        public static string UpperFirst(string s)
        {
            return Regex.Replace(s, @"\b[a-z]+", delegate(Match match)
            {
                string v = match.ToString();
                return char.ToUpper(v[0]) + v.Substring(1);
            });
        }
    }
}
