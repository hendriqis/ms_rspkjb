using System;
using System.Linq;
using System.Collections.Generic;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfoRegistrationLinkedFromCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnRegistrationIDCtl.Value = param;

            BindGridView();
        }

        protected void cbpViewInfoRegistrationLinkedFrom_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = (string.Format("LinkedToRegistrationID = {0}", hdnRegistrationIDCtl.Value));
            List<vRegistrationLinkedFrom2> lst = BusinessLayer.GetvRegistrationLinkedFrom2List(filterExpression);
            lvwView.DataSource = lst;
            lvwView.DataBind();

            if (lst.Count() > 0)
            {
                txtLinkedToRegistrationNo.Text = lst.FirstOrDefault().LinkedToRegistrationNo;
            }
            else
            {
                txtLinkedToRegistrationNo.Text = "";
            }
        }
    }
}