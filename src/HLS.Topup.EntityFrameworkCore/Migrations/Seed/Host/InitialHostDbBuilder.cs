using HLS.Topup.EntityFrameworkCore;

namespace HLS.Topup.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly TopupDbContext _context;

        public InitialHostDbBuilder(TopupDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
