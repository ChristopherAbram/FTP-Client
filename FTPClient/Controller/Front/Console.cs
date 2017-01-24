using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPClient.Request;

namespace FTPClient.Controller.Front
{
    using Interpreter;
    using FTPClient.Controller;
    using Request;
    using Response;
    using Command;
    using View;

    public class Console : Controller
    {

        private static Console __instance = null;
        private Helper.Application __applicationHelper = null;

        public static Console getInstance()
        {
            if (__instance == null)
                __instance = new Console();
            return __instance;
        }

        public new static void run()
        {
            Console instance = getInstance();
            // Initialize application:
            instance._init();
            // Handle application requests:
            instance._handle();
            return;
        }

        protected override void _init()
        {
            try
            {
                __applicationHelper = Helper.Application.getInstance();
                // Application helper initializes whole application registries, controls and settings:
                __applicationHelper.initialize();
            }
            catch (Helper.Exception hex)
            {
                throw new Exception("Unable to initialize application", hex);
            }
        }

        protected override void _handle()
        {
            Request request = new Request();
            do {
                
                View.State state = View.State.END;
                do {
                    try
                    {
                        // Init application controller:
                        Application.Controller applicationController = new Application.Controller(Registry.Application.getApplicationControllerMap(), request, Request.CMD);
                        Response response = null;
                        Command command = null;

                        // Execute commands until forwarding ends:
                        while ((command = applicationController.GetCommand()) != null)
                        {
                            // Execute found command:
                            command.execute(request);
                            // Acquire response:
                            if (response != null)
                                response.Join(command.response);
                            else
                                response = command.response;
                        }

                        if (response != null)
                        {
                            // Prepare view:
                            response.View = applicationController.GetView();
                            state = _invokeView(response);
                            System.Console.WriteLine("");
                            request = Registry.Request.getInstance().get();
                        }
                    
                    }
                    catch (System.Exception ex)
                    {
                        _handleError(ex);
                    }

                    // Clear last command:
                    request.setCommand(null);

                } while (state == View.State.REPEAT);

                // Interpret command giving command prompt:
                Command interpret = new Interpret();
                interpret.execute(request);
                // Set request from interpreter:
                request = Registry.Request.getInstance().get();

            } while (true);
        }

        protected void _handleError(System.Exception ex)
        {
            //System.Console.WriteLine("Following errors have occurred: ");
            int i = 1;
            do
            {
                System.Console.WriteLine("[" + i + "] " + ex.Message);
                ++i;
            } while ((ex = ex.InnerException) != null);
        }

        protected View.State _invokeView(Response response)
        {
            try
            {
                string viewname = response.View;
                Type baseview = Type.GetType("FTPClient.View.View");

                Type classname = null;
                if ((classname = Type.GetType(baseview.Namespace + ".Console." + viewname)) != null)
                {
                    if (classname.IsClass && classname.IsSubclassOf(baseview))
                    {
                        View view = Activator.CreateInstance(classname) as View;
                        if (view != null)
                        {
                            return view.run(response);
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
            return View.State.END;
        }

        private Console() { }

    }
}
