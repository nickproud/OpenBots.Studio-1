using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBots.UI.CustomControls.CustomUIControls
{
	public class UIIntellisenseListBoxItem
	{
		private string _myText;
		private int _myImageIndex;
		// properties 
		public string Text
		{
			get { return _myText; }
			set { _myText = value; }
		}
		public int ImageIndex
		{
			get { return _myImageIndex; }
			set { _myImageIndex = value; }
		}
		//constructor
		public UIIntellisenseListBoxItem(string text, int index)
		{
			_myText = text;
			_myImageIndex = index;
		}
		public UIIntellisenseListBoxItem(string text) : this(text, -1) { }
		public UIIntellisenseListBoxItem() : this("") { }
		public override string ToString()
		{
			return _myText;
		}
	}
}
