using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IconFontWpf
{
    /// <summary>
    /// Class IconFontControl which is the custom base class for any PackIcon control.
    /// </summary>
    /// <typeparam name="TKind">The type of the enum kind.</typeparam>
    /// <seealso cref="T:ControlzEx.PackIconBase`1" />
    public abstract class IconFontControl<TKind> : IconFontBase<TKind> where TKind : Enum
    {
        /// <summary>Identifies the Flip dependency property.</summary>
        public static readonly DependencyProperty FlipProperty = DependencyProperty.Register(nameof(Flip),
            typeof(IconFontFlipOrientation), typeof(IconFontControl<TKind>),
            new PropertyMetadata(IconFontFlipOrientation.Normal));

        /// <summary>Identifies the Rotation dependency property.</summary>
        public static readonly DependencyProperty RotationProperty = DependencyProperty.Register(nameof(Rotation),
            typeof(double), typeof(IconFontControl<TKind>),
            new PropertyMetadata(0.0, null, RotationPropertyCoerceValueCallback));

        /// <summary>Identifies the Spin dependency property.</summary>
        public static readonly DependencyProperty SpinProperty = DependencyProperty.Register(nameof(Spin), typeof(bool),
            typeof(IconFontControl<TKind>),
            new PropertyMetadata(false, SpinPropertyChangedCallback, SpinPropertyCoerceValueCallback));

        private static readonly string SpinnerStoryBoardName =
            $"{typeof(IconFontControl<TKind>).Name as object}SpinnerStoryBoard";

        /// <summary>Identifies the SpinDuration dependency property.</summary>
        public static readonly DependencyProperty SpinDurationProperty = DependencyProperty.Register(
            nameof(SpinDuration), typeof(double), typeof(IconFontControl<TKind>),
            new PropertyMetadata(1.0, SpinDurationPropertyChangedCallback, SpinDurationCoerceValueCallback));

        /// <summary>
        /// Identifies the SpinEasingFunction dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinEasingFunctionProperty = DependencyProperty.Register(
            nameof(SpinEasingFunction), typeof(IEasingFunction), typeof(IconFontControl<TKind>),
            new PropertyMetadata(null, SpinEasingFunctionPropertyChangedCallback));

        /// <summary>Identifies the SpinAutoReverse dependency property.</summary>
        public static readonly DependencyProperty SpinAutoReverseProperty = DependencyProperty.Register(
            nameof(SpinAutoReverse), typeof(bool), typeof(IconFontControl<TKind>),
            new PropertyMetadata(false, SpinAutoReversePropertyChangedCallback));

        public static readonly DependencyProperty ScaleXProperty = DependencyProperty.Register(
            "ScaleX", typeof(double), typeof(IconFontControl<TKind>), new PropertyMetadata(1d));


        public static readonly DependencyProperty ScaleYProperty = DependencyProperty.Register(
            "ScaleY", typeof(double), typeof(IconFontControl<TKind>), new PropertyMetadata(default(double)));

        private FrameworkElement _innerGrid;

        static IconFontControl()
        {
            OpacityProperty.OverrideMetadata(typeof(IconFontControl<TKind>),
                new UIPropertyMetadata(1.0,
                    (d, e) => d.CoerceValue(SpinProperty)));
            VisibilityProperty.OverrideMetadata(typeof(IconFontControl<TKind>),
                new UIPropertyMetadata(Visibility.Visible,
                    (d, e) => d.CoerceValue(SpinProperty)));
        }

        protected IconFontControl(Func<IDictionary<TKind, string>> dataIndexFactory)
            : base(dataIndexFactory)
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CoerceValue(SpinProperty);
            if (!Spin)
                return;
            StopSpinAnimation();
            BeginSpinAnimation();
        }

        /// <summary>Gets or sets the flip orientation.</summary>
        public IconFontFlipOrientation Flip
        {
            get => (IconFontFlipOrientation) GetValue(FlipProperty);
            set => SetValue(FlipProperty, value);
        }

        private static object RotationPropertyCoerceValueCallback(
            DependencyObject dependencyObject,
            object value)
        {
            double num = (double) value;
            if (num < 0.0)
                return 0.0;
            if (num <= 360.0)
                return value;
            return 360.0;
        }

        /// <summary>Gets or sets the rotation (angle).</summary>
        /// <value>The rotation.</value>
        public double Rotation
        {
            get => (double) GetValue(RotationProperty);
            set => SetValue(RotationProperty, value);
        }

        private static object SpinPropertyCoerceValueCallback(
            DependencyObject dependencyObject,
            object value)
        {
            if (dependencyObject is IconFontControl<TKind> iconFontControl &&
                (!iconFontControl.IsVisible || iconFontControl.Opacity <= 0.0 ||
                 iconFontControl.SpinDuration <= 0.0))
                return false;
            return value;
        }

        private static void SpinPropertyChangedCallback(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(dependencyObject is IconFontControl<TKind> iconFontControl) || e.OldValue == e.NewValue ||
                !(e.NewValue is bool))
                return;
            if ((bool) e.NewValue)
                iconFontControl.BeginSpinAnimation();
            else
                iconFontControl.StopSpinAnimation();
        }

        private FrameworkElement InnerGrid =>
            _innerGrid ??
            (_innerGrid = GetTemplateChild("PART_InnerGrid") as FrameworkElement);

        private void BeginSpinAnimation()
        {
            FrameworkElement innerGrid = InnerGrid;
            if (innerGrid == null)
                return;
            TransformGroup transformGroup = innerGrid.RenderTransform as TransformGroup ?? new TransformGroup();
            RotateTransform rotateTransform =
                transformGroup.Children.OfType<RotateTransform>().LastOrDefault();
            if (rotateTransform != null)
            {
                rotateTransform.Angle = 0.0;
            }
            else
            {
                transformGroup.Children.Add(new RotateTransform());
                innerGrid.RenderTransform = transformGroup;
            }

            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation1 = new DoubleAnimation
            {
                From = 0.0,
                To = 360.0,
                AutoReverse = SpinAutoReverse,
                EasingFunction = SpinEasingFunction,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new Duration(TimeSpan.FromSeconds(SpinDuration))
            };
            DoubleAnimation doubleAnimation2 = doubleAnimation1;
            storyboard.Children.Add(doubleAnimation2);
            Storyboard.SetTarget(doubleAnimation2, innerGrid);
            Storyboard.SetTargetProperty(doubleAnimation2,
                new PropertyPath("(0).(1)[2].(2)", RenderTransformProperty as object,
                    TransformGroup.ChildrenProperty as object, RotateTransform.AngleProperty as object));
            innerGrid.Resources.Add(SpinnerStoryBoardName, storyboard);
            storyboard.Begin();
        }

        private void StopSpinAnimation()
        {
            FrameworkElement innerGrid = InnerGrid;
            if (!(innerGrid?.Resources[SpinnerStoryBoardName] is Storyboard resource))
                return;
            resource.Stop();
            innerGrid.Resources.Remove(SpinnerStoryBoardName);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the inner icon is spinning.
        /// </summary>
        /// <value><c>true</c> if spin; otherwise, <c>false</c>.</value>
        public bool Spin
        {
            get => (bool) GetValue(SpinProperty);
            set => SetValue(SpinProperty, value);
        }

        private static void SpinDurationPropertyChangedCallback(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(dependencyObject is IconFontControl<TKind> iconFontControl) || e.OldValue == e.NewValue ||
                (!iconFontControl.Spin || !(e.NewValue is double)))
                return;
            iconFontControl.StopSpinAnimation();
            iconFontControl.BeginSpinAnimation();
        }

        private static object SpinDurationCoerceValueCallback(
            DependencyObject dependencyObject,
            object value)
        {
            if ((double) value >= 0.0)
                return value;
            return 0.0;
        }

        /// <summary>
        /// Gets or sets the duration of the spinning animation (in seconds). This will also restart the spin animation.
        /// </summary>
        /// <value>The duration of the spin in seconds.</value>
        public double SpinDuration
        {
            get => (double) GetValue(SpinDurationProperty);
            set => SetValue(SpinDurationProperty, value);
        }

        private static void SpinEasingFunctionPropertyChangedCallback(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(dependencyObject is IconFontControl<TKind> iconFontControl) || e.OldValue == e.NewValue ||
                !iconFontControl.Spin)
                return;
            iconFontControl.StopSpinAnimation();
            iconFontControl.BeginSpinAnimation();
        }

        /// <summary>
        /// Gets or sets the EasingFunction of the spinning animation. This will also restart the spin animation.
        /// </summary>
        /// <value>The spin easing function.</value>
        public IEasingFunction SpinEasingFunction
        {
            get => (IEasingFunction) GetValue(SpinEasingFunctionProperty);
            set => SetValue(SpinEasingFunctionProperty, value);
        }

        private static void SpinAutoReversePropertyChangedCallback(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(dependencyObject is IconFontControl<TKind> iconFontControl) || e.OldValue == e.NewValue ||
                (!iconFontControl.Spin || !(e.NewValue is bool)))
                return;
            iconFontControl.StopSpinAnimation();
            iconFontControl.BeginSpinAnimation();
        }

        /// <summary>
        /// Gets or sets the AutoReverse of the spinning animation. This will also restart the spin animation.
        /// </summary>
        /// <value><c>true</c> if [spin automatic reverse]; otherwise, <c>false</c>.</value>
        public bool SpinAutoReverse
        {
            get => (bool) GetValue(SpinAutoReverseProperty);
            set => SetValue(SpinAutoReverseProperty, value);
        }

        public double ScaleX
        {
            get => (double) GetValue(ScaleXProperty);
            set => SetValue(ScaleXProperty, value);
        }

        public double ScaleY
        {
            get => (double) GetValue(ScaleYProperty);
            set => SetValue(ScaleYProperty, value);
        }
    }
}