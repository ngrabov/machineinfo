using System.Data;
using System.Threading.Tasks;
using Dapper;
using System.Collections.Generic;
using machineinfo.ViewModels;

namespace machineinfo.Data
{
    public class HomeRepository : IHomeRepository
    {
        private IDbConnection db;

        public HomeRepository(IDbConnection db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<FailureVM>> GetFailuresByPriorityAsync()
        {
            var query = "SELECT * FROM Failures JOIN Machines ON Machines.MachineId = Failures.MachineId WHERE Status = '0' ORDER BY Priority DESC, EntryTime DESC";
            return await db.QueryAsync<FailureVM>(query);
        }
    }
}