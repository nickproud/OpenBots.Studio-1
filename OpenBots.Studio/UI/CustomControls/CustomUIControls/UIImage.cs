using Autofac;
using OpenBots.Core.Command;
using OpenBots.Core.Properties;
using OpenBots.Studio.Utilities;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace OpenBots.UI.CustomControls.Controls
{
    public static class UIImage
    {
        public static ImageList UIImageList(IContainer container)
        {
            var commandTypes = TypeMethods.GenerateCommandTypes(container);

            ImageList uiImages = new ImageList();
            uiImages.ImageSize = new Size(18, 18);
            uiImages.Images.Add("BrokenCodeCommentCommand", Resources.command_broken);

            foreach (var command in commandTypes)
            {
                ScriptCommand newCommand = (ScriptCommand)Activator.CreateInstance(command);
                uiImages.Images.Add(newCommand.CommandName, newCommand.CommandIcon);
                newCommand.CommandIcon = null;
            }

            GC.Collect();
            return uiImages;
        }

        public static Image ResizeImageFile(Image image)
        {
            using (Image oldImage = image)
            {
                using (Bitmap newImage = new Bitmap(20, 20, PixelFormat.Format32bppRgb))
                {
                    using (Graphics canvas = Graphics.FromImage(newImage))
                    {
                        canvas.SmoothingMode = SmoothingMode.AntiAlias;
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        canvas.DrawImage(oldImage, new Rectangle(new Point(0, 0), new Size(20, 20)));
                        return newImage;
                    }
                }
            }
        }
    }
}
