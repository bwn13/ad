using System;
using Gtk;

using PNotebook;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{


		Build ();
		ArticuloAction1.Activated += delegate {
			addPage (new MyTreeView (), "Articulo");
		};

		CategoriaAction1.Activated += delegate {
			addPage (new MyTreeView (), "Categoria");
		};

		notebook1.SwitchPage += delegate {
			onPageChanged ();
		};
		notebook1.PageRemoved += delegate {
			Console.WriteLine ("notebook1.CurrentPage = {0}", notebook1.CurrentPage);
		};
	} // Main Window
		private void onPageChanged (){
		Console.WriteLine ("notebook1.CurrentPage = {0}", notebook1.CurrentPage);
	}
	
		private void addPage (Widget widget, string label) {
		HBox hBox = new HBox ();
		hBox.Add(new Label (label));
		Button button = new Button (new Image (Stock.Cancel, IconSize.Button ));
		hBox.Add (button);
		hBox.ShowAll();
		notebook1.AppendPage (widget, new Label(label));

		button.Clicked += delegate {
			widget.Destroy ();
		};

		}
		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit();
			a.RetVal = true;
		}

}
