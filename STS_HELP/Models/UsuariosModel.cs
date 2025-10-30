using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace STS_HELP.Models
{

    [Table("usuarios")]
    public class UsuariosModel
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("auth_id")]
        public Guid AuthId { get; set; }


        [Column("nome")]
        public string Nome { get; set; } = "";


        [Column("email")]
        public string Email { get; set; } = "";


        [Column("tipo_usuario")]
        public string TipoUsuario { get; set; } = "";

        [Column("situacao_usuario")]
        public bool SituacaoUsuario { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Digite a senha")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        public string SenhaParaCadastro { get; set; } = "";

    }
}
