using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SpecimenDeliveryEntryCtl1 : BaseEntryPopupCtl3
    {
        protected int gridSpecimenPageCount = 1;

        protected static string _visitID = "0";
        protected static string _orderID = "0";

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            IsAdd = false;
            hdnVisitID.Value = "0";
            OnControlEntrySettingLocal();
            ReInitControl();
            hdnVisitID.Value = paramInfo[0];
            hdnTestOrderID.Value = paramInfo[1];
            _orderID = hdnTestOrderID.Value;
            _visitID = hdnVisitID.Value;
            vTestOrderHd entityHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}",Convert.ToInt32(hdnTestOrderID.Value))).FirstOrDefault();
            if (entityHd != null)
            {	  
                DateTime specimenDate = Convert.ToDateTime(entityHd.SpecimenTakenDate);
                if (specimenDate.Year != 1900)
		            txtSampleDate.Text = specimenDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                txtSampleTime.Text = entityHd.SpecimenTakenTime;

                if (entityHd.SpecimenTakenBy != 0)
                {
                    txtSpecimenTakenName.Text = entityHd.SpecimenTakenByName;
                    txtSpecimenTakenName.Enabled = false;
                }

                if (entityHd.SpecimenDeliveryBy != 0)
                {
                    hdnSpecimenDeliveryID.Value = entityHd.SpecimenDeliveryBy.ToString();
                    txtSpecimenDeliveryName.Text = entityHd.SpecimenDeliveryByName;
                    txtSpecimenDeliveryName.Enabled = false;
                }
                else
                {
                    txtSpecimenDeliveryName.Text = AppSession.UserLogin.UserFullName;
                    txtSpecimenDeliveryName.Enabled = false;
                }

                chkIsUsingExistingSample.Checked = entityHd.IsAdditionalOrder;
                txtRemarks.Text = entityHd.SpecimenRemarks;
            }

            BindGridViewSpecimen(1, true, ref gridSpecimenPageCount);
        }

        private void SetControlProperties()
        {
        }

        private void OnControlEntrySettingLocal()
        {
            SetControlEntrySetting(txtDeliveryDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtDeliveryTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
  
            SetControlProperties();
        }

        private void ControlToEntity(TestOrderHd entityHd)
        {
            entityHd.IsSpecimenDelivered = true;
            entityHd.SpecimenDeliveryBy = Convert.ToInt32(AppSession.UserLogin.UserID);
            entityHd.SpecimenDeliveryDate = Helper.GetDatePickerValue(txtDeliveryDate);
            entityHd.SpecimenDeliveryTime = txtDeliveryTime.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            return result;
        }

        private bool IsValid(ref string errMessage)
        {
            bool result = true;
            StringBuilder errMsg = new StringBuilder();

            #region Sample Date
            string date = txtDeliveryDate.Text;
            if (string.IsNullOrEmpty(date))
            {
                errMsg.AppendLine("Tanggal Pengiriman Sampel harus diisi");
            }
            else
            {
                DateTime startDate;
                string format = Constant.FormatString.DATE_PICKER_FORMAT;
                try
                {
                    startDate = DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    errMsg.AppendLine(string.Format("Format Tanggal Pengiriman Sampel {0} tidak benar atau invalid", date));
                }

                DateTime sdate = Helper.GetDatePickerValue(date);
                if (DateTime.Compare(sdate, DateTime.Now.Date) > 0)
                {
                    errMsg.AppendLine("Tanggal pengiriman sampel harus lebih kecil atau sama dengan tanggal saat ini.");
                }
            }
            #endregion

            #region Delivery Time
            if (string.IsNullOrEmpty(txtDeliveryTime.Text))
            {
                errMsg.AppendLine("Jam pengiriman sampel harus diisi");
            }
            else
            {
                if (!Methods.ValidateTimeFormat(txtDeliveryTime.Text))
                    errMsg.AppendLine("Format Jam Pengiriman Sampel tidak sesuai format (HH:MM)");
            }
            #endregion

            #region Pengirim Sampel
            if (string.IsNullOrEmpty(txtSpecimenTakenName.Text))
            {
                errMsg.AppendLine("Nama yang melakukan pengiriman sampel harus diisi");
            }
            #endregion

            errMessage = errMsg.ToString();

            result = string.IsNullOrEmpty(errMessage.ToString());

            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            try
            {
                if (IsValid(ref errMessage))
                {
                    TestOrderHd entityUpdate = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));
                    ControlToEntity(entityUpdate);
                    entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateTestOrderHd(entityUpdate);

                    retVal = entityUpdate.TestOrderID.ToString();
                }
                else
                {
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        #region Specimen
        private void BindGridViewSpecimen(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (Page.IsCallback && _orderID != "0")
            {
                hdnTestOrderID.Value = _orderID;
            }

            List<vTestOrderSpecimen> lstEntity = new List<vTestOrderSpecimen>();
            if (hdnTestOrderID.Value != "0")
            {
                string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", hdnVisitID.Value, hdnTestOrderID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvTestOrderSpecimenRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                lstEntity = BusinessLayer.GetvTestOrderSpecimenList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "SpecimenCode");
            }

            grdSpecimenView.DataSource = lstEntity;
            grdSpecimenView.DataBind();
        }
        protected void cbpSpecimenView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewSpecimen(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewSpecimen(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = string.Empty;
        }
        #endregion
    }
}