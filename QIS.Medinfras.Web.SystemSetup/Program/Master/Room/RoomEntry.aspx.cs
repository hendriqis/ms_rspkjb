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
    public partial class RoomEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.ROOM;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            String[] param = Request.QueryString["id"].Split('|');
            if (param[0] == "edit")
            {
                IsAdd = false;
                SetControlProperties();
                String roomID = param[1];
                hdnID.Value = roomID;
                Room entity = BusinessLayer.GetRoom(Convert.ToInt32(roomID));
                EntityToControl(entity);
                hdnHealthcareID.Value = entity.HealthcareID;
            }
            else
            {
                SetControlProperties();
                hdnHealthcareID.Value = param[1];
                IsAdd = true;
            }
            txtRoomCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstRT = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.ROOM_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboGCRoomType, lstRT, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCG = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.MEDICAL_RECORD_CLASS_GROUP));
            Methods.SetComboBoxField<StandardCode>(cboGCClassGroup, lstCG, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstFG = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.MEDICAL_RECORD_FLOOR_GROUP));
            Methods.SetComboBoxField<StandardCode>(cboGCFloorGroup, lstFG, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtItemServiceCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtItemServiceName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtRoomCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRoomName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkRoomInPatientWard, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsGenderValidation, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDepositAmount, new ControlEntrySetting(true, true, false, "0"));
            SetControlEntrySetting(cboGCRoomType, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCClassGroup, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCFloorGroup, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPrefix, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(Room entity)
        {
            if (entity.ItemID != null)
            {
                ItemMaster data = BusinessLayer.GetItemMaster(Convert.ToInt32(entity.ItemID));
                txtItemServiceCode.Text = data.ItemCode;
                txtItemServiceName.Text = data.ItemName1;
            }
            txtRoomCode.Text = entity.RoomCode;
            txtRoomName.Text = entity.RoomName;
            chkRoomInPatientWard.Checked = entity.IsWardRoom;
            chkIsGenderValidation.Checked = entity.IsGenderValidation;
            txtDepositAmount.Text = entity.DepositAmount.ToString();
            cboGCRoomType.Value = entity.GCRoomType;
            cboGCClassGroup.Value = entity.GCClassGroup;
            cboGCFloorGroup.Value = entity.GCFloorGroup;
            txtPrefix.Text = entity.RoomQueuePrefix;
            hdnItemID.Value = Convert.ToString(entity.ItemID);
        }

        private void ControlToEntity(Room entity)
        {
            entity.RoomCode = txtRoomCode.Text;
            entity.RoomName = txtRoomName.Text;
            entity.IsWardRoom = chkRoomInPatientWard.Checked;
            entity.IsGenderValidation = chkIsGenderValidation.Checked;

            if (txtDepositAmount.Text != "")
            {
                entity.DepositAmount = Convert.ToDecimal(txtDepositAmount.Text);
            }
            
            entity.RoomQueuePrefix = txtPrefix.Text;

            if (!String.IsNullOrEmpty(hdnItemID.Value))
            {
                entity.ItemID = Convert.ToInt32(hdnItemID.Value);
            }
            else 
            {
                entity.ItemID = null;            
            }

            if (cboGCRoomType.Value != null)
            {
                entity.GCRoomType = cboGCRoomType.Value.ToString();
            }

            if (cboGCClassGroup.Value != null)
            {
                entity.GCClassGroup = cboGCClassGroup.Value.ToString();
            }

            if (cboGCFloorGroup.Value != null)
            {
                entity.GCFloorGroup = cboGCFloorGroup.Value.ToString();
            }
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("RoomCode = '{0}'", txtRoomCode.Text);
            List<Room> lst = BusinessLayer.GetRoomList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Room with Code " + txtRoomCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 roomID = Convert.ToInt32(hdnID.Value);
            string FilterExpression = string.Format("RoomCode = '{0}' AND RoomID != {1}", txtRoomCode.Text, roomID);
            List<Room> lst = BusinessLayer.GetRoomList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Room with Code " + txtRoomCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            RoomDao entityDao = new RoomDao(ctx);
            bool result = false;
            try
            {
                Room entity = new Room();
                ControlToEntity(entity);
                entity.HealthcareID = hdnHealthcareID.Value;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();
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
                Room entity = BusinessLayer.GetRoom(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateRoom(entity);
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