# PaginatedDataGridView - Documentação Completa

## 📋 Visão Geral

O `PaginatedDataGridView` é um componente WinForms customizado que herda de `UserControl` e fornece uma experiência completa de paginação server-side com suporte a DataGridView.

## 🎯 Características Principais

✅ **Paginação Server-Side** - Carregamento de dados sob demanda  
✅ **Page Sizes Flexíveis** - Suporte para 10, 20, 50, 100 registros por página  
✅ **Navegação Intuitiva** - Botões: Primeira, Anterior, Próxima, Última  
✅ **Informações de Paginação** - Exibe "Página X de Y" e Total de Registros  
✅ **TextBox de Navegação** - Digite o número da página e pressione Enter  
✅ **ComboBox de Page Size** - Mude o tamanho de página dinamicamente  
✅ **Eventos Customizados** - Controle total sobre o carregamento de dados  
✅ **Thread-Safe** - Atualização segura de UI em operações assíncronas  

## 📦 Estrutura do Componente

```
GridFlow.Controls/
├── PaginationEventArgs.cs      # EventArgs customizado
├── PaginationState.cs           # Gerenciador de estado
├── PaginatedDataGridView.cs     # Classe principal
└── PaginatedDataGridView.Designer.cs  # Componentes visuais
```

## 🚀 Como Usar

### 1. Adicionar o Componente ao Formulário

```csharp
using GridFlow.Controls;

public partial class MyForm : Form
{
	private PaginatedDataGridView paginatedGrid;

	public MyForm()
	{
		InitializeComponent();

		// Criar instância
		paginatedGrid = new PaginatedDataGridView();
		paginatedGrid.Dock = DockStyle.Fill;

		// Configurar propriedades
		paginatedGrid.PageSize = 10;
		paginatedGrid.EnablePaginationUI = true;
		paginatedGrid.AllowPageSizeChange = true;

		// Subscrever aos eventos
		paginatedGrid.DataRequested += PaginatedGrid_DataRequested;

		this.Controls.Add(paginatedGrid);

		// Carregar primeira página
		paginatedGrid.LoadPage(1);
	}
}
```

### 2. Implementar o Evento DataRequested

```csharp
private void PaginatedGrid_DataRequested(object sender, PaginationEventArgs e)
{
	try
	{
		// e.CurrentPage - Página solicitada
		// e.PageSize - Quantidade de registros por página
		// e.TotalRecords - Total de registros (você deve atualizar)

		// Simular chamada ao servidor
		DataTable data = GetDataFromServer(e.CurrentPage, e.PageSize);

		// Carregar dados no componente
		paginatedGrid.LoadData(data);

		// Atualizar total de registros (obtido do servidor)
		paginatedGrid.TotalRecords = 157; // Total de registros na base de dados
	}
	catch (Exception ex)
	{
		MessageBox.Show($"Erro: {ex.Message}");
	}
}

private DataTable GetDataFromServer(int page, int pageSize)
{
	// Implementar lógica de carregamento do servidor/BD
	DataTable dt = new DataTable();
	dt.Columns.Add("ID", typeof(int));
	dt.Columns.Add("Nome", typeof(string));
	// ... adicionar mais colunas conforme necessário

	return dt;
}
```

## 📋 Propriedades Públicas

| Propriedade | Tipo | Descrição |
|-------------|------|-----------|
| `CurrentPage` | int | Página atual (somente leitura) |
| `PageSize` | int | Registros por página (10, 20, 50, 100) |
| `TotalRecords` | int | Total de registros na fonte de dados |
| `TotalPages` | int | Total de páginas (calculado automaticamente) |
| `EnablePaginationUI` | bool | Mostrar/ocultar controles de paginação |
| `AllowPageSizeChange` | bool | Permitir mudança de page size |
| `DataGrid` | DataGridView | Acesso ao DataGridView interno |

## 🎛️ Métodos Públicos

```csharp
// Carregamento de Dados
void LoadData(DataTable data);           // Carrega dados na grid
void LoadPage(int pageNumber);           // Carrega página específica
void RefreshCurrentPage();               // Recarrega página atual

// Navegação
void GoToFirstPage();                    // Vai para primeira página
void GoToLastPage();                     // Vai para última página
void GoToNextPage();                     // Próxima página
void GoToPreviousPage();                 // Página anterior

// Configuração
void SetPageSize(int size);              // Altera tamanho de página
```

## 📡 Eventos Customizados

### PageChanging
Disparado **ANTES** de mudar de página. Permite cancelar a mudança.

```csharp
paginatedGrid.PageChanging += (sender, e) =>
{
	// e.CurrentPage - página de destino
	// e.PageSize - tamanho da página
	// e.Cancel - defina como true para cancelar

	if (ValidarPermissao())
		e.Cancel = false;
	else
		e.Cancel = true; // Cancela navegação
};
```

