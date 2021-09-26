using machineinfo.Data;
using machineinfo;
using System;
using Xunit;
using System.Threading.Tasks;
using machineinfo.Models;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace machineinfo.UnitTests
{
    public class UnitTest1
    {
        public IConfiguration Configuration { get; }
        [Fact]
        public async Task Test1()
        {
            using(db = new NpgsqlConnection(Configuration.GetConnectionString("postgres")))
            {
                var failureRepository = new FailureRepository(db);

                var failure = await failureRepository.GetFailureDetailsAsync(7);
                Assert.Equal(7, failure.FailureId);
            }
        }
    }
}
