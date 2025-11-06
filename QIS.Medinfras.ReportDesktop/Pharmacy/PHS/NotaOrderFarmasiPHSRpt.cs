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
    public partial class NotaOrderFarmasiPHSRpt : BaseReceipt1Rpt
    {
        private int isCompound = 0;
        public NotaOrderFarmasiPHSRpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            GetPrescriptionOrderDtCustom entity = BusinessLayer.GetPrescriptionOrderDtCustomList(Convert.ToInt32(param[0])).FirstOrDefault();
            ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entity.ParamedicID)).FirstOrDefault();

            lblReportSubTitle.Text = string.Format("No. : {0}", entity.TransactionNo);
            lblPatientInfo.Text = entity.PatientName;
            lblRegistrationInfo.Text = string.Format("{0} / {1}", entity.RegistrationNo, entity.MedicalNo);
            lblBusinessPartnerName.Text = entity.BusinessPartnerName;
            lblTglLahir.Text = entity.DateOfBirthInString;
            lblPatientLocation.Text = string.Format("{0} {1}", entity.ServiceUnitName, entity.BedCode);
            lblPhysicianName.Text = entity.PrescriptionParamedicName;
            lblNoSIP.Text = entityParamedic.LicenseNo;
            lblServiceUnit.Text = entity.LocationName;
            lblPreception.Text = entity.JenisResep;
            lblUserName.Text = entity.ChargesUserName;

            List<GetPrescriptionOrderDtCustom> entityC = BusinessLayer.GetPrescriptionOrderDtCustomList(Convert.ToInt32(param[0]));
            foreach(GetPrescriptionOrderDtCustom e in entityC)
            {
                if (e.IsCompound)
                {
                    isCompound += 1;
                }
            }
            
            base.InitializeReport(param);
        }

        private void cIsRFlag_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;
        }

        private void lblNotesCaption_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (isCompound > 0)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
