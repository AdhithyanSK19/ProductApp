using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WalkersApp.Data
{
    public class WalksAuthDbContext:IdentityDbContext
    {
        public WalksAuthDbContext(DbContextOptions<WalksAuthDbContext> dbContextOptions):base(dbContextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var readerRoleId = "4a75a7a5-c1c8-46be-a29c-18361af5f233";
            var writerRoleId = "9845d53c-e14e-411d-811b-6f2ef7418092";

            var roles = new List<IdentityRole>(){
                new ()
                {
                    Id = readerRoleId,
                    ConcurrencyStamp=readerRoleId,
                    Name="Reader",
                    NormalizedName="Reader".ToUpper()

                },
                new()
                {
                    Id= writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name="Writer",
                    NormalizedName="Writer".ToUpper()

                }
            };
            
            modelBuilder.Entity<IdentityRole>().HasData(roles);

        }
    }
}
