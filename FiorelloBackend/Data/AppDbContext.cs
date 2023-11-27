using FiorelloBackend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FiorelloBackend.Data
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options){}

        public DbSet<Slider> Sliders { get; set; }

        public DbSet<SliderText> SliderTexts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<About> AboutContents { get; set; }
        public DbSet<HomeAboutIcon> HomeAboutIcons{ get; set; }
        public DbSet<Expert> Experts { get; set; }
        public DbSet<HomeSubscribe> HomeSubscribes { get; set; }
        public DbSet<HomeSay> HomeSays { get; set; }
        public DbSet<HomeInstagram> HomeInstagrams { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<CategoryArchive> CategoryArchives { get; set; }
    }
}
