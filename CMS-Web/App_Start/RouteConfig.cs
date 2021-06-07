using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CMS_Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            //Account controller
            routes.MapRoute(
            name: "Account",
            url: "cccount",
            defaults: new
            {
                controller = "Account",
                action = "Index",
                namespaces = new[] { "CMS_Web.Controllers" }
            });

            routes.MapRoute(
            name: "AccountDealer",
            url: "vendor",
            defaults: new
            {
                controller = "Account",
                action = "Dealer",
                namespaces = new[] { "CMS_Web.Controllers" }
            });

            routes.MapRoute(
            name: "AccountProfileAccount",
            url: "account-detail",
            defaults: new
            {
                controller = "Account",
                action = "ProfileAccount",
                namespaces = new[] { "CMS_Web.Controllers" }
            });

            routes.MapRoute(
            name: "Location",
            url: "location",
            defaults: new
            {
                controller = "Location",
                action = "Index",
                namespaces = new[] { "CMS_Web.Controllers" }
            });

            //Styles controller
            routes.MapRoute(
            name: "Styles",
            url: "style",
            defaults: new
            {
                controller = "Styles",
                action = "Index",
                namespaces = new[] { "CMS_Web.Controllers" }
            });
            //end Styles controller

            //Categories controller
            routes.MapRoute(
            name: "Categories",
            url: "categories",
            defaults: new
            {
                controller = "Categories",
                action = "Index",
                namespaces = new[] { "CMS_Web.Controllers" }
            });

            //Contact controller
            routes.MapRoute(
            name: "Contact",
            url: "contact",
            defaults: new
            {
                controller = "Contact",
                action = "Index",
                namespaces = new[] { "CMS_Web.Controllers" }
            });
            //end contact controller

            //About controller
            routes.MapRoute(
            name: "About",
            url: "about",
            defaults: new
            {
                controller = "About",
                action = "Index",
                namespaces = new[] { "CMS_Web.Controllers" }
            });
            //end about controller

            //Notfound controller
            routes.MapRoute(
            name: "NotFound",
            url: "404-error",
            defaults: new
            {
                controller = "NotFound",
                action = "Index",
                namespaces = new[] { "CMS_Web.Controllers" }
            });
            //end Notfound controller

            //Login controller
            routes.MapRoute(
            name: "LoginSignIn",
            url: "login",
            defaults: new
            {
                controller = "Login",
                action = "SignIn",
                namespaces = new[] { "CMS_Web.Controllers" }
            });

            routes.MapRoute(
            name: "register",
            url: "dang-ky",
            defaults: new
            {
                controller = "Login",
                action = "SignUp",
                namespaces = new[] { "CMS_Web.Controllers" }
            });

            routes.MapRoute(
            name: "Cart",
            url: "cart",
            defaults: new
            {
                controller = "Cart",
                action = "Index",
                namespaces = new[] { "CMS_Web.Controllers" }
            });

            routes.MapRoute(
            name: "CartCheckOut",
            url: "checkout",
            defaults: new
            {
                controller = "Cart",
                action = "CheckOut",
                namespaces = new[] { "CMS_Web.Controllers" }
            });

            /* route product */
            routes.MapRoute(
            name: "Product",
            url: "product/{q}",
            defaults: new
            {
                controller = "Shop",
                action = "Index",
                q = UrlParameter.Optional,
                namespaces = new[] { "CMS_Web.Controllers" }
            });

            /* route product detail */
            routes.MapRoute(
            name: "Detail",
            url: "detail/{c}/{q}",
            defaults: new
            {
                controller = "Shop",
                action = "Detail",
                q = UrlParameter.Optional,
                c = UrlParameter.Optional,
                namespaces = new[] { "CMS_Web.Controllers" }
            });


            /* Route brands */
            routes.MapRoute(
            name: "Brand",
            url: "brand",
            defaults: new
            {
                controller = "Brand",
                action = "Index",
                namespaces = new[] { "CMS_Web.Controllers" }
            });
            /* Routes brands detail */
            routes.MapRoute(
            name: "BrandDetail",
            url: "brand/{q}",
            defaults: new
            {
                controller = "Brand",
                action = "Detail",
                q = UrlParameter.Optional,
                namespaces = new[] { "CMS_Web.Controllers" }
            });

            routes.MapRoute(
            name: "NewsHome",
            url: "new",
            defaults: new
            {
                controller = "News",
                action = "Index",
                namespaces = new[] { "CMS_Web.Controllers" }
            });

            routes.MapRoute(
            name: "NewsDetail",
            url: "new/{q}",
            defaults: new
            {
                controller = "News",
                action = "Detail",
                q = UrlParameter.Optional,
                namespaces = new[] { "CMS_Web.Controllers" },
            });


            routes.MapRoute(
                 "Default", // Route name
                 "{controller}/{action}/{id}", // URL with parameters
                 new { area = "", controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                 null,
                 new[] { "CMS_Web.Controllers" }
             ).DataTokens.Add("area", "");
        }
    }
}
