using System.Collections.Generic;
using System.Linq;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ConsultVisitItemPackageBalanceCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] tempParam = param.Split('|');

            hdnRegistrationIDCtlPU.Value = tempParam[0];
            hdnVisitIDCtlPU.Value = tempParam[1];

            string filterExpression = string.Format("RegistrationID = {0} AND VisitID = {1} ORDER BY DtDepartmentID DESC, DtServiceUnitCode, DtItemType, DtItemCode", hdnRegistrationIDCtlPU.Value, hdnVisitIDCtlPU.Value);
            List<vConsultVisitItemPackageBalance> lstEntity = BusinessLayer.GetvConsultVisitItemPackageBalanceList(filterExpression);
            if (lstEntity.Count() > 0)
            {
                vConsultVisitItemPackageBalance entity = lstEntity.FirstOrDefault();
                txtRegistrationNoCVIPBalanceCtl.Text = entity.RegistrationNo;
                txtItemCode.Text = entity.ItemCode;
                txtItemName1.Text = entity.ItemName1;
            }
            else
            {
                vConsultVisitItemPackage cvip = BusinessLayer.GetvConsultVisitItemPackageList(filterExpression).FirstOrDefault();
                if (cvip != null)
                {
                    txtRegistrationNoCVIPBalanceCtl.Text = cvip.RegistrationNo;
                    txtItemCode.Text = cvip.ItemCode;
                    txtItemName1.Text = cvip.ItemName1;
                }
            }

            BindGridViewAIOQty(lstEntity);
            BindGridViewAIOAmount(lstEntity);
        }

        #region AIO Qty

        private void BindGridViewAIOQty(List<vConsultVisitItemPackageBalance> lstEntity)
        {
            lvwViewAIOQty.DataSource = lstEntity.Where(a => !a.IsBalanceTariff).ToList();
            lvwViewAIOQty.DataBind();
        }

        private void BindGridViewAIOQtyMovement()
        {
            if (hdnExpandIDAIOQty.Value != null && hdnExpandIDAIOQty.Value != "" && hdnExpandIDAIOQty.Value != "0")
            {
                List<vConsultVisitItemPackageMovement> lstMvm = BusinessLayer.GetvConsultVisitItemPackageMovementList(string.Format("DtID IN ({0})", hdnExpandIDAIOQty.Value), int.MaxValue, 1, "MovementID ASC");
                lvwMovementAIOQty.DataSource = lstMvm;
                lvwMovementAIOQty.DataBind();
            }
        }

        protected void cbpViewAIOQtyMovement_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "refresh")
                {
                    BindGridViewAIOQtyMovement();
                    result = "refreshAIOQtyMovement";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #endregion

        #region AIO Amount

        private void BindGridViewAIOAmount(List<vConsultVisitItemPackageBalance> lstEntity)
        {
            lvwViewAIOAmount.DataSource = lstEntity.Where(a => a.IsBalanceTariff).ToList();
            lvwViewAIOAmount.DataBind();
        }

        private void BindGridViewAIOAmountMovement()
        {
            if (hdnExpandIDAIOAmount.Value != null && hdnExpandIDAIOAmount.Value != "" && hdnExpandIDAIOAmount.Value != "0")
            {
                List<vConsultVisitItemPackageMovement> lstMvm = BusinessLayer.GetvConsultVisitItemPackageMovementList(string.Format("DtID IN ({0})", hdnExpandIDAIOAmount.Value), int.MaxValue, 1, "MovementID ASC");
                lvwMovementAIOAmount.DataSource = lstMvm;
                lvwMovementAIOAmount.DataBind();
            }
        }

        protected void cbpViewAIOAmountMovement_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "refresh")
                {
                    BindGridViewAIOAmountMovement();
                    result = "refreshAIOAmountMovement";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #endregion

    }
}