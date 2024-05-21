using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Back_End_TPI_PSS.Services.Interfaces;

namespace Back_End_TPI_PSS.Data.Entities
{
    public class Colour : IStatusEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }


    }
}
