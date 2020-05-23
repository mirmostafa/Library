using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Mohammad.Win.Renderers.Internal
{
    /// <summary>
    ///     Set the SmoothingMode=AntiAlias until instance disposed.
    /// </summary>
    internal class UseAntiAlias : IDisposable
    {
        #region Instance Fields

        private readonly Graphics _g;
        private readonly SmoothingMode _old;

        #endregion

        #region Identity

        /// <summary>
        ///     Initialize a new instance of the UseAntiAlias class.
        /// </summary>
        /// <param name="g">Graphics instance.</param>
        public UseAntiAlias(Graphics g)
        {
            this._g = g;
            this._old = this._g.SmoothingMode;
            this._g.SmoothingMode = SmoothingMode.AntiAlias;
        }

        /// <summary>
        ///     Revert the SmoothingMode back to original setting.
        /// </summary>
        public void Dispose() { this._g.SmoothingMode = this._old; }

        #endregion
    }

    /// <summary>
    ///     Set the TextRenderingHint.ClearTypeGridFit until instance disposed.
    /// </summary>
    public class UseClearTypeGridFit : IDisposable
    {
        #region Instance Fields

        private readonly Graphics _g;
        private readonly TextRenderingHint _old;

        #endregion

        #region Identity

        /// <summary>
        ///     Initialize a new instance of the UseClearTypeGridFit class.
        /// </summary>
        /// <param name="g">Graphics instance.</param>
        public UseClearTypeGridFit(Graphics g)
        {
            this._g = g;
            if (g != null)
            {
                this._old = this._g.TextRenderingHint;
                this._g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            }
        }

        /// <summary>
        ///     Revert the TextRenderingHint back to original setting.
        /// </summary>
        public void Dispose() { this._g.TextRenderingHint = this._old; }

        #endregion
    }

    /// <summary>
    ///     Set the clipping region until instance disposed.
    /// </summary>
    public class UseClipping : IDisposable
    {
        #region Instance Fields

        private readonly Graphics _g;
        private readonly Region _old;

        #endregion

        #region Identity

        /// <summary>
        ///     Initialize a new instance of the UseClipping class.
        /// </summary>
        /// <param name="g">Graphics instance.</param>
        /// <param name="path">Clipping path.</param>
        public UseClipping(Graphics g, GraphicsPath path)
        {
            if (g != null)
                this._g = g;
            if (g != null)
                this._old = g.Clip;
            var clip = this._old.Clone();
            clip.Intersect(path);
            if (g != null)
                this._g.Clip = clip;
        }

        /// <summary>
        ///     Initialize a new instance of the UseClipping class.
        /// </summary>
        /// <param name="g">Graphics instance.</param>
        /// <param name="region">Clipping region.</param>
        public UseClipping(Graphics g, Region region)
        {
            this._g = g;
            this._old = g.Clip;
            var clip = this._old.Clone();
            clip.Intersect(region);
            this._g.Clip = clip;
        }

        /// <summary>
        ///     Revert clipping back to origina setting.
        /// </summary>
        public void Dispose() { this._g.Clip = this._old; }

        #endregion
    }
}