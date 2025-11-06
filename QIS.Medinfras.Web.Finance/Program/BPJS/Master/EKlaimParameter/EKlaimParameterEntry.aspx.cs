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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class EKlaimParameterEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.BPJS_MASTER_EKLAIM_PARAMETER;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;

                EKlaimParameter entity = BusinessLayer.GetEKlaimParameterList(string.Format("EKlaimParameterID = {0}", ID)).FirstOrDefault();

                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }

            txtParameterCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtParameterCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtParameterName, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(EKlaimParameter entity)
        {
            txtParameterCode.Text = entity.EKlaimParameterCode;
            txtParameterName.Text = entity.EKlaimParameterName;
        }

        private void ControlToEntity(EKlaimParameter entity)
        {
            entity.EKlaimParameterCode = txtParameterCode.Text;
            entity.EKlaimParameterName = txtParameterName.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("EKlaimParameterCode = '{0}'", txtParameterCode.Text);
            List<EKlaimParameter> lst = BusinessLayer.GetEKlaimParameterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " E-Klaim Parameter With Code " + txtParameterCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("EKlaimParameterCode = '{0}' AND EKlaimParameterID != {1}", txtParameterCode.Text, hdnID.Value);
            List<EKlaimParameter> lst = BusinessLayer.GetEKlaimParameterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " E-Klaim Parameter With Code " + txtParameterCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            EKlaimParameterDao entityDao = new EKlaimParameterDao(ctx);

            try
            {
                EKlaimParameter entity = new EKlaimParameter();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();

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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            EKlaimParameterDao entityDao = new EKlaimParameterDao(ctx);

            try
            {
                int BankID = Convert.ToInt32(hdnID.Value);
                EKlaimParameter entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
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
    }
}