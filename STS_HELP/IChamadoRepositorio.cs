﻿using STS_HELP.Models;

namespace STS_HELP
{
    public interface IChamadoRepositorio
    {

        List<ChamadosModel> ListarChamados();

        ChamadosModel Adicionar(ChamadosModel chamados);

        ChamadosModel ExibeInfoChamado(int id);

        ChamadosModel AceitarEFinalizarChamado(int id);

        //ChamadosModel FinalizarChamados(int id);
       
    }
}
