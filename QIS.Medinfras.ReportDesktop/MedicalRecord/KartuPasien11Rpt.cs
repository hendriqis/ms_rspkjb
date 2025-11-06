using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class KartuPasien11Rpt : BaseRpt
    {
        public KartuPasien11Rpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string filterExpression = string.Format(" MRN = '{0}'", param[0]);
            Patient entity = BusinessLayer.GetPatientList(filterExpression)[0];
            
            lblMedicalNo.Text = entity.MedicalNo;

            string nameCard = "";
            if (entity.Name == "" || entity.Name == null)
            {
                nameCard = entity.FullName;
            }
            else
            {
                nameCard = entity.Name;
            }

            string[] nameList = nameCard.Split(' ');
            string nameTemp = "", nameCardFix = "";

            for (int i = 0; i < nameList.Length; i++)
            {
                nameTemp = nameList[i];
                if (nameCardFix == "")
                {
                    nameCardFix = nameTemp;
                }
                else
                {
                    if (nameCardFix.Length < 30 && (nameCardFix.Length + nameTemp.Length) <= 30)
                    {
                        nameCardFix += " " + nameTemp;
                    }
                }
            }

            nameCard = nameCardFix;
            lblPatientName.Text = nameCard;

            lblDOB_Gender.Text = string.Format("{0} ({1})", entity.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT), entity.cfGenderInPL);

            barcodeMedicalNo.Text = entity.MedicalNo;
        }
    }
}
