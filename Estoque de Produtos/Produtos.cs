using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Estoque_de_Produtos
{
    public partial class Produtos : Form
    {
        public Produtos()
        {
            InitializeComponent();
        }

        bool novoProduto, editarProduto = false;





        //
        // Carrega formulário de cadastramento de categorias
        //
        private void BtnCategorias_Click(object sender, EventArgs e)
        {
            Categorias Cat = new Categorias();
            Cat.Show();
            this.Hide();
        }

        //
        // Volta a tela de Login
        //
        private void BtnLogout_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }





        //
        // Funcionalidades dos botões minimizar e sair [CLIQUE]
        //
        private void BtnSairMinimizar(object sender, EventArgs e)
        {
            PictureBox botao = (PictureBox)sender;
            if (botao.Name == "btnSair")
            {
                //
                // Caixa de confirmação para sair do programa
                //
                if (MessageBox.Show("Tem certeza que deseja sair da aplicação?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            if (botao.Name == "btnMinimizar")
            {
                //
                // Minimiza a tela
                //
                this.WindowState = FormWindowState.Minimized;
            }
        }

        //
        // Funcionalidades dos botões minimizar e sair [MOUSE EM CIMA DO BOTAO]
        //
        private void BtnSairMinimizarMouseHover(object sender, EventArgs e)
        {
            PictureBox botaoEventos = (PictureBox)sender;
            if (botaoEventos.Name == "btnSair")
            {
                //
                // Muda a cor do botao minimizar para "Vermelho"
                //
                btnSair.BackColor = Color.FromArgb(189, 65, 58);
            }
            if (botaoEventos.Name == "btnMinimizar")
            {
                //
                // Muda a cor do botao minimizar para "Roxo claro"
                //
                btnMinimizar.BackColor = Color.FromArgb(58, 65, 189);
            }
        }

        //
        // Funcionalidades dos botões minimizar e sair [MOUSE SAI DO BOTAO]
        //
        private void BtnSairMinimizarMouseLeave(object sender, EventArgs e)
        {
            PictureBox botaoEventos = (PictureBox)sender;
            if (botaoEventos.Name == "btnSair")
            {
                //
                // Muda a cor do botao minimizar para "Roxo"
                //
                btnSair.BackColor = Color.FromArgb(24, 28, 112);
            }
            if (botaoEventos.Name == "btnMinimizar")
            {
                //
                // Muda a cor do botao minimizar para "Roxo"
                //
                btnMinimizar.BackColor = Color.FromArgb(24, 28, 112);
            }
        }





        //
        // Cria conexão com o banco de dados
        //
        private MySqlConnectionStringBuilder ConexaoBancoDeDados()
        {
            MySqlConnectionStringBuilder conexaoBD = new MySqlConnectionStringBuilder
            {
                Server = "sql10.freemysqlhosting.net",
                Database = "sql10436996",
                UserID = "sql10436996",
                Password = "xpdeifKZPZ",
                SslMode = 0
            };
            return conexaoBD;
        }





        //
        // Essa função serve para verificar se as TextBox Quantidade e Preço contêm dados numéricos...
        // ...esses campos não podem ser salvos com dados alfanuméricos
        //
        private void ConsistenciaDeDados()
        {
            try
            {
                //
                // Testa se TextBox Preco é convertível para "double"
                //
                double testePreco = Convert.ToDouble(txtPreco.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("O campo preço deve conter apenas números!", "ATENÇÃO");
                Console.WriteLine(ex.Message);
                throw;
            }
            try
            {
                //
                // Testa se TextBox Quantidade é convertível para "inteiro"
                //
                int testeQuantidade = Convert.ToInt32(txtQntd.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("O campo quantidade deve conter apenas números!", "ATENÇÃO");
                Console.WriteLine(ex.Message);
                throw;
            }
        }





        //
        // Funcionalidades dos botões "novo", "salvar", "editar", "excluir" e "cancelar" -- CRUD
        //
        private void BotoesCrud(object sender, EventArgs e)
        {
            Button botao = (Button)sender;

            //
            // [CLIQUE BOTAO SALVAR]
            //
            if (botao.Text == "Salvar")
            {
                //
                // Cadastra o novo produto
                //
                if (novoProduto)
                {
                    //
                    // Verifica se todos os campos estão preenchidos
                    //
                    if (txtNome.Text == "" || txtPreco.Text == "" || txtQntd.Text == "" || cbbCat.Text == "")
                    {
                        MessageBox.Show("Preencha todos os campos para cadastrar o produto!", "ATENÇÃO");
                    }
                    else
                    {
                        MySqlConnectionStringBuilder conexaoBD = ConexaoBancoDeDados();
                        MySqlConnection realizaConexaoBD = new MySqlConnection(conexaoBD.ToString());
                        
                        try
                        {
                            //
                            // Chama função consistenciadeDados
                            //
                            ConsistenciaDeDados();

                            realizaConexaoBD.Open();

                            MySqlCommand comandoMySql = realizaConexaoBD.CreateCommand();

                            //
                            // Insere um novo produto no banco de dados
                            //
                            comandoMySql.CommandText = "INSERT INTO produtos (prNome,prQuantidade,prCategoria,prPreco)" +
                                "VALUES('" + txtNome.Text + "', '" + Convert.ToInt64(txtQntd.Text) + "', '" + cbbCat.Text + "', '" + "R$ " + txtPreco.Text + "')";
                            comandoMySql.ExecuteNonQuery();

                            realizaConexaoBD.Close();
                            MessageBox.Show("Produto salvo com sucesso!");
                            AtualizarDgv();
                            btnCancelar.PerformClick();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            MessageBox.Show("Não foi possível salvar novo produto no banco de dados!", "ERRO");
                            btnCancelar.PerformClick();
                        }
                    }
                    
                }

                //
                // Altera as informações de um produto | "Editar"
                //
                if (editarProduto)
                {
                    if (txtNome.Text == "" || txtPreco.Text == "" || txtQntd.Text == "" || cbbCat.Text == "")
                    {
                        MessageBox.Show("Preencha todos os campos para cadastrar o produto!", "ATENÇÃO");
                    }
                    else
                    {
                        MySqlConnectionStringBuilder conexaoBD = ConexaoBancoDeDados();
                        MySqlConnection realizaConexaoBD = new MySqlConnection(conexaoBD.ToString());

                        try
                        {
                            //
                            // Chama função consistenciadeDados
                            //
                            ConsistenciaDeDados();

                            realizaConexaoBD.Open();

                            MySqlCommand comandoMySql = realizaConexaoBD.CreateCommand();
                            //
                            // Atualiza as informações do produto selecionado
                            //
                            comandoMySql.CommandText = "UPDATE produtos SET prNome = '" + txtNome.Text + "', " +
                                "prQuantidade = '" + Convert.ToInt64(txtQntd.Text) + "', " +
                                "prCategoria = '" + cbbCat.Text + "', " +
                                "prPreco = '" + "R$ " + txtPreco.Text + "'" +
                                "WHERE prId = " + labelID.Text + "";
                            comandoMySql.ExecuteNonQuery();

                            realizaConexaoBD.Close();
                            MessageBox.Show("Informações atualizadas com sucesso!");
                            AtualizarDgv();
                            btnCancelar.PerformClick();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            MessageBox.Show("Não foi possível alterar os dados do produto selecionado!", "ERRO");
                            btnCancelar.PerformClick();
                        }
                    }
                }
            }

            //
            // [CLIQUE BOTAO EXCLUIR]
            //
            if (botao.Text == "Excluir")
            {
                MySqlConnectionStringBuilder conexaoBD = ConexaoBancoDeDados();
                MySqlConnection realizaConexaoBD = new MySqlConnection(conexaoBD.ToString());
                try
                {
                    //
                    // Confirma a exlusão do produto selecionado
                    //
                    if (MessageBox.Show("Tem certeza que deseja excluir esse produto?", "CONFIRMAÇÃO", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        realizaConexaoBD.Open();

                        MySqlCommand comandoMySql = realizaConexaoBD.CreateCommand();

                        //
                        // Realiza a exclusão do produto do banco de dados
                        //
                        comandoMySql.CommandText = "DELETE FROM produtos WHERE prId = " + labelID.Text + "";
                        comandoMySql.ExecuteNonQuery();

                        realizaConexaoBD.Close();
                        MessageBox.Show("Produto deletado com sucesso!");
                        AtualizarDgv();
                        btnCancelar.PerformClick();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("Não foi possível deletar o produto selecionado!", "ERRO");
                }
            }

            //
            // [CLIQUE BOTAO EDITAR]
            //
            if (botao.Text == "Editar")
            {
                //
                // Habilita/Desabilita campos, textboxes, botões
                //
                editarProduto = true;
                btnEditar.Enabled = false;
                btnExcluir.Enabled = false;
                btnSalvar.Enabled = true;
                txtNome.Enabled = true;
                txtPreco.Enabled = true;
                txtQntd.Enabled = true;
                cbbCat.Enabled = true;
            }

            //
            // [CLIQUE BOTAO NOVO]
            //
            if (botao.Text == "Novo")
            {
                //
                // Habilita/Desabilita campos, textboxes, botões
                //
                novoProduto = true;
                btnNovo.Enabled = false;
                btnCancelar.Enabled = true;
                btnSalvar.Enabled = true;
                txtNome.Enabled = true;
                txtPreco.Enabled = true;
                txtQntd.Enabled = true;
                cbbCat.Enabled = true;
            }


        }

        //
        // [CLIQUE BOTAO CANCELAR]
        //
        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            //
            // Desabilita e Limpa campos, textboxes, botões
            //
            labelID.Text = string.Empty;
            txtNome.Clear();
            txtPreco.Clear();
            txtQntd.Clear();
            cbbCat.Text = string.Empty;
            btnNovo.Enabled = true;
            btnCancelar.Enabled = false;
            btnSalvar.Enabled = false;
            btnEditar.Enabled = false;
            btnExcluir.Enabled = false;
            txtNome.Enabled = false;
            txtPreco.Enabled = false;
            txtQntd.Enabled = false;
            cbbCat.Enabled = false;
            novoProduto = false;
        }





        //
        // Atualização da DataGridView
        // Se função for chamada com parâmetro "pesquisa" sendo verdadeiro...
        // ... uma pesquisa será realizada no banco de dados com o que está escrito na TextBox de Pesquisa
        //
        private void AtualizarDgv(bool pesquisa = false)
        {
            MySqlConnectionStringBuilder conexaoBD = ConexaoBancoDeDados();
            MySqlConnection realizaConexaoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexaoBD.Open();

                MySqlCommand comandoMySql = realizaConexaoBD.CreateCommand();
                if (pesquisa)
                {
                    //
                    // Comando SQL se parâmentro pesquisa == true
                    //
                    comandoMySql.CommandText = "SELECT * FROM produtos WHERE prNome LIKE '%" + txtPesquisa.Text + "%'";
                }
                else
                {
                    //
                    // Comando SQL se parâmentro pesquisa == false
                    //
                    comandoMySql.CommandText = "SELECT * FROM produtos";
                }
                MySqlDataReader reader = comandoMySql.ExecuteReader();

                dgvProdutos.Rows.Clear();

                while (reader.Read())
                {
                    DataGridViewRow row = (DataGridViewRow)dgvProdutos.Rows[0].Clone();
                    row.Cells[0].Value = reader.GetString(0);
                    row.Cells[1].Value = reader.GetString(1);
                    row.Cells[2].Value = reader.GetString(2);
                    row.Cells[3].Value = reader.GetString(3);
                    row.Cells[4].Value = reader.GetString(4);
                    dgvProdutos.Rows.Add(row);
                }

                realizaConexaoBD.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível carregar banco de dados!", "ERRO");
                Console.WriteLine(ex.Message);
            }
        }





        //
        // Realiza uma pesquisa no banco de dados
        //
        private void TxtPesquisa_TextChanged(object sender, EventArgs e)
        {
            // Chama a função atualizar datagrid com parâmetro pesquisa == true
            AtualizarDgv(true);
        }





        //
        // Seleciona os itens da DataGridView e passa para as TextBox
        //
        private void DgvProdutos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvProdutos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && !novoProduto)
            {
                dgvProdutos.CurrentRow.Selected = true;
                txtNome.Text = dgvProdutos.Rows[e.RowIndex].Cells["prNome"].FormattedValue.ToString();
                txtPreco.Text = dgvProdutos.Rows[e.RowIndex].Cells["prPreco"].FormattedValue.ToString().Replace("R$ ", "");
                txtQntd.Text = dgvProdutos.Rows[e.RowIndex].Cells["prQntd"].FormattedValue.ToString();
                cbbCat.Text = dgvProdutos.Rows[e.RowIndex].Cells["prCategoria"].FormattedValue.ToString();
                labelID.Text = dgvProdutos.Rows[e.RowIndex].Cells["prId"].FormattedValue.ToString();
                btnNovo.Enabled = false;
                btnCancelar.Enabled = true;
                btnEditar.Enabled = true;
                btnExcluir.Enabled = true;
            }
        }





        //
        // Carrega as categorias cadastradas no banco de dados na ComboBox categoria
        //
        private void CarregarCategorias()
        {
            MySqlConnectionStringBuilder conexaoBD = ConexaoBancoDeDados();
            MySqlConnection realizaConexaoBD = new MySqlConnection(conexaoBD.ToString());

            try
            {
                realizaConexaoBD.Open();

                MySqlCommand comandoMySql = realizaConexaoBD.CreateCommand();
                //
                // Seleciona a tabela categorias e ordena em ordem alfabética
                //
                comandoMySql.CommandText = "SELECT * FROM categorias ORDER BY ctNome";
                MySqlDataReader reader = comandoMySql.ExecuteReader();

                //
                // DataTable representa uma ou mais tabelas que permanecem alocadas em memória e pode ser manipulado através de métodos
                //
                DataTable tabelaCategorias = new DataTable();
                tabelaCategorias.Load(reader); // Preenche o DataTable com os dados da consulta
                DataRow linha = tabelaCategorias.NewRow(); // Linha de dados do DataTable
                linha["ctNome"] = ""; // Carrega uma linha vazia
                tabelaCategorias.Rows.InsertAt(linha, 0); // Insere linhas começando pelo índice 0
                cbbCat.DataSource = tabelaCategorias; // Carrega a DataTable na ComboBox de Categorias
                cbbCat.ValueMember = "ctId";
                cbbCat.DisplayMember = "ctNome";

                realizaConexaoBD.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível carregar as categorias do banco de dados!", "ERRO");
                Console.WriteLine(ex.Message);
            }
        }

        

        

        //
        // Atualiza a tabela ao carregar o formulário dos Produtos e carrega as categorias na ComboBox
        //
        private void Produtos_Load(object sender, EventArgs e)
        {
            AtualizarDgv();
            CarregarCategorias();
        }
    }
}
