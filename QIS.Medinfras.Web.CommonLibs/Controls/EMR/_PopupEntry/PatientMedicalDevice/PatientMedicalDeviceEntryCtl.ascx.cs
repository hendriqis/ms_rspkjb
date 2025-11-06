using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientMedicalDeviceEntryCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnUserID.Value = AppSession.UserLogin.UserID.ToString();

            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                IsAdd = paramInfo[0] == "0";
                hdnID.Value = paramInfo[0];
                hdnVisitID.Value = paramInfo[1];
                hdnMRN.Value = paramInfo[2];
                hdnTestOrderID.Value = paramInfo[3];
                OnControlEntrySettingLocal();
                ReInitControl();
                if (!IsAdd)
                {
                    vPatientMedicalDevice entity = BusinessLayer.GetvPatientMedicalDeviceList(string.Format("ID = {0}", Convert.ToInt32(hdnID.Value))).FirstOrDefault();
                    EntityToControl(entity);
                }
            }
            else
            {
                OnControlEntrySettingLocal();
                ReInitControl();
                hdnID.Value = "0";
                IsAdd = true;
            }
        }

        private void OnControlEntrySettingLocal()
        {
            SetControlEntrySetting(txtImplantDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtReviewDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlProperties();
        }

        private void EntityToControl(vPatientMedicalDevice entity)
        {
            txtImplantDate.Text = entity.ImplantDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtReviewDate.Text = entity.ReviewDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtItemCode.Text = entity.ItemCode;
            txtItemName.Text = entity.ItemName;
            txtSerialNo.Text = entity.SerialNumber;
            txtRemarks.Text = entity.Remarks;
            chkIsUsingMaster.Checked = Convert.ToInt32(entity.ItemID) != 0;
            hdnItemID.Value = entity.ItemID.ToString();
            if (entity.TransactionDtID > 0)
            {
                hdnTransactionDtID.Value = entity.TransactionDtID.ToString();
                txtTransactionNo.Text = entity.TransactionNo;
            }
            else
            {
                hdnTransactionDtID.Value = "0";
                txtTransactionNo.Text = string.Empty;
            }
        }

        private void ControlToEntity(PatientMedicalDevice entity)
        {
            entity.MRN = Convert.ToInt32(hdnMRN.Value);
            entity.ImplantDate = Helper.GetDatePickerValue(txtImplantDate);
            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entity.TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
            if (chkIsUsingMaster.Checked)
            {
                entity.ItemID = Convert.ToInt32(hdnItemID.Value);
                entity.ItemName = null;
                entity.IsInternalService = true;
            }
            else
            {
                entity.ItemID = null;
                entity.ItemName = txtItemName.Text;
                entity.IsInternalService = false;
            }
            entity.ReviewDate = Helper.GetDatePickerValue(txtReviewDate);
            entity.SerialNumber = txtSerialNo.Text;
            entity.Remarks = txtRemarks.Text;
            if (hdnTransactionDtID.Value != "0" && hdnTransactionDtID.Value != string.Empty)
            {
                entity.TransactionDtID = Convert.ToInt32(hdnTransactionDtID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PatientMedicalDeviceDao deviceDao = new PatientMedicalDeviceDao(ctx);
            PatientChargesDtInfoDao infoDao = new PatientChargesDtInfoDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);

            try
            {
                PatientMedicalDevice entity = new PatientMedicalDevice();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                int id = deviceDao.InsertReturnPrimaryKeyID(entity);

                #region update ChargesDtInfo
                if (hdnTransactionDtID.Value != "0" && hdnTransactionDtID.Value != string.Empty)
                {
                    string filterMedical = string.Format("TestOrderID = {0} AND TransactionDtID = {1} AND IsDeleted = 0 AND ID > {2}", entity.TestOrderID, entity.TransactionDtID, id);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<PatientMedicalDevice> lstCheck = BusinessLayer.GetPatientMedicalDeviceList(filterMedical, ctx);
                    if (lstCheck.Count <= 0 && !String.IsNullOrEmpty(entity.SerialNumber))
                    {
                        PatientChargesDtInfo info = infoDao.Get(Convert.ToInt32(entity.TransactionDtID));
                        info.SerialNo = entity.SerialNumber;
                        info.LastUpdatedBy = AppSession.UserLogin.UserID;
                        infoDao.Update(info);
                    }
                }
                #endregion

                Patient oPatient = patientDao.Get(entity.MRN);
                if (!oPatient.IsUsingImplant)
                {
                    #region Update Patient Status - Using Implant
                    oPatient.IsUsingImplant = true;
                    oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                    patientDao.Update(oPatient);
                    #endregion
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PatientMedicalDeviceDao deviceDao = new PatientMedicalDeviceDao(ctx);
            PatientChargesDtInfoDao infoDao = new PatientChargesDtInfoDao(ctx);
            try
            {
                PatientMedicalDevice entityUpdate = deviceDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                deviceDao.Update(entityUpdate);

                #region update ChargesDtInfo
                if (hdnTransactionDtID.Value != "0" && hdnTransactionDtID.Value != string.Empty)
                {
                    string filterMedical = string.Format("TestOrderID = {0} AND TransactionDtID = {1} AND IsDeleted = 0 AND ID > {2}", entityUpdate.TestOrderID, entityUpdate.TransactionDtID, entityUpdate.ID);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<PatientMedicalDevice> lstCheck = BusinessLayer.GetPatientMedicalDeviceList(filterMedical, ctx);
                    if (lstCheck.Count <= 0 && !String.IsNullOrEmpty(entityUpdate.SerialNumber))
                    {
                        PatientChargesDtInfo info = infoDao.Get(Convert.ToInt32(entityUpdate.TransactionDtID));
                        info.SerialNo = entityUpdate.SerialNumber;
                        info.LastUpdatedBy = AppSession.UserLogin.UserID;
                        infoDao.Update(info);
                    }
                }
                #endregion

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
    }
}