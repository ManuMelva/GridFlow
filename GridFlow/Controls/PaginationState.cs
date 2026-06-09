namespace GridFlow.Controls
{
    /// <summary>
    /// Classe para gerenciar o estado da paginação
    /// </summary>
    public class PaginationState
    {
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalRecords = 0;

        /// <summary>
        /// Página atual (começa em 1)
        /// </summary>
        public int CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value > 0 ? value : 1; }
        }

        /// <summary>
        /// Tamanho da página
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set 
            { 
                if (value == 10 || value == 20 || value == 50 || value == 100)
                    _pageSize = value;
            }
        }

        /// <summary>
        /// Total de registros
        /// </summary>
        public int TotalRecords
        {
            get { return _totalRecords; }
            set { _totalRecords = value > 0 ? value : 0; }
        }

        /// <summary>
        /// Total de páginas calculado automaticamente
        /// </summary>
        public int TotalPages
        {
            get
            {
                if (TotalRecords == 0)
                    return 1;

                return (TotalRecords + PageSize - 1) / PageSize; // Ceiling division
            }
        }

        /// <summary>
        /// Valida se o número da página é válido
        /// </summary>
        public bool IsValidPageNumber(int pageNumber)
        {
            return pageNumber >= 1 && pageNumber <= TotalPages;
        }

        /// <summary>
        /// Reseta a paginação para o estado inicial
        /// </summary>
        public void Reset()
        {
            _currentPage = 1;
            _pageSize = 10;
            _totalRecords = 0;
        }

        public override string ToString()
        {
            return $"Página {CurrentPage} de {TotalPages} | PageSize: {PageSize} | Total: {TotalRecords}";
        }
    }
}
