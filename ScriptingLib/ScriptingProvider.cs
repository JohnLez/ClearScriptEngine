using ScriptingLib.DAL;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptingLib.WebAccess;


namespace ScriptingLib
{
    public class ScriptingProvider
    {

        private const string scriptHostAccessObjectName = "xHost";
        private const string scriptTypeAccessObjectName = "xLib";

        public DbAccessProvider DbAccessibility { get; set; }
        public WebProvider WebAccessibility { get; set; }

        public ScriptingProvider(DbAccessProvider db, WebProvider web)
        {
            DbAccessibility = db;
            WebAccessibility = web;
        }



        /// <summary>
        /// Executes a V8 engine script
        /// </summary>
        /// <typeparam name="T">The type T to deseriliaze the script result into</typeparam>
        /// <param name="scriptContent">The content of the script to execute</param>
        /// <param name="parms">Any parameteres to pass into the first method that executes inside the script</param>
        /// <param name="typesToLoad">Types of data to load to enable C# capabilities inside script (ClearCript magic)</param>
        /// <returns>The deserialed script result as object T</returns>
        public T ExecuteScript<T>(string scriptContent, object[] parms, List<Type> typesToLoad = null,bool enableDebugging = false) where T : class
        {
            dynamic returnobj;
            if(enableDebugging)
            {
                using (var engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableDebugging |
                V8ScriptEngineFlags.AwaitDebuggerAndPauseOnStart, 9222))
                {
                    LoadHostConfiguration(engine, typesToLoad);
                    engine.Execute(scriptContent);
                    returnobj = engine.Script.custom(parms);

                    return JsonConvert.DeserializeObject<T>(returnobj);
                }
            }
            else
            {
                using (var engine = new V8ScriptEngine())
                {
                    LoadHostConfiguration(engine, typesToLoad);
                    engine.Execute(scriptContent);
                    returnobj = engine.Script.custom(parms);

                    return JsonConvert.DeserializeObject<T>(returnobj);
                }
            }

        }

        /// <summary>
        /// Executes a V8 engine script
        /// </summary>
        /// <param name="scriptContent">The content of the script to execute</param>
        /// <param name="parms">Any parameteres to pass into the first method that executes inside the script</param>
        /// <param name="typesToLoad">Types of data to load to enable C# capabilities inside script (ClearCript magic)</param>
        /// <returns>An object whose operations will be resolved in runtime</returns>
        public dynamic ExecuteScript(string scriptContent, object[] parms , List<Type> typesToLoad = null, bool enableDebugging = false)
        {
            dynamic returnobj;
            if(enableDebugging)
            {
                using (var engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableDebugging |
                            V8ScriptEngineFlags.AwaitDebuggerAndPauseOnStart, 9222))
                {
                    LoadHostConfiguration(engine, typesToLoad);
                    engine.Execute(scriptContent);
                    returnobj = engine.Script.custom(parms);

                    return returnobj;
                }
            }
            else
            {
                using (var engine = new V8ScriptEngine())
                {
                    LoadHostConfiguration(engine, typesToLoad);
                    engine.Execute(scriptContent);
                    returnobj = engine.Script.custom(parms);

                    return returnobj;
                }
            }
            
        }

        /// <summary>
        /// Loads root level objects into the script engine
        /// </summary>
        /// <param name="engine">The V8ScriptEngine</param>
        /// <param name="typesToLoad">The C# types to load into the engine for extended C# capabilities</param>
        private void LoadHostConfiguration(V8ScriptEngine engine, List<Type> typesToLoad)
        {
            engine.AddHostType(typeof(JsonConvert));
            engine.AddHostType("Console", typeof(Console));
            engine.AddHostObject("db", DbAccessibility);
            engine.AddHostObject("web", WebAccessibility);
            engine.AddHostObject(scriptHostAccessObjectName, new HostFunctions());
            engine.AddHostObject(scriptTypeAccessObjectName, new ExtendedHostFunctions());
            if (typesToLoad != null)
                typesToLoad.ForEach(t => engine.AddHostType(t.Name.ToString(), t));
        }
    }
}
