using System.ComponentModel.DataAnnotations.Schema;

namespace STS_HELP.Models
{

    [Table("status")]
    public class StatusModel
    {

            [Column("id_status")]
            public int Id { get; set; }

            [Column("nm_status")]
            public string Nome { get; set; }

    }
}
