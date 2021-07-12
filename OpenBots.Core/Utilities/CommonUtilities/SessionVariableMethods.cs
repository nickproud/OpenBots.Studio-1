using OpenBots.Core.Interfaces;

namespace OpenBots.Core.Utilities.CommonUtilities
{
    public class SessionVariableMethods
    {
        public static string GetJobId(IAutomationEngineInstance engine)
        {
            var sessionVariablesDict = engine.EngineContext.SessionVariables;
            object value;

            if (sessionVariablesDict != null && sessionVariablesDict.Count > 0)
            {
                bool keyExists = sessionVariablesDict.TryGetValue("JobId", out value);
                if (keyExists)
                    return value.ToString();
            }

            return null;
        }

        public static void SetJobId(IAutomationEngineInstance engine, string jobId)
        {
            var sessionVariablesDict = engine.EngineContext.SessionVariables;
            sessionVariablesDict.Add("JobId", jobId);
        }
    }
}
