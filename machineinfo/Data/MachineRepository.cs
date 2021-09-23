using machineinfo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using machineinfo.ViewModels;

namespace machineinfo.Data
{
    public class MachineRepository : IMachineRepository
    {
        private IDbConnection db;

        public MachineRepository(IDbConnection db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Machine>> GetMachinesAsync()
        {
            var query = "SELECT * FROM Machines";
            return await db.QueryAsync<Machine>(query);
        }

        public int Create(Machine machine)
        {
            var query = "INSERT INTO machines (MachineName) SELECT @MachineName WHERE NOT EXISTS (SELECT MachineName FROM Machines WHERE MachineName = @MachineName)";
            return db.Execute(query, new[]{new{MachineName = machine.MachineName}});
        }

        public async Task<IEnumerable<FailureVM>> GetMachineByIDAsync(int? id)
        {
            var q = "SELECT Failures.FailureId, Failures.Name, Failures.Priority, Failures.Description, Failures.Status, Failures.fileURLs, Failures.EntryTime, Machines.MachineName FROM Machines LEFT JOIN Failures ON Machines.MachineId = Failures.MachineId WHERE Machines.MachineID = " + @id;
            return await db.QueryAsync<FailureVM>(q);
        }

        public async Task<Machine> MachineToUpdateAsync(int? id)
        {
            var query = "SELECT * FROM machines WHERE MachineId = @id";
            return await db.QuerySingleOrDefaultAsync<Machine>(query, new{id});
        }

        public int Update(int? id, Machine machine)
        {
            var query = "UPDATE Machines SET MachineName = @MachineName WHERE MachineId = '" + @id + "' AND  NOT EXISTS (SELECT MachineName FROM Machines WHERE MachineName = @MachineName)";

            var param = new DynamicParameters();
            param.Add("MachineName", machine.MachineName);

            return db.Execute(query, param);
        }

        public void Delete(int? id)
        {
            var query = "DELETE FROM Machines WHERE MachineId = @id";
            db.Execute(query, new {id});
        }
    }
}