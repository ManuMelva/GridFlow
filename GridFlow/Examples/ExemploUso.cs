using System;
using System.Data;
using System.Windows.Forms;
using GridFlow.Controls;

namespace GridFlow.Examples
{
    /// <summary>
    /// Exemplo de uso do PaginatedDataGridView
    /// 
    /// INSTRUÇÕES PARA USAR:
    /// 1. Crie um novo Form no Visual Studio
    /// 2. Abra o designer e adicione um PaginatedDataGridView (deve estar na Toolbox)
    /// 3. Copie o código abaixo no evento Load do formulário
    /// </summary>
    public class ExemploUso
    {
        public static void ConfigurarPaginatedDataGridView(PaginatedDataGridView paginatedGrid)
        {
            // Configurar propriedades
            paginatedGrid.PageSize = 10;
            paginatedGrid.EnablePaginationUI = true;
            paginatedGrid.AllowPageSizeChange = true;

            // Subscrever aos eventos
            paginatedGrid.DataRequested += (sender, e) =>
            {
                PaginatedGrid_DataRequested(sender, e, paginatedGrid);
            };

            paginatedGrid.PageChanged += (sender, e) =>
            {
                Console.WriteLine($"Página alterada para: {e.CurrentPage}");
            };

            paginatedGrid.PageSizeChanged += (sender, e) =>
            {
                Console.WriteLine($"Tamanho de página alterado para: {paginatedGrid.PageSize}");
            };

            // Carregar primeira página
            paginatedGrid.LoadPage(1);
        }

        /// <summary>
        /// Evento disparado quando dados são solicitados pelo componente
        /// Aqui você deve implementar a lógica de carregamento server-side
        /// </summary>
        private static void PaginatedGrid_DataRequested(object sender, PaginationEventArgs e, PaginatedDataGridView paginatedGrid)
        {
            try
            {
                // Simular carregamento de dados do servidor
                DataTable dt = GetDataFromServer(e.CurrentPage, e.PageSize);

                // Atualizar o componente com os dados
                paginatedGrid.LoadData(dt);

                // Atualizar total de registros (geralmente vem do servidor)
                paginatedGrid.TotalRecords = 157; // Exemplo: total de registros no servidor
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Simula carregamento de dados do servidor
        /// Em uma aplicação real, isso faria uma chamada HTTP ou banco de dados
        /// </summary>
        private static DataTable GetDataFromServer(int pageNumber, int pageSize)
        {
            DataTable dt = new DataTable();

            // Criar colunas
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Nome", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Ativo", typeof(bool));

            // Simular dados (em produção, viriam do servidor)
            int startIndex = (pageNumber - 1) * pageSize + 1;
            for (int i = startIndex; i < startIndex + pageSize && i <= 157; i++)
            {
                dt.Rows.Add(i, $"Usuário {i}", $"usuario{i}@example.com", i % 2 == 0);
            }

            return dt;
        }
    }
}
