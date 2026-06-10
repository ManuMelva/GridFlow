# PaginationControl - Documentação Completa

## Visão Geral

O `PaginationControl` é um componente WinForms que adiciona paginação estilo web a qualquer `DataGridView`. Diferente de soluções que embutem o grid dentro do controle, o `PaginationControl` é uma barra de paginação independente que controla um `DataGridView` externo — sem conflitos com o designer do Visual Studio.

## Características Principais

- **Paginação Server-Side** — Carregamento de dados sob demanda via `DataRequested`
- **Page Sizes Fixos** — 10, 20, 50, 100 registros por página
- **Navegação estilo Web** — Botões de página numerados com truncamento (`1 2 3 ... 10 11 12`)
- **Transição Suave** — Feedback visual instantâneo ao navegar ("Carregando..." + botão destacado)
- **Design Fluent UI** — Flat buttons, cor de destaque `#0078D4`, hover `#E5F3FF`
- **Independência total** — O `DataGridView` fica fora do controle, sem problemas de designer
- **Thread-Safe** — Atualização segura de UI em operações assíncronas

## Estrutura do Componente

```
GridFlow.Controls/
├── PaginationEventArgs.cs      # EventArgs customizado
├── PaginationState.cs           # Gerenciador de estado
├── PaginationControl.cs         # Classe principal
└── PaginationControl.Designer.cs# Componentes visuais
```

## Como Usar

### 1. Adicionar ao Formulário (Designer)

1. Adicione um `DataGridView` ao seu formulário (docked Fill)
2. Adicione um `PaginationControl` ao seu formulário (docked Bottom)
3. No código, conecte os dois:

```csharp
using GridFlow.Controls;

public partial class MyForm : Form
{
    public MyForm()
    {
        InitializeComponent();

        paginationControl1.DataGrid = dataGridView1;
        paginationControl1.DataRequested += PaginationControl_DataRequested;
        paginationControl1.LoadPage(1);
    }
}
```

### 2. Ou usar o helper programático

```csharp
paginationControl1.ConfigureWithGrid(dataGridView1, this);
```

### 3. Implementar o Evento DataRequested

```csharp
private void PaginationControl_DataRequested(object sender, PaginationEventArgs e)
{
    try
    {
        DataTable data = GetDataFromServer(e.CurrentPage, e.PageSize);
        paginationControl1.LoadData(data);
        paginationControl1.TotalRecords = 157;
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Erro: {ex.Message}");
    }
}

private DataTable GetDataFromServer(int page, int pageSize)
{
    DataTable dt = new DataTable();
    dt.Columns.Add("ID", typeof(int));
    dt.Columns.Add("Nome", typeof(string));
    // ... adicionar mais colunas
    return dt;
}
```

## Propriedades Públicas

| Propriedade | Tipo | Descrição |
|-------------|------|-----------|
| `CurrentPage` | int | Página atual (somente leitura) |
| `PageSize` | int | Registros por página (10, 20, 50, 100) |
| `TotalRecords` | int | Total de registros na fonte de dados |
| `TotalPages` | int | Total de páginas (calculado automaticamente) |
| `EnablePaginationUI` | bool | Mostrar/ocultar controles de paginação |
| `AllowPageSizeChange` | bool | Permitir mudança de page size |
| `DataGrid` | DataGridView | Referência ao DataGridView externo |

## Métodos Públicos

```csharp
// Carregamento de Dados
void LoadData(DataTable data);           // Carrega dados no DataGridView externo
void LoadPage(int pageNumber);           // Carrega página específica
void RefreshCurrentPage();               // Recarrega página atual

// Navegação
void GoToFirstPage();                    // Vai para primeira página
void GoToLastPage();                     // Vai para última página
void GoToNextPage();                     // Próxima página
void GoToPreviousPage();                 // Página anterior

// Configuração
void SetPageSize(int size);              // Altera tamanho de página (10, 20, 50, 100)
```

## Eventos Customizados

### PageChanging
Disparado **ANTES** de mudar de página. Permite cancelar a mudança.

```csharp
paginationControl1.PageChanging += (sender, e) =>
{
    if (!ValidarPermissao())
        e.Cancel = true;
};
```

### PageChanged
Disparado **APÓS** os dados serem carregados na nova página.

