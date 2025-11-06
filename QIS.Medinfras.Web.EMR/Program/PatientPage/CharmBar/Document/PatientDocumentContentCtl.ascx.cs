using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientDocumentContentCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            string filterExpression = "";
            filterExpression = string.Format("ID = {0} AND IsDeleted = 0", queryString);
            vPatientDocument entity = BusinessLayer.GetvPatientDocumentList(filterExpression).FirstOrDefault();
            EntityToControl(entity);
        }

        private void EntityToControl(vPatientDocument entity)
        {
            //spnParamedicName.InnerHtml = entity.ParamedicName;
            //spnObservationDateTime.InnerHtml = entity.DisplayObservationDateTime;
            spnDocumentName.InnerHtml = entity.DocumentName;
            spnDocumentType.InnerHtml = entity.DocumentType;
            spnDocumentDate.InnerHtml = entity.cfDocumentDate;
            txtRemarks.InnerHtml = entity.Notes;

            imgDocument.Src = entity.FileImageUrl;
            hdnDocumentID.Value = entity.ID.ToString();
        }
    }
}