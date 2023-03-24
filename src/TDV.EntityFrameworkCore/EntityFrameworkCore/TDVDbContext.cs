using TDV.Rapor;
using TDV.Kalite;
using TDV.Constants;
using TDV.Corporation;
using TDV.Authorization;
using TDV.Burial;
using TDV.Payment;
using TDV.Communication;
using TDV.Flight;
using TDV.Location;
using Abp.IdentityServer4vNext;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TDV.Authorization.Delegation;
using TDV.Authorization.Roles;
using TDV.Authorization.Users;
using TDV.Editions;
using TDV.MultiTenancy;
using TDV.MultiTenancy.Accounting;
using TDV.MultiTenancy.Payments;
using TDV.Storage;

namespace TDV.EntityFrameworkCore
{
    public class TDVDbContext : AbpZeroDbContext<Tenant, Role, User, TDVDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<Talep> Taleps { get; set; }

        public virtual DbSet<StokOlcu> StokOlcus { get; set; }

        public virtual DbSet<Stok> Stoks { get; set; }

        public virtual DbSet<Olcum> Olcums { get; set; }

        public virtual DbSet<CompanyTransaction> CompanyTransactions { get; set; }

        public virtual DbSet<Currency> Currencies { get; set; }

        public virtual DbSet<DataList> DataLists { get; set; }

        public virtual DbSet<CompanyContact> CompanyContacts { get; set; }

        public virtual DbSet<Vehicle> Vehicles { get; set; }

        public virtual DbSet<FuneralPackage> FuneralPackages { get; set; }

        public virtual DbSet<Company> Companies { get; set; }

        public virtual DbSet<UserDetail> UserDetails { get; set; }

        public virtual DbSet<FuneralTranportOrder> FuneralTranportOrders { get; set; }

        public virtual DbSet<FuneralAddres> FuneralAddreses { get; set; }

        public virtual DbSet<FuneralDocument> FuneralDocuments { get; set; }

        public virtual DbSet<FuneralFlight> FuneralFlights { get; set; }

        public virtual DbSet<Funeral> Funerals { get; set; }

        public virtual DbSet<FuneralType> FuneralTypes { get; set; }

        public virtual DbSet<FixedPrice> FixedPrices { get; set; }

        public virtual DbSet<FixedPriceDetail> FixedPriceDetails { get; set; }

        public virtual DbSet<ContractFormule> ContractFormules { get; set; }

        public virtual DbSet<Contract> Contracts { get; set; }

        public virtual DbSet<ContactNetsisDetail> ContactNetsisDetails { get; set; }

        public virtual DbSet<Contact> Contacts { get; set; }

        public virtual DbSet<ContactDetail> ContactDetails { get; set; }

        public virtual DbSet<AirlineCompany> AirlineCompanies { get; set; }

        public virtual DbSet<AirportRegion> AirportRegions { get; set; }

        public virtual DbSet<Airport> Airports { get; set; }

        public virtual DbSet<Quarter> Quarters { get; set; }

        public virtual DbSet<District> Districts { get; set; }

        public virtual DbSet<Region> Regions { get; set; }

        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<RecentPassword> RecentPasswords { get; set; }

        public TDVDbContext(DbContextOptions<TDVDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique()
                    .HasFilter("[IsDeleted] = 0");
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}