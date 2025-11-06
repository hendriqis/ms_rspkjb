using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs
{
    public partial class PeriodeCtl : BaseUserControlCtl
    {
        public void InitializeControl()
        {
            List<StandardCode> listPeriode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID NOT BETWEEN '{0}^050' AND '{0}^060' AND IsDeleted = 0", Constant.StandardCode.REPORTING_PERIOD));
            Methods.SetComboBoxField<StandardCode>(cboPeriode, listPeriode, "StandardCodeName", "StandardCodeID");
            cboPeriode.SelectedIndex = 0;
        }

        public void GetPeriodDate(ref DateTime startDate, ref DateTime endDate)
        {
            int num = Convert.ToInt32(txtValueNum.Text);

            switch (cboPeriode.Value.ToString())
            {
                //Custom
                case "X106^090": startDate = Helper.GetDatePickerValue(txtValueDateFrom.Text); endDate = Helper.GetDatePickerValue(txtValueDateTo.Text); break;
                //Last n Years
                case "X106^010": startDate = DateTime.Today.AddYears(-num); break;
                //Last n Months
                case "X106^011": startDate = DateTime.Today.AddMonths(-num); break;
                //Last n Weeks
                case "X106^012": startDate = DateTime.Today.AddDays(-7 * num); break;
                //Last n Days
                case "X106^013": startDate = DateTime.Today.AddDays(-num); break;
                //Last Year
                case "X106^014": startDate = DateTime.Today.AddYears(-1); break;
                //Last Month
                case "X106^015": startDate = DateTime.Today.AddMonths(-1); break;
                //Last Week
                case "X106^016": startDate = DateTime.Today.AddDays(-7); break;
                //Yesterday
                case "X106^017": startDate = DateTime.Today.AddDays(-1); break;
            }
        }
    }
}