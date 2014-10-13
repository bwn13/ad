using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnButtonClicked (object sender, EventArgs e) //Evento del boton
	{
		//Console.WriteLine ("Has hecho click en Aceptar");
		//throw new NotImplementedException ();

		/* 3 formas de hacerlo */

		//labelSaludo.LabelProp = "Hola " + entry.Text;
		//labelSaludo.LabelProp = string.Format ("Hola {0}", entry.Text);
		labelSaludo.Text = "Hola " + entry.Text;
	}


	protected void OnNewActionActivated (object sender, EventArgs e) //Evento del icono
	{
		Console.WriteLine ("Has activado la acción newAction");
	
	}
}
