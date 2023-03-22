using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO;
using System.Data;

namespace VitroCore
{
    public class LocalDatabase : IDisposable
    {
        private SQLiteConnection connection;
        private bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public void CreateDatabase(string dbpath)
        {
            if (!File.Exists(dbpath))
            {
                SQLiteConnection.CreateFile(dbpath);
            }
            connection = new SQLiteConnection($"Data Source={dbpath};Version=3");
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS Account(AccountId TEXT PRIMARY KEY, UserName TEXT, FingerPrint TEXT, Pais TEXT); CREATE TABLE IF NOT EXISTS Producto(ProductoId TEXT PRIMERY KEY, SAP TEXT, NAGS TEXT, Alto REAL, Ancho REAL, Boton INTEGER, Clasificacion TEXT, Color TEXT, Descripcion TEXT, EndYear INTEGER, Holder INTEGER, Homologo INTEGER, Marca TEXT, Mercado TEXT, Modelo TEXT, Moldura INTEGER, Perforacion REAL, Procedencia TEXT, Red INTEGER, SensorCondensacion INTEGER, SensorLluvia INTEGER, Serigrafia INTEGER, StartYear INTEGER, TipoParte TEXT, TipoVidrio TEXT, Imagen BLOB); CREATE TABLE IF NOT EXISTS Pais(PaisId TEXT PRIMARY KEY, Nombre TEXT); CREATE TABLE IF NOT EXISTS Marca(MarcaId TEXT PRIMARY KEY, Nombre TEXT, PaisId TEXT); CREATE TABLE IF NOT EXISTS Modelo(ModeloId TEXT PRIMARY KEY, Nombre TEXT, MarcaId TEXT);", connection);
            command.ExecuteNonQuery();
        }

        public void ClearTables()
        {
            SQLiteCommand command = new SQLiteCommand("DELETE FROM Account; DELETE FROM Producto; DELETE FROM Pais; DELETE FROM Marca; DELETE FROM Modelo;", connection);
            command.ExecuteNonQuery();
        }

        public void SaveAccount(List<AccountExportModel> model)
        {
            foreach (var account in model)
            {
                SQLiteCommand command = new SQLiteCommand("INSERT INTO Account(AccountId,UserName,FingerPrint,Pais) VALUES(@ID,@USERNAME,@FINGERPRINT,@PAIS)", connection);
                command.Parameters.Add(new SQLiteParameter("@ID", Guid.NewGuid().ToString()));
                command.Parameters.Add(new SQLiteParameter("@USERNAME", account.UserName));
                command.Parameters.Add(new SQLiteParameter("@FINGERPRINT", account.FingerPrint));
                command.Parameters.Add(new SQLiteParameter("@PAIS", account.Pais));
                System.Diagnostics.Debug.WriteLine(command.CommandText);
                command.ExecuteNonQuery();
            }
        }

