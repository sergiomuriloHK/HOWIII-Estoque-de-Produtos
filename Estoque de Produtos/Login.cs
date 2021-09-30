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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }




        //
        // [CLIQUE Botão Sair] 
        //
        private void BtnSair(object sender, EventArgs e)
        {
            //
            // Caixa de confirmação para sair do programa
            //
            if (MessageBox.Show("Tem certeza que deseja sair da aplicação?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        //
        // Funcionalidades do botão sair [MOUSE EM CIMA DO BOTAO]
        //
        private void BtnSairMouseHover(object sender, EventArgs e)
        {
            //
            // Muda a cor do botao minimizar para "vermelho"
            //
            Sair.BackColor = Color.FromArgb(189, 65, 58);
        }

        //
        // Funcionalidades do botão sair [MOUSE SAI DO BOTAO]
        //
        private void BtnSairMouseLeave(object sender, EventArgs e)
        {
            //
            // Muda a cor do botao minimizar para "Roxo"
            //
            Sair.BackColor = Color.FromArgb(24, 28, 112);
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
        // [CLIQUE] botão ENTRAR
        //
        private void BtnEntrar_Click(object sender, EventArgs e)
        {
            //
            // Analisa de campos usuário e senha estão preenchidos
            //
            if (txtUsuario.Text == "" || txtSenha.Text == "")
            {
                MessageBox.Show("Preencha os campos Usuário e Senha!", "ATENÇÃO");
                txtUsuario.Focus();
            }
            else
            {
                //
                // Realiza Conexão com o banco de dados
                //
                MySqlConnectionStringBuilder conexaoBD = ConexaoBancoDeDados();
                MySqlConnection realizaConexaoBD = new MySqlConnection(conexaoBD.ToString());

                try
                {
                    realizaConexaoBD.Open();

                    MySqlCommand comandoMySql = realizaConexaoBD.CreateCommand();
                    //
                    // Verifica se as credenciais das TextBox estão iguais as do banco de dados
                    // Usuário: PCAdmin
                    // Senha: PC123456
                    //
                    comandoMySql.CommandText = "SELECT * FROM credenciais WHERE crNome = '" + txtUsuario.Text + "' and crSenha = '" + txtSenha.Text +"'";
                    MySqlDataReader reader = comandoMySql.ExecuteReader();

                    if (reader.HasRows)
                    {
                        //
                        // Se o usuário digitou as credências corretamente o formulário Home será carregado
                        //
                        Home home = new Home();
                        home.Show();
                        this.Hide();
                    }
                    else
                    {
                        //
                        // Se credências estiverem incorretas usuário será informado
                        //
                        MessageBox.Show("Usuário ou senha inválidos!", "ATENÇÃO");
                        txtUsuario.Clear();
                        txtSenha.Clear();
                        txtUsuario.Focus();
                    }

                    realizaConexaoBD.Close();
                }catch (Exception ex)
                {
                    MessageBox.Show("Não foi possível acessar o banco de dados!", "ERRO");
                    Console.WriteLine(ex.Message);
                }
            }
        }




        private void Login_Load(object sender, EventArgs e)
        {
            txtUsuario.Focus();
        }
    }
}
