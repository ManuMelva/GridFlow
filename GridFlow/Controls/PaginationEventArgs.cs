using System;

namespace GridFlow.Controls
{
    /// <summary>
    /// EventArgs customizado para eventos de paginação
    /// </summary>
    public class PaginationEventArgs : EventArgs
    {
        /// <summary>
        /// Número da página atual
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Tamanho da página (quantidade de registros por página)
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total de registros
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Total de páginas
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Flag para indicar se o evento deve ser cancelado
        /// </summary>
        public bool Cancel { get; set; }

        public PaginationEventArgs()
        {
            Cancel = false;
        }

        public PaginationEventArgs(int currentPage, int pageSize, int totalRecords, int totalPages)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = totalPages;
            Cancel = false;
        }
    }
}
