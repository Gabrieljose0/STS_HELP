﻿using Microsoft.AspNetCore.Mvc;
using STS_HELP.Filters;
using STS_HELP.Models;
using STS_HELP.Repositorio;

namespace STS_HELP.Controllers
{
    [PaginaParaUsuariosLogados("Gestor", "Tecnico")]
    public class Chamados : Controller
    {
        private readonly IChamadoRepositorio _chamadoRepositorio;


        public Chamados(IChamadoRepositorio chamadoRepositorio)
        {
            _chamadoRepositorio = chamadoRepositorio;
        }


        //METODOS GET
        public IActionResult Index()
        {
            List<ChamadosModel> listaChamados = _chamadoRepositorio.ListarChamados();



            return View(listaChamados);
        }

        public IActionResult AceitarChamado(int id)
        {

            return View();
        }

        public IActionResult FinalizarChamado()
        {
            return View();
        }

        public IActionResult CriarChamado()
        {
            return View();
        }



        //METODOS POST
        [HttpPost]
        public IActionResult CriarChamado(ChamadosModel chamados)
        {
            if (ModelState.IsValid)
            {
                chamados.status = 1;

                // Define a data de abertura como o momento exato da criação do chamado.
                chamados.dt_Abertura = DateTime.UtcNow;

                

                _chamadoRepositorio.Adicionar(chamados);

                return RedirectToAction("Index");
            }
            return View(chamados);
        }


        [HttpPost]
        public IActionResult AceitarEFinalizarChamado(int id)
        {
            // Chamo repositório passando o ID
            _chamadoRepositorio.AceitarEFinalizarChamado(id);


            return RedirectToAction("Index");
        }






    }
}
