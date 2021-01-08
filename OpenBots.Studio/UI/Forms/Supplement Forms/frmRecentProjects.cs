using OpenBots.Core.Settings;
using OpenBots.Core.UI.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    public partial class frmRecentProjects : UIForm
    {
        public string RecentProjectPath { get; set; }

        public frmRecentProjects()
        {            
            InitializeComponent();
        }

        private void frmRecentProjects_Load(object sender, EventArgs e)
        {
            ApplicationSettings appSettings = new ApplicationSettings().GetOrCreateApplicationSettings();
            List<string> recentlyOpenedProjectPaths = appSettings.ClientSettings.RecentProjects;

            if (recentlyOpenedProjectPaths != null)
            {
                foreach (string path in recentlyOpenedProjectPaths)
                {
                    string projectName = new DirectoryInfo(path).Name;
                    ListViewItem projectItem = new ListViewItem(new string[] { projectName, path });
                    projectItem.Tag = path;
                    lvRecentProjects.Items.Add(projectItem);
                }
            }
            
            ColorListViewHeader(ref lvRecentProjects, Color.FromArgb(20, 136, 204), Color.White, new Font("Segoe UI", 10, FontStyle.Bold));
        }

        private void ColorListViewHeader(ref ListView list, Color backColor, Color foreColor, Font font)
        {
            list.OwnerDraw = true;
            list.DrawColumnHeader +=
                new DrawListViewColumnHeaderEventHandler
                (
                    (sender, e) => HeaderDraw(sender, e, backColor, foreColor, font)
                );
            list.DrawItem += new DrawListViewItemEventHandler(BodyDraw);
        }

        private void HeaderDraw(object sender, DrawListViewColumnHeaderEventArgs e, Color backColor, Color foreColor, Font font)
        {
            using (SolidBrush backBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);
            }

            using (SolidBrush foreBrush = new SolidBrush(foreColor))
            {
                e.Graphics.DrawString(e.Header.Text, font, foreBrush, e.Bounds);
            }
        }

        private void BodyDraw(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void lvRecentProjects_DoubleClick(object sender, EventArgs e)
        {
            RecentProjectPath = lvRecentProjects.SelectedItems[0].Tag.ToString();
            DialogResult = DialogResult.OK;
        }
    }
}