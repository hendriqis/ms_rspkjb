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
    public partial class BHonorDokterX : BaseDailyPortraitRpt
    {
        public BHonorDokterX()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            try
            {
                vTransRevenueSharingHd header = BusinessLayer.GetvTransRevenueSharingHdList(param[0])[0];
                lblCreatedByName.Text = header.CreatedByName;
            }
            catch (Exception ex)
            {
                lblCreatedByName.Text = string.Empty;
            }
            
            base.InitializeReport(param);
        }
    }
}
