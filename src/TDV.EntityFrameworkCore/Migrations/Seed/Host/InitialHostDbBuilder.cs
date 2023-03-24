using TDV.EntityFrameworkCore;

namespace TDV.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly TDVDbContext _context;

        public InitialHostDbBuilder(TDVDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();


            //new DefaultLocationCreator(_context).Create();

            new DemoThingsCreator(_context).Create();
            _context.SaveChanges();
        }
    }
}
