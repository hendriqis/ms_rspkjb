using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class CDCGrowthChartCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            Patient entity = BusinessLayer.GetPatient(AppSession.RegisteredPatient.MRN);
            if (entity.GCGender == Constant.Gender.MALE)
                hdnGender.Value = "1";
            else
                hdnGender.Value = "2";

            ddlChartType.Items.Add(new ListItem { Text = "Weight-for-age", Value = "W" });
            ddlChartType.Items.Add(new ListItem { Text = "Length-for-age", Value = "H" });
            ddlChartType.Items.Add(new ListItem { Text = "Head Circumference", Value = "C" });

            ddlChartType.SelectedIndex = 0;
        }
    }
}