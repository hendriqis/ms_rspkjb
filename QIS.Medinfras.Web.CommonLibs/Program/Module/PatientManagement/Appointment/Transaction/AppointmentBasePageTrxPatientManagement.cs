using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QIS.Medinfras.Web.Common.UI;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public abstract class AppointmentBasePageTrxPatientManagement : BasePageTrx
    {
        public abstract void SaveTransactionHeader(IDbContext ctx, ref int transactionID);
        public abstract Int32 GetAppointmentID();
        public abstract Int32 GetClassID();
        public abstract Int32 GetAppointmentPhysicianID();
        public abstract Int32 GetLocationID();
        public abstract Int32 GetLogisticLocationID();
        public abstract String GetDepartmentID();
        public abstract Int32 GetHealthcareServiceUnitID();
        public abstract String GetTransactionHdID();
        public abstract String GetTransactionDate();
        public abstract String GetTransactionTime();
        public abstract String GetAppointmentStatus();
        public abstract String GetGCCustomerType();
        public abstract void SetTransactionHdID(string val);
        //public virtual Boolean IsPatientBillSummaryPage()
        //{
        //    return false;
        //}
    }
}
