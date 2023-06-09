﻿using System.Drawing.Text;

namespace AstronomyPictureOfTheDayWallpaperApp
{
    public class WallpaperAPODdraw : IDisposable
    {
        const float minTitleHeightRatio = 0.13f;
        const float maxTitleHeightRatio = 0.25f; // Change text height from bottom
        const float minDescriptionHeightRatio = 0.06f;
        const float maxDescriptionHeightRatio = 0.15f; // Change text height from bottom
        private bool disposedValue;
        
        // Set title in image and size is by width and heigh of the image and add shadow
        public void SetTitle(Graphics graphic, Image pictureModified, RectangleF descriptionRect, string title, string description, PrivateFontCollection fontCollection)
        {
            int titlePadding = (int)(0.03 * pictureModified.Width);
            float titleHeightRatio = Math.Max(Math.Min(pictureModified.Height * 0.005f, maxTitleHeightRatio), minTitleHeightRatio);
            int titleHeight = (int)((pictureModified.Height) * titleHeightRatio);
            int offset = (int)(pictureModified.Height * -0.035); // Set height above description
            RectangleF titleRect = new(
                    titlePadding,
                    pictureModified.Height - titleHeight - 1.5f * titlePadding - offset,
                    pictureModified.Width - 2 * titlePadding,
                    titleHeight
            );
            float maxFontSize = titleRect.Height;
            float titleFontSize = maxFontSize;
            Font titleFont = new(fontCollection.Families[1], titleFontSize, FontStyle.Bold);
            SizeF textSize = graphic.MeasureString(title, titleFont, (int)descriptionRect.Width);
            float step = Math.Max(maxFontSize / 5.0f, 2.0f); // Sets the step size for font size reduction
            while (textSize.Height > titleRect.Height && titleFontSize > 1)
            {
                titleFontSize -= step;
                titleFont = new Font(fontCollection.Families[1], titleFontSize, FontStyle.Bold);
                textSize = graphic.MeasureString(description, titleFont, (int)titleRect.Width);
                if (step > 2.0f) // Gradually reduce step size
                {
                    step *= 0.5f;
                }
            }
            SolidBrush shadowBrush = new(Color.FromArgb(128, Color.Black));
            SolidBrush textColor = new(Color.White);
            StringFormat titleFormat = new() { Alignment = StringAlignment.Far };
            float shadowOffset = pictureModified.Height * 0.002f;
            RectangleF shadowRect = new(titleRect.X + shadowOffset, titleRect.Y + shadowOffset, titleRect.Width, titleRect.Height);
            graphic.DrawString(title, titleFont, shadowBrush, shadowRect, titleFormat); // Draw title shadow
            graphic.DrawString(title, titleFont, textColor, titleRect, titleFormat); // Draw title            
        }
        
        // Set description in image and size is by width and heigh of the image
        public Task<RectangleF> SetDescription(Graphics graphic, Image pictureModified, string description, PrivateFontCollection fontCollection)
        {
            int descriptionPadding = (int)(0.03 * pictureModified.Width);
            float descriptionHeightRatio = Math.Max(Math.Min(pictureModified.Height * 0.0005f, maxDescriptionHeightRatio), minDescriptionHeightRatio);
            int descriptionHeight = (int)((pictureModified.Height - 30) * descriptionHeightRatio);
            int offset = (int)(pictureModified.Height * 0.0005);
            RectangleF descriptionRect = new(
                    descriptionPadding,
                    pictureModified.Height - descriptionHeight - 2 * descriptionPadding - 10 - offset,
                    pictureModified.Width - 2 * descriptionPadding,
                    descriptionHeight
            );
            float maxFontSize = descriptionRect.Height;
            float descriptionFontSize = maxFontSize;
            Font descriptionFont = new(fontCollection.Families[0], descriptionFontSize, FontStyle.Regular);
            SizeF textSize = graphic.MeasureString(description, descriptionFont, (int)descriptionRect.Width);
            float step = Math.Max(maxFontSize / 5.0f, 2.0f); // Sets the step size for font size reduction
            while (textSize.Height > descriptionRect.Height && descriptionFontSize > 1)
            {
                descriptionFontSize -= step;
                descriptionFont = new Font(fontCollection.Families[0], descriptionFontSize, FontStyle.Regular);
                textSize = graphic.MeasureString(description, descriptionFont, (int)descriptionRect.Width);
                if (step > 2.0f) // Gradually reduce step size
                {
                    step *= 0.5f;
                }
            }
            // Draw shadow
            SolidBrush shadowBrush = new(Color.FromArgb(128, Color.Black));
            StringFormat shadowFormat = new() { Alignment = StringAlignment.Far };
            float shadowOffset = pictureModified.Height * 0.0015f;
            RectangleF shadowRect = new(
                descriptionRect.X + shadowOffset,
                descriptionRect.Y + shadowOffset,
                descriptionRect.Width,
                descriptionRect.Height
            );
            graphic.DrawString(description, descriptionFont, shadowBrush, shadowRect, shadowFormat);
            Brush textColor = new SolidBrush(Color.White);
            StringFormat descriptionFormat = new() { Alignment = StringAlignment.Far };
            graphic.DrawString(description, descriptionFont, textColor, descriptionRect, descriptionFormat); // Draw the text using the calculated font size            
            return Task.FromResult(descriptionRect);
        }
        
        // Dispose method with optional disposing parameter.
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }
                disposedValue = true;
            }
        }
        
        // Finalize the WallpaperAPODdraw object
        ~WallpaperAPODdraw()
        {
            Dispose(disposing: false);
        }
        
        // Public Dispose method that calls the protected Dispose method with disposing set to true, and suppresses finalization
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}