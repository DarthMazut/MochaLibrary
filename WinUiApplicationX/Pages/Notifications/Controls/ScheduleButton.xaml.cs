using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiApplicationX.Pages.Notifications.Controls
{
    public sealed partial class ScheduleButton : UserControl
    {
        private static readonly Vector3KeyFrameAnimation _showPaperplaneAnimation;

        public static readonly DependencyProperty IsScheduableProperty =
            DependencyProperty.Register(nameof(IsScheduable), typeof(bool), typeof(ScheduleButton), new PropertyMetadata(false));

        public static readonly DependencyProperty ProgressValueProperty =
            DependencyProperty.Register(nameof(ProgressValue), typeof(double), typeof(ScheduleButton), new PropertyMetadata(0));
        
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ScheduleButton), new PropertyMetadata(null));

        public bool IsScheduable
        {
            get => (bool)GetValue(IsScheduableProperty);
            set => SetValue(IsScheduableProperty, value);
        }

        public double ProgressValue
        {
            get => (double)GetValue(ProgressValueProperty);
            set => SetValue(ProgressValueProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public ScheduleButton()
        {
            this.InitializeComponent();
            SetupAnimations();
            RegisterPropertyChangedCallback(IsScheduableProperty, OnScheduableChanged);
        }

        private void SetupAnimations()
        {
            _showPaperplaneAnimation = CompositionTarget.GetCompositorForCurrentThread().CreateVector3KeyFrameAnimation();

        }

        private void OnScheduableChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (sender is ScheduleButton thisControl)
            {
                if (thisControl.IsScheduable)
                {
                    Compositor compositor = CompositionTarget.GetCompositorForCurrentThread();
                    Vector3KeyFrameAnimation translationAnimation = compositor.CreateVector3KeyFrameAnimation();
                }
                else
                {

                }
            } 
        }
    }
}
