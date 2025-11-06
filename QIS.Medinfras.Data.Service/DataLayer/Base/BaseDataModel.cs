using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Data.Service
{
    public class BaseDataModel : DbDataModel
    {
        private String _CreatedBy;
        private DateTime _CreatedDate;
        private String _LastUpdatedBy;
        private DateTime _LastUpdatedDate;

        [Column(Name = "CreatedBy", IsNullable = true, DataType = "String")]
        public String CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }

        [Column(Name = "CreatedDate", IsNullable = true, DataType = "DateTime")]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }

        [Column(Name = "LastUpdatedBy", DataType = "String")]
        public String LastUpdatedBy
        {
            get { return _LastUpdatedBy; }
            set { _LastUpdatedBy = value; }
        }

        [Column(Name = "LastUpdatedDate", IsNullable = true, DataType = "DateTime")]
        public DateTime LastUpdatedDate
        {
            get { return _LastUpdatedDate; }
            set { _LastUpdatedDate = value; }
        }
    }
}
