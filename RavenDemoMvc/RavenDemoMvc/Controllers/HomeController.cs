using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;
using Raven.Client.Document;

namespace RavenDemoMvc.Controllers
{
    public class HomeController : Controller
    {
        private IDocumentSession _session;

        public HomeController()
        {
            // TODO: point it at a demo DB
            _session =
                new Raven.Client.Document.DocumentStore
                {
                    Url = "http://localhost:8080/",
                    /*Conventions = new DocumentConvention()
                    {
                        FailoverBehavior = FailoverBehavior.AllowReadsFromSecondariesAndWritesToSecondaries
                    }*/
                }.OpenSession();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    // heavy handed, composition would be better using your favourite IoC container
    public class RavenController : Controller
    {
        public new IDocumentSession Session { get; set; }

        private static IDocumentStore documentStore;

        public static IDocumentStore DocumentStore
        {
            get
            {
                if (documentStore != null)
                    return documentStore;

                // yes a singleton
                lock (typeof (RavenController))
                {
                    if (documentStore != null)
                        return documentStore;

                    documentStore = new DocumentStore
                    {
                        ConnectionStringName = "RavenDB"
                    }.Initialize();
                }

                return documentStore;
            }
        }

        // Following Ayende here picking OnActionEx over a filter
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Session = DocumentStore.OpenSession();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            using (Session)
            {
                // session could be null if OnActionExecuting could have failed
                if (Session != null && filterContext.Exception == null)
                    Session.SaveChanges();
            }
        }
    }
}