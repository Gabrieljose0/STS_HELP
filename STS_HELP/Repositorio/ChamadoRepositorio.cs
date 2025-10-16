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
            return _bancoContext.Chamados.OrderBy(u => u.Id).ToList(); 
                
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


        public ChamadosModel AceitarEFinalizarChamado(int id)
        {
            ChamadosModel ChamadosDB = ExibeInfoChamado(id);

            if (ChamadosDB == null)
            {
                throw new Exception("Erro ao atualizar status do Chamado. ID não Encontrado.");
            }

            // Altere o status do Chamado

            if (ChamadosDB.status == 1)
            {
                ChamadosDB.status = 2;
            }
            else if(ChamadosDB.status == 2) 
            {
                ChamadosDB.status = 3;

                ChamadosDB.dt_fechamento = DateTime.UtcNow;
            }

            //_bancoContext.Chamados.Update(ChamadosDB);
            _bancoContext.SaveChanges();

            return ChamadosDB;
        }

        
    }
}
