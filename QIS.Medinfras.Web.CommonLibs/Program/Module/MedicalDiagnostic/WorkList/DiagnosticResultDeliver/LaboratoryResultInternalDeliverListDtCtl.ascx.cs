using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class LaboratoryResultInternalDeliverListDtCtl : BaseEntryPopupCtl
    {
        private LaboratoryResultInternalDeliverList DetailPage
        {
            get { return (LaboratoryResultInternalDeliverList)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            IsAdd = false;
            hdnLaboratoryResultIDCtl.Value = param;
            string filterResultHd = string.Format("ID = {0}", param);
            vLaboratoryResultHd entity = BusinessLayer.GetvLaboratoryResultHdList(filterResultHd).FirstOrDefault();
            EntityToControl(entity);
        }

        private void EntityToControl(vLaboratoryResultHd entity)
        {
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtRegistrationDate.Text = entity.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT);
            txtRegistrationTime.Text = entity.RegistrationTime;
            txtPatientInfo.Text = entity.cfPatientInfo;
            txtTransactionNo.Text = entity.ChargesTransactionNo;
            txtResultDate.Text = entity.ResultDate.ToString(Constant.FormatString.DATE_FORMAT);
            txtResultTime.Text = entity.ResultTime;
            if (entity.TestOrderID != null && entity.TestOrderID != 0)
            {
                txtOrderNo.Text = entity.TestOrderNo;
                txtOrderDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_FORMAT);
                txtOrderTime.Text = entity.TestOrderTime;
            }
            txtNotes.Text = entity.Remarks;

            if (entity.ResultInternalDeliveredToBy != null && entity.ResultInternalDeliveredToBy != 0)
            {
                txtResultInternalDeliveredToDate.Text = entity.ResultInternalDeliveredToDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtResultInternalDeliveredToTime.Text = entity.ResultInternalDeliveredToTime;
                txtResultInternalDeliveredToFullName.Text = entity.ResultInternalDeliveredToName;
                txtResultInternalDeliveredToDateTime.Text = entity.ResultInternalDeliveredToDateTime.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
                txtResultInternalDeliveredToByName.Text = entity.ResultInternalDeliveredToByName;
            }
            else
            {
                txtResultInternalDeliveredToDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtResultInternalDeliveredToTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                txtResultInternalDeliveredToFullName.Text = "";
                txtResultInternalDeliveredToDateTime.Text = "";
                txtResultInternalDeliveredToByName.Text = "";
            }
        }

        private void ControlToEntity(LaboratoryResultHd entity)
        {
            entity.ResultInternalDeliveredToDate = Helper.GetDatePickerValue(txtResultInternalDeliveredToDate.Text);
            entity.ResultInternalDeliveredToTime = txtResultInternalDeliveredToTime.Text;
            entity.ResultInternalDeliveredTo = Convert.ToInt32(hdnResultInternalDeliveredTo.Value);
            entity.ResultInternalDeliveredToDateTime = DateTime.Now;
            entity.ResultInternalDeliveredToBy = AppSession.UserLogin.UserID;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            LaboratoryResultHdDao entityDao = new LaboratoryResultHdDao(ctx);

            try
            {
                if (hdnResultInternalDeliveredTo.Value != null && hdnResultInternalDeliveredTo.Value != "0")
                {
                    int labResultID = Convert.ToInt32(hdnLaboratoryResultIDCtl.Value);
                    LaboratoryResultHd entity = entityDao.Get(labResultID);
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Harap pilih user penerima hasil terlebih dahulu.";
                    ctx.RollBackTransaction();
                }
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
    }
}