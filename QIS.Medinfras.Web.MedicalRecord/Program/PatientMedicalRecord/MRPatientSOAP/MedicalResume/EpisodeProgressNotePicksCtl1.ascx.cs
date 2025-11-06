using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class EpisodeProgressNotePicksCtl1 : BaseProcessPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;

            string[] paramInfo = param.Split('|');

            hdnPopupVisitID.Value = paramInfo[0];
            hdnPopupParamedicID.Value = paramInfo[1];

            BindGridView(1, true, ref PageCount);
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected void cbpPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }


        private string GetFilterExpression()
        {
            string filterExpression = string.Format("MRN = {0} AND VisitID = {1} AND GCPrescriptionType = '{2}' AND GCTransactionStatus NOT IN ('{3}','{4}')", AppSession.RegisteredPatient.MRN, hdnPopupVisitID.Value, Constant.PrescriptionType.DISCHARGE_PRESCRIPTION, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.VOID);
            return filterExpression;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            try
            {
                string filterExpression = string.Format("VisitID = {0} AND ParamedicID = {1} AND  GCPatientNoteType IN ('X011^011') AND SubjectiveText IS NOT NULL AND ObjectiveText IS NOT NULL ORDER BY NoteDate DESC, NoteTime DESC", hdnPopupVisitID.Value, hdnPopupParamedicID.Value);
                List<vPatientVisitNote1> lstEntity = BusinessLayer.GetvPatientVisitNote1List(filterExpression);

                lvwPopupView.DataSource = lstEntity;
                lvwPopupView.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        protected void lvwPopupView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            string[] resultTextInfo = hdnSelectedItem.Value.Split('|');

            StringBuilder tmpText = new StringBuilder();
            foreach (string text in resultTextInfo)
            {
                if (tmpText.ToString() != string.Empty)
                    tmpText.AppendLine();

                string[] textInfo = text.Split(';');
                tmpText.AppendLine(string.Format("{0},{1}", textInfo[0], textInfo[1]));
                if (textInfo.Length >= 3)
                {
                    if (!string.IsNullOrEmpty(textInfo[2]))
                    {
                        tmpText.Append("S:");
                        tmpText.AppendLine(textInfo[2]);
                    } 
                }
                if (textInfo.Length >= 4)
                {
                    if (!string.IsNullOrEmpty(textInfo[3]))
                    {
                        tmpText.AppendLine("O:");
                        tmpText.AppendLine(textInfo[3]);
                    }  
                }    
            }
            retval = tmpText.ToString();
            return result;
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}