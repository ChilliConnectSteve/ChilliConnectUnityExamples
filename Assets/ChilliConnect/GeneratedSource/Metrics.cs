//
//  This file was auto-generated using the ChilliConnect SDK Generator.
//
//  The MIT License (MIT)
//
//  Copyright (c) 2015 Tag Games Ltd
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using SdkCore;

namespace ChilliConnect 
{
	/// <summary>
	/// <para>The ChillConnect Metrics module. This provides the means to log metrics events
	/// with the server.</para>
	///
	/// <para>This is thread-safe.</para>
	/// </summary>
	public sealed class Metrics
	{
		private const int SuccessHttpResponseCode = 200;
		
		private Logging m_logging;
		private TaskScheduler m_taskScheduler;
		private ServerRequestSystem m_serverRequestSystem;
		private DataStore m_dataStore;
		
		/// <summary>
		/// Initialises a new instance of the module with the given logger, task scheduler
		/// and server request system.
		/// </summary>
		///
		/// <param name="logging">Provides basic logging functionality.</param>
		/// <param name="taskScheduler">The system which allows scheduling of tasks on different threads.</param>
		/// <param name="serverRequestSystem">The system which processes all server requests.</param>
		/// <param name="dataStore">The data store used for persisting data across the session.</param>
		public Metrics(Logging logging, TaskScheduler taskScheduler, ServerRequestSystem serverRequestSystem, DataStore dataStore)
		{
			ReleaseAssert.IsNotNull(logging, "Logging cannot be null.");
			ReleaseAssert.IsNotNull(taskScheduler, "Task Scheduler cannot be null.");
			ReleaseAssert.IsNotNull(serverRequestSystem, "Server Request System cannot be null.");
			ReleaseAssert.IsNotNull(dataStore, "Data Store cannot be null.");
		
			m_logging = logging;
			m_taskScheduler = taskScheduler;
			m_serverRequestSystem = serverRequestSystem;
			m_dataStore = dataStore;
		}
		
		/// <summary>
		/// Refreshes the current player session. Depending on ChilliConnect settings, the
		/// existing session could be refreshed, or a new session generated.
		/// </summary>
		///
		/// <param name="successCallback">The delegate which is called if the request was successful.</param>
		/// <param name="errorCallback">The delegate which is called if the request was unsuccessful. Provides 
		/// a container with information on what went wrong.</param>
		public void RefreshSession(Action successCallback, Action<RefreshSessionError> errorCallback)
		{
			m_logging.LogVerboseMessage("Sending Refresh Session request.");
			
			var serverUrl = m_dataStore.GetString("MetricsUrl");
            var metricsAccessToken = m_dataStore.GetString("MetricsAccessToken");
			var request = new RefreshSessionRequest(metricsAccessToken, serverUrl);
	
			m_serverRequestSystem.SendImmediateRequest(request, (IImmediateServerRequest sentRequest, ServerResponse serverResponse) =>
			{
				ReleaseAssert.IsTrue(request == sentRequest, "Received request is not the same as the one sent!");
				
				if (serverResponse.Result == HttpResult.Success && serverResponse.HttpResponseCode == SuccessHttpResponseCode)
				{
					NotifyRefreshSessionSuccess(serverResponse, successCallback);
				} 
				else 
				{
					NotifyRefreshSessionError(serverResponse, errorCallback);
				}
			});
		}
		
		/// <summary>
		/// Closes a metrics session. On successful close an empty response with a HTTP code
		/// of 200 is returned. No request body is expected.
		/// </summary>
		///
		/// <param name="successCallback">The delegate which is called if the request was successful.</param>
		/// <param name="errorCallback">The delegate which is called if the request was unsuccessful. Provides 
		/// a container with information on what went wrong.</param>
		public void EndSession(Action successCallback, Action<EndSessionError> errorCallback)
		{
			m_logging.LogVerboseMessage("Sending End Session request.");
			
			var serverUrl = m_dataStore.GetString("MetricsUrl");
            var metricsAccessToken = m_dataStore.GetString("MetricsAccessToken");
			var request = new EndSessionRequest(metricsAccessToken, serverUrl);
	
			m_serverRequestSystem.SendImmediateRequest(request, (IImmediateServerRequest sentRequest, ServerResponse serverResponse) =>
			{
				ReleaseAssert.IsTrue(request == sentRequest, "Received request is not the same as the one sent!");
				
				if (serverResponse.Result == HttpResult.Success && serverResponse.HttpResponseCode == SuccessHttpResponseCode)
				{
					NotifyEndSessionSuccess(serverResponse, successCallback);
				} 
				else 
				{
					NotifyEndSessionError(serverResponse, errorCallback);
				}
			});
		}
		
