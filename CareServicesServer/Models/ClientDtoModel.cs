using System.ComponentModel.DataAnnotations.Schema;

namespace CareServicesServer.Models
{
    public class ClientDtoModel
    {
        public int Id { get; set; }
        public string Tz { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string? Street { get; set; }
        public int? HouseNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? MobilePhone { get; set; }
        public CoronaDataDto? CoronaData { get; set; }
        public List<int>? ToremoveCoronaVaccineData { get; set; }
    }

    public class CoronaDataDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime? PositiveDate { get; set; }
        public DateTime? RecoverDate { get; set; }
        public List<CoronaVaccineDataDto>? CoronaVaccineData { get; set; }
    }

    public class CoronaVaccineDataDto
    {
        public int Id { get; set; }
        public int CoronaDataId { get; set; }
        public DateTime DateReceiptVaccination { get; set; }
        public string VaccineManufacturer { get; set; }

    }
}
