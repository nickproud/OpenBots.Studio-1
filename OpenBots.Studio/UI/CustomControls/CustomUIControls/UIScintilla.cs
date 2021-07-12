using ScintillaNET;
using System.Windows.Forms;

namespace OpenBots.UI.CustomControls.CustomUIControls
{
    public partial class UIScintilla : Scintilla
    {
        public bool ListBoxShown { get; set; }
        public int TabSize { get; set; } = 4;


        public UIScintilla()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message Msg, Keys keyData)
        {
            if (ListBoxShown && keyData == Keys.Enter)
            {
                OnKeyDown(new KeyEventArgs(keyData));
                return true;
            }
            //else if (keyData == Keys.Tab)
            //{
            //    SelectedText += new string(' ', TabSize);
            //    return true;
            //}
            return base.ProcessCmdKey(ref Msg, keyData);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            Keys key = (keyData & Keys.KeyCode);

            if (ListBoxShown && key == Keys.Enter)
            {
                OnKeyDown(new KeyEventArgs(key));
                return true;
            }
            //else if (keyData == Keys.Tab)
            //{
            //    SelectedText += new string(' ', TabSize);
            //    return true;
            //}

            return base.ProcessDialogKey(keyData);
        }
    }
}
