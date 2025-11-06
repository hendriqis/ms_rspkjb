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
using System.Data;
using QIS.Medinfras.Web.CommonLibs.Program;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Nursing.Program
{
    public partial class NursingJournalInfoLink : BasePageTrx
    {
        protected int PageCount = 1;
        protected bool IsEditable = true;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nursing.INFO_NURSING_JOURNAL;
        }

        protected override void InitializeDataControl()
        {
            BindGridView();
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            if (txtRegistrationNo.Text != String.Empty)
                filterExpression = string.Format("LinkField LIKE '{0}%' AND IsDeleted = 0 AND ISNULL(NursingTransactionInterventionDtID,0) = 0", txtRegistrationNo.Text);
            else
                filterExpression = "1=0";
            return filterExpression;
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();
            List<vNursingJournal> lstEntity = BusinessLayer.GetvNursingJournalList(filterExpression);
            
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        #region Process
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpHeader_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                LoadHeader();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            
        }
        #endregion

        private void LoadHeader()
        {
            vInpatientPatientListLink entity = BusinessLayer.GetvInpatientPatientListLinkList(String.Format("RegistrationNo = '{0}'", txtRegistrationNo.Text)).FirstOrDefault();
            if (entity != null)
            {
                ctlPatientBanner.InitializePatientBanner(entity);
                hdnVisitID.Value = entity.VisitID.ToString();
            }
            else
            {
                ctlPatientBanner.InitializeEmptyInpatientPatientBanner();
                hdnVisitID.Value = "0";
            }
        }
    }
}