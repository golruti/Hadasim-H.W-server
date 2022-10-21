using System.ComponentModel.DataAnnotations.Schema;

namespace CareServicesServer.Models
{
    public class CoronaVaccineData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(CoronaData))]
        public int CoronaDataId { get; set; }
        public DateTime DateReceiptVaccination { get; set; }
        public string VaccineManufacturer { get; set; }

        public virtual CoronaData CoronaData { get; set; }
    }
}
