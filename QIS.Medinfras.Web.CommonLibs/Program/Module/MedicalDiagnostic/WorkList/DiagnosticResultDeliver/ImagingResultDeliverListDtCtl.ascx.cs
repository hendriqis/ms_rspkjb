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
    public partial class ImagingResultDeliverListDtCtl : BaseEntryPopupCtl
    {
        private ImagingResultDeliverList DetailPage
        {
            get { return (ImagingResultDeliverList)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            IsAdd = false;
            hdnImagingResultIDCtl.Value = param;
            string filterResultHd = string.Format("ID = {0}", param);
            vImagingResultHd entity = BusinessLayer.GetvImagingResultHdList(filterResultHd).FirstOrDefault();
            EntityToControl(entity);
        }

        private void EntityToControl(vImagingResultHd entity)
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

            if (entity.ResultDeliveredBy != null && entity.ResultDeliveredBy != 0)
            {
                txtResultDeliveredDate.Text = entity.ResultDeliveredDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtResultDeliveredTime.Text = entity.ResultDeliveredTime;
                txtResultDeliveredToName.Text = entity.ResultDeliveredToName;
                txtResultDeliveredDateTime.Text = entity.ResultDeliveredDateTime.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
                txtResultDeliveredByName.Text = entity.ResultDeliveredByName;
            }
            else
            {
                txtResultDeliveredDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtResultDeliveredTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                txtResultDeliveredToName.Text = "";
                txtResultDeliveredDateTime.Text = "";
                txtResultDeliveredByName.Text = "";
            }
        }

        private void ControlToEntity(ImagingResultHd entity)
        {
            entity.ResultDeliveredDate = Helper.GetDatePickerValue(txtResultDeliveredDate.Text);
            entity.ResultDeliveredTime = txtResultDeliveredTime.Text;
            entity.ResultDeliveredToName = txtResultDeliveredToName.Text;
            entity.ResultDeliveredDateTime = DateTime.Now;
            entity.ResultDeliveredBy = AppSession.UserLogin.UserID;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ImagingResultHdDao entityDao = new ImagingResultHdDao(ctx);

            try
            {
                int labResultID = Convert.ToInt32(hdnImagingResultIDCtl.Value);
                ImagingResultHd entity = entityDao.Get(labResultID);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                ctx.CommitTransaction();
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