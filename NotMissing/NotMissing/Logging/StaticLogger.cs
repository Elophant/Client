namespace NotMissing.Logging
{
    /// <summary>
    /// Static methods for accessing Logger.Instance
    /// </summary>
    public static class StaticLogger
    {
        public static void Register(ILogListener listener) { Logger.Instance.Register(listener); }
        public static void Unregister(ILogListener listener) { Logger.Instance.Unregister(listener); }
        public static void Log(Levels level, object obj) { Logger.Instance.Log(level, obj); }
        public static void Trace(object obj) { Logger.Instance.Log(Levels.Trace, obj); }
        public static void Debug(object obj) { Logger.Instance.Log(Levels.Debug, obj); }
        public static void Warning(object obj) { Logger.Instance.Log(Levels.Warning, obj); }
        public static void Info(object obj) { Logger.Instance.Log(Levels.Info, obj); }
        public static void Error(object obj) { Logger.Instance.Log(Levels.Error, obj); }
        public static void Fatal(object obj) { Logger.Instance.Log(Levels.Fatal, obj); }
    }
}