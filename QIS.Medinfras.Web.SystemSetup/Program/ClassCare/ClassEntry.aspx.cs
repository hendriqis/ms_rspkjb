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
    public partial class ClassEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.CLASS_CARE;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                hdnID.Value = Request.QueryString["id"];
                SetControlProperties();
                ClassCare entity = BusinessLayer.GetClassCare(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtClassCode.Focus();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(String.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.RL_CLASS));
            Methods.SetComboBoxField<StandardCode>(cboGCClassRL, lst, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtClassCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtClassName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtShortName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCClassRL, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMarginPercentage1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMarginPercentage2, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMarginPercentage3, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDepositAmount, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsInPatientClass, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(ClassCare entity)
        {
            txtClassCode.Text = entity.ClassCode;
            txtClassName.Text = entity.ClassName;
            txtShortName.Text = entity.ShortName;
            cboGCClassRL.Value = entity.GCClassRL;
            txtMarginPercentage1.Text = entity.MarginPercentage1.ToString();
            txtMarginPercentage2.Text = entity.MarginPercentage2.ToString();
            txtMarginPercentage3.Text = entity.MarginPercentage3.ToString();
            txtDepositAmount.Text = entity.DepositAmount.ToString();
            chkIsInPatientClass.Checked = entity.IsInPatientClass;
        }

        private void ControlToEntity(ClassCare entity)
        {
            entity.ClassCode = txtClassCode.Text;
            entity.ClassName = txtClassName.Text;
            entity.ShortName = txtShortName.Text;
            entity.GCClassRL = cboGCClassRL.Value.ToString();
            entity.MarginPercentage1 = Convert.ToDecimal(txtMarginPercentage1.Text);
            entity.MarginPercentage2 = Convert.ToDecimal(txtMarginPercentage2.Text);
            entity.MarginPercentage3 = Convert.ToDecimal(txtMarginPercentage3.Text);
            entity.DepositAmount = Convert.ToDecimal(txtDepositAmount.Text);
            entity.IsInPatientClass = chkIsInPatientClass.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ClassCode = '{0}'", txtClassCode.Text);
            List<ClassCare> lst = BusinessLayer.GetClassCareList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Class Care with Code " + txtClassCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("ClassCode = '{0}' AND ClassID != {1}", txtClassCode.Text, hdnID.Value);
            List<ClassCare> lst = BusinessLayer.GetClassCareList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Class Care with Code " + txtClassCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ClassCare entity = new ClassCare();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertClassCare(entity);
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
                ClassCare entity = BusinessLayer.GetClassCare(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateClassCare(entity);
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