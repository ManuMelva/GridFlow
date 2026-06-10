# PaginationControl - Guia Rapido de Uso

## Arquivos Criados

```
GridFlow/
├── Controls/
│   ├── PaginationEventArgs.cs           EventArgs customizado
│   ├── PaginationState.cs                Gerenciador de estado
│   ├── PaginationControl.cs              Classe principal
│   └── PaginationControl.Designer.cs     Componentes visuais
├── Examples/
│   └── ExemploUso.cs                     Exemplo de implementacao
└── README.md                             Documentacao completa
```

## Como Comecar em 3 Passos

### Passo 1: Adicionar ao Formulario

No designer:
1. Adicione um **DataGridView** ao formulario (dock = Fill)
2. Adicione um **PaginationControl** ao formulario (dock = Bottom)

### Passo 2: Conectar Grid + Paginacao

```csharp
private void Form_Load(object sender, EventArgs e)
{
    paginationControl1.DataGrid = dataGridView1;
    paginationControl1.DataRequested += PaginationControl_DataRequested;
    paginationControl1.LoadPage(1);
}
```

Ou usar o helper:
```csharp
paginationControl1.ConfigureWithGrid(dataGridView1, this);
```

### Passo 3: Implementar Carregamento de Dados

```csharp
private void PaginationControl_DataRequested(object sender, PaginationEventArgs e)
{
    DataTable dados = BuscarDadosDoServidor(e.CurrentPage, e.PageSize);
    paginationControl1.LoadData(dados);
    paginationControl1.TotalRecords = ObterTotalDeRegistros();
}

private DataTable BuscarDadosDoServidor(int pagina, int tamanho)
{
    DataTable dt = new DataTable();
    dt.Columns.Add("ID", typeof(int));
    dt.Columns.Add("Nome", typeof(string));
    // Sua logica de banco de dados aqui (LIMIT/OFFSET)
    return dt;
}
```

## Funcionalidades Principais

### Propriedades
```csharp
pagination.CurrentPage        // Pagina atual (somente leitura)
pagination.PageSize           // Registros por pagina (10, 20, 50, 100)
pagination.TotalRecords       // Total de registros
pagination.TotalPages         // Total de paginas (automatico)
pagination.EnablePaginationUI // Mostrar/ocultar controles
pagination.AllowPageSizeChange// Permitir mudanca de page size
pagination.DataGrid           // Referencia ao DataGridView externo
```

### Metodos
```csharp
pagination.LoadPage(2)          // Ir para pagina 2
pagination.LoadData(dataTable)  // Carrega dados
pagination.RefreshCurrentPage() // Recarrega pagina atual
pagination.GoToFirstPage()      // Primeira pagina
pagination.GoToLastPage()       // Ultima pagina
pagination.GoToNextPage()       // Proxima pagina
pagination.GoToPreviousPage()   // Pagina anterior
pagination.SetPageSize(20)      // Altera para 20 registros
pagination.ConfigureWithGrid(grid, parent) // Helper de setup
```

### Eventos
```csharp
// Antes de mudar de pagina (pode cancelar)
pagination.PageChanging += (s, e) => { e.Cancel = true; };

// Apos dados carregados na nova pagina
pagination.PageChanged += (s, e) => {
    Console.WriteLine($"Agora na pagina {e.CurrentPage}");
};

// Quando dados sao solicitados
pagination.DataRequested += (s, e) => {
    var dados = BuscarDados(e.CurrentPage, e.PageSize);
    pagination.LoadData(dados);
};

// Quando page size e alterado
pagination.PageSizeChanged += (s, e) => {
    Console.WriteLine("Page size alterado");
};
```

## Interface Visual

```
<< [1] [2] [3] ... [10] [11] [12] >>   Mostrando 1-10 de 157   [10 v]

- << / <  : Primeira / Anterior
- [1]..[N]: Botoes numerados (pagina atual destacada em azul)
- ...     : Indicador de paginas ocultas
- > / >>  : Proxima / Ultima
- Info    : "Mostrando X-Y de Z"
- [10 v]  : Seletor de page size
```

- Design Fluent UI: botoes flat, cor #0078D4, hover #E5F3FF
- Transicao suave com "Carregando..." e botao em tom mais claro

## Dicas e Boas Praticas

- **Sempre atribuir `DataGrid`** antes de usar o componente
- **Sempre atualizar `TotalRecords`** apos carregar dados
- **Usar apenas page sizes validos**: 10, 20, 50, 100
- **Implementar cancelamento** no evento PageChanging se necessario
- **Usar async/await** para nao bloquear UI em operacoes de rede
- **Customizar DataGridView** diretamente, sem passar pelo componente

## Troubleshooting

### Componente nao aparece na Toolbox
- Reconstruir solucao: `Rebuild Solution`
- Fechar e reabrir Visual Studio

### Grid vazio apos LoadData
- Verificar se `DataGrid` property foi atribuida
- Verificar se DataTable possui dados
- Verificar se TotalRecords foi atualizado

### Botoes sempre desabilitados
- Verificar se TotalRecords esta sendo atualizado
- Verificar se o DataGridView externo existe

Consulte `README.md` para documentacao completa com mais exemplos!
