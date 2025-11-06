using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class MorphologyEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.MORPHOLOGY;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID.ToString();
                Morphology entity = BusinessLayer.GetMorphology(ID);
                EntityToControl(entity);
                if (entity.DiagnoseID != null && entity.DiagnoseID != "")
                {
                    Diagnose entityDiagnose = BusinessLayer.GetDiagnose(entity.DiagnoseID);
                    txtDiagnoseName.Text = entityDiagnose.DiagnoseName;
                }
            }
            else
            {
                IsAdd = true;
            }
            txtMorphologyID.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtMorphologyID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtMorphologyName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDiagnoseCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDiagnoseName, new ControlEntrySetting(false, false, false));
            
        }

        private void EntityToControl(Morphology entity)
        {
            txtMorphologyID.Text = entity.MorphologyID;
            txtMorphologyName.Text = entity.MorphologyName;
            txtDiagnoseCode.Text = entity.DiagnoseID;
        }

        private void ControlToEntity(Morphology entity)
        {
            entity.MorphologyID = txtMorphologyID.Text;
            entity.MorphologyName = txtMorphologyName.Text;
            entity.DiagnoseID = txtDiagnoseCode.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("MorphologyID = '{0}'", txtMorphologyID.Text);
            List<Morphology> lst = BusinessLayer.GetMorphologyList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Morphology with Code " + txtMorphologyID.Text + " is already exist!";

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