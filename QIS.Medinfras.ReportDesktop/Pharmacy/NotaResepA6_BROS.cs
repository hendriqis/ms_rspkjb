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
    public partial class NotaResepA6_BROS : BaseA6Rpt
    {
        private int isCompound = 0;
        public NotaResepA6_BROS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {

            GetPrescriptionOrderDtCustom entity = BusinessLayer.GetPrescriptionOrderDtCustomList(Convert.ToInt32(param[0])).FirstOrDefault();

            lblsubTitle.Text = string.Format("No. : {0}", entity.TransactionNo);

            lblRegistrationInfo.Text = string.Format("{0} / {1}", entity.RegistrationNo, entity.MedicalNo);
            lblPatientInfo.Text = entity.PatientName;
            lblDateofBirth.Text = entity.DateOfBirthInString;
            lblPharmacy.Text = entity.LocationName;
            lblServiceUnit.Text = string.Format("{0} {1}", entity.ServiceUnitName, entity.BedCode);
            lblBusinessPartner.Text = entity.BusinessPartnerName;
            lblPhysicianName.Text = entity.ParamedicName;
            lblUserName.Text = string.Format("Dibuat oleh : {0}", entity.ChargesUserName);
            lblResep.Text = entity.JenisResep;

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

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean IsCompound = Convert.ToBoolean(GetCurrentColumnValue("IsCompound"));
            if (!IsCompound)
            {
                e.Cancel = true;
            }
        }

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean IsRFlag = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag"));
            Boolean IsCompound = Convert.ToBoolean(GetCurrentColumnValue("IsCompound"));
            String ChargedQuantity = Convert.ToString(GetCurrentColumnValue("cfQtyItemUnit"));
            Decimal LineAmount = Convert.ToDecimal(GetCurrentColumnValue("LineAmount"));
            String TakenQty = Convert.ToString(GetCurrentColumnValue("TakenQty"));
            String DosingUnit = Convert.ToString(GetCurrentColumnValue("DosingUnit"));
            if (!IsCompound && IsRFlag)
            {
                qtyHeader.Text = ChargedQuantity;
                LineAmountHeader.Text = LineAmount.ToString("N2");
            }
            else
            {
                qtyHeader.Text = string.Format("{0} {1}", TakenQty, DosingUnit);
                LineAmountHeader.Text = "";
            }
        }
    }
}
