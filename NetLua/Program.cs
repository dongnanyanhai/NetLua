using NLua;
using System;
using System.Text;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace NetLua
{
    internal static class Program
    {

        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            try
            {

                using (Lua lua = new Lua())
                {
                    lua.State.Encoding = Encoding.UTF8;

                    string[] args = Environment.GetCommandLineArgs();

                    lua.NewTable("arg");

                    LuaTable argc = (LuaTable)lua["arg"];

                    argc[0] = "NetLua";

                    for (int i = 0; i < args.Length; i++)
                    {
                        argc[i + 1] = args[i];
                    }

                    argc["n"] = args.Length;

                    lua.LoadCLRPackage();

                    try
                    {
                        string filepath = "bootstrap.nlua";

                        if (args.Length >= 2 && (args[1] != null && args[1] != ""))
                        {
                            filepath = args[1];
                        }

                        lua.DoFile(filepath);
                    }
                    catch (Exception e)
                    {

                        string trace = "";

                        if (e.StackTrace != null)
                        {
                            trace = e.StackTrace;
                            if (e.StackTrace.Length > 1300)
                            {
                                trace = e.StackTrace.Substring(0, 1300) + " [...] (traceback cut short)";
                            }
                        }

                        MessageBox.Show(e.Message + Environment.NewLine + (e.Source + " raised a " + e.GetType().ToString()) + Environment.NewLine + trace);

                    }
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message + Environment.NewLine + (e.Source + " raised a " + e.GetType().ToString()));
            }

        }
    }
}