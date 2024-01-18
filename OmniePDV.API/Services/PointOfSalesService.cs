using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OmniePDV.API.Data;
using OmniePDV.API.Data.Entities;
using OmniePDV.API.Entities;
using OmniePDV.API.Exceptions;
using OmniePDV.API.Models.InputModels;
using OmniePDV.API.Models.Requests;
using OmniePDV.API.Options;
using OmniePDV.API.Services.Interfaces;

namespace OmniePDV.API.Services;

public sealed class PointOfSalesService(
    IMongoContext context,
    IMessageProducer messageProducer,
    IOptions<DefaultClientOptions> options) : IPointOfSalesService
{
    private readonly IMongoContext _context = context;
    private readonly IMessageProducer _messageProducer = messageProducer;
    private readonly DefaultClientOptions _defaultClientOptions = options.Value;

    public async Task<Sale> GetOpenedSaleAsync()
    {
        return await _context.Sales
            .Find(s => s.Status == SaleStatusEnum.Open)
            .FirstOrDefaultAsync();
    }

    public async Task<Sale> AddProductToSaleAsync(AddProductToSaleInputModel inputModel)
    {
        Product product = await _context.Products
            .Find(p => p.Barcode == inputModel.Barcode)
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("Product not found");

        Sale sale = await GetOpenedSaleAsync();
        if (sale == null)
        {
            Client defaultClient = await _context.Clients
                .Find(c => c.Name.Equals(_defaultClientOptions.Name))
                .FirstOrDefaultAsync();

            long totalSales = _context.Sales.CountDocuments(s => true);
            sale = new Sale(
                Number: totalSales + 1,
                Subtotal: 0,
                Total: 0,
                Client: defaultClient,
                Products: []
            );
            await _context.Sales.InsertOneAsync(sale);
        }

        SaleProduct saleProduct = new(
            UID: Guid.NewGuid(),
            Order: sale.Products.Count + 1,
            Quantity: inputModel.Quantity,
            Product: product
        );
        sale.AddProduct(saleProduct);
        await _context.Sales.ReplaceOneAsync(s =>
            s.UID == sale.UID, sale);

        return sale;
    }

    public async Task<Sale> ChangeOpenedSaleStatusAsync(ChangeOpenedSaleStatusInputModel inputModel)
    {
        Sale sale = await GetOpenedSaleAsync() ??
            throw new BadRequestException("There's no opened sale");

        if (inputModel.Status == SaleStatusEnum.Cancelled)
            sale.CancelSale();
        if (inputModel.Status == SaleStatusEnum.Closed)
            sale.CloseSale();

        await _context.Sales.ReplaceOneAsync(s => s.UID == sale.UID, sale);

        return sale;
    }

    public async Task SendReceiptThroughEmailAsync(SendSaleReceiptEmailInputModel inputModel)
    {
        if (inputModel.SaleID.Equals(Guid.Empty))
            throw new BadRequestException("Invalid sale_id");

        Sale sale = await _context.Sales
            .Find(s => s.UID.Equals(inputModel.SaleID))
            .FirstOrDefaultAsync() ??
            throw new BadRequestException($"There's no sale with the id {inputModel.SaleID}");

        if ((sale.Client.SSN.Equals(_defaultClientOptions.SSN)
             || string.IsNullOrEmpty(sale.Client.Email)
            ) && string.IsNullOrEmpty(inputModel.Email))
            throw new BadRequestException("Email cannot be null or empty");

        if (!sale.Client.SSN.Equals(_defaultClientOptions.SSN)
            && string.IsNullOrEmpty(sale.Client.Email))
        {
            if (string.IsNullOrEmpty(inputModel.Email))
                throw new BadRequestException("Email cannot be null or empty");

            Client client = await _context.Clients
                .Find(c => c.UID.Equals(sale.Client.UID))
                .FirstOrDefaultAsync();

            client.SetEmail(inputModel.Email);
            await _context.Clients.ReplaceOneAsync(c =>
                c.UID.Equals(client.UID), client);

            sale.SetClient(client);
            await _context.Sales.ReplaceOneAsync(c =>
                c.UID.Equals(sale.UID), sale);
        }

        SendReceiptEmailMessageRequest request = new(
            sale: sale,
            email: inputModel.Email
        );
        _messageProducer.SendMessage(request);
    }

    public async Task<Sale> AddDiscountToSaleAsync(AddDiscountToSaleInputModel inputModel)
    {
        Sale sale = await GetOpenedSaleAsync() ??
            throw new BadRequestException("There's no opened sale");

        sale.AddDiscount(inputModel.Value, inputModel.Type);
        await _context.Sales.ReplaceOneAsync(s =>
            s.UID == sale.UID, sale);

        return sale;
    }

    public async Task<Sale> AddClientToSaleAsync(AddClientToSaleInputModel inputModel)
    {
        Sale sale = await _context.Sales
            .Find(s => s.Status == SaleStatusEnum.Open)
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("There's no opened sale");

        Client client = await _context.Clients
            .Find(p => p.UID.Equals(inputModel.ClientID))
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("Client not found");

        sale.SetClient(client);
        await _context.Sales.ReplaceOneAsync(s =>
            s.UID == sale.UID, sale);

        return sale;
    }

    public async Task<Sale> DeleteProductFromSaleAsync(int order)
    {
        Sale sale = await GetOpenedSaleAsync() ??
            throw new BadRequestException("There's no opened sale");

        SaleProduct? productToDelete = sale.Products
            .FirstOrDefault(p => p.Order == order) ??
            throw new BadRequestException(string.Format("There's no product with the order {0}", order));

        productToDelete.DeleteProduct();
        sale.UpdateSubtotal();
        await _context.Sales.ReplaceOneAsync(s =>
            s.UID == sale.UID, sale);

        return sale;
    }
}