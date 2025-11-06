using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class VisitTransactionDetail : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                List<vPatientChargesDtProcedureCoding> ListPatientChargesDt = BusinessLayer.GetvPatientChargesDtProcedureCodingList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID.ToString()), int.MaxValue, 1, "ItemCode, ParamedicName, GCItemUnit");
                BindGrid(ListPatientChargesDt);
            }
        }

        private void BindGrid(List<vPatientChargesDtProcedureCoding> ListPatientChargesDt)
        {
            List<vPatientChargesDtProcedureCoding> lstService = ListPatientChargesDt;
            lvwCharges.DataSource = lstService;
            lvwCharges.DataBind();

            //List<vPatientChargesDt> lstService = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE).ToList();
            //lvwService.DataSource = lstService;
            //lvwService.DataBind();

            //List<vPatientChargesDt> lstDrugMS = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            //lvwDrugMS.DataSource = lstDrugMS;
            //lvwDrugMS.DataBind();

            //List<vPatientChargesDt> lstLogistic = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            //lvwLogistic.DataSource = lstLogistic;
            //lvwLogistic.DataBind();

            //List<vPatientChargesDt> lstServiceRad = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY).ToList();
            //lvwServiceRad.DataSource = lstServiceRad;
            //lvwServiceRad.DataBind();

            //List<vPatientChargesDt> lstServiceLab = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.LABORATORY).ToList();
            //lvwServiceLab.DataSource = lstServiceLab;
            //lvwServiceLab.DataBind();

            //List<vPatientChargesDt> lstServiceOtherDiagnose = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            //lvwServiceOtherDiagnose.DataSource = lstServiceOtherDiagnose;
            //lvwServiceOtherDiagnose.DataBind();
        }
    }
}