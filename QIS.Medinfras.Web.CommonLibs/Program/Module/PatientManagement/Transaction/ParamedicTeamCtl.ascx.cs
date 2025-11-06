using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ParamedicTeamCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnID.Value = param;

            hdnVisitIDCtlPT.Value = AppSession.RegisteredPatient.VisitID.ToString();

            PatientChargesDt entity = BusinessLayer.GetPatientChargesDtList(string.Format("ID = {0}", hdnID.Value)).FirstOrDefault();
            ItemMaster item = BusinessLayer.GetItemMaster(entity.ItemID);
            txtHeaderCode.Text = item.ItemCode;
            txtHeaderName.Text = item.ItemName1;
            hdnMainParamedicID.Value = entity.ParamedicID.ToString();
            hdnClassID.Value = entity.ChargeClassID.ToString();
            hdnParentItemID.Value = entity.ItemID.ToString();
            hdnItemID.Value = entity.ItemID.ToString();
            txtItemCode.Text = item.ItemCode;

            PatientChargesHd entityHd = BusinessLayer.GetPatientChargesHd(entity.TransactionID);
            hdnChargesHealthcareServiceUnitID.Value = entityHd.HealthcareServiceUnitID.ToString();
            hdnTransactionDate.Value = entityHd.TransactionDateInDatePickerFormat;
            hdnTransactionTime.Value = entityHd.TransactionTime;

            ItemService itemService = BusinessLayer.GetItemService(entity.ItemID);
            if (!itemService.IsPackageItem)
            {
                trItemDetail.Style.Add("display", "none");
                //hdnItemID.Value = entity.ItemID.ToString();
                //txtItemCode.Text = item.ItemCode;
            }
            else
            {
                List<vItemServiceDt> lstDetail = BusinessLayer.GetvItemServiceDtList(string.Format("ItemID = {0} AND GCItemType NOT IN ('{1}','{2}','{3}')", entity.ItemID.ToString(), Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_MEDIS));
                if (lstDetail.Count == 0)
                {
                    trItemDetail.Style.Add("display", "none");
                    //hdnItemID.Value = entity.ItemID.ToString();
                    //txtItemCode.Text = item.ItemCode;
                }
            }

            Helper.SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboParamedicRole, new ControlEntrySetting(true, true, true), "mpEntryPopup");

            hdnIsPackageItem.Value = itemService.IsPackageItem ? "1" : "0";

            List<StandardCode> lstCusttype = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PARAMEDIC_ROLE));
            Methods.SetComboBoxField<StandardCode>(cboParamedicRole, lstCusttype, "StandardCodeName", "StandardCodeID");

            BindGridView();
        }

        protected string OnGetItemFilterExpression()
        {
            return string.Format("ItemID IN (SELECT DetailItemID FROM ItemServiceDt WHERE ItemID = {0} AND IsDeleted = 0) AND GCItemType NOT IN ('{1}','{2}')", hdnParentItemID.Value, Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.LOGISTIC);
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvPatientChargesDtParamedicList(string.Format("ID = {0}", hdnID.Value));
            grdView.DataBind();
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnIsAdd.Value.ToString() == "0")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;

        }

        private void ControlToEntity(PatientChargesDtParamedic entity)
        {
            entity.GCParamedicRole = cboParamedicRole.Value.ToString();
            if (hdnRevenueSharingID.Value != "" && hdnRevenueSharingID.Value != "0")
                entity.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            else
                entity.RevenueSharingID = null;

        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;

            try
            {
                IDbContext ctx = DbFactory.Configure(true);
                PatientChargesHdDao chargesHdDao = new PatientChargesHdDao(ctx);
                PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);
                PatientChargesDtParamedic entity = new PatientChargesDtParamedic();

                entity.ID = Convert.ToInt32(hdnID.Value);
                PatientChargesDt entityDt = chargesDtDao.Get(entity.ID);
                if (chargesHdDao.Get(entityDt.TransactionID).GCTransactionStatus != Constant.TransactionStatus.VOID)
                {
                    if (!chargesDtDao.Get(entity.ID).IsDeleted)
                    {
                        entity.ItemID = entityDt.ItemID;
                        entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                        ControlToEntity(entity);
                        BusinessLayer.InsertPatientChargesDtParamedic(entity);
                    }
                }
                else
                {
                    //string filterResult = string.Format("ChargeTransactionID = {0}", chargesDtDao.Get(entity.ID).TransactionID);
                    //ImagingResultHd resultHd = BusinessLayer.GetImagingResultHdList(filterResult).FirstOrDefault();
                    //if (resultHd != null)
                    //{
                    //    if (resultHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    //    {
                    //        entity.ItemID = Convert.ToInt32(hdnItemID.Value);
                    //        entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                    //        ControlToEntity(entity);
                    //        BusinessLayer.InsertPatientChargesDtParamedic(entity);
                    //    }
                    //    else
                    //    {
                    //        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                    //        result = false;
                    //    }
                    //}
                    //else
                    //{
                    //    errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                    //    result = false;
                    //}

                    errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;

            try
            {
                IDbContext ctx = DbFactory.Configure(true);
                PatientChargesHdDao chargesHdDao = new PatientChargesHdDao(ctx);
                PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);

                if (chargesHdDao.Get(chargesDtDao.Get(Convert.ToInt32(hdnID.Value)).TransactionID).GCTransactionStatus != Constant.TransactionStatus.VOID)
                {
                    if (!chargesDtDao.Get(Convert.ToInt32(hdnID.Value)).IsDeleted)
                    {
                        PatientChargesDtParamedic entity = BusinessLayer.GetPatientChargesDtParamedicList(string.Format("ID = {0} AND ParamedicID = {1}", hdnID.Value, hdnParamedicID.Value)).FirstOrDefault();
                        ControlToEntity(entity);
                        BusinessLayer.UpdatePatientChargesDtParamedic(entity);
                    }
                }
                else
                {
                    errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;

            try
            {
                IDbContext ctx = DbFactory.Configure(true);
                PatientChargesHdDao chargesHdDao = new PatientChargesHdDao(ctx);
                PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);

                if (chargesHdDao.Get(chargesDtDao.Get(Convert.ToInt32(hdnID.Value)).TransactionID).GCTransactionStatus != Constant.TransactionStatus.VOID)
                {
                    if (!chargesDtDao.Get(Convert.ToInt32(hdnID.Value)).IsDeleted)
                    {
                        BusinessLayer.DeletePatientChargesDtParamedic(Convert.ToInt32(hdnID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnItemID.Value));
                    }
                }
                else
                {
                    errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }
    }
}