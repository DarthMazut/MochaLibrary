using Microsoft.UI;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using ViewModelsX.Pages.Notfifications;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiApplicationX.Pages.Notifications
{
    public sealed partial class NotificationsGeneralTab : UserControl
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(NotificationsGeneralTabViewModel), typeof(NotificationsGeneralTab), new PropertyMetadata(null));

        public NotificationsGeneralTabViewModel ViewModel
        {
            get => (NotificationsGeneralTabViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public NotificationsGeneralTab()
        {
            this.InitializeComponent();
            CreateInteractiveBorders();
        }

        private void CreateInteractiveBorders()
        {
            CreateBorder(TitleTextBox, 80, 30, 70, 195);
            CreateBorder(ContentTextBox, 80, 30, 70, 215);
        }

        private void CreateBorder(UIElement element, int width, int height, int left, int top)
        {
            // Spring animation
            Compositor compositor = CompositionTarget.GetCompositorForCurrentThread();
            SpringVector3NaturalMotionAnimation springAnimation = compositor.CreateSpringVector3Animation();
            springAnimation.Target = "Scale";
            springAnimation.InitialValue = new(0);
            springAnimation.FinalValue = new(1);

            // Create rectangle
            Rectangle rectangle = new()
            {
                Width = width,
                Height = height,
                Stroke = new SolidColorBrush(Colors.Orange),
                StrokeDashArray = [2],
                StrokeThickness = 2,
                Visibility = Visibility.Collapsed,
                CenterPoint = new(width/2,height/2,0)
            };

            rectangle.SetValue(Canvas.TopProperty, top);
            rectangle.SetValue(Canvas.LeftProperty, left);

            element.GotFocus += (s, e) =>
            {
                rectangle.Visibility = Visibility.Visible;
                rectangle.StartAnimation(springAnimation);
            };
            element.LostFocus += (s, e) => rectangle.Visibility = Visibility.Collapsed;

            InteractiveCanvas.Children.Add(rectangle);

            // Any animation
            ExpressionAnimation antAnimation = CompositionTarget.GetCompositorForCurrentThread().CreateExpressionAnimation("rect.StrokeDashOffset-4");
            antAnimation.SetExpressionReferenceParameter("rect", rectangle);
            antAnimation.Target = "StrokeDashOffset";
            antAnimation.
            rectangle.StartAnimation(antAnimation);

            // Moving ant animation
            /*
            DoubleAnimation doubleAnimation = new()
            {
                EnableDependentAnimation = true,
                RepeatBehavior = RepeatBehavior.Forever,
                To = -4,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
            };

            Storyboard storyboard = new();
            storyboard.Children.Add(doubleAnimation);

            Storyboard.SetTarget(doubleAnimation, rectangle);
            Storyboard.SetTargetProperty(doubleAnimation, "StrokeDashOffset");

            storyboard.Begin();
            */
        }
    }
}
