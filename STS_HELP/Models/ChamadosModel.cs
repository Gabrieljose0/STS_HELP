using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STS_HELP.Models
{
    [Table("chamados")]
    public class ChamadosModel
    {
        [Column("id_chamados")]
        public int Id { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; } = "";


        [Column("texto")]
        public string Texto { get; set; } = "";


        [Column("categoria")]
        public int CategoriaId { get; set; }
        public CategoriaModel Categoria { get; set; }


        [Column("prioridade")]
        public int PrioridadeId { get; set; }
        public PrioridadeModel Prioridade { get; set; }

        [Column("usuario")]
        public int UsuarioId { get; set; }
        public UsuariosModel Usuario { get; set; }


        [Column("dt_abertura")]
        public DateTime dt_Abertura { get; set; }



        [Column("dt_fechamento")]
        public DateTime? dt_fechamento { get; set; }


        [Column("id_tecnico")]
        public int? idTecnico { get; set; }

        [ForeignKey("idTecnico")]
        public UsuariosModel Tecnico { get; set; }


        [Column("status")]
        public int statusId { get; set; }
        public StatusModel Status { get; set; }

    }
}
