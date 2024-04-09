using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace prlLocadora
{
    public partial class frmFilmes : Form
    {
        String connectionString = @"Server=DARNASSUS\MOTORHEAD;Database=db_230776;User Id=230776;Password=@166116";
        int registroAtual = 0;
        int totalregistros = 0;
        bool novo;
        DataTable dtFilmes = new DataTable();
        DataTable dtProdutoras = new DataTable();

        private void navegar()
        {
            carregaComboProdutoras();
            txtCodFilme.Text = dtFilmes.Rows[registroAtual][0].ToString();
            txtTituloFilme.Text = dtFilmes.Rows[registroAtual][1].ToString();
            txtAno.Text = dtFilmes.Rows[registroAtual][2].ToString();
            //cbbProdutora.Text = dtFilmes.Rows[registroAtual][3].ToString();
            cbbGenero.Text = dtFilmes.Rows[registroAtual][4].ToString();
        }

        private void carregaTudoProdutoras()
        {
            dtProdutoras = new DataTable();
            string sql = "SELECT * FROM  tblProdutora ";
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = System.Data.CommandType.Text;
            SqlDataReader reader;
            con.Open();
            try
            {
                using (reader = cmd.ExecuteReader())
                {
                    dtProdutoras.Load(reader);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.ToString());
            }
            finally
            {
                con.Close();
            }
            cbbProdutora.DataSource = dtProdutoras;
            cbbProdutora.DisplayMember = "nomeProd";
            cbbProdutora.ValueMember = "codProd";
        }

        private void carregaComboProdutoras()
        {
            dtProdutoras = new DataTable();
            string sql = "SELECT * FROM  tblProdutora WHERE codProd=" +
                 dtFilmes.Rows[registroAtual][3].ToString();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = System.Data.CommandType.Text;
            SqlDataReader reader;
            con.Open();
            try
            {
                using (reader = cmd.ExecuteReader())
                {
                    dtProdutoras.Load(reader);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.ToString());
            }
            finally
            {
                con.Close();
            }
            cbbProdutora.DataSource = dtProdutoras;
            cbbProdutora.DisplayMember = "nomeProd";
            cbbProdutora.ValueMember = "codProd";
        }

        public frmFilmes()
        {
            InitializeComponent();
        }

        private void frmFilmes_Load(object sender, EventArgs e)
        {
            btnSalvar.Enabled = false;
            txtCodFilme.Enabled = false;
            txtTituloFilme.Enabled = false;
            txtAno.Enabled = false;
            cbbProdutora.Enabled = false;
            cbbGenero.Enabled = false;
            string sql = "SELECT * FROM tblFilme";
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = System.Data.CommandType.Text;
            SqlDataReader reader;
            con.Open();
            try
            {
                using (reader = cmd.ExecuteReader())
                {
                    dtFilmes.Load(reader);
                    totalregistros = dtFilmes.Rows.Count;
                    registroAtual = 0;
                    navegar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        private void btnProximo_Click(object sender, EventArgs e)
        {
            if (registroAtual < totalregistros - 1)
            {
                registroAtual++;
                navegar();
            }
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (registroAtual > 0)
            {
                registroAtual--;
                navegar();
            }
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            if (registroAtual < totalregistros - 1)
            {
                registroAtual = totalregistros - 1;
                navegar();
            }
        }

        private void btnPrimeiro_Click(object sender, EventArgs e)
        {
            if (registroAtual > 0)
            {
                registroAtual = 0;
                navegar();
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            string sql = "DELETE FROM tblfilme WHERE codFilme=" + txtCodFilme.Text;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = System.Data.CommandType.Text;
            con.Open();

            try
            {
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    MessageBox.Show("Filme apagado com sucesso!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro" + ex.ToString());
            }

            finally
            {
                con.Close();

            }
            dtFilmes = new DataTable();
            this.frmFilmes_Load(this, e);


        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            novo = false;
            txtTituloFilme.Enabled = true;
            txtAno.Enabled = true;
            cbbGenero.Enabled = true;
            cbbProdutora.Enabled = true;
            btnSalvar.Enabled = true;
            btnNovo.Enabled = false;
            btnPrimeiro.Enabled = false;
            btnProximo.Enabled = false;
            btnUltimo.Enabled = false;
            btnAnterior.Enabled = false;
            btnExcluir.Enabled = false;
            carregaTudoProdutoras();
            btnAlterar.Enabled = false;
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            novo = true;
            txtTituloFilme.Clear();
            txtAno.Enabled = true;
            cbbGenero.Enabled = true;
            cbbProdutora.SelectedIndex = 0;
            txtAno.Clear();
            btnSalvar.Enabled = true;
            btnNovo.Enabled = false;
            btnPrimeiro.Enabled = false;
            btnProximo.Enabled = false;
            btnUltimo.Enabled = false;
            btnAnterior.Enabled = false;
            btnExcluir.Enabled = false;
            carregaTudoProdutoras();
            cbbProdutora.SelectedIndex = 0;
            btnAlterar.Enabled = false;
            txtCodFilme.Clear();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (novo)
            {
                string sql = "INSERT INTO tblFilme(tituloFilme, " +
                    "anoFilme, generoFilme, codProd) VALUES ('" + txtTituloFilme.Text + "'," + txtAno.Text +
                    ",'" + cbbProdutora.SelectedItem.ToString() +
                    ", '" + cbbGenero.Text + "')";

                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader;
                con.Open();
                try
                {
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        MessageBox.Show("Filme cadastrado com sucesso");
                        this.frmFilmes_Load(this, e);
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex); ToString();
                }

                finally
                {
                    con.Close();
                }

            }

            else
            {

            }
        }
    }
}
