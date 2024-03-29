﻿using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Library.Wpf.Media;

public static class Animations
{
    public static void AnimateDouble([DisallowNull] FrameworkElement element, [DisallowNull] string propertyPath, double from, double to, int duration = 350)
    {
        Check.MustBeArgumentNotNull(element);
        Check.MustBeArgumentNotNull(propertyPath);

        var animation = new DoubleAnimation(from, to, new Duration(TimeSpan.FromMilliseconds(duration)));
        var storyboard = new Storyboard();
        storyboard.Children.Add(animation);
        Storyboard.SetTargetName(animation, element.Name);
        Storyboard.SetTargetProperty(animation, new PropertyPath(propertyPath));

        element.Visibility = Visibility.Visible;
        storyboard.Begin(element, true);
    }

    public static void AnimateDouble(FrameworkElement element, DependencyProperty prop, double from, double to, int duration = 350, bool autoReverse = false)
    {
        Check.MustBeArgumentNotNull(element);
        var animation = new DoubleAnimation(from, to, new Duration(TimeSpan.FromMilliseconds(duration)));
        var storyboard = new Storyboard { AutoReverse = autoReverse };
        storyboard.Children.Add(animation);
        Storyboard.SetTargetName(animation, element.Name);
        Storyboard.SetTargetProperty(animation, new PropertyPath(prop));

        element.Visibility = Visibility.Visible;
        storyboard.Begin(element, true);
    }

    public static void FadeIn(FrameworkElement element, double toValue = 1, int duration = 350)
        => AnimateDouble(element, UIElement.OpacityProperty, element.Opacity, toValue, duration);

    public static void FadeOut(FrameworkElement element, double toValue = 0, int duration = 350)
        => AnimateDouble(element, UIElement.OpacityProperty, element.Opacity, toValue, duration);

    public static void Flick(FrameworkElement element, int duration = 500)
        => AnimateDouble(element, UIElement.OpacityProperty, 0, 1, duration, true);

    public static void MoveIn(FrameworkElement element, int fromValue = 5000, int toValue = 0, int duration = 350)
        => InnerMove(Storyboard.SetTarget, element, fromValue, toValue, duration);

    public static void MoveOut(FrameworkElement element, int fromValue = 0, int toValue = 5000, int duration = 350)
        => InnerMove((a, e) => Storyboard.SetTargetName(a, e.Name), element, fromValue, toValue, duration);

    public static void SkewIn(FrameworkElement element, int duration = 350)
    {
        var animation = new DoubleAnimation(90, 0, new Duration(TimeSpan.FromMilliseconds(duration)));
        element.RenderTransform = new SkewTransform(0, 0);
        Storyboard.SetTargetName(animation, element.Name);
        Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(SkewTransform.AngleX)"));
        var storyboard = new Storyboard();
        storyboard.Children.Add(animation);

        element.Visibility = Visibility.Visible;
        storyboard.Begin(element);
    }

    public static void SkewOut(FrameworkElement element, int duration = 350)
    {
        Check.MustBeArgumentNotNull(element);
        var animation = new DoubleAnimation(0, 90, new Duration(TimeSpan.FromMilliseconds(duration)));
        element.RenderTransform = new SkewTransform(0, 0);
        Storyboard.SetTargetName(animation, element.Name);
        Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(SkewTransform.AngleX)"));
        var storyboard = new Storyboard();
        storyboard.Children.Add(animation);

        element.Visibility = Visibility.Visible;
        storyboard.Begin(element);
    }

    private static void InnerMove(Action<DoubleAnimation, FrameworkElement> animate, FrameworkElement element, int fromValue = 5000, int toValue = 0, int duration = 350)
    {
        Check.MustBeArgumentNotNull(element);

        var animation = new DoubleAnimation(fromValue, toValue, new Duration(TimeSpan.FromMilliseconds(duration))) { DecelerationRatio = 0.65 };
        element.RenderTransform = new TranslateTransform(0, 0);
        animate(animation, element);
        Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
        var storyboard = new Storyboard();
        storyboard.Children.Add(animation);

        element.Visibility = Visibility.Visible;
        storyboard.Begin(element);
    }
}