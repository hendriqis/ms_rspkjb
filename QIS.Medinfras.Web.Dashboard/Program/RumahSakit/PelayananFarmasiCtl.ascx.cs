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
    public partial class PelayananFarmasiCtl : BaseViewPopupCtl
    {
        List<GetPrescriptionOrderHdInfoDisplay> lstData;
        List<GetOutpatientWaitingTime> lstWaitData;
        public override void InitializeDataControl(string param)
        {            
            lstData = BusinessLayer.GetPrescriptionOrderHdInfoDisplay(DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT2));
            lstWaitData = BusinessLayer.GetOutpatientWaitingTime(DateTime.Now.Year, DateTime.Now.Month);

            RawatJalanOrder();
            FarmasiOrder();
            OrderRacikProgress();
            OrderNonRacikProgress();
            OrderRacikComplete();
            OrderNonRacikComplete();
            OrderRacikGiven();
            OrderNonRacikGiven();
            StatusOrder();
            WaktuTunggu();
        }

        private void RawatJalanOrder()
        {
            //List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            var resultOrderRJ = (from lp in lstData
                                 where lp.DepartmentID != "PHARMACY"
                                 select lp).ToList();

            if (resultOrderRJ.Count() < 1)
            {
                string resultOrderEmpty = "0";
                lblOrderFCount.InnerText = string.Format("{0}", resultOrderEmpty);
            }
            else
            {
                lblOrderRJCount.InnerText = string.Format("{0}", resultOrderRJ.Count());
            }
        }

        private void FarmasiOrder()
        {
            //List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            var resultOrderF = (from lp in lstData
                                where lp.DepartmentID == "PHARMACY"
                                select lp).ToList();

            if (resultOrderF.Count() < 1)
            {
                string resultOrderEmpty = "0";
                lblOrderFCount.InnerText = string.Format("{0}", resultOrderEmpty);
            }
            else
            {
                lblOrderFCount.InnerText = string.Format("{0}", resultOrderF.Count());
            }
        }

        private void OrderRacikProgress()
        {
            //List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            //cek progress
            var resultOrderRacikProgress = (from lp in lstData
                                            where lp.PrescriptionStartedBy == 1 && lp.PrescriptionCompletedBy != 1 && lp.PrescriptionClosedBy != 1 && lp.IsHasCompound == true
                                            select lp).ToList();

            if (resultOrderRacikProgress.Count() < 1)
            {
                string resultOrderEmpty = "0";
                lblOrderRacikProgressCount.InnerText = string.Format("{0}", resultOrderEmpty);
            }
            else
            {
                lblOrderRacikProgressCount.InnerText = string.Format("{0}", resultOrderRacikProgress.Count());
            }
        }

        private void OrderNonRacikProgress()
        {
            //List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            //cek progress
            var resultOrderNonRacikProgress = (from lp in lstData
                                               where lp.PrescriptionStartedBy == 1 && lp.PrescriptionCompletedBy != 1 && lp.PrescriptionClosedBy != 1 && lp.IsHasCompound == false
                                               select lp).ToList();

            if (resultOrderNonRacikProgress.Count() < 1)
            {
                string resultOrderEmpty = "0";
                lblOrderNonRacikProgressCount.InnerText = string.Format("{0}", resultOrderEmpty);
            }
            else
            {
                lblOrderNonRacikProgressCount.InnerText = string.Format("{0}", resultOrderNonRacikProgress.Count());
            }
        }

        private void OrderRacikComplete()
        {
            //List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            //cek progress
            var resultOrderRacikComplete = (from lp in lstData
                                            where lp.PrescriptionStartedBy == 1 && lp.PrescriptionCompletedBy == 1 && lp.PrescriptionClosedBy != 1 && lp.IsHasCompound == true
                                            select lp).ToList();

            if (resultOrderRacikComplete.Count() < 1)
            {
                string resultOrderEmpty = "0";
                lblOrderRacikCompleteCount.InnerText = string.Format("{0}", resultOrderEmpty);
            }
            else
            {
                lblOrderRacikCompleteCount.InnerText = string.Format("{0}", resultOrderRacikComplete.Count());
            }
        }

        private void OrderNonRacikComplete()
        {
            //List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            //cek progress
            var resultOrderNonRacikComplete = (from lp in lstData
                                               where lp.PrescriptionStartedBy == 1 && lp.PrescriptionCompletedBy == 1 && lp.PrescriptionClosedBy != 1 && lp.IsHasCompound == false
                                               select lp).ToList();

            if (resultOrderNonRacikComplete.Count() < 1)
            {
                string resultOrderEmpty = "0";
                lblOrderNonRacikCompleteCount.InnerText = string.Format("{0}", resultOrderEmpty);
            }
            else
            {
                lblOrderNonRacikCompleteCount.InnerText = string.Format("{0}", resultOrderNonRacikComplete.Count());
            }
        }

        private void OrderRacikGiven()
        {
            //List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            //cek progress
            var resultOrderRacikGiven = (from lp in lstData
                                         where lp.PrescriptionStartedBy == 1 && lp.PrescriptionCompletedBy == 1 && lp.PrescriptionClosedBy == 1 && lp.IsHasCompound == true
                                         select lp).ToList();

            if (resultOrderRacikGiven.Count() < 1)
            {
                string resultOrderEmpty = "0";
                lblOrderRacikGivenCount.InnerText = string.Format("{0}", resultOrderEmpty);
            }
            else
            {
                lblOrderRacikGivenCount.InnerText = string.Format("{0}", resultOrderRacikGiven.Count());
            }
        }

        private void OrderNonRacikGiven()
        {
            //List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            //cek progress
            var resultOrderNonRacikGiven = (from lp in lstData
                                            where lp.PrescriptionStartedBy == 1 && lp.PrescriptionCompletedBy == 1 && lp.PrescriptionClosedBy == 1 && lp.IsHasCompound == false
                                            select lp).ToList();

            if (resultOrderNonRacikGiven.Count() < 1)
            {
                string resultOrderEmpty = "0";
                lblOrderNonRacikGivenCount.InnerText = string.Format("{0}", resultOrderEmpty);
            }
            else
            {
                lblOrderNonRacikGivenCount.InnerText = string.Format("{0}", resultOrderNonRacikGiven.Count());
            }
        }

        private void StatusOrder()
        {
            //List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            //cek progress
            List<GetPrescriptionOrderHdInfoDisplay> result = (from res in lstData
                                                              where res.PrescriptionClosedBy != 1 && res.PrescriptionCompletedBy == 1
                                                              orderby res.PrescriptionCompletedDateTime
                                                              select res).Take(5).ToList();
            int cnt = 0;
            List<DataStatusOrder> lstStatus = new List<DataStatusOrder>();
            foreach (GetPrescriptionOrderHdInfoDisplay e in result)
            {
                DataStatusOrder entity = new DataStatusOrder();
                entity.PatientName = e.PatientName;
                entity.TransactionNo = e.TransactionNo;
                entity.Status = "SIAP DIAMBIL";
                lstStatus.Add(entity);
            }
            if (lstStatus.Count() == 1)
            {
                lblTransactionNo1.InnerText = string.Format("{0}", lstStatus[0].TransactionNo);
                lblNamaPasien1.InnerText = string.Format("{0}", lstStatus[0].PatientName);
                lblStatus1.InnerText = string.Format("{0}", lstStatus[0].Status);
            }
            else if (lstStatus.Count() == 2)
            {
                lblTransactionNo1.InnerText = string.Format("{0}", lstStatus[0].TransactionNo);
                lblNamaPasien1.InnerText = string.Format("{0}", lstStatus[0].PatientName);
                lblStatus1.InnerText = string.Format("{0}", lstStatus[0].Status);
                lblTransactionNo2.InnerText = string.Format("{0}", lstStatus[1].TransactionNo);
                lblNamaPasien2.InnerText = string.Format("{0}", lstStatus[1].PatientName);
                lblStatus2.InnerText = string.Format("{0}", lstStatus[1].Status);
            }
            else if (lstStatus.Count() == 3)
            {
                lblTransactionNo1.InnerText = string.Format("{0}", lstStatus[0].TransactionNo);
                lblNamaPasien1.InnerText = string.Format("{0}", lstStatus[0].PatientName);
                lblStatus1.InnerText = string.Format("{0}", lstStatus[0].Status);
                lblTransactionNo2.InnerText = string.Format("{0}", lstStatus[1].TransactionNo);
                lblNamaPasien2.InnerText = string.Format("{0}", lstStatus[1].PatientName);
                lblStatus2.InnerText = string.Format("{0}", lstStatus[1].Status);
                lblTransactionNo3.InnerText = string.Format("{0}", lstStatus[2].TransactionNo);
                lblNamaPasien3.InnerText = string.Format("{0}", lstStatus[2].PatientName);
                lblStatus3.InnerText = string.Format("{0}", lstStatus[2].Status);
            }
            else if (lstStatus.Count() == 4)
            {
                lblTransactionNo1.InnerText = string.Format("{0}", lstStatus[0].TransactionNo);
                lblNamaPasien1.InnerText = string.Format("{0}", lstStatus[0].PatientName);
                lblStatus1.InnerText = string.Format("{0}", lstStatus[0].Status);
                lblTransactionNo2.InnerText = string.Format("{0}", lstStatus[1].TransactionNo);
                lblNamaPasien2.InnerText = string.Format("{0}", lstStatus[1].PatientName);
                lblStatus2.InnerText = string.Format("{0}", lstStatus[1].Status);
                lblTransactionNo3.InnerText = string.Format("{0}", lstStatus[2].TransactionNo);
                lblNamaPasien3.InnerText = string.Format("{0}", lstStatus[2].PatientName);
                lblStatus3.InnerText = string.Format("{0}", lstStatus[2].Status);
                lblTransactionNo4.InnerText = string.Format("{0}", lstStatus.ElementAt(3).TransactionNo);
                lblNamaPasien4.InnerText = string.Format("{0}", lstStatus[3].PatientName);
                lblStatus4.InnerText = string.Format("{0}", lstStatus[3].Status);
            }
            else if (lstStatus.Count() == 5)
            {
                lblTransactionNo1.InnerText = string.Format("{0}", lstStatus[0].TransactionNo);
                lblNamaPasien1.InnerText = string.Format("{0}", lstStatus[0].PatientName);
                lblStatus1.InnerText = string.Format("{0}", lstStatus[0].Status);
                lblTransactionNo2.InnerText = string.Format("{0}", lstStatus[1].TransactionNo);
                lblNamaPasien2.InnerText = string.Format("{0}", lstStatus[1].PatientName);
                lblStatus2.InnerText = string.Format("{0}", lstStatus[1].Status);
                lblTransactionNo3.InnerText = string.Format("{0}", lstStatus[2].TransactionNo);
                lblNamaPasien3.InnerText = string.Format("{0}", lstStatus[2].PatientName);
                lblStatus3.InnerText = string.Format("{0}", lstStatus[2].Status);
                lblTransactionNo4.InnerText = string.Format("{0}", lstStatus.ElementAt(3).TransactionNo);
                lblNamaPasien4.InnerText = string.Format("{0}", lstStatus[3].PatientName);
                lblStatus4.InnerText = string.Format("{0}", lstStatus[3].Status);
                lblTransactionNo5.InnerText = string.Format("{0}", lstStatus.ElementAt(4).TransactionNo);
                lblNamaPasien5.InnerText = string.Format("{0}", lstStatus[4].PatientName);
                lblStatus5.InnerText = string.Format("{0}", lstStatus[4].Status);
            }
        }

        private void WaktuTunggu()
        {
            var lstRegTime = lstWaitData.Select(x => new { x.RegistrationDate, x.ObservationDate, x.RegistrationTime, x.ObservationTime }).ToList();

            List<DataOutpatientWaitTime> lstWaitTime = new List<DataOutpatientWaitTime>();
            foreach (var e in lstRegTime)
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
            TimeSpan time60m = TimeSpan.Parse("01:00:00");
            TimeSpan time90m = TimeSpan.Parse("01:30:00");
            TimeSpan time120m = TimeSpan.Parse("02:00:00");

            int category60m = lstWaitTime.Where(x => x.WaitTime < time60m).Count();
            lblWaitUnder60m.InnerText = string.Format("{0}", lstWaitTime.Where(x => x.WaitTime < time60m).Count());
            lblWaitUnder90m.InnerText = string.Format("{0}", lstWaitTime.Where(x => x.WaitTime > time60m && x.WaitTime < time90m).Count());
            lblWaitUnder120m.InnerText = string.Format("{0}", lstWaitTime.Where(x => x.WaitTime > time90m && x.WaitTime < time120m).Count());
            lblWaitUnderXm.InnerText = string.Format("{0}", lstWaitTime.Where(x => x.WaitTime > time120m).Count());
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
        public class DataOrderRJ
        {
            public String DepartmentID { get; set; }
            public Int32 Jumlah { get; set; }
        }

        public class DataOrderProgress
        {
            public Int32 PrescriptionStartedBy { get; set; }
            public Int32 PrescriptionCompletedBy { get; set; }
            public Int32 PrescriptionClosedBy { get; set; }
            public Int32 Jumlah { get; set; }
        }

        public class DataStatusOrder
        {
            public String PatientName { get; set; }
            public String TransactionNo { get; set; }
            public String Status { get; set; }
        }

        public class DataOutpatientWaitTime
        {
            public TimeSpan RegistrationTime { get; set; }
            public TimeSpan ObservationTime { get; set; }
            public DateTime RegistrationDate { get; set; }
            public DateTime ObservationDate { get; set; }
            public TimeSpan WaitTime { get; set; }
        }
    }
}