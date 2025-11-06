using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BRingkasanMasukKeluarRSSK : BaseCustomDailyPotraitRpt
    {
        public BRingkasanMasukKeluarRSSK()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            Healthcare healthcare = BusinessLayer.GetHealthcare(appSession.HealthcareID);
            Address address = BusinessLayer.GetAddress(Convert.ToInt32(healthcare.AddressID));

            vMedicalRecordRSSK entity = BusinessLayer.GetvMedicalRecordRSSKList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();

            #region Procedure
            if (entity.PatientProcedure != null || entity.PatientProcedure != "")
            {
                lblProcedure.Text = entity.PatientProcedure;
            }
            else
            {
                lblProcedure.Text = "";
            }
            #endregion

            #region Detail
            if (entity.cfSehat)
            {
                chkSehat.Checked = entity.cfSehat;

                chkMembaik.Visible = false;
                chkBelumSembuh.Visible = false;
                chkMeninggalKD48Jam.Visible = false;
                chkMeninggalLD48Jam.Visible = false;
            }
            else
            {
                chkSehat.Visible = false;
            }

            if (entity.cfMembaik)
            {
                chkMembaik.Checked = entity.cfMembaik;

                chkSehat.Visible = false;
                chkBelumSembuh.Visible = false;
                chkMeninggalKD48Jam.Visible = false;
                chkMeninggalLD48Jam.Visible = false;
            }
            else
            {
                chkMembaik.Visible = false;
            }

            if (entity.cfBelumSembuh)
            {
                chkBelumSembuh.Checked = entity.cfBelumSembuh;

                chkMembaik.Visible = false;
                chkSehat.Visible = false;
                chkMeninggalKD48Jam.Visible = false;
                chkMeninggalLD48Jam.Visible = false;
            }
            else
            {
                chkBelumSembuh.Visible = false;
            }

            if (entity.cfMeninggalKD48jam)
            {
                chkMeninggalKD48Jam.Checked = entity.cfMeninggalKD48jam;

                chkSehat.Visible = false;
                chkMembaik.Visible = false;
                chkBelumSembuh.Visible = false;
                chkMeninggalLD48Jam.Visible = false;
            }
            else
            {
                chkMeninggalKD48Jam.Visible = false;
            }

            if (entity.cfAtasPersetujuan)
            {
                chkAtasPersetujuan.Checked = entity.cfAtasPersetujuan;

                chkTransferRajal.Visible = false;
                chkPulangPaksa.Visible = false;
                chkPindahRS.Visible = false;
                chkPulangAPS.Visible = false;
                chkLari.Visible = false;
                chkMasukRS.Visible = false;
                chkKamarJenazah.Visible = false;
                chkDOA.Visible = false;
            }
            else
            {
                chkAtasPersetujuan.Visible = false;
            }

            if (entity.cfDirujukRawatJalan)
            {
                chkTransferRajal.Checked = entity.cfDirujukRawatJalan;

                chkAtasPersetujuan.Visible = false;
                chkPulangPaksa.Visible = false;
                chkPindahRS.Visible = false;
                chkPulangAPS.Visible = false;
                chkLari.Visible = false;
                chkMasukRS.Visible = false;
                chkKamarJenazah.Visible = false;
                chkDOA.Visible = false;
            }
            else
            {
                chkTransferRajal.Visible = false;
            }

            if (entity.cfPulangPaksa)
            {
                chkPulangPaksa.Checked = entity.cfPulangPaksa;

                chkAtasPersetujuan.Visible = false;
                chkTransferRajal.Visible = false;
                chkPindahRS.Visible = false;
                chkPulangAPS.Visible = false;
                chkLari.Visible = false;
                chkMasukRS.Visible = false;
                chkKamarJenazah.Visible = false;
                chkDOA.Visible = false;
            }
            else
            {
                chkPulangPaksa.Visible = false;
            }

            if (entity.cfPindahRS)
            {
                chkPindahRS.Checked = entity.cfPindahRS;

                chkAtasPersetujuan.Visible = false;
                chkTransferRajal.Visible = false;
                chkPulangPaksa.Visible = false;
                chkPulangAPS.Visible = false;
                chkLari.Visible = false;
                chkMasukRS.Visible = false;
                chkKamarJenazah.Visible = false;
                chkDOA.Visible = false;
            }
            else
            {
                chkPindahRS.Visible = false;
            }

            if (entity.cfPulangAPS)
            {
                chkPulangAPS.Checked = entity.cfPulangAPS;

                chkAtasPersetujuan.Visible = false;
                chkTransferRajal.Visible = false;
                chkPulangPaksa.Visible = false;
                chkPindahRS.Visible = false;
                chkLari.Visible = false;
                chkMasukRS.Visible = false;
                chkKamarJenazah.Visible = false;
                chkDOA.Visible = false;
            }
            else
            {
                chkPulangAPS.Visible = false;
            }

            if (entity.cfLari)
            {
                chkLari.Checked = entity.cfLari;

                chkAtasPersetujuan.Visible = false;
                chkTransferRajal.Visible = false;
                chkPulangPaksa.Visible = false;
                chkPindahRS.Visible = false;
                chkPulangAPS.Visible = false;
                chkMasukRS.Visible = false;
                chkKamarJenazah.Visible = false;
                chkDOA.Visible = false;
            }
            else
            {
                chkLari.Visible = false;
            }

            if (entity.cfMasukRS)
            {
                chkMasukRS.Checked = entity.cfMasukRS;

                chkAtasPersetujuan.Visible = false;
                chkTransferRajal.Visible = false;
                chkPulangPaksa.Visible = false;
                chkPindahRS.Visible = false;
                chkPulangAPS.Visible = false;
                chkLari.Visible = false;
                chkKamarJenazah.Visible = false;
                chkDOA.Visible = false;
            }
            else
            {
                chkMasukRS.Visible = false;
            }

            if (entity.cfKamarJenazah)
            {
                chkKamarJenazah.Checked = entity.cfKamarJenazah;

                chkAtasPersetujuan.Visible = false;
                chkTransferRajal.Visible = false;
                chkPulangPaksa.Visible = false;
                chkPindahRS.Visible = false;
                chkPulangAPS.Visible = false;
                chkLari.Visible = false;
                chkMasukRS.Visible = false;
                chkDOA.Visible = false;
            }
            else
            {
                chkKamarJenazah.Visible = false;
            }

            if (entity.cfDOA)
            {
                chkDOA.Checked = entity.cfDOA;

                chkAtasPersetujuan.Visible = false;
                chkTransferRajal.Visible = false;
                chkPulangPaksa.Visible = false;
                chkPindahRS.Visible = false;
                chkPulangAPS.Visible = false;
                chkLari.Visible = false;
                chkMasukRS.Visible = false;
                chkKamarJenazah.Visible = false;
            }
            else
            {
                chkDOA.Visible = false;
            }
            #endregion

            base.InitializeReport(param);
        }

    }
}
