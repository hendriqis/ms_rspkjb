using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ServiceOrderEntry : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            //string[] param = Page.Request.QueryString["id"].Split('|');
            //switch (hdnDepartmentID.Value)
            //{
            //    case Constant.Facility.INPATIENT:
            //        if (param[1] == "er")
            //            return Constant.MenuCode.Inpatient.EMERGENCY_ORDER;
            //        else
            //            return Constant.MenuCode.Inpatient.OUTPATIENT_ORDER;
            //    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.OUTPATIENT_ORDER;
            //    default: return Constant.MenuCode.Outpatient.EMERGENCY_ORDER;
            //}
            return "";
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.VisitStatus.CLOSED);
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                hdnPageTitle.Value = ((MPMain)((MPTrx2)Master).Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode()).MenuCaption;

                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param[1] == "er")
                {
                    trServiceUnit.Style.Add("display", "none");
                    vHealthcareServiceUnit HSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND IsDeleted = 0", Constant.Facility.EMERGENCY, AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                    hdnHealthcareServiceUnitID.Value = HSU.HealthcareServiceUnitID.ToString();
                }
                hdnCode.Value = param[1];
                hdnVisitID.Value = param[0];
                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];

                ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);
                hdnDepartmentID.Value = entity.DepartmentID;
                hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnClassID.Value = entity.ClassID.ToString();

                hdnDefaultParamedicID.Value = hdnPhysicianID.Value = entity.ParamedicID.ToString();
                hdnDefaultParamedicCode.Value = txtPhysicianCode.Text = entity.ParamedicCode;
                hdnDefaultParamedicName.Value = txtPhysicianName.Text = entity.ParamedicName;
            }   
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnServiceOrderID, new ControlEntrySetting(false, false, false, "0"));

            SetControlEntrySetting(txtServiceOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtServiceOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtServiceOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, false, false, hdnDefaultParamedicID.Value));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, hdnDefaultParamedicCode.Value));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, true, hdnDefaultParamedicName.Value));

            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false));
        }

        public override void OnAddRecord()
        {
        }
        
        public override int OnGetRowCount()
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            return BusinessLayer.GetvServiceOrderHdRowCount(filterExpression);
        }
        
        #region Load Entity
        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {   
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            vServiceOrderHd entity = BusinessLayer.GetvServiceOrderHd(filterExpression, PageIndex, " ServiceOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
             
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            PageIndex = BusinessLayer.GetvServiceOrderHdRowIndex(filterExpression, keyValue, "ServiceOrderID DESC");
            vServiceOrderHd entity = BusinessLayer.GetvServiceOrderHd(filterExpression, PageIndex, "ServiceOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vServiceOrderHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }
            hdnServiceOrderID.Value = entity.ServiceOrderID.ToString();
            txtServiceOrderNo.Text = entity.ServiceOrderNo;
            txtServiceOrderDate.Text = entity.ServiceOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtServiceOrderTime.Text = entity.ServiceOrderTime;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtNotes.Text = entity.Remarks;
        }
        #endregion

        #region Save Entity
        public void SaveServiceOrderHd(IDbContext ctx, ref int serviceOrderID)
        {
            ServiceOrderHdDao entityHdDao = new ServiceOrderHdDao(ctx);
            if (hdnServiceOrderID.Value == "0")
            {
                ServiceOrderHd entityHd = new ServiceOrderHd();
                entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityHd.ServiceOrderDate = Helper.GetDatePickerValue(txtServiceOrderDate.Text);
                entityHd.ServiceOrderTime = txtServiceOrderTime.Text;
                entityHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                entityHd.Remarks = txtNotes.Text;
                entityHd.ReferenceNo = txtReferenceNo.Text;
                entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                {
                    if (hdnCode.Value == "op")
                        entityHd.TransactionCode = Constant.TransactionCode.IP_OUTPATIENT_ORDER;
                    else entityHd.TransactionCode = Constant.TransactionCode.IP_EMERGENCY_ORDER;
                }
                else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                    entityHd.TransactionCode = Constant.TransactionCode.OP_EMERGENCY_ORDER;
                else
                    entityHd.TransactionCode = Constant.TransactionCode.ER_OUTPATIENT_ORDER;
                entityHd.ServiceOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.ServiceOrderDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                entityHdDao.Insert(entityHd);

                serviceOrderID = BusinessLayer.GetServiceUnitMasterMaxID(ctx);
            }
            else
            {
                serviceOrderID = Convert.ToInt32(hdnServiceOrderID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int serviceOrderID = 0;
                SaveServiceOrderHd(ctx, ref serviceOrderID);

                retval = serviceOrderID.ToString();
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                ServiceOrderHd entityHd = BusinessLayer.GetServiceOrderHd(Convert.ToInt32(hdnServiceOrderID.Value));
                entityHd.Remarks = txtNotes.Text;
                entityHd.ReferenceNo = txtReferenceNo.Text;
                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateServiceOrderHd(entityHd);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion

        #region Void Entity
        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ServiceOrderHdDao entityHdDao = new ServiceOrderHdDao(ctx);
            try
            {
                Int32 ServiceOrderID = Convert.ToInt32(hdnServiceOrderID.Value);
                ServiceOrderHd entity = entityHdDao.Get(ServiceOrderID);
                entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Update(entity);

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
        #endregion

        #region Proposed Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            try
            {
                Int32 ServiceOrderID = Convert.ToInt32(hdnServiceOrderID.Value);
                ServiceOrderHd entity = BusinessLayer.GetServiceOrderHd(ServiceOrderID);
                entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateServiceOrderHd(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion
    }
}