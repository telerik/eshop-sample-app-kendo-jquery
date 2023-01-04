using System.ComponentModel.DataAnnotations;

namespace Models.InputModels
{
    public class AddressUserInputModel
    {
        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zipcode { get; set; }

        public string? Country { get; set; }
    }
}
