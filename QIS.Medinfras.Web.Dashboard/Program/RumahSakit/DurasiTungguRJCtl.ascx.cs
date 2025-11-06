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
    public partial class DurasiTungguRJCtl : BaseViewPopupCtl
    {
        List<GetOutpatientWaitingTime> lstWaitData;
        public override void InitializeDataControl(string param)
        {
//          string filterExp = string.Format("TransactionDate = '{0}'", DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT2));
            lstWaitData = BusinessLayer.GetOutpatientWaitingTime(DateTime.Now.Year, DateTime.Now.Month);

            WaktuTungguPria();
            WaktuTungguPerempuan();
            WaktuTunggu();
            avgWaitDept();
            WaktuTungguUnknown();
        }

        private void WaktuTunggu()
        {
            var WaitTime = lstWaitData.Select(x => new { x.RegistrationDate, x.ObservationDate, x.RegistrationTime, x.ObservationTime }).ToList();
            TimeSpan time60m = TimeSpan.Parse("01:00:00");
            TimeSpan time90m = TimeSpan.Parse("01:30:00");
            TimeSpan time120m = TimeSpan.Parse("02:00:00");

            List<DataOutpatientWaitTime> lstWaitTime = new List<DataOutpatientWaitTime>();
            foreach (var e in WaitTime)
            {
                DataOutpatientWaitTime entity = new DataOutpatientWaitTime();
                entity.RegistrationDate = e.RegistrationDate.Date;
                entity.ObservationDate = e.ObservationDate.Date;


                DateTime regTime = DateTime.Parse(e.RegistrationTime);
                var regTimeOnly = regTime - regTime.Date;
                DateTime observeTime = DateTime.Parse(e.ObservationTime);
                var observeTimeOnly = observeTime - observeTime.Date;

                DateTime regDateTime = e.RegistrationDate.Add(regTimeOnly);
                DateTime obvDateTime = e.ObservationDate.Add(observeTimeOnly);

                entity.RegistrationTime = regTimeOnly;
                entity.ObservationTime = observeTimeOnly;
                entity.WaitTime = obvDateTime - regDateTime;

                lstWaitTime.Add(entity);
            }
            lblWaitUnder60m.InnerText = string.Format("{0}", lstWaitTime.Where(x => x.WaitTime < time60m).Count());
            lblWaitUnder90m.InnerText = string.Format("{0}", lstWaitTime.Where(x => x.WaitTime > time60m && x.WaitTime < time90m).Count());
            lblWaitUnder120m.InnerText = string.Format("{0}", lstWaitTime.Where(x => x.WaitTime > time90m && x.WaitTime < time120m).Count());
            lblWaitUnderXm.InnerText = string.Format("{0}", lstWaitTime.Where(x => x.WaitTime > time120m).Count());

            Int32[] listWait = new Int32[4];
            listWait[0] = lstWaitTime.Where(x => x.WaitTime < time60m).Count();
            listWait[1] = lstWaitTime.Where(x => x.WaitTime > time60m && x.WaitTime < time90m).Count();
            listWait[2] = lstWaitTime.Where(x => x.WaitTime > time90m && x.WaitTime < time120m).Count();
            listWait[3] = lstWaitTime.Where(x => x.WaitTime > time120m).Count();

            JsonChartWaitGlobal.Value = JsonConvert.SerializeObject(listWait, Formatting.Indented);
        }

        private void WaktuTungguPria()
        {
            var maleWaitTime = lstWaitData.Where(y => y.Gender == "Laki-Laki").Select(x => new { x.RegistrationDate, x.ObservationDate, x.RegistrationTime, x.ObservationTime }).ToList();
            TimeSpan time60m = TimeSpan.Parse("01:00:00");
            TimeSpan time90m = TimeSpan.Parse("01:30:00");
            TimeSpan time120m = TimeSpan.Parse("02:00:00");

            List<DataOutpatientWaitTime> lstWaitTime = new List<DataOutpatientWaitTime>();
            foreach (var e in maleWaitTime)
            {
                DataOutpatientWaitTime entity = new DataOutpatientWaitTime();
                entity.RegistrationDate = e.RegistrationDate.Date;
                entity.ObservationDate = e.ObservationDate.Date;


                DateTime regTime = DateTime.Parse(e.RegistrationTime);
                var regTimeOnly = regTime - regTime.Date;
                DateTime observeTime = DateTime.Parse(e.ObservationTime);
                var observeTimeOnly = observeTime - observeTime.Date;

                DateTime regDateTime = e.RegistrationDate.Add(regTimeOnly);
                DateTime obvDateTime = e.ObservationDate.Add(observeTimeOnly);

                entity.RegistrationTime = regTimeOnly;
                entity.ObservationTime = observeTimeOnly;
                entity.WaitTime = obvDateTime - regDateTime;

                lstWaitTime.Add(entity);
            }
            lblMWaitUnder60m.InnerText = string.Format("{0}", lstWaitTime.Where(x => x.WaitTime < time60m).Count());
            lblMWaitUnder90m.InnerText = string.Format("{0}", lstWaitTime.Where(x => x.WaitTime > time60m && x.WaitTime < time90m).Count());
            lblMWaitUnder120m.InnerText = string.Format("{0}", lstWaitTime.Where(x => x.WaitTime > time90m && x.WaitTime < time120m).Count());
            lblMWaitUnderXm.InnerText = string.Format("{0}", lstWaitTime.Where(x => x.WaitTime > time120m).Count());

            Int32[] listWaitM = new Int32[4];
            listWaitM[0] = lstWaitTime.Where(x => x.WaitTime < time60m).Count();
            listWaitM[1] = lstWaitTime.Where(x => x.WaitTime > time60m && x.WaitTime < time90m).Count();
            listWaitM[2] = lstWaitTime.Where(x => x.WaitTime > time90m && x.WaitTime < time120m).Count();
            listWaitM[3] = lstWaitTime.Where(x => x.WaitTime > time120m).Count();

            JsonChartWaitMale.Value = JsonConvert.SerializeObject(listWaitM, Formatting.Indented);
        }

        private void WaktuTungguPerempuan()
        {
            var femaleWaitTime = lstWaitData.Where(y => y.Gender == "Perempuan").Select(x => new { x.RegistrationDate, x.ObservationDate, x.RegistrationTime, x.ObservationTime }).ToList();
            TimeSpan time60m = TimeSpan.Parse("01:00:00");
            TimeSpan time90m = TimeSpan.Parse("01:30:00");
            TimeSpan time120m = TimeSpan.Parse("02:00:00");

            List<DataOutpatientWaitTime> lstWaitTime = new List<DataOutpatientWaitTime>();
            foreach (var e in femaleWaitTime)
            {
                DataOutpatientWaitTime entity = new DataOutpatientWaitTime();
                entity.RegistrationDate = e.RegistrationDate.Date;
                entity.ObservationDate = e.ObservationDate.Date;


                DateTime regTime = DateTime.Parse(e.RegistrationTime);
                var regTimeOnly = regTime - regTime.Date;
                DateTime observeTime = DateTime.Parse(e.ObservationTime);
                var observeTimeOnly = observeTime - observeTime.Date;

                DateTime regDateTime = e.RegistrationDate.Add(regTimeOnly);
                DateTime obvDateTime = e.ObservationDate.Add(observeTimeOnly);

                entity.RegistrationTime = regTimeOnly;
                entity.ObservationTime = observeTimeOnly;
                entity.WaitTime = obvDateTime - regDateTime;

                lstWaitTime.Add(entity);
            }
            lblFMWaitUnder60m.InnerText = string.Format("{0}", lstWaitTime.Where(x => x.WaitTime < time60m).Count());
            lblFMWaitUnder90m.InnerText = string.Format("{0}", lstWaitTime.Where(x => x.WaitTime > time60m && x.WaitTime < time90m).Count());
            lblFMWaitUnder120m.InnerText = string.Format("{0}", lstWaitTime.Where(x => x.WaitTime > time90m && x.WaitTime < time120m).Count());
            lblFMWaitUnderXm.InnerText = string.Format("{0}", lstWaitTime.Where(x => x.WaitTime > time120m).Count());

            Int32[] listWaitF = new Int32[4];
            listWaitF[0] = lstWaitTime.Where(x => x.WaitTime < time60m).Count();
            listWaitF[1] = lstWaitTime.Where(x => x.WaitTime > time60m && x.WaitTime < time90m).Count();
            listWaitF[2] = lstWaitTime.Where(x => x.WaitTime > time90m && x.WaitTime < time120m).Count();
            listWaitF[3] = lstWaitTime.Where(x => x.WaitTime > time120m).Count();

            JsonChartWaitFemale.Value = JsonConvert.SerializeObject(listWaitF, Formatting.Indented);
        }

        private void WaktuTungguUnknown()
        {
            var unknownWaitTime = lstWaitData.Where(y => y.Gender != "Perempuan" && y.Gender != "Laki-Laki").Select(x => new { x.RegistrationDate, x.ObservationDate, x.RegistrationTime, x.ObservationTime }).ToList();
            TimeSpan time60m = TimeSpan.Parse("01:00:00");
            TimeSpan time90m = TimeSpan.Parse("01:30:00");
            TimeSpan time120m = TimeSpan.Parse("02:00:00");

            List<DataOutpatientWaitTime> lstWaitTime = new List<DataOutpatientWaitTime>();
            foreach (var e in unknownWaitTime)
            {
                DataOutpatientWaitTime entity = new DataOutpatientWaitTime();
                entity.RegistrationDate = e.RegistrationDate.Date;
                entity.ObservationDate = e.ObservationDate.Date;


                DateTime regTime = DateTime.Parse(e.RegistrationTime);
                var regTimeOnly = regTime - regTime.Date;
                DateTime observeTime = DateTime.Parse(e.ObservationTime);
                var observeTimeOnly = observeTime - observeTime.Date;

                DateTime regDateTime = e.RegistrationDate.Add(regTimeOnly);
                DateTime obvDateTime = e.ObservationDate.Add(observeTimeOnly);

                entity.RegistrationTime = regTimeOnly;
                entity.ObservationTime = observeTimeOnly;
                entity.WaitTime = obvDateTime - regDateTime;

                lstWaitTime.Add(entity);
            }
            Int32[] listWaitu = new Int32[4];
            listWaitu[0] = lstWaitTime.Where(x => x.WaitTime < time60m).Count();
            listWaitu[1] = lstWaitTime.Where(x => x.WaitTime > time60m && x.WaitTime < time90m).Count();
            listWaitu[2] = lstWaitTime.Where(x => x.WaitTime > time90m && x.WaitTime < time120m).Count();
            listWaitu[3] = lstWaitTime.Where(x => x.WaitTime > time120m).Count();

            JsonChartWaitUnknown.Value = JsonConvert.SerializeObject(listWaitu, Formatting.Indented);
        }

        private void avgWaitDept()
        {
            var lstDept = lstWaitData.GroupBy(x => x.ServiceUnitName).Select(grp => grp.First()).Select(y => new { y.ServiceUnitName, y.RegistrationDate, y.RegistrationTime, y.ObservationDate, y.ObservationTime }).ToList();

        }

        public class DataOutpatientWaitTime
        {
            public TimeSpan RegistrationTime { get; set; }
            public TimeSpan ObservationTime { get; set; }
            public DateTime RegistrationDate { get; set; }
            public DateTime ObservationDate { get; set; }
            public TimeSpan WaitTime { get; set; }
        }

        public class DataAvgWaitDept
        {
            public String ServiceUnitName { get; set; }
            public TimeSpan avgTime { get; set; }
        }

        //protected void cbpView1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        //{
        //    string result = "";
        //    if (e.Parameter != null && e.Parameter != "")
        //    {
        //        string[] param = e.Parameter.Split('|');
        //        BindGridView();
        //        result = string.Format("refresh");
        //    }

        //    ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
        //    panel.JSProperties["cpResult"] = result;
        //}

        //protected void cboDepartment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        //{
        //    List<Department> lst = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration=1 and IsActive=1"));
        //    Methods.SetComboBoxField<Department>(cboDepartment, lst, "DepartmentName", "DepartmentID");
        //    cboDepartment.SelectedIndex = -1;
        //}
    }
}