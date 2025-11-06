using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PrescriptionList1 : BasePagePatientPageList
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.ONLINE_PRESCRIPTION;
        }

        public override bool IsEntryUsePopup()
        {
            return false;
        }

        public override bool IsShowRightPanel()
        {
            return true;
        }

        protected override void InitializeDataControl()
        {
            hdnCurrentUserID.Value = AppSession.UserLogin.UserID.ToString();
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();

            string filterSetvar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_USING_DRUG_ALERT, Constant.SettingParameter.EM_IS_DOCTOR_ALLOW_CHANGE_ANOTHER_DOCTOR_PRESCRIPTION);
            List<SettingParameterDt> lstSetvarDT = BusinessLayer.GetSettingParameterDtList(filterSetvar);
            hdnIsUsingDrugAlert.Value = lstSetvarDT.Where(t => t.ParameterCode == Constant.SettingParameter.IS_USING_DRUG_ALERT).FirstOrDefault().ParameterValue;
            hdnIsAllowChangeAnotherDoctorPrescription.Value = lstSetvarDT.Where(t => t.ParameterCode == Constant.SettingParameter.EM_IS_DOCTOR_ALLOW_CHANGE_ANOTHER_DOCTOR_PRESCRIPTION).FirstOrDefault().ParameterValue;

            BindGridView(1, true, ref PageCount);
            BindGridViewDt(1, true, ref PageCount);
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = true;
            IsAllowEdit = false;
            IsAllowDelete = false;
        }

        #region Prescription Hd
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            hdnIsOutstandingOrder.Value = "0";
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            String TransactionStatus = String.Format("'{0}','{1}'", Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            filterExpression += string.Format("VisitID = {0} AND GCTransactionStatus NOT IN ({1}) AND ISNULL(ChargesGCTransactionStatus,'') NOT IN ('{2}')", AppSession.RegisteredPatient.VisitID, TransactionStatus, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPrescriptionOrderHd5RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPrescriptionOrderHd5> lstEntity = BusinessLayer.GetvPrescriptionOrderHd5List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionOrderID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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


        protected void cbpProposed_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (e.Parameter != null && e.Parameter != "")
            {
                int PrescriptionOrderID = Convert.ToInt32(e.Parameter);
            }
        }
        #endregion

        #region Prescription Dt
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnID.Value != "")
            {
                filterExpression = string.Format("PrescriptionOrderID = {0} AND ParentID IS NULL", hdnID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPrescriptionOrderDt1RowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            string code = ddlViewType.SelectedValue;

            if (code == "1")
                filterExpression += string.Format(" AND GCPrescriptionOrderStatus IN ('{0}','{1}','{2}') AND TakenQty > 0", Constant.OrderStatus.IN_PROGRESS, Constant.OrderStatus.COMPLETED, Constant.OrderStatus.CLOSED);
            else if (code == "2")
                filterExpression += string.Format(" AND GCPrescriptionOrderStatus IN ('{0}','{1}','{2}') AND TakenQty = 0", Constant.OrderStatus.IN_PROGRESS, Constant.OrderStatus.COMPLETED, Constant.OrderStatus.CLOSED);
            else if (code == "3")
                filterExpression += string.Format(" AND GCPrescriptionOrderStatus IN ('{0}')", Constant.OrderStatus.CANCELLED);

            List<vPrescriptionOrderDt10> lstEntity = BusinessLayer.GetvPrescriptionOrderDt10List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionOrderDetailID");
            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind();
        }
        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        protected override bool OnBeforeEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PrescriptionOrderHd entity = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnID.Value));
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
                {
                    errMessage = "Maaf, order resep tidak bisa dilakukan karena sudah dikirim ke farmasi, harap lakukan <b>REOPEN</b> terlebih dahulu.";
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
            }

            return result;
        }

        protected override bool OnBeforeDeleteRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PrescriptionOrderHd entity = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnID.Value));
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    errMessage = "Maaf, order resep tidak bisa dihapus karena sudah diproses oleh farmasi.";
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
            }

            return result;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            bool result = true;
            Registration oRegistration = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
            if (oRegistration != null)
            {
                if (oRegistration.GCRegistrationStatus == Constant.VisitStatus.CLOSED)
                {
                    errMessage = "Registration has already been closed.";
                    result = false;
                }

                if (oRegistration.IsLockDown != true)
                {
                    url = ResolveUrl("~/Program/PatientPage/Planning/Prescription/PrescriptionEntry1.aspx");
                    result = true;
                }
                else
                {
                    errMessage = "Transaction is currently being locked for Patient Billing Process.";
                    result = false;
                }
            }
            return result;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {

            bool result = true;
            if (hdnID.Value != "")
            {
                Registration oRegistration = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
                if (oRegistration != null)
                {
                    if (oRegistration.GCRegistrationStatus == Constant.VisitStatus.CLOSED)
                    {
                        errMessage = "Registration has already been closed.";
                        result = false;
                    }

                    if (oRegistration.IsLockDown != true)
                    {
                        url = ResolveUrl(string.Format("~/Program/PatientPage/Planning/Prescription/PrescriptionEntry1.aspx?id={0}", hdnID.Value));
                        result = true;
                    }
                    else
                    {
                        errMessage = "Transaction is currently being locked for Patient Billing Process.";
                        result = false;
                    }
                }
            }
            return result;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
                PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
                try
                {
                    PrescriptionOrderHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnID.Value));
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entityHd.GCOrderStatus = Constant.OrderStatus.CANCELLED;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    List<PrescriptionOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", entityHd.PrescriptionOrderID), ctx);
                    foreach (PrescriptionOrderDt entityDt in lstEntityDt)
                    {
                        entityDt.IsDeleted = true;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                    }

                    #region Log PrescriptionTaskOrder
                    Helper.InsertPrescriptionOrderTaskLog(ctx, entityHd.PrescriptionOrderID, Constant.PrescriptionTaskLogStatus.Void, AppSession.UserLogin.UserID, false);
                    #endregion

                    ctx.CommitTransaction();

                    if (AppSession.SA0137 == "1")
                    {
                        if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                        {
                            List<vPrescriptionOrderDt1> lstDt = BusinessLayer.GetvPrescriptionOrderDt1List(string.Format("PrescriptionOrderID = {0} AND OrderIsDeleted = 0", entityHd.PrescriptionOrderID));
                            BridgingToMedinfrasV1(3, entityHd, lstDt);
                        }
                    }
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
            return false;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            int emrV1ProcessType = 0;

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityOrderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityOrderDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderHdOriginalDao entityOrderHdOriginalDao = new PrescriptionOrderHdOriginalDao(ctx);
            PrescriptionOrderDtOriginalDao entityOrderDtOriginalDao = new PrescriptionOrderDtOriginalDao(ctx);

            List<PrescriptionOrderDt> lstEntity = new List<PrescriptionOrderDt>();
            List<vPrescriptionOrderDt1> lstEntityAPI = new List<vPrescriptionOrderDt1>();
            try
            {
                PrescriptionOrderHd entity = entityOrderHdDao.Get(Convert.ToInt32(hdnID.Value));

                if (type == "Propose")
                {
                    #region Proposed

                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        string filterExpression = string.Format("PrescriptionOrderID = {0} AND ParentID IS NULL AND IsDeleted = 0 AND (IsAllergyAlert = 1 OR IsAdverseReactionAlert = 1 OR IsDuplicateTheraphyAlert = 1)", hdnID.Value);
                        lstEntity = BusinessLayer.GetPrescriptionOrderDtList(filterExpression, ctx);

                        string filterExpressionAPI = string.Format("PrescriptionOrderID = {0} AND ParentID IS NULL AND OrderIsDeleted = 0 AND (IsAllergyAlert = 1 OR IsAdverseReactionAlert = 1 OR IsDuplicateTheraphyAlert = 1)", hdnID.Value);
                        lstEntityAPI = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpressionAPI, ctx);

                        if (lstEntity.Count > 0)
                        {
                            errMessage = "There is item(s) should be confirmed due to allergy, adverse reaction and duplicate theraphy.";
                            result = false;
                        }
                        else
                        {

                            bool isValidToProceed = true;
                            if (AppSession.EM0088 == "1")
                            {
                                filterExpression = string.Format("PrescriptionOrderID = {0} AND OrderIsDeleted = 0 AND IsRestrictiveAntibiotics = 1 AND (PPRAFormStatus NOT IN ('1','2') OR PPRAFormStatus IS NULL)", hdnID.Value);
                                List<vPrescriptionOrderDt1> lstPPRAItem = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression, ctx);
                                if (lstPPRAItem.Count > 0)
                                {
                                    if (hdnCurrentParamedicID.Value == hdnParamedicID.Value)
                                    {
                                        errMessage = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", "Terdapat item yang termasuk dalam kategori Program Pengendalian Resistensi Antimikroba (PPRA) yang perlu dilakukan pengisian Form PPRA", "confirmPPRA", hdnID.Value, "Program Pengendalian Resistensi Antimikroba (PPRA)", "800", "600");
                                        result = false;
                                        isValidToProceed = false;
                                    }
                                    else
                                    {
                                        errMessage = "Proses Kirim Order ke Farmasi tidak dapat dilakukan karena nomor resep ini terdapat item yang termasuk dalam kategori Program Pengendalian Resistensi Antimikroba (PPRA) yang perlu dilakukan pengisian Form PPRA oleh Dokter yang melakukan peresepan";
                                        result = false;
                                        isValidToProceed = false;
                                    }
                                }
                            }

                            if (isValidToProceed)
                            {
                                entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                entity.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                                entity.SendOrderDateTime = DateTime.Now;
                                entity.SendOrderBy = AppSession.UserLogin.UserID;
                                entity.ProposedBy = AppSession.UserLogin.UserID;
                                entity.ProposedDate = DateTime.Now;
                                entityOrderHdDao.Update(entity);

                                //Log : Copy of Current Prescription Order
                                int historyID = 0;
                                if (entity.IsOrderedByPhysician)
                                {
                                    PrescriptionOrderHdOriginal originalHd = new PrescriptionOrderHdOriginal();
                                    CopyHeaderObject(entity, ref originalHd);
                                    historyID = entityOrderHdOriginalDao.InsertReturnPrimaryKeyID(originalHd);
                                }

                                filterExpression = string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", hdnID.Value);
                                lstEntity = BusinessLayer.GetPrescriptionOrderDtList(filterExpression, ctx);

                                List<PrescriptionOrderDtOriginal> lstOriginalDt = new List<PrescriptionOrderDtOriginal>();

                                foreach (PrescriptionOrderDt item in lstEntity)
                                {
                                    PrescriptionOrderDt orderDt = entityOrderDtDao.Get(item.PrescriptionOrderDetailID);
                                    if (orderDt != null)
                                    {
                                        orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
                                        entityOrderDtDao.Update(orderDt);
                                        if (historyID > 0)
                                        {
                                            PrescriptionOrderDtOriginal originalDt = new PrescriptionOrderDtOriginal();
                                            CopyDetailObject(orderDt, ref originalDt);
                                            ////originalDt.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
                                            originalDt.HistoryHeaderID = historyID;
                                            lstOriginalDt.Add(originalDt);
                                        }
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

                                #region Log PrescriptionTaskOrder
                                Helper.InsertPrescriptionOrderTaskLog(ctx, entity.PrescriptionOrderID, Constant.PrescriptionTaskLogStatus.Sent, AppSession.UserLogin.UserID, false);
                                #endregion

                                hdnIsOutstandingOrder.Value = "0";
                                result = true;
                            }
                        }

                        emrV1ProcessType = 1;

                        if (AppSession.IsPrintPHOrderTracerFromEMR)
                        {
                            PrintOrderTracer(entity.PrescriptionOrderID);
                        }
                    }

                    #endregion
                }
                else if (type == "ReOpen")
                {
                    #region ReOpen

                    if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.GCOrderStatus = Constant.OrderStatus.OPEN;
                        entityOrderHdDao.Update(entity);

                        string filterExpression = string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", hdnID.Value);
                        lstEntity = BusinessLayer.GetPrescriptionOrderDtList(filterExpression, ctx);
                        foreach (PrescriptionOrderDt item in lstEntity)
                        {
                            PrescriptionOrderDt orderDt = entityOrderDtDao.Get(item.PrescriptionOrderDetailID);
                            if (orderDt != null)
                            {
                                orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.OPEN;
                                entityOrderDtDao.Update(orderDt);
                            }
                        }

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        string filterHdOri = string.Format("PrescriptionOrderID = {0} AND GCTransactionStatus <> '{1}'", entity.PrescriptionOrderID, Constant.TransactionStatus.VOID);
                        List<PrescriptionOrderHdOriginal> lstHdOri = BusinessLayer.GetPrescriptionOrderHdOriginalList(filterHdOri, ctx);
                        foreach (PrescriptionOrderHdOriginal hdOri in lstHdOri)
                        {
                            hdOri.GCTransactionStatus = Constant.TransactionStatus.VOID;
                            hdOri.GCOrderStatus = Constant.OrderStatus.CANCELLED;
                            hdOri.GCVoidReason = Constant.DeleteReason.OTHER;
                            hdOri.VoidReason = "Re-open Prescription Order from EMR.";
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
                                dtOri.GCVoidReason = Constant.DeleteReason.OTHER;
                                dtOri.VoidReason = "Re-open Prescription Order from EMR.";
                                dtOri.VoidBy = AppSession.UserLogin.UserID;
                                dtOri.VoidDateTime = DateTime.Now;
                                dtOri.LastUpdatedBy = AppSession.UserLogin.UserID;
                                dtOri.LastUpdatedDate = DateTime.Now;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityOrderDtOriginalDao.Update(dtOri);
                            }
                        }

                        hdnIsOutstandingOrder.Value = "1";
                        result = true;

                        #region Log PrescriptionTaskOrder
                        Helper.InsertPrescriptionOrderTaskLog(ctx, entity.PrescriptionOrderID, Constant.PrescriptionTaskLogStatus.Reopen, AppSession.UserLogin.UserID, false);
                        #endregion

                        emrV1ProcessType = 2;
                    }
                    else
                    {
                        errMessage = "Status Order sudah berubah, haraf refresh halaman ini";
                        result = false;
                    }

                    #endregion
                }

                ctx.CommitTransaction();

                if (AppSession.SA0137 == "1")
                {
                    if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                    {
                        BridgingToMedinfrasV1(emrV1ProcessType, entity, lstEntityAPI);
                    }
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

        private string PrintOrderTracer(int prescriptionOrderID)
        {

            string result = string.Empty;
            Healthcare entityHSU = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.ORDER_FARMASI);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

            string printerUrl1 = "";
            if (lstPrinter.Count > 0)
            {
                printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;
            }

            if (entityHSU.Initial == "RSPR") //RSPR
            {
                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
                {
                    ZebraPrinting.PrintOrderFarmasiRSPR(prescriptionOrderID, printerUrl1);
                }
            }
            else if (entityHSU.Initial == "RSDI") //RSDI
            {
                SettingParameterDt oSetPar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode='{0}'", Constant.SettingParameter.PH0071)).FirstOrDefault();
                printerUrl1 = oSetPar.ParameterValue;
                ZebraPrinting.PrintOrderFarmasiRSDI(prescriptionOrderID, printerUrl1);
            }
            else if (entityHSU.Initial == "RSUKI") //RSUKI
            {
                ZebraPrinting.PrintOrderFarmasiRSUKI(prescriptionOrderID, printerUrl1);
            }
            else if (entityHSU.Initial == "JWCC")
            {
                SettingParameterDt oSetPar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode='{0}'", Constant.SettingParameter.PH0071)).FirstOrDefault();
                printerUrl1 = oSetPar.ParameterValue;
                ZebraPrinting.PrintOrderFarmasiJWCC(prescriptionOrderID, printerUrl1);
            }
            else if (entityHSU.Initial == "RSRT")
            {
                List<SettingParameterDt> lstSeparDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID='{0}' AND ParameterCode IN('{1}','{2}')",
                   AppSession.UserLogin.HealthcareID,
                  Constant.SettingParameter.PH0071, //rajal
                  Constant.SettingParameter.PH0078 //ranap
                  ));

                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
                {
                    SettingParameterDt oSetPar = lstSeparDt.Where(p => p.ParameterCode == Constant.SettingParameter.PH0078).FirstOrDefault();
                    if (oSetPar != null)
                    {
                        printerUrl1 = oSetPar.ParameterValue;
                        if (!string.IsNullOrEmpty(printerUrl1) && printerUrl1 != "-")  /// tanda (-) untuk kosongin supaya tidak baca
                        {
                            ZebraPrinting.PrintOrderFarmasiRSRT(prescriptionOrderID, printerUrl1);
                        }
                    }

                }
                else
                {

                    SettingParameterDt oSetPar = lstSeparDt.Where(p => p.ParameterCode == Constant.SettingParameter.PH0071).FirstOrDefault();
                    if (oSetPar != null)
                    {
                        printerUrl1 = oSetPar.ParameterValue;
                        if (!string.IsNullOrEmpty(printerUrl1))
                        {
                            ZebraPrinting.PrintOrderFarmasiRSRT(prescriptionOrderID, printerUrl1);
                        }

                    }
                }
            }
            else if (entityHSU.Initial == "PHS")
            {
                List<SettingParameterDt> lstOSetPar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.PH0071, Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN));

                SettingParameterDt oSetPar = lstOSetPar.Where(t => t.ParameterCode == Constant.SettingParameter.PH0071).FirstOrDefault();
                string[] printUrl = oSetPar.ParameterValue.Split('|');
                string bpjsID = lstOSetPar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault().ParameterValue;
                Customer bp = BusinessLayer.GetCustomer(AppSession.RegisteredPatient.BusinessPartnerID);
                printerUrl1 = printUrl[0];

                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
                {
                    printerUrl1 = printUrl[2];
                }
                else
                {
                    if (bp.GCCustomerType == Constant.CustomerType.BPJS)
                    {
                        printerUrl1 = printUrl[1];
                    }
                }
                ZebraPrinting.PrintOrderFarmasiRSPM(prescriptionOrderID, printerUrl1);
            }
            else if (entityHSU.Initial == "BROS")
            {
                List<SettingParameterDt> lstOSetPar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.PH0071, Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN));

                SettingParameterDt oSetPar = lstOSetPar.Where(t => t.ParameterCode == Constant.SettingParameter.PH0071).FirstOrDefault();
                string[] printUrl = oSetPar.ParameterValue.Split('|');
                string bpjsID = lstOSetPar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault().ParameterValue;
                Customer bp = BusinessLayer.GetCustomer(AppSession.RegisteredPatient.BusinessPartnerID);
                printerUrl1 = printUrl[0];

                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
                {
                    printerUrl1 = printUrl[2];
                }
                else if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.EMERGENCY)
                {
                    printerUrl1 = printUrl[0];
                }
                else
                {
                    if (bp.GCCustomerType == Constant.CustomerType.BPJS)
                    {
                        printerUrl1 = printUrl[0];
                    }
                    else
                    {
                        printerUrl1 = printUrl[1];
                    }
                }
                ZebraPrinting.PrintOrderFarmasi(prescriptionOrderID, printerUrl1);
            }
            else
            {
                //if (entityHSU.Initial == "DEMO")
                //{
                //    ZebraPrinting.PrintOrderFarmasi(prescriptionOrderID, printerUrl1);
                //}
                ZebraPrinting.PrintOrderFarmasi(prescriptionOrderID, printerUrl1);
            }
            return result;
        }

        private void SendNotification(PrescriptionOrderHd order, string ipAddress, string port)
        {
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.AppendLine(string.Format("No : {0}", order.PrescriptionOrderNo));
            sbMessage.AppendLine(string.Format("Fr : {0}", AppSession.UserLogin.UserFullName));
            sbMessage.AppendLine(string.Format("Px : {0}", AppSession.RegisteredPatient.PatientName));
            sbMessage.AppendLine(string.Format("R/ :    "));
            sbMessage.AppendLine(string.Format("{0}", order.Remarks));
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse(ipAddress), Convert.ToInt16(port));
            NetworkStream stream = client.GetStream();
            using (BinaryWriter w = new BinaryWriter(stream))
            {
                using (BinaryReader r = new BinaryReader(stream))
                {
                    w.Write(string.Format(@"{0}", sbMessage.ToString()).ToCharArray());
                }
            }
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPrescriptionOrderHd5 entity = e.Row.DataItem as vPrescriptionOrderHd5;
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    hdnIsOutstandingOrder.Value = "1";
                }
            }
        }

        protected void grdViewDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPrescriptionOrderDt10 entity = e.Row.DataItem as vPrescriptionOrderDt10;

                HtmlImage imgHAM = e.Row.FindControl("imgHAM") as HtmlImage;
                if (imgHAM != null)
                {
                    imgHAM.Visible = entity.IsHAM;
                }

                HtmlImage imgIsHasRestriction = e.Row.FindControl("imgIsHasRestriction") as HtmlImage;
                HtmlInputText lblHasRestrictionInformation = e.Row.FindControl("lblHasRestrictionInformation") as HtmlInputText;
                if (imgIsHasRestriction != null)
                {
                    if (entity.IsHasRestrictionInformation == true)
                    {
                        if (!string.IsNullOrEmpty(entity.RestrictionInformation))
                        {
                            imgIsHasRestriction.Attributes["title"] = string.Format("{0}", entity.RestrictionInformation);
                            imgIsHasRestriction.Visible = true;
                            if (lblHasRestrictionInformation != null)
                            {
                                lblHasRestrictionInformation.Value = "1";
                            }
                        }
                        else
                        {
                            imgIsHasRestriction.Visible = true;
                            imgIsHasRestriction.Attributes["title"] = string.Format("Drug Restriction");
                            if (lblHasRestrictionInformation != null)
                            {
                                lblHasRestrictionInformation.Value = "0";
                            }
                        }
                    }
                    else
                    {
                        imgIsHasRestriction.Visible = false;
                    }
                }

                HtmlImage imgPPRA = e.Row.FindControl("imgPPRA") as HtmlImage;
                if (imgPPRA != null)
                {
                    if (entity.IsRestrictiveAntibiotics == true)
                    {
                        imgPPRA.Visible = true;
                    }
                    else
                    {
                        imgPPRA.Visible = false;
                    }
                }
            }
        }

        private void BridgingToMedinfrasV1(int ProcessType, PrescriptionOrderHd entity, List<vPrescriptionOrderDt1> lstEntity)
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