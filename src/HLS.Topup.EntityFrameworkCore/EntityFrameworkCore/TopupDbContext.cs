using HLS.Topup.Notifications;
using HLS.Topup.BalanceManager;
using HLS.Topup.LimitationManager;
using HLS.Topup.FeeManager;
using HLS.Topup.Sale;
using HLS.Topup.Vendors;
using HLS.Topup.Address;
using HLS.Topup.Configuration;
using HLS.Topup.StockManagement;
using HLS.Topup.DiscountManager;
using HLS.Topup.Deposits;
using HLS.Topup.Banks;
using HLS.Topup.Providers;
using HLS.Topup.Products;
using HLS.Topup.Categories;
using HLS.Topup.Services;
using Abp.IdentityServer4;
using Abp.Organizations;
using Abp.Zero.EntityFrameworkCore;
using HLS.Topup.AgentsManager;
using HLS.Topup.Authorization;
using Microsoft.EntityFrameworkCore;
using HLS.Topup.Authorization.Delegation;
using HLS.Topup.Authorization.Organization;
using HLS.Topup.Authorization.Roles;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Chat;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Authentication;
using HLS.Topup.Editions;
using HLS.Topup.Friendships;
using HLS.Topup.MultiTenancy;
using HLS.Topup.MultiTenancy.Accounting;
using HLS.Topup.MultiTenancy.Payments;
using HLS.Topup.Paybacks;
using HLS.Topup.Security;
using HLS.Topup.Security.HLS.Topup.Security;
using HLS.Topup.Storage;

namespace HLS.Topup.EntityFrameworkCore
{
    public class TopupDbContext : AbpZeroDbContext<Tenant, Role, User, TopupDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<AbpIdentittyServerStorage> AbpIdentittyServerStorage { get; set; }
        public virtual DbSet<ChangeUserNameHistories> ChangeUserNameHistories { get; set; }
        public virtual DbSet<NotificationSchedule> NotificationSchedules { get; set; }

        public virtual DbSet<SystemAccountTransfer> SystemAccountTransfers { get; set; }

        public virtual DbSet<PayBatchBillDetail> PayBatchBillDetails { get; set; }
        public virtual DbSet<PayBatchBill> PayBatchBills { get; set; }
        public virtual DbSet<AccountBlockBalance> AccountBlockBalances { get; set; }
        public virtual DbSet<AccountBlockBalanceDetail> AccountBlockBalanceDetails { get; set; }
        public virtual DbSet<LimitProductDetail> LimitProductDetails { get; set; }
        public virtual DbSet<LimitProduct> LimitProducts { get; set; }
        public virtual DbSet<PayBack> Paybacks { get; set; }
        public virtual DbSet<PayBackDetail> PayBackDetails { get; set; }
        public virtual DbSet<Fee> Fees { get; set; }
        public virtual DbSet<FeeDetail> FeeDetails { get; set; }
        public virtual DbSet<SaleAssignAgent> SaleAssignAgents { get; set; }
        public virtual DbSet<SaleClearDebtHistory> SaleClearDebtHistories { get; set; }
        public virtual DbSet<SaleClearDebt> SaleClearDebts { get; set; }
        public virtual DbSet<SaleLimitDebt> SaleLimitDebts { get; set; }
        public virtual DbSet<SaleManLocation> SaleManLocations { get; set; }
        //public virtual DbSet<SaleMan> SaleMans { get; set; }

        public virtual DbSet<Odp> Odps { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }

        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }

        public virtual DbSet<District> Districts { get; set; }

        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<StaffConfiguration> StaffConfigurations { get; set; }
        public virtual DbSet<ServiceConfiguration> ServiceConfigurations { get; set; }
        public virtual DbSet<PartnerServiceConfiguration> PartnerServiceConfigurations { get; set; }

        public virtual DbSet<Otp> Otps { get; set; }
        public virtual DbSet<OtpMessage> OtpMessages { get; set; }
        public virtual DbSet<DiscountDetail> DiscountDetails { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }

        public virtual DbSet<Deposit> Deposits { get; set; }
        public virtual DbSet<AbpGeneratorId> AbpGeneratorIds { get; set; }


        public virtual DbSet<Bank> Banks { get; set; }

        public virtual DbSet<OrganizationsUnitCustom> OrganizationsUnitCustoms { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Service> Services { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public TopupDbContext(DbContextOptions<TopupDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);








            //







            modelBuilder.Entity<NotificationSchedule>(n =>
            {
                n.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<SystemAccountTransfer>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<PayBatchBill>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<AccountBlockBalance>(a =>
            {
                a.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<LimitProduct>(l =>
                       {
                           l.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Fee>(f =>
                       {
                           f.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<SaleClearDebt>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<SaleLimitDebt>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            //modelBuilder.Entity<SaleMan>(s =>
            // {
            //     s.HasIndex(e => new { e.TenantId });
            // });
            modelBuilder.Entity<Vendor>(v =>
                       {
                           v.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Ward>(w =>
                       {
                           w.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<District>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<City>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Country>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ServiceConfiguration>(s => { s.HasIndex(e => new { e.TenantId }); });
            modelBuilder.Entity<PartnerServiceConfiguration>(s => { s.HasIndex(e => new { e.TenantId }); });
            modelBuilder.Entity<Discount>(d => { d.HasIndex(e => new { e.TenantId }); });
            modelBuilder.Entity<Category>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
                c.HasIndex(e => e.CategoryCode).IsUnique();
            });
            modelBuilder.Entity<AccountBlockBalance>(c => { c.HasIndex(e => e.TransCode).IsUnique(); });
            modelBuilder.Entity<Fee>(c => { c.HasIndex(e => e.Code).IsUnique(); });
            modelBuilder.Entity<Discount>(c => { c.HasIndex(e => e.Code).IsUnique(); });
            modelBuilder.Entity<LimitProduct>(c => { c.HasIndex(e => e.Code).IsUnique(); });
            modelBuilder.Entity<Product>(c => { c.HasIndex(e => e.ProductCode).IsUnique(); });
            modelBuilder.Entity<Service>(c => { c.HasIndex(e => e.ServiceCode).IsUnique(); });
            modelBuilder.Entity<Provider>(c => { c.HasIndex(e => e.Code).IsUnique(); });
            modelBuilder.Entity<User>(c => { c.HasIndex(e => e.AccountCode).IsUnique(); });
            //modelBuilder.Entity<User>(c => { c.HasIndex(e => e.PhoneNumber).IsUnique(); });
            modelBuilder.Entity<Deposit>(d => { d.HasIndex(e => new { e.TenantId }); });
            modelBuilder.Entity<Bank>(b => { b.HasIndex(e => new { e.TenantId }); });
            modelBuilder.Entity<Provider>(p => { p.HasIndex(e => new { e.TenantId }); });
            modelBuilder.Entity<Product>(p => { p.HasIndex(e => new { e.TenantId }); });
            modelBuilder.Entity<Category>(c => { c.HasIndex(e => new { e.TenantId }); });
            modelBuilder.Entity<Service>(s => { s.HasIndex(e => new { e.TenantId }); });
            modelBuilder.Entity<Deposit>(s => { s.HasIndex(e => new { e.TransCode }); });
            modelBuilder.Entity<BinaryObject>(b => { b.HasIndex(e => new { e.TenantId }); });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
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
                    .IsUnique();
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
