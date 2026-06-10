using System;
using System.Data;
using System.Windows.Forms;
using GridFlow.Controls;

namespace GridFlow.Examples
{
    public class ExemploUso
    {
        public static void ConfigurarPaginationControl(
            PaginationControl pagination, DataGridView grid)
        {
            pagination.DataGrid = grid;

            pagination.DataRequested += (sender, e) =>
            {
                PaginationControl_DataRequested(sender, e, pagination);
            };

            pagination.PageChanged += (sender, e) =>
            {
                Console.WriteLine($"Página alterada para: {e.CurrentPage}");
            };

            pagination.PageSizeChanged += (sender, e) =>
            {
                Console.WriteLine($"Tamanho de página alterado para: {pagination.PageSize}");
            };

            pagination.LoadPage(1);
        }

        private static void PaginationControl_DataRequested(
            object sender, PaginationEventArgs e, PaginationControl pagination)
        {
            try
            {
                DataTable dt = GetDataFromServer(e.CurrentPage, e.PageSize);

                pagination.LoadData(dt);

                pagination.TotalRecords = 157;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao carregar dados: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static DataTable GetDataFromServer(int pageNumber, int pageSize)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Nome", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Ativo", typeof(bool));

            int startIndex = (pageNumber - 1) * pageSize + 1;
            for (int i = startIndex; i < startIndex + pageSize && i <= 157; i++)
            {
                dt.Rows.Add(i, $"Usuário {i}", $"usuario{i}@example.com", i % 2 == 0);
            }

            return dt;
        }
    }
}
