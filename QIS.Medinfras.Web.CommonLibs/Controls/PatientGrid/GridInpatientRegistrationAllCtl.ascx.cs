using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Drawing;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class GridInpatientRegistrationAllCtl : System.Web.UI.UserControl
    {
        protected int PageCount = 1;
        protected int ClassCount = 0;

        List<ClassCare> tempClassCare = null;

        public void InitializeControl()
        {
            tempClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0");
            ClassCount = tempClassCare.Count;

            rptClassCareHeader.DataSource = tempClassCare;
            rptClassCareHeader.DataBind();
            BindGridView(1, true, ref PageCount);
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                TempHospitalizedPatient obj = (TempHospitalizedPatient)e.Item.DataItem;
                Repeater rptClassCare = (Repeater)e.Item.FindControl("rptClassCare");

                List<DataTempJumlah> lstTariff = obj.JumlahPasien.Split('|').Select(t => new DataTempJumlah { Jumlah = t }).ToList();
                rptClassCare.DataSource = lstTariff;
                rptClassCare.DataBind();
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {

            string filterExpression = "1 = 1 ORDER BY ServiceUnitName ASC";
            //List<vHospitalizedPatient> lstEntity = BusinessLayer.GetvHospitalizedPatientList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, "ServiceUnitName ASC");
            List<vHospitalizedPatient> lstEntity = BusinessLayer.GetvHospitalizedPatientList(filterExpression);
            List<vHealthcareServiceUnit> tempHsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID = '{0}' AND IsDeleted = 0", Constant.Facility.INPATIENT));
            tempHsu = tempHsu.OrderBy(unit => unit.ServiceUnitName).ToList();
            List<TempHospitalizedPatient> lstTempHospitalizedPatient = new List<TempHospitalizedPatient>();
            int grandTotalPasien = 0;
            int countHSU = tempHsu.Count;
            foreach (vHealthcareServiceUnit entity in tempHsu)
            {
                TempHospitalizedPatient newEntity = new TempHospitalizedPatient();
                newEntity.ServiceUnitName = entity.ServiceUnitName;
                string tempJmlhPasien = string.Empty;
                int tempTotalPasien = 0;
                foreach (ClassCare entityClassCare in tempClassCare)
                {
                    vHospitalizedPatient entityFinal = lstEntity.Where(t => t.ServiceUnitID == entity.ServiceUnitID && t.ClassID == entityClassCare.ClassID).FirstOrDefault();
                    if (entityFinal != null)
                    {
                        tempJmlhPasien += entityFinal.Jumlah;
                        tempTotalPasien += entityFinal.Jumlah;
                    }
                    else
                    {
                        tempJmlhPasien += "0";
                        tempTotalPasien += 0;
                    }

                    tempJmlhPasien += "|";
                }
                tempJmlhPasien = tempJmlhPasien.Remove(tempJmlhPasien.Length - 1);
                newEntity.JumlahPasien = tempJmlhPasien;
                grandTotalPasien += tempTotalPasien;
                newEntity.TotalPasienServiceUnit = tempTotalPasien.ToString();
                lstTempHospitalizedPatient.Add(newEntity);
            }


            #region total
            string tempJmlhPasienSummary = string.Empty;
            foreach (ClassCare entityClassCare in tempClassCare)
            {
                List<vHospitalizedPatient> entityFinal = lstEntity.Where(t => t.ClassID == entityClassCare.ClassID).ToList();
                if (entityFinal != null)
                {
                    tempJmlhPasienSummary += entityFinal.Sum(t => t.Jumlah).ToString();
                }
                else
                {
                    tempJmlhPasienSummary += "0";
                    tempJmlhPasienSummary += 0;
                }

                tempJmlhPasienSummary += "|";
            }
            TempHospitalizedPatient summary = new TempHospitalizedPatient();
            summary.ServiceUnitName = "GRAND TOTAL";
            summary.JumlahPasien = tempJmlhPasienSummary.Remove(tempJmlhPasienSummary.Length - 1);
            summary.TotalPasienServiceUnit = lstEntity.Sum(t => t.Jumlah).ToString();
            lstTempHospitalizedPatient.Add(summary);
            #endregion

            //lstTempHospitalizedPatient.Add(new TempHospitalizedPatient { ServiceUnitName = "TOTAL", JumlahPasien = grandTotalPasien.ToString() });
            lvwView.DataSource = lstTempHospitalizedPatient;
            lvwView.DataBind();
        }

        protected string GetLabel(string code)
        {
            return ((BasePageRegisteredPatient)Page).GetLabel(code);
        }

    }
    public class TempHospitalizedPatient
    {
        private String _ServiceUnitName;
        private String _JumlahPasien;
        private String _TotalPasienServiceUnit;

        public String ServiceUnitName
        {
            get { return _ServiceUnitName; }
            set { _ServiceUnitName = value; }
        }

        public String JumlahPasien
        {
            get { return _JumlahPasien; }
            set { _JumlahPasien = value; }
        }

        public String TotalPasienServiceUnit
        {
            get { return _TotalPasienServiceUnit; }
            set { _TotalPasienServiceUnit = value; }
        }

    }

    public class DataTempJumlah
    {
        private string _Jumlah;

        public string Jumlah
        {
            get { return _Jumlah; }
            set { _Jumlah = value; }
        }
    }
}