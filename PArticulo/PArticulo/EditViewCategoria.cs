using System;
using System.Data;
using SerpisAd;

namespace PArticulo
{
	public partial class EditViewCategoria : Gtk.Window
	{
		private object id;
		public EditViewCategoria () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}
		public EditViewCategoria (object id) : this () {

			this.id = id;
			entry1.Text = "id=" +id;
			IDbCommand dbCommand = App.Instance.DbConnection.CreateCommand ();
			dbCommand.CommandText = String.Format (
				"select * from categoria where id={0}", id);

			IDataReader dataReader = dbCommand.ExecuteReader ();
			dataReader.Read ();

			entry1.Text = dataReader ["nombre"].ToString ();
			dataReader.Close ();

		}
		protected void OnSaveActionActivated (object sender, EventArgs e)
		{
			IDbCommand dbCommand =
				App.Instance.DbConnection.CreateCommand ();
			dbCommand.CommandText = String.Format (
				"update categoria set nombre=@nombre where id={0}", id);

			dbCommand.AddParameter ("nombre", entry1.Text);
			dbCommand.ExecuteNonQuery ();

			Destroy ();
		}

	}
}

