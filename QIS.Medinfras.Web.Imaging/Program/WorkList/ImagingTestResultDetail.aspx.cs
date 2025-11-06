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
using QIS.Medinfras.Web.CommonLibs.Program;

namespace QIS.Medinfras.Web.Imaging.Program
{
    public partial class ImagingTestResultDetail : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public int trigger = 0;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Imaging.IMAGING_TEST_ITEM;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            //IsAllowAdd = (hdnGCRegistrationStatus.Value != Constant.RegistrationStatus.CLOSED);
            IsAllowAdd = true;
            IsAllowSave = true;
            IsAllowNextPrev = false;
            IsAllowVoid = false;
           
        }

        public string GetFilterExpression()
        {
            string filterExpression = string.Format("TransactionID='{0}'",hdnTransactionHdID.Value);
           
            return filterExpression;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
           
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();
            List<vPatientChargesDt> lstEntity = BusinessLayer.GetvPatientChargesDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, PageCount);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        public override void OnAddRecord()
        {
        }

        protected override void InitializeDataControl()
        {
            string TransactionID;
            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                TransactionID = param[0];
                
                vPatientChargesHd entityPCHD = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", TransactionID))[0];
                hdnTransactionHdID.Value = TransactionID;
                hdnVisitID.Value = Convert.ToString(entityPCHD.VisitID);
                vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value))[0];


                ctlPatientBanner.InitializePatientBanner(entity);
                hdnTransactionHdID.Value = TransactionID;
                if (param.Count() < 2)
                {
                    hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
                    hdnDepartmentID.Value = entity.DepartmentID;
                    IsLoadFirstRecord = true;
                    
                }
                else
                {
                    SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID);
                    
                    hdnTransactionHdID.Value = "0";

                }
                //txtServiceCode.Attributes.Add("validationgroup", "mpTrxService");
            }
            BindGridView();

            List<ImagingResultHd> entitytemp = BusinessLayer.GetImagingResultHdList(string.Format("ChargeTransactionID='{0}'",hdnTransactionHdID.Value));
            IsLoadFirstRecord = entitytemp.Count > 0;
            if (entitytemp.Count > 0)
            {
                IsLoadFirstRecord = true;
                EntityToControl(entitytemp[0]);
                AppSession.HeaderID = entitytemp[0].ID;
            }
            else
            {
                IsLoadFirstRecord = false;
                txtReferenceNo.Text = "";
                txtResultDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtResultTime.Text = DateTime.Today.ToString(Constant.FormatString.TIME_FORMAT);
                txtProvider.Text = "";
                txtParamedic.Text = "";
            }

        }


        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtResultDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtResultTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtProvider, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtParamedic, new ControlEntrySetting(true, false, true));
        }


        public override int OnGetRowCount()
        {
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1}", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value);
            return BusinessLayer.GetvPatientChargesHdRowCount(filterExpression);
        }

        #region Load Entity

        private void EntityToControl(vPatientChargesHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            /*    
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }

             * */
        }
        #endregion

        #region Save Entity
        public void SaveImagingResultHd(IDbContext ctx, ref int ImagingHdID)
        {
            ImagingResultHdDao entityHdDao = new ImagingResultHdDao(ctx);
            if (hdnID.Value == "0")
            {
                ImagingResultHd entityHd = new ImagingResultHd();
                ControlToEntity(entityHd);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityHd.CreatedBy = AppSession.UserLogin.UserID;

                entityHdDao.Insert(entityHd);

                ImagingHdID = BusinessLayer.GetImagingResultHdMaxID(ctx);
            }
            else
            {
                ImagingHdID = Convert.ToInt32(hdnTransactionHdID.Value);
            }
        }


        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {

            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int ImagingHdID = 0;
                SaveImagingResultHd(ctx, ref ImagingHdID);
                PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionHdID.Value));
                entity.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                BusinessLayer.UpdatePatientChargesHd(entity);
                retval = ImagingHdID.ToString();
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

        private void ControlToEntity(ImagingResultHd entity)
        {
            entity.ReferenceNo = txtReferenceNo.Text;
            entity.ResultDate = Helper.GetDatePickerValue(txtResultDate.Text);
            entity.ResultTime = txtResultTime.Text;
            entity.ProviderName = txtProvider.Text;
            entity.ParamedicName = txtParamedic.Text;
            entity.IsInternal = true;
            entity.IsDeleted = false;
            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entity.ChargeTransactionID = Convert.ToInt32(hdnTransactionHdID.Value);
        }

        private void EntityToControl(ImagingResultHd entity)
        {
            hdnID.Value = entity.ID.ToString();
            txtReferenceNo.Text = entity.ReferenceNo;
            txtResultDate.Text = entity.ResultDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtResultTime.Text = entity.ResultTime;
            txtProvider.Text = entity.ProviderName;
            txtParamedic.Text = entity.ParamedicName;
        }
        
        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            /*
            try
            {
                //ImagingResultHd entity = BusinessLayer.GetImagingResultHd(Convert.ToInt32(hdnVisitID.Value));
                ImagingResultHd entityIRHD = new ImagingResultHd();
                ControlToEntity(entityIRHD);

                BusinessLayer.InsertImagingResultHd(entityIRHD);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
            */

            return true;
            
        }
        
        #endregion

        #region Region Process
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int ImagingHdID = 0;
                SaveImagingResultHd(ctx, ref ImagingHdID);

                ctx.CommitTransaction();
               
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
        } 
        #endregion
    }
}