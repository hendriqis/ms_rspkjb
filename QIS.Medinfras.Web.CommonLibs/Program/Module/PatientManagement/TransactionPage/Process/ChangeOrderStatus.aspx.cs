using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using System.Reflection;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ChangeOrderStatus : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_PAGE_CHANGE_ORDER_STATUS;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_CHANGE_ORDER_STATUS;
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.PATIENT_PAGE_CHANGE_ORDER_STATUS;
                case Constant.Module.RADIOTHERAPHY: return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_CHANGE_ORDER_STATUS;
                default: return Constant.MenuCode.Outpatient.PATIENT_PAGE_CHANGE_ORDER_STATUS;
            }
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


            if (cboServiceUnitPerHealthcare.Value != null && cboServiceUnitPerHealthcare.Value.ToString() != "")
            {
                filterExpression += string.Format(" AND ServiceUnitCode = '{0}'", cboServiceUnitPerHealthcare.Value);
            }
            filterExpression += " ORDER BY OrderDate, OrderTime, OrderID";
            List<vPatientOrderAll> lstEntity = BusinessLayer.GetvPatientOrderAllList(filterExpression);
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
            String filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID NOT IN ('{1}','{2}') AND HealthcareServiceUnitID != {3} AND IsDeleted = 0 AND IsUsingRegistration = 1",
                    AppSession.UserLogin.HealthcareID, Constant.Facility.INPATIENT, Constant.Facility.MEDICAL_CHECKUP, AppSession.RegisteredPatient.HealthcareServiceUnitID);
            List<vHealthcareServiceUnitCustom> lstEntity = BusinessLayer.GetvHealthcareServiceUnitCustomList(filterExpression);
            lstEntity.Insert(0, new vHealthcareServiceUnitCustom() { ServiceUnitName = "", ServiceUnitCode = "" });
            lstEntity.Insert(1, new vHealthcareServiceUnitCustom() { ServiceUnitName = "Order Menu Makanan", ServiceUnitCode = "Makanan" });
            Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboServiceUnitPerHealthcare, lstEntity, "ServiceUnitName", "ServiceUnitCode");

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
            PrescriptionOrderHdOriginalDao entityOrderHdOriginalDao = new PrescriptionOrderHdOriginalDao(ctx);
            PrescriptionOrderDtOriginalDao entityOrderDtOriginalDao = new PrescriptionOrderDtOriginalDao(ctx);
            NutritionOrderHdDao entityNutritionOrderHdDao = new NutritionOrderHdDao(ctx);
            NutritionOrderDtDao entityNutritionOrderDtDao = new NutritionOrderDtDao(ctx);

            try
            {
                if (type == "void")
                {
                    #region VOID

                    List<TestOrderHd> lstTestOrder = new List<TestOrderHd>();
                    for (int i = 0; i < paramID.Length; i++)
                    {
                        List<vPatientOrderAll> lstEntity = BusinessLayer.GetvPatientOrderAllList(String.Format("OrderID IN ({0}) AND OrderType = '{1}'", paramID[i], paramType[i]));

                        List<PrescriptionOrderHd> lstPrescription = new List<PrescriptionOrderHd>();
                        List<PrescriptionReturnOrderHd> lstReturnPrescription = new List<PrescriptionReturnOrderHd>();
                        List<ServiceOrderHd> lstServiceOrder = new List<ServiceOrderHd>();
                        List<NutritionOrderHd> lstNutritionOrder = new List<NutritionOrderHd>();

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
                            else if (entity.OrderType == "PO")
                            {
                                PrescriptionOrderHd entityPo = new PrescriptionOrderHd();
                                entityPo = BusinessLayer.GetPrescriptionOrderHdList(String.Format("PrescriptionOrderID = {0} AND GCTransactionStatus IN ('{1}','{2}')",
                                                    entity.OrderID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL)).FirstOrDefault();

                                if (entityPo != null)
                                {
                                    lstPrescription.Add(entityPo);
                                }
                            }
                            else if (entity.OrderType == "RO")
                            {
                                PrescriptionReturnOrderHd entityRo = new PrescriptionReturnOrderHd();
                                entityRo = BusinessLayer.GetPrescriptionReturnOrderHdList(String.Format("PrescriptionReturnOrderID = {0} AND GCTransactionStatus IN ('{1}','{2}')",
                                                    entity.OrderID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL)).FirstOrDefault();

                                if (entityRo != null)
                                {
                                    lstReturnPrescription.Add(entityRo);
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
                            else if (entity.OrderType == "NO")
                            {
                                NutritionOrderHd entityNo = new NutritionOrderHd();
                                entityNo = BusinessLayer.GetNutritionOrderHdList(String.Format("NutritionOrderHdID = {0} AND GCTransactionStatus IN ('{1}','{2}')",
                                                    entity.OrderID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL)).FirstOrDefault();
                                if (entityNo != null)
                                {
                                    lstNutritionOrder.Add(entityNo);
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
                                    entity.VoidBy = AppSession.UserLogin.UserID;
                                    entity.VoidDate = DateTime.Now;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityTestOrderHdDao.Update(entity);

                                    UpdateLog(statusLogDao, entity.TestOrderID, oldStatus, Constant.TransactionStatus.VOID, cboVoidReason.Value.ToString(), txtVoidOtherReason.Text);
                                }
                            }
                        }

                        #endregion

                        #region Prescription Order

                        if (lstPrescription.Count > 0)
                        {
                            foreach (PrescriptionOrderHd entity in lstPrescription)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                                {
                                    string filterDt = string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", entity.PrescriptionOrderID);
                                    List<PrescriptionOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionOrderDtList(filterDt, ctx);
                                    foreach (PrescriptionOrderDt entityDt in lstEntityDt)
                                    {
                                        entityDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityPrescriptionOrderDtDao.Update(entityDt);
                                    }

                                    string oldStatus = entity.GCTransactionStatus;
                                    entity.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
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
                                    entityPrescriptionOrderHdDao.Update(entity);

                                    #region Header Original
                                    if (entity.IsOrderedByPhysician == true)
                                    {

                                        string filterHdOri = string.Format("PrescriptionOrderID = {0} AND GCTransactionStatus <> '{1}'", entity.PrescriptionOrderID, Constant.TransactionStatus.VOID);
                                        List<PrescriptionOrderHdOriginal> lstHdOri = BusinessLayer.GetPrescriptionOrderHdOriginalList(filterHdOri, ctx);
                                        foreach (PrescriptionOrderHdOriginal hdOri in lstHdOri)
                                        {
                                            hdOri.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                            hdOri.GCOrderStatus = Constant.OrderStatus.CANCELLED;
                                            hdOri.GCVoidReason = entity.GCVoidReason;
                                            hdOri.VoidReason = entity.GCVoidReason;
                                            hdOri.VoidBy = AppSession.UserLogin.UserID;
                                            hdOri.VoidDate = DateTime.Now;
                                            hdOri.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            hdOri.LastUpdatedDate = DateTime.Now;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            entityOrderHdOriginalDao.Update(hdOri);

                                            string filterDtOri = string.Format("PrescriptionOrderID = {0} AND GCPrescriptionOrderStatus <> '{1}'", hdOri.PrescriptionOrderID, Constant.OrderStatus.CANCELLED);
                                            List<PrescriptionOrderDtOriginal> lstDtOri = BusinessLayer.GetPrescriptionOrderDtOriginalList(filterDtOri, ctx);
                                            foreach (PrescriptionOrderDtOriginal dtOri in lstDtOri)
                                            {
                                                dtOri.IsDeleted = true;
                                                dtOri.GCPrescriptionOrderStatus = Constant.OrderStatus.CANCELLED;
                                                dtOri.VoidBy = AppSession.UserLogin.UserID;
                                                dtOri.VoidDateTime = DateTime.Now;
                                                dtOri.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                dtOri.LastUpdatedDate = DateTime.Now;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                entityOrderDtOriginalDao.Update(dtOri);
                                            }
                                        }
                                    }
                                    #endregion

                                    UpdateLog(statusLogDao, entity.PrescriptionOrderID, oldStatus, Constant.TransactionStatus.VOID, cboVoidReason.Value.ToString(), txtVoidOtherReason.Text);
                                }
                            }
                        }

                        #endregion

                        #region Prescription Return Order

                        if (lstReturnPrescription.Count > 0)
                        {
                            foreach (PrescriptionReturnOrderHd entity in lstReturnPrescription)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                                {
                                    string filterDt = string.Format("PrescriptionReturnOrderID = {0} AND IsDeleted = 0", entity.PrescriptionReturnOrderID);
                                    List<PrescriptionReturnOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionReturnOrderDtList(filterDt, ctx);
                                    foreach (PrescriptionReturnOrderDt entityDt in lstEntityDt)
                                    {
                                        entityDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityPrescriptionReturnOrderDtDao.Update(entityDt);
                                    }

                                    string oldStatus = entity.GCTransactionStatus;
                                    entity.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityPrescriptionReturnOrderHdDao.Update(entity);

                                    UpdateLog(statusLogDao, entity.PrescriptionReturnOrderID, oldStatus, Constant.TransactionStatus.VOID, cboVoidReason.Value.ToString(), txtVoidOtherReason.Text);
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
                                    string oldStatus = entity.GCTransactionStatus;
                                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityServiceOrderHdDao.Update(entity);

                                    UpdateLog(statusLogDao, entity.ServiceOrderID, oldStatus, Constant.TransactionStatus.VOID, cboVoidReason.Value.ToString(), txtVoidOtherReason.Text);
                                }
                            }
                        }

                        #endregion

                        #region Nutrition Order

                        if (lstNutritionOrder.Count > 0)
                        {
                            foreach (NutritionOrderHd entity in lstNutritionOrder)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN || entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                                {
                                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    if (cboVoidReason.Value != null)
                                    {
                                        entity.GCVoidReason = cboVoidReason.Value.ToString();
                                        if (entity.GCVoidReason.Equals(Constant.DeleteReason.OTHER))
                                        {
                                            entity.VoidReason = txtVoidOtherReason.Text;
                                        }
                                    }
                                    entity.VoidBy = AppSession.UserLogin.UserID;
                                    entity.VoidDate = DateTime.Now;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityNutritionOrderHdDao.Update(entity);
                                }
                            }
                        }
                        #endregion
                    }
                    ctx.CommitTransaction();

                    if (AppSession.SA0137 == "1")
                    {
                        if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                        {
                            BridgingToMedinfrasV1(3, lstTestOrder);
                        }
                    }

                    #endregion
                }
                else if (type == "propose")
                {
                    #region PROPOSED

                    List<TestOrderHd> lstTestOrder = new List<TestOrderHd>();
                    for (int i = 0; i < paramID.Length; i++)
                    {
                        List<vPatientOrderAll> lstEntity = BusinessLayer.GetvPatientOrderAllList(String.Format("OrderID IN ({0}) AND OrderType = '{1}'", paramID[i], paramType[i]));

                        List<PrescriptionOrderHd> lstPrescription = new List<PrescriptionOrderHd>();
                        List<PrescriptionReturnOrderHd> lstReturnPrescription = new List<PrescriptionReturnOrderHd>();
                        List<ServiceOrderHd> lstServiceOrder = new List<ServiceOrderHd>();
                        List<NutritionOrderHd> lstNutritionOrder = new List<NutritionOrderHd>();

                        #region List

                        foreach (vPatientOrderAll entity in lstEntity)
                        {
                            if (entity.OrderType == "TO")
                            {
                                TestOrderHd entityTo = new TestOrderHd();
                                entityTo = BusinessLayer.GetTestOrderHdList(String.Format("TestOrderID = {0} AND GCTransactionStatus = '{1}'",
                                                    entity.OrderID, Constant.TransactionStatus.OPEN)).FirstOrDefault();

                                if (entityTo != null)
                                {
                                    lstTestOrder.Add(entityTo);
                                }
                            }
                            else if (entity.OrderType == "PO")
                            {
                                PrescriptionOrderHd entityPo = new PrescriptionOrderHd();
                                entityPo = BusinessLayer.GetPrescriptionOrderHdList(String.Format("PrescriptionOrderID = {0} AND GCTransactionStatus = '{1}'",
                                                    entity.OrderID, Constant.TransactionStatus.OPEN)).FirstOrDefault();

                                if (entityPo != null)
                                {
                                    lstPrescription.Add(entityPo);
                                }
                            }
                            else if (entity.OrderType == "RO")
                            {
                                PrescriptionReturnOrderHd entityRo = new PrescriptionReturnOrderHd();
                                entityRo = BusinessLayer.GetPrescriptionReturnOrderHdList(String.Format("PrescriptionReturnOrderID = {0} AND GCTransactionStatus = '{1}'",
                                                    entity.OrderID, Constant.TransactionStatus.OPEN)).FirstOrDefault();

                                if (entityRo != null)
                                {
                                    lstReturnPrescription.Add(entityRo);
                                }
                            }
                            else if (entity.OrderType == "SO")
                            {
                                ServiceOrderHd entitySo = new ServiceOrderHd();
                                entitySo = BusinessLayer.GetServiceOrderHdList(String.Format("ServiceOrderID = {0} AND GCTransactionStatus ='{1}'",
                                                    entity.OrderID, Constant.TransactionStatus.OPEN)).FirstOrDefault();
                                if (entitySo != null)
                                {
                                    lstServiceOrder.Add(entitySo);
                                }
                            }
                            else if (entity.OrderType == "NO")
                            {
                                NutritionOrderHd entityNo = new NutritionOrderHd();
                                entityNo = BusinessLayer.GetNutritionOrderHdList(String.Format("NutritionOrderHdID = {0} AND GCTransactionStatus IN ('{1}')",
                                                    entity.OrderID, Constant.TransactionStatus.OPEN)).FirstOrDefault();
                                if (entityNo != null)
                                {
                                    lstNutritionOrder.Add(entityNo);
                                }
                            }
                        }

                        #endregion

                        #region Test Order

                        if (lstTestOrder.Count > 0)
                        {
                            foreach (TestOrderHd entity in lstTestOrder)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                                {
                                    List<TestOrderDt> lstDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0}", entity.TestOrderID), ctx);
                                    foreach (TestOrderDt entityDt in lstDt)
                                    {
                                        if (entity.IsCITO == true)
                                        {
                                            if (entityDt.IsCITO == false && entityDt.IsDeleted == false && entityDt.GCTestOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                            {
                                                entityDt.IsCITO = true;
                                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                entityTestOrderDtDao.Update(entityDt);
                                            }
                                        }
                                    }

                                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    entity.ProposedBy = AppSession.UserLogin.UserID;
                                    entity.ProposedDate = DateTime.Now;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityTestOrderHdDao.Update(entity);
                                }
                            }
                        }

                        #endregion

                        #region Prescription Order

                        if (lstPrescription.Count > 0)
                        {
                            foreach (PrescriptionOrderHd entity in lstPrescription)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                                {
                                    entity.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    entity.SendOrderDateTime = DateTime.Now.Date;
                                    entity.SendOrderBy = AppSession.UserLogin.UserID;
                                    entity.ProposedBy = AppSession.UserLogin.UserID;
                                    entity.ProposedDate = DateTime.Now;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityPrescriptionOrderHdDao.Update(entity);

                                    //Log : Copy of Current Prescription Order
                                    int historyID = 0;
                                    if (entity.IsOrderedByPhysician)
                                    {
                                        #region Log Header
                                        PrescriptionOrderHdOriginal originalHd = new PrescriptionOrderHdOriginal();
                                        CopyHeaderObject(entity, ref originalHd);
                                        historyID = entityOrderHdOriginalDao.InsertReturnPrimaryKeyID(originalHd);
                                        #endregion
                                    }
                                    List<PrescriptionOrderDtOriginal> lstOriginalDt = new List<PrescriptionOrderDtOriginal>();

                                    string filterDt = string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", entity.PrescriptionOrderID);
                                    List<PrescriptionOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionOrderDtList(filterDt, ctx);
                                    foreach (PrescriptionOrderDt entityDt in lstEntityDt)
                                    {
                                        entityDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityPrescriptionOrderDtDao.Update(entityDt);

                                        if (historyID > 0)
                                        {
                                            PrescriptionOrderDtOriginal originalDt = new PrescriptionOrderDtOriginal();
                                            CopyDetailObject(entityDt, ref originalDt);
                                            originalDt.HistoryHeaderID = historyID;
                                            lstOriginalDt.Add(originalDt);
                                        }
                                    }

                                    #region Log Detail
                                    if (lstOriginalDt.Count > 0)
                                    {
                                        foreach (PrescriptionOrderDtOriginal originalDt in lstOriginalDt)
                                        {
                                            entityOrderDtOriginalDao.Insert(originalDt);
                                        }
                                    }
                                    #endregion

                                }
                            }
                        }

                        #endregion

                        #region Prescription Return Order

                        if (lstReturnPrescription.Count > 0)
                        {
                            foreach (PrescriptionReturnOrderHd entity in lstReturnPrescription)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                                {
                                    string filterDt = string.Format("PrescriptionReturnOrderID = {0} AND IsDeleted = 0", entity.PrescriptionReturnOrderID);
                                    List<PrescriptionReturnOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionReturnOrderDtList(filterDt);
                                    foreach (PrescriptionReturnOrderDt entityDt in lstEntityDt)
                                    {
                                        entityDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityPrescriptionReturnOrderDtDao.Update(entityDt);
                                    }

                                    entity.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    entity.ProposedBy = AppSession.UserLogin.UserID;
                                    entity.ProposedDate = DateTime.Now;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityPrescriptionReturnOrderHdDao.Update(entity);
                                }
                            }
                        }

                        #endregion

                        #region Service Order

                        if (lstServiceOrder.Count > 0)
                        {
                            foreach (ServiceOrderHd entity in lstServiceOrder)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                                {
                                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    entity.ProposedBy = AppSession.UserLogin.UserID;
                                    entity.ProposedDate = DateTime.Now;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityServiceOrderHdDao.Update(entity);
                                }
                            }
                        }

                        #endregion

                        #region Nutrition Order

                        if (lstNutritionOrder.Count > 0)
                        {
                            foreach (NutritionOrderHd entity in lstNutritionOrder)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                                {
                                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    entity.ProposedBy = AppSession.UserLogin.UserID;
                                    entity.ProposedDate = DateTime.Now;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityNutritionOrderHdDao.Update(entity);
                                }
                            }
                        }
                        #endregion
                    }
                    ctx.CommitTransaction();

                    if (AppSession.SA0137 == "1")
                    {
                        if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                        {
                            BridgingToMedinfrasV1(1, lstTestOrder);
                        }
                    }

                    #endregion
                }
                else
                {
                    #region OPEN

                    List<TestOrderHd> lstTestOrder = new List<TestOrderHd>();
                    for (int i = 0; i < paramID.Length; i++)
                    {
                        List<vPatientOrderAll> lstEntity = BusinessLayer.GetvPatientOrderAllList(String.Format("OrderID IN ({0}) AND OrderType = '{1}'", paramID[i], paramType[i]));

                        List<PrescriptionOrderHd> lstPrescription = new List<PrescriptionOrderHd>();
                        List<PrescriptionReturnOrderHd> lstReturnPrescription = new List<PrescriptionReturnOrderHd>();
                        List<ServiceOrderHd> lstServiceOrder = new List<ServiceOrderHd>();
                        List<NutritionOrderHd> lstNutritionOrder = new List<NutritionOrderHd>();

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
                            else if (entity.OrderType == "PO")
                            {
                                PrescriptionOrderHd entityPo = new PrescriptionOrderHd();
                                entityPo = BusinessLayer.GetPrescriptionOrderHdList(String.Format("PrescriptionOrderID = {0} AND GCTransactionStatus IN ('{1}','{2}')",
                                                    entity.OrderID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL)).FirstOrDefault();

                                if (entityPo != null)
                                {
                                    lstPrescription.Add(entityPo);
                                }
                            }
                            else if (entity.OrderType == "RO")
                            {
                                PrescriptionReturnOrderHd entityRo = new PrescriptionReturnOrderHd();
                                entityRo = BusinessLayer.GetPrescriptionReturnOrderHdList(String.Format("PrescriptionReturnOrderID = {0} AND GCTransactionStatus IN ('{1}','{2}')",
                                                    entity.OrderID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL)).FirstOrDefault();

                                if (entityRo != null)
                                {
                                    lstReturnPrescription.Add(entityRo);
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
                            else if (entity.OrderType == "NO")
                            {
                                NutritionOrderHd entityNo = new NutritionOrderHd();
                                entityNo = BusinessLayer.GetNutritionOrderHdList(String.Format("NutritionOrderHdID = {0} AND GCTransactionStatus IN ('{1}','{2}')",
                                                    entity.OrderID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL)).FirstOrDefault();
                                if (entityNo != null)
                                {
                                    lstNutritionOrder.Add(entityNo);
                                }
                            }
                        }

                        #endregion

                        #region Test Order

                        if (lstTestOrder.Count > 0)
                        {
                            foreach (TestOrderHd entity in lstTestOrder)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                                {
                                    List<TestOrderDt> lstDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0}", entity.TestOrderID), ctx);
                                    foreach (TestOrderDt dt in lstDt)
                                    {
                                        TestOrderDt entityDt = entityTestOrderDtDao.Get(dt.ID);
                                        entityDt.GCTestOrderStatus = Constant.OrderStatus.OPEN;
                                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityTestOrderDtDao.Update(entityDt);
                                    }

                                    entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityTestOrderHdDao.Update(entity);

                                    UpdateLog(statusLogDao, entity.TestOrderID, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.OPEN, cboVoidReason.Value.ToString(), txtVoidOtherReason.Text);
                                }
                            }
                        }

                        #endregion

                        #region Prescription Order

                        if (lstPrescription.Count > 0)
                        {
                            foreach (PrescriptionOrderHd entity in lstPrescription)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                                {
                                    string filterDt = string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", entity.PrescriptionOrderID);
                                    List<PrescriptionOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionOrderDtList(filterDt, ctx);
                                    foreach (PrescriptionOrderDt entityDt in lstEntityDt)
                                    {
                                        entityDt.GCPrescriptionOrderStatus = Constant.OrderStatus.OPEN;
                                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityPrescriptionOrderDtDao.Update(entityDt);
                                    }

                                    entity.GCOrderStatus = Constant.OrderStatus.OPEN;
                                    entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityPrescriptionOrderHdDao.Update(entity);

                                    UpdateLog(statusLogDao, entity.PrescriptionOrderID, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.OPEN, cboVoidReason.Value.ToString(), txtVoidOtherReason.Text);
                                }
                            }
                        }

                        #endregion

                        #region Prescription Return Order

                        if (lstReturnPrescription.Count > 0)
                        {
                            foreach (PrescriptionReturnOrderHd entity in lstReturnPrescription)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                                {
                                    string filterDt = string.Format("PrescriptionReturnOrderID = {0} AND IsDeleted = 0", entity.PrescriptionReturnOrderID);
                                    List<PrescriptionReturnOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionReturnOrderDtList(filterDt, ctx);
                                    foreach (PrescriptionReturnOrderDt entityDt in lstEntityDt)
                                    {
                                        entityDt.GCPrescriptionReturnOrderStatus = Constant.OrderStatus.OPEN;
                                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityPrescriptionReturnOrderDtDao.Update(entityDt);
                                    }

                                    entity.GCOrderStatus = Constant.OrderStatus.OPEN;
                                    entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityPrescriptionReturnOrderHdDao.Update(entity);

                                    UpdateLog(statusLogDao, entity.PrescriptionReturnOrderID, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.OPEN, cboVoidReason.Value.ToString(), txtVoidOtherReason.Text);
                                }
                            }
                        }

                        #endregion

                        #region Service Order

                        if (lstServiceOrder.Count > 0)
                        {
                            foreach (ServiceOrderHd entity in lstServiceOrder)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                                {
                                    entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityServiceOrderHdDao.Update(entity);
                                }

                                UpdateLog(statusLogDao, entity.ServiceOrderID, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.OPEN, cboVoidReason.Value.ToString(), txtVoidOtherReason.Text);
                            }
                        }

                        #endregion

                        #region Nutrition Order

                        if (lstNutritionOrder.Count > 0)
                        {
                            foreach (NutritionOrderHd entity in lstNutritionOrder)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                                {
                                    List<NutritionOrderDt> lstDt = BusinessLayer.GetNutritionOrderDtList(string.Format("NutritionOrderHdID = {0} AND GCItemDetailStatus != '{1}'", entity.NutritionOrderHdID, Constant.TransactionStatus.VOID), ctx);
                                    foreach (NutritionOrderDt dt in lstDt)
                                    {
                                        NutritionOrderDt entityDt = entityNutritionOrderDtDao.Get(dt.NutritionOrderDtID);
                                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
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

                    if (AppSession.SA0137 == "1")
                    {
                        if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                        {
                            BridgingToMedinfrasV1(2, lstTestOrder);
                        }
                    }

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

        private void UpdateLog(ChargesStatusLogDao logDao, int transactionID, string oldStatus, string newStatus, string gcReason, string otherReason)
        {
            ChargesStatusLog statusLog = new ChargesStatusLog();

            statusLog.VisitID = AppSession.RegisteredPatient.VisitID;
            statusLog.TransactionID = transactionID;
            statusLog.GCReopenReason = gcReason;
            if (gcReason == Constant.ChargesChangeStatusReason.LAIN_LAIN)
            {
                statusLog.ReopenReason = otherReason;
            }
            statusLog.GCTransactionStatusOLD = oldStatus;
            statusLog.GCTransactionStatusNEW = newStatus;
            statusLog.LogDate = DateTime.Now;
            statusLog.UserID = AppSession.UserLogin.UserID;
            logDao.Insert(statusLog);
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        private void BridgingToMedinfrasV1(int ProcessType, List<TestOrderHd> lstEntity)
        {
            foreach (TestOrderHd entity in lstEntity)
            {
                APIMessageLog apiLog = new APIMessageLog();
                apiLog.MessageDateTime = DateTime.Now;
                apiLog.Sender = Constant.BridgingVendor.HIS;
                apiLog.Recipient = Constant.BridgingVendor.MEDINFRAS_API;

                MedinfrasV1Service oService = new MedinfrasV1Service();
                string serviceResult = oService.OnSendOrderMedicalDiagnosticServices(ProcessType, entity, null, null);
                string[] serviceResultInfo = serviceResult.Split('|');
                if (serviceResultInfo[0] == "1")
                {
                    apiLog.IsSuccess = true;
                    apiLog.MessageText = serviceResultInfo[1];
                    apiLog.Response = serviceResultInfo[2];
                }
                else
                {
                    apiLog.IsSuccess = false;
                    apiLog.MessageText = serviceResultInfo[1];
                    apiLog.Response = serviceResultInfo[2];
                    apiLog.ErrorMessage = serviceResultInfo[2];
                }
                BusinessLayer.InsertAPIMessageLog(apiLog);
            }
        }

        private void CopyHeaderObject(PrescriptionOrderHd source, ref PrescriptionOrderHdOriginal destination)
        {
            var fields = source.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var value = field.GetValue(source);
                destination.GetType().GetProperty(field.Name.Replace("_", "")).SetValue(destination, value, null);
            }
        }

        private void CopyDetailObject(PrescriptionOrderDt source, ref PrescriptionOrderDtOriginal destination)
        {
            var fields = source.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var value = field.GetValue(source);
                destination.GetType().GetProperty(field.Name.Replace("_", "")).SetValue(destination, value, null);
            }
        }
    }
}