using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NursingProcessList1 : BasePagePatientPageList
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EmergencyCare.NURSING_ASSESSMENT_PROCESS;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override bool IsEntryUsePopup()
        {
            return false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            BindGridView(1, true, ref PageCount);
            BindGridViewDt(1, true, ref PageCount);
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = true;
            IsAllowEdit = false;
            IsAllowDelete = false;
        }

        #region Nursing Transaction Hd
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            hdnIsOutstandingOrder.Value = "0";
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            String TransactionStatus = String.Format("'{0}','{1}'", Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            filterExpression += string.Format("VisitID = {0} AND GCTransactionStatus NOT IN ({1})", AppSession.RegisteredPatient.VisitID, TransactionStatus);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvNursingTransactionHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vNursingTransactionHd> lstEntity = BusinessLayer.GetvNursingTransactionHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TransactionID DESC");
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

        #region Nursing Transaction Dt
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnID.Value != "")
            {
                filterExpression = string.Format("TransactionID = {0}", hdnID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvNursingTransactionDtRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }

            List<vNursingTransactionDt> lstEntity = BusinessLayer.GetvNursingTransactionDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID");
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
                url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Nursing/Assessment/Process/NursingProcessEntry1.aspx");
                result = true;
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
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    List<PrescriptionOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", entityHd.PrescriptionOrderID), ctx);
                    foreach (PrescriptionOrderDt entityDt in lstEntityDt)
                    {
                        entityDt.IsDeleted = true;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
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
            return false;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityOrderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityOrderDtDao = new PrescriptionOrderDtDao(ctx);

            try
            {
                PrescriptionOrderHd entity = entityOrderHdDao.Get(Convert.ToInt32(hdnID.Value)); ;

                if (type == "Propose")
                {
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        string filterExpression = string.Format("PrescriptionOrderID = {0} AND ParentID IS NULL AND OrderIsDeleted = 0 AND (IsAllergyAlert = 1 OR IsAdverseReactionAlert = 1 OR IsDuplicateTheraphyAlert = 1)", hdnID.Value);
                        List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression, ctx);

                        if (lstEntity.Count > 0)
                        {
                            errMessage = "There is item(s) should be confirmed due to allergy, adverse reaction and duplicate theraphy.";
                            result = false;
                        }
                        else
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entity.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                            entity.SendOrderDateTime = DateTime.Now;
                            entity.SendOrderBy = AppSession.UserLogin.UserID;
                            entity.ProposedBy = AppSession.UserLogin.UserID;
                            entity.ProposedDate = DateTime.Now;
                            entityOrderHdDao.Update(entity);

                            filterExpression = string.Format("PrescriptionOrderID = {0} AND OrderIsDeleted = 0", hdnID.Value);
                            lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression, ctx);

                            foreach (vPrescriptionOrderDt1 item in lstEntity)
                            {
                                PrescriptionOrderDt orderDt = entityOrderDtDao.Get(item.PrescriptionOrderDetailID);
                                if (orderDt != null)
                                {
                                    orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
                                    entityOrderDtDao.Update(orderDt);
                                }
                            }

                            hdnIsOutstandingOrder.Value = "0";
                            result = true;
                        }

                        //try
                        //{
                        //    HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(entity.DispensaryServiceUnitID));
                        //    string ipAddress = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;

                        //    if (!String.IsNullOrEmpty(ipAddress))
                        //    {
                        //        SendNotification(entity,ipAddress,"6000");
                        //    }
                        //}
                        //catch (Exception)
                        //{
                        //}e
                    }
                }
                else if (type == "ReOpen")
                {
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.GCOrderStatus = Constant.OrderStatus.OPEN;
                        entityOrderHdDao.Update(entity);

                        string filterExpression = string.Format("PrescriptionOrderID = {0} AND OrderIsDeleted = 0", hdnID.Value);
                        List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression, ctx);

                        foreach (vPrescriptionOrderDt1 item in lstEntity)
                        {
                            PrescriptionOrderDt orderDt = entityOrderDtDao.Get(item.PrescriptionOrderDetailID);
                            if (orderDt != null)
                            {
                                orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.OPEN;
                                entityOrderDtDao.Update(orderDt);
                            }
                        }

                        hdnIsOutstandingOrder.Value = "1";
                        result = true;
                    }
                    else
                    {
                        errMessage = "Status Order sudah berubah, haraf refresh halaman ini";
                        result = false;
                    }
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
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
            }
        }
    }
}