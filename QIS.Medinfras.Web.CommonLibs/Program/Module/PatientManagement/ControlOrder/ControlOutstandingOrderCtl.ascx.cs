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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ControlOutstandingOrderCtl : BaseViewPopupCtl
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

            String filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID NOT IN ('{1}','{2}','{3}') AND HealthcareServiceUnitID != {4} AND IsDeleted = 0",
                    AppSession.UserLogin.HealthcareID, Constant.Facility.INPATIENT, Constant.Facility.PHARMACY, Constant.Facility.MEDICAL_CHECKUP, AppSession.RegisteredPatient.HealthcareServiceUnitID);
            List<vHealthcareServiceUnit> lstEntity = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            lstEntity.Insert(0, new vHealthcareServiceUnit() { ServiceUnitName = "", ServiceUnitCode = "" });
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnitPerHealthcare, lstEntity, "ServiceUnitName", "ServiceUnitCode");

            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'", Constant.StandardCode.DELETE_REASON));
            Methods.SetComboBoxField<StandardCode>(cboVoidReason, lstSC, "StandardCodeName", "StandardCodeID");

            BindGridView();
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = "";

            if (param[0].Contains("void"))
            {
                String test = hdnSelectedMember.Value.Substring(1);
                String[] orderID = hdnSelectedMember.Value.Substring(1).Split(',');

                if (OnVoidOrder(ref errMessage, orderID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else
            {
                result += "refresh";
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnVoidOrder(ref string errMessage, string[] OrderID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityTestOrderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityTestOrderDtDao = new TestOrderDtDao(ctx);
            ServiceOrderHdDao entityServiceOrderHdDao = new ServiceOrderHdDao(ctx);
            try
            {
                for (int i = 0; i < OrderID.Length; i++)
                {
                    List<vPatientOrderAll> lstEntity = BusinessLayer.GetvPatientOrderAllList(String.Format("OrderID IN ({0}) AND VisitID = {1}", OrderID[i], hdnVisitIDCtl.Value));

                    List<TestOrderHd> lstTestOrder = new List<TestOrderHd>();
                    List<ServiceOrderHd> lstServiceOrder = new List<ServiceOrderHd>();

                    #region List

                    foreach (vPatientOrderAll entity in lstEntity)
                    {
                        if (entity.OrderType == "TO")
                        {
                            TestOrderHd entityTo = new TestOrderHd();
                            entityTo = BusinessLayer.GetTestOrderHdList(String.Format("TestOrderID = {0} AND GCTransactionStatus IN ('{1}','{2}')",
                                                entity.OrderID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL)).FirstOrDefault();

                            if (entityTo != null)
                            {
                                lstTestOrder.Add(entityTo);
                            }
                        }
                        else if (entity.OrderType == "SO")
                        {
                            ServiceOrderHd entitySo = new ServiceOrderHd();
                            entitySo = BusinessLayer.GetServiceOrderHdList(String.Format("ServiceOrderID = {0} AND GCTransactionStatus IN ('{1}','{2}')",
                                                entity.OrderID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL)).FirstOrDefault();
                            if (entitySo != null)
                            {
                                lstServiceOrder.Add(entitySo);
                            }
                        }
                    }

                    #endregion

                    #region Test Order
                    if (lstTestOrder.Count > 0)
                    {
                        foreach (TestOrderHd entity in lstTestOrder)
                        {
                            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                            {
                                List<TestOrderDt> lstDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0}", entity.TestOrderID), ctx);
                                foreach (TestOrderDt dt in lstDt)
                                {
                                    TestOrderDt entityDt = entityTestOrderDtDao.Get(dt.ID);
                                    entityDt.GCTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                    entityDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                    entityDt.VoidReason = "HEADER IS CANCELLED";
                                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityTestOrderDtDao.Update(entityDt);
                                }

                                entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                if (cboVoidReason.Value != null)
                                {
                                    entity.GCVoidReason = cboVoidReason.Value.ToString();
                                    if (entity.GCVoidReason.Equals(Constant.DeleteReason.OTHER))
                                    {
                                        entity.VoidReason = txtVoidOtherReason.Text;
                                    }
                                }
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityTestOrderHdDao.Update(entity);
                            }
                        }
                    }

                    #endregion

                    #region Service Order

                    if (lstServiceOrder.Count > 0)
                    {
                        foreach (ServiceOrderHd entity in lstServiceOrder)
                        {
                            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                            {
                                entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityServiceOrderHdDao.Update(entity);
                            }
                        }
                    }

                    #endregion
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private void BindGridView()
        {
            String filterExpression = string.Format(string.Format("VisitID = {0} AND GCTransactionStatus IN ('{1}','{2}')", hdnVisitIDCtl.Value, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL));
            if (cboServiceUnitPerHealthcare.Value != null)
            {
                filterExpression += string.Format(" AND ServiceUnitCode = '{0}'", cboServiceUnitPerHealthcare.Value);
            }

            List<vPatientOrderAll> lstEntity = BusinessLayer.GetvPatientOrderAllList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}