using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using HLS.Topup.Configuration;
using HLS.Topup.Web;

namespace HLS.Topup.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class TopupDbContextFactory : IDesignTimeDbContextFactory<TopupDbContext>
    {
        public TopupDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TopupDbContext>();
            var configuration = AppConfigurations.Get(
                WebContentDirectoryFinder.CalculateContentRootFolder(),
                addUserSecrets: true
            );

            TopupDbContextConfigurer.Configure(builder, configuration.GetConnectionString(TopupConsts.ConnectionStringName));

            return new TopupDbContext(builder.Options);
        }
    }
}