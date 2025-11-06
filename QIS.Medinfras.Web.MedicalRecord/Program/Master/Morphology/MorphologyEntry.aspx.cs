using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class MorphologyEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.MORPHOLOGY;
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
                Morphology entity = BusinessLayer.GetMorphology(ID);
                EntityToControl(entity);
                if (entity.DiagnoseID != null && entity.DiagnoseID != "")
                {
                    Diagnose entityD = BusinessLayer.GetDiagnose(entity.DiagnoseID);
                    txtDiagnoseName.Text = entityD.DiagnoseID;
                }
            }
            else
            {
                IsAdd = true;
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtMorphologyID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDiagnoseID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDiagnoseName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtMorphologyName, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(Morphology entity)
        {
            txtMorphologyID.Text = entity.MorphologyID;
            txtDiagnoseID.Text = entity.DiagnoseID;
            txtMorphologyName.Text = entity.MorphologyName;
        }

        private void ControlToEntity(Morphology entity)
        {
            entity.MorphologyID = txtMorphologyID.Text;
            entity.DiagnoseID = txtDiagnoseID.Text;
            entity.MorphologyName = txtMorphologyName.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("MorphologyID = '{0}'", txtMorphologyID.Text);
            List<Morphology> lst = BusinessLayer.GetMorphologyList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Diagnose Group with Code " + txtMorphologyID.Text + " is already exist!";
            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                Morphology entity = new Morphology();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertMorphology(entity);
                retval = entity.MorphologyID;
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                Morphology entity = BusinessLayer.GetMorphology(hdnID.Value);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMorphology(entity);
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