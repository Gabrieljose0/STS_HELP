using Microsoft.EntityFrameworkCore;
using STS_HELP.Controllers;
using STS_HELP.Data;
using STS_HELP.Models;
using System;


namespace STS_HELP.Repositorio
{
    public class ChamadoRepositorio : IChamadoRepositorio
    {

        private readonly BancoContext _bancoContext;

        public ChamadoRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }


        public List<ChamadosModel> ListarChamados()
        {

            return _bancoContext.Chamados
                         .Include(c => c.Usuario)
                         .Include(c => c.Status)
                         .Include(c => c.Categoria)
                         .Include(c => c.Prioridade)
                         .Include(c => c.Tecnico)
                         .OrderBy(c => c.Id)
                         .ToList();

        }


        public int TotalChamados()
        {
            return _bancoContext.Chamados.Count();
        }

        public int TotalChamadosAberto()
        {
            return _bancoContext.Chamados
                .Count(c => c.Status.Nome == "Aberto");
        }

        public int TotalChamadosEmAtendimento()
        {
            return _bancoContext.Chamados
                .Count(c => c.Status.Nome == "Em Atendimento");
        }

        public int TotalChamadosFinalizados()
        {
            return _bancoContext.Chamados.Count(c => c.Status.Nome == "Concluido");
        }




        public ChamadosModel Adicionar(ChamadosModel chamados)
        {
            
            _bancoContext.Chamados.Add(chamados);
            _bancoContext.SaveChanges();
            return chamados;

        }


        public ChamadosModel ExibeInfoChamado(int id)
        {
            return _bancoContext.Chamados.FirstOrDefault(x => x.Id == id);
        }


        public ChamadosModel AceitarEFinalizarChamado(int id, int idTecnico)
        {
            ChamadosModel ChamadosDB = ExibeInfoChamado(id);

            if (ChamadosDB == null)
            {
                throw new Exception("Erro ao atualizar status do Chamado. ID não Encontrado.");
            }

            // Altere o status do Chamado

            if (ChamadosDB.statusId == 1)
            {
                ChamadosDB.statusId = 2;

                ChamadosDB.idTecnico = idTecnico;
            }
            else if(ChamadosDB.statusId == 2) 
            {
                ChamadosDB.statusId = 3;

                //ChamadosDB.dt_fechamento = DateTime.UtcNow;

                DateTime horarioUtc = DateTime.UtcNow;


                string idFuso = "E. South America Standard Time";
                TimeZoneInfo fusoBrasilia;

                try
                {
                    fusoBrasilia = TimeZoneInfo.FindSystemTimeZoneById(idFuso);
                }
                catch
                {
                    fusoBrasilia = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
                }

                // 3. Converte e salva no objeto
                ChamadosDB.dt_fechamento = DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(horarioUtc, fusoBrasilia), DateTimeKind.Utc);

            }

            
            _bancoContext.SaveChanges();

            return ChamadosDB;
        }

        public int TotalChamadosProblemaEquipamento()
        {
            return _bancoContext.Chamados
                .Count(c => c.Categoria.Nome == "Problema de Equipamento");
        }

        public int TotalChamadosProblemaRede()
        {
            return _bancoContext.Chamados
                .Count(c => c.Categoria.Nome == "Problemas de Rede");
        }

        public int TotalChamadosErroNoSistema()
        {
            return _bancoContext.Chamados
                .Count(c => c.Categoria.Nome == "Erro no Sistema");
        }

        public int TotalChamadosAcessoNegado()
        {
            return _bancoContext.Chamados
                 .Count(c => c.Categoria.Nome == "Acesso Negado");
        }

        public int TotalChamadosSolicitacaoDeEquipamento()
        {
            return _bancoContext.Chamados
                .Count(c => c.Categoria.Nome == "Solicitação de Equipamento");
        }

        public int TotalChamadosSolicitacaoDeSoftware()
        {
            return _bancoContext.Chamados
                .Count(c => c.Categoria.Nome == "Solicitação de Software");
        }


        public int TotalAtendimentosPorTecnico(int idTecnico)
        {
            return _bancoContext.Chamados
        .Count(c => c.idTecnico == idTecnico && c.Status.Nome == "Concluido");
        }




        public List<ChamadosModel> BuscarRelatorioPersonalizado(string tecnico, string solicitante, DateTime? dataInicio)
        {
            var query = _bancoContext.Chamados
                .Include(x => x.Status)
                .Include(x => x.Usuario)
                .Include(x => x.Tecnico)
                .AsQueryable();

            // 1. Correção do Filtro de TÉCNICO (Ignorando maiúsculas/minúsculas)
            if (!string.IsNullOrEmpty(tecnico))
            {
                // .ToLower() garante que "Ivanildo" e "ivanildo" sejam iguais
                query = query.Where(x => x.Tecnico != null && x.Tecnico.Nome.ToLower().Contains(tecnico.ToLower()));
            }

            // 2. Correção do Filtro de SOLICITANTE (Ignorando maiúsculas/minúsculas)
            if (!string.IsNullOrEmpty(solicitante))
            {
                query = query.Where(x => x.Usuario != null && x.Usuario.Nome.ToLower().Contains(solicitante.ToLower()));
            }

            // 3. Correção do ERRO DE DATA (PostgreSQL)
            if (dataInicio.HasValue)
            {
                // O PostgreSQL exige UTC. Essa linha força a data a ser UTC sem mudar o horário.
                DateTime dataUtc = DateTime.SpecifyKind(dataInicio.Value, DateTimeKind.Utc);

                // Compara com a data UTC
                query = query.Where(x => x.dt_Abertura >= dataUtc);
            }

            return query.OrderByDescending(x => x.dt_Abertura).ToList();
        }



    }

}
