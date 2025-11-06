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

namespace QIS.Medinfras.Web.Nursing.Program
{
    public partial class NursingDiagnoseEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nursing.NURSING_DIAGNOSE;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                vNursingDiagnose entity = BusinessLayer.GetvNursingDiagnoseList(String.Format("NurseDiagnoseID = {0}",Convert.ToInt32(ID))).FirstOrDefault();
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtNursingDiagnoseCode.Focus();
        }

        protected override void SetControlProperties()
        {
            List<NursingDomainClass> lstDomain = BusinessLayer.GetNursingDomainClassList(string.Format("GCNurseDomainClassType = '{0}' AND IsDeleted = 0", Constant.NursingDomainClassType.DOMAIN));
            Methods.SetComboBoxField<NursingDomainClass>(cboNurseDomain, lstDomain, "NurseDomainClassText", "NurseDomainClassID");

            List<StandardCode> lstStdCode = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.NURSING_DIAGNOSIS_TYPE));
            lstStdCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboGCNursingDiagnosisType, lstStdCode.Where(lst => lst.ParentID == Constant.StandardCode.NURSING_DIAGNOSIS_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtNursingDiagnoseCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNursingDiagnoseName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboNurseDomain, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCNursingDiagnosisType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtClassCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtClassName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtDescription, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(vNursingDiagnose entity)
        {
            txtNursingDiagnoseCode.Text = entity.NurseDiagnoseCode;
            txtNursingDiagnoseName.Text = entity.NurseDiagnoseName;
            cboNurseDomain.Value = entity.NurseDomainClassParentID.ToString();
            cboGCNursingDiagnosisType.Value = entity.GCNursingDiagnosisType;
            hdnNurseDomainClassID.Value = entity.NurseDomainClassID.ToString();
            txtClassCode.Text = entity.NurseDomainClassCode;
            txtClassName.Text = entity.NurseDomainClassText;
            txtDescription.Text = entity.Description;
        }

        private void ControlToEntity(NursingDiagnose entity)
        {
            entity.NurseDiagnoseCode= txtNursingDiagnoseCode.Text;
            entity.NurseDiagnoseName= txtNursingDiagnoseName.Text;
            entity.NurseDomainClassID = Convert.ToInt32(hdnNurseDomainClassID.Value);
            if (cboGCNursingDiagnosisType.Value != null)
                entity.GCNursingDiagnosisType = cboGCNursingDiagnosisType.Value.ToString();
            else
                entity.GCNursingDiagnosisType = null;
            entity.Description = txtDescription.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            NursingDiagnoseDao entityDao = new NursingDiagnoseDao(ctx);
            bool result = false;
            try
            {
                NursingDiagnose entity = new NursingDiagnose();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetNursingDiagnoseMaxID(ctx).ToString();
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
                NursingDiagnose entity = BusinessLayer.GetNursingDiagnose(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNursingDiagnose(entity);
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