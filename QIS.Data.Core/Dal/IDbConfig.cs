using System.Data;

namespace QIS.Data.Core.Dal
{
    public interface IDbConfig
    {
        IDbConnection GetConnection();
        IDataAdapter GetDataAdapter(IDbCommand command);
        IDataParameter GetDataParameter();
        string GetParameterName(string name);
    }
}