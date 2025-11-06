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
    public partial class BSuratIzinPulangRSCK : BaseCustomDailyPotraitRpt
    {
        public BSuratIzinPulangRSCK()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit9 entityCV = BusinessLayer.GetvConsultVisit9List(param[0])[0];

            lblNamaPasien.Text = entityCV.PatientName;
            lblNoRM.Text = entityCV.MedicalNo;
            lblNoReg.Text = entityCV.RegistrationNo;
            lblPenjaminBayar.Text = entityCV.BusinessPartnerName;
            lblTglPulang.Text = entityCV.DischargeDateInString;

            if (entityCV.DepartmentID == Constant.Facility.INPATIENT)
            {
                lblCaptionEnd.Text = "Pasien tersebut telah melunasi semua tagihannya selama di rawat inap, dengan demikian pasien tersebut diizinkan untuk pulang.";
            }
            else if (entityCV.DepartmentID == Constant.Facility.EMERGENCY)
            {
                lblCaptionEnd.Text = "Pasien tersebut telah melunasi semua tagihannya selama di rawat darurat, dengan demikian pasien tersebut diizinkan untuk pulang.";
            }
            else
            {
                lblCaptionEnd.Text = "Pasien tersebut telah melunasi semua tagihannya selama di rawat jalan, dengan demikian pasien tersebut diizinkan untuk pulang.";
            }


            lblPenanggungJawab.Text = appSession.UserFullName;

            base.InitializeReport(param);
        }

    }
}
