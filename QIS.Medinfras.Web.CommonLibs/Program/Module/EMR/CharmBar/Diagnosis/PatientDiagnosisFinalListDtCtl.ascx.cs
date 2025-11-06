using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using Newtonsoft.Json;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program.PatientPage
{
    public partial class PatientDiagnosisFinalListDtCtl : BaseContentPopupCtl
    {
        protected int PageCountDt = 1;

        public override void InitializeControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnDiagnosisIDCBCtl.Value = paramInfo[0];

            txtMedicalNoCBCtl.Text = AppSession.RegisteredPatient.MedicalNo;
            txtPatientNameCBCtl.Text = AppSession.RegisteredPatient.PatientName;
            txtDiagnosisTextCBCtl.Text = string.Format("({1}) {0}", paramInfo[1], paramInfo[0]);

            BindGridView1(1, true, ref PageCountDt);
        }

        private void BindGridView1(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND FinalDiagnosisID = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, hdnDiagnosisIDCBCtl.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDiagnosisRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_FIVE);
            }

            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression, Constant.GridViewPageSize.GRID_FIVE, pageIndex, "GCDiagnoseType, ID");
            grdDiagnosisFinalPopupDtView.DataSource = lstEntity;
            grdDiagnosisFinalPopupDtView.DataBind();
        }

        protected void cbpDiagnosisFinalPopupDtView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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