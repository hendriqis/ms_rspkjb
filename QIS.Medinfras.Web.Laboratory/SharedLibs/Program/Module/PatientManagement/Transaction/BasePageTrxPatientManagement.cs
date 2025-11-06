using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QIS.Medinfras.Web.Common.UI;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public abstract class BasePageTrxPatientManagement : BasePageTrx
    {
        public abstract void UpdateTransactionHeaderTotal(IDbContext ctx);
        public abstract void SaveTransactionHeader(IDbContext ctx, ref int transactionID);
        public abstract Int32 GetClassID();
        public abstract Int32 GetPhysicianID();
        public abstract Int32 GetLocationID();
        public abstract Int32 GetHealthcareServiceUnitID();
        public abstract String GetTransactionHdID();
        public abstract void SetTransactionHdID(string val);
    }
}
