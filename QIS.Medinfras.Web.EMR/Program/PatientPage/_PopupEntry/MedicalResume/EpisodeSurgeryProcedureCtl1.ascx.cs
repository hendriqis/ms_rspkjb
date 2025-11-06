using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using Newtonsoft.Json;
using QIS.Medinfras.Web.CommonLibs.Service;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class EpisodeSurgeryProcedureCtl1 : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnPopupVisitID.Value = paramInfo[0];
            BindGridView();
        }

        private void BindGridView()
        {
            string transactionCode = string.Empty;
            List<EpisodeSurgeryProcedure> lstDetail = BusinessLayer.GetEpisodeSurgeryProcedureList(Convert.ToInt32(hdnPopupVisitID.Value));
            grdView.DataSource = lstDetail;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            BindGridView();
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            string title = "Prosedur/Tindakan Kamar Operasi :";
            string medicationLineText = hdnSelectedItem.Value.Replace("|", Environment.NewLine);
            retval = string.Format("{0}{1}{2}",title,Environment.NewLine,medicationLineText);
            return result;
        }
    }
}