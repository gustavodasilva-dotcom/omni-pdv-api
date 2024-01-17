using OmniePDV.API.Data.Entities;
using OmniePDV.API.Models.InputModels;

namespace OmniePDV.API.Services.Interfaces;

public interface IPointOfSalesService
{
    Task<Sale> GetOpenedSaleAsync();
    Task<Sale> AddProductToSaleAsync(AddProductToSaleInputModel inputModel);
    Task<Sale> ChangeOpenedSaleStatusAsync(ChangeOpenedSaleStatusInputModel inputModel);
    Task<Sale> AddDiscountToSaleAsync(AddDiscountToSaleInputModel inputModel);
    Task<Sale> AddClientToSaleAsync(AddClientToSaleInputModel inputModel);
    Task<Sale> DeleteProductFromSaleAsync(int order);
}