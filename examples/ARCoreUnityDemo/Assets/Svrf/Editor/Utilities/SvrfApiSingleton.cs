namespace Svrf.Unity.Editor.Utilities
{
    internal class SvrfApiSingleton
    {
        private static SvrfApi _instance;

        internal static SvrfApi Instance => _instance ?? (_instance = new SvrfApi());
    }
}
