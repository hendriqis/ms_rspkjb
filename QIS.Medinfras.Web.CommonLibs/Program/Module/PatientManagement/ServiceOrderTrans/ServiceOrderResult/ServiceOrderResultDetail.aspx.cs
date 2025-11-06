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
using QIS.Medinfras.Web.CommonLibs.Program;
using QIS.Medinfras.Web.CommonLibs.Controls;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ServiceOrderResultDetail : BasePageTrx
    {
        protected bool IsEditable = true;

        public override string OnGetMenuCode()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            switch (param[0])
            {
                case "eo": return Constant.MenuCode.EmergencyCare.SERVICE_ORDER_RESULT_TRANS;
                case "op": return Constant.MenuCode.Outpatient.SERVICE_ORDER_RESULT_TRANS;
                default: return Constant.MenuCode.EmergencyCare.SERVICE_ORDER_RESULT_TRANS;
            }
        }

        protected string GetMainParamedicRole()
        {
            return Constant.ParamedicRole.PELAKSANA;
        }

        public int GetVisitID()
        {
            return Convert.ToInt32(Request.Form[hdnVisitID.UniqueID]);
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override void InitializeDataControl()
        {
            string TransactionID;
            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                hdnType.Value = param[0];
                TransactionID = param[1];

                PatientChargesHd entityPCHD = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID = {0}", TransactionID)).FirstOrDefault();
                hdnTransactionHdID.Value = TransactionID;
                hdnVisitID.Value = Convert.ToString(entityPCHD.VisitID);
                hdnServiceOrderID.Value = entityPCHD.ServiceOrderID != null ? entityPCHD.ServiceOrderID.ToString() : "";
                txtTransactionNo.Text = entityPCHD.TransactionNo;
                
                if (hdnServiceOrderID.Value != "0" && hdnServiceOrderID.Value != "")
                {
                    vServiceOrderHd entityServiceOrderHd = BusinessLayer.GetvServiceOrderHdList(string.Format("ServiceOrderID = {0}", hdnServiceOrderID.Value)).FirstOrDefault();
                    txtOrderBy.Text = entityServiceOrderHd.ParamedicName;
                    txtOrderDate.Text = entityServiceOrderHd.ServiceOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtOrderTime.Text = entityServiceOrderHd.ServiceOrderTime;
                    txtOrderNo.Text = entityServiceOrderHd.ServiceOrderNo;
                }
                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();

                ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);
                hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnPhysicianID.Value = entity.ParamedicID.ToString();
                hdnPhysicianCode.Value = entity.ParamedicCode;
                hdnKdGudang.Value = entity.LocationID.ToString();
                hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
                hdnClassID.Value = entity.ClassID.ToString();

                hdnHealthcareServiceUnitID.Value = entityPCHD.HealthcareServiceUnitID.ToString();
                hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;
                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

                IsLoadFirstRecord = true;
                
                Helper.SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
                Helper.SetControlEntrySetting(txtParamedicName, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            }
        }

        #region Load Entity
        public override int OnGetRowCount()
        {
            return 1;
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            PatientChargesHd entity = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID = {0}", hdnTransactionHdID.Value)).FirstOrDefault();
            if (entity != null)
            {
                EntityToControl(entity, ref isShowWatermark, ref watermarkText);
                IsEditable = (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.VOID);
            }
            else
            {
                string filterExpression = string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionHdID.Value);
                if (hdnID.Value == "")
                    hdnID.Value = "0";
                BindGridView();
            }
        }

        private void EntityToControl(PatientChargesHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                hdnIsStatusOpen.Value = "1";
            }
            else
            {
                hdnWatermarkText.Value = BusinessLayer.GetStandardCode(entity.GCTransactionStatus).TagProperty;
                hdnIsStatusOpen.Value = "0";
            }
            hdnTransactionHdID.Value = entity.TransactionID.ToString();
            txtTransactionNo.Text = entity.TransactionNo;
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionHdID.Value);
            if (hdnID.Value == "")
                hdnID.Value = "0";
            List<vPatientChargesDt> lstEntity = BusinessLayer.GetvPatientChargesDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        #endregion
                
        #region Process
        protected bool OnSaveEditParamedic(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtDao entityDao = new PatientChargesDtDao(ctx);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            try
            {
                if (hdnChargesDtID.Value != "0" && hdnChargesDtID.Value != "")
                {
                    PatientChargesDt entity = entityDao.Get(Convert.ToInt32(hdnChargesDtID.Value));

                    PatientChargesHd entityHd = entityHdDao.Get(entity.TransactionID);

                    //string filterRevDt = string.Format("PatientChargesDtID = {0} AND PatientChargesID = {1} AND IsDeleted = 0", entity.ID, entity.TransactionID);
                    //List<TransRevenueSharingDt> lstTransRevDt = BusinessLayer.GetTransRevenueSharingDtList(filterRevDt);
                    //if (lstTransRevDt.Count == 0)
                    //{
                        ItemMaster im = BusinessLayer.GetItemMaster(entity.ItemID);
                        GetItemRevenueSharing rv = BusinessLayer.GetItemRevenueSharing(im.ItemCode, Convert.ToInt32(hdnParamedicID.Value), entity.ChargeClassID, GetMainParamedicRole(), Convert.ToInt32(hdnVisitID.Value), entityHd.HealthcareServiceUnitID, entityHd.TransactionDate, entityHd.TransactionTime).FirstOrDefault();

                        entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);

                        if (rv.RevenueSharingID != null && rv.RevenueSharingID != 0)
                        {
                            entity.RevenueSharingID = rv.RevenueSharingID;
                        }

                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);

                        ctx.CommitTransaction();
                    //}
                    //else
                    //{
                    //    result = false;
                    //    errMessage = "Dokter/Paramedis tidak bisa diubah karena sudah proses honor dokter.";
                    //    Exception ex = new Exception(errMessage);
                    //    Helper.InsertErrorLog(ex);
                    //    ctx.RollBackTransaction();
                    //}
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
        #endregion
        
        #region Decline Entity
        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            if (type == "saveparamedic")
            {
                result = OnSaveEditParamedic(ref errMessage);
                BindGridView();
            }
            else if (type=="decline")
            {
                IDbContext ctx = DbFactory.Configure(true);
                PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
                ImagingResultHdDao resultHdDao = new ImagingResultHdDao(ctx);
                ImagingResultDtDao resultDtDao = new ImagingResultDtDao(ctx);
                try
                {
                    PatientChargesHd entity = patientChargesHdDao.Get(Convert.ToInt32(hdnTransactionHdID.Value));
                    entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    patientChargesHdDao.Update(entity);
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
            }
            return result;
        }
        #endregion

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }
    }
}