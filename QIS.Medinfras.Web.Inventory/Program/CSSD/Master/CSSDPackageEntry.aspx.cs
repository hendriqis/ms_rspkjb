using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class CSSDPackageEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CSSD_MASTER_PACKAGE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            String ID = Request.QueryString["id"];
            if (ID != "" && ID != null)
            {
                IsAdd = false;
                hdnID.Value = ID;
                vCSSDItemPackageHd entity = BusinessLayer.GetvCSSDItemPackageHdList(string.Format("PackageID = {0}",ID)).FirstOrDefault();
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }

            txtPackageCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }
        
        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtPackageCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPackageName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }
        
        private void EntityToControl(vCSSDItemPackageHd entity)
        {
            txtPackageCode.Text = entity.PackageCode;
            txtPackageName.Text = entity.PackageName;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(CSSDItemPackageHd entity)
        {
            entity.PackageCode = txtPackageCode.Text;
            entity.PackageName = txtPackageName.Text;
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("PackageCode = '{0}' AND IsDeleted = 0", txtPackageCode.Text);
            List<CSSDItemPackageHd> lst = BusinessLayer.GetCSSDItemPackageHdList(FilterExpression);

            if (lst.Count > 0)
            {
                errMessage = "Package with code " + txtPackageCode.Text + " is already exist!";
            }

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            CSSDItemPackageHdDao entityDao = new CSSDItemPackageHdDao(ctx);

            try
            {
                CSSDItemPackageHd entity = new CSSDItemPackageHd();
                ControlToEntity(entity);
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

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("PackageCode = '{0}' AND IsDeleted = 0", txtPackageCode.Text);
            List<CSSDItemPackageHd> lst = BusinessLayer.GetCSSDItemPackageHdList(FilterExpression);

            if (lst.Count > 0)
            {
                errMessage = "Package with code " + txtPackageCode.Text + " is already exist!";
            }

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            CSSDItemPackageHdDao entityDao = new CSSDItemPackageHdDao(ctx);

            try
            {
                int PackageID = Convert.ToInt32(hdnID.Value);
                CSSDItemPackageHd entity = entityDao.Get(PackageID);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
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