using FiorelloBackend.Services.Interfaces;
using FiorelloBackend.ViewModels;

namespace FiorelloBackend.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly IBasketService _basketService;
        private readonly ISettingsService _settingService;
        public LayoutService(IBasketService basketService,
                            ISettingsService settingsService)
        {
            _basketService = basketService;
            _settingService = settingsService;
        }
        

        public HeaderVm GetHeaderDatas()
        {
            Dictionary<string, string> settingDatas = _settingService.GetSettings();
            int basketCount = _basketService.GetCount();

            return new HeaderVm
            {
                BasketCount = basketCount,
                Logo = settingDatas["HeaderLogo"]
            };
        }
    }
}
