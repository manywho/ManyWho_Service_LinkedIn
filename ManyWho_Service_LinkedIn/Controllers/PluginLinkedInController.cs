using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using ManyWho.Flow.SDK.Controller;
using ManyWho.Flow.SDK.Utils;
using ManyWho.Flow.SDK.Draw.Flow;
using ManyWho.Flow.SDK.Draw.Content;
using ManyWho.Flow.SDK.Draw.Elements;
using ManyWho.Flow.SDK.Draw.Elements.UI;
using ManyWho.Flow.SDK.Draw.Elements.Map;
using ManyWho.Flow.SDK.Draw.Elements.Type;
using ManyWho.Flow.SDK.Draw.Elements.Value;
using ManyWho.Flow.SDK.Run;
using ManyWho.Flow.SDK.Run.State;
using ManyWho.Flow.SDK.Run.Elements.UI;
using ManyWho.Flow.SDK.Run.Elements.Map;
using ManyWho.Flow.SDK.Run.Elements.Type;
using ManyWho.Flow.SDK.Run.Elements.Config;
using ManyWho.Flow.SDK.Describe;
using ManyWho.Flow.SDK.Security;
using ManyWho.Flow.SDK.Social;
using ManyWho.Flow.SDK;
using ManyWho.Service.LinkedIn.Models;

namespace ManyWho.Service.LinkedIn.Controllers
{
    public class PluginLinkedInController : ApiController
    {
        public const Int32 MAXIMUM_RETRIES = 3;

        public const String SERVICE_ACTION_CANCEL = "cancel";

        public const String SERVICE_VALUE_CLIENT_ID = "API Key";
        public const String SERVICE_VALUE_CLIENT_SECRET = "Secret Key";
        public const String SERVICE_VALUE_ADMIN_EMAIL = "Admin Email";
        public const String SERVICE_VALUE_SCOPE = "Scope";

        public const String SERVICE_VALUE_DIRECTORY_ID = "LinkedIn";
        public const String SERVICE_VALUE_DIRECTORY_NAME = "LinkedIn";
        public const String SERVICE_VALUE_IDENTITY_PROVIDER = "LinkedIn";
        public const String SERVICE_VALUE_TENANT_NAME = "LinkedIn";

        public const String SERVICE_URL = "https://flow.manywho.com";

        [HttpGet]
        [ActionName("Check")]
        public String Check()
        {
            return "OK";
        }

