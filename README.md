# 🚀 STS Help - Sistema de Gestão de Chamados Corporativos

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET Core](https://img.shields.io/badge/.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![FlutterFlow](https://img.shields.io/badge/FlutterFlow-4285F4?style=for-the-badge&logo=flutter&logoColor=white)
![Supabase](https://img.shields.io/badge/Supabase-3ECF8E?style=for-the-badge&logo=supabase&logoColor=white)

## 📋 Sobre o Projeto

O **STS Help** é uma solução completa de Help Desk desenvolvida como Projeto de Conclusão de Curso (TCC). O objetivo foi criar um ecossistema que unisse uma arquitetura de backend robusta com uma interface mobile ágil para o usuário final.

O sistema resolve o problema de desorganização de demandas de TI, centralizando tickets em um banco de dados relacional e permitindo acompanhamento em tempo real.

---

## 📱 Live Demo (Módulo Mobile)

A interface de abertura de chamados para o usuário final foi desenvolvida utilizando **FlutterFlow**, priorizando a experiência mobile-first. Você pode testar a interface no link abaixo:

👉 **[Acessar App de Abertura de Chamados](https://stshelp-qypm6o.flutterflow.app/)**

---

## 🛠 Arquitetura e Tecnologias

O projeto foi construído utilizando uma arquitetura híbrida, separando a responsabilidade do Frontend do Cliente e do Painel Administrativo.

### 🔙 Backend & Painel Administrativo (Web)
* **Linguagem:** C#
* **Framework:** ASP.NET Core MVC
* **ORM:** Entity Framework Core
* **Estilização:** Bootstrap 5
* **Funcionalidades:** Gestão de SLA, atribuição de técnicos, trâmite de status e encerramento de tickets.

### 📱 Frontend Cliente (Mobile)
* **Plataforma:** FlutterFlow (Low-Code)
* **Foco:** Abertura rápida de tickets e consulta de histórico pelo usuário comum.

### 🗄 Banco de Dados
* **SGBD:** PostgreSQL
* **Hospedagem:** Supabase (Cloud)
* **Modelagem:** Estruturação relacional manual para garantir integridade referencial entre Usuários, Técnicos e Chamados.

---

## ✨ Funcionalidades Principais

1.  **Abertura de Chamados (Cross-Platform):** Integração via Web ou App FlutterFlow.
2.  **Fluxo de Atendimento:** Mudança de status (Aberto -> Em Análise -> Em Atendimento -> Concluído).
3.  **Gestão de Perfis:**
    * *Administrador:* Visão total do sistema.
    * *Técnico:* Visualiza e atende os chamados atribuídos.
    * *Usuário:* Abre chamados e acompanha o status.
4.  **Banco de Dados na Nuvem:** Conexão persistente com Supabase.

---

## 💻 Como Executar o Backend (Localmente)

Pré-requisitos: .NET SDK 6.0 ou superior.

```bash
# Clone este repositório
$ git clone [https://github.com/Gabrieljose0/STS_HELP.git](https://github.com/Gabrieljose0/STS_HELP.git)

# Acesse a pasta do projeto
$ cd STS_HELP

# Restaure as dependências
$ dotnet restore

# Configure a String de Conexão no appsettings.json com seu banco PostgreSQL
# "ConnectionStrings": { "DefaultConnection": "SuaStringDoSupabaseAqui" }

# Execute o projeto
$ dotnet run
