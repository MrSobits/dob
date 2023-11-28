// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="">
//   
// </copyright>
// <summary>
//   The home controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.B4.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;

    /// <summary>
    /// The home controller.
    /// </summary>
    [HandleError]
    public class HomeController : BaseController
    {
        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// </returns>
        public ActionResult Index()
        {
            return this.View(this.ViewData);
        }
    }
}