using OpenBots.Core.Infrastructure;
using System.Threading;
using System.Windows.Forms;

namespace OpenBots.Core.Utilities.FormsUtilities
{
    public static class FormsHelper
    {
        public static void ShowAllForms(bool isDebugMode)
        {
            foreach (Form form in Application.OpenForms)
                ShowForm(isDebugMode, form);

            Thread.Sleep(1000);
        }

        public delegate void ShowFormDelegate(bool isDebugMode, Form form);
        public static void ShowForm(bool isDebugMode, Form form)
        {
            if (form.InvokeRequired)
            {
                var d = new ShowFormDelegate(ShowForm);
                form.Invoke(d, new object[] { isDebugMode, form });
            }
            else
            {
                if (isDebugMode)
                    form.WindowState = FormWindowState.Normal;
            }
        }

        public static void HideAllForms()
        {
            foreach (Form form in Application.OpenForms)
                HideForm(form);

            Thread.Sleep(1000);
        }

        public delegate void HideFormDelegate(Form form);
        public static void HideForm(Form form)
        {
            if (form.InvokeRequired)
            {
                var d = new HideFormDelegate(HideForm);
                form.Invoke(d, new object[] { form });
            }
            else
                form.WindowState = FormWindowState.Minimized;
        }
    }
}
