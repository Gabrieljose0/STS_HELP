// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification

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





//GRAFICO PIZZA
// Adicione este "ouvinte" para garantir que o HTML carregou antes de rodar o script
document.addEventListener("DOMContentLoaded", function () {

    // Verifica se o elemento do gráfico existe *nesta* página
    var ctx = document.getElementById("myPieChart");

    // Só executa o código do gráfico se encontrar o canvas
    if (ctx) {

        // 1. LÊ OS DADOS DOS ATRIBUTOS 'data-*' DO HTML
        var dadosProblemaEquipamento = ctx.dataset.equipamento;
        var dadosProblemaRede = ctx.dataset.rede;
        var dadosErroNoSistema = ctx.dataset.sistema;
        var dadosAcessoNegado = ctx.dataset.acesso;
        var dadosSolicitacaoDeEquipamento = ctx.dataset.solicitacao;
        var dadosSolicitacaoDeSoftware = ctx.dataset.software;

        // 2. CRIA O GRÁFICO
        var myPieChart = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: ["Problema Equipamento", "Problema Rede", "Problema Sistema", "Acesso Negado", "Solicitação Equipamento", "Solicitação Software"],
                datasets: [{
                    // 3. USA AS VARIÁVEIS QUE ACABAMOS DE LER
                    data: [dadosProblemaEquipamento, dadosProblemaRede, dadosErroNoSistema, dadosAcessoNegado, dadosSolicitacaoDeEquipamento, dadosSolicitacaoDeSoftware],
                    backgroundColor: ['#4e73df', '#f6c23e', '#1cc88a', '#FF0000', '#FFA500', '#808080'], // Azul, Amarelo, Verde, Vermelho, Laranja, Cinza
                    hoverBackgroundColor: ['#2e59d9', '#dda20a', '#17a673'],
                    hoverBorderColor: "rgba(234, 236, 244, 1)",
                }],
            },
            options: {
                maintainAspectRatio: false,
                tooltips: {
                    backgroundColor: "rgb(255,255,255)",
                    bodyFontColor: "#858796",
                    borderColor: '#dddfeb',
                    borderWidth: 1,
                    xPadding: 15,
                    yPadding: 15,
                    displayColors: false,
                    caretPadding: 10,
                },
                legend: {
                    display: false
                },
                cutoutPercentage: 80,
            },
        });
    }

});



//Grafico Linhas
// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';

document.addEventListener("DOMContentLoaded", function () {

   

    // --- GRÁFICO DE LINHA (Atendimentos por Técnico) ---
    var areaCtx = document.getElementById("myAreaChart");

    // Só executa se o canvas existr
    if (areaCtx) {

        // 1. LÊ OS DADOS DOS ATRIBUTOS 'data-*' DO HTML
        var dadosIvanildo = areaCtx.dataset.ivanildo;
        var dadosViviane = areaCtx.dataset.viviane;
        var dadosEvelyn = areaCtx.dataset.evelyn;
        var dadosAugusto = areaCtx.dataset.augusto;

        // (Função de formatação de número que você já tinha)
        function number_format(number, decimals, dec_point, thousands_sep) {
            number = (number + '').replace(',', '').replace(' ', '');
            var n = !isFinite(+number) ? 0 : +number,
                prec = !isFinite(+decimals) ? 0 : Math.abs(decimals),
                sep = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep,
                dec = (typeof dec_point === 'undefined') ? '.' : dec_point,
                s = '',
                toFixedFix = function (n, prec) {
                    var k = Math.pow(10, prec);
                    return '' + Math.round(n * k) / k;
                };
            s = (prec ? toFixedFix(n, prec) : '' + Math.round(n)).split('.');
            if (s[0].length > 3) {
                s[0] = s[0].replace(/\B(?=(?:\d{3})+(?!\d))/g, sep);
            }
            if ((s[1] || '').length < prec) {
                s[1] = s[1] || '';
                s[1] += new Array(prec - s[1].length + 1).join('0');
            }
            return s.join(dec);
        }

        var myLineChart = new Chart(areaCtx, {
            type: 'line',
            data: {
                // 2. Rótulos dos técnicos
                labels: ["Augusto", "Evelyn", "Ivanildo", "Viviane"],
                datasets: [{
                    label: "Atendimentos",
                    lineTension: 0.3,
                    backgroundColor: "rgba(78, 115, 223, 0.05)",
                    borderColor: "rgba(78, 115, 223, 1)",
                    pointRadius: 3,
                    pointBackgroundColor: "rgba(78, 115, 223, 1)",
                    pointBorderColor: "rgba(78, 115, 223, 1)",
                    pointHoverRadius: 3,
                    pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
                    pointHoverBorderColor: "rgba(78, 115, 223, 1)",
                    pointHitRadius: 10,
                    pointBorderWidth: 2,

                    data: [dadosAugusto, dadosEvelyn, dadosIvanildo, dadosViviane],
                }],
            },
            options: {
                maintainAspectRatio: false,
                layout: {
                    padding: { left: 10, right: 25, top: 25, bottom: 0 }
                },
                scales: {
                    xAxes: [{
                        gridLines: {
                            display: false,
                            drawBorder: false
                        },
                        ticks: {
                            maxTicksLimit: 7
                        }
                    }],
                    yAxes: [{
                        ticks: {
                            maxTicksLimit: 5,
                            padding: 10,
                            callback: function (value, index, values) {
                                return number_format(value);
                            }
                        },
                        gridLines: {
                            color: "rgb(234, 236, 244)",
                            zeroLineColor: "rgb(234, 236, 244)",
                            drawBorder: false,
                            borderDash: [2],
                            zeroLineBorderDash: [2]
                        }
                    }],
                },
                legend: {
                    display: false
                },
                tooltips: {
                    backgroundColor: "rgb(255,255,255)",
                    bodyFontColor: "#858796",
                    titleMarginBottom: 10,
                    titleFontColor: '#6e707e',
                    titleFontSize: 14,
                    borderColor: '#dddfeb',
                    borderWidth: 1,
                    xPadding: 15,
                    yPadding: 15,
                    displayColors: false,
                    intersect: false,
                    mode: 'index',
                    caretPadding: 10,
                    callbacks: {
                        label: function (tooltipItem, chart) {
                            var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                            return datasetLabel + ': ' + number_format(tooltipItem.yLabel);
                        }
                    }
                }
            }
        });
    }

});