using Microsoft.EntityFrameworkCore;
using Services.ServiceExtensions;
using Services.Interfaces;
using System.Reflection;
using Models.ViewModels;
using Data;
using System.Linq;
using Entities;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly EShopDatabaseContext dbContext;

        public ProductService(EShopDatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public IEnumerable<ProductAllViewModel> GetAllProducts()
        {
            var products = dbContext.Products.ToList();

            return MapProductsToProductAllViewModel(products);
        }

        public IEnumerable<ProductAllViewModel> GetListOfProducts(IEnumerable<int> productIds)
        {
            var products = dbContext.Products.Where(p => productIds.Contains(p.ProductId));

            return MapProductsToProductAllViewModel(products);
        }

        public IQueryable<ProductAllViewModel> GetSimilarProducts(int productId, int count)
        {
            var product = dbContext.Products.FirstOrDefault(p => p.ProductId == productId);
            var products = dbContext.Products.Where(p => p.ProductSubCategoryName == product.ProductSubCategoryName && p.ProductId != productId).Take(count);

            return MapProductsToProductAllViewModel(products).Shuffle().AsQueryable();
        }

        private IEnumerable<ProductAllViewModel> MapProductsToProductAllViewModel(IEnumerable<Product> products)
        {
            var viewModels = products.Select(p => new ProductAllViewModel
            {
                Id = p.ProductId,
                Name = p.ProductName,
                ModelId = p.ProductModelId,
                Model = p.ProductModel,
                Price = p.ListPrice,
                SubCategory = p.ProductSubCategoryName,
                SubCategoryId = p.ProductSubcategoryId,
                Description = p.Description,
                AverageRating = p.Rating ?? 0,
                Quantity = p.Quantity,
                DiscountPct = p.DiscountPct,
                ProductCategoryName = p.ProductCategoryName,
                Color = p.Color,
                Size = p.Size,
                PhotoId = p.PhotoId,
                LargePhoto = p.LargePhoto,
                ThumbNailPhoto = p.ThumbNailPhoto,
                Weight = p.Weight ?? 0
            });

            return viewModels;
        }
      
        public async Task<ProductAllViewModel?> GetProductDetailsById(int productId)
        {
            if (!await dbContext.Products.AnyAsync(p => p.ProductId == productId))
            {
                return null;
            }

            var productFromDb = await dbContext.Products.FirstAsync(p => p.ProductId == productId);
            var productViewModel = new ProductAllViewModel
            {
                Id = productFromDb.ProductId,
                Name = productFromDb.ProductName,
                ProductNumber = productFromDb.ProductNumber,
                Size = productFromDb.Size,
                Weight = productFromDb.Weight,
                Color = productFromDb.Color,
                Description = productFromDb.Description,
                Price = productFromDb.ListPrice,
                PhotoId=productFromDb.PhotoId,                
                DiscountPct = productFromDb.DiscountPct != null ? productFromDb.DiscountPct : 0,
                ProductCategoryName = productFromDb.ProductCategoryName,
                ProductCategoryId = productFromDb.ProductCategoryId,
                SubCategory = productFromDb.ProductSubCategoryName,
                ModelId = productFromDb.ProductModelId
            };

            return productViewModel;
        }

        public IQueryable<SearchSuggestionViewModel> GetSearchSuggestions(string text)
        {
            var suggestions = dbContext.Products.Where(p => p.ProductName.Contains(text) 
                && p.ProductCategoryName != null                
                && p.ProductSubCategoryName != null
                && p.ListPrice > 0).Select(p => new SearchSuggestionViewModel
            {
                ProductName = p.ProductName,
                ProductId = p.ProductId,
                SubCategory = p.ProductSubCategoryName
            });

            return suggestions;
        }

        public async Task<IQueryable<string>> GetAvailableSizes(int productId)
        {
            if (!await dbContext.Products.AnyAsync(p => p.ProductId == productId))
            {
                return Enumerable.Empty<string>().AsQueryable();
            }

            var color = await dbContext.Products.Where(p => p.ProductId == productId).Select(p => p.Color).FirstAsync();
            var modelId = await dbContext.Products.Where(p => p.ProductId == productId).Select(p => p.ProductModelId).FirstAsync();

            if (modelId == null)
            {
                return Enumerable.Empty<string>().AsQueryable();
            }

            var availableSizes = dbContext.Products
                .Where(p => p.ProductModelId == modelId && p.Color == color && p.Size != null)
                .Select(p => p.Size ?? "")
                .Distinct();

            return availableSizes;
        }

        public async Task<IQueryable<string>> GetAvailableColors(int productId)
        {
            if (!await dbContext.Products.AnyAsync(p => p.ProductId == productId))
            {
                return Enumerable.Empty<string>().AsQueryable();
            }

            var size = await dbContext.Products.Where(p => p.ProductId == productId).Select(p => p.Size).FirstAsync();
            var modelId = await dbContext.Products.Where(p => p.ProductId == productId).Select(p => p.ProductModelId).FirstAsync();

            if (modelId == null)
            {
                return Enumerable.Empty<string>().AsQueryable();
            }

            var availableColors = dbContext.Products
                .Where(p => p.ProductModelId == modelId && p.Size == size && p.Color != null)
                .Select(p => p.Color ?? "")
                .Distinct();

            return availableColors;
        }

        public async Task<int?> GetProductIdByModelAndSize(int modelId, string size)
        {
            if (!await dbContext.Products.AnyAsync(p => p.ProductModelId == modelId && p.Size == size))
            {
                return null;
            }

            var productId = await dbContext.Products
                .Where(p => p.ProductModelId == modelId && p.Size == size)
                .Select(p => p.ProductId)
                .FirstAsync();

            return productId;
        }

        public async Task<int?> GetProductIdByModelAndColor(int modelId, string color)
        {
            if (!await dbContext.Products.AnyAsync(p => p.ProductModelId == modelId && p.Color == color))
            {
                return null;
            }

            var productId = await dbContext.Products
                .Where(p => p.ProductModelId == modelId && p.Color == color)
                .Select(p => p.ProductId)
                .FirstAsync();

            return productId;
        }

        public async Task<int?> GetProductIdByModelSizeAndColor(int modelId, string size, string color)
        {
            if (!await dbContext.Products.AnyAsync(p => p.ProductModelId == modelId && p.Size == size && p.Color == color))
            {
                return null;
            }

            var productId = await dbContext.Products
                .Where(p => p.ProductModelId == modelId && p.Size == size && p.Color == color)
                .Select(p => p.ProductId)
                .FirstAsync();

            return productId;
        }

        public IEnumerable<string> GetAllCategoryNames()
        {
            var categories = dbContext.Products.Select(pc => pc.ProductName).ToList();

            return categories;
        }        

        public IQueryable<string> GetAllModelsInSubCategory(string subCategory)
        {
            var models = dbContext.Products
                .Where(pm => pm.ProductSubCategoryName == subCategory)
                .Select(pm => pm.ProductModel)
                .Distinct();

            return models;
        }

        public IQueryable<string> GetAllSizes()
        {
            var sizes = dbContext.Products.Where(p => p.Size != null).Select(p => p.Size ?? "").Distinct();

            return sizes;
        }

        public IQueryable<string> GetAllSizesInSubCategory(string subCategory)
        {
            var sizes = dbContext.Products
                .Where(p => p.ProductSubCategoryName != null && p.ProductSubCategoryName == subCategory)
                .Where(p => p.Size != null)
                .Select(p => p.Size ?? "")
                .Distinct();

            return sizes;
        }

        public IQueryable<string> GetAllColors()
        {
            var colors = dbContext.Products.Where(p => p.Color != null).Select(p => p.Color ?? "").Distinct();

            return colors;
        }

        public IQueryable<string> GetAllColorsInSubCategory(string subCategory)
        {
            var colors = dbContext.Products
                .Where(p => p.ProductSubCategoryName != null && p.ProductSubCategoryName == subCategory)
                .Where(p => p.Color != null)
                .Select(p => p.Color ?? "")
                .Distinct();

            return colors;
        }      

        public async Task<byte[]?> GetProductThumbnailById(int photoId)
        {
            if (await dbContext.Products.AnyAsync(p => p.PhotoId == photoId))
            {
                return (await dbContext.Products.FirstAsync(p => p.PhotoId == photoId)).ThumbNailPhoto;
            }

            return null;
        }

        public async Task<byte[]?> GetProductLargePhotoById(int photoId)
        {
            if (await dbContext.Products.AnyAsync(p => p.PhotoId == photoId))
            {
                return (await dbContext.Products.FirstAsync(p => p.PhotoId == photoId)).LargePhoto;
            }

            return null;
        }

        public async Task<IEnumerable<SubCategoryViewModel>> GetTopSellingSubCategories(int count)
        {
            var products = dbContext.Products.Where(p => p.ProductCategoryId != null && p.ProductSubcategoryId != null && p.ProductSubCategoryName != null)
                .Shuffle().Take(count).Select(psc => new SubCategoryViewModel
                {
                    SubCategoryId = psc.ProductSubcategoryId,
                    Name = psc.ProductSubCategoryName,
                    CategoryId = psc.ProductCategoryId,
                    TotalSales = psc.Quantity,
                    ImageName = GetCategoryImageName(psc.ProductSubcategoryId)
                }).DistinctBy(c => c.Name).ToList();

            return products;
        }

        private static string GetCategoryImageName(int? productSubcategoryId)
        {
            switch (productSubcategoryId)
            {
                case 1:
                    return "mountain_bikes.png";
                case 2:
                    return "road_bikes.png";
                case 3:
                    return "touring_bikes.png";
                case 4:
                    return "handlebars.png";
                case 7:
                    return "chains.png";
                case 9:
                    return "derailleurs.png";
                case 10:
                    return "forks.png";
                case 12:
                    return "mountain_frames.png";
                case 13:
                    return "pedals.png";
                case 15:
                    return "saddles.png";
                case 16:
                    return "touring_frames.png";
                case 17:
                    return "wheels.png";
                case 21:
                    return "jerseys.png";
                case 22:
                    return "shorts.png";
                case 28:
                    return "bottles_cages.png";
                case 34:
                    return "locks.png";
                case 36:
                    return "pumps.png";
                case 37:
                    return "tires_tubes1.png";
                default:
                    return "generic_bike1.png";
            }
        }

        public async Task<string> GetParentCategory(string subCategory)
        {
            if(subCategory != null)
            {
                var productCategory = dbContext.Products.FirstOrDefault(psc => psc.ProductSubCategoryName == subCategory);
                if (productCategory != null)
                {
                    return productCategory.ProductCategoryName;
                }
            }          

            return "Other";
        }
    }
}