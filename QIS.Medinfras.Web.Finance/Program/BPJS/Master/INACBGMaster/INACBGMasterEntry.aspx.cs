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
    public partial class INACBGMasterEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.BPJS_INACBGs_MASTER;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;

                vINACBGMaster entity = BusinessLayer.GetvINACBGMasterList(string.Format("ID = {0}", ID)).FirstOrDefault();

                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }

            txtGrouperCode.Focus();

        }

        protected override void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtGrouperCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtGrouperDescription, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtHealthcareCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtHealthcareName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtEKlaimTariffCategory1, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtEKlaimTariffCategory2, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtClassCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtClassName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtINACBGClass, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtINACBGTariff, new ControlEntrySetting(true, true, true, "0"));
        }

        private void EntityToControl(vINACBGMaster entity)
        {
            txtGrouperCode.Text = entity.GrouperCode;
            txtGrouperDescription.Text = entity.GrouperDescription;
            hdnHealthcareID.Value = entity.HealthcareID;
            txtHealthcareCode.Text = entity.HealthcareID;
            txtHealthcareName.Text = entity.HealthcareName;
            txtEKlaimTariffCategory1.Text = entity.EKlaimTariffCategory1;
            txtEKlaimTariffCategory2.Text = entity.EKlaimTariffCategory2;
            hdnClassID.Value = entity.ClassID.ToString();
            txtClassCode.Text = entity.ClassCode;
            txtClassName.Text = entity.ClassName;
            txtINACBGClass.Text = entity.INACBGClass;
            txtINACBGTariff.Text = entity.GrouperTariff.ToString();
        }

        private void ControlToEntity(INACBGMaster entity)
        {
            entity.GrouperCode = txtGrouperCode.Text;
            entity.GrouperDescription = txtGrouperDescription.Text;
            entity.HealthcareID = hdnHealthcareID.Value;
            entity.ClassID = Convert.ToInt32(hdnClassID.Value);
            entity.GrouperTariff = Convert.ToDecimal(txtINACBGTariff.Text);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("GrouperCode = '{0}' AND ClassID = {1} AND IsDeleted = 0", txtGrouperCode.Text, hdnClassID.Value);
            List<INACBGMaster> lst = BusinessLayer.GetINACBGMasterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " INACBGs Master with Code " + txtGrouperCode.Text + " and Class " + txtClassName.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("GrouperCode = '{0}' AND ClassID = {1} AND ID != {2} AND IsDeleted = 0", txtGrouperCode.Text, hdnClassID.Value, hdnID.Value);
            List<INACBGMaster> lst = BusinessLayer.GetINACBGMasterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " INACBGs Master with Code " + txtGrouperCode.Text + " and Class " + txtClassName.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            INACBGMasterDao entityDao = new INACBGMasterDao(ctx);

            try
            {
                INACBGMaster entity = new INACBGMaster();
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
            INACBGMasterDao entityDao = new INACBGMasterDao(ctx);

            try
            {
                int BankID = Convert.ToInt32(hdnID.Value);
                INACBGMaster entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
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