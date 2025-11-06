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
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.Program;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InformationResultLabDetailCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        private InformationResultLab DetailPage
        {
            get { return (InformationResultLab)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnTransactionID.Value = param;
            string filterExpression = string.Format("TransactionID = {0}", hdnTransactionID.Value);
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
            LaboratoryResultHd entityResult = BusinessLayer.GetLaboratoryResultHdList(string.Format("ChargeTransactionID = {0}", hdnTransactionID.Value)).FirstOrDefault();

            if (entity != null)
            {
                txtTransactionNo.Text = entity.TransactionNo;
                txtTransactionDate.Text = entity.TransactionDateInString;
                txtTransactionTime.Text = entity.TransactionTime;
                txtMedicalNo.Text = entity.MedicalNo;
                txtPatientName.Text = entity.PatientName;
                txtOrderPhysician.Text = string.Format("{0}", entity.TestOrderPhysician);

                if (entityResult != null)
                {
                    txtResultDate.Text = entityResult.ResultDate.ToString(Constant.FormatString.DATE_FORMAT);
                    txtResultTime.Text = entityResult.ResultTime;
                }
                else
                {
                    txtResultDate.Enabled = false;
                    txtResultTime.Enabled = false;
                }
            }

            BindGridView(1, true, ref PageCount);

        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            List<vPatientChargesDt> lstDetail = BusinessLayer.GetvPatientChargesDtList(filterExpression);
            grdPopupView.DataSource = lstDetail;
            grdPopupView.DataBind();

            ////if (hdnLabResultID.Value == "")
            ////    hdnLabResultID.Value = "0";
            string orderBy = "FractionDisplayOrder";
            List<vLaboratoryResultDt> lstLabEntity = BusinessLayer.GetvLaboratoryResultDtList(string.Format("ChargeTransactionID = {0} AND IsDeleted = 0 ORDER BY {1}", hdnTransactionID.Value, orderBy));

            grdView2.DataSource = lstLabEntity;
            grdView2.DataBind();
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