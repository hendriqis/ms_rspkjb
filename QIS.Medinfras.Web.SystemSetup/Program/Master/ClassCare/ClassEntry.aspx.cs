using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ClassEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.CLASS_CARE;
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
                ClassCare entity = BusinessLayer.GetClassCare(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtClassCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(String.Format(
                                                                        "ParentID IN ('{0}','{1}','{2}') AND IsDeleted = 0 AND IsActive = 1",
                                                                        Constant.StandardCode.RL_CLASS,
                                                                        Constant.StandardCode.BPJS_KELAS_NAIK,
                                                                        Constant.StandardCode.INCBGS_CLASS
                                                                    ));

            Methods.SetComboBoxField<StandardCode>(cboGCClassRL, lst.Where(t => t.ParentID == Constant.StandardCode.RL_CLASS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBPJSKelasNaik, lst.Where(t => t.ParentID == Constant.StandardCode.BPJS_KELAS_NAIK).ToList(), "StandardCodeName", "StandardCodeID");

            lst.Add(new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboINACBGClass, lst.Where(t => t.ParentID == Constant.StandardCode.INCBGS_CLASS || t.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            List<Variable> lstVariable = new List<Variable>();
            lstVariable.Add(new Variable { Code = "0", Value = "" });
            lstVariable.Add(new Variable { Code = "1", Value = "1" });
            lstVariable.Add(new Variable { Code = "2", Value = "2" });
            lstVariable.Add(new Variable { Code = "3", Value = "3" });
            Methods.SetComboBoxField<Variable>(cboBPJSClassType, lstVariable, "Value", "Code");
            cboBPJSClassType.Value = "0";
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtClassCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtClassName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtShortName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtClassPriority, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCClassRL, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMarginPercentage1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMarginPercentage2, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMarginPercentage3, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDepositAmount, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsInPatientClass, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsUsedInChargeClass, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsBPJSClass, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBPJSCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBPJSName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtKodeKelasApplicares, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNamaKelasApplicares, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboBPJSClassType, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboINACBGClass, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(chkIsControlUnitPrice, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtServiceUnitPrice, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDrugSuppliesUnitPrice, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtLogisticUnitPrice, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtPengaliINADitempati, new ControlEntrySetting(true, true, false));

            #region Administration & Service
            //SetControlEntrySetting(chkAdministrationPercentage, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAdministrationAmount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtMinAdministrationAmount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtMaxAdministrationAmount, new ControlEntrySetting(true, true, true, 0));

            SetControlEntrySetting(txtPatientAdmAmount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtMinPatientAdmAmount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtMaxPatientAdmAmount, new ControlEntrySetting(true, true, true, 0));

            //SetControlEntrySetting(chkServiceChargePercentage, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtServiceChargeAmount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtMinServiceChargeAmount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtMaxServiceChargeAmount, new ControlEntrySetting(true, true, true, 0));

            SetControlEntrySetting(txtPatientServiceAmount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtMinPatientServiceAmount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtMaxPatientServiceAmount, new ControlEntrySetting(true, true, true, 0));
            #endregion
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
            chkIsUsedInChargeClass.Checked = entity.IsUsedInChargeClass;
            chkIsBPJSClass.Checked = entity.IsBPJSClass;
            if (chkIsBPJSClass.Checked)
            {
                trBPJSClassCodeName.Attributes.CssStyle.Remove("display");
                trBPJSClassType.Attributes.CssStyle.Remove("display");
                trBPJSKelasNaik.Attributes.CssStyle.Remove("display");
            }
            txtBPJSCode.Text = entity.BPJSClassCode;
            txtBPJSName.Text = entity.BPJSClassName;
            txtInhealthClassCode.Text = entity.InhealthClassCode;
            txtInhealthClassName.Text = entity.InhealthClassName;
            if (entity.BPJSClassType == "1")
            {
                cboBPJSClassType.Value = "1";
            }
            else if (entity.BPJSClassType == "2")
            {
                cboBPJSClassType.Value = "2";
            }
            else if (entity.BPJSClassType == "3")
            {
                cboBPJSClassType.Value = "3";
            }
            else
            {
                cboBPJSClassType.Value = "0";
            }

            cboBPJSKelasNaik.Value = entity.BPJSKelasRawatNaikNama;

            cboINACBGClass.Value = entity.GCINACBGClass;

            txtClassPriority.Text = entity.ClassPriority.ToString();
            txtKodeKelasApplicares.Text = entity.AplicaresClassCode.ToString();
            txtNamaKelasApplicares.Text = entity.AplicaresClassName.ToString();

            chkIsControlUnitPrice.Checked = entity.IsControlUnitPrice;
            txtServiceUnitPrice.Text = entity.ServiceUnitPrice.ToString();
            txtDrugSuppliesUnitPrice.Text = entity.DrugSuppliesUnitPrice.ToString();
            txtLogisticUnitPrice.Text = entity.LogisticUnitPrice.ToString();

            txtPengaliINADitempati.Text = entity.Pengali_INADitempati.ToString();

            #region Administration & Service
            chkAdministrationPercentage.Checked = entity.IsAdministrationFeeInPct;
            txtAdministrationAmount.Text = entity.AdministrationFeeAmount.ToString();
            txtMinAdministrationAmount.Text = entity.MinAdministrationFeeAmount.ToString();
            txtMaxAdministrationAmount.Text = entity.MaxAdministrationFeeAmount.ToString();

            chkPatientAdminPercentage.Checked = entity.IsPatientAdmFeeInPct;
            txtPatientAdmAmount.Text = entity.PatientAdmFeeAmount.ToString();
            txtMinPatientAdmAmount.Text = entity.MinPatientAdmFeeAmount.ToString();
            txtMaxPatientAdmAmount.Text = entity.MaxPatientAdmFeeAmount.ToString();

            chkServiceChargePercentage.Checked = entity.IsServiceFeeInPct;
            txtServiceChargeAmount.Text = entity.ServiceFeeAmount.ToString();
            txtMinServiceChargeAmount.Text = entity.MinServiceFeeAmount.ToString();
            txtMaxServiceChargeAmount.Text = entity.MaxServiceFeeAmount.ToString();

            chkPatientServicePercentage.Checked = entity.IsPatientServiceFeeInPct;
            txtPatientServiceAmount.Text = entity.PatientServiceFeeAmount.ToString();
            txtMinPatientServiceAmount.Text = entity.MinPatientServiceFeeAmount.ToString();
            txtMaxPatientServiceAmount.Text = entity.MaxPatientServiceFeeAmount.ToString();
            #endregion
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
            entity.IsUsedInChargeClass = chkIsUsedInChargeClass.Checked;
            entity.IsBPJSClass = chkIsBPJSClass.Checked;
            entity.ClassPriority = Convert.ToInt16(txtClassPriority.Text);
            entity.BPJSClassType = "1";
            entity.BPJSClassCode = txtBPJSCode.Text;
            entity.BPJSClassName = txtBPJSName.Text;
            entity.BPJSClassType = cboBPJSClassType.Value.ToString() != "0" ? cboBPJSClassType.Value.ToString() : null;

            if (cboINACBGClass.Value != null)
            {
                if (cboINACBGClass.Value.ToString() != "")
                {
                    entity.GCINACBGClass = cboINACBGClass.Value.ToString();
                }
                else
                {
                    entity.GCINACBGClass = null;
                }
            }
            else
            {
                entity.GCINACBGClass = null;
            }

            entity.InhealthClassCode = txtInhealthClassCode.Text;
            entity.InhealthClassName = txtInhealthClassName.Text;

            string standardCodeID = Convert.ToString(cboBPJSKelasNaik.Value);
            if (standardCodeID != "")
            {
                StandardCode entityStandardCode = BusinessLayer.GetStandardCode(standardCodeID);
                entity.BPJSKelasRawatNaikType = entityStandardCode.TagProperty;
                entity.BPJSKelasRawatNaikKode = entityStandardCode.StandardCodeID;
                entity.BPJSKelasRawatNaikNama = entityStandardCode.StandardCodeName;
            }

            entity.AplicaresClassCode = txtKodeKelasApplicares.Text;
            entity.AplicaresClassName = txtNamaKelasApplicares.Text;

            entity.IsControlUnitPrice = chkIsControlUnitPrice.Checked;
            entity.ServiceUnitPrice = Convert.ToDecimal(txtServiceUnitPrice.Text);
            entity.DrugSuppliesUnitPrice = Convert.ToDecimal(txtDrugSuppliesUnitPrice.Text);
            entity.LogisticUnitPrice = Convert.ToDecimal(txtLogisticUnitPrice.Text);

            entity.Pengali_INADitempati = Convert.ToDecimal(txtPengaliINADitempati.Text);

            #region Administration & Service
            entity.IsAdministrationFeeInPct = chkAdministrationPercentage.Checked;
            entity.AdministrationFeeAmount = Convert.ToDecimal(txtAdministrationAmount.Text);
            entity.MinAdministrationFeeAmount = Convert.ToDecimal(txtMinAdministrationAmount.Text);
            entity.MaxAdministrationFeeAmount = Convert.ToDecimal(txtMaxAdministrationAmount.Text);

            entity.IsPatientAdmFeeInPct = chkPatientAdminPercentage.Checked;
            entity.PatientAdmFeeAmount = Convert.ToDecimal(txtPatientAdmAmount.Text);
            entity.MinPatientAdmFeeAmount = Convert.ToDecimal(txtMinPatientAdmAmount.Text);
            entity.MaxPatientAdmFeeAmount = Convert.ToDecimal(txtMaxPatientAdmAmount.Text);

            entity.IsServiceFeeInPct = chkServiceChargePercentage.Checked;
            entity.ServiceFeeAmount = Convert.ToDecimal(txtServiceChargeAmount.Text);
            entity.MinServiceFeeAmount = Convert.ToDecimal(txtMinServiceChargeAmount.Text);
            entity.MaxServiceFeeAmount = Convert.ToDecimal(txtMaxServiceChargeAmount.Text);

            entity.IsPatientServiceFeeInPct = chkPatientServicePercentage.Checked;
            entity.PatientServiceFeeAmount = Convert.ToDecimal(txtPatientServiceAmount.Text);
            entity.MinPatientServiceFeeAmount = Convert.ToDecimal(txtMinPatientServiceAmount.Text);
            entity.MaxPatientServiceFeeAmount = Convert.ToDecimal(txtMaxPatientServiceAmount.Text);
            #endregion
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ClassCode = '{0}' AND IsDeleted = 0", txtClassCode.Text);
            List<ClassCare> lst = BusinessLayer.GetClassCareList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Class Care with Code " + txtClassCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }
      
        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("ClassCode = '{0}' AND IsDeleted = 0 AND ClassID != {1}", txtClassCode.Text, hdnID.Value);
            List<ClassCare> lst = BusinessLayer.GetClassCareList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Class Care with Code " + txtClassCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            ClassCareDao entityDao = new ClassCareDao(ctx);
            bool result = false;
            try
            {
                ClassCare entity = new ClassCare();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetClassCareMaxID(ctx).ToString();
                ctx.CommitTransaction();
                result = true;
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
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}