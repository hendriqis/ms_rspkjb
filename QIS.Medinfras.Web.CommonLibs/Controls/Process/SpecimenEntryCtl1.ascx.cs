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
    public partial class SpecimenEntryCtl1 : BaseEntryPopupCtl3
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
                else
                    txtSampleDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                if (entityHd.SpecimenTakenBy != 0)
                {
                    hdnSpecimenTakenID.Value = entityHd.SpecimenTakenBy.ToString();
                    txtSpecimenTakenName.Text = entityHd.SpecimenTakenByName;
                }
                else
                {
                    hdnSpecimenTakenID.Value = AppSession.UserLogin.UserID.ToString();
                    txtSpecimenTakenName.Text = AppSession.UserLogin.UserFullName;
                }

                txtSpecimenTakenName.Enabled = false;

                chkIsUsingExistingSample.Checked = entityHd.IsAdditionalOrder;
                txtRemarks.Text = entityHd.SpecimenRemarks;

                BindGridViewDt();
            }

            BindGridViewSpecimen(1, true, ref gridSpecimenPageCount);
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0",
         Constant.StandardCode.JENIS_TABUNG_SAMPEL));

            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.JENIS_TABUNG_SAMPEL).ToList();

            Methods.SetComboBoxField<StandardCode>(cboContainerType, lstCode1, "StandardCodeName", "StandardCodeID");

            txtSpecimenTakenName.Text = AppSession.UserLogin.UserFullName;
        }

        private void OnControlEntrySettingLocal()
        {
            SetControlEntrySetting(txtSampleTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
  
            SetControlProperties();
        }

        private void ControlToEntity(TestOrderHd entityHd)
        {
            entityHd.IsAdditionalOrder = chkIsUsingExistingSample.Checked;
            entityHd.SpecimenTakenBy = Convert.ToInt32(hdnSpecimenTakenID.Value);
            entityHd.SpecimenTakenDate = Helper.GetDatePickerValue(txtSampleDate);
            entityHd.SpecimenTakenTime = txtSampleTime.Text;
            entityHd.SpecimenRemarks = txtRemarks.Text;
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
            string date = txtSampleDate.Text;
            if (string.IsNullOrEmpty(date))
            {
                errMsg.AppendLine("Tanggal Sampel harus diisi");
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
                    errMsg.AppendLine(string.Format("Format Tanggal Sampel {0} tidak benar atau invalid", date));
                }

                DateTime sdate = Helper.GetDatePickerValue(date);
                if (DateTime.Compare(sdate, DateTime.Now.Date) > 0)
                {
                    errMsg.AppendLine("Tanggal sampel harus lebih kecil atau sama dengan tanggal saat ini.");
                }
            }
            #endregion

            #region Order Time
            if (string.IsNullOrEmpty(txtSampleTime.Text))
            {
                errMsg.AppendLine("Jam sampel harus diisi");
            }
            else
            {
                if (!Methods.ValidateTimeFormat(txtSampleTime.Text))
                    errMsg.AppendLine("Format Jam Sampel tidak sesuai format (HH:MM)");
            }
            #endregion

            #region Pengambil Sampel
            if (string.IsNullOrEmpty(txtSpecimenTakenName.Text))
            {
                errMsg.AppendLine("Nama yang melakukan pengambilan sampel harus diisi");
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

        private bool IsValidateSpecimen(ref string errMessage)
        {
            StringBuilder message = new StringBuilder();

            if (string.IsNullOrEmpty(hdnEntrySpecimenID.Value))
            {
                message.AppendLine("Jenis Sampel harus diisi|");
            }

            if (string.IsNullOrEmpty(cboContainerType.Value.ToString()))
            {
                message.AppendLine("Jenis Tabung harus diisi|");
            }

            if (string.IsNullOrEmpty(txtSpecimenQty.Text))
            {
                message.AppendLine("Jumlah sampel harus diisi|");
            }
            else
            {
                if (!Methods.IsNumeric(txtSpecimenQty.Text))
                {
                    message.AppendLine("Jumlah sampel harus dalam angka|");
                }
                else
                {
                    if (Convert.ToDecimal(txtSpecimenQty.Text)<=0)
                    {
                        message.AppendLine("Jumlah sampel harus lebih besar dari 0|");
                    }
                }
            }

            errMessage = message.ToString().Replace("|","<br />");

            return string.IsNullOrEmpty(errMessage);
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
        protected void cbpSpecimen_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderSpecimenDao specimenDao = new TestOrderSpecimenDao(ctx);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    int orderID = Convert.ToInt32(hdnTestOrderID.Value);

                    if (param[0] == "add" || param[0] == "edit")
                    {
                        string errMessage = string.Empty;
                        if (!IsValidateSpecimen(ref errMessage))
                        {
                            result = string.Format("0|process|{0}", errMessage);
                            ctx.RollBackTransaction();
                        }
                        else
                        {
                            if (param[0] == "add")
                            {
                                TestOrderSpecimen obj = new TestOrderSpecimen();

                                obj.TestOrderID = orderID;
                                obj.VisitID = Convert.ToInt32(hdnVisitID.Value);
                                obj.SpecimenID = Convert.ToInt32(hdnEntrySpecimenID.Value);
                                obj.GCSpecimenContainerType = cboContainerType.Value.ToString();
                                obj.Quantity = Convert.ToDecimal(txtSpecimenQty.Text);
                                obj.CreatedBy = AppSession.UserLogin.UserID;
                                specimenDao.Insert(obj);

                                result = "1|add|" + _orderID;                                
                            }
                            else
                            {
                                int recordID = Convert.ToInt32(hdnOrderDtSpecimenID.Value);
                                TestOrderSpecimen entity = BusinessLayer.GetTestOrderSpecimen(recordID);

                                if (entity != null)
                                {
                                    entity.SpecimenID = Convert.ToInt32(hdnEntrySpecimenID.Value);
                                    entity.GCSpecimenContainerType = cboContainerType.Value.ToString();
                                    entity.Quantity = Convert.ToDecimal(txtSpecimenQty.Text);
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    specimenDao.Update(entity);
                                    result = "1|edit|" + _orderID;
                                }
                                else
                                {
                                    result = string.Format("0|delete|{0}", "Jenis Spesimen tidak valid");
                                }
                            }
                        }
                    }
                    else
                    {
                        int recordID = Convert.ToInt32(hdnOrderDtSpecimenID.Value);
                        TestOrderSpecimen entity = BusinessLayer.GetTestOrderSpecimen(recordID);

                        if (entity != null)
                        {
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            specimenDao.Update(entity);
                            result = "1|delete|" + _orderID;
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Jenis Spesimen tidak valid");
                        }
                    }
                    ctx.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Detail Item
        private void BindGridViewDt()
        {
            string filterExpression = "1 = 0";
            if (hdnTestOrderID.Value != "")
                filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0 ORDER BY ID DESC", hdnTestOrderID.Value);
            List<vTestOrderDt> lstEntity = BusinessLayer.GetvTestOrderDtList(filterExpression);
            grdPopupViewDt.DataSource = lstEntity;
            grdPopupViewDt.DataBind();
        }

        protected void cbpPopupDetailView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridViewDt();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        } 
        #endregion
    }
}