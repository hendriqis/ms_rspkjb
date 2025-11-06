using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class ControlOrderDtCustomCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            string[] temp = param.Split('|');
            hdnVisitIDCtl.Value = temp[0];
            hdnRegistrationNoCtl.Value = temp[1];
            hdnPatientNameCtl.Value = temp[2];
            hdnItemIDCtl.Value = temp[3];
            hdnItemCodeCtl.Value = temp[4];
            hdnItemNameCtl.Value = temp[5];

            txtItemServiceName.Text = string.Format("{0} - {1}", hdnItemCodeCtl.Value, hdnItemNameCtl.Value);
            txtNoReg.Text = string.Format("{0} - {1}", hdnRegistrationNoCtl.Value, hdnPatientNameCtl.Value);
            
            BindGridView();
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (OnSaveEditRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            List<vMCUOrderStatus> lstEntity = BusinessLayer.GetvMCUOrderStatusList(string.Format(string.Format("VisitID = {0} AND ItemPackageID = {1} ORDER BY ServiceUnitName, ItemName1", hdnVisitIDCtl.Value, hdnItemIDCtl.Value)));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtDao testOrderDtDao = new TestOrderDtDao(ctx);
            ServiceOrderDtDao serviceOrderDtDao = new ServiceOrderDtDao(ctx);
            PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);

            try
            {
                if (hdnSelectedDepartmentID.Value != Constant.Facility.OUTPATIENT)
                {
                    TestOrderDt orderDt = testOrderDtDao.Get(Convert.ToInt32(hdnSelectedOrderDtID.Value));
                    orderDt.ItemID = Convert.ToInt32(hdnItemID.Value);
                    orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    testOrderDtDao.Update(orderDt);
                }
                else
                {
                    ServiceOrderDt orderDt = serviceOrderDtDao.Get(Convert.ToInt32(hdnSelectedOrderDtID.Value));
                    orderDt.ItemID = Convert.ToInt32(hdnItemID.Value);
                    orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    serviceOrderDtDao.Update(orderDt);
                }

                PatientChargesDt chargesDt = chargesDtDao.Get(Convert.ToInt32(hdnSelectedTransactionDtID.Value));
                chargesDt.ItemID = Convert.ToInt32(hdnItemID.Value);
                chargesDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                chargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                chargesDtDao.Update(chargesDt);

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }
    }
}