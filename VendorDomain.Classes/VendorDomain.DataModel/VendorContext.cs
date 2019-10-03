using System.Data.Entity;
using VendorDomain.Classes;
using VendorDomain.Classes.Interfaces;
using System.Linq;
using System;

namespace VendorDomain.DataModel
{
    public class VendorContext : DbContext
    {
        public VendorContext() : base("name=VendorAppDatabase")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<VendorContext>());
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<EquipmentElement> Equipment { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Types().Configure(x => x.Ignore("IsDirty"));
            base.OnModelCreating(modelBuilder);
        }
        public override int SaveChanges()
        {
            foreach (var history in ChangeTracker.Entries().Where(e => e.Entity is IModificationHistory && e.State == EntityState.Added || e.State == EntityState.Modified)
                                                           .Select(e => e.Entity as IModificationHistory))
            {
                history.DateModified = DateTime.Now;
                if (history.DateCreated == DateTime.MinValue)
                {
                    history.DateCreated = DateTime.Now;
                }
            }

            int result = base.SaveChanges();
            foreach (var history in ChangeTracker.Entries().Where(e => e is IModificationHistory)
                                                           .Select(e => e as IModificationHistory))
            {
                history.IsDirty = false;
            }

            return result;
        }
    }
}
