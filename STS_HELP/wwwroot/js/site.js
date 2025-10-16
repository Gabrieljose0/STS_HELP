// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.



//Modais Index Usuarios
const inativarModal = document.getElementById('inativarModal');
const ativarModal = document.getElementById('ativarModal');


if (inativarModal) {

    inativarModal.addEventListener('show.bs.modal', event => {
        // Pega o botão que abriu o modal
        const button = event.relatedTarget;

        // Extrai o ID do atributo data-id do botão
        const usuarioId = button.getAttribute('data-id');

        // Encontra o input escondido dentro do modal e define seu valor
        const hiddenInput = inativarModal.querySelector('#usuarioIdParaInativar');
        hiddenInput.value = usuarioId;
    });
}


if (ativarModal) {

    ativarModal.addEventListener('show.bs.modal', event => {
        // Pega o botão que abriu o modal
        const button = event.relatedTarget;

        // Extrai o ID do atributo data-id do botão
        const usuarioId = button.getAttribute('data-id');

        // Encontra o input escondido dentro do modal e define seu valor
        const hiddenInput = ativarModal.querySelector('#usuarioIdParaAtivar');
        hiddenInput.value = usuarioId;
    });

}




//Modais Index Chamados
const AceitarChamadoModal = document.getElementById('AceitarChamadoModal');
const finalizarChamadoModal = document.getElementById('FinalizarChamadoModal');
const VisualizartextoModal = document.getElementById('VisualizartextoModal');


if (AceitarChamadoModal) {

    AceitarChamadoModal.addEventListener('show.bs.modal', event => {
        // Pega o botão que abriu o modal
        const button = event.relatedTarget;

        // Extrai o ID do atributo data-id do botão
        const chamadoId = button.getAttribute('data-id');

        // Encontra o input escondido dentro do modal e define seu valor
        const hiddenInput = AceitarChamadoModal.querySelector('#ChamadoIdIniciar');

        hiddenInput.value = chamadoId;
    });
}



if (finalizarChamadoModal) {

    finalizarChamadoModal.addEventListener('show.bs.modal', event => {
        // Pega o botão que abriu o modal
        const button = event.relatedTarget;

        // Extrai o ID do atributo data-id do botão
        const chamadoId = button.getAttribute('data-id');

        // Encontra o input escondido dentro do modal e define seu valor
        const hiddenInput = finalizarChamadoModal.querySelector('#ChamadoIdFinalizar');
        hiddenInput.value = chamadoId;
    });

}

if (VisualizartextoModal) {

    VisualizartextoModal.addEventListener('show.bs.modal', event => {
        // Pega o botão que abriu o modal
        const button = event.relatedTarget;

        // Extrai o ID do atributo data-id do botão
        const chamadoId = button.getAttribute('data-id');
        const chamadoTexto = button.getAttribute('data-texto');

        // Encontra o input escondido dentro do modal e define seu valor
        const hiddenInput = VisualizartextoModal.querySelector('#ChamadoIdIniciar');
        const pTexto = VisualizartextoModal.querySelector('#textoDoChamado');

        hiddenInput.value = chamadoId;
        pTexto.textContent = chamadoTexto;
    });
}



if ($('#tabelaUsuarios').length) {

    $(document).ready(function () {
        $('#tabelaUsuarios').DataTable({
            "language": {
                "url": "https://cdn.datatables.net/plug-ins/2.0.7/i18n/pt-BR.json"
            }
        });
    });

}

if ($('tabelaChamados')) {

    $(document).ready(function () {
        $('#tabelaChamados').DataTable({
            "language": {
                "url": "https://cdn.datatables.net/plug-ins/2.0.7/i18n/pt-BR.json"
            }
        });
    });

}




