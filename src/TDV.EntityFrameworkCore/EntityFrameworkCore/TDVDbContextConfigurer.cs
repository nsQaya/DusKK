using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace TDV.EntityFrameworkCore
{
    public static class TDVDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<TDVDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<TDVDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}