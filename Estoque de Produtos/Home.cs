using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estoque_de_Produtos
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }





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
        //Abre tela para cadastramento de categorias
        //
        private void BtnCategorias_Click(object sender, EventArgs e)
        {
            Categorias Cat = new Categorias();
            Cat.Show();
            this.Hide();
        }

        //
        // Volta para tela de Login
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
                // Muda a Cor do botao sair para "vermelho"
                //
                btnSair.BackColor = Color.FromArgb(189, 65, 58);
            }
            if (botaoEventos.Name == "btnMinimizar")
            {
                //
                // Muda a cor do botao minimizar para "roxo claro"
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
                // Muda a cor do botao sair para "Roxo"
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
    }
}
