using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Dashboard.Program
{
    public partial class BedInformation2Ctl : BaseViewPopupCtl
    {
        List<vBed> lstBed;
        public int bedCount;
        public override void InitializeDataControl(string param)
        {   
            string filterExpressionBed = string.Format("IsDeleted = 0");
            lstBed = BusinessLayer.GetvBedList(filterExpressionBed);
            var resultbed = lstBed.GroupBy(bed => bed.BedID).Select(grp => grp.First()).ToList().OrderBy(x => x.BedID);
            bedCount = resultbed.Count();
        }



        protected void cbpView1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                //BindGridView();
                result = string.Format("refresh");
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        //protected void cboDepartment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        //{
        //    List<Department> lst = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration=1 and IsActive=1"));
        //    Methods.SetComboBoxField<Department>(cboDepartment, lst, "DepartmentName", "DepartmentID");
        //    cboDepartment.SelectedIndex = -1;
        //}
    }
}