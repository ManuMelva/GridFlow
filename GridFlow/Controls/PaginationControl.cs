using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace GridFlow.Controls
{
    public partial class PaginationControl : UserControl
    {
        public delegate void PaginationEventHandler(object sender, PaginationEventArgs e);

        public event PaginationEventHandler PageChanging;
        public event PaginationEventHandler PageChanged;
        public event EventHandler PageSizeChanged;
        public event PaginationEventHandler DataRequested;

        private readonly PaginationState _paginationState;
        private bool _isLoading;
        private DataGridView _dataGrid;

        private static readonly Color AccentColor = Color.FromArgb(0x00, 0x78, 0xD4);
        private static readonly Color LoadingAccentColor = Color.FromArgb(0x5B, 0xA3, 0xE6);
        private static readonly Color HoverColor = Color.FromArgb(0xE5, 0xF3, 0xFF);
        private static readonly Color TextColor = Color.FromArgb(0x32, 0x31, 0x30);
        private static readonly Color DisabledTextColor = Color.FromArgb(0xA1, 0x9F, 0x9D);

        [Browsable(true)]
        [Category("Paginação")]
        [Description("Página atual")]
        public int CurrentPage
        {
            get => _paginationState.CurrentPage;
            private set => _paginationState.CurrentPage = value;
        }

        [Browsable(true)]
        [Category("Paginação")]
        [Description("Quantidade de registros por página")]
        [DefaultValue(10)]
        public int PageSize
        {
            get => _paginationState.PageSize;
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

        [Browsable(true)]
        [Category("Paginação")]
        [Description("Total de registros")]
        [DefaultValue(0)]
        public int TotalRecords
        {
            get => _paginationState.TotalRecords;
            set
            {
                _paginationState.TotalRecords = value;
                if (!_isLoading)
                    UpdatePaginationUI();
            }
        }

        [Browsable(false)]
        public int TotalPages => _paginationState.TotalPages;

        [Browsable(true)]
        [Category("Paginação")]
        [Description("Mostrar ou ocultar controles de paginação")]
        [DefaultValue(true)]
        public bool EnablePaginationUI
        {
            get => paginationPanel.Visible;
            set => paginationPanel.Visible = value;
        }

        [Browsable(true)]
        [Category("Paginação")]
        [Description("Permitir mudança de tamanho de página")]
        [DefaultValue(true)]
        public bool AllowPageSizeChange
        {
            get => cmbPageSize.Enabled;
            set => cmbPageSize.Enabled = value;
        }

        [Browsable(false)]
        public DataGridView DataGrid
        {
            get => _dataGrid;
            set => _dataGrid = value;
        }

        public PaginationControl()
        {
            _paginationState = new PaginationState();
            InitializeComponent();
            SetupNavigationButtons();
            UpdatePaginationUI();
        }

        public void LoadData<T>(List<T> data)
        {
            if (_dataGrid == null)
                return;

            try
            {
                _dataGrid.DataSource = data;
                _isLoading = false;
                UpdatePaginationUI();
            }
            catch (Exception ex)
            {
                _isLoading = false;
                MessageBox.Show(
                    $"Erro ao carregar dados: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public void LoadData(DataTable data)
        {
            if (_dataGrid == null)
                return;

            try
            {
                _dataGrid.DataSource = data;
                _isLoading = false;
                UpdatePaginationUI();
            }
            catch (Exception ex)
            {
                _isLoading = false;
                MessageBox.Show(
                    $"Erro ao carregar dados: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public void LoadPage(int pageNumber)
        {
            if (_isLoading)
                return;

            if (!_paginationState.IsValidPageNumber(pageNumber) && pageNumber != 1)
            {
                MessageBox.Show(
                    "Número de página inválido",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var args = new PaginationEventArgs(
                pageNumber, PageSize, TotalRecords, TotalPages);

            OnPageChanging(args);

            if (args.Cancel)
                return;

            _isLoading = true;
            CurrentPage = pageNumber;

            ShowLoadingState();

            if (DataRequested == null)
            {
                _isLoading = false;
                UpdatePaginationUI();
            }
            else
            {
                OnDataRequested(args);
            }
        }

        public void RefreshCurrentPage()
        {
            LoadPage(CurrentPage);
        }

        public void GoToFirstPage()
        {
            LoadPage(1);
        }

        public void GoToLastPage()
        {
            LoadPage(TotalPages);
        }

        public void GoToNextPage()
        {
            if (CurrentPage < TotalPages)
                LoadPage(CurrentPage + 1);
        }

        public void GoToPreviousPage()
        {
            if (CurrentPage > 1)
                LoadPage(CurrentPage - 1);
        }

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

        public void ConfigureWithGrid(DataGridView grid, Control parentContainer)
        {
            _dataGrid = grid;
            grid.Dock = DockStyle.Fill;
            Dock = DockStyle.Bottom;
            parentContainer.Controls.Add(grid);
            if (parentContainer != Parent)
            {
                parentContainer.Controls.Add(this);
            }
        }

        protected virtual void OnPageChanging(PaginationEventArgs e)
        {
            PageChanging?.Invoke(this, e);
        }

        protected virtual void OnPageChanged(PaginationEventArgs e)
        {
            PageChanged?.Invoke(this, e);
        }

        protected virtual void OnDataRequested(PaginationEventArgs e)
        {
            DataRequested?.Invoke(this, e);
        }

        protected virtual void OnPageSizeChanged()
        {
            _paginationState.CurrentPage = 1;
            LoadPage(1);
            PageSizeChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ShowLoadingState()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ShowLoadingState));
                return;
            }

            lblInfo.Text = "Carregando...";
            RebuildPageButtons(isLoading: true);
            EnableDisableNavigationButtons();
        }

        private void UpdatePaginationUI()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdatePaginationUI));
                return;
            }

            int start = (CurrentPage - 1) * PageSize + 1;
            int end = Math.Min(CurrentPage * PageSize, TotalRecords);
            int total = TotalRecords;

            if (total == 0)
            {
                lblInfo.Text = "Nenhum registro";
            }
            else
            {
                lblInfo.Text = $"Mostrando {start}-{end} de {total}";
            }

            RebuildPageButtons(isLoading: false);
            EnableDisableNavigationButtons();

            OnPageChanged(new PaginationEventArgs(
                CurrentPage, PageSize, TotalRecords, TotalPages));
        }

        private void RebuildPageButtons(bool isLoading)
        {
            pnlPageNumbers.Controls.Clear();

            int total = TotalPages;
            int current = CurrentPage;

            var items = GetPageItems(total, current);

            foreach (var item in items)
            {
                if (item.IsEllipsis)
                {
                    var lbl = new Label
                    {
                        Text = "\u22EF",
                        Font = new Font("Segoe UI", 9F),
                        ForeColor = DisabledTextColor,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Size = new Size(20, 24),
                        Margin = new Padding(0),
                        AutoSize = false
                    };
                    pnlPageNumbers.Controls.Add(lbl);
                }
                else
                {
                    var btn = new Button
                    {
                        Text = item.PageNumber.ToString(),
                        Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                        FlatStyle = FlatStyle.Flat,
                        Size = new Size(28, 24),
                        Margin = new Padding(1, 0, 1, 0),
                        UseVisualStyleBackColor = false,
                        TabStop = false,
                        Cursor = Cursors.Hand
                    };
                    btn.FlatAppearance.BorderSize = 0;

                    bool isCurrent = item.PageNumber == current;
                    if (isCurrent)
                    {
                        btn.BackColor = isLoading ? LoadingAccentColor : AccentColor;
                        btn.ForeColor = Color.White;
                        btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                    }
                    else
                    {
                        btn.BackColor = Color.Transparent;
                        btn.ForeColor = TextColor;
                        btn.FlatAppearance.MouseOverBackColor = HoverColor;
                    }

                    int page = item.PageNumber;
                    btn.Click += (s, e) => LoadPage(page);
                    pnlPageNumbers.Controls.Add(btn);
                }
            }
        }

        private struct PageButtonItem
        {
            public int PageNumber { get; }
            public bool IsEllipsis { get; }

            public PageButtonItem(int pageNumber, bool isEllipsis)
            {
                PageNumber = pageNumber;
                IsEllipsis = isEllipsis;
            }
        }

        private static List<PageButtonItem> GetPageItems(int totalPages, int currentPage)
        {
            var items = new List<PageButtonItem>();

            if (totalPages <= 7)
            {
                for (int i = 1; i <= totalPages; i++)
                    items.Add(new PageButtonItem(i, false));
                return items;
            }

            items.Add(new PageButtonItem(1, false));

            if (currentPage > 4)
                items.Add(new PageButtonItem(0, true));

            int start = Math.Max(2, currentPage - 2);
            int end = Math.Min(totalPages - 1, currentPage + 2);

            for (int i = start; i <= end; i++)
                items.Add(new PageButtonItem(i, false));

            if (currentPage < totalPages - 3)
                items.Add(new PageButtonItem(0, true));

            items.Add(new PageButtonItem(totalPages, false));

            return items;
        }

        private void EnableDisableNavigationButtons()
        {
            bool canGoBack = !_isLoading && CurrentPage > 1;
            bool canGoForward = !_isLoading && CurrentPage < TotalPages;

            btnFirst.Enabled = canGoBack;
            btnPrev.Enabled = canGoBack;
            btnNext.Enabled = canGoForward;
            btnLast.Enabled = canGoForward;
        }

        private void SetupNavigationButtons()
        {
            SetupNavButton(btnFirst);
            SetupNavButton(btnPrev);
            SetupNavButton(btnNext);
            SetupNavButton(btnLast);
        }

        private static void SetupNavButton(Button btn)
        {
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = Color.Transparent;
            btn.ForeColor = AccentColor;
            btn.FlatAppearance.MouseOverBackColor = HoverColor;
            btn.Cursor = Cursors.Hand;
        }

        private void BtnFirst_Click(object sender, EventArgs e)
        {
            GoToFirstPage();
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            GoToPreviousPage();
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            GoToNextPage();
        }

        private void BtnLast_Click(object sender, EventArgs e)
        {
            GoToLastPage();
        }

        private void CmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPageSize.SelectedItem != null
                && int.TryParse(cmbPageSize.SelectedItem.ToString(), out int newSize))
            {
                SetPageSize(newSize);
            }
        }
    }
}
