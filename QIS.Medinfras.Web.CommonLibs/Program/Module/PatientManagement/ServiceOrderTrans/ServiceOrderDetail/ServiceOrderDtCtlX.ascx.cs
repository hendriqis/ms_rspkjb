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
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ServiceOrderDtCtlX : BaseViewPopupCtl
    {
        public int PageCount = 1;
        public int CurrPage = 1;
        public override void InitializeDataControl(string param)
        {
            tblVoidReason.Visible = true;

            hdnServiceOrderID.Value = param;

            vServiceOrderHdVisit entity = BusinessLayer.GetvServiceOrderHdVisitList(string.Format("ServiceOrderID = {0}", hdnServiceOrderID.Value)).FirstOrDefault();
            hdnLinkedChargesID.Value = entity.LinkedChargesID.ToString();
            hdnVisitID.Value = entity.VisitID.ToString();
            hdnClassID.Value = BusinessLayer.GetConsultVisit(entity.VisitID).ChargeClassID.ToString();
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnDepartmentID.Value = entity.OrderDepartmentID;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            
            txtNotes.Text = entity.Remarks;

            PatientChargesHd patientChargesHd = BusinessLayer.GetPatientChargesHdList(GetServiceOrderTransactionFilterExpression()).FirstOrDefault();
            if (patientChargesHd != null)
            {
                txtServiceOrderDate.Text = patientChargesHd.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceOrderTime.Text = patientChargesHd.TransactionTime;
                txtTransactionNo.Text = patientChargesHd.TransactionNo;
                hdnTransactionID.Value = patientChargesHd.TransactionID.ToString();
            }
            else
            {
                txtServiceOrderDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceOrderTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }

            List<ParamedicMaster> lstParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND IsDeleted = 0", hdnHealthcareServiceUnitID.Value));
            if (lstParamedic.Count == 1)
            {
                ParamedicMaster paramedic = lstParamedic.FirstOrDefault();
                hdnPhysicianID.Value = paramedic.ParamedicID.ToString();
                txtPhysicianCode.Text = paramedic.ParamedicCode;
                txtPhysicianName.Text = paramedic.FullName;
            }

            //Helper.SetControlEntrySetting(txtServiceOrderDate, new ControlEntrySetting(true, true, true), "mpEntry");
            //Helper.SetControlEntrySetting(txtServiceOrderTime, new ControlEntrySetting(true, true, true), "mpEntry");
            //Helper.SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, true), "mpEntry");

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DELETE_REASON));
            Methods.SetComboBoxField<StandardCode>(cboVoidReason, lstSc, "StandardCodeName", "StandardCodeID");
            cboVoidReason.SelectedIndex = 0;

            BindGridView(CurrPage, true, ref PageCount);
        }

        protected string GetServiceOrderTransactionFilterExpression()
        {
            return string.Format("HealthcareServiceUnitID = {0} AND ServiceorderID = {1} AND GCTransactionStatus = '{2}'", hdnHealthcareServiceUnitID.Value, hdnServiceOrderID.Value, Constant.TransactionStatus.OPEN);
        }

        private string GetFilterExpression()
        {
            return string.Format("ServiceOrderID = {0} AND IsDeleted = 0", hdnServiceOrderID.Value);
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                vServiceOrderDt entity = e.Row.DataItem as vServiceOrderDt;
                if (entity.GCServiceOrderStatus != Constant.TestOrderStatus.OPEN)
                {
                    CheckBox chkIsSelected = (CheckBox)e.Row.FindControl("chkIsSelected");
                    chkIsSelected.Visible = false;
                }
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvServiceOrderDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vServiceOrderDt> lstEntity = BusinessLayer.GetvServiceOrderDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");

            if (lstEntity.Count > 0)
            {
                //if (lstEntity.Where(p => p.GCServiceOrderStatus == Constant.TestOrderStatus.OPEN).Count() < 1)
                //{
                //    trTransactionDateTime.Style.Add("display", "none");
                //    trTransactionNo.Style.Add("display", "none");
                //    trTransactionParamedic.Style.Add("display", "none");
                //    tblApproveDecline.Style.Add("display", "none");
                //    tblVoidReason.Style.Add("display", "none");
                //}
                grdView.DataSource = lstEntity;
                grdView.DataBind();
            }
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
                else
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }


        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string retval = "";
            string result = param + "|";
            string errMessage = "";

            if (param == "approve")
            {
                if (OnApproveRecord(ref errMessage, ref retval))
                    result += "success";
                else
                    result += "fail|" + errMessage;
            }
            else if (param == "decline")
            {
                if (OnDeclineRecord(ref errMessage))
                    result += "success";
                else
                    result += "fail|" + errMessage;
            }
            else if (param == "close")
            {
                if (OnCloseRecord(ref errMessage))
                    result += "success";
                else
                    result += "fail|" + errMessage;
            }

            BindGridView(CurrPage, true, ref PageCount);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
        }

        private bool OnApproveRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);
            ServiceOrderDtDao ServiceOrderDtDao = new ServiceOrderDtDao(ctx);
            ServiceOrderHdDao ServiceOrderHdDao = new ServiceOrderHdDao(ctx);
            try
            {
                String filterExpressionHd = String.Format("ServiceOrderID = {0} AND ItemID IN ({1})", hdnServiceOrderID.Value, hdnParam.Value);

                List<ServiceOrderDt> lstServiceOrderDt = BusinessLayer.GetServiceOrderDtList(filterExpressionHd, ctx);
                List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();
                int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                int visitID = Convert.ToInt32(hdnVisitID.Value);
                foreach (ServiceOrderDt ServiceDt in lstServiceOrderDt)
                {
                    ServiceDt.GCServiceOrderStatus = Constant.TestOrderStatus.COMPLETED;
                    ServiceDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ServiceOrderDtDao.Update(ServiceDt);

                    PatientChargesDt patientChargesDt = new PatientChargesDt();
                    patientChargesDt.ItemID = ServiceDt.ItemID;
                    patientChargesDt.ChargeClassID = Convert.ToInt32(hdnClassID.Value);
                    patientChargesDt.ItemPackageID = ServiceDt.ItemPackageID;
                    patientChargesDt.ReferenceDtID = ServiceDt.ID;

                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, visitID, patientChargesDt.ChargeClassID, ServiceDt.ItemID, 1, DateTime.Now, ctx);
                    decimal discountAmount = 0;
                    decimal coverageAmount = 0;
                    decimal price = 0;
                    decimal basePrice = 0;
                    bool isCoverageInPercentage = false;
                    bool isDiscountInPercentage = false;
                    if (list.Count > 0)
                    {
                        GetCurrentItemTariff obj = list[0];
                        discountAmount = obj.DiscountAmount;
                        coverageAmount = obj.CoverageAmount;
                        price = obj.Price;
                        basePrice = obj.BasePrice;
                        isCoverageInPercentage = obj.IsCoverageInPercentage;
                        isDiscountInPercentage = obj.IsDiscountInPercentage;
                    }

                    patientChargesDt.BaseTariff = basePrice;
                    patientChargesDt.Tariff = price;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();

                    ItemMaster entityItemMaster = itemDao.Get(ServiceDt.ItemID);
                    patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                    patientChargesDt.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);

                    patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visitID, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Helper.GetDatePickerValue(txtServiceOrderDate.Text), txtServiceOrderTime.Text).FirstOrDefault().RevenueSharingID;
                    if (patientChargesDt.RevenueSharingID == 0)
                        patientChargesDt.RevenueSharingID = null;
                    patientChargesDt.IsVariable = false;
                    patientChargesDt.IsUnbilledItem = false;

                    decimal totalDiscountAmount = 0;
                    decimal grossLineAmount = 1 * price;
                    if (isDiscountInPercentage)
                        totalDiscountAmount = grossLineAmount * discountAmount / 100;
                    else
                        totalDiscountAmount = discountAmount * 1;
                    if (totalDiscountAmount > grossLineAmount)
                        totalDiscountAmount = grossLineAmount;

                    decimal total = grossLineAmount - totalDiscountAmount;
                    decimal totalPayer = 0;
                    if (isCoverageInPercentage)
                        totalPayer = total * coverageAmount / 100;
                    else
                        totalPayer = coverageAmount * 1;
                    if (totalPayer > total)
                        totalPayer = total;

                    patientChargesDt.IsCITO = false;
                    patientChargesDt.CITOAmount = 0;
                    patientChargesDt.IsComplication = false;
                    patientChargesDt.ComplicationAmount = 0;
                    patientChargesDt.IsDiscount = false;
                    patientChargesDt.DiscountAmount = totalDiscountAmount;
                    patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = 1;
                    patientChargesDt.PatientAmount = total - totalPayer;
                    patientChargesDt.PayerAmount = totalPayer;
                    patientChargesDt.LineAmount = total;
                    patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;

                    patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    lstPatientChargesDt.Add(patientChargesDt);
                }

                #region Patient Charges
                PatientChargesHd patientChargesHd = null;
                if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0" || patientChargesHdDao.Get(Convert.ToInt32(hdnTransactionID.Value)).GCTransactionStatus != Constant.TransactionStatus.OPEN)
                {
                    patientChargesHd = new PatientChargesHd();
                    patientChargesHd.VisitID = visitID;
                    //if (hdnLinkedChargesID.Value != "0" && hdnLinkedChargesID.Value != "")
                    //    patientChargesHd.LinkedChargesID = Convert.ToInt32(hdnLinkedChargesID.Value);
                    //else
                    //    patientChargesHd.LinkedChargesID = null;
                    patientChargesHd.LinkedChargesID = null;
                    patientChargesHd.ServiceOrderID = Convert.ToInt32(hdnServiceOrderID.Value);
                    patientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                    switch (hdnDepartmentID.Value)
                    {
                        case Constant.Facility.EMERGENCY: patientChargesHd.TransactionCode = Constant.TransactionCode.ER_CHARGES; break;
                        default: patientChargesHd.TransactionCode = Constant.TransactionCode.OP_CHARGES; break;
                    }
                    patientChargesHd.TransactionDate = Helper.GetDatePickerValue(txtServiceOrderDate);
                    patientChargesHd.TransactionTime = txtServiceOrderTime.Text;
                    patientChargesHd.PatientBillingID = null;
                    patientChargesHd.ReferenceNo = "";
                    patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    patientChargesHd.GCVoidReason = null;
                    patientChargesHd.TotalPatientAmount = 0;
                    patientChargesHd.TotalPayerAmount = 0;
                    patientChargesHd.TotalAmount = 0;
                    patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                    patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
                }
                else
                {
                    patientChargesHd = patientChargesHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                }
                retval = patientChargesHd.TransactionNo;
                foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                {
                    patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                    patientChargesDtDao.Insert(patientChargesDt);
                }
                #endregion

                int testOrderDtCount = BusinessLayer.GetServiceOrderDtRowCount(string.Format("ServiceOrderID = {0} AND GCServiceOrderStatus = '{1}' AND IsDeleted = 0", hdnServiceOrderID.Value, Constant.TestOrderStatus.OPEN), ctx);
                if (testOrderDtCount < 1)
                {
                    ServiceOrderHd testOrderHd = ServiceOrderHdDao.Get(Convert.ToInt32(hdnServiceOrderID.Value));
                    testOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                    testOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ServiceOrderHdDao.Update(testOrderHd);
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnDeclineRecord(ref string errMessage)
        {
            bool result = true;

            if (String.IsNullOrEmpty(cboVoidReason.Value.ToString()))
            {
                result = false;
                errMessage = "Alasan Pembatalan harus diisi";
                return result;
            }

            IDbContext ctx = DbFactory.Configure(true);
            ServiceOrderDtDao ServiceOrderDtDao = new ServiceOrderDtDao(ctx);
            ServiceOrderHdDao ServiceOrderHdDao = new ServiceOrderHdDao(ctx);
            try
            {
                String filterExpressionHd = String.Format("ServiceOrderID = {0} AND ItemID IN ({1})", hdnServiceOrderID.Value, hdnParam.Value);

                List<ServiceOrderDt> lstServiceOrderDt = BusinessLayer.GetServiceOrderDtList(filterExpressionHd);
                foreach (ServiceOrderDt ServiceDt in lstServiceOrderDt)
                {
                    ServiceDt.GCServiceOrderStatus = Constant.TestOrderStatus.CANCELLED;
                    ServiceDt.GCVoidReason = cboVoidReason.Value.ToString();
                    if (ServiceDt.GCVoidReason == Constant.DeleteReason.OTHER)
                        ServiceDt.VoidReason = txtVoidReason.Text;
                    ServiceDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ServiceOrderDtDao.Update(ServiceDt);
                }
                int testOrderDtCount = BusinessLayer.GetServiceOrderDtRowCount(string.Format("ServiceOrderID = {0} AND GCServiceOrderStatus = '{1}' AND IsDeleted = 0", hdnServiceOrderID.Value, Constant.TransactionStatus.OPEN), ctx);
                if (testOrderDtCount < 1)
                {
                    ServiceOrderHd testOrderHd = ServiceOrderHdDao.Get(Convert.ToInt32(hdnServiceOrderID.Value));
                    testOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                    testOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ServiceOrderHdDao.Update(testOrderHd);
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnCloseRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ServiceOrderHdDao ServiceOrderHdDao = new ServiceOrderHdDao(ctx);
            try
            {
                ServiceOrderHd testOrderHd = ServiceOrderHdDao.Get(Convert.ToInt32(hdnServiceOrderID.Value));
                testOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                testOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                ServiceOrderHdDao.Update(testOrderHd);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
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