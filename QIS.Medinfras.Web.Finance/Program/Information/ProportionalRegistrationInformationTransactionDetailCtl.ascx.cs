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
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ProportionalRegistrationInformationTransactionDetailCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            string[] paramSplit = param.Split('|');
            hdnRegistrationIDCtl.Value = paramSplit[0];
            txtRegistrationNo.Text = paramSplit[1];
            txtPatient.Text = string.Format("({0}) {1}", paramSplit[2], paramSplit[3]);

            List<StandardCode> lstStd = new List<StandardCode>();

            StandardCode sc = new StandardCode();
            sc.StandardCodeID = "0";
            sc.StandardCodeName = "Semua";
            lstStd.Add(sc);

            StandardCode sc1 = new StandardCode();
            sc1.StandardCodeID = "1";
            sc1.StandardCodeName = "Belum Proposional";
            lstStd.Add(sc1);

            StandardCode sc2 = new StandardCode();
            sc2.StandardCodeID = "2";
            sc2.StandardCodeName = "Sudah Proposional";
            lstStd.Add(sc2);

            Methods.SetComboBoxField<StandardCode>(cboStatus, lstStd, "StandardCodeName", "StandardCodeID");
            cboStatus.SelectedIndex = 0;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = String.Format("RegistrationID = {0}", hdnRegistrationIDCtl.Value);

            if (cboStatus.Value.ToString() == "1")
            {
                filterExpression += string.Format(" AND ((TariffComp1Final + TariffComp2Final + TariffComp3Final) <= 0)");
            }
            else if (cboStatus.Value.ToString() == "2")
            {
                filterExpression += string.Format(" AND ((TariffComp1Final + TariffComp2Final + TariffComp3Final) > 0)");
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesDtPropotionalInfoRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vPatientChargesDtPropotionalInfo> lstEntity = BusinessLayer.GetvPatientChargesDtPropotionalInfoList(filterExpression, 8, pageIndex, "TransactionID ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpViewPopUpCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            int pageCount = 1;
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }
            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}