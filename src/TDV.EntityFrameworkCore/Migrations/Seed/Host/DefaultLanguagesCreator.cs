using System.Collections.Generic;
using System.Linq;
using Abp.Localization;
using Microsoft.EntityFrameworkCore;
using TDV.EntityFrameworkCore;

namespace TDV.Migrations.Seed.Host
{
    public class DefaultLanguagesCreator
    {
        public static List<ApplicationLanguage> InitialLanguages => GetInitialLanguages();

        private readonly TDVDbContext _context;

        private static List<ApplicationLanguage> GetInitialLanguages()
        {
            var tenantId = TDVConsts.MultiTenancyEnabled ? null : (int?)1;
            return new List<ApplicationLanguage>
            {
                new ApplicationLanguage(tenantId, "en", "English", "famfamfam-flags us"),
                new ApplicationLanguage(tenantId, "tr", "Türkçe", "famfamfam-flags tr"),
            };
        }

        public DefaultLanguagesCreator(TDVDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateLanguages();
        }

        private void CreateLanguages()
        {
            foreach (var language in InitialLanguages)
            {
                AddLanguageIfNotExists(language);
            }
        }

        private void AddLanguageIfNotExists(ApplicationLanguage language)
        {
            if (_context.Languages.IgnoreQueryFilters().Any(l => l.TenantId == language.TenantId && l.Name == language.Name))
            {
                return;
            }

            _context.Languages.Add(language);

            _context.SaveChanges();
        }
    }
}
