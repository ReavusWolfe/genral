using System;
using Microsoft.Win32;

namespace ReavusWolfe.Services
{
    public interface IRegistryService : IDisposable
    {
        double GetWindowScale();
        void SetWindowScale(double scaleValue);
        string GetUsername();
        void SetUsername(string username);
    }

    public class RegistryService : IRegistryService
    {
        private const string REGISTRY_PATH = "SOFTWARE\\TotalRen\\BoilerPlate\\";

        public string GetUsername()
        {
            try
            {
                var myKey = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true);

                if (myKey == null)
                    return null;

                var username = myKey.GetValue("Username") as string;

                if (string.IsNullOrWhiteSpace(username))
                    return null;

                return username.ToUpper();
            }
            catch (Exception)
            {
                // Do nothing
            }
            return null;
        }

        public void SetUsername(string username)
        {
            try
            {
                Registry.CurrentUser.CreateSubKey(REGISTRY_PATH);

                var myKey = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true);

                if (myKey == null)
                    return;

                myKey.SetValue("Username", username.ToUpper(), RegistryValueKind.String);
            }
            catch
            {
                // Do nothing
            }
        }

        public double GetWindowScale()
        {
            const int DEFAULT_SCALE = 1;
            try
            {
                var myKey = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true);

                if (myKey == null)
                    return DEFAULT_SCALE;

                return Convert.ToDouble(myKey.GetValue("WindowScaleValue"));
            }
            catch (Exception)
            {
                // Do nothing
            }
            return DEFAULT_SCALE;
        }

        public void SetWindowScale(double scaleValue)
        {
            try
            {
                Registry.CurrentUser.CreateSubKey(REGISTRY_PATH);

                var myKey = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true);

                if (myKey == null)
                    return;

                myKey.SetValue("WindowScaleValue", scaleValue, RegistryValueKind.String);
            }
            catch
            {
                // Do nothing
            }
        }

        public void Dispose()
        {

        }
    }
}
