using machineinfo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Http;
using Dapper;
using machineinfo.ViewModels;

namespace machineinfo.Repositories
{
    public class FailureRepository : IFailureRepository
    {
        private IDbConnection db;

        public FailureRepository(IDbConnection db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<FailureVM>> GetFailuresAsync()
        {
            var query = "SELECT * FROM failures JOIN Machines ON Failures.MachineId = Machines.MachineId";
            return await db.QueryAsync<FailureVM>(query);
        }

        public async Task<int> Create(Failure failure, List<IFormFile> files)
        {
            var query = "INSERT INTO Failures (Name, Description, Priority, Status, EntryTime, MachineId, fileURLs) " +
             "SELECT @Name, @Description, @Priority, '0', current_timestamp, @MachineId, @fileURLs WHERE NOT EXISTS (SELECT " + 
             " FailureId FROM Failures WHERE Status = '0' AND MachineId = @MachineId)";
            
            return await db.ExecuteAsync(query, new[]{
                new{Name = failure.Name, Description = failure.Description, Priority = failure.Priority, 
                Status = failure.Status, EntryTime = System.DateTime.Now, MachineId = failure.MachineId, fileURLs = failure.fileURLs}
            });
        } 

        public async Task<FailureVM> GetFailureDetailsAsync(int? id)
        {
            var q = "SELECT Failures.FailureId, Failures.Name, Failures.Priority, Failures.EntryTime, Failures.ConclusionTime, Failures.Description, " +
            "Failures.Status, Failures.fileURLs, Machines.MachineName FROM Failures JOIN Machines ON Machines.MachineId = Failures.MachineId WHERE Failures.FailureID = " + @id;
            return await db.QuerySingleOrDefaultAsync<FailureVM>(q);
        }

        public async Task<Failure> GetFailureByIDAsync(int? id)
        {
            var query = "SELECT * FROM failures WHERE FailureId = " + @id;
            return await db.QuerySingleOrDefaultAsync<Failure>(query);
        }

        public async Task<int> Update(int? id, Failure failure)
        {
            string query = "UPDATE failures SET Name = @Name, Description = @Description, Priority = @Priority, MachineId = @MachineId, " +
                "fileURLs = @fileURLs WHERE FailureId = '" + @id + "' AND NOT EXISTS (SELECT " + 
                " FailureId FROM Failures WHERE Status = '0' AND MachineId = @MachineId)";;

            var param = new DynamicParameters();
            param.Add("Name", failure.Name);
            param.Add("Description", failure.Description);
            param.Add("Priority", failure.Priority);
            param.Add("MachineId", failure.MachineId);
            param.Add("fileURLS", failure.fileURLs);

            return await db.ExecuteAsync(query, param);
        }

        public async Task Resolve(int? id)
        {
            var query = "UPDATE failures SET Status = '1', ConclusionTime = '" + System.DateTime.Now + "' WHERE FailureId = '" + @id + "'";
            await db.ExecuteAsync(query, new{id});
        }

        public async Task Delete(int? id)
        {
            var query = "DELETE FROM failures WHERE FailureId = @Id";
            await db.ExecuteAsync(query, new{id});
        }

        public IEnumerable<Machine> GetMachines()
        {
            var query = "SELECT * FROM Machines";
            return db.Query<Machine>(query);
        }
    }
}