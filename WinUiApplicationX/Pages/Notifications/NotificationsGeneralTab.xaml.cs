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
        }

        private void CreateBorder(TextBox titleTextBox, int width, int height, int left, int top)
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
                Name = $"{nameof(titleTextBox)}Rectangle",
                Width = width,
                Height = height,
                Style = (Style)Resources["InteractiveBorderStyle"]
            };

            rectangle.SetValue(Canvas.TopProperty, top);
            rectangle.SetValue(Canvas.LeftProperty, left);

            //Visual visual = ElementCompositionPreview.GetElementVisual(rectangle);
            //visual.CenterPoint = new Vector3((float)rectangle.Width / 2, (float)rectangle.Height / 2, 0);

            titleTextBox.GotFocus += (s, e) =>
            {
                rectangle.Visibility = Visibility.Visible;
                rectangle.StartAnimation(springAnimation);
            };
            titleTextBox.LostFocus += (s, e) => rectangle.Visibility = Visibility.Collapsed;

            InteractiveCanvas.Children.Add(rectangle);
            
            // Moving ant animation
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
        }
    }
}
