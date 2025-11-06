using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class EditBloodBankDetailOrder : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            string[] paramInfo = param.Split('|');
            hdnVisitIDBloodBankCtl.Value = paramInfo[0];
            hdnTestOrderIDBloodBankCtl.Value = paramInfo[1];

            string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1}", hdnVisitIDBloodBankCtl.Value, hdnTestOrderIDBloodBankCtl.Value);
            vBloodBankOrder1 entityBB = BusinessLayer.GetvBloodBankOrder1List(filterExpression).FirstOrDefault();
            EntityToControl(entityBB);

        }

        private void EntityToControl(vBloodBankOrder1 entity)
        {
            rblGCSourceType.SelectedValue = entity.GCSourceType;
            rblGCUsageType.SelectedValue = entity.GCUsageType;
            rblGCPaymentType.SelectedValue = entity.GCPaymentType;

            if (entity.GCSourceType == Constant.BloodBankSourceType.PMI)
            {
                trPaymentTypeInfo.Attributes.Remove("style");
            }
            else
            {
                trPaymentTypeInfo.Attributes.Add("style", "display:none");
            }
        }

        private void ControlToEntity(TestOrderHd entity)
        {
            entity.GCSourceType = rblGCSourceType.SelectedValue;
            entity.GCUsageType = rblGCUsageType.SelectedValue;
            if (entity.GCSourceType == Constant.BloodBankSourceType.PMI)
            {
                entity.GCPaymentType = rblGCPaymentType.SelectedValue;
            }
            else
            {
                entity.GCPaymentType = null;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            return false;
        }

        protected override bool OnSaveEditRecord(ref string url, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityDao = new TestOrderHdDao(ctx);

            try
            {
                TestOrderHd entity = entityDao.Get(Convert.ToInt32(hdnTestOrderIDBloodBankCtl.Value));
                if (entity != null)
                {
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Data order bank darah tidak ditemukan.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
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
    }
}