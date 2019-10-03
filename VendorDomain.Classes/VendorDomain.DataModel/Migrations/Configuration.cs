namespace VendorDomain.DataModel.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<VendorDomain.DataModel.VendorContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(VendorDomain.DataModel.VendorContext context)
        {
            //  This method will be called after migrating to the latest version.
            DefaultDbStartup.CreateDefaultDb(context);
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
