using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PickupAnnouncerLegacy.Interfaces
{
    public interface IDbService
    {
        Task<IEnumerable<T>> Get<T>(string whereConditions = null, object parameters = null);
        Task Insert<T>(IEnumerable<T> itemsToInsert);
        Task<int?> Insert<T>(T itemToInsert);
        Task<int> Update<T>(T itemToUpdate);
        Task<int> Delete<T>(int id);
        Task<IList<IDictionary<string, object>>> ExecuteQuery(string queryText, IDictionary<string, object> parameters);
        Task<IList<IDictionary<string, object>>> ExecuteStoredProcedure(string sprocName, IDictionary<string, object> parameters = null);
        SqlConnection GetSqlConnection();
    }
}

