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
    public partial class SpecimenInfoCtl1 : BaseViewPopupCtl
    {
        protected int gridSpecimenPageCount = 1;

        protected static string _visitID = "0";
        protected static string _orderID = "0";

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnVisitID.Value = "0";
            OnControlEntrySettingLocal();
            hdnVisitID.Value = paramInfo[0];
            hdnTestOrderID.Value = paramInfo[1];
            _orderID = hdnTestOrderID.Value;
            _visitID = hdnVisitID.Value;
            TestOrderHd entityHd = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));
            if (entityHd != null)
            {	  
                DateTime specimenDate = Convert.ToDateTime(entityHd.SpecimenTakenDate);
                if (specimenDate.Year != 1900)
		            txtSampleDate.Text = specimenDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                if (entityHd.SpecimenTakenBy != null)
                {
                    cboParamedicID.Value = entityHd.SpecimenTakenBy.ToString();
                }
                txtSampleTime.Text = entityHd.SpecimenTakenTime;
                chkIsUsingExistingSample.Checked = entityHd.IsAdditionalOrder;
                txtRemarks.Text = entityHd.SpecimenRemarks;

                chkIsDelivery.Checked = entityHd.IsSpecimenDelivered;
                DateTime specimenDeliveryDate = Convert.ToDateTime(entityHd.SpecimenDeliveryDate);
                if (specimenDeliveryDate.Year != 1900)
                    txtDeliveryDate.Text = specimenDeliveryDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDeliveryTime.Text = entityHd.SpecimenDeliveryTime;
                if (entityHd.SpecimenDeliveryBy != null)
                {
                    cboParamedic2.Value = entityHd.SpecimenDeliveryBy.ToString();
                }
            }

            cboParamedicID.ClientEnabled = false;
            cboParamedic2.ClientEnabled = false;
            BindGridViewSpecimen(1, true, ref gridSpecimenPageCount);
        }

        private void SetControlProperties()
        {
            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType IN ('{0}','{1}')", Constant.ParamedicType.Nurse,Constant.ParamedicType.Bidan));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedic2, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.SelectedIndex = 0;
        }

        private void OnControlEntrySettingLocal()
        { 
            SetControlProperties();

            if (!string.IsNullOrEmpty(AppSession.UserLogin.GCParamedicMasterType) && AppSession.UserLogin.GCParamedicMasterType != Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboParamedicID.Value = userLoginParamedic.ToString();
                cboParamedicID.ClientEnabled = false;
            }

            if (!string.IsNullOrEmpty(AppSession.UserLogin.GCParamedicMasterType) && AppSession.UserLogin.GCParamedicMasterType != Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboParamedic2.Value = userLoginParamedic.ToString();
                cboParamedic2.ClientEnabled = false;
            }
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