using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRSurgeryReportRSSK : DevExpress.XtraReports.UI.XtraReport
    {
        public MRSurgeryReportRSSK()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int PatientSurgeryID)
        {
            vPatientSurgery entityPS = BusinessLayer.GetvPatientSurgeryList(string.Format("VisitID IN ({0}) AND PatientSurgeryID = '{1}' AND IsDeleted = 0", VisitID, PatientSurgeryID)).FirstOrDefault();
            lblTestOrderNo.Text = entityPS.TestOrderNo;
            lblReportDate.Text = entityPS.cfReportDate;
            lblReportTime.Text = entityPS.ReportTime;
            lblStartDate.Text = entityPS.cfStartDateInString;
            lblStartTime.Text = entityPS.StartTime;
            lblEndDate.Text = entityPS.cfEndDateNoHourInString;
            lblEndTime.Text = entityPS.EndTime;
            lblDuration.Text = string.Format("{0} menit", entityPS.Duration.ToString());
            if (entityPS.IsFirstSurgery)
            {
                lblOperasiKe.Text = "1 (Pertama)";
            }
            else
            {
                lblOperasiKe.Text = string.Format("Re-Do, {0}", entityPS.SurgeryNo);
            }
            lblPembiusan.Text = entityPS.AnesthesiaType;
            //lblPembedahan.Text = entityPS.WoundType;
            if (entityPS.IsUsingAntibiotics)
            {
                lblAntibioticsType.Text = string.Format("Ya, {0}", entityPS.AntibioticsType);
                lblAntibioticsTime.Text = entityPS.AntibioticsTime;
            }
            else
            {
                lblAntibioticsType.Text = "Tidak";
                lblAntibioticsTime.Visible = false;
                xrLabel28.Visible = false;
                xrLabel29.Visible = false;
            }
            if (entityPS.IsHasComplication)
            {
                lblKomplikasi.Text = string.Format("Ya, {0}", entityPS.ComplicationRemarks);
            }
            else
            {
                lblKomplikasi.Text = "Tidak";
            }
            if (entityPS.IsHasHemorrhage)
            {
                lblPerdarahan.Text = string.Format("Ya, {0} ml", entityPS.Hemorrhage);
            }
            else
            {
                lblPerdarahan.Text = "Tidak";
            }
            if (entityPS.IsBloodDrain)
            {
                lblDrain.Text = string.Format("Ya, {0}", entityPS.OtherBloodDrainType);
            }
            else
            {
                lblDrain.Text = "Tidak";
            }
            if (entityPS.IsUsingTampon)
            {
                lblTampon.Text = string.Format("Ya, {0}", entityPS.TamponType);
            }
            else
            {
                lblTampon.Text = "Tidak";
            }
            if (entityPS.IsBloodTransfussion)
            {
                lblTransfusi.Text = string.Format("Ya, {0} ml", entityPS.BloodTransfussion);
            }
            else
            {
                lblTransfusi.Text = "Tidak";
            }
            if (entityPS.IsTestKultur)
            {
                lblKultur.Text = string.Format("Ya, {0}", entityPS.OtherTestKulturType);
            }
            else
            {
                lblKultur.Text = "Tidak";
            }
            if (entityPS.IsTestCytology)
            {
                lblSitologi.Text = string.Format("Ya, {0}", entityPS.OtherTestCytologyType);
            }
            else
            {
                lblSitologi.Text = "Tidak";
            }
            if (entityPS.IsTestPA)
            {
                lblSpecimen.Text = "Ya";
                lblJenisSpesimen.Text = entityPS.SpecimenName;

                if (!string.IsNullOrEmpty(entityPS.OtherSpecimenType))
                {
                    lblOtherSpecimen.Text = entityPS.OtherSpecimenType;
                }
                else
                {
                    lblOtherSpecimen.Visible = false;
                }
            }
            else
            {
                lblSpecimen.Text = "Tidak";
                lblJenisSpesimen.Visible = false;
                xrLabel14.Visible = false;
                xrLabel9.Visible = false;
                lblOtherSpecimen.Visible = false;
            }
        }
    }
}
