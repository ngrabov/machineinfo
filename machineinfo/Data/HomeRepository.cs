using System.Data;
using System.Threading.Tasks;
using Dapper;
using machineinfo.Models;
using System.Collections.Generic;

namespace machineinfo.Data
{
    public class HomeRepository : IHomeRepository
    {
        private IDbConnection db;

        public HomeRepository(IDbConnection db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Failure>> GetFailuresByPriorityAsync()
        {
            var query = "SELECT * FROM Failures WHERE Status = '0' ORDER BY Priority, EntryTime";
            var failures = await db.QueryAsync<Failure>(query);
            return failures;
        }
    }
}