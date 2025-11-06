using System;
using System.Collections.Generic;
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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TestPartnerItemEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramList = param.Split('|');
            hdnRequestIDItemCtl.Value = paramList[0];
            hdnBusinessPartnerIDItemCtl.Value = paramList[1];

            BusinessPartners bp = BusinessLayer.GetBusinessPartners(Convert.ToInt32(hdnBusinessPartnerIDItemCtl.Value));
            txtPartnerCodeName.Text = string.Format("{0} - {1}", bp.BusinessPartnerCode, bp.BusinessPartnerName);

            TestPartner tp = BusinessLayer.GetTestPartner(bp.BusinessPartnerID);
            hdnGCTestPartnerTypeCtl.Value = tp.GCTestPartnerType;

            txtStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnDateTodayInPickerFormat.Value = txtStartDate.Text;

            BindGridView();
        }

        #region Test Partner Item

        private void BindGridView()
        {
            string filterExpression = string.Format("BusinessPartnerID = {0} AND IsDeleted = 0", hdnBusinessPartnerIDItemCtl.Value);

            List<vTestPartnerItem> lstEntity = BusinessLayer.GetvTestPartnerItemList(filterExpression, int.MaxValue, 1, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "refresh")
            {
                BindGridView();
                result = string.Format("refresh|");
            }
            else if (param[0] == "save")
            {
                if (hdnTestPartnerItemID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }

                BindGridView();
                result += "|";
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
                
                BindGridView();
                result += "|";
            }
            else if (param[0] == "saveTariff")
            {
                if (hdnTestPartnerItemTariffID.Value.ToString() != "")
                {
                    if (OnSaveEditTariffRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddTariffRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "deleteTariff")
            {
                if (OnDeleteTariffRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(TestPartnerItem entity)
        {
            entity.ItemID = Convert.ToInt32(hdnItemID.Value);
            entity.PartnerItemCode = txtPartnerItemCode.Text;
            entity.PartnerItemName = txtPartnerItemName.Text;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestPartnerItemDao entityPartnerItemDao = new TestPartnerItemDao(ctx);

            try
            {
                TestPartnerItem entity = new TestPartnerItem();
                ControlToEntity(entity);
                entity.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerIDItemCtl.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityPartnerItemDao.Insert(entity);

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
            TestPartnerItemDao entityPartnerItemDao = new TestPartnerItemDao(ctx);

            try
            {
                TestPartnerItem entity = entityPartnerItemDao.Get(Convert.ToInt32(hdnTestPartnerItemID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityPartnerItemDao.Update(entity);

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
            TestPartnerItemDao entityPartnerItemDao = new TestPartnerItemDao(ctx);
            TestPartnerItemTariffDao entityPartnerItemTariffDao = new TestPartnerItemTariffDao(ctx);

            try
            {
                TestPartnerItem entity = entityPartnerItemDao.Get(Convert.ToInt32(hdnTestPartnerItemID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityPartnerItemDao.Update(entity);

                string filterItemTariff = string.Format("TestPartnerItemID = {0} AND IsDeleted = 0", entity.ID);
                List<TestPartnerItemTariff> lstItemTariff = BusinessLayer.GetTestPartnerItemTariffList(filterItemTariff, ctx);
                foreach (TestPartnerItemTariff itemTariff in lstItemTariff)
                {
                    itemTariff.IsDeleted = true;
                    itemTariff.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityPartnerItemTariffDao.Update(itemTariff);
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

        #region Test Partner Item Tariff

        private void BindGridViewTariff()
        {
            int otpItemID = 0;

            if (hdnTestPartnerItemID.Value != null && hdnTestPartnerItemID.Value != "" && hdnTestPartnerItemID.Value != "0")
            {
                otpItemID = Convert.ToInt32(hdnTestPartnerItemID.Value);
            }

            string filterDtTariff = string.Format("TestPartnerItemID = {0} AND IsDeleted = 0", otpItemID.ToString());
            List<vTestPartnerItemTariff> lstTariff = BusinessLayer.GetvTestPartnerItemTariffList(filterDtTariff, int.MaxValue, 1, "StartDate, Amount");
            grdDetailTariff.DataSource = lstTariff;
            grdDetailTariff.DataBind();
        }

        protected void cbpViewDetailTariff_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";

            BindGridViewTariff();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntityTariff(TestPartnerItemTariff entity)
        {
            entity.StartDate = Helper.GetDatePickerValue(txtStartDate.Text);
            entity.Amount = Convert.ToDecimal(txtAmount.Text);
        }

        private bool OnSaveAddTariffRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestPartnerItemTariffDao entityPartnerItemTariffDao = new TestPartnerItemTariffDao(ctx);

            try
            {
                TestPartnerItemTariff entity = new TestPartnerItemTariff();
                ControlToEntityTariff(entity);
                entity.TestPartnerItemID = Convert.ToInt32(hdnTestPartnerItemID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityPartnerItemTariffDao.Insert(entity);

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

        private bool OnSaveEditTariffRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestPartnerItemTariffDao entityPartnerItemTariffDao = new TestPartnerItemTariffDao(ctx);

            try
            {
                TestPartnerItemTariff entity = entityPartnerItemTariffDao.Get(Convert.ToInt32(hdnTestPartnerItemTariffID.Value));
                ControlToEntityTariff(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityPartnerItemTariffDao.Update(entity);

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

        private bool OnDeleteTariffRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestPartnerItemTariffDao entityPartnerItemTariffDao = new TestPartnerItemTariffDao(ctx);

            try
            {
                TestPartnerItemTariff entity = entityPartnerItemTariffDao.Get(Convert.ToInt32(hdnTestPartnerItemTariffID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityPartnerItemTariffDao.Update(entity);

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