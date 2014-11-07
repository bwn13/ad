using System;
using Gtk;
using System.Data;
using SerpisAd;
using PArticulo;

public partial class MainWindow: Gtk.Window
{	
	/*variables globales*/
	private ListStore listStore;
	private ListStore listStore2;
	private IDbConnection dbConnection;
	private IDbCommand dbCommand;
	private IDataReader dataReader;


	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{

		Build ();



		dbConnection = App.Instance.DbConnection;	

		//Categoria
		//Por defecto aparecen estos iconos no operativos(hasta que se seleccione una fila)
		deleteAction.Sensitive = false;
		editAction.Sensitive = false;
		//Articulo
		deleteAction1.Sensitive = false;
		editAction1.Sensitive = false;


		treeview1.AppendColumn ("id", new CellRendererText (), "text", 0); //siendo 0 el index de la columna
		treeview1.AppendColumn ("nombre", new CellRendererText (), "text", 1);

		treeview2.AppendColumn ("id",  new CellRendererText (), "text", 0);
		treeview2.AppendColumn ("nombre", new CellRendererText (), "text", 1);
		treeview2.AppendColumn ("categoria", new CellRendererText (), "text", 2);
		treeview2.AppendColumn ("precio",  new CellRendererText (), "text", 3);


		listStore = new ListStore (typeof(ulong), typeof(string));
		listStore2 = new ListStore (typeof(ulong), typeof(string),typeof(string),typeof(string));

		treeview1.Model = listStore; 
		fillListStore ();

		treeview2.Model = listStore2; 
		fillListStore2 ();

		treeview1.Selection.Changed += delegate {

		//bool hasSelected = treeview1.Selection.CountSelectedRows () > 0;
		deleteAction.Sensitive = treeview1.Selection.CountSelectedRows () > 0; // cuando es mayor que 0, es true
		editAction.Sensitive = treeview1.Selection.CountSelectedRows () > 0;
		};

		treeview2.Selection.Changed += delegate {

			//bool hasSelected = treeview2.Selection.CountSelectedRows () > 0;
			deleteAction1.Sensitive = treeview2.Selection.CountSelectedRows () > 0; // cuando es mayor que 0, es true
			editAction1.Sensitive = treeview2.Selection.CountSelectedRows () > 0;
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

	private void fillListStore2 () {
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = "select * from articulo";

		dataReader = dbCommand.ExecuteReader (); 

		while (dataReader.Read()) { 

			object id = dataReader ["id"]; 
			object nombre =dataReader["nombre"];
			object categoria =dataReader["categoria"].ToString();
			object precio =dataReader["precio"].ToString();

			listStore2.AppendValues (id, nombre, categoria, precio);

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

		TreeIter treeiter; //Con Treeiter sabemos la posición exacta en el ListStore,la fila, que es como se visualiza el TreeView
		treeview1.Selection.GetSelected (out treeiter);
		object id =listStore.GetValue (treeiter, 0);
		EditViewCategoria editviewcategoria = new EditViewCategoria (id);


	}

	/////////////// ARTICULO /////////////////

	protected void OnAddAction1Activated (object sender, EventArgs e)
	{
		string insertSql1 = "insert into articulo (nombre) values ('{0}')";
		//string insertSql1 = "insert into articulo (nombre,categoria,precio) values ('{0}','{1}','{2}')";
		insertSql1 = string.Format (insertSql1, "Nuevonombre "+ DateTime.Now);

		dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = insertSql1;

		dbCommand.ExecuteNonQuery ();
	}

	protected void OnRefreshAction1Activated (object sender, EventArgs e)
	{
		listStore2.Clear ();
		fillListStore2 ();
	}

	protected void OnDeleteAction1Activated (object sender, EventArgs e)
	{
		TreeIter treeiter2; //Con Treeiter sabemos la posición exacta en el ListStore,la fila, que es como se visualiza el TreeView
		treeview2.Selection.GetSelected (out treeiter2);
		object id =listStore2.GetValue (treeiter2, 0);


		MessageDialog messageDialog = new MessageDialog (
			this,
			DialogFlags.Modal,
			MessageType.Question,
			ButtonsType.OkCancel,
			"¿Desea eliminar el registro de la Base de Datos?"
			);


		messageDialog.Title = Title; 
		if ((ResponseType)messageDialog.Run () == ResponseType.Ok) {

			string deleteSql= string.Format("DELETE FROM articulo WHERE id={0}", id);
			dbCommand = dbConnection.CreateCommand ();
			dbCommand.CommandText =deleteSql;

			dbCommand.ExecuteNonQuery ();
		}
		messageDialog.Destroy ();
	}


} // Class
