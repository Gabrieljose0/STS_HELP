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


            //Imprelementar Dps Join com as tabelas Categoria, Status, Usuario.
            //return _bancoContext.Chamados.OrderBy(u => u.Id).ToList();

            return _bancoContext.Chamados
                         .Include(c => c.Usuario)
                         .Include(c => c.Status)
                         .Include(c => c.Categoria)
                         .Include(c => c.Prioridade)
                         .Include(c => c.Tecnico)
                         .OrderBy(c => c.Id)
                         .ToList();


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

                ChamadosDB.dt_fechamento = DateTime.UtcNow;
            }

            
            _bancoContext.SaveChanges();

            return ChamadosDB;
        }

        
    }

}
