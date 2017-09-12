using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using ReavusWolfe.Main.ViewModels;
using ReavusWolfe.Main;
using ReavusWolfe.Main.ViewModels;
using ReavusWolfe.Services;

namespace ReavusWolfe.Main.Views
{
    public class ReavusWolfeWindow : MetroWindow
    {
        private ReavusWolfeViewModelBase _viewModel;
        readonly ScaleTransform _transform = new ScaleTransform();

        private static double _scaleValue;
        private const double SCALE_INCREMENT = 0.1;
        private const int SCALE_DEFAULT = 1;
        private const double MAX_SCALE = 1.5;
        private const double MIN_SCALE = 0.8;

        public bool ShowZoomButtons { get; set; }

        public bool AutoFocusToFirstControl { get; set; } = true;

        public static double ScaleValue
        {
            get
            {
                if (_scaleValue <= MIN_SCALE)
                    return MIN_SCALE;
                return _scaleValue;
            }
            set
            {
                if (value > MAX_SCALE)
                    return;
                if (value < MIN_SCALE)
                    return;
                _scaleValue = value;

                using (var messengerService = App.Injector.GetUniqueInstance<IMessengerService>())
                using (var registryService = App.Injector.GetUniqueInstance<IRegistryService>())
                {
                    messengerService.Send(new ScaleChangedMessage());
                    registryService.SetWindowScale(value);
                }

            }
        }

        protected ReavusWolfeViewModelBase ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                _viewModel.Close += Close;
                DataContext = _viewModel;
            }
        }

        public ReavusWolfeWindow()
        {
            using (var registryService = App.Injector.GetUniqueInstance<IRegistryService>())
            {
                _scaleValue = registryService.GetWindowScale();
            }

            ShowZoomButtons = false;
            BorderThickness = new Thickness(1);
            BorderBrush = (Brush)Application.Current.Resources["AccentColorBrush"];
            GlowBrush = (SolidColorBrush)Application.Current.Resources["AccentColorBrush"];

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var vm = DataContext as ReavusWolfeViewModelBase;
            if (vm != null)
                vm.Cleanup();
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            if (Owner != null)
                Owner.Activate();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (Owner != null)
            {
                Topmost = true;
                ShowInTaskbar = false;
            }
            else
                ShowInTaskbar = true;
        }

        private void Window_Deactived(object sender, EventArgs e)
        {
            if (Owner == null || Owner.IsActive)
            {
                return;
            }
            bool hasActiveWindow = false;
            foreach (Window ownedWindow in Owner.OwnedWindows)
            {
                if (ownedWindow.IsActive)
                    hasActiveWindow = true;
            }

            if (!hasActiveWindow)
                Topmost = false;
        }
        private void RefreshScale()
        {
            _transform.ScaleX = ScaleValue;
            _transform.ScaleY = ScaleValue;
        }

        public class ScaleChangedMessage { }
        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var child = VisualTreeHelper.GetChild(this, 0);
            var grid = child as Grid;
            if (grid != null)
            {
                _transform.CenterX = 0;
                _transform.CenterY = 0;
                _transform.ScaleX = ScaleValue;
                _transform.ScaleY = ScaleValue;

                grid.LayoutTransform = _transform;

                var messenger = App.Injector.GetUniqueInstance<IMessengerService>();
                messenger.Register<ScaleChangedMessage>(this, m => RefreshScale());

                if (ShowZoomButtons)
                {
                    var commands = new WindowCommands();

                    MakeZoomInButton(commands);
                    MakeZoomOutButton(commands);
                    MakeResetZoomButton(commands);

                    SetValue(RightWindowCommandsProperty, commands);
                }
            }

            if (AutoFocusToFirstControl)
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private static readonly ICommand ZoomInCommand = new RelayCommand(IncreaseScale);
        private static readonly ICommand ZoomOutCommand = new RelayCommand(DecreaseScale);
        private static readonly ICommand ResetCommand = new RelayCommand(ResetScale);

        private static void MakeZoomInButton(WindowCommands commands)
        {
            var binding = new Binding
            {
                RelativeSource = new RelativeSource
                {
                    AncestorType = typeof(Button)
                },
                Path = new PropertyPath(ForegroundProperty)
            };
            var rect = new Rectangle
            {
                Width = 20,
                Height = 20,
                OpacityMask = new VisualBrush
                {
                    Stretch = Stretch.Fill,
                    Visual = (Visual)Application.Current.Resources["appbar_magnify_add"]
                }
            };
            rect.SetBinding(Shape.FillProperty, binding);

            var text = new TextBlock
            {
                Margin = new Thickness(4, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                Text = "zoom in"
            };

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            stackPanel.Children.Add(rect);
            stackPanel.Children.Add(text);

            var zoomIn = new Button
            {
                Command = ZoomInCommand,
                Content = stackPanel
            };
            commands.Items.Add(zoomIn);
        }

        private static void MakeZoomOutButton(WindowCommands commands)
        {
            var binding = new Binding
            {
                RelativeSource = new RelativeSource
                {
                    AncestorType = typeof(Button)
                },
                Path = new PropertyPath(ForegroundProperty)
            };
            var rect = new Rectangle
            {
                Width = 20,
                Height = 20,
                OpacityMask = new VisualBrush
                {
                    Stretch = Stretch.Fill,
                    Visual = (Visual)Application.Current.Resources["appbar_magnify_minus"]
                }
            };
            rect.SetBinding(Shape.FillProperty, binding);

            var text = new TextBlock
            {
                Margin = new Thickness(4, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                Text = "zoom out"
            };

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            stackPanel.Children.Add(rect);
            stackPanel.Children.Add(text);

            var zoomIn = new Button
            {
                Command = ZoomOutCommand,
                Content = stackPanel
            };
            commands.Items.Add(zoomIn);
        }

        private static void MakeResetZoomButton(WindowCommands commands)
        {
            var binding = new Binding
            {
                RelativeSource = new RelativeSource
                {
                    AncestorType = typeof(Button)
                },
                Path = new PropertyPath(ForegroundProperty)
            };
            var rect = new Rectangle
            {
                Width = 20,
                Height = 20,
                OpacityMask = new VisualBrush
                {
                    Stretch = Stretch.Fill,
                    Visual = (Visual)Application.Current.Resources["appbar_magnify"]
                }
            };
            rect.SetBinding(Shape.FillProperty, binding);

            var text = new TextBlock
            {
                Margin = new Thickness(4, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                Text = "reset zoom"
            };

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            stackPanel.Children.Add(rect);
            stackPanel.Children.Add(text);

            var zoomIn = new Button
            {
                Command = ResetCommand,
                Content = stackPanel
            };
            commands.Items.Add(zoomIn);
        }

        private static void ResetScale()
        {
            ScaleValue = SCALE_DEFAULT;
        }

        private static void DecreaseScale()
        {
            ScaleValue -= SCALE_INCREMENT;
        }

        private static void IncreaseScale()
        {
            ScaleValue += SCALE_INCREMENT;
        }

    }

    public class DetailsWindow : ReavusWolfeWindow
    {
        private DetailsViewModelBase _viewModel;

        protected new DetailsViewModelBase ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                _viewModel.Close += Close;
                DataContext = _viewModel;
            }
        }

        public void LoadNewObject(int id)
        {
            ViewModel.LoadNewObject(id);
        }

        public void LoadExistingObject(int id)
        {
            ViewModel.LoadExistingObject(id);
        }
    }
}
