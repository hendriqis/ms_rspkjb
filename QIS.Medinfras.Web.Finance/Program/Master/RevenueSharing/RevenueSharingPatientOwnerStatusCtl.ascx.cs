using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RevenueSharingPatientOwnerStatusCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnRevenueSharingIDCtl.Value = param;

            RevenueSharingHd revSharingHd = BusinessLayer.GetRevenueSharingHd(Convert.ToInt32(hdnRevenueSharingIDCtl.Value));
            txtRevenueSharingCodeCtl.Text = revSharingHd.RevenueSharingCode;
            txtRevenueSharingNameCtl.Text = revSharingHd.RevenueSharingName;

            BindGridView();

            string filter = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.PATIENT_OWNER_STATUS);
            List<StandardCode> lstOperationalType = BusinessLayer.GetStandardCodeList(filter);
            Methods.SetComboBoxField<StandardCode>(cboPatientOwnerStatus, lstOperationalType, "StandardCodeName", "StandardCodeID");
            cboPatientOwnerStatus.SelectedIndex = 0;
            hdnPatientOwnerStatusCtl.Value = cboPatientOwnerStatus.Value.ToString();
        }

        private void BindGridView()
        {
            String filterExpression = string.Format("RevenueSharingID = {0} AND IsDeleted = 0", hdnRevenueSharingIDCtl.Value);
            List<vRevenueSharingPerPatientOwnerStatus> lst = BusinessLayer.GetvRevenueSharingPerPatientOwnerStatusList(filterExpression);
            grdView.DataSource = lst;
            grdView.DataBind();
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            int pageCount = 1;

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "refresh")
            {
                BindGridView();
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                {
                    result += "success";
                    BindGridView();
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "save")
            {
                if (hdnIDCtl.Value != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView();
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView();
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(RevenueSharingPerPatientOwnerStatus entity)
        {
            entity.GCPatientOwnerStatus = cboPatientOwnerStatus.Value.ToString();
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RevenueSharingPerPatientOwnerStatusDao entityDao = new RevenueSharingPerPatientOwnerStatusDao(ctx);
            try
            {
                RevenueSharingPerPatientOwnerStatus entity = new RevenueSharingPerPatientOwnerStatus();
                ControlToEntity(entity);
                entity.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingIDCtl.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;

                entityDao.Insert(entity);

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

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RevenueSharingPerPatientOwnerStatusDao entityDao = new RevenueSharingPerPatientOwnerStatusDao(ctx);
            try
            {
                RevenueSharingPerPatientOwnerStatus entity = entityDao.Get(Convert.ToInt32(hdnIDCtl.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityDao.Update(entity);

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

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RevenueSharingPerPatientOwnerStatusDao entityDao = new RevenueSharingPerPatientOwnerStatusDao(ctx);
            try
            {
                RevenueSharingPerPatientOwnerStatus entity = entityDao.Get(Convert.ToInt32(hdnIDCtl.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityDao.Update(entity);

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
    }
}