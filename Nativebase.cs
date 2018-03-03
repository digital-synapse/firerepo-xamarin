using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FireRepo.Xamarin
{
    internal  class Nativebase
    {
        public static async Task<T> Get<T>(string name)
        {
            var tcs = new TaskCompletionSource<T>();
            var obj = default( T );
            var task= Task.Factory.StartNew(() =>
            {
                try
                {

                    var ctx = Application.Context;
                    var sharedPref = ctx.GetSharedPreferences("data", FileCreationMode.Private);
                    var json = sharedPref.GetString(name, null);
                    if (json != null)
                    {
                        obj = JsonConvert.DeserializeObject<T>(json);                        
                    }
                    tcs.SetResult(obj);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                    Debugger.Break();
                }
            });
            return await tcs.Task;
        }       

        public static async Task<bool> Put(string name, object obj)
        {
            var ok = false;
            var tcs = new TaskCompletionSource<bool>();
            var task= Task.Factory.StartNew(() => { 
                try
                {
                    var ctx = Application.Context;
                    var sharedPref = ctx.GetSharedPreferences("data", FileCreationMode.Private);
                    var editor = sharedPref.Edit();
                    var json = JsonConvert.SerializeObject(obj);
                    editor.PutString(name, json);
                    editor.Commit();
                    ok = true;
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                    tcs.SetException(ex);
                }
            });
            return await tcs.Task;
        }

    }
}