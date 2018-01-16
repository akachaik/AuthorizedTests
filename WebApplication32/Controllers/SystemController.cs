using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication32.Controllers
{
    public class SystemController : Controller
    {
        public IActionResult Index()
        {
            var assembly = Assembly.GetExecutingAssembly();
            //var types = assembly.GetTypes();
            //var controllers = types.Where(t => typeof(Controller).IsAssignableFrom(t)).ToList();
            var result = string.Empty;

            //foreach (var controller in controllers)
            //{
            //    result = "Conroller Name: " + controller.Name;
            //    foreach (var method in controller.GetMethods().Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute))))
            //    {
            //        result += "Prop Name: " + method.Name + Environment.NewLine;
            //    }
            //}

            var controlleractionlist = assembly.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                .Select(x => new
                {
                    Controller = x.DeclaringType.Name,
                    Action = x.Name,
                    ReturnType = x.ReturnType.Name,
                    Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))
                })
                .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

            foreach (var item in controlleractionlist)
            {
                result += $"Controller : {item.Controller} Action : {item.Action} Attribute: {item.Attributes} Return : {item.ReturnType}{Environment.NewLine}";
            }
            return Content(result);
        }
    }
}