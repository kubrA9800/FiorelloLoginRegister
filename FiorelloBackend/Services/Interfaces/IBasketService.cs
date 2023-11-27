using FiorelloBackend.Helpers.Responses;
using FiorelloBackend.Models;
using FiorelloBackend.ViewModels;

namespace FiorelloBackend.Services.Interfaces
{
    public interface IBasketService
    {
        void AddBasket(int id,Product product);
        Task<List<BasketDetailVm>> GetBasketDatasAsync();
        int GetCount();
        Task<DeleteBasketResponse> DeleteProductFromBasket(int id);
        Task<ProductCountResponse> ProductCountPlus(int id);
        Task<ProductCountResponse> ProductCountMinus(int id);
    }
}
