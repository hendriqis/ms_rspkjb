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
using System.Globalization;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class iCareInformationCtl : BasePage
    {
        protected int PageCount = 1;

        #region List

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

                List<SettingParameterDt> lstDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", "001", Constant.SettingParameter.SA0222));

                hdnIsBridgingToiCare.Value = lstDt.Where(w => w.ParameterCode == Constant.SettingParameter.SA0222).FirstOrDefault().ParameterValue;

                ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = '{0}'", AppSession.UserLogin.ParamedicID)).FirstOrDefault();
                if (entityParamedic != null)
                {
                    if (!string.IsNullOrEmpty(entityParamedic.BPJSReferenceInfo))
                    {
                        string[] bpjsRef = entityParamedic.BPJSReferenceInfo.Split(';');
                        string[] hfis = bpjsRef[1].Split('|');
                        txtPhysicianHFISCode.Text = hfis[0];
                        txtPhysicianHFISName.Text = hfis[1];
                        hdnPhysicianHFIS.Value = hfis[0];
                    }
                }

                BindGridView();
            }
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("MRN = {0}", AppSession.RegisteredPatient.MRN);

            List<vPatient> lstPatient = BusinessLayer.GetvPatientList(filterExpression);

            grdView.DataSource = lstPatient;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "icare")
                {
                    string nik = param[1];
                    string noKartu = param[2];
                    string hfis = hdnPhysicianHFIS.Value;
                    result = "icare|";
                    BPJSService oService = new BPJSService();
                    string resultInfo = oService.GetICareData(noKartu, nik, hfis);
                    string[] resultInfoArr = resultInfo.Split('|');
                    if (resultInfoArr[0] == "1")
                    {
                        result += "success|" + resultInfoArr[1];
                    }
                    else
                    {
                        result += "failed|" + resultInfoArr[2];
                    }
                }
                else
                {
                    result = "refresh";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #endregion
    }
}