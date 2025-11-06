using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Dashboard.Program
{
    public partial class Pivot : BasePageTrx
    {
        private const string STATUS_IMAGE_PATH = "~/libs/Images/Dashboard/";
        List<vConsultVisit9> lstData;
        List<vConsultVisit9> lstDataInpatient;
        List<vConsultVisit9> lstDataOutpatient;
        List<vConsultVisit9> lstDataIGD;
        List<vConsultVisit9> lstDataTotal;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Dashboard.RekamMedis1;
        }
        protected override void InitializeDataControl()
        {
            //string filterExpressionInpatient = string.Format("DepartmentID = '{0}' AND GCVisitStatus != '{1}' AND MONTH(VisitDate) = '{2}' AND YEAR(VisitDate) = '{3}'", Constant.Facility.INPATIENT, Constant.VisitStatus.CANCELLED, DateTime.Now.Month, DateTime.Now.Year);
            //string filterExpressionOutpatient = string.Format("DepartmentID = '{0}' AND GCVisitStatus != '{1}' AND MONTH(VisitDate) = '{2}' AND YEAR(VisitDate) = '{3}'", Constant.Facility.OUTPATIENT, Constant.VisitStatus.CANCELLED, DateTime.Now.Month, DateTime.Now.Year);
            //string filterExpressionIGD = string.Format("DepartmentID = '{0}' AND GCVisitStatus != '{1}' AND MONTH(VisitDate) = '{2}' AND YEAR(VisitDate) = '{3}'", Constant.Facility.EMERGENCY, Constant.VisitStatus.CANCELLED, DateTime.Now.Month, DateTime.Now.Year);
            //string filterExpressionTotal = string.Format("DepartmentID IN ('{0}','{1}','{2}') AND GCVisitStatus != '{3}' AND MONTH(VisitDate) = '{4}' AND YEAR(VisitDate) = '{5}'", Constant.Facility.INPATIENT, Constant.Facility.OUTPATIENT, Constant.Facility.EMERGENCY, Constant.VisitStatus.CANCELLED, DateTime.Now.Month, DateTime.Now.Year);
            //string filterExpression = string.Format("GCVisitStatus != '{0}' AND MONTH(VisitDate) = '{1}' AND YEAR(VisitDate) = '{2}'", Constant.VisitStatus.CANCELLED, DateTime.Now.Month, DateTime.Now.Year);
            //lstData = BusinessLayer.GetvConsultVisit9List(filterExpression);
            //List<Department> lst = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration=1 and IsActive=1"));
            //Methods.SetComboBoxField<Department>(cboDepartment, lst, "DepartmentName", "DepartmentID");
            //cboDepartment.SelectedIndex = 0;
            //lblDateTime.InnerText = DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT);
            //lstDataInpatient = BusinessLayer.GetvConsultVisit9List(filterExpressionInpatient);
            //lstDataOutpatient = BusinessLayer.GetvConsultVisit9List(filterExpressionOutpatient);
            //lstDataIGD = BusinessLayer.GetvConsultVisit9List(filterExpressionIGD);
            //lstDataTotal = BusinessLayer.GetvConsultVisit9List(filterExpressionTotal);

            //#region patient
            //var resultinpatient = lstDataInpatient.GroupBy(inpatient => inpatient.MRN).Select(grp => grp.First()).ToList().OrderBy(x => x.MRN);
            //var resultoutpatient = lstDataOutpatient.GroupBy(outpatient => outpatient.MRN).Select(grp => grp.First()).ToList().OrderBy(x => x.MRN);
            //var resultigd = lstDataIGD.GroupBy(igd => igd.MRN).Select(grp => grp.First()).ToList().OrderBy(x => x.MRN);
            //var resulttotal = lstDataTotal.GroupBy(total => total.MRN).Select(grp => grp.First()).ToList().OrderBy(x => x.MRN);
            ////lblInpatientVisitCount.InnerText = string.Format("{0}", resultinpatient.Count());
            ////lblOutpatientVisitCount.InnerText = string.Format("{0}", resultoutpatient.Count());
            ////lblIGDVisitCount.InnerText = string.Format("{0}", resultigd.Count());
            ////lblTotalVisitCount.InnerText = string.Format("{0}", resulttotal.Count());
            ////lblHeader.InnerText = string.Format("Hello, {0}", AppSession.UserLogin.ParamedicName);
            ////imgInpatient.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "user.png");
            ////imgOutpatient.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "user.png");
            ////imgIGD.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "user.png");
            ////imgTotal.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "user.png");
            //#endregion

            //  BindGridView();
            ///  BindGridViewPieChart();
            // BindGridDetail();  
            string filterEkspresion = "";
            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterEkspresion);
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboPivot, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboPivot.SelectedIndex = 0;

            string date = "2021-01-01";
            DateTime date1 = Convert.ToDateTime(date);
            List<PatientCencusInformation> lstEntity = BusinessLayer.GetCencusInformationList(date1, 0);
            if (lstEntity.Count > 0)
            {
                Pivot1.Value = JsonConvert.SerializeObject(lstEntity, Formatting.None);
            }
        }

        protected void cboPivot_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterEkspresion = "";
            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterEkspresion);
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboPivot, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboPivot.SelectedIndex = -1;
        }

    }
}