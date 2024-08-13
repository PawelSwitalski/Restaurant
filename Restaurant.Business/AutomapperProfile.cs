using AutoMapper;
using Restaurant.Business.Models;
using Restaurant.Data.Entities;

namespace Restaurant.Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Receipt, ReceiptModel>()
                .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            CreateMap<Product, ProductModel>()
                .ForMember(pm => pm.CategoryName, p => p.MapFrom(x => x.Category.CategoryName))
                .ForMember(pm => pm.ProductCategoryId, p => p.MapFrom(x => x.Category.Id))
                .ForMember(pm => pm.ReceiptDetailIds, p => p.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            CreateMap<ReceiptDetail, ReceiptDetailModel>()
                .ReverseMap();


            CreateMap<ProductCategory, ProductCategoryModel>()
                .ForMember(pm => pm.ProductIds, pc => pc.MapFrom(x => x.Products.Select(p => p.Id)))
                .ReverseMap();

        }
    }
}