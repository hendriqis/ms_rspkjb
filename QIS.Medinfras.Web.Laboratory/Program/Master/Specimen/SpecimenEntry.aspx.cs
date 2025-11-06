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

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class SpecimenEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.SPECIMEN_SOURCE_CODE;
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
                int ID = Convert.ToInt32(Request.QueryString["id"]);
                hdnID.Value = ID.ToString();
                Specimen entity = BusinessLayer.GetSpecimen(ID);
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtSpecimenCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtSpecimenCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSpecimenName, new ControlEntrySetting(true, true, true));

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0",
         Constant.StandardCode.JENIS_TABUNG_SAMPEL));

            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.JENIS_TABUNG_SAMPEL).ToList();

            Methods.SetComboBoxField<StandardCode>(cboContainerType, lstCode1, "StandardCodeName", "StandardCodeID");
        }

        private void EntityToControl(Specimen entity)
        {
            txtSpecimenCode.Text = entity.SpecimenCode;
            txtSpecimenName.Text = entity.SpecimenName;
            cboContainerType.Value = entity.GCSpecimenContainerType;
        }

        private void ControlToEntity(Specimen entity)
        {
            entity.SpecimenCode = txtSpecimenCode.Text;
            entity.SpecimenName = txtSpecimenName.Text;
            if (cboContainerType.Value != null)
            {
                entity.GCSpecimenContainerType = cboContainerType.Value.ToString();
            }
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("SpecimenCode = '{0}'", txtSpecimenCode.Text);
            List<Specimen> lst = BusinessLayer.GetSpecimenList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Specimen with Code " + txtSpecimenCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            SpecimenDao entityDao = new SpecimenDao(ctx);
            bool result = false;
            try
            {
                Specimen entity = new Specimen();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetSpecimenMaxID(ctx).ToString();
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
                Specimen entity = BusinessLayer.GetSpecimen(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateSpecimen(entity);
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