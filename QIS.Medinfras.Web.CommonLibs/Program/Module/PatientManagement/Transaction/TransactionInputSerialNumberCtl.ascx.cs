using System;
using System.Data;
using System.Collections.Generic;
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
    public partial class TransactionInputSerialNumberCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", param)).FirstOrDefault();
                hdnVisitIDCtlImplant.Value = entity.VisitID.ToString();
                hdnTestOrderIDCtlImplant.Value = entity.TestOrderID.ToString();
                hdnMRNCtlImplant.Value = entity.MRN.ToString();
                hdnTransactionIDCtl1.Value = entity.TransactionID.ToString();
                txtTransactionNoCtl1.Text = entity.TransactionNo;
                txtTransactionDateTimeCtl1.Text = entity.cfTransactionDate + " " + entity.TransactionTime;
                txtServiceUnitNameCtl1.Text = entity.ServiceUnitName;

                txtImplantDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtReviewDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                Helper.SetControlEntrySetting(txtImplantDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)), "mpImplant");
                Helper.SetControlEntrySetting(txtReviewDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)), "mpImplant");

                BindGridView();
            }
        }

        private void BindGridView()
        {
            string filter = string.Format("TransactionID = {0}", hdnTransactionIDCtl1.Value);
            grdChargesSerialNo.DataSource = BusinessLayer.GetvPatientChargesDtCustom1List(filter);
            grdChargesSerialNo.DataBind();
        }

        protected void cbpChargesDtInfoSerialNo_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (hdnChargesDtIDCtl1.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(PatientMedicalDevice entity, Int32 itemID, Int32 ID)
        {
            entity.MRN = Convert.ToInt32(hdnMRNCtlImplant.Value);
            entity.ImplantDate = Helper.GetDatePickerValue(txtImplantDate);
            entity.VisitID = Convert.ToInt32(hdnVisitIDCtlImplant.Value);
            if (!String.IsNullOrEmpty(hdnTestOrderIDCtlImplant.Value) && hdnTestOrderIDCtlImplant.Value != "0")
            {
                entity.TestOrderID = Convert.ToInt32(hdnTestOrderIDCtlImplant.Value);
            }
            entity.ItemID = itemID;
            entity.ItemName = null;
            entity.IsInternalService = true;
            entity.ReviewDate = Helper.GetDatePickerValue(txtReviewDate);
            entity.SerialNumber = txtSerialNoCtl1.Text;
            entity.Remarks = txtRemarks.Text;
            entity.TransactionDtID = ID;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PatientMedicalDeviceDao deviceDao = new PatientMedicalDeviceDao(ctx);
            PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtInfoDao infoDao = new PatientChargesDtInfoDao(ctx);

            try
            {
                PatientChargesDt entityDt = chargesDtDao.Get(Convert.ToInt32(hdnChargesDtIDCtl1.Value));
                PatientChargesDtInfo entity = infoDao.Get(Convert.ToInt32(hdnChargesDtIDCtl1.Value));
                entity.SerialNo = txtSerialNoCtl1.Text;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                infoDao.Update(entity);

                string filterMedicalDevice = string.Format("TransactionDtID = {0} AND ItemID = {1} AND  IsDeleted = 0 ORDER BY ID DESC", entity.ID, entityDt.ItemID);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                PatientMedicalDevice entityPatientMedicalDevice = BusinessLayer.GetPatientMedicalDeviceList(filterMedicalDevice, ctx).FirstOrDefault();
                if (entityPatientMedicalDevice != null)
                {
                    ControlToEntity(entityPatientMedicalDevice, entityDt.ItemID, entity.ID);
                    entityPatientMedicalDevice.LastUpdatedBy = AppSession.UserLogin.UserID;
                    deviceDao.Update(entityPatientMedicalDevice);
                }
                else
                {
                    PatientMedicalDevice entityMedicalDevice = new PatientMedicalDevice();
                    ControlToEntity(entityMedicalDevice, entityDt.ItemID, entity.ID);
                    entityMedicalDevice.CreatedBy = AppSession.UserLogin.UserID;
                    deviceDao.InsertReturnPrimaryKeyID(entityMedicalDevice);
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

    }
}