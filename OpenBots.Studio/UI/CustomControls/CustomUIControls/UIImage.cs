using OpenBots.Core.Properties;
using OpenBots.Core.UI.Controls;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace OpenBots.UI.CustomControls.Controls
{
    public static class UIImage
    {
        public static ImageList UIImageList(List<AutomationCommand> automationCommands)
        {
            Dictionary<string, Image> commandIcons = new Dictionary<string, Image>();

            foreach(var command in automationCommands)
                commandIcons[command.Command.CommandName] = command.CommandIcon;

            ImageList uiImages = new ImageList();
            uiImages.ImageSize = new Size(18, 18);
            uiImages.Images.Add("BrokenCodeCommentCommand", Resources.command_broken);

            foreach (var icon in commandIcons)
            {
                //var someImage = icon.Value;

                //using (Image src = icon.Value)
                //using (Bitmap dst = new Bitmap(20, 20))
                //using (Graphics g = Graphics.FromImage(dst))
                //{
                //    g.SmoothingMode = SmoothingMode.AntiAlias;
                //    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //    g.DrawImage(src, 0, 0, dst.Width, dst.Height);
                //    uiImages.Images.Add(icon.Key, dst);
                //}
                uiImages.Images.Add(icon.Key, icon.Value);
            }
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
