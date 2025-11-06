using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class SatuSehatIntegrationLogCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnID.Value = param;
            Registration entityRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(param));
            txtRegistrationNo.Text = entityRegistration.RegistrationNo;
            BindGridView(param);
        }

        public void BindGridView(string registrationID)
        {
            List<SatuSehatIntegrationLog> lstLog = BusinessLayer.GetSatuSehatIntegrationLogList(string.Format("RegistrationID = {0}", registrationID)).OrderByDescending(o => o.ID).ToList();
            grdView.DataSource = lstLog;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                SatuSehatIntegrationLog entity = e.Row.DataItem as SatuSehatIntegrationLog;
                TextBox txtMessageText = e.Row.FindControl("txtMessageText") as TextBox;
                TextBox txtRespose = e.Row.FindControl("txtRespose") as TextBox;

                txtMessageText.Text = entity.cfMessageText;
                txtRespose.Text = entity.cfResponseText;
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(hdnID.Value);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(hdnID.Value);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}