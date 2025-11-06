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

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class NutrientEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nutrition.NUTRIENT;
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
                Nutrient entity = BusinessLayer.GetNutrient(Convert.ToInt32(ID));
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtNutrientCode.Focus();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND ISActive = 1 AND IsDeleted = 0", Constant.StandardCode.NUTRIENT_UNIT));
            Methods.SetComboBoxField<StandardCode>(cboNutrientUnit, lstStandardCode, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtNutrientCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNutrientName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNutrientValue, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(cboNutrientUnit, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(Nutrient entity)
        {
            txtNutrientCode.Text = entity.NutrientCode;
            txtNutrientName.Text = entity.NutrientName;
            txtNutrientValue.Text = entity.PercentDailyValue.ToString();
            cboNutrientUnit.Text = entity.GCNutrientUnit;
        }

        private void ControlToEntity(Nutrient entity)
        {
            entity.NutrientCode = txtNutrientCode.Text;
            entity.NutrientName = txtNutrientName.Text;
            entity.PercentDailyValue = Convert.ToDecimal(txtNutrientValue.Text);
            entity.GCNutrientUnit = cboNutrientUnit.Value.ToString();
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("NutrientCode = '{0}' AND IsDeleted = 0", txtNutrientCode.Text);
            List<Nutrient> lst = BusinessLayer.GetNutrientList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Nutrient With Code " + txtNutrientCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("NutrientCode = '{0}' AND NutrientID != {1} AND IsDeleted = 0", txtNutrientCode.Text, hdnID.Value);
            List<Nutrient> lst = BusinessLayer.GetNutrientList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Nutrient With Code " + txtNutrientCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            NutrientDao entityDao = new NutrientDao(ctx);
            bool result = false;
            try
            {
                Nutrient entity = new Nutrient();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();
                ctx.CommitTransaction();
                result = true;
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
            IDbContext ctx = DbFactory.Configure(true);
            NutrientDao entityDao = new NutrientDao(ctx);
            bool result = false;
            try
            {
                Nutrient entity = new Nutrient();
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
                ctx.CommitTransaction();
                result = true;
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