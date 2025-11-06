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
    public partial class BloodBankOrderDtInfoEntryCtl1Old : BaseViewPopupCtl
    {
        protected int gridPageCount1 = 1;

        protected static string _visitID = "0";
        protected static string _orderID = "0";

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            this.PopupTitle = "Pengkodean Labu Darah";

            hdnVisitID.Value = "0";
            OnControlEntrySettingLocal();
            hdnVisitID.Value = paramInfo[0];
            hdnTestOrderID.Value = paramInfo[1];
            _orderID = hdnTestOrderID.Value;
            _visitID = hdnVisitID.Value;
            TestOrderHd entityHd = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));
            if (entityHd != null)
            {	  
            }

            BindGridView(1, true, ref gridPageCount1);
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
         Constant.StandardCode.KUALITAS_KANTONG_DARAH, Constant.StandardCode.KUALITAS_DARAH));

            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.KUALITAS_KANTONG_DARAH).ToList();
            List<StandardCode> lstCode2 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.KUALITAS_DARAH).ToList();


            Methods.SetComboBoxField<StandardCode>(cboPackingQuality, lstCode1, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBloodQuality, lstCode2, "StandardCodeName", "StandardCodeID");
        }

        private void OnControlEntrySettingLocal()
        { 
            SetControlProperties();
        }

        private void ControlToEntity(TestOrderHd entityHd)
        {
        }

        private bool IsValid(ref string errMessage)
        {
            bool result = true;
            StringBuilder errMsg = new StringBuilder();

            #region Sample Date
            //string date = txtSampleDate.Text;
            //if (string.IsNullOrEmpty(date))
            //{
            //    errMsg.AppendLine("Tanggal Sampel harus diisi");
            //}
            //else
            //{
            //    DateTime startDate;
            //    string format = Constant.FormatString.DATE_PICKER_FORMAT;
            //    try
            //    {
            //        startDate = DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
            //    }
            //    catch (FormatException)
            //    {
            //        errMsg.AppendLine(string.Format("Format Tanggal Sampel {0} tidak benar atau invalid", date));
            //    }

            //    DateTime sdate = Helper.GetDatePickerValue(date);
            //    if (DateTime.Compare(sdate, DateTime.Now.Date) > 0)
            //    {
            //        errMsg.AppendLine("Tanggal sampel harus lebih kecil atau sama dengan tanggal saat ini.");
            //    }
            //}
            #endregion

            #region Order Time
            //if (string.IsNullOrEmpty(txtSampleTime.Text))
            //{
            //    errMsg.AppendLine("Jam sampel harus diisi");
            //}
            //else
            //{
            //    if (!Methods.ValidateTimeFormat(txtSampleTime.Text))
            //        errMsg.AppendLine("Format Jam Sampel tidak sesuai format (HH:MM)");
            //}
            #endregion

            #region Perawat
            //if (cboParamedicID.Value != null)
            //{
            //    if (string.IsNullOrEmpty(cboParamedicID.Value.ToString()))
            //    {
            //        errMsg.AppendLine("Perawat yang melakukan pengambilan sampel harus diisi");
            //    }
            //}
            //else
            //{
            //    errMsg.AppendLine("Perawat yang melakukan pengambilan sampel harus diisi");
            //}
            #endregion

            errMessage = errMsg.ToString();

            result = string.IsNullOrEmpty(errMessage.ToString());

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

            if (string.IsNullOrEmpty(cboPackingQuality.Value.ToString()))
            {
                message.AppendLine("Jenis Tabung harus diisi|");
            }

            //if (string.IsNullOrEmpty(txtSpecimenQty.Text))
            //{
            //    message.AppendLine("Jumlah sampel harus diisi|");
            //}
            //else
            //{
            //    if (!Methods.IsNumeric(txtSpecimenQty.Text))
            //    {
            //        message.AppendLine("Jumlah sampel harus dalam angka|");
            //    }
            //    else
            //    {
            //        if (Convert.ToDecimal(txtSpecimenQty.Text)<=0)
            //        {
            //            message.AppendLine("Jumlah sampel harus lebih besar dari 0|");
            //        }
            //    }
            //}

            errMessage = message.ToString().Replace("|","<br />");

            return string.IsNullOrEmpty(errMessage);
        }

        #region Header
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", hdnVisitID.Value, hdnTestOrderID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderDtBloodBank1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderDtBloodBank1> lstEntity = BusinessLayer.GetvTestOrderDtBloodBank1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemCode");

            grdItemList.DataSource = lstEntity;
            grdItemList.DataBind();
        }

        protected void cbpItemList_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = string.Empty;
        }
        #endregion

        #region Detail
        private void BindGridViewDetail(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            List<vTestOrderDtBloodBag> lstEntity = new List<vTestOrderDtBloodBag>();
            if (hdnTestOrderID.Value != "0")
            {
                string filterExpression = string.Format("TestOrderDtID = {0} AND IsDeleted = 0", hdnID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvTestOrderDtBloodBagRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                lstEntity = BusinessLayer.GetvTestOrderDtBloodBagList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LabelNo");
            }

            grdDetailView.DataSource = lstEntity;
            grdDetailView.DataBind();
        }
        protected void cbpDetailView1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDetail(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDetail(1, true, ref pageCount);
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
                                obj.GCSpecimenContainerType = cboPackingQuality.Value.ToString();
                                //obj.Quantity = Convert.ToDecimal(txtSpecimenQty.Text);
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
                                    entity.GCSpecimenContainerType = cboPackingQuality.Value.ToString();
                                    //entity.Quantity = Convert.ToDecimal(txtSpecimenQty.Text);
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
                            result = "1|delete|";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Jenis Spesimen tidak valid");
                        }
                        result = "1|delete|";
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
    }
}