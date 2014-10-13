using System;
using System.Data;
using Gtk;
using PCategoria;

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

		deleteAction.Sensitive = false;
		editAction.Sensitive = false;

		dbConnection = App.Instance.DbConnection;
		dbConnection.Open ();

		/*mySqlConnection= new MySqlConnection
			(
				"DataSource = localhost;" +
				"Database=dbprueba;" +
				"User ID = root;" +
				"Password=sistemas;"
				);

		mySqlConnection.Open ();
	*/
		/*añadimos dos columnas al treeView utilizando el método AppendColumn*/

		treeView.AppendColumn ("id", new CellRendererText (), "text", 0); //siendo 0 el index de la columna
		treeView.AppendColumn ("nombre", new CellRendererText (), "text", 1);

		listStore = new ListStore (typeof(ulong), typeof(string)); // lo declaramos al principio para que sea global, y poder reutilzar la variable

		treeView.Model = listStore; /*en java treeView.setModel (listStore) ; usamos el listStore como modelo del treeView*/

		fillListStore ();


		/* Dos formas de crear el método para el evento Changed, o bien declarando el nombre para el método(selectionChanged), y 
		creando un private void selectionChanged (){}, o creando un método anónimo como delegate, que se ejecutará como un método normal, al inicializarse el evento
		*/

		//treeView.Selection.Changed += selectionChanged; 
		treeView.Selection.Changed += delegate {
			/*
			 * También podemos crear una variable booleana:
			 * bool hasSelected = treeView.Selection.CountSelectedRows() > 0;
			 * editAction.Sensitive = hasSelected
			 */
			bool hasSelected = treeView.Selection.CountSelectedRows ()>0;

			deleteAction.Sensitive=treeView.Selection.CountSelectedRows() > 0; // cuando es mayor que 0, es true
			editAction.Sensitive= treeView.Selection.CountSelectedRows() > 0;
		
		};

	}


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
		/*
		 * if (!Confirm("Quiere eliminar el registro de la Base de Datos"))
		 * return;
		 *  
		 * Con esto solo nos quedaría en el evento la declaración del TreeIter y la consulta

		 */

		TreeIter treeiter; //Con Treeiter sabemos la posición exacta en el ListStore,la fila, que es como se visualiza el TreeView
		treeView.Selection.GetSelected (out treeiter);
		object id =listStore.GetValue (treeiter, 0);


		MessageDialog messageDialog = new MessageDialog (
			this,
			DialogFlags.Modal,
			MessageType.Question,
			ButtonsType.OkCancel,
			"¿Desea eliminar el registro de la Base de Datos?"
			);

	
		/*
		 * Una forma de hacerlo...
		 *
		 *ResponseType response = (ResponseType)messageDialog.Run ();
		messageDialog.Destroy ();

		if (response != ResponseType.Ok)
			return;

			Consulta..

		 */
		 
		messageDialog.Title = Title; 
		if ((ResponseType)messageDialog.Run () == ResponseType.Ok) {

			string deleteSql= string.Format("DELETE FROM categoria WHERE id={0}", id);
			dbCommand = dbConnection.CreateCommand ();
			dbCommand.CommandText =deleteSql;

			dbCommand.ExecuteNonQuery ();
		}

		messageDialog.Destroy ();

	}

	/*
	 * Refractorizamos, creando un método para dejarlo más limpio
	 * 
	 * public bool Confirm (string text) {
		MessageDialog messageDialog = new MessageDialog (
			this,
			DialogFlags.Modal,
			MessageType.Question,
			ButtonsType.OkCancel,
			"¿Desea eliminar el registro de la Base de Datos?"
			);
		ResponseType response = (ResponseType)messageDialog.Run ();
		messageDialog.Destroy ();

		return response == ResponseType.Ok;
	}
	*/

	protected void OnEditActionActivated (object sender, EventArgs e)
	{

		TreeIter treeiter; //Con Treeiter sabemos la posición exacta en el ListStore,la fila, que es como se visualiza el TreeView
		treeView.Selection.GetSelected (out treeiter);
		object id =listStore.GetValue (treeiter, 0);
		CategoriaView categoriaView = new CategoriaView (); // no hace falta llamar a Show (categoriaView.Show) porque en las propiedades de CategoriaView, 'Visible' está activado
		categoriaView (id);
	
	}

}