		/// <summary>
		/// Records a custom metrics event that occured within the context of a session. The
		/// behaviour of this method is identical to AddEvents method, with the exception
		/// that the request format accepts a single Event json object rather than an array.
		/// </summary>
		///
		/// <param name="desc">The request description.</param>
		/// <param name="successCallback">The delegate which is called if the request was successful.</param>
		/// <param name="errorCallback">The delegate which is called if the request was unsuccessful. Provides 
		/// a container with information on what went wrong.</param>
		public void AddEvent(AddEventRequestDesc desc, Action<AddEventRequest> successCallback, Action<AddEventRequest, AddEventError> errorCallback)
		{
			m_logging.LogVerboseMessage("Sending Add Event request.");
			
			var serverUrl = m_dataStore.GetString("MetricsUrl");
            var metricsAccessToken = m_dataStore.GetString("MetricsAccessToken");
			var request = new AddEventRequest(desc, metricsAccessToken, serverUrl);
	
			m_serverRequestSystem.SendImmediateRequest(request, (IImmediateServerRequest sentRequest, ServerResponse serverResponse) =>
			{
				ReleaseAssert.IsTrue(request == sentRequest, "Received request is not the same as the one sent!");
				
				if (serverResponse.Result == HttpResult.Success && serverResponse.HttpResponseCode == SuccessHttpResponseCode)
				{
					NotifyAddEventSuccess(serverResponse, request, successCallback);
				} 
				else 
				{
					NotifyAddEventError(serverResponse, request, errorCallback);
				}
			});
		}
		
		/// <summary>
		/// Records one or more custom metrics event that occurred within the context of a
		/// session. The posted body to this method should be a json encoded array of
		/// individual custom events. Events are validated against the custom event
		/// definitions created within the ChilliConnect dashboard. If any events are
		/// invalid, the request will not be processed and an InvalidRequest response
		/// returned. The data property of the response will contain a JSON structure that
		/// indicates the number of events successfully processed as well as the number
		/// failed in addition to specific error messages for each failed event as well as
		/// that events index within the original upload. If the provided events are valid,
		/// an empty json object will be returned.
		/// </summary>
		///
		/// <param name="events">An array of events.</param>
		/// <param name="successCallback">The delegate which is called if the request was successful.</param>
		/// <param name="errorCallback">The delegate which is called if the request was unsuccessful. Provides 
		/// a container with information on what went wrong.</param>
		public void AddEvents(IList<MetricsEvent> events, Action<AddEventsRequest> successCallback, Action<AddEventsRequest, AddEventsError> errorCallback)
		{
			m_logging.LogVerboseMessage("Sending Add Events request.");
			
			var serverUrl = m_dataStore.GetString("MetricsUrl");
            var metricsAccessToken = m_dataStore.GetString("MetricsAccessToken");
			var request = new AddEventsRequest(events, metricsAccessToken, serverUrl);
	
			m_serverRequestSystem.SendImmediateRequest(request, (IImmediateServerRequest sentRequest, ServerResponse serverResponse) =>
			{
				ReleaseAssert.IsTrue(request == sentRequest, "Received request is not the same as the one sent!");
				
				if (serverResponse.Result == HttpResult.Success && serverResponse.HttpResponseCode == SuccessHttpResponseCode)
				{
					NotifyAddEventsSuccess(serverResponse, request, successCallback);
				} 
				else 
				{
					NotifyAddEventsError(serverResponse, request, errorCallback);
				}
			});
		}
		
