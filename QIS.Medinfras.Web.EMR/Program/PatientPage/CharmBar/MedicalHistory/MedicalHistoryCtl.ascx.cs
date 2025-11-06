using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class MedicalHistoryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            BindGridView(1, ref PageCount);
        }

        private void BindGridView(int pageIndex, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND VisitID != {1}", AppSession.RegisteredPatient.MRN, AppSession.RegisteredPatient.VisitID);

            int rowCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression);
            pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
        }
    }
}