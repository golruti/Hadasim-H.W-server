using System.ComponentModel.DataAnnotations.Schema;

namespace CareServicesServer.Models
{
    public class CoronaData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }
        public DateTime? PositiveDate { get; set; }
        public DateTime? RecoverDate { get; set; }

        public virtual Clients Client { get; set; }
       // public virtual List<CoronaVaccineData> CoronaVaccineData { get; set; }

    }
}
