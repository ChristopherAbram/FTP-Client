using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Helper
{
    public class Application : Helper
    {
        private static Application __instance = null;

        public static Application getInstance()
        {
            if (__instance == null)
                __instance = new Application();
            return __instance;
        }


        public override void initialize()
        {
            // Init application registry:
            try {
                Registry.Application registry = Registry.Application.getInstance();
                registry.init();
            } catch(Registry.Exception rex)
            {
                throw new Exception("Unable to load application registry", rex);
            }

            // Init control flow map for application:
            try
            {
                Control.Control control = new Control.Control(Registry.Application.getControlFlowFilepath());
                control.Parse();
                Registry.Application.setApplicationControllerMap(control.Map);
            }
            catch (Control.Exception cex)
            {
                throw new Exception("Unable to load control flow application map", cex);
            }
        }

        private Application() { }
    }
}
