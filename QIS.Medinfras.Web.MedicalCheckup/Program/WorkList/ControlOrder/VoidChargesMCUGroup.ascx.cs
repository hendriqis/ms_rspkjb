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
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class VoidChargesMCUGroup : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            string[] temp = param.Split('|');
            txtRegistrationDate.Text = temp[0];
            
            BindGridView();
        }

        protected void lvwViewDt_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vConsultVisitMCUItem entity = e.Item.DataItem as vConsultVisitMCUItem;
                HtmlInputHidden hdnKey = (HtmlInputHidden)e.Item.FindControl("hdnKey");
                HtmlInputHidden hdnRegistrationID = (HtmlInputHidden)e.Item.FindControl("hdnRegistrationID");
                HtmlInputHidden hdnVisitID = (HtmlInputHidden)e.Item.FindControl("hdnVisitID");
                HtmlInputHidden hdnRegistrationNo = (HtmlInputHidden)e.Item.FindControl("hdnRegistrationNo");
                HtmlInputHidden hdnPatientName = (HtmlInputHidden)e.Item.FindControl("hdnPatientName");
                HtmlInputHidden hdnItemID = (HtmlInputHidden)e.Item.FindControl("hdnItemID");
                HtmlInputHidden hdnItemCode = (HtmlInputHidden)e.Item.FindControl("hdnItemCode");
                HtmlInputHidden hdnItemName1 = (HtmlInputHidden)e.Item.FindControl("hdnItemName1");
                hdnKey.Value = entity.ID.ToString();
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnVisitID.Value = entity.VisitID.ToString();
                hdnRegistrationNo.Value = entity.RegistrationNo;
                hdnPatientName.Value = entity.PatientName;
                hdnItemID.Value = entity.ItemID.ToString();
                hdnItemCode.Value = entity.ItemCode;
                hdnItemName1.Value = entity.ItemName1;
            }
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
            else if (param == "void")
            {
                if (ValidateMCUResultFormExist(ref errMessage))
                {
                    if (OnVoidRegistrationCharges(ref errMessage))
                    {
                        result += "success";
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("VisitDate = '{0}' AND GCItemDetailStatus = '{1}' AND GCRegistrationStatus = '{2}' AND BusinessPartnerID != 1 AND RegistrationID IN (SELECT RegistrationID FROM Registration WHERE PaymentAmount = 0 AND RegistrationDate = '{0}') AND VisitID IN (SELECT VisitID FROM PatientChargesHd WHERE GCTransactionStatus != '{3}' AND TransactionDate = '{0}')", Helper.GetDatePickerValue(txtRegistrationDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.PROCESSED, Constant.VisitStatus.CHECKED_IN, Constant.TransactionStatus.VOID);
            filterExpression += " ORDER BY RegistrationID, ID";

            List<vConsultVisitMCUItem> lstEntity = BusinessLayer.GetvConsultVisitMCUItemList(filterExpression);
            lvwViewDt.DataSource = lstEntity;
            lvwViewDt.DataBind();
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

        private bool OnVoidRegistrationCharges(ref string errMessage) 
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            TestOrderHdDao entityOrderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityOrderDtDao = new TestOrderDtDao(ctx);
            ServiceOrderHdDao entityServiceHdDao = new ServiceOrderHdDao(ctx);
            ServiceOrderDtDao entityServiceDtDao = new ServiceOrderDtDao(ctx);

            try
            {
                List<PatientChargesHd> lstChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("VisitID IN ({0}) AND GCTransactionStatus NOT IN ('{1}')", hdnSelectedMemberDtVisitID.Value, Constant.TransactionStatus.VOID), ctx);
                if (lstChargesHd.Count > 0)
                {
                    foreach (PatientChargesHd hd in lstChargesHd)
                    {
                        hd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        hd.VoidBy = AppSession.UserLogin.UserID;
                        hd.VoidDate = DateTime.Now;
                        hd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        hd.LastUpdatedDate = DateTime.Now;
                        entityChargesHdDao.Update(hd);

                        List<PatientChargesDt> lstChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND GCTransactionDetailStatus NOT IN ('{1}')", hd.TransactionID, Constant.TransactionStatus.VOID), ctx);
                        foreach (PatientChargesDt dt in lstChargesDt)
                        {
                            dt.IsApproved = false;
                            dt.IsDeleted = false;
                            dt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            dt.DeleteBy = AppSession.UserLogin.UserID;
                            dt.DeleteDate = DateTime.Now;
                            dt.GCDeleteReason = Constant.DeleteReason.OTHER;
                            dt.DeleteReason = "Pembatalan untuk MCU Kelompok";
                            dt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            dt.LastUpdatedDate = DateTime.Now;
                            entityChargesDtDao.Update(dt);
                        }

                        if (hd.TestOrderID > 0)
                        {
                            List<TestOrderHd> lstTOHD = BusinessLayer.GetTestOrderHdList(string.Format("TestOrderID = {0} AND GCTransactionStatus != '{1}'", hd.TestOrderID, Constant.TransactionStatus.VOID), ctx);
                            if (lstTOHD.Count > 0)
                            {
                                TestOrderHd tohd = lstTOHD.Where(w => w.TestOrderID == hd.TestOrderID).FirstOrDefault();
                                tohd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                tohd.VoidBy = AppSession.UserLogin.UserID;
                                tohd.VoidDate = DateTime.Now;
                                tohd.GCVoidReason = Constant.DeleteReason.OTHER;
                                tohd.VoidReason = "Pembatalan untuk MCU Kelompok";
                                tohd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                tohd.LastUpdatedDate = DateTime.Now;
                                entityOrderHdDao.Update(tohd);

                                List<TestOrderDt> lstTODT = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND GCTestOrderStatus != '{1}' AND IsDeleted = 0", tohd.TestOrderID, Constant.TestOrderStatus.CANCELLED), ctx);
                                if (lstTODT.Count > 0)
                                {
                                    foreach (TestOrderDt dt in lstTODT)
                                    {
                                        dt.GCTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                        dt.IsDeleted = true;
                                        dt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        dt.LastUpdatedDate = DateTime.Now;
                                        entityOrderDtDao.Update(dt);
                                    }
                                }
                            }
                        }

                        if (hd.ServiceOrderID > 0)
                        {
                            List<ServiceOrderHd> lstSOHD = BusinessLayer.GetServiceOrderHdList(string.Format("ServiceOrderID = {0} AND GCTransactionStatus != '{1}'", hd.ServiceOrderID, Constant.TransactionStatus.VOID), ctx);
                            if (lstSOHD.Count > 0)
                            {
                                ServiceOrderHd sohd = lstSOHD.Where(w => w.ServiceOrderID == hd.ServiceOrderID).FirstOrDefault();
                                sohd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                sohd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                sohd.LastUpdatedDate = DateTime.Now;
                                entityServiceHdDao.Update(sohd);

                                List<ServiceOrderDt> lstSODT = BusinessLayer.GetServiceOrderDtList(string.Format("ServiceOrderID = {0} AND IsDeleted = 0 AND GCServiceOrderStatus != '{1}'", sohd.ServiceOrderID, Constant.TestOrderStatus.CANCELLED), ctx);
                                if (lstSODT.Count > 0)
                                {
                                    foreach (ServiceOrderDt dt in lstSODT)
                                    {
                                        dt.GCServiceOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                        dt.IsDeleted = true;
                                        dt.GCVoidReason = Constant.DeleteReason.OTHER;
                                        dt.VoidReason = "Pembatalan untuk MCU Kelompok";
                                        dt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        dt.LastUpdatedDate = DateTime.Now;
                                        entityServiceDtDao.Update(dt);
                                    }
                                }
                            }
                        }
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

        private bool ValidateMCUResultFormExist(ref string errMessage)
        {
            bool result = true;

            List<vMCUResultForm> lstResult = BusinessLayer.GetvMCUResultFormList(string.Format("VisitID IN ({0}) AND IsDeleted = 0", hdnSelectedMemberDtVisitID.Value));
            if (lstResult.Count > 0)
            {
                result = false;
                errMessage = string.Format("Tidak bisa melanjutkan proses karena sudah ada hasil pemeriksaan MCU (Form)");
            }

            List<MCUResult> lstMCUResult = BusinessLayer.GetMCUResultList(string.Format("VisitID IN ({0}) AND Result = 1", hdnSelectedMemberDtVisitID.Value));
            if (lstMCUResult.Count > 0)
            {
                result = false;
                errMessage = string.Format("Tidak bisa melanjutkan proses karena sudah ada hasil pemeriksaan MCU Luar");
            }

            List<PatientBill> lstBill = BusinessLayer.GetPatientBillList(string.Format("RegistrationID IN (SELECT RegistrationID FROM ConsultVisit WHERE VisitID IN ({0}))", hdnSelectedMemberDtVisitID.Value));
            if (lstBill.Count > 0)
            {
                result = false;
                errMessage = string.Format("Tidak bisa melanjutkan proses karena sudah ada tagihan");
            }

            return result;
        }
    }
}