using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PrescriptionAlertConfirmCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnPrescriptionOrderID.Value = paramInfo[0];
            txtAlertRemarks.Text = string.Empty;
            SetControlProperties();
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            string lstRecordID = hdnSelectedID.Value;
            string lstRecordRemarks = hdnSelectedRemarks.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            result = ConfirmMedicationAlert(hdnPrescriptionOrderID.Value, lstRecordID, lstRecordRemarks);

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string ConfirmMedicationAlert(string prescriptionOrderID, string lstRecordID, string lstRecordRemarks)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);

            try
            {
                string filterExpression = string.Format("PrescriptionOrderID = {0} AND PrescriptionOrderDetailID IN ({1}) ORDER BY PrescriptionOrderDetailID", prescriptionOrderID, lstRecordID);
                List<PrescriptionOrderDt> lstDetail = BusinessLayer.GetPrescriptionOrderDtList(filterExpression, ctx);
                int i = 0;
                string[] remarksInfo = lstRecordRemarks.Split(',');
                foreach (PrescriptionOrderDt item in lstDetail)
                {
                    item.IsAlertConfirmed = true;
                    item.AlertConfirmedBy = AppSession.UserLogin.ParamedicID;
                    item.AlertConfirmedRemarks = remarksInfo[i];
                    orderDtDao.Update(item);
                    i += 1;
                }
                ctx.CommitTransaction();
                result = string.Format("process|1|");
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = string.Format("process|0|{0}", ex.Message);
            }
            return result;
        }

        private void SetControlProperties()
        {
            BindGridView();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            //if (e.Item.ItemType == ListViewItemType.DataItem)
            //{
            //    CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsProcessItem");
            //    //HtmlTextInput chkIsSelected = (CheckBox)e.Item.FindControl("chkIsProcessItem");
            //    vMedicationSchedule item = (vMedicationSchedule)e.Item.DataItem;
            //    if (item.GCMedicationStatus != Constant.MedicationStatus.OPEN)
            //    {
            //        chkIsSelected.Visible = false;
            //    }
            //}
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("PrescriptionOrderID = {0} AND OrderIsDeleted = 0 AND (IsAllergyAlert = 1 OR IsAdverseReactionAlert = 1 OR IsDuplicateTheraphyAlert = 1) AND IsAlertConfirmed = 0", hdnPrescriptionOrderID.Value);

            List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }
    }
}