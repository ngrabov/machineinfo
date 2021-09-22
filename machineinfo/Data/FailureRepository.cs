using machineinfo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Dapper;

namespace machineinfo.Data
{
    public class FailureRepository : IFailureRepository
    {
        private IDbConnection db;

        public FailureRepository(IDbConnection db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Failure>> GetFailuresAsync()
        {
            var query = "SELECT * FROM failures";
            db.Open();
            var failure = await db.QueryAsync<Failure>(query);
            db.Dispose();
            return failure;
        }

        public void Create(Failure failure, List<IFormFile> files)
        {
            db.Open();
            var query = "INSERT INTO Failures (Name, Description, Priority, Status, EntryTime, MachineId, fileURLs) VALUES (@Name, @Description, @Priority, @Status, current_timestamp, @MachineId, @fileURLs)";
            
            db.Execute(query, new[]{
                new{Name = failure.Name, Description = failure.Description, Priority = failure.Priority, 
                Status = failure.Status, EntryTime = System.DateTime.Now, MachineId = failure.MachineId, fileURLs = failure.fileURLs}
            });

            db.Dispose();
        } 

    }
}