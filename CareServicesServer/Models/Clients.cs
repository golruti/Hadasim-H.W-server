using System.ComponentModel.DataAnnotations.Schema;

namespace CareServicesServer.Models
{
    public class Clients
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

       // public virtual List<CoronaData> CoronaData { get; set; }


    }
}
