using System.Web.Mvc;

namespace Health.Areas.Prescription
{
    public class PrescriptionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Prescription";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Prescription_default",
                "Prescription/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Health.Areas.Prescription.Controllers" }
            );
        }
    }
}