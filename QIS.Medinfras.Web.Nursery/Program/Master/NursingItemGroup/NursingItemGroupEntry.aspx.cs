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
    public partial class NursingItemGroupEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nursing.NURSING_ITEM_GROUP;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                vNursingItemGroupSubGroup entity = BusinessLayer.GetvNursingItemGroupSubGroupList(String.Format("NursingItemGroupSubGroupID = {0}", Convert.ToInt32(ID))).FirstOrDefault();

                SetControlProperties();

                EntityToControl(entity);
            }
            else
            {
                
                IsAdd = true;
                SetControlProperties();
            }
            txtNursingItemGroupCode.Focus();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstStdCode = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.NURSING_EVALUATION, Constant.StandardCode.NURSING_DIAGNOSIS_TYPE));
            lstStdCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboGCNursingDiagnosisType, lstStdCode.Where(lst => lst.ParentID == Constant.StandardCode.NURSING_DIAGNOSIS_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCNursingEvaluation, lstStdCode.Where(lst => lst.ParentID == Constant.StandardCode.NURSING_EVALUATION).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtNursingItemGroupCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNursingItemGroupText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtParentCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtParentName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDisplayCaption, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDisplayOrder, new ControlEntrySetting(true, true, true,0));
            SetControlEntrySetting(cboGCNursingDiagnosisType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCNursingEvaluation, new ControlEntrySetting(true, true, false));
        }

        protected string OnGetConstantNursingDomain()
        {
            return Constant.NursingDomainClassType.DOMAIN;
        }


        private void EntityToControl(vNursingItemGroupSubGroup entity)
        {
            txtNursingItemGroupCode.Text = entity.NursingItemGroupSubGroupCode;
            txtNursingItemGroupText.Text = entity.NursingItemGroupSubGroupText;
            hdnParentID.Value = entity.ParentID.ToString();
            txtParentCode.Text = entity.ParentCode;
            txtParentName.Text = entity.ParentText;
            txtDisplayCaption.Text = entity.DisplayCaption;
            cboGCNursingEvaluation.Value = entity.GCNursingEvaluation;
            cboGCNursingDiagnosisType.Value = entity.GCNursingDiagnosisType;
            txtDisplayOrder.Text = entity.DisplayOrder.ToString();
            chkIsHeader.Checked = entity.IsHeader;
            chkIsNursingOutcome.Checked = entity.IsNursingOutcome;
            chkIsSubjectiveObjectiveData.Checked = entity.IsSubjectiveObjectiveData;
            chkIsShowInReport.Checked = entity.IsShowInReport;
        }

        private void ControlToEntity(NursingItemGroupSubGroup entity)
        {
            entity.NursingItemGroupSubGroupCode = txtNursingItemGroupCode.Text;
            entity.NursingItemGroupSubGroupText = txtNursingItemGroupText.Text;
            if (hdnParentID.Value != String.Empty)
                entity.ParentID = Convert.ToInt32(hdnParentID.Value);
            else
                entity.ParentID = null;
            entity.DisplayCaption = txtDisplayCaption.Text;
            if (cboGCNursingDiagnosisType.Value != null)
                entity.GCNursingDiagnosisType = cboGCNursingDiagnosisType.Value.ToString();
            else
                entity.GCNursingDiagnosisType = null;
            if (cboGCNursingEvaluation.Value != null)
                entity.GCNursingEvaluation = cboGCNursingEvaluation.Value.ToString();
            else
                entity.GCNursingEvaluation = null;
            entity.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);
            entity.IsHeader = chkIsHeader.Checked;
            entity.IsNursingOutcome = chkIsNursingOutcome.Checked;
            entity.IsSubjectiveObjectiveData = chkIsSubjectiveObjectiveData.Checked;
            entity.IsShowInReport = chkIsShowInReport.Checked;

        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            NursingItemGroupSubGroupDao entityDao = new NursingItemGroupSubGroupDao(ctx);
            bool result = false;
            try
            {
                NursingItemGroupSubGroup entity = new NursingItemGroupSubGroup();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetNursingItemGroupSubGroupMaxID(ctx).ToString();
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
                NursingItemGroupSubGroup entity = BusinessLayer.GetNursingItemGroupSubGroup(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNursingItemGroupSubGroup(entity);
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