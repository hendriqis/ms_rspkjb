using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class CopyItemRequestCtl : BaseViewPopupCtl
    {
        protected string filterExpressionLocationToCopy = "";

        public override void InitializeDataControl(string param)
        {
            hdnItemRequestID.Value = param;

            hdnIsUsedProductLine.Value = AppSession.IsUsedProductLine;

            filterExpressionLocationToCopy = string.Format("{0};0;{1};", AppSession.UserLogin.HealthcareID, Constant.TransactionCode.ITEM_DISTRIBUTION);

            vItemRequestHd2 irh = BusinessLayer.GetvItemRequestHd2List(string.Format("ItemRequestID = {0}", hdnItemRequestID.Value)).FirstOrDefault();
            EntityToControl(irh);

            txtItemRequestDateCopy.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtItemRequestTimeCopy.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            Helper.SetControlEntrySetting(txtItemRequestDateCopy, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemRequestTimeCopy, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnLocationIDToCopy, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtLocationCodeToCopy, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtLocationNameToCopy, new ControlEntrySetting(false, false, true), "mpTrxPopup");
        }

        protected void cbpCopyItemRequest_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string retval = "";
            string[] param = e.Parameter.Split('|');
            if (param[0] == "save")
            {
                if (OnSaveRecord(ref errMessage, ref retval))
                    result = "success|" + retval;
                else
                    result = "fail|" + errMessage;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void EntityToControl(vItemRequestHd2 prhd)
        {
            txtItemRequestNo.Text = prhd.ItemRequestNo;
            txtItemRequestDate.Text = prhd.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtItemRequestTime.Text = prhd.TransactionTime;
            hdnLocationIDFrom.Value = prhd.FromLocationID.ToString();
            txtLocationCode.Text = prhd.FromLocationCode;
            txtLocationName.Text = prhd.FromLocationName;
            hdnLocationIDTo.Value = prhd.ToLocationID.ToString();
            txtLocationCodeTo.Text = prhd.ToLocationCode;
            txtLocationNameTo.Text = prhd.ToLocationName;
            hdnProductLineID.Value = prhd.ProductLineID.ToString();

            hdnLocationIDFromCopy.Value = prhd.ToLocationID.ToString();
            txtLocationCodeFromCopy.Text = prhd.ToLocationCode;
            txtLocationNameFromCopy.Text = prhd.ToLocationName;
        }

        private void ControlToEntityHd(ItemRequestHd entityHd)
        {
            entityHd.ReferenceNo = Request.Form[txtItemRequestNo.UniqueID];
            entityHd.FromLocationID = Convert.ToInt32(hdnLocationIDFromCopy.Value);
            entityHd.ToLocationID = Convert.ToInt32(hdnLocationIDToCopy.Value);
            entityHd.TransactionDate = Helper.GetDatePickerValue(txtItemRequestDateCopy.Text);
            entityHd.TransactionTime = txtItemRequestTimeCopy.Text;
            entityHd.Remarks = string.Format("Salin permintaan barang dari lokasi {0} di nomor permintaan {1}.", Request.Form[txtLocationName.UniqueID], Request.Form[txtItemRequestNo.UniqueID]);

            if (hdnIsUsedProductLine.Value == "1")
            {
                entityHd.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            }

        }

        private bool OnSaveRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemRequestHdDao entityHdDao = new ItemRequestHdDao(ctx);
            ItemRequestDtDao entityDtDao = new ItemRequestDtDao(ctx);

            try
            {
                int NewItemRequestID = 0;

                string filterIRCheck = string.Format("ReferenceNo = '{0}' AND GCTransactionStatus = '{1}'", Request.Form[txtItemRequestNo.UniqueID], Constant.TransactionStatus.APPROVED);
                List<ItemRequestHd> lstItemRequestCheck = BusinessLayer.GetItemRequestHdList(filterIRCheck, ctx);
                if (lstItemRequestCheck.Count() == 0)
                {
                    ItemRequestHd entityHd = new ItemRequestHd();
                    ControlToEntityHd(entityHd);
                    entityHd.ItemRequestNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.ITEM_REQUEST, entityHd.TransactionDate, ctx);
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    NewItemRequestID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                    string filterIRFrom = string.Format("ItemRequestID = {0} AND IsDeleted = 0 AND GCItemDetailStatus = '{1}'", hdnItemRequestID.Value, Constant.TransactionStatus.APPROVED);
                    List<ItemRequestDt> lstItemRequestFrom = BusinessLayer.GetItemRequestDtList(filterIRFrom, ctx);
                    foreach (ItemRequestDt irdFrom in lstItemRequestFrom)
                    {
                        ItemRequestDt entityDt = new ItemRequestDt();

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        entityDt.ItemRequestID = NewItemRequestID;
                        entityDt.ItemID = irdFrom.ItemID;
                        entityDt.Quantity = irdFrom.Quantity;
                        entityDt.GCItemUnit = irdFrom.GCItemUnit;
                        entityDt.GCBaseUnit = irdFrom.GCItemUnit;
                        entityDt.ConversionFactor = irdFrom.ConversionFactor;
                        entityDt.GCItemRequestType = irdFrom.GCItemRequestType;
                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Insert(entityDt);
                    }

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    ItemRequestHd entityHdUpdate = entityHdDao.Get(NewItemRequestID);
                    entityHdUpdate.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    entityHdUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHdUpdate);

                    retval = entityHdUpdate.ItemRequestNo;

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Nomor permintaan {0} tidak dapat disalin karena sudah diproses.", Request.Form[txtItemRequestNo.UniqueID]);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
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
    }
}