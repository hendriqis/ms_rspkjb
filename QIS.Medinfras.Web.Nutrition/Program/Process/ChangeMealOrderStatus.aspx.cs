using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class ChangeMealOrderStatus : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nutrition.CHANGE_MEAL_STATUS_ORDER;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboVoidReason, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnSelectedMember, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnSelectedMemberType, new ControlEntrySetting(false, false, false));
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param.Length > 1)
                {
                    hdnDepartmentID.Value = param[0];
                }
                else
                {
                    hdnDepartmentID.Value = Page.Request.QueryString["id"];
                }
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnGCRegistrationStatus.Value = AppSession.RegisteredPatient.GCRegistrationStatus;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnClassID.Value = AppSession.RegisteredPatient.ClassID.ToString();
            BindGridView();
        }

        private void BindGridView()
        {

            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);

            string code = ddlProcessType.SelectedValue;

            if (code == "1")
                filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}')", Constant.TransactionStatus.OPEN);
            else if (code == "2")
                filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}')", Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            else if (code == "3")
                filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}')", Constant.TransactionStatus.WAIT_FOR_APPROVAL);

            List<vNutritionOrderHd> lstEntity = BusinessLayer.GetvNutritionOrderHdList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
                cboVoidReason.Value = null;
                txtVoidOtherReason.Text = string.Empty;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'", Constant.StandardCode.DELETE_REASON));
            Methods.SetComboBoxField<StandardCode>(cboVoidReason, lstSC, "StandardCodeName", "StandardCodeID");
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;

            if (type == "void" || type == "reopen")
            {
                if (cboVoidReason.Value == null)
                {
                    errMessage = "Alasan Void/Reopen harus dilengkapi";
                    return false;
                }
            }

            String[] paramID = hdnSelectedMember.Value.Substring(1).Split(',');
            String[] paramType = hdnSelectedMemberType.Value.Substring(1).Split(',');
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityTestOrderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityTestOrderDtDao = new TestOrderDtDao(ctx);
            PrescriptionOrderHdDao entityPrescriptionOrderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityPrescriptionOrderDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionReturnOrderHdDao entityPrescriptionReturnOrderHdDao = new PrescriptionReturnOrderHdDao(ctx);
            PrescriptionReturnOrderDtDao entityPrescriptionReturnOrderDtDao = new PrescriptionReturnOrderDtDao(ctx);
            ServiceOrderHdDao entityServiceOrderHdDao = new ServiceOrderHdDao(ctx);
            ServiceOrderDtDao entityServiceOrderDtDao = new ServiceOrderDtDao(ctx);
            ChargesStatusLogDao statusLogDao = new ChargesStatusLogDao(ctx);

            NutritionOrderHdDao entityNutritionOrderHdDao = new NutritionOrderHdDao(ctx);
            NutritionOrderDtDao entityNutritionOrderDtDao = new NutritionOrderDtDao(ctx);

            try
            {
                if (type == "void")
                {
                    #region VOID

                    for (int i = 0; i < paramID.Length; i++)
                    {
                        List<vNutritionOrderHd> lstEntity = BusinessLayer.GetvNutritionOrderHdList(String.Format("NutritionOrderHdID IN ({0})", paramID[i]));

                        List<TestOrderHd> lstTestOrder = new List<TestOrderHd>();
                        List<PrescriptionOrderHd> lstPrescription = new List<PrescriptionOrderHd>();
                        List<PrescriptionReturnOrderHd> lstReturnPrescription = new List<PrescriptionReturnOrderHd>();
                        List<ServiceOrderHd> lstServiceOrder = new List<ServiceOrderHd>();
                        List<NutritionOrderHd> lstNutritionOrder = new List<NutritionOrderHd>();

                        #region List

                        foreach (vNutritionOrderHd entity in lstEntity)
                        {
                            NutritionOrderHd entityTo = new NutritionOrderHd();
                            entityTo = BusinessLayer.GetNutritionOrderHdList(String.Format("NutritionOrderHdID = {0} AND GCTransactionStatus IN ('{1}','{2}')",
                                                entity.NutritionOrderHdID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL)).FirstOrDefault();

                            if (entityTo != null)
                            {
                                lstNutritionOrder.Add(entityTo);
                            }
                        }

                        #endregion

                        #region Test Order

                        if (lstNutritionOrder.Count > 0)
                        {
                            foreach (NutritionOrderHd entity in lstNutritionOrder)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                                {
                                    List<NutritionOrderDt> lstDt = BusinessLayer.GetNutritionOrderDtList(string.Format("NutritionOrderHdID = {0}", entity.NutritionOrderHdID), ctx);
                                    foreach (NutritionOrderDt dt in lstDt)
                                    {
                                        NutritionOrderDt entityDt = entityNutritionOrderDtDao.Get(dt.NutritionOrderDtID);
                                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityDt.LastUpdatedDate = DateTime.Now;
                                        entityNutritionOrderDtDao.Update(entityDt);
                                    }

                                    string oldStatus = entity.GCTransactionStatus;
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
                                    entityNutritionOrderHdDao.Update(entity);
                                }
                            }
                        }

                        #endregion
                    }
                    ctx.CommitTransaction();

                    #endregion
                }
                else if (type == "propose")
                {
                    #region PROPOSED

                    for (int i = 0; i < paramID.Length; i++)
                    {
                        List<vNutritionOrderHd> lstEntity = BusinessLayer.GetvNutritionOrderHdList(String.Format("NutritionOrderHdID IN ({0})", paramID[i]));

                        List<TestOrderHd> lstTestOrder = new List<TestOrderHd>();
                        List<PrescriptionOrderHd> lstPrescription = new List<PrescriptionOrderHd>();
                        List<PrescriptionReturnOrderHd> lstReturnPrescription = new List<PrescriptionReturnOrderHd>();
                        List<ServiceOrderHd> lstServiceOrder = new List<ServiceOrderHd>();
                        List<NutritionOrderHd> lstNutritionOrder = new List<NutritionOrderHd>();

                        #region List

                        foreach (vNutritionOrderHd entity in lstEntity)
                        {
                            NutritionOrderHd entityTo = new NutritionOrderHd();
                            entityTo = BusinessLayer.GetNutritionOrderHdList(String.Format("NutritionOrderHdID = {0} AND GCTransactionStatus = '{1}'",
                                                entity.NutritionOrderHdID, Constant.TransactionStatus.OPEN)).FirstOrDefault();

                            if (entityTo != null)
                            {
                                lstNutritionOrder.Add(entityTo);
                            }
                        }

                        #endregion

                        #region Test Order

                        if (lstNutritionOrder.Count > 0)
                        {
                            foreach (NutritionOrderHd entity in lstNutritionOrder)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                                {
                                    List<NutritionOrderDt> lstDt = BusinessLayer.GetNutritionOrderDtList(string.Format("NutritionOrderHdID = {0}", entity.NutritionOrderHdID), ctx);
                                    foreach (NutritionOrderDt entityDt in lstDt)
                                    {
                                        if (entityDt.GCItemDetailStatus != Constant.TransactionStatus.VOID)
                                        {
                                            entityDt.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            entityDt.LastUpdatedDate = DateTime.Now;
                                            entityNutritionOrderDtDao.Update(entityDt);
                                        }
                                    }

                                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityNutritionOrderHdDao.Update(entity);
                                }
                            }
                        }

                        #endregion
                    }
                    ctx.CommitTransaction();

                    #endregion
                }
                else 
                {
                    #region OPEN

                    for (int i = 0; i < paramID.Length; i++)
                    {
                        List<vNutritionOrderHd> lstEntity = BusinessLayer.GetvNutritionOrderHdList(String.Format("NutritionOrderHdID IN ({0})", paramID[i]));

                        List<TestOrderHd> lstTestOrder = new List<TestOrderHd>();
                        List<PrescriptionOrderHd> lstPrescription = new List<PrescriptionOrderHd>();
                        List<PrescriptionReturnOrderHd> lstReturnPrescription = new List<PrescriptionReturnOrderHd>();
                        List<ServiceOrderHd> lstServiceOrder = new List<ServiceOrderHd>();
                        List<NutritionOrderHd> lstNutritionOrder = new List<NutritionOrderHd>();

                        #region List

                        foreach (vNutritionOrderHd entity in lstEntity)
                        {
                            NutritionOrderHd entityTo = new NutritionOrderHd();
                            entityTo = BusinessLayer.GetNutritionOrderHdList(String.Format("NutritionOrderHdID = {0} AND GCTransactionStatus IN ('{1}','{2}')",
                                                entity.NutritionOrderHdID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL)).FirstOrDefault();

                            if (entityTo != null)
                            {
                                lstNutritionOrder.Add(entityTo);
                            }
                        }

                        #endregion

                        #region Test Order

                        if (lstNutritionOrder.Count > 0)
                        {
                            foreach (NutritionOrderHd entity in lstNutritionOrder)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                                {
                                    List<NutritionOrderDt> lstDt = BusinessLayer.GetNutritionOrderDtList(string.Format("NutritionOrderHdID = {0}", entity.NutritionOrderHdID), ctx);
                                    foreach (NutritionOrderDt dt in lstDt)
                                    {
                                        NutritionOrderDt entityDt = entityNutritionOrderDtDao.Get(dt.NutritionOrderDtID);
                                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityDt.LastUpdatedDate = DateTime.Now;
                                        entityNutritionOrderDtDao.Update(entityDt);
                                    }

                                    entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityNutritionOrderHdDao.Update(entity);
                                }
                            }
                        }

                        #endregion
                    }
                    ctx.CommitTransaction();

                    #endregion
                }
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}