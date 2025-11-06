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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class OutstandingOrderDetailListCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] parameter = param.Split('|');
            hdnOrderID.Value = parameter[0].ToString();
            hdnOrderTypeID.Value = parameter[1].ToString();
            String filterExpression = string.Format("OrderId = {0} AND OrderType = '{1}'", hdnOrderID.Value, hdnOrderTypeID.Value);
            vPatientOrderAll entity = BusinessLayer.GetvPatientOrderAllList(filterExpression).FirstOrDefault();
            txtTestOrderHdNo.Text = entity.OrderNo;
            txtServiceUnitName.Text = entity.ServiceUnitName;
            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'", Constant.StandardCode.DELETE_REASON));
            Methods.SetComboBoxField<StandardCode>(cboVoidCtlReason, lstSC, "StandardCodeName", "StandardCodeID");
            Helper.SetControlEntrySetting(cboVoidCtlReason, new ControlEntrySetting(true, true, true), "mpOutstandingOrderCtl");
            BindGridView();

        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnOrderID.Value != "")
                filterExpression = string.Format("OrderID = {0} AND OrderType = '{1}' AND IsDeleted = 0 ORDER BY ID DESC", hdnOrderID.Value, hdnOrderTypeID.Value);
            List<vPatientOrderAllDt> lstEntity = BusinessLayer.GetvPatientOrderAllDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
            
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "void")
            {
                if (OnVoidRecord(ref errMessage))
                {
                    result += "success";
                    cboVoidCtlReason.Value = null;
                    txtVoidOtherReason.Text = "";
                }
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }


        private bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            String[] paramID = hdnSelectedMemberCtl.Value.Substring(1).Split(',');
            String[] paramType = hdnSelectedMemberTypeCtl.Value.Substring(1).Split(',');
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtDao orderDtDao = new TestOrderDtDao(ctx);
            TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);
            PrescriptionOrderDtDao PrescriptionOrderDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderHdDao PrescriptionOrderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionReturnOrderDtDao PrescriptionReturnOrderDtDao = new PrescriptionReturnOrderDtDao(ctx);
            PrescriptionReturnOrderHdDao PrescriptionReturnOrderHdDao = new PrescriptionReturnOrderHdDao(ctx);
            ServiceOrderDtDao ServicenOrderDtDao = new ServiceOrderDtDao(ctx);
            ServiceOrderHdDao ServiceOrderHdDao = new ServiceOrderHdDao(ctx);
            NutritionOrderHdDao entityNutritionOrderHdDao = new NutritionOrderHdDao(ctx);
            NutritionOrderDtDao entityNutritionOrderDtDao = new NutritionOrderDtDao(ctx);

            try
            {
                for (int i = 0; i < paramID.Length; i++)
                {
                    vPatientOrderAllDt entityDt = BusinessLayer.GetvPatientOrderAllDtList(string.Format("ID = {0} AND OrderType = '{1}'", paramID[i], paramType[i])).FirstOrDefault();

                    if (entityDt.OrderType == "TO")
                    {
                        List<TestOrderDt> lstTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("ID IN ({0}) AND GCTestOrderStatus = '{1}' AND IsDeleted = 0", paramID[i], Constant.TestOrderStatus.OPEN));
                        foreach (TestOrderDt entity in lstTestOrderDt)
                        {
                            if (!entity.IsDeleted)
                            {
                                entity.GCTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                if (cboVoidCtlReason.Value != null)
                                {
                                    entity.GCVoidReason = cboVoidCtlReason.Value.ToString();
                                    if (entity.GCVoidReason.Equals(Constant.DeleteReason.OTHER))
                                    {
                                        entity.VoidReason = txtVoidOtherReason.Text;
                                    }
                                }
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                orderDtDao.Update(entity);
                            }
                        }
                        int testOrderDtCount = BusinessLayer.GetTestOrderDtRowCount(string.Format("TestOrderID = {0} AND GCTestOrderStatus = '{1}' AND IsDeleted = 0", hdnOrderID.Value, Constant.TestOrderStatus.OPEN), ctx);
                        if (testOrderDtCount < 1)
                        {
                            TestOrderHd testOrderHd = orderHdDao.Get(Convert.ToInt32(hdnOrderID.Value));
                            if (testOrderHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || testOrderHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                            {
                                testOrderHd.VoidBy = AppSession.UserLogin.UserID;
                                testOrderHd.VoidDate = DateTime.Now;
                                testOrderHd.GCVoidReason = cboVoidCtlReason.Value.ToString();
                                if (testOrderHd.GCVoidReason.Equals(Constant.DeleteReason.OTHER))
                                {
                                    testOrderHd.VoidReason = txtVoidOtherReason.Text;
                                }
                                testOrderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                testOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                orderHdDao.Update(testOrderHd);
                            }
                        }
                    }
                    else if (entityDt.OrderType == "PO")
                    {
                        List<PrescriptionOrderDt> lstPrescriptionOrderDt = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderDetailID IN ({0}) AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", paramID[i], Constant.TestOrderStatus.OPEN));
                        foreach (PrescriptionOrderDt entity in lstPrescriptionOrderDt)
                        {
                            if (!entity.IsDeleted)
                            {
                                entity.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                if (cboVoidCtlReason.Value != null)
                                {
                                    entity.GCVoidReason = cboVoidCtlReason.Value.ToString();
                                    if (entity.GCVoidReason.Equals(Constant.DeleteReason.OTHER))
                                    {
                                        entity.VoidReason = txtVoidOtherReason.Text;
                                    }
                                }
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                PrescriptionOrderDtDao.Update(entity);
                            }
                        }
                        int PrescriptionOrderDtCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", hdnOrderID.Value, Constant.TestOrderStatus.OPEN), ctx);
                        if (PrescriptionOrderDtCount < 1)
                        {
                            PrescriptionOrderHd PrescriptionOrderHd = PrescriptionOrderHdDao.Get(Convert.ToInt32(hdnOrderID.Value));
                            if (PrescriptionOrderHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || PrescriptionOrderHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                            {
                                PrescriptionOrderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                PrescriptionOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                PrescriptionOrderHdDao.Update(PrescriptionOrderHd);
                            }
                        }
                    }
                    else if (entityDt.OrderType == "RO")
                    {
                        List<PrescriptionReturnOrderDt> lstPrescriptionReturnOrderDt = BusinessLayer.GetPrescriptionReturnOrderDtList(string.Format("PrescriptionReturnOrderDtID IN ({0}) AND IsDeleted = 0", paramID[i]));
                        //foreach (PrescriptionReturnOrderDt entity in lstPrescriptionReturnOrderDt)
                        //{
                        //    entity.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                        //    if (cboVoidCtlReason.Value != null)
                        //    {
                        //        entity.GCVoidReason = cboVoidCtlReason.Value.ToString();
                        //        if (entity.GCVoidReason.Equals(Constant.DeleteReason.OTHER))
                        //        {
                        //            entity.VoidReason = txtVoidOtherReason.Text;
                        //        }
                        //    }
                        //    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        //    PrescriptionReturnOrderDtDao.Update(entity);
                        //}
                        int PrescriptionReturnOrderDtCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionReturnOrderID = {0} AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", hdnOrderID.Value, Constant.TestOrderStatus.OPEN), ctx);
                        if (PrescriptionReturnOrderDtCount < 1)
                        {
                            PrescriptionReturnOrderHd PrescriptionReturnOrderHd = PrescriptionReturnOrderHdDao.Get(Convert.ToInt32(hdnOrderID.Value));
                            if (PrescriptionReturnOrderHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || PrescriptionReturnOrderHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                            {
                                PrescriptionReturnOrderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                PrescriptionReturnOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                PrescriptionReturnOrderHdDao.Update(PrescriptionReturnOrderHd);
                            }
                        }
                    }
                    else if (entityDt.OrderType == "SO")
                    {
                        List<ServiceOrderDt> lstServiceOrderDt = BusinessLayer.GetServiceOrderDtList(String.Format("ID IN ({0}) AND GCServiceOrderStatus = '{1}'", paramID[i], Constant.TestOrderStatus.OPEN));
                        foreach (ServiceOrderDt entity in lstServiceOrderDt)
                        {
                            if (!entity.IsDeleted)
                            {
                                entity.GCServiceOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                if (cboVoidCtlReason.Value != null)
                                {
                                    entity.GCVoidReason = cboVoidCtlReason.Value.ToString();
                                    if (entity.GCVoidReason.Equals(Constant.DeleteReason.OTHER))
                                    {
                                        entity.VoidReason = txtVoidOtherReason.Text;
                                    }
                                }
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ServicenOrderDtDao.Update(entity);
                            }
                        }
                        int ServiceOrderDtCount = BusinessLayer.GetServiceOrderDtRowCount(string.Format("ServiceOrderID = {0} AND GCServiceOrderStatus = '{1}' AND IsDeleted = 0", hdnOrderID.Value, Constant.TestOrderStatus.OPEN), ctx);
                        if (ServiceOrderDtCount < 1)
                        {
                            ServiceOrderHd ServiceOrderHd = ServiceOrderHdDao.Get(Convert.ToInt32(hdnOrderID.Value));
                            if (ServiceOrderHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || ServiceOrderHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                            {
                                ServiceOrderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                ServiceOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ServiceOrderHdDao.Update(ServiceOrderHd);
                            }
                        }
                    }
                    else if (entityDt.OrderType == "NO")
                    {
                        List<NutritionOrderDt> lstNutritionOrderDt = BusinessLayer.GetNutritionOrderDtList(string.Format("NutritionOrderDtID IN ({0}) AND GCItemDetailStatus = '{1}'", paramID[i], Constant.TransactionStatus.OPEN));
                        foreach (NutritionOrderDt entity in lstNutritionOrderDt)
                        {
                            entity.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityNutritionOrderDtDao.Update(entity);

                        }
                        int nutritionOrderDtCount = BusinessLayer.GetNutritionOrderDtRowCount(string.Format("NutritionOrderHdID = {0} AND GCItemDetailStatus = '{1}'", hdnOrderID.Value, Constant.TransactionStatus.OPEN), ctx);
                        if (nutritionOrderDtCount < 1)
                        {
                            NutritionOrderHd nutritionOrderHd = entityNutritionOrderHdDao.Get(Convert.ToInt32(hdnOrderID.Value));
                            if (nutritionOrderHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || nutritionOrderHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                            {
                                nutritionOrderHd.VoidBy = AppSession.UserLogin.UserID;
                                nutritionOrderHd.VoidDate = DateTime.Now;
                                nutritionOrderHd.GCVoidReason = cboVoidCtlReason.Value.ToString();
                                if (nutritionOrderHd.GCVoidReason.Equals(Constant.DeleteReason.OTHER))
                                {
                                    nutritionOrderHd.VoidReason = txtVoidOtherReason.Text;
                                }
                                nutritionOrderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                nutritionOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityNutritionOrderHdDao.Update(nutritionOrderHd);
                            }
                        }
                    }

                    //List<TestOrderDt> lstTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("ID IN ({0})", hdnSelectedMemberCtl.Value.Substring(1)));
                    //foreach (TestOrderDt entity in lstTestOrderDt)
                    //{
                    //    entity.GCTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
                    //    if (cboVoidCtlReason.Value != null)
                    //    {
                    //        entity.GCVoidReason = cboVoidCtlReason.Value.ToString();
                    //        if (entity.GCVoidReason.Equals(Constant.DeleteReason.OTHER))
                    //        {
                    //            entity.VoidReason = txtVoidOtherReason.Text;
                    //        }
                    //    }
                    //    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    //    orderDtDao.Update(entity);
                    //}
                    //int testOrderDtCount = BusinessLayer.GetTestOrderDtRowCount(string.Format("TestOrderID = {0} AND GCTestOrderStatus = '{1}' AND IsDeleted = 0", hdnOrderID.Value, Constant.TestOrderStatus.OPEN), ctx);
                    //if (testOrderDtCount < 1)
                    //{
                    //    TestOrderHd testOrderHd = orderHdDao.Get(Convert.ToInt32(hdnOrderID.Value));
                    //    testOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                    //    testOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    //    orderHdDao.Update(testOrderHd);
                    //}
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
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