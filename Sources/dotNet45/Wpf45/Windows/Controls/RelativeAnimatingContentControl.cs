using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     The platform does not currently support relative sized translation values.
    ///     This primitive control walks through visual state animation storyboards
    ///     and looks for identifying values to use as percentages.
    /// </summary>
    public class RelativeAnimatingContentControl : ContentControl
    {
        /// <summary>
        ///     The last known height of the control.
        /// </summary>
        private double _KnownHeight;

        /// <summary>
        ///     The last known width of the control.
        /// </summary>
        private double _KnownWidth;

        /// <summary>
        ///     A set of custom animation adapters used to update the animation
        ///     storyboards when the size of the control changes.
        /// </summary>
        private List<AnimationValueAdapter> _SpecialAnimations;

        /// <summary>
        ///     A simple Epsilon-style value used for trying to determine if a double
        ///     has an identifying value.
        /// </summary>
        private const double SIMPLE_DOUBLE_COMPARISON_EPSILON = 0.000009;

        /// <summary>
        ///     Initializes a new instance of the RelativeAnimatingContentControl
        ///     type.
        /// </summary>
        public RelativeAnimatingContentControl() { this.SizeChanged += this.OnSizeChanged; }

        /// <summary>
        ///     Handles the size changed event.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e != null && e.NewSize.Height > 0 && e.NewSize.Width > 0)
            {
                this._KnownWidth = e.NewSize.Width;
                this._KnownHeight = e.NewSize.Height;

                this.UpdateAnyAnimationValues();
            }
        }

        /// <summary>
        ///     Walks through the known storyboards in the control's template that
        ///     may contain identifying values, storing them for future
        ///     use and updates.
        /// </summary>
        private void UpdateAnyAnimationValues()
        {
            if (this._KnownHeight > 0 && this._KnownWidth > 0)
            {
                // Initially, before any special animations have been found,
                // the visual state groups of the control must be explored. 
                // By definition they must be at the implementation root of the
                // control.
                if (this._SpecialAnimations == null)
                {
                    this._SpecialAnimations = new List<AnimationValueAdapter>();

                    foreach (VisualStateGroup group in VisualStateManager.GetVisualStateGroups(this))
                    {
                        if (group == null)
                            continue;
                        foreach (VisualState state in group.States)
                        {
                            var sb = state?.Storyboard;

                            if (sb != null)
                                // Examine all children of the storyboards,
                                // looking for either type of double
                                // animation.
                                foreach (var timeline in sb.Children)
                                {
                                    var da = timeline as DoubleAnimation;
                                    var dakeys = timeline as DoubleAnimationUsingKeyFrames;
                                    if (da != null)
                                        this.ProcessDoubleAnimation(da);
                                    else if (dakeys != null)
                                        this.ProcessDoubleAnimationWithKeys(dakeys);
                                }
                        }
                    }
                }

                // Update special animation values relative to the current size.
                this.UpdateKnownAnimations();

                // HACK: force storyboard to use new values
                foreach (VisualStateGroup group in VisualStateManager.GetVisualStateGroups(this))
                {
                    if (group == null)
                        continue;
                    foreach (VisualState state in group.States)
                        state?.Storyboard?.Begin(this);
                }
            }
        }

        /// <summary>
        ///     Walks through all special animations, updating based on the current
        ///     size of the control.
        /// </summary>
        private void UpdateKnownAnimations()
        {
            foreach (var adapter in this._SpecialAnimations)
                adapter.UpdateWithNewDimension(this._KnownWidth, this._KnownHeight);
        }

        /// <summary>
        ///     Processes a double animation with keyframes, looking for known
        ///     special values to store with an adapter.
        /// </summary>
        /// <param name="da">The double animation using key frames instance.</param>
        private void ProcessDoubleAnimationWithKeys(DoubleAnimationUsingKeyFrames da)
        {
            // Look through all keyframes in the instance.
            foreach (DoubleKeyFrame frame in da.KeyFrames)
            {
                var d = DoubleAnimationFrameAdapter.GetDimensionFromIdentifyingValue(frame.Value);
                if (d.HasValue)
                    this._SpecialAnimations.Add(new DoubleAnimationFrameAdapter(d.Value, frame));
            }
        }

        /// <summary>
        ///     Processes a double animation looking for special values.
        /// </summary>
        /// <param name="da">The double animation instance.</param>
        private void ProcessDoubleAnimation(DoubleAnimation da)
        {
            // Look for a special value in the To property.
            if (da.To.HasValue)
            {
                var d = DoubleAnimationToAdapter.GetDimensionFromIdentifyingValue(da.To.Value);
                if (d.HasValue)
                    this._SpecialAnimations.Add(new DoubleAnimationToAdapter(d.Value, da));
            }

            // Look for a special value in the From property.
            if (da.From.HasValue)
            {
                var d = DoubleAnimationFromAdapter.GetDimensionFromIdentifyingValue(da.To.Value);
                if (d.HasValue)
                    this._SpecialAnimations.Add(new DoubleAnimationFromAdapter(d.Value, da));
            }
        }

        #region Private animation updating system

        /// <summary>
        ///     A simple class designed to store information about a specific
        ///     animation instance and its properties. Able to update the values at
        ///     runtime.
        /// </summary>
        private abstract class AnimationValueAdapter
        {
            /// <summary>
            ///     Gets or sets the original double value.
            /// </summary>
            protected double OriginalValue { get; set; }

            /// <summary>
            ///     Gets the dimension of interest for the control.
            /// </summary>
            public DoubleAnimationDimension Dimension { get; }

            /// <summary>
            ///     Initializes a new instance of the AnimationValueAdapter type.
            /// </summary>
            /// <param name="dimension">The dimension of interest for updates.</param>
            public AnimationValueAdapter(DoubleAnimationDimension dimension) { this.Dimension = dimension; }

            /// <summary>
            ///     Updates the original instance based on new dimension information
            ///     from the control. Takes both and allows the subclass to make the
            ///     decision on which ratio, values, and dimension to use.
            /// </summary>
            /// <param name="width">The width of the control.</param>
            /// <param name="height">The height of the control.</param>
            public abstract void UpdateWithNewDimension(double width, double height);
        }

        /// <summary>
        ///     A selection of dimensions of interest for updating an animation.
        /// </summary>
        private enum DoubleAnimationDimension
        {
            /// <summary>
            ///     The width (horizontal) dimension.
            /// </summary>
            Width,

            /// <summary>
            ///     The height (vertical) dimension.
            /// </summary>
            Height
        }

        /// <summary>
        ///     Adapter for double key frames.
        /// </summary>
        private class DoubleAnimationFrameAdapter : GeneralAnimationValueAdapter<DoubleKeyFrame>
        {
            /// <summary>
            ///     Initializes a new instance of the DoubleAnimationFrameAdapter
            ///     type.
            /// </summary>
            /// <param name="dimension">The dimension of interest.</param>
            /// <param name="frame">The instance of the animation type.</param>
            public DoubleAnimationFrameAdapter(DoubleAnimationDimension dimension, DoubleKeyFrame frame)
                : base(dimension, frame) { }

            /// <summary>
            ///     Gets the value of the underlying property of interest.
            /// </summary>
            /// <returns>Returns the value of the property.</returns>
            protected override double GetValue() { return this.Instance.Value; }

            /// <summary>
            ///     Sets the value for the underlying property of interest.
            /// </summary>
            /// <param name="newValue">The new value for the property.</param>
            protected override void SetValue(double newValue) { this.Instance.Value = newValue; }
        }

        /// <summary>
        ///     Adapter for DoubleAnimation's From property.
        /// </summary>
        private class DoubleAnimationFromAdapter : GeneralAnimationValueAdapter<DoubleAnimation>
        {
            /// <summary>
            ///     Initializes a new instance of the DoubleAnimationFromAdapter
            ///     type.
            /// </summary>
            /// <param name="dimension">The dimension of interest.</param>
            /// <param name="instance">The instance of the animation type.</param>
            public DoubleAnimationFromAdapter(DoubleAnimationDimension dimension, DoubleAnimation instance)
                : base(dimension, instance) { }

            /// <summary>
            ///     Gets the value of the underlying property of interest.
            /// </summary>
            /// <returns>Returns the value of the property.</returns>
            protected override double GetValue() { return (double) this.Instance.From; }

            /// <summary>
            ///     Sets the value for the underlying property of interest.
            /// </summary>
            /// <param name="newValue">The new value for the property.</param>
            protected override void SetValue(double newValue) { this.Instance.From = newValue; }
        }

        /// <summary>
        ///     Adapter for DoubleAnimation's To property.
        /// </summary>
        private class DoubleAnimationToAdapter : GeneralAnimationValueAdapter<DoubleAnimation>
        {
            /// <summary>
            ///     Initializes a new instance of the DoubleAnimationToAdapter type.
            /// </summary>
            /// <param name="dimension">The dimension of interest.</param>
            /// <param name="instance">The instance of the animation type.</param>
            public DoubleAnimationToAdapter(DoubleAnimationDimension dimension, DoubleAnimation instance)
                : base(dimension, instance) { }

            /// <summary>
            ///     Gets the value of the underlying property of interest.
            /// </summary>
            /// <returns>Returns the value of the property.</returns>
            protected override double GetValue() { return (double) this.Instance.To; }

            /// <summary>
            ///     Sets the value for the underlying property of interest.
            /// </summary>
            /// <param name="newValue">The new value for the property.</param>
            protected override void SetValue(double newValue) { this.Instance.To = newValue; }
        }

        private abstract class GeneralAnimationValueAdapter<T> : AnimationValueAdapter
        {
            /// <summary>
            ///     The ratio based on the original identifying value, used for computing
            ///     the updated animation property of interest when the size of the
            ///     control changes.
            /// </summary>
            private readonly double _Ratio;

            /// <summary>
            ///     Stores the animation instance.
            /// </summary>
            protected T Instance { get; }

            /// <summary>
            ///     Gets the initial value (minus the identifying value portion) that the
            ///     designer stored within the visual state animation property.
            /// </summary>
            protected double InitialValue { get; }

            /// <summary>
            ///     Initializes a new instance of the GeneralAnimationValueAdapter
            ///     type.
            /// </summary>
            /// <param name="d">The dimension of interest.</param>
            /// <param name="instance">The animation type instance.</param>
            protected GeneralAnimationValueAdapter(DoubleAnimationDimension d, T instance)
                : base(d)
            {
                this.Instance = instance;

                this.InitialValue = this.StripIdentifyingValueOff(this.GetValue());
                this._Ratio = this.InitialValue / 100;
            }

            /// <summary>
            ///     Gets the value of the underlying property of interest.
            /// </summary>
            /// <returns>Returns the value of the property.</returns>
            protected abstract double GetValue();

            /// <summary>
            ///     Sets the value for the underlying property of interest.
            /// </summary>
            /// <param name="newValue">The new value for the property.</param>
            protected abstract void SetValue(double newValue);

            /// <summary>
            ///     Approximately removes the identifying value from a value.
            /// </summary>
            /// <param name="number">The initial number.</param>
            /// <returns>
            ///     Returns a double with an adjustment for the identifying
            ///     value portion of the number.
            /// </returns>
            public double StripIdentifyingValueOff(double number) { return this.Dimension == DoubleAnimationDimension.Width ? number - .1 : number - .2; }

            /// <summary>
            ///     Retrieves the dimension, if any, from the number. If the number
            ///     does not have an identifying value, null is returned.
            /// </summary>
            /// <param name="number">The double value.</param>
            /// <returns>
            ///     Returns a double animation dimension if the number was
            ///     contained an identifying value; otherwise, returns null.
            /// </returns>
            public static DoubleAnimationDimension? GetDimensionFromIdentifyingValue(double number)
            {
                var floor = Math.Floor(number);
                var remainder = number - floor;

                if (remainder >= .1 - SIMPLE_DOUBLE_COMPARISON_EPSILON && remainder <= .1 + SIMPLE_DOUBLE_COMPARISON_EPSILON)
                    return DoubleAnimationDimension.Width;
                if (remainder >= .2 - SIMPLE_DOUBLE_COMPARISON_EPSILON && remainder <= .2 + SIMPLE_DOUBLE_COMPARISON_EPSILON)
                    return DoubleAnimationDimension.Height;
                return null;
            }

            /// <summary>
            ///     Updates the animation instance based on the dimensions of the
            ///     control.
            /// </summary>
            /// <param name="width">The width of the control.</param>
            /// <param name="height">The height of the control.</param>
            public override void UpdateWithNewDimension(double width, double height)
            {
                var size = this.Dimension == DoubleAnimationDimension.Width ? width : height;
                this.UpdateValue(size);
            }

            /// <summary>
            ///     Updates the value of the property.
            /// </summary>
            /// <param name="sizeToUse">
            ///     The size of interest to use with a ratio
            ///     computation.
            /// </param>
            private void UpdateValue(double sizeToUse) { this.SetValue(sizeToUse * this._Ratio); }
        }

        #endregion
    }
}