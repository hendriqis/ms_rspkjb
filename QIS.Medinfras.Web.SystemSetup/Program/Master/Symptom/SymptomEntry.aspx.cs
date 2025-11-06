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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class SymptomEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.SYMPTOM;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String symptomID = Request.QueryString["id"];
                hdnID.Value = symptomID.ToString();
                SetControlProperties();
                Symptom entity = BusinessLayer.GetSymptom(Convert.ToInt32(symptomID));
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtSymptomCode.Focus();
        }
        protected override void SetControlProperties()
        {
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.REVIEW_OF_SYSTEM));
            Methods.SetComboBoxField<StandardCode>(cboROSystem, listStandardCode, "StandardCodeName", "StandardCodeID");
            cboROSystem.SelectedIndex = 0;
        }
        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtSymptomCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtSymptomName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboROSystem, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(rboGender, new ControlEntrySetting(true, true, true));

        }

        private void EntityToControl(Symptom entity)
        {
            txtSymptomCode.Text = entity.SymptomCode;
            txtSymptomName.Text = entity.SymptomName;
            cboROSystem.Value = entity.GCROSystem;
            switch (entity.Gender)
            {
                case " ": rboGender.Items.FindByText("Both").Selected = true; break;
                case "F": rboGender.Items.FindByText("Female").Selected = true; break;
                case "M": rboGender.Items.FindByText("Male").Selected = true; break;
            }
        }

        private void ControlToEntity(Symptom entity)
        {
            entity.SymptomCode = txtSymptomCode.Text;
            entity.SymptomName = txtSymptomName.Text;
            entity.GCROSystem = cboROSystem.Value.ToString();
            entity.Gender = rboGender.SelectedItem.Value.ToString();
            if (entity.Gender == "B") entity.Gender = "";
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("SymptomCode = '{0}'", txtSymptomCode.Text);
            List<Symptom> lst = BusinessLayer.GetSymptomList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Symptom with Code " + txtSymptomCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            SymptomDao entityDao = new SymptomDao(ctx);
            bool result = false;
            try
            {
                Symptom entity = new Symptom();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetSymptomMaxID(ctx).ToString();
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
                Symptom entity = BusinessLayer.GetSymptom(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateSymptom(entity);
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