```csharp
paginationControl1.PageChanged += (sender, e) =>
{
    Console.WriteLine($"Página alterada para: {e.CurrentPage}");
};
```

### DataRequested
Disparado quando o componente necessita de dados. Implemente aqui a lógica de busca.

```csharp
paginationControl1.DataRequested += (sender, e) =>
{
    var data = GetDataFromServer(e.CurrentPage, e.PageSize);
    paginationControl1.LoadData(data);
    paginationControl1.TotalRecords = ObterTotalDeRegistros();
};
```

### PageSizeChanged
Disparado quando o usuário altera o tamanho de página.

```csharp
paginationControl1.PageSizeChanged += (sender, e) =>
{
    Console.WriteLine($"Page size alterado para: {paginationControl1.PageSize}");
};
```

## Interface Visual

```
┌──────────────────────────────────────────────────────────────────┐
│  ID │ Nome     │ Email          │ Ativo                        │
│  ────────────────────────────────────────────────────────       │
│  1  │ Usuário1 │ user1@email.com│ ✓                            │
│  2  │ Usuário2 │ user2@email.com│ ✗                            │
│  3  │ Usuário3 │ user3@email.com│ ✓                            │
│  ...                                                             │
├──────────────────────────────────────────────────────────────────┤
│  « ‹ [1] [2] [3] ⋯ [10] [11] [12] › »   Mostrando 1-10 de 157  │
│                                          [10 ▼]                 │
└──────────────────────────────────────────────────────────────────┘
```

## Exemplo Completo com API

```csharp
using System;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using GridFlow.Controls;

public partial class MyForm : Form
{
    private PaginationControl pagination;
    private DataGridView dataGrid;
    private HttpClient httpClient = new HttpClient();

    public MyForm()
    {
        InitializeComponent();
        InitializePagination();
    }

    private void InitializePagination()
    {
        pagination.DataGrid = dataGrid;
        pagination.PageSize = 20;
        pagination.DataRequested += PaginationControl_DataRequested;
        pagination.LoadPage(1);
    }

    private async void PaginationControl_DataRequested(object sender, PaginationEventArgs e)
    {
        try
        {
            var response = await httpClient.GetAsync(
                $"https://api.example.com/users?page={e.CurrentPage}&pageSize={e.PageSize}"
            );

            var content = await response.Content.ReadAsStringAsync();
            var json = Newtonsoft.Json.Linq.JObject.Parse(content);

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Nome", typeof(string));
            dt.Columns.Add("Email", typeof(string));

            foreach (var item in json["data"])
            {
                dt.Rows.Add(item["id"], item["name"], item["email"]);
            }

            pagination.LoadData(dt);
            pagination.TotalRecords = (int)json["total"];
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao carregar dados: {ex.Message}");
        }
    }
}
```

## Detalhes Técnicos

### Thread-Safety
O componente utiliza `Invoke` automaticamente quando operações são feitas de threads diferentes:

```csharp
Task.Run(() =>
{
    pagination.TotalRecords = 1000;
    pagination.LoadData(myDataTable);
});
```

### Validação de Páginas
Páginas inválidas são automaticamente ignoradas:

```csharp
pagination.TotalRecords = 50;
pagination.PageSize = 10;
pagination.TotalPages; // Retorna 5

pagination.LoadPage(10); // Inválido — mensagem de aviso
```

## Troubleshooting

### Componente não aparece na Toolbox
- Reconstruir a solução: `Rebuild Solution`
- Limpar cache: Deletar pasta `bin` e `obj`
- Fechar e reabrir Visual Studio

### DataGridView vazio após LoadData
- Verificar se `DataGrid` property foi atribuída
- Verificar se DataTable possui dados
- Verificar se TotalRecords está configurado corretamente

### Botões desabilitados sempre
- Verificar se TotalRecords está sendo atualizado
- Validar se CurrentPage está entre 1 e TotalPages

## Notas Importantes

1. **DataGrid property** — Sempre atribuir o `DataGridView` externo via `DataGrid` ou `ConfigureWithGrid()`
2. **Total de Registros** — Sempre atualizar `TotalRecords` após carregar dados
3. **Page Size** — Apenas os valores 10, 20, 50, 100 são válidos
4. **Eventos Assíncronos** — Use `await` em `DataRequested` para operações assíncronas
5. **Cancelamento** — Defina `e.Cancel = true` em `PageChanging` para cancelar navegação