		/// <summary>
		/// Records a successfully completed IAP transaction.
		/// </summary>
		///
		/// <param name="item">A string identifying the item that the player purchased.</param>
		/// <param name="localCost">The amount of local currency paid by the player for the IAP.</param>
		/// <param name="localCurrency">The local currency with which the player purchased the IAP. This must be a valid
		/// ISO-4217 currency code.</param>
		/// <param name="successCallback">The delegate which is called if the request was successful.</param>
		/// <param name="errorCallback">The delegate which is called if the request was unsuccessful. Provides 
		/// a container with information on what went wrong.</param>
		public void AddIapEvent(string item, float localCost, string localCurrency, Action<AddIapEventRequest> successCallback, Action<AddIapEventRequest, AddIapEventError> errorCallback)
		{
			m_logging.LogVerboseMessage("Sending Add Iap Event request.");
			
			var serverUrl = m_dataStore.GetString("MetricsUrl");
            var metricsAccessToken = m_dataStore.GetString("MetricsAccessToken");
			var request = new AddIapEventRequest(item, localCost, localCurrency, metricsAccessToken, serverUrl);
	
			m_serverRequestSystem.SendImmediateRequest(request, (IImmediateServerRequest sentRequest, ServerResponse serverResponse) =>
			{
				ReleaseAssert.IsTrue(request == sentRequest, "Received request is not the same as the one sent!");
				
				if (serverResponse.Result == HttpResult.Success && serverResponse.HttpResponseCode == SuccessHttpResponseCode)
				{
					NotifyAddIapEventSuccess(serverResponse, request, successCallback);
				} 
				else 
				{
					NotifyAddIapEventError(serverResponse, request, errorCallback);
				}
			});
		}
		
		/// <summary>
		/// Notifies the user that a Refresh Session request was successful.
		/// </summary>
		///
		/// <param name="serverResponse">A container for information on the response from the server. Only 
		/// successful responses can be passed into this method.</param>
		/// <param name="callback">The success callback.</param>
		private void NotifyRefreshSessionSuccess(ServerResponse serverResponse, Action successCallback)
		{
			ReleaseAssert.IsTrue(serverResponse.Result == HttpResult.Success && serverResponse.HttpResponseCode == SuccessHttpResponseCode, "Input server request must describe a success.");
			
			m_logging.LogVerboseMessage("RefreshSession request succeeded.");
	
            var metricsAccessToken = serverResponse.Body["MetricsAccessToken"] as string;
            if (metricsAccessToken != null) {
            	m_dataStore.Set("MetricsAccessToken", metricsAccessToken);
        	}
        
			m_taskScheduler.ScheduleMainThreadTask(() =>
			{
				successCallback();
			});
		}
		
		/// <summary>
		/// Notifies the user that a End Session request was successful.
		/// </summary>
		///
		/// <param name="serverResponse">A container for information on the response from the server. Only 
		/// successful responses can be passed into this method.</param>
		/// <param name="callback">The success callback.</param>
		private void NotifyEndSessionSuccess(ServerResponse serverResponse, Action successCallback)
		{
			ReleaseAssert.IsTrue(serverResponse.Result == HttpResult.Success && serverResponse.HttpResponseCode == SuccessHttpResponseCode, "Input server request must describe a success.");
			
			m_logging.LogVerboseMessage("EndSession request succeeded.");
	
			m_taskScheduler.ScheduleMainThreadTask(() =>
			{
				successCallback();
			});
		}
		
		/// <summary>
		/// Notifies the user that a Add Event request was successful.
		/// </summary>
		///
		/// <param name="serverResponse">A container for information on the response from the server. Only 
		/// successful responses can be passed into this method.</param>
		/// <param name="request"> The request that was sent to the server.</param>
		/// <param name="callback">The success callback.</param>
		private void NotifyAddEventSuccess(ServerResponse serverResponse, AddEventRequest request, Action<AddEventRequest> successCallback)
		{
			ReleaseAssert.IsTrue(serverResponse.Result == HttpResult.Success && serverResponse.HttpResponseCode == SuccessHttpResponseCode, "Input server request must describe a success.");
			
			m_logging.LogVerboseMessage("AddEvent request succeeded.");
	
			m_taskScheduler.ScheduleMainThreadTask(() =>
			{
				successCallback(request);
			});
		}
		
