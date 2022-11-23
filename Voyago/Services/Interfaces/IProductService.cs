using Data;
using Models.ViewModels;

namespace Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<ProductAllViewModel> GetAllProducts();

        IEnumerable<ProductAllViewModel> GetListOfProducts(IEnumerable<int> productIds);

        IQueryable<ProductAllViewModel> GetSimilarProducts(int productId, int count);

        Task<ProductAllViewModel?> GetProductDetailsById(int productId);

        IQueryable<SearchSuggestionViewModel> GetSearchSuggestions(string text);

        Task<IQueryable<string>> GetAvailableSizes(int productId);

        Task<IQueryable<string>> GetAvailableColors(int productId);

        Task<int?> GetProductIdByModelAndSize(int modelId, string size);

        Task<int?> GetProductIdByModelAndColor(int modelId, string color);

        Task<int?> GetProductIdByModelSizeAndColor(int modelId, string size, string color);

        Task<byte[]?> GetProductThumbnailById(int photoId);

        Task<byte[]?> GetProductLargePhotoById(int photoId);

        IEnumerable<SortParmeterViewModel> GetAllSortParameters();

        IEnumerable<string> GetAllCategoryNames();

        //IEnumerable<string> GetAllSubCategoryNames();

        //IQueryable<string> GetAllModels();

        IQueryable<string> GetAllModelsInSubCategory(string subCategory);

        IQueryable<string> GetAllSizes();
        
        IQueryable<string> GetAllSizesInSubCategory(string subCategory);
        
        IQueryable<string> GetAllColors();
        
        IQueryable<string> GetAllColorsInSubCategory(string subCategory);
        
        Task<IEnumerable<SubCategoryViewModel>> GetTopSellingSubCategories(int count);

        Task<string> GetParentCategory(string subCategory);
    }
}
