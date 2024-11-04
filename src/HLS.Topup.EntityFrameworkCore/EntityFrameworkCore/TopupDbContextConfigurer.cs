using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace HLS.Topup.EntityFrameworkCore
{
    public static class TopupDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<TopupDbContext> builder, string connectionString)
        {
            builder.UseNpgsql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<TopupDbContext> builder, DbConnection connection)
        {
            builder.UseNpgsql(connection);
        }
    }
}