		/// <summary>
		/// Notifies the user that a Add Events request was successful.
		/// </summary>
		///
		/// <param name="serverResponse">A container for information on the response from the server. Only 
		/// successful responses can be passed into this method.</param>
		/// <param name="request"> The request that was sent to the server.</param>
		/// <param name="callback">The success callback.</param>
		private void NotifyAddEventsSuccess(ServerResponse serverResponse, AddEventsRequest request, Action<AddEventsRequest> successCallback)
		{
			ReleaseAssert.IsTrue(serverResponse.Result == HttpResult.Success && serverResponse.HttpResponseCode == SuccessHttpResponseCode, "Input server request must describe a success.");
			
			m_logging.LogVerboseMessage("AddEvents request succeeded.");
	
			m_taskScheduler.ScheduleMainThreadTask(() =>
			{
				successCallback(request);
			});
		}
		
		/// <summary>
		/// Notifies the user that a Add Iap Event request was successful.
		/// </summary>
		///
		/// <param name="serverResponse">A container for information on the response from the server. Only 
		/// successful responses can be passed into this method.</param>
		/// <param name="request"> The request that was sent to the server.</param>
		/// <param name="callback">The success callback.</param>
		private void NotifyAddIapEventSuccess(ServerResponse serverResponse, AddIapEventRequest request, Action<AddIapEventRequest> successCallback)
		{
			ReleaseAssert.IsTrue(serverResponse.Result == HttpResult.Success && serverResponse.HttpResponseCode == SuccessHttpResponseCode, "Input server request must describe a success.");
			
			m_logging.LogVerboseMessage("AddIapEvent request succeeded.");
	
			m_taskScheduler.ScheduleMainThreadTask(() =>
			{
				successCallback(request);
			});
		}
		
		/// <summary>
		/// Notifies the user that a Refresh Session request has failed.
		/// </summary>
		///
		/// <param name="serverResponse">A container for information on the response from the server. Only 
		/// failed responses can be passed into this method.</param>
		/// <param name="callback">The error callback.</param>
		private void NotifyRefreshSessionError(ServerResponse serverResponse, Action<RefreshSessionError> errorCallback)
		{
			ReleaseAssert.IsTrue(serverResponse.Result != HttpResult.Success || serverResponse.HttpResponseCode != SuccessHttpResponseCode, "Input server request must describe an error.");
			
			switch (serverResponse.Result) 
			{
				case HttpResult.Success:
					m_logging.LogVerboseMessage("Refresh Session request failed with http response code: " + serverResponse.HttpResponseCode);
					break;
				case HttpResult.CouldNotConnect:
					m_logging.LogVerboseMessage("Refresh Session request failed becuase a connection could be established.");
					break;
				default:
					m_logging.LogVerboseMessage("Refresh Session request failed for an unknown reason.");
					throw new ArgumentException("Invalid value for server response result.");
			}
			
			RefreshSessionError error = new RefreshSessionError(serverResponse);	
			m_taskScheduler.ScheduleMainThreadTask(() =>
			{
				errorCallback(error);
			});	
		}
		
		/// <summary>
		/// Notifies the user that a End Session request has failed.
		/// </summary>
		///
		/// <param name="serverResponse">A container for information on the response from the server. Only 
		/// failed responses can be passed into this method.</param>
		/// <param name="callback">The error callback.</param>
		private void NotifyEndSessionError(ServerResponse serverResponse, Action<EndSessionError> errorCallback)
		{
			ReleaseAssert.IsTrue(serverResponse.Result != HttpResult.Success || serverResponse.HttpResponseCode != SuccessHttpResponseCode, "Input server request must describe an error.");
			
			switch (serverResponse.Result) 
			{
				case HttpResult.Success:
					m_logging.LogVerboseMessage("End Session request failed with http response code: " + serverResponse.HttpResponseCode);
					break;
				case HttpResult.CouldNotConnect:
					m_logging.LogVerboseMessage("End Session request failed becuase a connection could be established.");
					break;
				default:
					m_logging.LogVerboseMessage("End Session request failed for an unknown reason.");
					throw new ArgumentException("Invalid value for server response result.");
			}
			
			EndSessionError error = new EndSessionError(serverResponse);	
			m_taskScheduler.ScheduleMainThreadTask(() =>
			{
				errorCallback(error);
			});	
		}
		
