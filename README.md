# MicroAPI

Este é um projeto simples de **Client API** (semelhante a um Postman simplificado) desenvolvido em **WPF** com **.NET 8**, utilizando o padrão **MVVM** e **Injeção de Dependência**.

## Sobre o Código

> **Arquitetura MVVM (Model-View-ViewModel)**
> O projeto separa a interface gráfica (`MainWindow.xaml`) da lógica de negócios (`MainViewModel`), facilitando testes e manutenção.
> A comunicação entre a View e a ViewModel é feita através de **Data Binding** e **Commands** do pacote `CommunityToolkit.Mvvm`.

> **Injeção de Dependência (DI)**
> Utilizamos o `Microsoft.Extensions.Hosting` para configurar um contêiner de DI.
> O `App.xaml.cs` remove o fluxo padrão do WPF (`StartupUri`) e inicializa manualmente a janela, injetando o `HttpClientFactory` e a `MainViewModel` necessários.

> **Interface Gráfica (Material Design)**
> A UI é construída com o pacote `MaterialDesignThemes` para um visual moderno.
> Recursos como `Converters` (ex: `InverseBoolConverter`) são usados para controlar a visibilidade e habilitação de elementos dinamicamente (bloquear botão durante requisição).

> **Http Client**
> As requisições são feitas usando `IHttpClientFactory`, que é uma prática recomendada em .NET para gerenciar instâncias de `HttpClient` de forma eficiente, evitando exaustão de sockets.

### Funcionalidades

- Criar coleções, e nelas adicionar as requisições.
- Os principaís métodos de requisição estarão prontos para uso: GET, PUT, POST, DELETE.
- Scripts javascript Pre-request e Post-response e mostrar no console a saída.
- Tipos de autênticação: No Auth, Basic Auth e Bearer Token.
- Mostrar os códigos de status (200, 201, 401, …).
- Salvar o histórico numa pasta local

#### Requisitos Funcionais (RF)

- **RF01 - Gerenciamento de Coleções:** O sistema deve permitir a criação, edição e exclusão de coleções para agrupar requisições HTTP.
- **RF02 - Execução de Requisições HTTP Básicas:** O sistema deve permitir a configuração e o envio de requisições utilizando os métodos `GET`, `POST`, `PUT` e `DELETE`.
- **RF03 - Manipulação de Parâmetros e Headers:** O sistema deve permitir a inserção de parâmetros de URL (*Query Params*), cabeçalhos (*Headers*) e corpo da requisição (*Body*).
- **RF04 - Autenticação:** O sistema deve suportar os mecanismos de autenticação: *No Auth*, *Basic Auth* e *Bearer Token*.
- **RF05 - Execução de Scripts (Sandbox):** O sistema deve permitir a escrita e execução de scripts JavaScript antes da requisição (*Pre-request*) e após a resposta (*Post-response*).
- **RF06 - Console de Depuração:** O sistema deve exibir um console local para exibir as saídas (`console.log`) geradas pelos scripts JavaScript.
- **RF07 - Exibição de Resposta HTTP:** O sistema deve exibir o código de status HTTP (ex: 200, 401, 500), o tempo de resposta, o tamanho do payload e o corpo da resposta formatado em JSON.
- **RF08 - Persistência Local (Histórico):** O sistema deve salvar automaticamente o histórico de requisições executadas e as coleções criadas em um diretório local da máquina do usuário.

#### Requisitos Não Funcionais (RNF)

- **RNF01 - Portabilidade (Multiplataforma):** O aplicativo deve ser executado de forma nativa nos sistemas operacionais Windows e Linux.
- **RNF02 - Arquitetura Desconectada:** O sistema deve funcionar perfeitamente sem dependência de conexões externas ou servidores de terceiros para o seu funcionamento core, operando 100% localmente.
- **RNF03 - Desempenho e Leveza:** O consumo de memória RAM em repouso (*idle*) não deve exceder limites elevados (estabelecer um teto, ex: 150MB).
- **RNF04 - Interface Minimalista:** A interface do usuário deve seguir padrões visuais modernos.
- **RNF05 - Segurança na Execução de Scripts:** A engine que executará o JavaScript dos parâmetros *Pre-request* e *Post-response* deve rodar de forma isolada (*sandbox*), impedindo que scripts maliciosos acessem o sistema de arquivos do usuário além do escopo permitido pelo app.

### Instalação e Execução

Para rodar este projeto, você precisará do **.NET 8 SDK** instalado.

1. Clone o repositório:
```bash
git clone <url-do-repositorio>
cd MicroAPI
```

2. Instale as dependências:
> O projeto utiliza os seguintes pacotes NuGet:
```bash
dotnet add package MaterialDesignThemes
dotnet add package CommunityToolkit.Mvvm
dotnet add package Microsoft.Extensions.Hosting
dotnet add package Microsoft.Extensions.Http
```

3. Compile e execute o projeto:
```bash
dotnet build
dotnet run --project MicroAPI
```
