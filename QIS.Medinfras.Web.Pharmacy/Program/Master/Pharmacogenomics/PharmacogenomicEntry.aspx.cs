using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Text;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PharmacogenomicEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.PHARMACOGENOMICS;
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
                String id = Request.QueryString["id"];
                hdnID.Value = id;
                Pharmacogenetic entity = BusinessLayer.GetPharmacogenetic(Convert.ToInt32(id));
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtPharmacogeneticCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtPharmacogeneticCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPharmacogeneticName, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(Pharmacogenetic entity)
        {
            txtPharmacogeneticCode.Text = entity.PharmacogeneticCode;
            txtPharmacogeneticName.Text = entity.PharmacogeneticName;
        }

        private void ControlToEntity(Pharmacogenetic entity)
        {
            entity.PharmacogeneticCode = txtPharmacogeneticCode.Text;
            entity.PharmacogeneticName = txtPharmacogeneticName.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("PharmacogeneticCode = '{0}'", txtPharmacogeneticCode.Text);
            List<Pharmacogenetic> lst = BusinessLayer.GetPharmacogeneticList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Code " + txtPharmacogeneticCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 id = Convert.ToInt32(hdnID.Value);
            string FilterExpression = string.Format("PharmacogeneticCode = '{0}' AND PharmacogeneticID != {1}", txtPharmacogeneticCode.Text, id);
            List<Pharmacogenetic> lst = BusinessLayer.GetPharmacogeneticList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Code " + txtPharmacogeneticCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            PharmacogeneticDao entityDao = new PharmacogeneticDao(ctx);
            bool result = false;
            try
            {
                Pharmacogenetic entity = new Pharmacogenetic();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetPharmacogeneticMaxID(ctx).ToString();
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                Pharmacogenetic entity = BusinessLayer.GetPharmacogenetic(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                BusinessLayer.UpdatePharmacogenetic(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}