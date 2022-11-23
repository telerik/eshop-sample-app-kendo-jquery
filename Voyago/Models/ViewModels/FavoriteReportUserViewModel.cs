namespace Models.ViewModels
{
    public class FavoriteUserProductViewModel
    {
        //public int Id { get; set; }      

        public string UserName { get; set; }
      
        public List<FavoriteReportProductViewModel> FavoriteReportProducts { get; set; }

    }
}
