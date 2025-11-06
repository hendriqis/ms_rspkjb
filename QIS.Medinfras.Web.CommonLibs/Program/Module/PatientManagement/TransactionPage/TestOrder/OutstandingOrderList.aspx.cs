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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class OutstandingOrderList : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            string param = Page.Request.QueryString["id"];
            if (param == "MD")
            {
                return Constant.MenuCode.MedicalDiagnostic.PATIENT_OUTSTANDING_ORDER_LIST_MD035802;
            }
            else if (param == "ER")
            {
                return Constant.MenuCode.EmergencyCare.PATIENT_OUTSTANDING_ORDER_LIST_ER020172;
            }
            else
            {
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_OUTSTANDING_ORDER_LIST;
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PATIENT_OUTSTANDING_ORDER_LIST;
                    case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.PATIENT_PAGE_OUTSTANDING_ORDER;
                    case Constant.Facility.LABORATORY: return Constant.MenuCode.Laboratory.PATIENT_OUTSTANDING_ORDER_LIST;
                    case Constant.Facility.DIAGNOSTIC:
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                            return Constant.MenuCode.Imaging.PATIENT_OUTSTANDING_ORDER_LIST;
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                            return Constant.MenuCode.Laboratory.PATIENT_OUTSTANDING_ORDER_LIST;
                        return Constant.MenuCode.MedicalDiagnostic.PATIENT_OUTSTANDING_ORDER_LIST;
                    default: return Constant.MenuCode.MedicalDiagnostic.PATIENT_OUTSTANDING_ORDER_LIST;
                }
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
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnClassID.Value = entity.ClassID.ToString();
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("VisitID = {0} AND GCTransactionStatus IN ('{1}','{2}')", hdnVisitID.Value, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
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
            NutritionOrderHdDao entityNutritionOrderHdDao = new NutritionOrderHdDao(ctx);
            NutritionOrderDtDao entityNutritionOrderDtDao = new NutritionOrderDtDao(ctx);

            try
            {
                if (type == "void")
                {
                    List<TestOrderHd> lstTestOrder = new List<TestOrderHd>();
                    List<PrescriptionOrderHd> lstPrescription = new List<PrescriptionOrderHd>();

                    #region VOID

                    for (int i = 0; i < paramID.Length; i++)
                    {
                        List<vPatientOrderAll> lstEntity = BusinessLayer.GetvPatientOrderAllList(String.Format("OrderID IN ({0}) AND OrderType = '{1}'", paramID[i], paramType[i]));

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
                                    entity.VoidBy = AppSession.UserLogin.UserID;
                                    entity.VoidDate = DateTime.Now;
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
                                    entity.VoidBy = AppSession.UserLogin.UserID;
                                    entity.VoidDate = DateTime.Now;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityPrescriptionOrderHdDao.Update(entity);
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
                                    entity.VoidBy = AppSession.UserLogin.UserID;
                                    entity.VoidDate = DateTime.Now;
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
                            if (lstTestOrder.Count > 0)
                            {
                                BridgingToMedinfrasV1(3, lstTestOrder);
                            }

                            if (lstPrescription.Count > 0)
                            {
                                foreach (PrescriptionOrderHd hd in lstPrescription)
                                {
                                    string filterExpression = string.Format("PrescriptionOrderID = {0} AND (IsAllergyAlert = 1 OR IsAdverseReactionAlert = 1 OR IsDuplicateTheraphyAlert = 1) AND IsAlertConfirmed = 0", hd.PrescriptionOrderID);
                                    List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression);
                                    if (lstEntity.Count > 0)
                                    {
                                        BridgingToMedinfrasV1Prescription(3, hd, lstEntity);
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                }
                else if (type == "propose")
                {
                    #region PROPOSED

                    List<TestOrderHd> lstTestOrder = new List<TestOrderHd>();
                    List<PrescriptionOrderHd> lstPrescription = new List<PrescriptionOrderHd>();
                    for (int i = 0; i < paramID.Length; i++)
                    {
                        List<vPatientOrderAll> lstEntity = BusinessLayer.GetvPatientOrderAllList(String.Format("OrderID IN ({0}) AND OrderType = '{1}'", paramID[i], paramType[i]));

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
                                string filterPo = String.Format("PrescriptionOrderID = {0} AND GCTransactionStatus = '{1}'", entity.OrderID, Constant.TransactionStatus.OPEN);
                                if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                                {
                                    if (AppSession.SA0137 == "1")
                                    {
                                        filterPo = String.Format("PrescriptionOrderID = {0} AND GCTransactionStatus != '{1}'", entity.OrderID, Constant.TransactionStatus.VOID);
                                    }
                                }

                                entityPo = BusinessLayer.GetPrescriptionOrderHdList(filterPo).FirstOrDefault();
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
                                    string filterDt = string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", entity.PrescriptionOrderID);
                                    List<PrescriptionOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionOrderDtList(filterDt, ctx);
                                    foreach (PrescriptionOrderDt entityDt in lstEntityDt)
                                    {
                                        entityDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityPrescriptionOrderDtDao.Update(entityDt);
                                    }

                                    entity.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    entity.SendOrderBy = AppSession.UserLogin.UserID;
                                    entity.SendOrderDateTime = DateTime.Now.Date;
                                    entity.ProposedBy = AppSession.UserLogin.UserID;
                                    entity.ProposedDate = DateTime.Now;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityPrescriptionOrderHdDao.Update(entity);
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

                        if (lstPrescription.Count > 0)
                        {
                            foreach (PrescriptionOrderHd hd in lstPrescription)
                            {
                                string filterExpression = string.Format("PrescriptionOrderID = {0} AND OrderIsDeleted = 0 AND (IsAllergyAlert = 1 OR IsAdverseReactionAlert = 1 OR IsDuplicateTheraphyAlert = 1) AND IsAlertConfirmed = 0", hd.PrescriptionOrderID);
                                List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression);
                                if (lstEntity.Count > 0)
                                {
                                    BridgingToMedinfrasV1Prescription(1, hd, lstEntity);
                                }
                            }
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        private void BridgingToMedinfrasV1(int ProcessType, List<TestOrderHd> lstTestOrder)
        {
            APIMessageLog apiLog = new APIMessageLog();
            apiLog.MessageDateTime = DateTime.Now;
            apiLog.Sender = Constant.BridgingVendor.HIS;
            apiLog.Recipient = Constant.BridgingVendor.MEDINFRAS_API;

            MedinfrasV1Service oService = new MedinfrasV1Service();
            foreach (TestOrderHd entity in lstTestOrder)
            {
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


        private void BridgingToMedinfrasV1Prescription(int ProcessType, PrescriptionOrderHd entity, List<vPrescriptionOrderDt1> lstEntity)
        {
            APIMessageLog apiLog = new APIMessageLog();
            apiLog.MessageDateTime = DateTime.Now;
            apiLog.Sender = Constant.BridgingVendor.HIS;
            apiLog.Recipient = Constant.BridgingVendor.MEDINFRAS_API;

            MedinfrasV1Service oService = new MedinfrasV1Service();
            string serviceResult = oService.OnSendOrderMedicalDiagnosticServices(ProcessType, null, entity, lstEntity);
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
}