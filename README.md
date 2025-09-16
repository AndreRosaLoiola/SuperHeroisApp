# SuperHeroisApp

Pré-requisitos

Node.js (versão 20 ou superior)
npm ou yarn
Quasar CLI (caso não tenha, instale com npm install -g @quasar/cli)
Instale as dependências

npm install
# ou
yarn

quasar dev
npx quasar dev


_______________________________________________________________________________________________________________________________________________

Este projeto é uma API RESTful desenvolvida em ASP.NET Core (.NET 8) para gerenciamento de super-heróis e seus superpoderes. O sistema permite cadastrar, listar, atualizar e remover heróis, além de associar múltiplos superpoderes a cada um deles.
Funcionalidades
•	Cadastro, edição, exclusão e consulta de super-heróis
•	Associação de superpoderes aos heróis
•	Listagem de todos os superpoderes disponíveis
•	Validação para evitar nomes de heróis duplicados
•	API documentada com Swagger
•	Testes unitários utilizando xUnit e banco de dados em memória
Tecnologias Utilizadas
•	ASP.NET Core (.NET 8)
•	Entity Framework Core (InMemory)
•	xUnit (testes unitários)
•	Swagger (documentação da API)
Como executar
1.	Clone o repositório
2.	Abra a solução no Visual Studio
3.	Execute o projeto principal (WebApplication1)
4.	Acesse a documentação da API em /swagger
5.	Para rodar os testes, execute o comando dotnet test no terminal ou utilize o Test Explorer do Visual Studio
Estrutura
•	WebApplication1: Projeto principal da API
•	TesteHerois ou TestProject1: Projeto de testes unitários
Observações
•	O banco de dados é em memória.
•	O projeto segue boas práticas de injeção de dependência e separação de responsabilidades.
