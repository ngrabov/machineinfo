using machineinfo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Http;
using Dapper;
using machineinfo.ViewModels;

namespace machineinfo.Data
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

        public void Create(Failure failure, List<IFormFile> files)
        {
            var query = "INSERT INTO Failures (Name, Description, Priority, Status, EntryTime, MachineId, fileURLs) VALUES (@Name, @Description, @Priority, @Status, current_timestamp, @MachineId, @fileURLs)";
            
            db.Execute(query, new[]{
                new{Name = failure.Name, Description = failure.Description, Priority = failure.Priority, 
                Status = failure.Status, EntryTime = System.DateTime.Now, MachineId = failure.MachineId, fileURLs = failure.fileURLs}
            });
        } 

        public async Task<FailureVM> GetFailureDetailsAsync(int? id)
        {
            var q = "SELECT Failures.FailureId, Failures.Name, Failures.Priority, Failures.Description, Failures.Status, Failures.fileURLs, Machines.MachineName FROM Failures JOIN Machines ON Machines.MachineId = Failures.MachineId WHERE Failures.FailureID = " + @id;
            return await db.QuerySingleOrDefaultAsync<FailureVM>(q);
        }

        public async Task<Failure> GetFailureByIDAsync(int? id)
        {
            var query = "SELECT * FROM failures WHERE FailureId = " + @id;
            return await db.QuerySingleOrDefaultAsync<Failure>(query);
        }

        public void Update(int? id, Failure failure)
        {
            var query = "UPDATE failures SET Name = @Name, Description = @Description, Priority = @Priority, Status = @Status, MachineId = @MachineId, fileURLs = @fileURLs WHERE FailureId = '" + @id + "'";

            var param = new DynamicParameters();
            param.Add("Name", failure.Name);
            param.Add("Description", failure.Description);
            param.Add("Priority", failure.Priority);
            param.Add("Status", failure.Status);
            param.Add("MachineId", failure.MachineId);
            param.Add("fileURLS", failure.fileURLs);

            //check the id
            db.Execute(query, param);
        }

        public void Resolve(int? id)
        {
            var query = "UPDATE failures SET Status = '1' WHERE FailureId = '" + @id + "'";
            db.Execute(query, new{id});
        }

        public void Delete(int? id)
        {
            var query = "DELETE FROM failures WHERE FailureId = @Id";
            db.Execute(query, new{id});
        }
    }
}