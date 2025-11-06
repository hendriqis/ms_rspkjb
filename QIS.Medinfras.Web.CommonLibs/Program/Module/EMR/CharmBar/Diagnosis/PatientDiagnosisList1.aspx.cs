using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientDiagnosisList1 : BasePage
    {
        protected int PageCount = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGridView1(1, true, ref PageCount);
                BindGridView2();
                BindGridView3();
            }
        }

        private void BindGridView1(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            List<vPatientDiagnosis1> lstEntity = BusinessLayer.GetvPatientDiagnosis1List(filterExpression, int.MaxValue, 1, "GCDiagnoseType, ID");
            grdPatientDiagnosisNowView.DataSource = lstEntity;
            grdPatientDiagnosisNowView.DataBind();
        }

        private void BindGridView2()
        {
            List<PatientFinalDiagnosisSummary> lstEntity = BusinessLayer.GetPatientFinalDiagnosisSummaryList(AppSession.RegisteredPatient.MRN);
            grdDiagnosisSummaryFinalView.DataSource = lstEntity;
            grdDiagnosisSummaryFinalView.DataBind();
        }

        private void BindGridView3()
        {
            List<PatientClaimDiagnosisSummary> lstEntity = BusinessLayer.GetPatientClaimDiagnosisSummaryList(AppSession.RegisteredPatient.MRN);
            grdDiagnosisSummaryClaimView.DataSource = lstEntity;
            grdDiagnosisSummaryClaimView.DataBind();
        }

        protected void cbpPatientDiagnosisNowView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView1(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView1(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}