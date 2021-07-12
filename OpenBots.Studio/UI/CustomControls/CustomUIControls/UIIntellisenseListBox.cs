using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.UI.CustomControls.CustomUIControls
{
	public class UIIntellisenseListBox : ListBox
	{
		private ImageList _myImageList;
		public ImageList ImageList
		{
			get { return _myImageList; }
			set { _myImageList = value; }
		}
		public UIIntellisenseListBox()
		{
			// Set owner draw mode
			DrawMode = DrawMode.OwnerDrawFixed;
		}
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			e.DrawBackground();
			e.DrawFocusRectangle();
			UIIntellisenseListBoxItem item;
			Rectangle bounds = e.Bounds;
			Size imageSize = _myImageList.ImageSize;
			ItemHeight = imageSize.Height + 5;
			try
			{
				item = (UIIntellisenseListBoxItem)Items[e.Index];
				if (item.ImageIndex != -1)
				{
					_myImageList.Draw(e.Graphics, bounds.Left, bounds.Top, item.ImageIndex);
					e.Graphics.DrawString(item.Text, e.Font, new SolidBrush(e.ForeColor),
						bounds.Left + imageSize.Width, bounds.Top);
				}
				else
				{
					e.Graphics.DrawString(item.Text, e.Font, new SolidBrush(e.ForeColor),
						bounds.Left, bounds.Top);
				}
			}
			catch
			{
				if (e.Index != -1)
				{
					e.Graphics.DrawString(Items[e.Index].ToString(), e.Font,
						new SolidBrush(e.ForeColor), bounds.Left, bounds.Top);
				}
				else
				{
					e.Graphics.DrawString(Text, e.Font, new SolidBrush(e.ForeColor),
						bounds.Left, bounds.Top);
				}
			}
			base.OnDrawItem(e);
		}
	}
}