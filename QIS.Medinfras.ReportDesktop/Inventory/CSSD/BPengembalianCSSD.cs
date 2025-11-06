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
    public partial class BPengembalianCSSD : BaseCustomDailyPotraitRpt
    {
        public BPengembalianCSSD()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vMDServiceRequestHd entityHd = BusinessLayer.GetvMDServiceRequestHdList(param[0])[0];
            lblPengirimCSSD.Text = string.Format("PENGIRIM : {0}", entityHd.SentByName);
            lblTipeService.Text = entityHd.ServiceType;
            lblTanggalPengiriman.Text = string.Format("TANGGAL : {0}", entityHd.SentDate.ToString(Constant.FormatString.DATE_FORMAT));
            lblJamPengiriman.Text = string.Format("JAM PENGIRIMAN : {0} WIB", entityHd.SentDate.ToString(Constant.FormatString.TIME_FORMAT));
            lblPetugasCSSD.Text = string.Format("PETUGAS CSSD : {0}", entityHd.ReceivedByName);
            lblPencucian.Text = entityHd.WashingMethod;
            lblKeterangan.Text = string.Format("Keterangan : {0}", entityHd.Remarks);
            lblPengemasan.Text = entityHd.PackagingType;
            lblSterilisasi.Text = entityHd.SterilitationType;
            lblJamQC.Text = string.Format("SELESAI : {0} WIB", entityHd.ControlledDate.ToString(Constant.FormatString.TIME_FORMAT));

            if (entityHd.ControlledDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
            {
                lblTanggalQC.Text = string.Format("TANGGAL : {0}", entityHd.ControlledDate.ToString(Constant.FormatString.DATE_FORMAT));
            }
            else
            {
                lblTanggalQC.Text = string.Format("TANGGAL : ____________");
            }

            lblPetugasQC.Text = string.Format("PETUGAS : {0}", entityHd.ControlledByName);

            if (entityHd.ConfirmedDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
            {
                lblTanggalConfirm.Text = string.Format("TANGGAL : {0}", entityHd.ConfirmedDate.ToString(Constant.FormatString.DATE_FORMAT));
            }
            else
            {
                lblTanggalConfirm.Text = string.Format("TANGGAL : ____________");
            }

            lblJamConfirm.Text = string.Format("JAM : {0} WIB", entityHd.ConfirmedDate.ToString(Constant.FormatString.TIME_FORMAT));
            lblPetugasConfirm.Text = string.Format("PETUGAS BAGIAN : {0}", entityHd.ConfirmedByName);
            base.InitializeReport(param);
        }

        private void bs_CurrentChanged(object sender, EventArgs e)
        {

        }

    }
}
