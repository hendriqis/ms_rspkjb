using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Finance.Program;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class InformasiDaftarTransaksiPembayaranDetailCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        private InformasiDaftarTransaksiPembayaran DetailPage
        {
            get { return (InformasiDaftarTransaksiPembayaran)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnRegistrationID.Value = param;

            vInformasiTransaksiVSPembayaran entity = BusinessLayer.GetvInformasiTransaksiVSPembayaranList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];

            txtRegistrationNo.Text = entity.RegistrationNo;
            txtPatientName.Text = entity.PatientName;
            txtParamedicName.Text = entity.ParamedicName;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvInformasiTransaksiVSPembayaranRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vInformasiTransaksiVSPembayaran> lstEntity = BusinessLayer.GetvInformasiTransaksiVSPembayaranList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "RegistrationID");
            grdPopupView.DataSource = lstEntity;
            grdPopupView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
        }
    }
}