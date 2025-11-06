using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class RoomBedEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnRoomID.Value = param;
            hdnIsBridgingToAplicares.Value = AppSession.IsBridgingToAPLICARES ? "1" : "0";

            Room hsu = BusinessLayer.GetRoom(Convert.ToInt32(param));
            txtHeaderText.Text = string.Format("{0} - {1}", hsu.RoomCode, hsu.RoomName);

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID NOT IN ('{1}','{2}','{3}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.BED_STATUS, Constant.BedStatus.BOOKED, Constant.BedStatus.OCCUPIED, Constant.BedStatus.WAIT_TO_BE_TRANSFERRED));
            Methods.SetComboBoxField<StandardCode>(cboBedStatus, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.BED_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
          
            BindGridView();
            txtBedCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtExtensionNo.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvBedList(string.Format("RoomID = {0} AND IsDeleted = 0 ORDER BY BedCode ASC", hdnRoomID.Value));
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
            
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(Bed entity)
        {
            entity.BedCode = txtBedCode.Text;
            entity.ExtensionNo = txtExtensionNo.Text;
            entity.GCBedStatus = cboBedStatus.Value.ToString();
            entity.IsNeedConfirmation = chkIsNeedConfirmation.Checked;
            entity.IsTemporary = chkIsTemporary.Checked;
            entity.IsBORCalculation = chkISBORCalculation.Checked;
            entity.IsPatientAccompany = chkIsPatientAccompany.Checked;
            entity.IsNewBornBed = chkIsNewBornBed.Checked;
            entity.IsPandemicBed = chkIsPandemicBed.Checked;
            entity.IsWithVentilator = chkIsWithVentilator.Checked;
            entity.IsWithPressure = chkIsWithPressure.Checked;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                Bed entity = new Bed();
                ControlToEntity(entity);
                entity.RoomID = Convert.ToInt32(hdnRoomID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertBed(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                Bed entity = BusinessLayer.GetBed(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateBed(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                Bed entity = BusinessLayer.GetBed(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateBed(entity);
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