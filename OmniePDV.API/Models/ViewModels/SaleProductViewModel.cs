﻿using OmniePDV.API.Data.Entities;
using System.Text.Json.Serialization;

namespace OmniePDV.API.Models.ViewModels;

public sealed class SaleProductViewModel
{
    [JsonPropertyName("uid")]
    public Guid UID { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("quantity")]
    public double Quantity { get; set; }

    [JsonPropertyName("product")]
    public ProductViewModel Product { get; set; }

    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }
}

public static class SaleProductViewModelExtensions
{
    public static SaleProductViewModel ToViewModel(this SaleProduct model) => new()
    {
        UID = model.UID,
        Order = model.Order,
        Quantity = model.Quantity,
        Product = model.Product.ToViewModel(),
        Deleted = model.Deleted
    };

    public static List<SaleProductViewModel> ToViewModel(this List<SaleProduct> model) =>
        model.Select(m => m.ToViewModel()).ToList();
}
