using System.Collections.Generic;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class OutstandingOrderListCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            BindGridView(param);
        }

        private void BindGridView(string param)
        {
            string filterExpression = string.Empty;

            // ini ditutup karna dari perawat (bagian Transaksi Pasien) bingung, ada muncul logo tapi tidak muncul detailnya -> By:RN
            //
            //if (param.Equals("tr"))
            //{
            //    filterExpression = string.Format("VisitID = {0} AND GCTransactionStatus = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.TransactionStatus.OPEN);
            //}
            //else
            //{
            //    filterExpression = string.Format("VisitID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}','{3}')",
            //            AppSession.RegisteredPatient.VisitID, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            //}

            // VisitID di ubah menjadi RegistrationID karena diharuskan menampilkan semua outstanding order meskipun pasien itu multi visit : By MC (1 Maret 2019)
            //filterExpression = string.Format("VisitID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}','{3}')",
            //        AppSession.RegisteredPatient.VisitID, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);

            filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}','{3}')",
                    AppSession.RegisteredPatient.RegistrationID, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            
            List<vRegistrationOutstandingDetail> lstEntity = BusinessLayer.GetvRegistrationOutstandingDetailList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

    }
}