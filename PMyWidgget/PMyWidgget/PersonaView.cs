using System;
using Gtk;

namespace PMyWidgget
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PersonaView : Gtk.Bin
	{
		public PersonaView ()
		{

			this.Build ();
		}

		public  Entry EntryNombre {get{return entryNombre;}}

		public Entry EntryApellidos {get{return entryApellidos;}}
	
	
	}
}

