using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientTransactionHistoryDtCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                List<vPatientChargesDt> ListPatientChargesDt = BusinessLayer.GetvPatientChargesDtList(string.Format("RegistrationID = {0} AND IsDeleted = 0",param));
                BindGrid(ListPatientChargesDt);
            }
        }

        private void BindGrid(List<vPatientChargesDt> ListPatientChargesDt)
        {
            List<vPatientChargesDt> lstService = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemType.PELAYANAN).ToList();
            lvwService.DataSource = lstService;
            lvwService.DataBind();

            List<vPatientChargesDt> lstDrugMS = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemType.OBAT_OBATAN || p.GCItemType == Constant.ItemType.BARANG_MEDIS).ToList();
            lvwDrugMS.DataSource = lstDrugMS;
            lvwDrugMS.DataBind();

            List<vPatientChargesDt> lstLogistic = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemType.BARANG_UMUM || p.GCItemType == Constant.ItemType.BAHAN_MAKANAN).ToList();
            lvwLogistic.DataSource = lstLogistic;
            lvwLogistic.DataBind();

            List<vPatientChargesDt> lstServiceRad = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemType.RADIOLOGI).ToList();
            lvwServiceRad.DataSource = lstServiceRad;
            lvwServiceRad.DataBind();

            List<vPatientChargesDt> lstServiceLab = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemType.LABORATORIUM).ToList();
            lvwServiceLab.DataSource = lstServiceLab;
            lvwServiceLab.DataBind();

            List<vPatientChargesDt> lstServiceOtherDiagnose = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemType.PENUNJANG_MEDIS).ToList();
            lvwServiceOtherDiagnose.DataSource = lstServiceOtherDiagnose;
            lvwServiceOtherDiagnose.DataBind();
        }
    }
}