        [HttpPost]
        [ActionName("Describe")]
        public DescribeServiceResponseAPI Describe(DescribeServiceRequestAPI describeServiceRequest)
        {
            //DescribeServiceInstallResponseAPI describeServiceInstallResponse = null;
            //DescribeServiceActionResponseAPI describeServiceAction = null;
            DescribeServiceResponseAPI describeServiceResponse = null;
            String clientId = null;
            String clientSecret = null;
            String scope = null;
            String adminEmail = null;

            if (describeServiceRequest == null)
            {
                throw ErrorUtils.GetWebException(HttpStatusCode.BadRequest, "DescribeServiceRequest object cannot be null.");
            }

            // We do not require configuration values in the describe call as this is a refresh type operation
            if (describeServiceRequest.configurationValues != null &&
                describeServiceRequest.configurationValues.Count > 0)
            {
                // If the configuration values are provided, then all of them are required
                clientId = SettingUtils.GetConfigurationValue(SERVICE_VALUE_CLIENT_ID, describeServiceRequest.configurationValues, true);
                clientSecret = SettingUtils.GetConfigurationValue(SERVICE_VALUE_CLIENT_SECRET, describeServiceRequest.configurationValues, true);
                scope = SettingUtils.GetConfigurationValue(SERVICE_VALUE_SCOPE, describeServiceRequest.configurationValues, true);
                adminEmail = SettingUtils.GetConfigurationValue(SERVICE_VALUE_ADMIN_EMAIL, describeServiceRequest.configurationValues, true);
            }

            // Start building the describe service response so the caller knows what they need to provide to use this service
            describeServiceResponse = new DescribeServiceResponseAPI();
            describeServiceResponse.culture = new CultureAPI();
            describeServiceResponse.culture.country = "US";
            describeServiceResponse.culture.language = "EN";
            describeServiceResponse.culture.variant = null;
            describeServiceResponse.providesDatabase = false;
            describeServiceResponse.providesLogic = false;
            describeServiceResponse.providesViews = false;
            describeServiceResponse.providesIdentity = true;
            describeServiceResponse.providesSocial = false;

            // Create the main configuration values
            describeServiceResponse.configurationValues = new List<DescribeValueAPI>();
            describeServiceResponse.configurationValues.Add(new DescribeValueAPI() { contentType = ManyWhoConstants.CONTENT_TYPE_STRING, developerName = SERVICE_VALUE_CLIENT_ID, contentValue = clientId, isRequired = true });
            describeServiceResponse.configurationValues.Add(new DescribeValueAPI() { contentType = ManyWhoConstants.CONTENT_TYPE_PASSWORD, developerName = SERVICE_VALUE_CLIENT_SECRET, contentValue = clientSecret, isRequired = true });
            describeServiceResponse.configurationValues.Add(new DescribeValueAPI() { contentType = ManyWhoConstants.CONTENT_TYPE_STRING, developerName = SERVICE_VALUE_SCOPE, contentValue = scope, isRequired = true });
            describeServiceResponse.configurationValues.Add(new DescribeValueAPI() { contentType = ManyWhoConstants.CONTENT_TYPE_STRING, developerName = SERVICE_VALUE_ADMIN_EMAIL, contentValue = adminEmail, isRequired = true });

            // If the user has provided these values as part of a re-submission, we can then go about configuring the rest of the service
            if (clientId != null &&
                clientId.Trim().Length > 0 &&
                clientSecret != null &&
                clientSecret.Trim().Length > 0)
            {
                //describeServiceResponse.actions = new List<DescribeServiceActionResponseAPI>();

                //// We have logic for cancelling subscriptions in one shot
                //describeServiceAction = new DescribeServiceActionResponseAPI();
                //describeServiceAction.uriPart = SERVICE_ACTION_CANCEL;
                //describeServiceAction.developerName = "Cancel Subscription";
                //describeServiceAction.developerSummary = "This action cancels a subscription.";
                //describeServiceAction.isViewMessageAction = false;

                //// Create the inputs for the subscription cancellation
                //describeServiceAction.serviceInputs = new List<DescribeValueAPI>();
                //describeServiceAction.serviceInputs.Add(new DescribeValueAPI() { contentType = ManyWhoConstants.CONTENT_TYPE_DATETIME, developerName = SERVICE_VALUE_CONTRACT_EFFECTIVE_DATE, isRequired = false });
                //describeServiceAction.serviceInputs.Add(new DescribeValueAPI() { contentType = ManyWhoConstants.CONTENT_TYPE_DATETIME, developerName = SERVICE_VALUE_CUSTOMER_ACCEPTANCE_DATE, isRequired = false });
                //describeServiceAction.serviceInputs.Add(new DescribeValueAPI() { contentType = ManyWhoConstants.CONTENT_TYPE_DATETIME, developerName = SERVICE_VALUE_EFFECTIVE_DATE, isRequired = false });
                //describeServiceAction.serviceInputs.Add(new DescribeValueAPI() { contentType = ManyWhoConstants.CONTENT_TYPE_DATETIME, developerName = SERVICE_VALUE_SUBSCRIPTION_ID, isRequired = true });

                //// Add the task action to the response
                //describeServiceResponse.actions.Add(describeServiceAction);

                //// We now create the associated things for this service that we'd like to install into the manywho account
                //describeServiceInstallResponse = new DescribeServiceInstallResponseAPI();
                //describeServiceInstallResponse.typeElements = ZuoraObjectManagerSingleton.GetInstance().GetTypeElements();

                //// Assign the installation object to our main describe response
                //describeServiceResponse.install = describeServiceInstallResponse;
            }

            return describeServiceResponse;
        }

        //[HttpPost]
        //[ActionName("DescribeTables")]
        //public List<TypeElementBindingAPI> DescribeTables(ObjectDataRequestAPI objectDataRequestAPI)
        //{
        //    try
        //    {
        //        //return SalesforceServiceSingleton.GetInstance().DescribeTables(objectDataRequestAPI);
        //        throw new NotImplementedException();
        //    }
        //    catch (Exception exception)
        //    {
        //        throw ErrorUtils.GetWebException(HttpStatusCode.BadRequest, exception);
        //    }
        //}

        //[HttpPost]
        //[ActionName("DescribeFields")]
        //public List<TypeElementPropertyBindingAPI> DescribeFields(ObjectDataRequestAPI objectDataRequestAPI)
        //{
        //    try
        //    {
        //        //return SalesforceServiceSingleton.GetInstance().DescribeFields(objectDataRequestAPI);
        //        throw new NotImplementedException();
        //    }
        //    catch (Exception exception)
        //    {
        //        throw ErrorUtils.GetWebException(HttpStatusCode.BadRequest, exception);
        //    }
        //}

