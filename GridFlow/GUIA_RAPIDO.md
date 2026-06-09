# 📊 PaginatedDataGridView - Guia Rápido de Uso

## ✅ Arquivos Criados

```
GridFlow/
├── Controls/
│   ├── PaginationEventArgs.cs           ✅ EventArgs customizado
│   ├── PaginationState.cs                ✅ Gerenciador de estado
│   ├── PaginatedDataGridView.cs          ✅ Classe principal
│   └── PaginatedDataGridView.Designer.cs ✅ Componentes visuais
├── Examples/
│   └── ExemploUso.cs                     ✅ Exemplo de implementação
└── README_PaginatedDataGridView.md       ✅ Documentação completa
```

## 🚀 Como Começar em 3 Passos

### Passo 1: Adicionar à Toolbox
1. Abra seu formulário no Designer Visual Studio
2. Na Toolbox, procure por "PaginatedDataGridView"
3. Arraste e solte no seu formulário

### Passo 2: Configurar no Formulário
```csharp
private PaginatedDataGridView paginatedGrid;

private void Form_Load(object sender, EventArgs e)
{
	// Obter referência do componente do designer
	paginatedGrid = this.paginatedDataGridView1;

	// Configurar propriedades
	paginatedGrid.PageSize = 10;
	paginatedGrid.EnablePaginationUI = true;

	// Subscrever ao evento de carregamento de dados
	paginatedGrid.DataRequested += PaginatedGrid_DataRequested;

	// Carregar primeira página
	paginatedGrid.LoadPage(1);
}
```

### Passo 3: Implementar Carregamento de Dados
```csharp
private void PaginatedGrid_DataRequested(object sender, PaginationEventArgs e)
{
	// Buscar dados do servidor/banco de dados
	DataTable dados = BuscarDadosDoServidor(e.CurrentPage, e.PageSize);

	// Atualizar componente
	paginatedGrid.LoadData(dados);

	// Atualizar total de registros
	paginatedGrid.TotalRecords = ObterTotalDeRegistros();
}

private DataTable BuscarDadosDoServidor(int pagina, int tamanho)
{
	DataTable dt = new DataTable();
	dt.Columns.Add("ID", typeof(int));
	dt.Columns.Add("Nome", typeof(string));

	// Sua lógica de banco de dados aqui
	// Usar LIMIT/OFFSET ou equivalente

	return dt;
}
```

## 🎛️ Funcionalidades Principais

### Propriedades
```csharp
paginatedGrid.CurrentPage        // Página atual (somente leitura)
paginatedGrid.PageSize           // Registros por página (10, 20, 50, 100)
paginatedGrid.TotalRecords       // Total de registros
paginatedGrid.TotalPages         // Total de páginas (automático)
paginatedGrid.EnablePaginationUI // Mostrar/ocultar controles
paginatedGrid.AllowPageSizeChange// Permitir mudança de page size
paginatedGrid.DataGrid           // Acesso ao DataGridView interno
```

### Métodos
```csharp
paginatedGrid.LoadPage(2)        // Ir para página 2
paginatedGrid.LoadData(dataTable)// Carrega dados
paginatedGrid.RefreshCurrentPage()// Recarrega página atual
paginatedGrid.GoToFirstPage()    // Primeira página
paginatedGrid.GoToLastPage()     // Última página
paginatedGrid.GoToNextPage()     // Próxima página
paginatedGrid.GoToPreviousPage() // Página anterior
paginatedGrid.SetPageSize(20)    // Altera para 20 registros
```

### Eventos
```csharp
// Antes de mudar de página (pode cancelar)
paginatedGrid.PageChanging += (s, e) => 
{
	e.Cancel = true; // Cancela se necessário
};

// Após mudar de página
paginatedGrid.PageChanged += (s, e) => 
{
	Console.WriteLine($"Agora na página {e.CurrentPage}");
};

// Quando dados são solicitados
paginatedGrid.DataRequested += (s, e) =>
{
	var dados = BuscarDados(e.CurrentPage, e.PageSize);
	paginatedGrid.LoadData(dados);
};

// Quando page size é alterado
paginatedGrid.PageSizeChanged += (s, e) =>
{
	Console.WriteLine("Page size alterado");
};
```

