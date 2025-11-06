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
    public partial class PatientInstructionEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PATIENT_INSTRUCTION;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                SetControlProperties();
                hdnID.Value = ID;
                Instruction entity = BusinessLayer.GetInstruction(Convert.ToInt32(hdnID.Value));
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
            List<StandardCode> lststd = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.PATIENT_INSTRUCTION_GROUP));
            Methods.SetComboBoxField<StandardCode>(cboInstructionGroup, lststd.Where(sc => sc.ParentID == Constant.StandardCode.PATIENT_INSTRUCTION_GROUP).ToList(), "StandardCodeName", "StandardCodeID");
            cboInstructionGroup.SelectedIndex = 0;
        }
        /*IRENE*/
        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtInstructionCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboInstructionGroup, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtInstructionDesc1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtInstructionDesc2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDisplayOrder, new ControlEntrySetting(true, true, true, 0));;
        }

        private void EntityToControl(Instruction entity)
        {
            txtInstructionCode.Text = entity.InstructionCode;
            cboInstructionGroup.Value = entity.GCInstructionGroup;
            txtInstructionDesc1.Text = entity.InstructionDescription1;
            txtInstructionDesc2.Text = entity.InstructionDescription2;
            txtDisplayOrder.Text = entity.DisplayOrder.ToString();
        }

        private void ControlToEntity(Instruction entity)
        {
            entity.InstructionCode = txtInstructionCode.Text;
            entity.GCInstructionGroup = cboInstructionGroup.Value.ToString();
            entity.InstructionDescription1 = txtInstructionDesc1.Text;
            entity.InstructionDescription2 = txtInstructionDesc2.Text;
            entity.DisplayOrder = Convert.ToInt16(txtDisplayOrder.Text);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("InstructionCode = '{0}'", txtInstructionCode.Text);
            List<Instruction> lst = BusinessLayer.GetInstructionList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Patient Instruction with Code " + txtInstructionCode.Text + " is already exist!";

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
                retval = BusinessLayer.GetInstructionMaxID(ctx).ToString();//memunculkan yg terakhr diadd
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