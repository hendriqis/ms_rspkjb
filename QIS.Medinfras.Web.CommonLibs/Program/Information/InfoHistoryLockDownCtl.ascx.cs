using System;
using System.Collections.Generic;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfoHistoryLockDownCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnRegistrationNo.Value = AppSession.RegisteredPatient.RegistrationNo;

            BindGridView();
        }

        private void BindGridView()
        {
            List<tempHistoryLockDown> lstHistory = new List<tempHistoryLockDown>();
            List<tempHistoryLockDownDt> lstHistoryDetail = new List<tempHistoryLockDownDt>();

            string filterExpression = string.Format("ObjectType = '{0}' AND RegistrationID = {1}", Constant.AuditLog.RegistrationBillingDB, hdnRegistrationID.Value);
            List<vAuditLogRegistrationBillingStatusHistory> lstEntity = BusinessLayer.GetvAuditLogRegistrationBillingStatusHistoryList(filterExpression);
            foreach (vAuditLogRegistrationBillingStatusHistory entity in lstEntity)
            {
                tempHistoryLockDownDt oldDt = JsonConvert.DeserializeObject<tempHistoryLockDownDt>(entity.OldValues);
                tempHistoryLockDownDt newDt = JsonConvert.DeserializeObject<tempHistoryLockDownDt>(entity.NewValues);

                tempHistoryLockDown hist = new tempHistoryLockDown();

                hist.ID = entity.ID.ToString();
                hist.LogDate = entity.cfLogDateTimeInString;
                hist.UserFullName = entity.UserFullName;

                hist.OldIsLockDown = oldDt.IsLockDown == "1" ? "TUTUP" : "BUKA";
                hist.OldLockDownByName = oldDt.LockDownByName;
                hist.OldLockDownDate = oldDt.LockDownDate;
                hist.NewIsLockDown = newDt.IsLockDown == "1" ? "TUTUP" : "BUKA";
                hist.NewLockDownByName = newDt.LockDownByName;
                hist.NewLockDownDate = newDt.LockDownDate;

                if (hist.OldIsLockDown != hist.NewIsLockDown)
                {
                    lstHistory.Add(hist);
                }
            }

            lvwView.DataSource = lstHistory;
            lvwView.DataBind();
        }
    }

    public class tempHistoryLockDownDt
    {
        public string RegistrationID { get; set; }
        public string RegistrationNo { get; set; }
        public string IsLockDown { get; set; }
        public string LockDownBy { get; set; }
        public string LockDownByName { get; set; }
        public string LockDownDate { get; set; }
        public string IsBillingClosed { get; set; }
        public string BillingClosedBy { get; set; }
        public string BillingClosedByName { get; set; }
        public string BillingClosedDate { get; set; }
        public string IsBillingReopen { get; set; }
        public string BillingReopenBy { get; set; }
        public string BillingReopenByName { get; set; }
        public string BillingReopenDate { get; set; }
    }

    public class tempHistoryLockDown
    {
        public string ID { get; set; }
        public string LogDate { get; set; }
        public string UserFullName { get; set; }
        public string OldIsLockDown { get; set; }
        public string OldLockDownByName { get; set; }
        public string OldLockDownDate { get; set; }
        public string NewIsLockDown { get; set; }
        public string NewLockDownByName { get; set; }
        public string NewLockDownDate { get; set; }
    }

    public class SortList : IComparer<string>
    {
        public int Compare(string x, string y)
        {

            if (x == null || y == null)
            {
                return 0;
            }

            // "CompareTo()" method 
            return x.CompareTo(y);

        }
    }
}