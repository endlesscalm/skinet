using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IBasketRepo
    {
        Task<CustomerBasket> GetBacketAsync(string basketId);
        Task<CustomerBasket> UpdateBacketAsync(CustomerBasket basket);
        Task<bool> DeleteBacketAsync(string basketId);
    }
}