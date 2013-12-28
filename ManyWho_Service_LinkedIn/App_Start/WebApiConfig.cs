using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ManyWho.Service.LinkedIn
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "PluginLinkedInDescribe",
                constraints: new { httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(HttpMethod.Post) },
                routeTemplate: "plugins/api/linkedin/1/metadata",
                defaults: new
                {
                    controller = "PluginLinkedIn",
                    action = "Describe"
                }
            );

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInDescribeTables",
            //    routeTemplate: "plugins/api/linkedin/1/metadata/table",
            //    defaults: new
            //    {
            //        controller = "PluginLinkedIn",
            //        action = "DescribeTables"
            //    }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInDescribeFields",
            //    routeTemplate: "plugins/api/linkedin/1/metadata/field",
            //    defaults: new
            //    {
            //        controller = "PluginLinkedIn",
            //        action = "DescribeFields"
            //    }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInView",
            //    routeTemplate: "plugins/api/linkedin/1/view/{actionName}",
            //    defaults: new
            //    {
            //        controller = "PluginLinkedIn",
            //        action = "View",
            //        actionName = RouteParameter.Optional
            //    }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInDelete",
            //    routeTemplate: "plugins/api/linkedin/1/data/delete",
            //    defaults: new
            //    {
            //        controller = "PluginLinkedIn",
            //        action = "Delete"
            //    }
            //);

            config.Routes.MapHttpRoute(
                name: "PluginLinkedInGetUserInAuthorizationContext",
                routeTemplate: "plugins/api/linkedin/1/authorization",
                defaults: new
                {
                    controller = "PluginLinkedIn",
                    action = "GetUserInAuthorizationContext"
                }
            );

            config.Routes.MapHttpRoute(
                name: "PluginLinkedInLoadUserAttributes",
                routeTemplate: "plugins/api/linkedin/1/authorization/user/attribute",
                defaults: new
                {
                    controller = "PluginLinkedIn",
                    action = "LoadUserAttributes"
                }
            );

            config.Routes.MapHttpRoute(
                name: "PluginLinkedInLoadGroupAttributes",
                routeTemplate: "plugins/api/linkedin/1/authorization/group/attribute",
                defaults: new
                {
                    controller = "PluginLinkedIn",
                    action = "LoadGroupAttributes"
                }
            );

            config.Routes.MapHttpRoute(
                name: "PluginLinkedInLoadUsers",
                routeTemplate: "plugins/api/linkedin/1/authorization/user",
                defaults: new
                {
                    controller = "PluginLinkedIn",
                    action = "LoadUsers"
                }
            );

            config.Routes.MapHttpRoute(
                name: "PluginLinkedInLoadGroups",
                routeTemplate: "plugins/api/linkedin/1/authorization/group",
                defaults: new
                {
                    controller = "PluginLinkedIn",
                    action = "LoadGroups"
                }
            );

            config.Routes.MapHttpRoute(
                name: "PluginLinkedInLogin",
                routeTemplate: "plugins/api/linkedin/1/authentication",
                defaults: new
                {
                    controller = "PluginLinkedIn",
                    action = "Login"
                }
            );

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInLoad",
            //    constraints: new { httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(HttpMethod.Post) },
            //    routeTemplate: "plugins/api/linkedin/1/data",
            //    defaults: new
            //    {
            //        controller = "PluginLinkedIn",
            //        action = "Load"
            //    }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInSave",
            //    constraints: new { httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(HttpMethod.Put) },
            //    routeTemplate: "plugins/api/linkedin/1/data",
            //    defaults: new
            //    {
            //        controller = "PluginLinkedIn",
            //        action = "Save"
            //    }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInGetCurrentUserInfo",
            //    constraints: new { httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(HttpMethod.Post) },
            //    routeTemplate: "plugins/api/linkedin/1/social/stream/{streamId}/user/me",
            //    defaults: new
            //    {
            //        controller = "PluginLinkedIn",
            //        action = "GetCurrentUserInfo"
            //    }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInSearchUsersByName",
            //    routeTemplate: "plugins/api/linkedin/1/social/stream/{streamId}/user/name/{name}",
            //    defaults: new
            //    {
            //        controller = "PluginLinkedIn",
            //        action = "SearchUsersByName",
            //        streamId = RouteParameter.Optional,
            //        name = RouteParameter.Optional
            //    }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInGetUserInfo",
            //    constraints: new { httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(HttpMethod.Post) },
            //    routeTemplate: "plugins/api/linkedin/1/social/stream/{streamId}/user/{userId}",
            //    defaults: new
            //    {
            //        controller = "PluginLinkedIn",
            //        action = "GetUserInfo"
            //    }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInGetStreamFollowers",
            //    routeTemplate: "plugins/api/linkedin/1/social/stream/{streamId}/follower",
            //    defaults: new
            //    {
            //        controller = "PluginLinkedIn",
            //        action = "GetStreamFollowers",
            //        streamId = RouteParameter.Optional
            //    }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInShareMessage",
            //    routeTemplate: "plugins/api/linkedin/1/social/stream/{streamId}/share",
            //    defaults: new
            //    {
            //        controller = "PluginLinkedIn",
            //        action = "ShareMessage",
            //        streamId = RouteParameter.Optional
            //    }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInPostNewMessage",
            //    routeTemplate: "plugins/api/linkedin/1/social/stream/{streamId}/message",
            //    defaults: new
            //    {
            //        controller = "PluginLinkedIn",
            //        action = "PostNewMessage",
            //        streamId = RouteParameter.Optional
            //    }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInLikeMessage",
            //    routeTemplate: "plugins/api/linkedin/1/social/stream/{streamId}/message/{messageId}/like/{like}",
            //    defaults: new
            //    {
            //        controller = "PluginLinkedIn",
            //        action = "LikeMessage",
            //        streamId = RouteParameter.Optional,
            //        messageId = RouteParameter.Optional,
            //        like = RouteParameter.Optional
            //    }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInDeleteMessage",
            //    routeTemplate: "plugins/api/linkedin/1/social/stream/{streamId}/message/{messageId}",
            //    defaults: new
            //    {
            //        controller = "Social",
            //        action = "PluginLinkedIn",
            //        streamId = RouteParameter.Optional,
            //        messageId = RouteParameter.Optional
            //    }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInFollowStream",
            //    routeTemplate: "plugins/api/linkedin/1/social/stream/{streamId}/follow/{follow}",
            //    defaults: new
            //    {
            //        controller = "PluginLinkedIn",
            //        action = "FollowStream",
            //        streamId = RouteParameter.Optional,
            //        follow = RouteParameter.Optional
            //    }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInCreateStream",
            //    constraints: new { httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(HttpMethod.Post) },
            //    routeTemplate: "plugins/api/linkedin/1/social/stream",
            //    defaults: new
            //    {
            //        controller = "PluginLinkedIn",
            //        action = "CreateStream"
            //    }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInGetStreamMessages",
            //    routeTemplate: "plugins/api/linkedin/1/social/stream/{streamId}",
            //    defaults: new
            //    {
            //        controller = "PluginLinkedIn",
            //        action = "GetStreamMessages",
            //        streamId = RouteParameter.Optional
            //    }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "PluginLinkedInInvoke",
            //    routeTemplate: "plugins/api/linkedin/1/{actionName}",
            //    defaults: new
            //    {
            //        controller = "PluginLinkedIn",
            //        action = "Invoke",
            //        actionName = RouteParameter.Optional
            //    }
            //);

            // Make JSON the default format for the service
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }
    }
}
