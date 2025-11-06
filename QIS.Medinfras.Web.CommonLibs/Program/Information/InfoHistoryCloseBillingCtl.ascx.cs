using System;
using System.Collections.Generic;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfoHistoryCloseBillingCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnRegistrationID.Value = param;

            Registration entityReg = BusinessLayer.GetRegistration(Convert.ToInt32(param));
            hdnRegistrationNo.Value = entityReg.RegistrationNo;

            BindGridView();
        }

        private void BindGridView()
        {
            List<tempHistoryBillingClosed> lstHistory = new List<tempHistoryBillingClosed>();
            List<tempHistoryBillingClosedDt> lstHistoryDetail = new List<tempHistoryBillingClosedDt>();

            string filterExpression = string.Format("ObjectType = '{0}' AND RegistrationID = {1}", Constant.AuditLog.RegistrationBillingDB, hdnRegistrationID.Value);
            List<vAuditLogRegistrationBillingStatusHistory> lstEntity = BusinessLayer.GetvAuditLogRegistrationBillingStatusHistoryList(filterExpression);
            foreach (vAuditLogRegistrationBillingStatusHistory entity in lstEntity)
            {
                tempHistoryBillingClosedDt oldDt = JsonConvert.DeserializeObject<tempHistoryBillingClosedDt>(entity.OldValues);
                tempHistoryBillingClosedDt newDt = JsonConvert.DeserializeObject<tempHistoryBillingClosedDt>(entity.NewValues);

                tempHistoryBillingClosed hist = new tempHistoryBillingClosed();

                hist.ID = entity.ID.ToString();
                hist.LogDate = entity.cfLogDateTimeInString;
                hist.UserFullName = entity.UserFullName;

                hist.OldIsBillingClosed = oldDt.IsBillingClosed == "1" ? "V" : "X";
                hist.OldBillingClosedByName = oldDt.BillingClosedByName;
                hist.OldBillingClosedDate = oldDt.BillingClosedDate;
                hist.OldIsBillingReopen = oldDt.IsBillingReopen == "1" ? "V" : "X";
                hist.OldBillingReopenByName = oldDt.BillingReopenByName;
                hist.OldBillingReopenDate = oldDt.BillingReopenDate;

                hist.NewIsBillingClosed = newDt.IsBillingClosed == "1" ? "V" : "X";
                hist.NewBillingClosedByName = newDt.BillingClosedByName;
                hist.NewBillingClosedDate = newDt.BillingClosedDate;
                hist.NewIsBillingReopen = newDt.IsBillingReopen == "1" ? "V" : "X";
                hist.NewBillingReopenByName = newDt.BillingReopenByName;
                hist.NewBillingReopenDate = newDt.BillingReopenDate;

                if (hist.OldIsBillingClosed != hist.NewIsBillingClosed)
                {
                    lstHistory.Add(hist);
                }
                else if (hist.OldIsBillingReopen != hist.NewIsBillingReopen)
                {
                    lstHistory.Add(hist);
                }
            }

            lvwView.DataSource = lstHistory;
            lvwView.DataBind();
        }
    }

    public class tempHistoryBillingClosedDt
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

    public class tempHistoryBillingClosed
    {
        public string ID { get; set; }
        public string LogDate { get; set; }
        public string UserFullName { get; set; }
        public string OldIsBillingClosed { get; set; }
        public string OldBillingClosedByName { get; set; }
        public string OldBillingClosedDate { get; set; }
        public string OldIsBillingReopen { get; set; }
        public string OldBillingReopenByName { get; set; }
        public string OldBillingReopenDate { get; set; }
        public string NewIsBillingClosed { get; set; }
        public string NewBillingClosedByName { get; set; }
        public string NewBillingClosedDate { get; set; }
        public string NewIsBillingReopen { get; set; }
        public string NewBillingReopenByName { get; set; }
        public string NewBillingReopenDate { get; set; }
    }
}