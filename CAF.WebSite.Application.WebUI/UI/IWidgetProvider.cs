﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace CAF.WebSite.Application.WebUI.UI
{
	/// <summary>
	/// Allows request scoped registration of custom action routes, whose results get injected into widget zones.
	/// </summary>
	public interface IWidgetProvider
	{
		/// <summary>
		/// Registers an action route for a widget zone
		/// </summary>
		/// <param name="widgetZone">The name of zone to inject the action result to</param>
		/// <param name="actionName">Action name</param>
		/// <param name="controllerName">Controller name</param>
		/// <param name="routeValues">Route values</param>
		/// <param name="order">Sort order of action result within the specified widget zone</param>
		void RegisterAction(string widgetZone, string actionName, string controllerName, RouteValueDictionary routeValues, int order = 0);

		IEnumerable<WidgetRouteInfo> GetWidgets(string widgetZone);
	}

	public static class IWidgetProviderExtensions
	{
		public static void RegisterAction(this IWidgetProvider provider, string widgetZone, string actionName, string controllerName, object routeValues, int order = 0)
		{
			provider.RegisterAction(widgetZone, actionName, controllerName, new RouteValueDictionary(routeValues), order);
		}
	}
}
