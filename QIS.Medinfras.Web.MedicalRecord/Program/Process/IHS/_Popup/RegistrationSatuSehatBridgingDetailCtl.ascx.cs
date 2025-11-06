using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class RegistrationSatuSehatBridgingDetailCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                hdnRegistrationID.Value = param;
                lstSatuSehatInformation = BusinessLayer.GetSatuSehatRegistrationInformation(Convert.ToInt32(hdnRegistrationID.Value));
                BindGridView(1, true, ref PageCount);
            }
        }

        public class ResourceList
        {
            public string ResourceID { get; set; }
            public string ResourceType { get; set; }
            public List<GetSatuSehatRegistrationInformation> lstDetail { get; set; }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (lstSatuSehatInformation.Count > 0)
            {
                txtRegistrationNo.Text = lstSatuSehatInformation.FirstOrDefault().RegistrationNo;

                if (isCountPageCount)
                {
                    int rowCount = lstSatuSehatInformation.GroupBy(g => g.EncounterID).Select(s => s.FirstOrDefault()).ToList().Count;
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
                }

                List<ResourceList> lstResource = new List<ResourceList>();

                #region Encounter
                ResourceList resourceEncounter = new ResourceList();
                resourceEncounter.ResourceID = "encounter";
                resourceEncounter.ResourceType = "Kunjungan (Encounter)";
                resourceEncounter.lstDetail = lstSatuSehatInformation.GroupBy(g => g.RegistrationID).Select(s => s.FirstOrDefault()).ToList();
                lstResource.Add(resourceEncounter);

                grdViewEncounter.DataSource = resourceEncounter.lstDetail;
                grdViewEncounter.DataBind();
                #endregion

                #region Condition
                ResourceList resourceCondition = new ResourceList();
                resourceCondition.ResourceID = "condition";
                resourceCondition.ResourceType = "Diagnosa (Condition)";
                resourceCondition.lstDetail = lstSatuSehatInformation.GroupBy(g => g.PatientDiagnosisID).Select(s => s.FirstOrDefault()).ToList();
                lstResource.Add(resourceCondition);

                grdViewCondition.DataSource = resourceCondition.lstDetail;
                grdViewCondition.DataBind();
                #endregion

                #region Observation
                ResourceList resourceObservation = new ResourceList();
                resourceObservation.ResourceID = "observation";
                resourceObservation.ResourceType = "Tanda Vital (Observation)";
                resourceObservation.lstDetail = lstSatuSehatInformation.GroupBy(g => g.VitalSignCode).Select(s => s.FirstOrDefault()).ToList();
                lstResource.Add(resourceObservation);

                grdViewObservation.DataSource = resourceObservation.lstDetail;
                grdViewObservation.DataBind();
                #endregion

                grdViewResourceType.DataSource = lstResource;
                grdViewResourceType.DataBind();
            }
        }

        protected void cbpViewResource_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewEncounter_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewCondition_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewObservation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public List<GetSatuSehatRegistrationInformation> lstSatuSehatInformation
        {
            get
            {
                if (Session["__lstSatuSehatInformation"] == null)
                    Session["__lstSatuSehatInformation"] = new List<GetSatuSehatRegistrationInformation>();

                return (List<GetSatuSehatRegistrationInformation>)Session["__lstSatuSehatInformation"];
            }
            set { Session["__lstSatuSehatInformation"] = value; }
        }
    }
}