        [HttpPost]
        [ActionName("Invoke")]
        public ServiceResponseAPI Invoke(String actionName, ServiceRequestAPI serviceRequest)
        {
            //ZuoraService zuoraService = null;
            ServiceResponseAPI serviceResponse = null;

            //try
            //{
            //    if (actionName.Equals(SERVICE_ACTION_CANCEL, StringComparison.InvariantCultureIgnoreCase) == true)
            //    {
            //        String username = null;
            //        String password = null;
            //        String adminEmail = null;
            //        String contractEffectiveDate = null;
            //        String customerAcceptanceDate = null;
            //        String effectiveDate = null;
            //        String subscriptionId = null;
            //        Amendment amendment = null;

            //        // Grab the values from the service request
            //        contractEffectiveDate = ValueUtils.GetContentValue(SERVICE_VALUE_CONTRACT_EFFECTIVE_DATE, serviceRequest.inputs, false);
            //        customerAcceptanceDate = ValueUtils.GetContentValue(SERVICE_VALUE_CUSTOMER_ACCEPTANCE_DATE, serviceRequest.inputs, false);
            //        effectiveDate = ValueUtils.GetContentValue(SERVICE_VALUE_EFFECTIVE_DATE, serviceRequest.inputs, false);
            //        subscriptionId = ValueUtils.GetContentValue(SERVICE_VALUE_SUBSCRIPTION_ID, serviceRequest.inputs, true);

            //        // Get the configuration information for the login
            //        username = ValueUtils.GetContentValue(SERVICE_VALUE_USERNAME, serviceRequest.configurationValues, true);
            //        password = ValueUtils.GetContentValue(SERVICE_VALUE_PASSWORD, serviceRequest.configurationValues, true);
            //        adminEmail = ValueUtils.GetContentValue(SERVICE_VALUE_ADMIN_EMAIL, serviceRequest.configurationValues, true);

            //        // Create a new amendment object to populate
            //        amendment = new Amendment();

            //        if (contractEffectiveDate != null &&
            //            contractEffectiveDate.Trim().Length > 0)
            //        {
            //            amendment.ContractEffectiveDate = DateTime.Parse(contractEffectiveDate);
            //        }
            //        else
            //        {
            //            amendment.ContractEffectiveDate = DateTime.Now;
            //        }

            //        if (customerAcceptanceDate != null &&
            //            customerAcceptanceDate.Trim().Length > 0)
            //        {
            //            amendment.CustomerAcceptanceDate = DateTime.Parse(customerAcceptanceDate);
            //        }
            //        else
            //        {
            //            amendment.CustomerAcceptanceDate = DateTime.Now;
            //        }

            //        if (effectiveDate != null &&
            //            effectiveDate.Trim().Length > 0)
            //        {
            //            amendment.EffectiveDate = DateTime.Parse(effectiveDate);
            //        }
            //        else
            //        {
            //            amendment.EffectiveDate = DateTime.Now;
            //        }

            //        amendment.Name = "Cancel Subscription";
            //        amendment.Description = "Cancelling the subscription via workflow";
            //        amendment.Status = "Completed";
            //        amendment.Type = "Cancellation";
            //        amendment.SubscriptionId = subscriptionId;
            //        amendment.ServiceActivationDate = DateTime.Now;

            //        // Get the zuora service using the provided credentials
            //        zuoraService = ZuoraObjectManagerSingleton.GetInstance().GetZuoraService(ZuoraObjectManagerSingleton.ZUORA_ENDPOINT, username, password);

            //        // Save the amendment back to zuora to cancel the subscription
            //        ZuoraObjectManagerSingleton.GetInstance().SaveObject(zuoraService, amendment);

            //        // Construct the service response
            //        serviceResponse = new ServiceResponseAPI();
            //        serviceResponse.invokeType = ManyWhoConstants.INVOKE_TYPE_FORWARD;
            //        serviceResponse.token = serviceRequest.token;
            //    }
            //    else
            //    {
            //        throw new NotImplementedException();
            //    }
            //}
            //catch (Exception exception)
            //{
            //    throw ErrorUtils.GetWebException(HttpStatusCode.BadRequest, exception);
            //}

            return serviceResponse;
        }

