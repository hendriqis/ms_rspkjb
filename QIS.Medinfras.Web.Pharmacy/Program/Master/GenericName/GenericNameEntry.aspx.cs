using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class GenericNameEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.PH012000;
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
                hdnID.Value = Request.QueryString["id"];
                SetControlProperties();

                string filterExp = string.Format("GenericID = {0}", hdnID.Value);
                vGenericName entity = BusinessLayer.GetvGenericNameList(filterExp).FirstOrDefault();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtDrugGenericName.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtDrugGenericName, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(vGenericName entity)
        {
            txtDrugGenericName.Text = entity.DrugGenericName;
            txtGUIDMIMSReferece.Text = entity.MIMSReferenceID.ToString();

            hdnKFAReferenceID.Value = entity.KFAReferenceID.ToString();
            txtKFAReferenceCode.Text = entity.KFAReferenceCode;
            txtKFAReferenceName.Text = entity.KFAReferenceName;

            hdnKFAReferenceIDVirtual.Value = entity.KFAReferenceNameVirtual.ToString();
            txtKFAReferenceCodeVirtual.Text = entity.KFAReferenceCodeVirtual;
            txtKFAReferenceNameVirtual.Text = entity.KFAReferenceNameVirtual;
        }

        private void ControlToEntity(GenericName entity)
        {
            entity.DrugGenericName = txtDrugGenericName.Text;
            entity.MIMSReferenceID = Guid.Parse(Request.Form[txtGUIDMIMSReferece.UniqueID]);
            entity.KFAReferenceID = Convert.ToInt32(hdnKFAReferenceID.Value);
            entity.KFAReferenceIDVirtual = Convert.ToInt32(hdnKFAReferenceIDVirtual.Value);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            GenericNameDao entityDao = new GenericNameDao(ctx);
            bool result = false;
            try
            {
                GenericName entity = new GenericName();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();
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
                GenericName entity = BusinessLayer.GetGenericName(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateGenericName(entity);
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