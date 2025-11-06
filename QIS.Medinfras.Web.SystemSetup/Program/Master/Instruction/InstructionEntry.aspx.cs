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
    public partial class InstructionEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.INSTRUCTION;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                int ID = Convert.ToInt32(Request.QueryString["id"]);
                hdnID.Value = ID.ToString();
                SetControlProperties();
                Instruction entity = BusinessLayer.GetInstruction(ID);
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtInstructionCode.Focus();
        }

        protected override void SetControlProperties()
        {
            string filter = string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.PATIENT_INSTRUCTION_GROUP);
            List<StandardCode> lstBodyDiagramGroup = BusinessLayer.GetStandardCodeList(filter);
            Methods.SetComboBoxField<StandardCode>(cboGCInstructionGroup, lstBodyDiagramGroup, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtInstructionCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCInstructionGroup, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtInstructionDescription1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtInstructionDescription2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDisplayOrder, new ControlEntrySetting(true, true, true));

        }

        private void EntityToControl(Instruction entity)
        {
            txtInstructionCode.Text = entity.InstructionCode;
            cboGCInstructionGroup.Value= entity.GCInstructionGroup;
            txtInstructionDescription1.Text = entity.InstructionDescription1;
            txtInstructionDescription2.Text = entity.InstructionDescription2;
            txtDisplayOrder.Text = entity.DisplayOrder.ToString();
        }

        private void ControlToEntity(Instruction entity)
        {
            entity.InstructionCode = txtInstructionCode.Text;
            entity.GCInstructionGroup = cboGCInstructionGroup.Value.ToString();
            entity.InstructionDescription1 = txtInstructionDescription1.Text;
            entity.InstructionDescription2 = txtInstructionDescription2.Text;
            entity.DisplayOrder = Convert.ToInt16(txtDisplayOrder.Text);
        }
        
        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("InstructionCode = '{0}'", txtInstructionCode.Text);
            List<Instruction> lst = BusinessLayer.GetInstructionList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Instruction with Code " + txtInstructionCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }
        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 InstructionID = Convert.ToInt32(hdnID.Value);
            string FilterExpression = string.Format("InstructionCode = '{0}' AND InstructionID != {1}", txtInstructionCode.Text, InstructionID);
            List<Instruction> lst = BusinessLayer.GetInstructionList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Instruction with Code " + txtInstructionCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            InstructionDao entityDao = new InstructionDao(ctx);
            bool result = false;
            try
            {
                Instruction entity = new Instruction();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetInstructionMaxID(ctx).ToString();
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
                Instruction entity = BusinessLayer.GetInstruction(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateInstruction(entity);
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