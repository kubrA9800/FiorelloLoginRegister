using FiorelloBackend.Areas.Admin.ViewModels.Slider;
using FiorelloBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FiorelloBackend.ViewModels.Home
{
    public class HomeVm
    {
        public List<SliderVM> Sliders { get; set; }
        public SliderText SliderTexts { get; set; }

        public List<Blog> Blogs { get; set; }
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public About Abouts { get; set; }
        public List<HomeAboutIcon> HomeAboutIcons { get; set; }
        public List<Expert> Experts { get; set; }
        public HomeSubscribe HomeSubscribes { get; set; }
        public List<HomeSay> HomeSays { get; set; }
        public List<HomeInstagram> HomeInstagrams { get; set; }
    }
}
