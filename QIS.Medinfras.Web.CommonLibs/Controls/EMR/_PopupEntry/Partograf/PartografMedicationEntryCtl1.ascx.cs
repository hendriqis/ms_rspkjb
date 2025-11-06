using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;
using Newtonsoft.Json;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PartografMedicationEntryCtl1 : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            hdnPopupID.Value = paramInfo[0];
            hdnPopupVisitID.Value = paramInfo[1];
            hdnPopupTestOrderID.Value = paramInfo[2];
            hdnPopupAntenatalRecordID.Value = paramInfo[3];

            SetControlProperties();

            if (!string.IsNullOrEmpty(hdnPopupID.Value) && hdnPopupID.Value != "0")
            {
                IsAdd = false;
                vIntraMedicationLog entity = BusinessLayer.GetvIntraMedicationLogList(string.Format("ID = {0}", hdnPopupID.Value)).FirstOrDefault();
                if (entity != null)
                {
                    SetEntityToControl(entity); 
                } 
            }
            else
            {
                IsAdd = true;
                if (paramInfo.Length >= 5)
                {
                    //copy mode
                    if (!string.IsNullOrEmpty(paramInfo[4]))
                    {
                        int copyRecordID = Convert.ToInt32(paramInfo[4]);
                        vIntraMedicationLog entity = BusinessLayer.GetvIntraMedicationLogList(string.Format("ID = {0}", copyRecordID)).FirstOrDefault();
                        SetEntityToControl(entity);
                        txtLogDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtLogTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    }
                }
            }
        }

        private void SetEntityToControl(vIntraMedicationLog entity)
        {
            txtLogDate.Text = entity.LogDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtLogTime.Text = entity.LogTime;
            hdnDrugID.Value = entity.ItemID.ToString();
            ledDrugName.Value = entity.ItemName;
            txtItemUnit.Text = entity.ItemUnit;
            txtDosingDose.Text = entity.NumberOfDosage.ToString("G29");
            cboDosingUnit.Value = entity.GCDosingUnit;
            cboMedicationRoute.Value = entity.GCRoute;
            txtMedicationAdministration.Text = entity.Remarks;
        }

        protected override void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");

            cboMedicationRoute.SelectedIndex = 0;

            txtLogDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtLogTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        }


        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            IntraMedicationLogDao entityDao = new IntraMedicationLogDao(ctx);
            try
            {
                if (IsValid(ref errMessage))
                {
                    IntraMedicationLog entity = new IntraMedicationLog();
                    ControlToEntity(entity);
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entity.CreatedDate = DateTime.Now;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entity.ID = entityDao.InsertReturnPrimaryKeyID(entity);
                    ctx.CommitTransaction();
                    retVal = entity.ID.ToString();
                }
                else
                {
                    result = false;
                    ctx.RollBackTransaction();
                    retVal = "0";                  
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                retVal = "0";
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
            IntraMedicationLogDao entityDao = new IntraMedicationLogDao(ctx);
            try
            {
                if (IsValid(ref errMessage))
                {
                    IntraMedicationLog entity = entityDao.Get(Convert.ToInt32(hdnPopupID.Value));
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                    entityDao.Update(entity);
                    ctx.CommitTransaction();
                    retVal = entity.ID.ToString();
                }
                else
                {
                    result = false;
                    ctx.RollBackTransaction();
                    retVal = "0";
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                retVal = "0";
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private void ControlToEntity(IntraMedicationLog oMedication)
        {
            oMedication.LogDate = Helper.GetDatePickerValue(txtLogDate);
            oMedication.LogTime = txtLogTime.Text;
            oMedication.VisitID = Convert.ToInt32(hdnPopupVisitID.Value); ;
            if (!string.IsNullOrEmpty(hdnPopupTestOrderID.Value) && hdnPopupTestOrderID.Value != "0")
            {
                oMedication.TestOrderID = Convert.ToInt32(hdnPopupTestOrderID.Value);                
            }
            if (!string.IsNullOrEmpty(hdnPopupAntenatalRecordID.Value) && hdnPopupAntenatalRecordID.Value != "0")
            {
                oMedication.AntenatalRecordID = Convert.ToInt32(hdnPopupAntenatalRecordID.Value);
            } 
            oMedication.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            oMedication.ItemID = Convert.ToInt32(hdnDrugID.Value);
            oMedication.IsNotMasterItem = false;
            oMedication.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
            oMedication.GCDosingUnit = cboDosingUnit.Value.ToString();
            oMedication.GCRoute = cboMedicationRoute.Value.ToString();
            oMedication.Remarks = txtMedicationAdministration.Text;

        }

        private bool IsValid(ref string errMessage)
        {
            StringBuilder message = new StringBuilder();

            string itemName = ledDrugName.Text;

            if (string.IsNullOrEmpty(itemName))
                message.AppendLine("Drug name should be filled");            

            if (string.IsNullOrEmpty(cboDosingUnit.Value.ToString()))
                message.AppendLine("Dosing unit should be filled");

            if (string.IsNullOrEmpty(cboDosingUnit.Value.ToString()))
                message.AppendLine("Dosing unit should be filled");


            decimal numberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
            if (numberOfDosage <= 0)
                message.AppendLine("Jumlah pemberian harus lebih besar dari 0");


            errMessage = message.ToString();
            return string.IsNullOrEmpty(errMessage);
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }
    }
}