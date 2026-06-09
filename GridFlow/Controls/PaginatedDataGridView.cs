using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace GridFlow.Controls
{
    /// <summary>
    /// DataGridView customizado com suporte a paginação server-side
    /// </summary>
    [ToolboxBitmap(typeof(DataGridView))]
    public partial class PaginatedDataGridView : UserControl
    {
        #region Delegates e Eventos

        /// <summary>
        /// Delegate para eventos de paginação
        /// </summary>
        public delegate void PaginationEventHandler(object sender, PaginationEventArgs e);

        /// <summary>
        /// Evento disparado antes de mudar de página
        /// </summary>
        public event PaginationEventHandler PageChanging;

        /// <summary>
        /// Evento disparado após mudar de página
        /// </summary>
        public event PaginationEventHandler PageChanged;

        /// <summary>
        /// Evento disparado quando o tamanho da página é alterado
        /// </summary>
        public event EventHandler PageSizeChanged;

        /// <summary>
        /// Evento disparado quando dados são solicitados (para server-side loading)
        /// </summary>
        public event PaginationEventHandler DataRequested;

        #endregion

        #region Fields

        private readonly PaginationState _paginationState;
        private bool _isLoading = false;
        private readonly int[] _availablePageSizes = { 10, 20, 50, 100 };

        #endregion

        #region Propriedades Públicas

        /// <summary>
        /// Página atual (somente leitura durante carregamento)
        /// </summary>
        [Browsable(true)]
        [Category("Paginação")]
        [Description("Página atual da paginação")]
        public int CurrentPage
        {
            get { return _paginationState.CurrentPage; }
            private set { _paginationState.CurrentPage = value; }
        }

        /// <summary>
        /// Tamanho da página (quantidade de registros por página)
        /// </summary>
        [Browsable(true)]
        [Category("Paginação")]
        [Description("Quantidade de registros por página")]
        [DefaultValue(10)]
        public int PageSize
        {
            get { return _paginationState.PageSize; }
            set
            {
                if (_paginationState.PageSize != value)
                {
                    _paginationState.PageSize = value;
                    cmbPageSize.SelectedItem = value;
                    OnPageSizeChanged();
                }
            }
        }

        /// <summary>
        /// Total de registros
        /// </summary>
        [Browsable(true)]
        [Category("Paginação")]
        [Description("Total de registros")]
        [DefaultValue(0)]
        public int TotalRecords
        {
            get { return _paginationState.TotalRecords; }
            set
            {
                _paginationState.TotalRecords = value;
                UpdatePaginationUI();
            }
        }

        /// <summary>
        /// Total de páginas (calculado automaticamente)
        /// </summary>
        [Browsable(false)]
        public int TotalPages
        {
            get { return _paginationState.TotalPages; }
        }

        /// <summary>
        /// Permite mostrar ou ocultar os controles de paginação
        /// </summary>
        [Browsable(true)]
        [Category("Paginação")]
        [Description("Mostrar ou ocultar controles de paginação")]
        [DefaultValue(true)]
        public bool EnablePaginationUI
        {
            get { return paginationPanel.Visible; }
            set { paginationPanel.Visible = value; }
        }

        /// <summary>
        /// Permite habilitar ou desabilitar a mudança de tamanho de página
        /// </summary>
        [Browsable(true)]
        [Category("Paginação")]
        [Description("Permitir mudança de tamanho de página")]
        [DefaultValue(true)]
        public bool AllowPageSizeChange
        {
            get { return cmbPageSize.Enabled; }
            set { cmbPageSize.Enabled = value; }
        }

        /// <summary>
        /// DataGridView interno para exibição de dados
        /// </summary>
        [Browsable(false)]
        public DataGridView DataGrid
        {
            get { return dataGridViewContent; }
        }

        #endregion

        #region Construtor e Inicialização

        public PaginatedDataGridView()
        {
            _paginationState = new PaginationState();
            InitializeComponent();
        }

        #endregion

        #region Métodos de Carregamento de Dados

        /// <summary>
        /// Carrega os dados de uma DataTable
        /// </summary>
        public void LoadData(DataTable data)
        {
            try
            {
                if (data == null)
                {
                    dataGridViewContent.DataSource = null;
                    return;
                }

                dataGridViewContent.DataSource = data;
                _isLoading = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Carrega uma página específica
        /// </summary>
        public void LoadPage(int pageNumber)
        {
            if (_isLoading)
                return;

            if (!_paginationState.IsValidPageNumber(pageNumber) && pageNumber != 1)
            {
                MessageBox.Show("Número de página inválido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Criar e disparar evento de PageChanging
            PaginationEventArgs args = new PaginationEventArgs(
                pageNumber,
                PageSize,
                TotalRecords,
                TotalPages
            );

            OnPageChanging(args);

            if (args.Cancel)
                return;

            _isLoading = true;
            CurrentPage = pageNumber;

            // Disparar evento DataRequested para o desenvolvedor carregar dados
            OnDataRequested(args);

            UpdatePaginationUI();
        }

        /// <summary>
        /// Recarrega a página atual
        /// </summary>
        public void RefreshCurrentPage()
        {
            LoadPage(CurrentPage);
        }

        #endregion

        #region Métodos de Navegação

        /// <summary>
        /// Navega para a primeira página
        /// </summary>
        public void GoToFirstPage()
        {
            LoadPage(1);
        }

        /// <summary>
        /// Navega para a última página
        /// </summary>
        public void GoToLastPage()
        {
            LoadPage(TotalPages);
        }

        /// <summary>
        /// Navega para a próxima página
        /// </summary>
        public void GoToNextPage()
        {
            if (CurrentPage < TotalPages)
                LoadPage(CurrentPage + 1);
        }

        /// <summary>
        /// Navega para a página anterior
        /// </summary>
        public void GoToPreviousPage()
        {
            if (CurrentPage > 1)
                LoadPage(CurrentPage - 1);
        }

        #endregion

        #region Métodos de Alteração de Page Size

        /// <summary>
        /// Altera o tamanho da página e recarrega
        /// </summary>
        public void SetPageSize(int size)
        {
            if (size == 10 || size == 20 || size == 50 || size == 100)
            {
                _paginationState.PageSize = size;
                _paginationState.CurrentPage = 1;
                cmbPageSize.SelectedItem = size;
                OnPageSizeChanged();
            }
        }

        #endregion

        #region Métodos Protegidos para Disparar Eventos

        /// <summary>
        /// Dispara o evento PageChanging
        /// </summary>
        protected virtual void OnPageChanging(PaginationEventArgs e)
        {
            PageChanging?.Invoke(this, e);
        }

        /// <summary>
        /// Dispara o evento PageChanged
        /// </summary>
        protected virtual void OnPageChanged(PaginationEventArgs e)
        {
            PageChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Dispara o evento PageSizeChanged
        /// </summary>
        protected virtual void OnPageSizeChanged()
        {
            _paginationState.CurrentPage = 1;
            LoadPage(1);
            PageSizeChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Dispara o evento DataRequested
        /// </summary>
        protected virtual void OnDataRequested(PaginationEventArgs e)
        {
            DataRequested?.Invoke(this, e);
        }

        #endregion

        #region Métodos Privados de UI

        /// <summary>
        /// Atualiza o estado da UI de paginação
        /// </summary>
        private void UpdatePaginationUI()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdatePaginationUI));
                return;
            }

            // Atualizar labels
            lblPageInfo.Text = $"Página {CurrentPage} de {TotalPages}";
            lblTotalRecords.Text = $"Total de Registros: {TotalRecords}";
            txtPageNumber.Text = CurrentPage.ToString();

            // Atualizar estado dos botões
            EnableDisableNavigationButtons();
        }

        /// <summary>
        /// Habilita ou desabilita os botões de navegação baseado no estado
        /// </summary>
        private void EnableDisableNavigationButtons()
        {
            btnFirstPage.Enabled = CurrentPage > 1;
            btnPreviousPage.Enabled = CurrentPage > 1;
            btnNextPage.Enabled = CurrentPage < TotalPages;
            btnLastPage.Enabled = CurrentPage < TotalPages;
        }

        #endregion

        #region Event Handlers

        private void BtnFirstPage_Click(object sender, EventArgs e)
        {
            GoToFirstPage();
        }

        private void BtnPreviousPage_Click(object sender, EventArgs e)
        {
            GoToPreviousPage();
        }

        private void BtnNextPage_Click(object sender, EventArgs e)
        {
            GoToNextPage();
        }

        private void BtnLastPage_Click(object sender, EventArgs e)
        {
            GoToLastPage();
        }

        private void TxtPageNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;

                if (int.TryParse(txtPageNumber.Text, out int pageNumber))
                {
                    LoadPage(pageNumber);
                }
                else
                {
                    MessageBox.Show("Digite um número de página válido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPageNumber.Text = CurrentPage.ToString();
                }
            }
        }

        private void CmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPageSize.SelectedItem != null)
            {
                if (int.TryParse(cmbPageSize.SelectedItem.ToString(), out int newSize))
                {
                    SetPageSize(newSize);
                }
            }
        }

        #endregion
    }
}
