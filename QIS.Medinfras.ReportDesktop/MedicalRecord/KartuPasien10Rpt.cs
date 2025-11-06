using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class KartuPasien10Rpt : BaseRpt
    {
        public KartuPasien10Rpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            this.Landscape = true;
            string medicalNo = param[0];
            string filterExpression = string.Format(" MRN ='{0}'", medicalNo);
            Patient entity = BusinessLayer.GetPatientList(filterExpression)[0];
            Address entityAddress = BusinessLayer.GetAddressList(string.Format("AddressID = {0}", entity.HomeAddressID))[0];

            lblMedicalNo.Text = entity.MedicalNo;

            //string textName = entity.Name;
            //string[] textLst = textName.Split(' ');
            //string name = "", nameText1 = "", nameText2 = "";

            //for (int i = 0; i < textLst.Length; i++)
            //{
            //    name = textLst[i];
            //    if (nameText1.Length <= 10)
            //    {
            //        if (nameText1 != "")
            //        {
            //            nameText1 += " ";
            //        }
            //        nameText1 += name;
            //    }

            //    if (nameText1.Length > 10)
            //    {
            //        if (nameText2.Length <= 10)
            //        {
            //            if (!nameText1.Contains(name))
            //            {
            //                if (nameText2 != "")
            //                {
            //                    nameText2 += " ";
            //                }

            //                nameText2 += name;
            //            }
            //        }
            //    }
            //}

            //lblPatientName.Text = nameText1;
            //lblPatientName2.Text = nameText2;
            lblPatientName.Text = entity.Name;
            //lblAddress.Text = entityAddress.StreetName;

                string StreetName = entityAddress.StreetName;
                if (entityAddress.StreetName.Length > 35)
                {
                    StreetName = entityAddress.StreetName.Substring(0, 35);

                    lblAddress.Text = string.Format("{0} ...", StreetName);
                }
                else
                {
                    lblAddress.Text = entityAddress.StreetName;
                }

            xrBarCode1.Text = entity.MedicalNo;
        }
    }
}
