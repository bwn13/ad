using System;
using System.Data;
using SerpisAd;

namespace PCategoria
{
	public partial class CategoriaView : Gtk.Window
	{
		private object id;
		public CategoriaView () : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}

		public CategoriaView (object id) : this () {

			this.id = id;
			//entryName.Text = "id=" +id;
			IDbCommand dbCommand = App.Instance.DbConnection.CreateCommand ();
			dbCommand.CommandText = String.Format (
				"select * from categoria where id={0}", id);

			IDataReader dataReader = dbCommand.ExecuteReader ();
			dataReader.Read ();

			entryName.Text = dataReader ["nombre"].ToString ();
			dataReader.Close ();

			}
			
		
		
		protected void OnSaveActionActivated (object sender, EventArgs e)
		{
			IDbCommand dbCommand =
				App.Instance.DbConnection.CreateCommand ();
			dbCommand.CommandText = String.Format (
				"update categoria set nombre=@nombre where id={0}", id);


			/*IDbDataParameter dbDataParameter = dbCommand.CreateParameter ();
			dbDataParameter.ParameterName = "nombre";
			dbDataParameter.Value = entryName.Text ;
			dbCommand.Parameters.Add(dbDataParameter);
			*/

			//DbCommandExtensions.AddParameter (dbCommand, "nombre", entryName.Text);
			dbCommand.AddParameter ("nombre", entryName.Text);
			dbCommand.ExecuteNonQuery ();

			Destroy ();

		
		}

	}
}

