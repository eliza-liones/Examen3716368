using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Examen3716368
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        System.Data.OleDb.OleDbConnection conBD = new System.Data.OleDb.OleDbConnection();

        String sAccion = "";

        public void abrirConexion()
        {
            conBD.ConnectionString = @"Provider=Microsoft.JET.OLEDB.4.0;  Data Source=C:\Users\estre\OneDrive\Escritorio\Practica3\crudExamen\examenBd.mdb";

            try
            {
                conBD.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexion " + ex);
            }
        }

        public void cerrarConexion()
        {
            if (conBD.State == ConnectionState.Open)
            {
                conBD.Close();
            }
        }


        private void btnMostrar_Click(object sender, EventArgs e)
        {
            if (double.TryParse(txtNota.Text, out double nota))
            {
                if (nota > 6)
                {
                    MessageBox.Show("Aprobado", "Resultado");
                }
                else
                {
                    MessageBox.Show("Reprobado", "Resultado");
                }
            }
            else
            {
                MessageBox.Show("Nota inválida", "Error");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            abrirConexion();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            cerrarConexion();

            this.Close();
        }

        private void desactivarAcciones()
        {
            btnConsulta.Enabled = false;
            btnAgregar.Enabled = false;
            btnEliminar.Enabled = false;
            btnModificar.Enabled = false;
            btnAceptar.Enabled = true;
            btnCancelar.Enabled = true;
        }

        private void activarAcciones()
        {
            btnConsulta.Enabled = true;
            btnAgregar.Enabled = true;
            btnEliminar.Enabled = true;
            btnModificar.Enabled = true;
            btnAceptar.Enabled = false;
            btnCancelar.Enabled = false;

            sAccion = "";
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            sAccion = "ConsultarR";

            desactivarAcciones();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            sAccion = "AgregarR";

            desactivarAcciones();

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            sAccion = "EliminarR";

            desactivarAcciones();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            sAccion = "ModificarR";

            desactivarAcciones();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            activarAcciones();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (sAccion == "ConsultarR")
            {
                try
                {
                    if (Convert.ToInt32(txtNie.Text) > 0)
                    {
                        string sConsulta = "SELECT * " + "FROM Registros " + "WHERE Nie= " + txtNie.Text;
                        OleDbCommand oleComando = new OleDbCommand(sConsulta, conBD);
                        OleDbDataReader oleReader = oleComando.ExecuteReader();

                        if (oleReader.Read())
                        {
                            txtNombre.Text = Convert.ToString(oleReader["Nombre"]);

                            txtNota.Text = Convert.ToString(oleReader["Nota"]);
                        }
                        else
                        {
                            MessageBox.Show("El codigo introducido no existe.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Introducir un codigo válido antes de consultar.");
                    }
                }
                catch
                {
                    MessageBox.Show("Introducir un codigo antes de consultar.");
                }
            }
            else
            {
                if (sAccion == "AgregarR")
                {
                    try
                    {
                        string sConsulta = "INSERT INTO Registros (Nombre, Nota) VALUES   (' " + txtNombre.Text + "', " + txtNota.Text + " )";

                        OleDbCommand oleComando = new OleDbCommand(sConsulta, conBD);
                        oleComando.CommandType = CommandType.Text;
                        oleComando.CommandText = sConsulta;
                        oleComando.Connection = conBD;

                        oleComando.ExecuteNonQuery();

                        txtNie.Text = "";
                        txtNombre.Text = "";
                        txtNota.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error en la inserccion" + ex.Message);
                    }
                }
                else
                {
                    if (sAccion == "EliminarR")
                    {
                        try
                        {
                            String sConsulta = "DELETE FROM Registros WHERE Nie = " + txtNie.Text;
                            OleDbCommand oleComando = new OleDbCommand();
                            oleComando.CommandType = CommandType.Text;
                            oleComando.CommandText = sConsulta;
                            oleComando.Connection = conBD;

                            oleComando.ExecuteNonQuery();
                            txtNie.Text = "";
                            txtNombre.Text = "";
                            txtNota.Text = "";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error en el borrado" + ex.Message);

                        }
                    }

                    else
                    {
                        try
                        {
                            String sConsulta = "UPDATE Registros SET Nombre = '" + txtNombre.Text + "' ,  Nota = '" + txtNota.Text + "' WHERE Nie = " + txtNie.Text;
                            OleDbCommand oleComando = new OleDbCommand();

                            oleComando.CommandType = CommandType.Text;
                            oleComando.CommandText = sConsulta;
                            oleComando.Connection = conBD;
                            oleComando.ExecuteNonQuery();

                            txtNie.Text = "";
                            txtNombre.Text = "";
                            txtNota.Text = "";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al modificar" + ex.Message);
                        }
                    }
                }
            }
            sAccion = "";

            activarAcciones();
        }
    }
}
