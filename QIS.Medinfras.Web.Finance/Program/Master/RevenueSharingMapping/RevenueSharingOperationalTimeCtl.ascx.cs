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
    public partial class RevenueSharingOperationalTimeCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnRevenueSharingIDCtl.Value = param;

            RevenueSharingHd revSharingHd = BusinessLayer.GetRevenueSharingHd(Convert.ToInt32(hdnRevenueSharingIDCtl.Value));
            txtRevenueSharingCodeCtl.Text = revSharingHd.RevenueSharingCode;
            txtRevenueSharingNameCtl.Text = revSharingHd.RevenueSharingName;

            BindGridView();

            List<Variable> lstVar = new List<Variable>(){ 
                new Variable{ Code = "Senin", Value = "1"},
                new Variable{ Code = "Selasa", Value = "2"},
                new Variable{ Code = "Rabu", Value = "3"},
                new Variable{ Code = "Kamis", Value = "4"},
                new Variable{ Code = "Jumat", Value = "5"},
                new Variable{ Code = "Sabtu", Value = "6"},
                new Variable{ Code = "Minggu", Value = "7"}};
            Methods.SetComboBoxField<Variable>(cboDay, lstVar, "Code", "Value");
            cboDay.SelectedIndex = 0;
            hdnDayNumberCtl.Value = cboDay.Value.ToString();

            string filter = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.OPERATIONAL_TYPE);
            List<StandardCode> lstOperationalType = BusinessLayer.GetStandardCodeList(filter);
            Methods.SetComboBoxField<StandardCode>(cboOperationalType, lstOperationalType, "StandardCodeName", "StandardCodeID");
            cboOperationalType.SelectedIndex = 0;
            hdnOperationalTypeCtl.Value = cboOperationalType.Value.ToString();
        }

        private void BindGridView()
        {
            String filterExpression = string.Format("RevenueSharingID = {0} AND IsDeleted = 0", hdnRevenueSharingIDCtl.Value);
            List<vRevenueSharingOperationalTime> lst = BusinessLayer.GetvRevenueSharingOperationalTimeList(filterExpression);
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

        private void ControlToEntity(RevenueSharingOperationalTime entity)
        {
            entity.DayNumber = Convert.ToInt16(cboDay.Value);
            entity.StartTime = txtStartTime.Text;
            entity.EndTime = txtEndTime.Text;
            entity.GCOperationalType = cboOperationalType.Value.ToString();
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RevenueSharingOperationalTimeDao entityDao = new RevenueSharingOperationalTimeDao(ctx);
            try
            {
                RevenueSharingOperationalTime entity = new RevenueSharingOperationalTime();
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
            RevenueSharingOperationalTimeDao entityDao = new RevenueSharingOperationalTimeDao(ctx);
            try
            {
                RevenueSharingOperationalTime entity = entityDao.Get(Convert.ToInt32(hdnIDCtl.Value));
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
            RevenueSharingOperationalTimeDao entityDao = new RevenueSharingOperationalTimeDao(ctx);
            try
            {
                RevenueSharingOperationalTime entity = entityDao.Get(Convert.ToInt32(hdnIDCtl.Value));
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