		/// <summary>
		/// Notifies the user that a Add Event request has failed.
		/// </summary>
		///
		/// <param name="serverResponse">A container for information on the response from the server. Only 
		/// failed responses can be passed into this method.</param>
		/// <param name="request"> The request that was sent to the server.</param>
		/// <param name="callback">The error callback.</param>
		private void NotifyAddEventError(ServerResponse serverResponse, AddEventRequest request, Action<AddEventRequest, AddEventError> errorCallback)
		{
			ReleaseAssert.IsTrue(serverResponse.Result != HttpResult.Success || serverResponse.HttpResponseCode != SuccessHttpResponseCode, "Input server request must describe an error.");
			
			switch (serverResponse.Result) 
			{
				case HttpResult.Success:
					m_logging.LogVerboseMessage("Add Event request failed with http response code: " + serverResponse.HttpResponseCode);
					break;
				case HttpResult.CouldNotConnect:
					m_logging.LogVerboseMessage("Add Event request failed becuase a connection could be established.");
					break;
				default:
					m_logging.LogVerboseMessage("Add Event request failed for an unknown reason.");
					throw new ArgumentException("Invalid value for server response result.");
			}
			
			AddEventError error = new AddEventError(serverResponse);	
			m_taskScheduler.ScheduleMainThreadTask(() =>
			{
				errorCallback(request, error);
			});	
		}
		
		/// <summary>
		/// Notifies the user that a Add Events request has failed.
		/// </summary>
		///
		/// <param name="serverResponse">A container for information on the response from the server. Only 
		/// failed responses can be passed into this method.</param>
		/// <param name="request"> The request that was sent to the server.</param>
		/// <param name="callback">The error callback.</param>
		private void NotifyAddEventsError(ServerResponse serverResponse, AddEventsRequest request, Action<AddEventsRequest, AddEventsError> errorCallback)
		{
			ReleaseAssert.IsTrue(serverResponse.Result != HttpResult.Success || serverResponse.HttpResponseCode != SuccessHttpResponseCode, "Input server request must describe an error.");
			
			switch (serverResponse.Result) 
			{
				case HttpResult.Success:
					m_logging.LogVerboseMessage("Add Events request failed with http response code: " + serverResponse.HttpResponseCode);
					break;
				case HttpResult.CouldNotConnect:
					m_logging.LogVerboseMessage("Add Events request failed becuase a connection could be established.");
					break;
				default:
					m_logging.LogVerboseMessage("Add Events request failed for an unknown reason.");
					throw new ArgumentException("Invalid value for server response result.");
			}
			
			AddEventsError error = new AddEventsError(serverResponse);	
			m_taskScheduler.ScheduleMainThreadTask(() =>
			{
				errorCallback(request, error);
			});	
		}
		
		/// <summary>
		/// Notifies the user that a Add Iap Event request has failed.
		/// </summary>
		///
		/// <param name="serverResponse">A container for information on the response from the server. Only 
		/// failed responses can be passed into this method.</param>
		/// <param name="request"> The request that was sent to the server.</param>
		/// <param name="callback">The error callback.</param>
		private void NotifyAddIapEventError(ServerResponse serverResponse, AddIapEventRequest request, Action<AddIapEventRequest, AddIapEventError> errorCallback)
		{
			ReleaseAssert.IsTrue(serverResponse.Result != HttpResult.Success || serverResponse.HttpResponseCode != SuccessHttpResponseCode, "Input server request must describe an error.");
			
			switch (serverResponse.Result) 
			{
				case HttpResult.Success:
					m_logging.LogVerboseMessage("Add Iap Event request failed with http response code: " + serverResponse.HttpResponseCode);
					break;
				case HttpResult.CouldNotConnect:
					m_logging.LogVerboseMessage("Add Iap Event request failed becuase a connection could be established.");
					break;
				default:
					m_logging.LogVerboseMessage("Add Iap Event request failed for an unknown reason.");
					throw new ArgumentException("Invalid value for server response result.");
			}
			
			AddIapEventError error = new AddIapEventError(serverResponse);	
			m_taskScheduler.ScheduleMainThreadTask(() =>
			{
				errorCallback(request, error);
			});	
		}
	}
}