        [HttpPut]
        [ActionName("Save")]
        public ObjectDataResponseAPI Save(ObjectDataRequestAPI objectDataRequestAPI)
        {
            ObjectDataResponseAPI objectDataResponse = null;
            ////List<zObject> zobjects = null;
            ////ZuoraService zuoraService = null;
            ////SaveResult[] saveResult = null;
            //String username = null;
            //String password = null;
            //String adminEmail = null;

            //try
            //{
            //    // Get the configuration information for the login
            //    username = ValueUtils.GetContentValue(SERVICE_VALUE_USERNAME, objectDataRequestAPI.configurationValues, true);
            //    password = ValueUtils.GetContentValue(SERVICE_VALUE_PASSWORD, objectDataRequestAPI.configurationValues, true);
            //    adminEmail = ValueUtils.GetContentValue(SERVICE_VALUE_ADMIN_EMAIL, objectDataRequestAPI.configurationValues, true);

            //    // Save the objects back to zuora
            //    ZuoraObjectManagerSingleton.GetInstance().Save(null, adminEmail, objectDataRequestAPI);

            //    // Create our object data response
            //    objectDataResponse = new ObjectDataResponseAPI();

            //    // TODO: load the data back from zuora
            //    objectDataResponse.culture = objectDataRequestAPI.culture;
            //    objectDataResponse.objectData = objectDataRequestAPI.objectData;
            //    objectDataResponse.hasMoreResults = false;
            //}
            //catch (Exception exception)
            //{
            //    throw ErrorUtils.GetWebException(HttpStatusCode.BadRequest, exception);
            //}

            return objectDataResponse;
        }

