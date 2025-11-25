using STS_HELP.Models;

namespace STS_HELP
{
    public interface IChamadoRepositorio
    {

        List<ChamadosModel> ListarChamados();

        ChamadosModel Adicionar(ChamadosModel chamados);

        ChamadosModel ExibeInfoChamado(int id);

        ChamadosModel AceitarEFinalizarChamado(int id, int idTecnico);

        int TotalChamados();

        int TotalChamadosAberto();
        int TotalChamadosEmAtendimento();

        int TotalChamadosFinalizados();


        int TotalChamadosProblemaEquipamento();
        int TotalChamadosProblemaRede();
        int TotalChamadosErroNoSistema();
        int TotalChamadosAcessoNegado();
        int TotalChamadosSolicitacaoDeEquipamento();
        int TotalChamadosSolicitacaoDeSoftware();

        int TotalAtendimentosPorTecnico(int idTecnico);


        List<ChamadosModel> BuscarRelatorioPersonalizado(string tecnico, string solicitante, DateTime? dataInicio);

        
    }
}
