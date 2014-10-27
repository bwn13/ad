using System;
using Gtk;
using System.Data;
using SerpisAd;

public partial class MainWindow: Gtk.Window
{	
	/*variables globales*/
	private ListStore listStore;
	private IDbConnection dbConnection;
	private IDbCommand dbCommand;
	private IDataReader dataReader;


	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{

		Build ();

		//treeview1.AppendColumn("precio", new CellRenderer (), "text", out);
		//ListStore liststore = new ListStore (typeof(string));
		//object value = new decimal (1.2).ToString();
		//listStore.AppendValues(value);


		deleteAction.Sensitive = false;
		editAction.Sensitive = false;

		dbConnection = App.Instance.DbConnection;	
		treeview1.AppendColumn ("id", new CellRendererText (), "text", 0); //siendo 0 el index de la columna
		treeview1.AppendColumn ("nombre", new CellRendererText (), "text", 1);

		listStore = new ListStore (typeof(ulong), typeof(string)); 

		treeview1.Model = listStore; 
		fillListStore ();


		treeview1.Selection.Changed += delegate {

			//bool hasSelected = treeview1.Selection.CountSelectedRows () > 0;

			deleteAction.Sensitive = treeview1.Selection.CountSelectedRows () > 0; // cuando es mayor que 0, es true
			editAction.Sensitive = treeview1.Selection.CountSelectedRows () > 0;
		};
	} //Main
	
	private void fillListStore () {
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = "select * from categoria";



		dataReader = dbCommand.ExecuteReader (); 

		while (dataReader.Read()) { 

			object id = dataReader ["id"]; 
			object nombre =dataReader["nombre"];

			listStore.AppendValues (id, nombre);

		}
		dataReader.Close ();

	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{

		dbConnection.Close ();	
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnAddActionActivated (object sender, EventArgs e)
	{


		string insertSql = "insert into categoria (nombre) values ('{0}')";
		insertSql = string.Format (insertSql, "Nuevo " + DateTime.Now);

		dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = insertSql;

		dbCommand.ExecuteNonQuery ();

	}

	protected void OnRefreshActionActivated (object sender, EventArgs e)
	{
		listStore.Clear ();
		fillListStore ();

	}

	protected void OnDeleteActionActivated (object sender, EventArgs e)
	{


		TreeIter treeiter; //Con Treeiter sabemos la posición exacta en el ListStore,la fila, que es como se visualiza el TreeView
		treeview1.Selection.GetSelected (out treeiter);
		object id =listStore.GetValue (treeiter, 0);


		MessageDialog messageDialog = new MessageDialog (
			this,
			DialogFlags.Modal,
			MessageType.Question,
			ButtonsType.OkCancel,
			"¿Desea eliminar el registro de la Base de Datos?"
			);

	

		messageDialog.Title = Title; 
		if ((ResponseType)messageDialog.Run () == ResponseType.Ok) {

			string deleteSql= string.Format("DELETE FROM categoria WHERE id={0}", id);
			dbCommand = dbConnection.CreateCommand ();
			dbCommand.CommandText =deleteSql;

			dbCommand.ExecuteNonQuery ();
		}

		messageDialog.Destroy ();

	}
	

	protected void OnEditActionActivated (object sender, EventArgs e)
	{



	}

	/*protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}*/
}
