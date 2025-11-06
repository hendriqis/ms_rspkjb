using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class PatientVisitLogCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            if (paramInfo[0] != string.Empty && paramInfo[1] != string.Empty)
            {
                hdnMRN.Value = paramInfo[0];
                string[] period = paramInfo[1].Split(';');
                hdnFromDate.Value = period[0].Substring(6, 4) + period[0].Substring(3, 2) + period[0].Substring(0, 2);
                hdnToDate.Value = period[1].Substring(6, 4) + period[1].Substring(3, 2) + period[1].Substring(0, 2);

                BindGridView(1, true, ref PageCount);
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExp = string.Format("MRN = {0} AND CONVERT(VARCHAR(10),VisitDate,112) BETWEEN '{1}' AND '{2}'", hdnMRN.Value, hdnFromDate.Value, hdnToDate.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisitRowCount(filterExp);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vConsultVisit> lstVisit = BusinessLayer.GetvConsultVisitList(filterExp, Constant.GridViewPageSize.GRID_MASTER,pageIndex,"VisitTime");
            grdView.DataSource = lstVisit;
            grdView.DataBind();
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = string.Format("refresh|{0}", pageCount);
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}