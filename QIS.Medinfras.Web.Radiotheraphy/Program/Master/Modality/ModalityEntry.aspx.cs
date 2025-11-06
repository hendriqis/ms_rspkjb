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

namespace QIS.Medinfras.Web.Imaging.Program
{
    public partial class ModalityEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Imaging.MODALITY;
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
                Modality entity = BusinessLayer.GetModality(ID);
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }

            SetControlProperties();
            txtModalityCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}')", Constant.StandardCode.MEDICAL_IMAGING_MODALITIES);
            List<StandardCode> lstScItemUnit = BusinessLayer.GetStandardCodeList(filterExpression);
            lstScItemUnit.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboGCModality, lstScItemUnit, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtModalityCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtModalityName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCModality, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAE, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(Modality entity)
        {
            txtModalityCode.Text = entity.ModalityCode;
            txtModalityName.Text = entity.ModalityName;
            if (entity.GCModality != null)
            {
                cboGCModality.Value = entity.GCModality;
            }
            txtAE.Text = entity.AETitle;
        }

        private void ControlToEntity(Modality entity)
        {
            entity.ModalityCode = txtModalityCode.Text;
            entity.ModalityName = txtModalityName.Text;
            if (cboGCModality.Value == null)
            {
                entity.GCModality = "";
            }
            else
            {
                entity.GCModality = cboGCModality.Value.ToString();
            }
            entity.AETitle = txtAE.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ModalityCode = '{0}'", txtModalityCode.Text);
            List<Modality> lst = BusinessLayer.GetModalityList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Modality with Code " + txtModalityCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            ModalityDao entityDao = new ModalityDao(ctx);
            bool result = false;
            try
            {
                Modality entity = new Modality();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetModalityMaxID(ctx).ToString();
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
                Modality entity = BusinessLayer.GetModality(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateModality(entity);
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