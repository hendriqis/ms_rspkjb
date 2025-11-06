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
    public partial class BFormHasilMCUDt : DevExpress.XtraReports.UI.XtraReport
    {
        public BFormHasilMCUDt()
        {
            InitializeComponent();
        }

        public void InitializeReport(MCUFormResultFieldReporting oData)
        {
            List<MUCFormResultFieldJobDt> lstData = new List<MUCFormResultFieldJobDt>();

            if (!string.IsNullOrEmpty(oData.Col169) || !string.IsNullOrEmpty(oData.Col171) || !string.IsNullOrEmpty(oData.Col173) || !string.IsNullOrEmpty(oData.Col175) || !string.IsNullOrEmpty(oData.Col177)
               || !string.IsNullOrEmpty(oData.Col179) || !string.IsNullOrEmpty(oData.Col181) || !string.IsNullOrEmpty(oData.Col183) || !string.IsNullOrEmpty(oData.Col184) )
            {
                MUCFormResultFieldJobDt row = new MUCFormResultFieldJobDt();
                row.NamaPerusahan = oData.Col169;
                row.JenisPekerjaan = oData.Col171;
                row.FxFisik = oData.Col173;
                row.FxKimia = oData.Col175;
                row.FxBiologi = oData.Col177;
                row.FxPsikologi = oData.Col179;
                row.FxErgonomi = oData.Col181;
                row.TahunPekerjaan = oData.Col183;
                row.BlnPekerjaan = oData.Col184;
                lstData.Add(row); 
            }

            if (!string.IsNullOrEmpty(oData.Col170) || !string.IsNullOrEmpty(oData.Col172) || !string.IsNullOrEmpty(oData.Col174) || !string.IsNullOrEmpty(oData.Col176) || !string.IsNullOrEmpty(oData.Col178)
              || !string.IsNullOrEmpty(oData.Col180) || !string.IsNullOrEmpty(oData.Col182) || !string.IsNullOrEmpty(oData.Col185) || !string.IsNullOrEmpty(oData.Col186))
            {
                MUCFormResultFieldJobDt row = new MUCFormResultFieldJobDt();
                row.NamaPerusahan = oData.Col170;
                row.JenisPekerjaan = oData.Col172;
                row.FxFisik = oData.Col174;
                row.FxKimia = oData.Col176;
                row.FxBiologi = oData.Col178;
                row.FxPsikologi = oData.Col180;
                row.FxErgonomi = oData.Col182;
                row.TahunPekerjaan = oData.Col185;
                row.BlnPekerjaan = oData.Col186;
                lstData.Add(row);
            }
            this.DataSource = lstData;
            lstData = null;
        }
    }
}
