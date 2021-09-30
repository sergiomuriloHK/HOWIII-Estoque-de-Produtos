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
    public partial class Categorias : Form
    {
        public Categorias()
        {
            InitializeComponent();
        }

        bool novaCategoria, editarCategoria = false;





        //
        // Abre tela para cadastramento de produtos
        //
        private void BtnProdutos_Click(object sender, EventArgs e)
        {
            Produtos Prd = new Produtos();
            Prd.Show();
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
                // Minimiza  a tela
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
                // Muda a cor do botao minimizar para "vermelho"
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
                // Cadastra nova categoria
                //
                if (novaCategoria)
                {
                    //
                    // Verifica se campo nome não está em branco
                    //
                    if (txtCategorias.Text == "")
                    {
                        MessageBox.Show("Preencha o campo nome para cadastrar uma nova categoria!", "ATENÇÃO");
                    }
                    else
                    {
                        MySqlConnectionStringBuilder conexaoBD = ConexaoBancoDeDados();
                        MySqlConnection realizaConexaoBD = new MySqlConnection(conexaoBD.ToString());
                        try
                        {
                            realizaConexaoBD.Open();

                            MySqlCommand comandoMySql = realizaConexaoBD.CreateCommand();

                            //
                            // Insere uma nova categoria no banco de dados
                            //
                            comandoMySql.CommandText = "INSERT INTO categorias (ctNome) VALUES('" + txtCategorias.Text + "')";
                            comandoMySql.ExecuteNonQuery();

                            realizaConexaoBD.Close();
                            MessageBox.Show("Categoria salva com sucesso!");
                            AtualizarDgv();
                            btnCancelar.PerformClick();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Não foi possível salvar nova categoria no banco de dados!", "ERRO");
                            Console.WriteLine(ex.Message);
                            btnCancelar.PerformClick();
                        }
                    }
                }

                //
                // Altera as informações de um produto | "Editar"
                //
                if (editarCategoria)
                {
                    if (txtCategorias.Text == "")
                    {
                        MessageBox.Show("Preencha o campo nome para cadastrar uma nova categoria!", "ATENÇÃO");
                    }
                    else
                    {
                        MySqlConnectionStringBuilder conexaoBD = ConexaoBancoDeDados();
                        MySqlConnection realizaConexaoBD = new MySqlConnection(conexaoBD.ToString());

                        try
                        {
                            realizaConexaoBD.Open();

                            MySqlCommand comandoMySql = realizaConexaoBD.CreateCommand();
                            //
                            // Atualiza as informações da categoria selecionada
                            //
                            comandoMySql.CommandText = "UPDATE categorias SET ctNome = '" + txtCategorias.Text + "'" +
                                "WHERE ctId = " + labelID.Text + "";
                            comandoMySql.ExecuteNonQuery();

                            realizaConexaoBD.Close();
                            MessageBox.Show("Informações atualizadas com sucesso!");
                            AtualizarDgv();
                            btnCancelar.PerformClick();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            MessageBox.Show("Não foi possível salvar nova categoria no banco de dados!", "ERRO");
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
                    // Confirma a exlusão da categoria selecionada
                    //
                    if (MessageBox.Show("Tem certeza que deseja excluir essa categoria?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        realizaConexaoBD.Open();

                        MySqlCommand comandoMySql = realizaConexaoBD.CreateCommand();

                        //
                        // Realiza a exclusão da categoria do banco de dados
                        //
                        comandoMySql.CommandText = "DELETE FROM categorias WHERE ctId = " + labelID.Text + "";
                        comandoMySql.ExecuteNonQuery();

                        realizaConexaoBD.Close();
                        MessageBox.Show("Categoria deletada com sucesso!");
                        AtualizarDgv();
                        btnCancelar.PerformClick();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("Não foi possível deletar a categoria selecionada!", "ERRO");
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
                editarCategoria = true;
                btnEditar.Enabled = false;
                btnExcluir.Enabled = false;
                btnSalvar.Enabled = true;
                txtCategorias.Enabled = true;
            }

            //
            // [CLIQUE BOTAO NOVO]
            //
            if (botao.Text == "Novo")
            {
                //
                // Habilita/Desabilita campos, textboxes, botões
                //
                novaCategoria = true;
                btnNovo.Enabled = false;
                btnCancelar.Enabled = true;
                btnSalvar.Enabled = true;
                txtCategorias.Enabled = true;
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
            txtCategorias.Clear();
            btnNovo.Enabled = true;
            btnCancelar.Enabled = false;
            btnSalvar.Enabled = false;
            btnEditar.Enabled = false;
            btnExcluir.Enabled = false;
            txtCategorias.Enabled = false;
            novaCategoria = editarCategoria = false;
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
                    comandoMySql.CommandText = "SELECT * FROM categorias WHERE ctNome LIKE '%" + txtPesquisa.Text + "%'";
                }
                else
                {
                    //
                    // Comando SQL se parâmentro pesquisa == false
                    //
                    comandoMySql.CommandText = "SELECT * FROM categorias";
                }
                MySqlDataReader reader = comandoMySql.ExecuteReader();

                dgvCategorias.Rows.Clear();

                while (reader.Read())
                {
                    DataGridViewRow row = (DataGridViewRow)dgvCategorias.Rows[0].Clone();
                    row.Cells[0].Value = reader.GetString(0);
                    row.Cells[1].Value = reader.GetString(1);
                    dgvCategorias.Rows.Add(row);
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
        private void DgvCategorias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCategorias.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && !novaCategoria)
            {
                dgvCategorias.CurrentRow.Selected = true;
                txtCategorias.Text = dgvCategorias.Rows[e.RowIndex].Cells["ctNome"].FormattedValue.ToString();
                labelID.Text = dgvCategorias.Rows[e.RowIndex].Cells["ctId"].FormattedValue.ToString();
                btnNovo.Enabled = false;
                btnCancelar.Enabled = true;
                btnEditar.Enabled = true;
                btnExcluir.Enabled = true;
            }
        }

        

        

        //
        // Atualiza a tabela ao carregar o formulário das Categorias
        //
        private void Categorias_Load(object sender, EventArgs e)
        {
            AtualizarDgv();
        }
    }
}
