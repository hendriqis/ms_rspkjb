using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using System.Text;
using System.Globalization;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class LaboratoryHistory : BasePage
    {
        private class LaboratoryResultInfo
        {
            public string ItemName { get; set; }
            public string FractionName { get; set; }
            public string Ref_Range { get; set; }
            public string Unit { get; set; }
            public DateTime ResultDate1 { get; set; }
            public string ResultTime1 { get; set; }
            public string ResultValue1 { get; set; }
            public string ResultFlag1 { get; set; }
            public DateTime ResultDate2 { get; set; }
            public string ResultTime2 { get; set; }
            public string ResultValue2 { get; set; }
            public string ResultFlag2 { get; set; }
            public DateTime ResultDate3 { get; set; }
            public string ResultTime3 { get; set; }
            public string ResultValue3 { get; set; }
            public string ResultFlag3 { get; set; }
            public DateTime ResultDate4 { get; set; }
            public string ResultTime4 { get; set; }
            public string ResultValue4 { get; set; }
            public string ResultFlag4 { get; set; }
            public DateTime ResultDate5 { get; set; }
            public string ResultTime5 { get; set; }
            public string ResultValue5 { get; set; }
            public string ResultFlag5 { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                List<PatientLaboratoryHistory> lst = BusinessLayer.GetPatientLaboratoryHistoryList(AppSession.RegisteredPatient.MRN);
                ConvertToDisplayModel(lst);
            }
        }

        private void ConvertToDisplayModel(List<PatientLaboratoryHistory> lst)
        {
            List<LaboratoryResultInfo> list = new List<LaboratoryResultInfo>();

            foreach (PatientLaboratoryHistory item in lst)
            {
                LaboratoryResultInfo oHistory = new LaboratoryResultInfo();
                oHistory.ItemName = item.ItemName;
                oHistory.FractionName = item.FractionName;
                oHistory.Ref_Range = item.Ref_Range;
                oHistory.Unit = item.MetricUnit;
                string[] resultInfo = item.ResultInfo.Split('|');
                int index = 1;
                foreach (string resultData in resultInfo)
                {
                    if (!string.IsNullOrEmpty(resultData))
                    {
                        string[] result = resultData.Split(';');
                        switch (index)
                        {
                            case 1:
                                oHistory.ResultDate1 = DateTime.ParseExact(result[0], Constant.FormatString.DATE_FORMAT_112, CultureInfo.InvariantCulture, DateTimeStyles.None);
                                oHistory.ResultTime1 = result[1];
                                oHistory.ResultValue1 = result[2];
                                oHistory.ResultFlag1 = result[3];
                                break;
                            case 2:
                                oHistory.ResultDate2 = DateTime.ParseExact(result[0], Constant.FormatString.DATE_FORMAT_112, CultureInfo.InvariantCulture, DateTimeStyles.None);
                                oHistory.ResultTime2 = result[1];
                                oHistory.ResultValue2 = result[2];
                                oHistory.ResultFlag2 = result[3];
                                break;
                            case 3:
                                oHistory.ResultDate3 = DateTime.ParseExact(result[0], Constant.FormatString.DATE_FORMAT_112, CultureInfo.InvariantCulture, DateTimeStyles.None);
                                oHistory.ResultTime3 = result[1];
                                oHistory.ResultValue3 = result[2];
                                oHistory.ResultFlag3 = result[3];
                                break;
                            case 4:
                                oHistory.ResultDate4 = DateTime.ParseExact(result[0], Constant.FormatString.DATE_FORMAT_112, CultureInfo.InvariantCulture, DateTimeStyles.None);
                                oHistory.ResultTime4 = result[1];
                                oHistory.ResultValue4 = result[2];
                                oHistory.ResultFlag4 = result[3];
                                break;
                            case 5:
                                oHistory.ResultDate5 = DateTime.ParseExact(result[0], Constant.FormatString.DATE_FORMAT_112, CultureInfo.InvariantCulture, DateTimeStyles.None);
                                oHistory.ResultTime5 = result[1];
                                oHistory.ResultValue5 = result[2];
                                oHistory.ResultFlag5 = result[3];
                                break;
                            default:
                                break;
                        }
                        list.Add(oHistory);
                    }
                    index += 1;
                }
            }

            rptView.DataSource = list;
            rptView.DataBind();
        }
    }
}