namespace Ensek.WebUI.Common
{
  using JetBrains.Annotations;
  using Microsoft.AspNetCore.Mvc;

  public static class ControllerExtensions
  {
    public static RedirectToActionResult SafeRedirect(
      this ControllerBase controller, [AspMvcController] string controllerName)
      => controller.RedirectToAction("Index", controllerName);
  }

}