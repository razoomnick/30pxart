using System.Data.Entity.Migrations;

namespace Patterns.Data
{
    public class MigrationConfiguration : DbMigrationsConfiguration<PatternsContext>
    {
        public MigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(PatternsContext context)
        {
        }
    }
}