## 🎨 Interface Visual

```
┌─────────────────────────────────────────────────────────┐
│                                                          │
│  ID │ Nome     │ Email          │ Ativo               │
│  ──────────────────────────────────────────────────     │
│  1  │ Usuário1 │ user1@email.com│ ✓                  │
│  2  │ Usuário2 │ user2@email.com│ ✗                  │
│  3  │ Usuário3 │ user3@email.com│ ✓                  │
│  ...                                                     │
│                                                          │
├─────────────────────────────────────────────────────────┤
│ |< < ┌──┐ Página 1 de 16 > >| Registros/pág: ┌────┐  │
│         │1 │                              │ 10 │  │
│         └──┘                              └────┘  │
│                          Total de Registros: 157   │
└─────────────────────────────────────────────────────────┘
```

## 💻 Exemplo Completo com API

```csharp
using System;
using System.Data;
using System.Windows.Forms;
using GridFlow.Controls;
using System.Net.Http;
using System.Threading.Tasks;

public partial class MinhaForm : Form
{
	private PaginatedDataGridView paginatedGrid;
	private HttpClient httpClient = new HttpClient();

	public MinhaForm()
	{
		InitializeComponent();
	}

	private void Form_Load(object sender, EventArgs e)
	{
		// Obter do designer ou criar programaticamente
		paginatedGrid = new PaginatedDataGridView();
		paginatedGrid.Dock = DockStyle.Fill;
		this.Controls.Add(paginatedGrid);

		// Configurar
		paginatedGrid.PageSize = 20;
		paginatedGrid.DataRequested += PaginatedGrid_DataRequested;

		// Carregar
		paginatedGrid.LoadPage(1);
	}

	private async void PaginatedGrid_DataRequested(object sender, PaginationEventArgs e)
	{
		try
		{
			// Chamar API com paginação
			string url = $"https://api.example.com/usuarios" +
						$"?page={e.CurrentPage}" +
						$"&pageSize={e.PageSize}";

			var response = await httpClient.GetAsync(url);
			var json = await response.Content.ReadAsStringAsync();

			// Parsear resposta (usando Newtonsoft.Json)
			dynamic resultado = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

			// Criar DataTable
			DataTable dt = new DataTable();
			dt.Columns.Add("ID", typeof(int));
			dt.Columns.Add("Nome", typeof(string));
			dt.Columns.Add("Email", typeof(string));

			// Preencher dados
			foreach (var item in resultado.dados)
			{
				dt.Rows.Add(item.id, item.nome, item.email);
			}

			// Atualizar grid
			paginatedGrid.LoadData(dt);
			paginatedGrid.TotalRecords = (int)resultado.total;
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Erro: {ex.Message}");
		}
	}
}
```

## ⚡ Dicas e Boas Práticas

✅ **Sempre atualizar TotalRecords** após carregar dados  
✅ **Usar apenas page sizes válidos**: 10, 20, 50, 100  
✅ **Implementar cancellamento** no evento PageChanging se necessário  
✅ **Usar async/await** para não bloquear UI em operações de rede  
✅ **Customizar DataGrid** através da propriedade DataGrid  
✅ **Validar dados** antes de chamar LoadData  

## 🔧 Troubleshooting

### Componente não aparece na Toolbox
- Reconstruir solução: `Rebuild Solution`
- Fechar e reabrir Visual Studio

### Erro "InitializeComponent does not exist"
- Este exemplo foi simplificado. Use o componente via drag-and-drop no Designer

### Grid vazio após LoadData
- Verificar se DataTable possui dados
- Verificar se TotalRecords foi atualizado
- Verificar console para exceptions

## 📞 Suporte

Consulte `README_PaginatedDataGridView.md` para documentação completa com mais exemplos!
