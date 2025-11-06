using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPermintaanBarang_RSDOSKA : BaseDailyPortraitRpt
    {
        public BPermintaanBarang_RSDOSKA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vItemRequestHd entity = BusinessLayer.GetvItemRequestHdList(param[0])[0];
            lblRequestNo.Text = entity.ItemRequestNo;
            lblRequestDate.Text = entity.TransactionDateInString;
            lblLocationFrom.Text = entity.FromLocationCode + " | " + entity.FromLocationName;
            lblLocationTo.Text = entity.ToLocationCode + " | " + entity.ToLocationName;
            lblRemaks.Text = entity.Remarks;
            if (entity.RegistrationID != null)
            {
                lblRegistration.Text = string.Format("{0} | {1} | ({2}) {3}", entity.RegistrationNo, entity.ServiceUnitName, entity.MedicalNo, entity.PatientName);
            }
            else
            {
                lblRegistration.Text = "-";
            }
            
            lblCreatedByName.Text = entity.CreatedByName;

            base.InitializeReport(param);
        }

    }
}
