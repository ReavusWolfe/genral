using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Ninject;
using ReavusWolfe.Main.Injection;
using ReavusWolfe.Main.Views.Common;

namespace ReavusWolfe.Main
{
    public partial class App
    {
        public static Injector Injector { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetCulture();
            var injector = CreateInjector();

#if (DEBUG)
            RunInDebugMode(injector);
#else
            RunInReleaseMode(injector);
#endif
        }

        private static void SetCulture()
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
        }

        private Injector CreateInjector()
        {
            var kernel = new StandardKernel();
            kernel.Load(new LiveModule());
            var injector = new Injector(kernel);
            kernel.Rebind<IInjector>().ToConstant(injector);
            Injector = injector;
            return injector;
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogException(e.ExceptionObject as Exception);
        }

        private static void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogException(e.Exception);
        }

        private static void LogException(Exception e)
        {
            MessageBox.Show(
                "Something bad has happened! If this problem persists, please report this to an administrator.\n\nError: " +
                e.Message, "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Environment.Exit(1);
        }

        private void RunInDebugMode(IInjector injector)
        {
            Current.MainWindow = injector.GetUniqueInstance<MainWindow>();
            Current.MainWindow.Show();
        }

        private static void RunInReleaseMode(IInjector injector)
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
            Current.DispatcherUnhandledException += Application_DispatcherUnhandledException;

            try
            {
                Current.MainWindow = injector.GetUniqueInstance<MainWindow>();
                Current.MainWindow.Show();
            }
            catch (Exception e)
            {
                LogException(e);
            }
        }
    }
}