### PageChanged
Disparado **APÓS** mudar de página com sucesso.

```csharp
paginatedGrid.PageChanged += (sender, e) =>
{
	Console.WriteLine($"Página alterada para: {e.CurrentPage}");
};
```

### DataRequested
Disparado quando o componente necessita de dados.

```csharp
paginatedGrid.DataRequested += (sender, e) =>
{
	// Implementar lógica de carregamento server-side
	var data = GetDataFromServer(e.CurrentPage, e.PageSize);
	paginatedGrid.LoadData(data);
	paginatedGrid.TotalRecords = ObterTotalDeRegistros();
};
```

### PageSizeChanged
Disparado quando o usuário altera o tamanho de página.

```csharp
paginatedGrid.PageSizeChanged += (sender, e) =>
{
	Console.WriteLine($"Page size alterado para: {paginatedGrid.PageSize}");
};
```

## 🎨 Customização Visual

### Acessar DataGridView Interno

```csharp
DataGridView grid = paginatedGrid.DataGrid;
grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
```

### Personalizar Aparência

```csharp
// Ocultar controles de paginação
paginatedGrid.EnablePaginationUI = false;

// Desabilitar mudança de page size
paginatedGrid.AllowPageSizeChange = false;

// Ajustar altura do componente
paginatedGrid.Height = 600;
```

## 💡 Exemplo Completo com API

```csharp
using System;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using GridFlow.Controls;
using Newtonsoft.Json.Linq;

public partial class MyForm : Form
{
	private PaginatedDataGridView paginatedGrid;
	private HttpClient httpClient = new HttpClient();

	public MyForm()
	{
		InitializeComponent();
		InitializePaginatedGrid();
	}

	private void InitializePaginatedGrid()
	{
		paginatedGrid = new PaginatedDataGridView();
		paginatedGrid.Dock = DockStyle.Fill;
		paginatedGrid.PageSize = 20;
		paginatedGrid.DataRequested += PaginatedGrid_DataRequested;
		this.Controls.Add(paginatedGrid);

		paginatedGrid.LoadPage(1);
	}

	private async void PaginatedGrid_DataRequested(object sender, PaginationEventArgs e)
	{
		try
		{
			// Chamar API com paginação
			var response = await httpClient.GetAsync(
				$"https://api.example.com/users?page={e.CurrentPage}&pageSize={e.PageSize}"
			);

			var content = await response.Content.ReadAsStringAsync();
			var json = JObject.Parse(content);

			// Parsear dados
			DataTable dt = new DataTable();
			dt.Columns.Add("ID", typeof(int));
			dt.Columns.Add("Nome", typeof(string));
			dt.Columns.Add("Email", typeof(string));

			foreach (var item in json["data"])
			{
				dt.Rows.Add(
					item["id"],
					item["name"],
					item["email"]
				);
			}

			paginatedGrid.LoadData(dt);
			paginatedGrid.TotalRecords = (int)json["total"];
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Erro ao carregar dados: {ex.Message}");
		}
	}
}
```

## ⚙️ Detalhes Técnicos

### Thread-Safety
O componente utiliza `Invoke` automaticamente quando operações são feitas de threads diferentes:

```csharp
// Seguro chamar de qualquer thread
Task.Run(() =>
{
	paginatedGrid.TotalRecords = 1000;
	paginatedGrid.LoadData(myDataTable);
});
```

### Validação de Páginas
Páginas inválidas são automaticamente ignoradas:

```csharp
paginatedGrid.TotalRecords = 50;
paginatedGrid.PageSize = 10;
paginatedGrid.TotalPages; // Retorna 5

paginatedGrid.LoadPage(10); // Inválido - página não existe
// Mensagem de aviso é exibida automaticamente
```

## 🐛 Troubleshooting

### Componente não aparece na Toolbox
- Reconstruir a solução: `Rebuild Solution`
- Limpar cache: Deletar pasta `bin` e `obj`
- Fechar e reabrir Visual Studio

### DataGridView vazio após LoadData
- Verificar se DataTable possui dados
- Verificar se TotalRecords está configurado corretamente
- Validar se o evento DataRequested está sendo disparado

### Botões desabilitados sempre
- Verificar se TotalRecords está sendo atualizado
- Validar se CurrentPage está entre 1 e TotalPages

## 📝 Notas Importantes

1. **Total de Registros**: Sempre atualizar `TotalRecords` após carregar dados
2. **Page Size**: Apenas os valores 10, 20, 50, 100 são válidos
3. **Eventos Assíncronos**: Use `await` em `DataRequested` para operações assíncronas
4. **Cancelamento**: Defina `e.Cancel = true` em `PageChanging` para cancelar navegação

## 📞 Suporte

Para dúvidas ou problemas, consulte os exemplos em `GridFlow.Examples.FormTesteDataGridPaginado`
