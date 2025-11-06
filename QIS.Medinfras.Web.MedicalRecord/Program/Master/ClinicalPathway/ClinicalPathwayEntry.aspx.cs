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

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class ClinicalPathwayEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.CLINICAL_PATHWAY;
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
                SetControlProperties();
                hdnID.Value = ID;
                ClinicalPathway entity = BusinessLayer.GetClinicalPathway(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtClinicalPathwayCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }


        protected override void SetControlProperties()
        {
        }
        /*IRENE*/
        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtClinicalPathwayCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtClinicalPathwayName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false)); ;
        }

        private void EntityToControl(ClinicalPathway entity)
        {
            txtClinicalPathwayCode.Text = entity.ClinicalPathwayCode;
            txtClinicalPathwayName.Text = entity.ClinicalPathwayName;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(ClinicalPathway entity)
        {
            entity.ClinicalPathwayCode = txtClinicalPathwayCode.Text;
            entity.ClinicalPathwayName = txtClinicalPathwayName.Text;
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ClinicalPathwayCode = '{0}'", txtClinicalPathwayCode.Text);
            List<ClinicalPathway> lst = BusinessLayer.GetClinicalPathwayList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Clinical Pathway with Code " + txtClinicalPathwayCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            ClinicalPathwayDao entityDao = new ClinicalPathwayDao(ctx);
            bool result = false;
            try
            {
                ClinicalPathway entity = new ClinicalPathway();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetClinicalPathwayMaxID(ctx).ToString();//memunculkan yg terakhr diadd
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
                ClinicalPathway entity = BusinessLayer.GetClinicalPathway(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateClinicalPathway(entity);
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