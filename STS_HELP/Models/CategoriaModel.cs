using System.ComponentModel.DataAnnotations.Schema;

namespace STS_HELP.Models
{

    [Table("categoria")]
    public class CategoriaModel
    {

        [Column("id_categoria")]
        public int Id { get; set; }

        [Column("nm_categoria")]
        public string Nome { get; set; }

    }
}