        [HttpPost]
        [ActionName("Load")]
        public ObjectDataResponseAPI Load(ObjectDataRequestAPI objectDataRequestAPI)
        {
            //ZuoraService zuoraService = null;
            //QueryResult queryResult = null;
            ObjectDataResponseAPI objectDataResponse = null;
            //ObjectDataTypeAPI objectDataType = null;
            //String zoql = null;
            //String username = null;
            //String password = null;
            //String adminEmail = null;

            //try
            //{
            //    // Get the configuration information for the login
            //    username = ValueUtils.GetContentValue(SERVICE_VALUE_USERNAME, objectDataRequestAPI.configurationValues, true);
            //    password = ValueUtils.GetContentValue(SERVICE_VALUE_PASSWORD, objectDataRequestAPI.configurationValues, true);
            //    adminEmail = ValueUtils.GetContentValue(SERVICE_VALUE_ADMIN_EMAIL, objectDataRequestAPI.configurationValues, true);

            //    // Start the zoql
            //    zoql = "";

            //    // Get the object type information out
            //    objectDataType = objectDataRequestAPI.objectDataType;

            //    // Check to see if we have a filter
            //    if (objectDataRequestAPI.listFilter != null)
            //    {
            //        // Parameters not yet supported in the filtering of zuora
            //        //objectDataRequestAPI.listFilter.filterByProvidedObjects;
            //        //objectDataRequestAPI.listFilter.limit;
            //        //objectDataRequestAPI.listFilter.offset;
            //        //objectDataRequestAPI.listFilter.orderByDirectionType;
            //        //objectDataRequestAPI.listFilter.orderByPropertyDeveloperName;
            //        //objectDataRequestAPI.listFilter.search;

            //        if (objectDataRequestAPI.listFilter.id != null &&
            //            objectDataRequestAPI.listFilter.id.Trim().Length > 0)
            //        {
            //            zoql += "Id = '" + objectDataRequestAPI.listFilter.id + "' ";
            //        }

            //        if (objectDataRequestAPI.listFilter.where != null &&
            //            objectDataRequestAPI.listFilter.where.Count > 0)
            //        {
            //            foreach (ListFilterWhereAPI listFilterWhere in objectDataRequestAPI.listFilter.where)
            //            {
            //                // If the comparison type is null or blank, we assume an AND
            //                if (objectDataRequestAPI.listFilter.comparisonType == null ||
            //                    objectDataRequestAPI.listFilter.comparisonType.Trim().Length == 0)
            //                {
            //                    objectDataRequestAPI.listFilter.comparisonType = ManyWhoConstants.LIST_FILTER_CONFIG_COMPARISON_TYPE_AND;
            //                }

            //                zoql += objectDataRequestAPI.listFilter.comparisonType + " ";
            //                zoql += listFilterWhere.columnName + " ";

            //                // We assume here that we're not dealing with LIKE type queries just yet
            //                zoql += SqlUtils.ConvertCriteriaTypeToSql(listFilterWhere.criteriaType, listFilterWhere.value) + " ";
            //            }
            //        }

            //        // Check to see if the zoql starts with the comparison - we remove it if it does
            //        if (zoql.IndexOf(objectDataRequestAPI.listFilter.comparisonType, StringComparison.InvariantCultureIgnoreCase) == 0)
            //        {
            //            zoql = zoql.Substring(objectDataRequestAPI.listFilter.comparisonType.Length);
            //        }
            //    }

            //    // Pull all of the zoql together with the columns and the where
            //    if (zoql.Trim().Length > 0)
            //    {
            //        zoql = ZuoraObjectManagerSingleton.GetInstance().CreateObjectSelect(objectDataType.developerName) + " WHERE " + zoql;
            //    }
            //    else
            //    {
            //        // We don't have any filtering
            //        zoql = ZuoraObjectManagerSingleton.GetInstance().CreateObjectSelect(objectDataType.developerName);
            //    }

            //    // Get the zuora service using the provided credentials
            //    zuoraService = ZuoraObjectManagerSingleton.GetInstance().GetZuoraService(ZuoraObjectManagerSingleton.ZUORA_ENDPOINT, username, password);

            //    // Execute the query against zuora
            //    queryResult = ZuoraObjectManagerSingleton.GetInstance().ExecuteQuery(zuoraService, zoql);

            //    // Create our object data response
            //    objectDataResponse = new ObjectDataResponseAPI();

            //    // Check to see if we have any records
            //    if (queryResult != null &&
            //        queryResult.records != null &&
            //        queryResult.records.Length > 0)
            //    {
            //        // Create a new list to store our object data
            //        objectDataResponse.objectData = new List<ObjectAPI>();

            //        if (objectDataType.developerName.Equals(ZuoraObjectManagerSingleton.ZUORA_OBJECT_ACCOUNT, StringComparison.InvariantCultureIgnoreCase) == true)
            //        {
            //            Account account = null;

            //            // Go through each of the records and convert them to object data
            //            for (Int32 i = 0; i < queryResult.records.Length; i++)
            //            {
            //                // Cast the result over to an account object
            //                account = (Account)queryResult.records[i];

            //                // The record can be null for various scenarios
            //                if (account != null)
            //                {
            //                    // Convert the object and add it to the list of objects to send back
            //                    objectDataResponse.objectData.Add(ZuoraObjectManagerSingleton.GetInstance().ConvertZuoraAccountObjectToManyWhoObjectAPI(account));
            //                }
            //            }
            //        }
            //        else if (objectDataType.developerName.Equals(ZuoraObjectManagerSingleton.ZUORA_OBJECT_SUBSCRIPTION, StringComparison.InvariantCultureIgnoreCase) == true)
            //        {
            //            Subscription subscription = null;

            //            // Go through each of the records and convert them to object data
            //            for (Int32 i = 0; i < queryResult.records.Length; i++)
            //            {
            //                // Cast the result over to an account object
            //                subscription = (Subscription)queryResult.records[i];

            //                // The record can be null if it's cancelled
            //                if (subscription != null)
            //                {
            //                    // Convert the object and add it to the list of objects to send back
            //                    objectDataResponse.objectData.Add(ZuoraObjectManagerSingleton.GetInstance().ConvertZuoraSubscriptionObjectToManyWhoObjectAPI(subscription));
            //                }
            //            }
            //        }
            //        else if (objectDataType.developerName.Equals(ZuoraObjectManagerSingleton.ZUORA_OBJECT_AMENDMENT, StringComparison.InvariantCultureIgnoreCase) == true)
            //        {
            //            Amendment amendment = null;

            //            // Go through each of the records and convert them to object data
            //            for (Int32 i = 0; i < queryResult.records.Length; i++)
            //            {
            //                // Cast the result over to an account object
            //                amendment = (Amendment)queryResult.records[i];

            //                // The record can be null for various scenarios
            //                if (amendment != null)
            //                {
            //                    // Convert the object and add it to the list of objects to send back
            //                    objectDataResponse.objectData.Add(ZuoraObjectManagerSingleton.GetInstance().ConvertZuoraAmendmentObjectToManyWhoObjectAPI(amendment));
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception exception)
            //{
            //    throw ErrorUtils.GetWebException(HttpStatusCode.BadRequest, exception);
            //}

