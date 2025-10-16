using System.ComponentModel.DataAnnotations.Schema;

namespace STS_HELP.Models
{

    [Table("prioridades")]
    public class PrioridadeModel
    {

            [Column("id_prioridade")]
            public int Id { get; set; }

            [Column("nm_prioridade")]
            public string Nome { get; set; }

    }
}
