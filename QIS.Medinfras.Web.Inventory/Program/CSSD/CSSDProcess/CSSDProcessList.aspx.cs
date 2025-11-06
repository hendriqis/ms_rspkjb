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
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class CSSDProcessList : BasePageTrx
    {
        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CSSD_PROCESS;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            BindGridViewWashing();
            BindGridViewPackaging();
            BindGridViewSterilitation();
            BindGridViewQualityControl();
        }

        #region WASHING

        private void BindGridViewWashing()
        {
            string filterExpression = string.Format("GCWashingMethod IS NULL AND GCServiceStatus = '{0}'", Constant.ServiceStatus.PROCESSED);

            List<vMDServiceRequestHd> lstWashing = BusinessLayer.GetvMDServiceRequestHdList(filterExpression);
            grdViewWashing.DataSource = lstWashing;
            grdViewWashing.DataBind();
        }

        protected void grdViewWashing_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ASPxComboBox cboWashingMethod = (ASPxComboBox)e.Row.FindControl("cboWashingMethod");

                cboWashingMethod.ClientInstanceName = string.Format("cboWashingMethod{0}", e.Row.DataItemIndex);

                List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                "ParentID IN ('{0}') AND IsDeleted = 0 AND IsActive = 1",
                                                Constant.StandardCode.WASHING_METHOD));
                Methods.SetComboBoxField<StandardCode>(cboWashingMethod, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.WASHING_METHOD).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
                cboWashingMethod.SelectedIndex = 0;
            }
        }

        protected void cbpViewWashing_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');

                if (param[0] == "process")
                {
                    if (OnProcessWashing(ref errMessage))
                    {
                        result += string.Format("{0}|success", param[0]);
                    }
                    else
                    {
                        result += string.Format("{0}|fail|{1}", param[0], errMessage);
                    }
                }
                else
                {
                    BindGridViewWashing();
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnProcessWashing(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            MDServiceRequestHdDao entityDao = new MDServiceRequestHdDao(ctx);

            try
            {
                string[] lstRequestID = hdnSelectedWashingProcess.Value.Substring(1).Split(',');
                string[] lstWashing = hdnSelectedWashingProcessMethod.Value.Substring(1).Split(',');

                for (int i = 0; i < lstRequestID.Count(); i++)
                {
                    MDServiceRequestHd entity = entityDao.Get(Convert.ToInt32(lstRequestID[i]));
                    entity.GCServiceStatus = Constant.ServiceStatus.WASHING;
                    entity.GCWashingMethod = lstWashing[i];
                    entity.WashingBy = AppSession.UserLogin.UserID;
                    entity.WashingDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                    entityDao.Update(entity);
                }
                
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

        #endregion

        #region PACKAGING

        private void BindGridViewPackaging()
        {
            string filterExpression = string.Format("GCPackagingType IS NULL AND GCServiceStatus = '{0}'", Constant.ServiceStatus.WASHING);

            List<vMDServiceRequestHd> lstPackaging = BusinessLayer.GetvMDServiceRequestHdList(filterExpression);
            grdViewPackaging.DataSource = lstPackaging;
            grdViewPackaging.DataBind();
        }

        protected void grdViewPackaging_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ASPxComboBox cboPackagingType = (ASPxComboBox)e.Row.FindControl("cboPackagingType");

                cboPackagingType.ClientInstanceName = string.Format("cboPackagingType{0}", e.Row.DataItemIndex);

                List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                "ParentID IN ('{0}') AND IsDeleted = 0 AND IsActive = 1",
                                                Constant.StandardCode.PACKAGING_TYPE));
                Methods.SetComboBoxField<StandardCode>(cboPackagingType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.PACKAGING_TYPE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
                cboPackagingType.SelectedIndex = 0;
            }
        }

        protected void cbpViewPackaging_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');

                if (param[0] == "process")
                {
                    if (OnProcessPackaging(ref errMessage))
                    {
                        result += string.Format("{0}|success", param[0]);
                    }
                    else
                    {
                        result += string.Format("{0}|fail|{1}", param[0], errMessage);
                    }
                }
                else
                {
                    BindGridViewPackaging();
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnProcessPackaging(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            MDServiceRequestHdDao entityDao = new MDServiceRequestHdDao(ctx);

            try
            {
                string[] lstRequestID = hdnSelectedPackagingProcess.Value.Substring(1).Split(',');
                string[] lstPackaging = hdnSelectedPackagingProcessType.Value.Substring(1).Split(',');

                for (int i = 0; i < lstRequestID.Count(); i++)
                {
                    MDServiceRequestHd entity = entityDao.Get(Convert.ToInt32(lstRequestID[i]));
                    entity.GCServiceStatus = Constant.ServiceStatus.PACKAGING;
                    entity.GCPackagingType = lstPackaging[i];
                    entity.PackagingBy = AppSession.UserLogin.UserID;
                    entity.PackagingDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                    entityDao.Update(entity);
                }

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

        #endregion

        #region STERILITATION

        private void BindGridViewSterilitation()
        {
            string filterExpression = string.Format("GCSterilitationType IS NULL AND GCServiceStatus = '{0}'", Constant.ServiceStatus.PACKAGING);

            List<vMDServiceRequestHd> lstSterilitation = BusinessLayer.GetvMDServiceRequestHdList(filterExpression);
            grdViewSterilitation.DataSource = lstSterilitation;
            grdViewSterilitation.DataBind();
        }

        protected void grdViewSterilitation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ASPxComboBox cboSterilitationType = (ASPxComboBox)e.Row.FindControl("cboSterilitationType");

                cboSterilitationType.ClientInstanceName = string.Format("cboSterilitationType{0}", e.Row.DataItemIndex);

                List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                "ParentID IN ('{0}') AND IsDeleted = 0 AND IsActive = 1",
                                                Constant.StandardCode.STERILITATION_TYPE));
                Methods.SetComboBoxField<StandardCode>(cboSterilitationType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.STERILITATION_TYPE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
                cboSterilitationType.SelectedIndex = 0;

                HtmlInputText txtExpiredDate = e.Row.FindControl("txtExpiredDate") as HtmlInputText;
                txtExpiredDate.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
        }

        protected void cbpViewSterilitation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');

                if (param[0] == "process")
                {
                    if (OnProcessSterilitation(ref errMessage))
                    {
                        result += string.Format("{0}|success", param[0]);
                    }
                    else
                    {
                        result += string.Format("{0}|fail|{1}", param[0], errMessage);
                    }
                }
                else
                {
                    BindGridViewSterilitation();
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnProcessSterilitation(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            MDServiceRequestHdDao entityDao = new MDServiceRequestHdDao(ctx);

            try
            {
                string[] lstRequestID = hdnSelectedSterilitationProcess.Value.Substring(1).Split(',');
                string[] lstSterilitation = hdnSelectedSterilitationProcessType.Value.Substring(1).Split(',');
                string[] lstSterilitationCycle = hdnSelectedSterilitationProcessCycle.Value.Substring(1).Split(',');
                string[] lstSterilitationExpired = hdnSelectedSterilitationProcessExpired.Value.Substring(1).Split(',');

                for (int i = 0; i < lstRequestID.Count(); i++)
                {
                    MDServiceRequestHd entity = entityDao.Get(Convert.ToInt32(lstRequestID[i]));
                    entity.GCServiceStatus = Constant.ServiceStatus.STERILITATION;
                    entity.GCSterilitationType = lstSterilitation[i];
                    entity.SterilitationCycle = Convert.ToInt32(lstSterilitationCycle[i]);
                    entity.SterilitationExpiredDate = Helper.GetDatePickerValue(lstSterilitationExpired[i]);
                    entity.SterilitationBy = AppSession.UserLogin.UserID;
                    entity.SterilitationDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                    entityDao.Update(entity);
                }

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

        #endregion
        
        #region QUALITY CONTROL

        private void BindGridViewQualityControl()
        {
            string filterExpression = string.Format("ControlledBy IS NULL AND GCServiceStatus = '{0}'", Constant.ServiceStatus.STERILITATION);

            List<vMDServiceRequestHd> lstQualityControl = BusinessLayer.GetvMDServiceRequestHdList(filterExpression);
            grdViewQualityControl.DataSource = lstQualityControl;
            grdViewQualityControl.DataBind();
        }

        protected void cbpViewQualityControl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');

                if (param[0] == "process")
                {
                    if (OnProcessQualityControl(ref errMessage))
                    {
                        result += string.Format("{0}|success", param[0]);
                    }
                    else
                    {
                        result += string.Format("{0}|fail|{1}", param[0], errMessage);
                    }
                }
                else if (param[0] == "declineqc")
                {
                    if (OnProcessDeclineQualityControl(ref errMessage))
                    {
                        result += string.Format("{0}|success", param[0]);
                    }
                    else
                    {
                        result += string.Format("{0}|fail|{1}", param[0], errMessage);
                    }
                }
                else
                {
                    BindGridViewQualityControl();
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnProcessQualityControl(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            MDServiceRequestHdDao entityDao = new MDServiceRequestHdDao(ctx);

            try
            {
                string filter = "";
                if (!String.IsNullOrEmpty(hdnSelectedQualityControlProcess.Value) && hdnSelectedQualityControlProcess.Value != ",")
                {
                    filter = String.Format("RequestID IN ({0}) AND ControlledBy IS NULL", hdnSelectedQualityControlProcess.Value.Substring(1));
                }

                List<MDServiceRequestHd> lst = BusinessLayer.GetMDServiceRequestHdList(filter, ctx);

                foreach (MDServiceRequestHd entity in lst)
                {
                    entity.GCServiceStatus = Constant.ServiceStatus.QUALITY_CONTROL;
                    entity.ControlledBy = AppSession.UserLogin.UserID;
                    entity.ControlledDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                    entityDao.Update(entity);
                }

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

        private bool OnProcessDeclineQualityControl(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            MDServiceRequestHdDao entityDao = new MDServiceRequestHdDao(ctx);

            try
            {
                string filter = "";
                if (!String.IsNullOrEmpty(hdnSelectedQualityControlProcess.Value) && hdnSelectedQualityControlProcess.Value != ",")
                {
                    filter = String.Format("RequestID IN ({0})", hdnSelectedQualityControlProcess.Value.Substring(1));
                }

                List<MDServiceRequestHd> lst = BusinessLayer.GetMDServiceRequestHdList(filter, ctx);

                foreach (MDServiceRequestHd entity in lst)
                {
                    entity.GCServiceStatus = Constant.ServiceStatus.WASHING;
                    entity.GCPackagingType = null;
                    entity.PackagingBy = null;
                    entity.PackagingDate = Helper.GetDatePickerValue("01-01-1900");
                    entity.GCSterilitationType = null;
                    entity.SterilitationCycle = 0;
                    entity.SterilitationExpiredDate = Helper.GetDatePickerValue("01-01-1900");
                    entity.SterilitationBy = null;
                    entity.SterilitationDate = Helper.GetDatePickerValue("01-01-1900");
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                    entityDao.Update(entity);
                }

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

        #endregion
    }
}