        public void SaveProductos(List<ProdExportModel> model)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    foreach (var producto in model)
                    {
                        command.CommandText = "INSERT INTO Producto(ProductoId,SAP,NAGS,Alto,Ancho,Boton,Clasificacion,Color,Descripcion,EndYear,Holder,Homologo,Marca,Mercado,Modelo,Moldura,Perforacion,Procedencia,Red,SensorCondensacion,SensorLluvia,Serigrafia,StartYear,TipoParte,TipoVidrio,Imagen) VALUES(@ID,@SAP,@NAGS,@ALTO,@ANCHO,@BOTON,@CLASIFICACION,@COLOR,@DESCRIPCION,@ENDYEAR,@HOLDER,@HOMOLOGO,@MARCA,@MERCADO,@MODELO,@MOLDURA,@PERFORACION,@PROCEDENCIA,@RED,@SENSORCONDENSACION,@SENSORLLUVIA,@SERIGRAFIA,@STARTYEAR,@TIPOPARTE,@TIPOVIDRIO,@IMAGEN)";
                        command.Parameters.Add(new SQLiteParameter("@ID", Guid.NewGuid().ToString()));
                        command.Parameters.Add(new SQLiteParameter("@SAP", producto.SAP));
                        command.Parameters.Add(new SQLiteParameter("@NAGS", producto.NAGS));
                        command.Parameters.Add(new SQLiteParameter("@ALTO", producto.Alto));
                        command.Parameters.Add(new SQLiteParameter("@ANCHO", producto.Ancho));
                        command.Parameters.Add(new SQLiteParameter("@BOTON", producto.Boton));
                        command.Parameters.Add(new SQLiteParameter("@CLASIFICACION", producto.Clasificacion));
                        command.Parameters.Add(new SQLiteParameter("@COLOR", producto.Color));
                        command.Parameters.Add(new SQLiteParameter("@DESCRIPCION", producto.Descripcion));
                        command.Parameters.Add(new SQLiteParameter("@ENDYEAR", producto.EndYear));
                        command.Parameters.Add(new SQLiteParameter("@HOLDER", producto.Holder));
                        command.Parameters.Add(new SQLiteParameter("@HOMOLOGO", producto.Homologo));
                        command.Parameters.Add(new SQLiteParameter("@MARCA", producto.Marca));
                        command.Parameters.Add(new SQLiteParameter("@MERCADO", producto.Mercado));
                        command.Parameters.Add(new SQLiteParameter("@MODELO", producto.Modelo));
                        command.Parameters.Add(new SQLiteParameter("@MOLDURA", producto.Moldura));
                        command.Parameters.Add(new SQLiteParameter("@PERFORACION", producto.Perforacion));
                        command.Parameters.Add(new SQLiteParameter("@PROCEDENCIA", producto.Procedencia));
                        command.Parameters.Add(new SQLiteParameter("@RED", producto.Red));
                        command.Parameters.Add(new SQLiteParameter("@SENSORCONDENSACION", producto.SensorCondensacion));
                        command.Parameters.Add(new SQLiteParameter("@SENSORLLUVIA", producto.SensorLluvia));
                        command.Parameters.Add(new SQLiteParameter("@SERIGRAFIA", producto.Serigrafia));
                        command.Parameters.Add(new SQLiteParameter("@STARTYEAR", producto.StartYear));
                        command.Parameters.Add(new SQLiteParameter("@TIPOPARTE", producto.TipoParte));
                        command.Parameters.Add(new SQLiteParameter("@TIPOVIDRIO", producto.TipoVidrio));
                        command.Parameters.Add(new SQLiteParameter("@IMAGEN", DbType.Binary, producto.Imagen.Length));
                        command.Parameters["@IMAGEN"].Value = producto.Imagen;
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }

        public void SavePais(List<PaisExportModel> model)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    foreach (var pais in model)
                    {
                        command.CommandText = "INSERT INTO Pais(PaisId,Nombre) VALUES(@ID,@NOMBRE)";
                        command.Parameters.Add(new SQLiteParameter("@ID", pais.PaisId));
                        command.Parameters.Add(new SQLiteParameter("@NOMBRE", pais.Nombre));
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }

        public void SaveMarca(List<MarcaExportModel> model)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    foreach (var marca in model)
                    {
                        command.CommandText = "INSERT INTO Marca(MarcaId,Nombre,PaisId) VALUES(@ID,@NOMBRE,@PAIS)";
                        command.Parameters.Add(new SQLiteParameter("@ID", marca.MarcaId));
                        command.Parameters.Add(new SQLiteParameter("@NOMBRE", marca.Nombre));
                        command.Parameters.Add(new SQLiteParameter("@PAIS", marca.PaisId));
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }

        public void SaveModelo(List<ModeloExportModel> model)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    foreach (var modelo in model)
                    {
                        command.CommandText = "INSERT INTO Modelo(ModeloId,Nombre,MarcaId) VALUES(@ID,@NOMBRE,@MARCA)";
                        command.Parameters.Add(new SQLiteParameter("@ID", modelo.ModeloId));
                        command.Parameters.Add(new SQLiteParameter("@NOMBRE", modelo.Nombre));
                        command.Parameters.Add(new SQLiteParameter("@MARCA", modelo.MarcaId));
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
            Dispose(true);
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) { return; }
            if (disposing)
            {
                handle.Dispose();
            }
            disposed = true;
        }
    }
}
