using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Linq;
namespace QIS.Medinfras.ReportDesktop
{
    public partial class LDaftarTransaksiJasaMedis : BaseCustom2DailyPotraitRpt
    {
        public LDaftarTransaksiJasaMedis()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
         {
             ParamedicMaster oParmedicName = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID='{0}'", param[1])).FirstOrDefault();
             if (oParmedicName != null) {
                 lblParamedicName.Text = oParmedicName.FullName;
             }

            base.InitializeReport(param);
        }

    }
}