            return objectDataResponse;
        }

        [HttpPost]
        [ActionName("GetUserInAuthorizationContext")]
        public ObjectDataResponseAPI GetUserInAuthorizationContext(ObjectDataRequestAPI objectDataRequestAPI)
        {
            ObjectDataResponseAPI objectDataResponseAPI = null;
            IAuthenticatedWho authenticatedWho = null;
            ObjectAPI userObject = null;
            String clientId = null;
            String clientSecret = null;
            String scope = null;
            String adminEmail = null;
            String loginUrl = null;

            //authenticatedWho = this.GetWho();

            try
            {
                if (authenticatedWho == null)
                {
                    throw ErrorUtils.GetWebException(HttpStatusCode.BadRequest, "AuthenticatedWho object cannot be null.");
                }

                if (objectDataRequestAPI == null)
                {
                    throw ErrorUtils.GetWebException(HttpStatusCode.BadRequest, "ObjectDataRequest object cannot be null.");
                }

                if (objectDataRequestAPI.configurationValues == null ||
                    objectDataRequestAPI.configurationValues.Count == 0)
                {
                    throw ErrorUtils.GetWebException(HttpStatusCode.BadRequest, "ObjectDataRequest.ConfigurationValues cannot be null or empty.");
                }

                // Get the configuration values out that are needed for the context lookup
                clientId = SettingUtils.GetConfigurationValue(SERVICE_VALUE_CLIENT_ID, objectDataRequestAPI.configurationValues, true);
                clientSecret = SettingUtils.GetConfigurationValue(SERVICE_VALUE_CLIENT_SECRET, objectDataRequestAPI.configurationValues, true);
                scope = SettingUtils.GetConfigurationValue(SERVICE_VALUE_SCOPE, objectDataRequestAPI.configurationValues, true);
                adminEmail = SettingUtils.GetConfigurationValue(SERVICE_VALUE_ADMIN_EMAIL, objectDataRequestAPI.configurationValues, true);

                // Create a new response object to house our results
                objectDataResponseAPI = new ObjectDataResponseAPI();

                // TODO: Should really get the culture that the authenticated user is running under
                objectDataResponseAPI.culture = objectDataRequestAPI.culture;

                if (authenticatedWho.Token != null &&
                    authenticatedWho.Token.Trim().Length > 0)
                {
                    LinkedInBasicProfileResultAPI linkedInProfileResultAPI = null;

                    linkedInProfileResultAPI = this.GetUserProfile(authenticatedWho, adminEmail, authenticatedWho.Token);

                    // Create the user object to respond with
                    userObject = new ObjectAPI();
                    userObject.developerName = ManyWhoConstants.MANYWHO_USER_DEVELOPER_NAME;
                    userObject.properties = new List<PropertyAPI>();
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_COUNTRY, null));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_LANGUAGE, null));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_LOCATION, null));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_DIRECTORY_ID, SERVICE_VALUE_DIRECTORY_ID));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_DIRECTORY_NAME, SERVICE_VALUE_DIRECTORY_NAME));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_USER_ID, linkedInProfileResultAPI.id));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_USERNAME, linkedInProfileResultAPI.emailAddress));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_EMAIL, null));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_FIRST_NAME, linkedInProfileResultAPI.firstName));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_LAST_NAME, linkedInProfileResultAPI.lastName));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_STATUS, ManyWhoConstants.AUTHORIZATION_STATUS_AUTHORIZED));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_AUTHENTICATION_TYPE, ManyWhoConstants.AUTHENTICATION_TYPE_OAUTH2));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_LOGIN_URL, loginUrl));

                    // Apply the user object to the response
                    objectDataResponseAPI.objectData = new List<ObjectAPI>();
                    objectDataResponseAPI.objectData.Add(userObject);
                }
                else
                {
                    // Create the login URL for linkedin, so the engine knows where to send the user to authenticate
                    // We automatically add the "state" parameter so we can properly track the oauth2 authentication
                    loginUrl = String.Format("https://www.linkedin.com/uas/oauth2/authorization?response_type=code&client_id={0}&scope={1)", clientId, scope);

                    // Create the user object to respond with
                    userObject = new ObjectAPI();
                    userObject.developerName = ManyWhoConstants.MANYWHO_USER_DEVELOPER_NAME;
                    userObject.properties = new List<PropertyAPI>();
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_COUNTRY, null));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_LANGUAGE, null));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_LOCATION, null));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_DIRECTORY_ID, SERVICE_VALUE_DIRECTORY_ID));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_DIRECTORY_NAME, SERVICE_VALUE_DIRECTORY_NAME));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_USER_ID, ManyWhoConstants.AUTHENTICATED_USER_PUBLIC_USER_ID));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_USERNAME, ManyWhoConstants.AUTHENTICATED_USER_PUBLIC_USER_ID));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_EMAIL, null));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_FIRST_NAME, ManyWhoConstants.AUTHENTICATED_USER_PUBLIC_USER_ID));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_LAST_NAME, ManyWhoConstants.AUTHENTICATED_USER_PUBLIC_USER_ID));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_STATUS, ManyWhoConstants.AUTHORIZATION_STATUS_NOT_AUTHORIZED));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_AUTHENTICATION_TYPE, ManyWhoConstants.AUTHENTICATION_TYPE_OAUTH2));
                    userObject.properties.Add(CreateProperty(ManyWhoConstants.MANYWHO_USER_PROPERTY_LOGIN_URL, loginUrl));

                    // Apply the user object to the response
                    objectDataResponseAPI.objectData = new List<ObjectAPI>();
                    objectDataResponseAPI.objectData.Add(userObject);
                }

                return objectDataResponseAPI;

            }
            catch (Exception exception)
            {
                throw ErrorUtils.GetWebException(HttpStatusCode.BadRequest, exception);
            }
        }

        [HttpPost]
        [ActionName("Login")]
        public AuthenticatedWhoResultAPI Login(AuthenticationCredentialsAPI authenticationCredentialsAPI)
        {
            AuthenticatedWhoResultAPI authenticatedWhoResultAPI = null;
            LinkedInAuthenticationResultAPI linkedInAuthenticationResultAPI = null;
            LinkedInBasicProfileResultAPI linkedInProfileResultAPI = null;
            IAuthenticatedWho authenticatedWho = null;
            HttpResponseMessage httpResponseMessage = null;
            HttpClient httpClient = null;
            String clientId = null;
            String clientSecret = null;
            String adminEmail = null;
            String loginUrl = null;

            // Grab the authenticated who from the header
            //authenticatedWho = this.GetWho();

            try
            {
                if (authenticatedWho == null)
                {
                    throw ErrorUtils.GetWebException(HttpStatusCode.BadRequest, "AuthenticatedWho object cannot be null.");
                }

                if (authenticationCredentialsAPI == null)
                {
                    throw ErrorUtils.GetWebException(HttpStatusCode.BadRequest, "AuthenticationCredentialsAPI object cannot be null.");
                }

                if (authenticationCredentialsAPI.configurationValues == null ||
                    authenticationCredentialsAPI.configurationValues.Count == 0)
                {
                    throw ErrorUtils.GetWebException(HttpStatusCode.BadRequest, "AuthenticationCredentialsAPI.ConfigurationValues cannot be null or empty.");
                }

                // Get the configuration values out that are needed for the context lookup
                clientId = SettingUtils.GetConfigurationValue(SERVICE_VALUE_CLIENT_ID, authenticationCredentialsAPI.configurationValues, true);
                clientSecret = SettingUtils.GetConfigurationValue(SERVICE_VALUE_CLIENT_SECRET, authenticationCredentialsAPI.configurationValues, true);
                adminEmail = SettingUtils.GetConfigurationValue(SERVICE_VALUE_ADMIN_EMAIL, authenticationCredentialsAPI.configurationValues, true);

                // Create the Url for the final verification piece of the oauth request
                loginUrl = String.Format("https://www.linkedin.com/uas/oauth2/accessToken?grant_type=authorization_code&code={0}&redirect_uri={1}&client_id={2}&client_secret={3}", authenticationCredentialsAPI.code, authenticationCredentialsAPI.redirectUri, clientId, clientSecret);

                // We enclose the request in a for loop to handle http errors
                for (int i = 0; i < MAXIMUM_RETRIES; i++)
                {
                    try
                    {
                        // Create a new client object
                        httpClient = new HttpClient();

                        // Call the get method on the chatter API to grab the user information
                        httpResponseMessage = httpClient.PostAsync(loginUrl, null).Result;

                        // Check the status of the response and respond appropriately
                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            // Grab the response from LinkedIn
                            linkedInAuthenticationResultAPI = httpResponseMessage.Content.ReadAsAsync<LinkedInAuthenticationResultAPI>().Result;

                            // We successfully executed the request, we can break out of the retry loop
                            break;
                        }
                        else
                        {
                            // Make sure we handle the lack of success properly
                            HttpUtils.HandleUnsuccessfulHttpResponseMessage(authenticatedWho, i, adminEmail, "PluginLinkedInController.Login", httpResponseMessage, loginUrl);
                        }
                    }
                    catch (Exception exception)
                    {
                        // Make sure we handle the exception properly
                        HttpUtils.HandleHttpException(authenticatedWho, i, adminEmail, "PluginLinkedInController.Login", exception, loginUrl);
                    }
                    finally
                    {
                        // Clean up the objects from the request
                        HttpUtils.CleanUpHttp(httpClient, null, httpResponseMessage);
                    }
                }

                // Get the profile for this user
                linkedInProfileResultAPI = this.GetUserProfile(authenticatedWho, adminEmail, linkedInAuthenticationResultAPI.access_token);

                // Create the authenticated who result to send back to manywho
                authenticatedWhoResultAPI = new AuthenticatedWhoResultAPI();
                authenticatedWhoResultAPI.directoryId = SERVICE_VALUE_DIRECTORY_ID;
                authenticatedWhoResultAPI.directoryName = SERVICE_VALUE_DIRECTORY_NAME;
                authenticatedWhoResultAPI.identityProvider = SERVICE_VALUE_IDENTITY_PROVIDER;
                authenticatedWhoResultAPI.status = ManyWhoConstants.AUTHENTICATED_USER_STATUS_AUTHENTICATED;
                authenticatedWhoResultAPI.statusMessage = null;
                authenticatedWhoResultAPI.tenantName = SERVICE_VALUE_TENANT_NAME;
                authenticatedWhoResultAPI.token = linkedInAuthenticationResultAPI.access_token;
                authenticatedWhoResultAPI.userId = linkedInProfileResultAPI.id;
                authenticatedWhoResultAPI.username = linkedInProfileResultAPI.emailAddress;
            }
            catch (Exception exception)
            {
                throw ErrorUtils.GetWebException(HttpStatusCode.BadRequest, exception);
            }

            return authenticatedWhoResultAPI;
        }

        private LinkedInBasicProfileResultAPI GetUserProfile(IAuthenticatedWho authenticatedWho, String adminEmail, String accessToken)
        {
            LinkedInBasicProfileResultAPI linkedInProfileResultAPI = null;
            HttpResponseMessage httpResponseMessage = null;
            HttpClient httpClient = null;
            String requestUrl = null;

            // Now we want to grab the basic information about this user from their profile
            requestUrl = String.Format("http://api.linkedin.com/v1/people/~?oauth2_access_token={0}", accessToken);

            // We enclose the request in a for loop to handle http errors
            for (int i = 0; i < MAXIMUM_RETRIES; i++)
            {
                try
                {
                    // Create a new client object
                    httpClient = new HttpClient();

                    // Call the get method on the chatter API to grab the user information
                    httpResponseMessage = httpClient.GetAsync(requestUrl).Result;

                    // Check the status of the response and respond appropriately
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        // Grab the response from LinkedIn
                        linkedInProfileResultAPI = httpResponseMessage.Content.ReadAsAsync<LinkedInBasicProfileResultAPI>().Result;

                        // We successfully executed the request, we can break out of the retry loop
                        break;
                    }
                    else
                    {
                        // Make sure we handle the lack of success properly
                        HttpUtils.HandleUnsuccessfulHttpResponseMessage(authenticatedWho, i, adminEmail, "PluginLinkedInController.Login", httpResponseMessage, requestUrl);
                    }
                }
                catch (Exception exception)
                {
                    // Make sure we handle the exception properly
                    HttpUtils.HandleHttpException(authenticatedWho, i, adminEmail, "PluginLinkedInController.Login", exception, requestUrl);
                }
                finally
                {
                    // Clean up the objects from the request
                    HttpUtils.CleanUpHttp(httpClient, null, httpResponseMessage);
                }
            }

            return linkedInProfileResultAPI;
        }

        /// <summary>
        /// Utility method for creating new properties.
        /// </summary>
        private PropertyAPI CreateProperty(String developerName, String contentValue)
        {
            PropertyAPI propertyAPI = null;

            propertyAPI = new PropertyAPI();
            propertyAPI.developerName = developerName;
            propertyAPI.contentValue = contentValue;

            return propertyAPI;
        }
    }
}
