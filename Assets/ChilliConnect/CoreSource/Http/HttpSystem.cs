//
//  Created by Ian Copland on 2015-11-10
//
//  The MIT License (MIT)
//
//  Copyright (c) 2015 Tag Games Limited
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

using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using SdkCore.MiniJSON;
using UnityEngine;
using UnityEngine.Networking;

namespace SdkCore
{
	/// <summary>
	/// <para>Provides a means to make both GET and POST requests to the given
	/// web-server. Requests can be both HTTP and HTTPS.</para>
	///
	/// <para>This is thread-safe.</para>
	/// </summary>
	public sealed class HttpSystem
	{
		private static readonly int[] ERROR_CODES = {500,503,504};

		private Logging m_logging;
		private TaskScheduler m_taskScheduler;

		/// <summary>
		/// Initializes a new instance of the HTTP system with the given task
		/// scheduler.
		/// </summary>
		///
		/// <param name="logging">Provides basic logging functionality.</param>
		/// <param name="taskScheduler">The task scheduler.</param>
		public HttpSystem(Logging logging, TaskScheduler taskScheduler)
		{
			ReleaseAssert.IsNotNull(logging, "Logging cannot be null.");
			ReleaseAssert.IsTrue(taskScheduler != null, "The task scheduler in a HTTP request system must not be null.");

			m_logging = logging;
			m_taskScheduler = taskScheduler;
		}

		/// <summary>
		/// Makes a HTTP GET request with the given request object. This is
		/// performed asynchronously, with the callback block run on a background
		/// thread.
		/// </summary>
		///
		/// <param name="request">The GET HTTP request.</param>
		/// <param name="callback">The callback which will provide the response from the server.
		/// The callback will be made on a background thread.</param>
		public void SendRequest(HttpGetRequest request, Action<HttpGetRequest, HttpResponse> callback)
		{
			ReleaseAssert.IsTrue(request != null, "The HTTP GET request must not be null when sending a request.");
			ReleaseAssert.IsTrue(callback != null, "The callback must not be null when sending a request.");

			SendRequest(request.Url, request.Headers, null, (HttpResponse response) =>
				{
					callback(request, response);
				});
		}

		/// <summary>
		/// Makes a HTTP POST request with the given request object. This is
		/// performed asynchronously, with the callback block run on a background
		/// thread.
		/// </summary>
		///
		/// <param name="request">The POST HTTP request.</param>
		/// <param name="callback">The callback which will provide the response from the server.
		/// The callback will be made on a background thread.</param>
		public void SendRequest(HttpPostRequest request, Action<HttpPostRequest, HttpResponse> callback)
		{
			ReleaseAssert.IsTrue(request != null, "The HTTP POST request must not be null when sending a request.");
			ReleaseAssert.IsTrue(callback != null, "The callback must not be null when sending a request.");

			var headers = new Dictionary<string, string>(request.Headers);

			if (request.ContentType != null)
			{
				headers.Add("Content-Type", request.ContentType);
			}

			SendRequest(request.Url, headers, request.Body, (HttpResponse response) =>
				{
					callback(request, response);
				});
		}

		/// <summary>
		/// Provides the means to send both GET and POST requests depending on the
		/// input data.
		/// </summary>
		///
		/// <param name="url">The URL that the request is targetting.</param>
		/// <param name="headers">The headers for the HTTP request.</param>
		/// <param name="body">The body of the request. If null, a GET request will be sent.</param>
		/// <param name="callback">The callback providing the response from the server.</param>
		private void SendRequest(String url, IDictionary<string, string> headers, byte[] body, Action<HttpResponse> callback)
		{
			ReleaseAssert.IsTrue(url != null, "The URL must not be null when sending a request.");
			ReleaseAssert.IsTrue(headers != null, "The headers must not be null when sending a request.");
			ReleaseAssert.IsTrue(callback != null, "The callback must not be null when sending a request.");

			m_taskScheduler.ScheduleMainThreadTask(() =>
				{
					m_taskScheduler.StartCoroutine(ProcessRequest(url, headers, body, callback));
				});
		}

		/// <summary>
		/// <para>The coroutine for processing the HTTP request. This will yield until the
		/// request has completed then get the data from the Web Request object.</para>
		/// </summary>
		///
		/// <returns>The coroutine enumerator.</returns>
		///
		/// <param name="url">The URL that the request is targetting.</param>
		/// <param name="headers">The headers for the HTTP request.</param>
		/// <param name="body">The body of the request. If null, a GET request will be sent.</param>
		/// <param name="callback">The callback providing the response from the server.</param>
		private IEnumerator ProcessRequest(String url, IDictionary<string, string> headers, byte[] body, Action<HttpResponse> callback)
		{
			ReleaseAssert.IsTrue(callback != null, "The callback must not be null when sending a request.");

			int responseCode = 500;
			float delayInSeconds = 0.0f;
			float delayIncrementInSeconds = 1.0f;
			float delayMultiplier = 2.0f;
			int retryAttempt = 1;

			for(int retries = 4; retries != 0; --retries)
			{
			    if(retries < 4){
			    	headers.Remove("X-Chilli-Retry");
			        headers.Add("X-Chilli-Retry", retryAttempt.ToString());
			        retryAttempt++;
			    }

				yield return new WaitForSecondsRealtime(delayInSeconds);

				UnityWebRequest webRequest = CreateUnityWebRequest(url, headers, body);
				ReleaseAssert.IsTrue(webRequest != null, "The webRequest must not be null when sending a request.");

				m_logging.LogVerboseMessage(string.Format("Sending request after delay {1}. Retries remaining {0}.", retries, delayInSeconds));

				yield return webRequest.Send();

				responseCode = (int)webRequest.responseCode;

				if(ERROR_CODES.Contains(responseCode) == false || retries == 1)
				{
					m_logging.LogVerboseMessage(string.Format("Request sent with response code {0}", responseCode));
					ProcessSentRequest(webRequest, responseCode, callback);
					break;
				}

                if(delayInSeconds == 0.0f){
				    delayInSeconds += delayIncrementInSeconds;
                } else {
                    delayInSeconds += delayMultiplier;
                }
			}
		}

		/// <summary>
		/// <para>Creates a new Web Request object. This is needed for handling 503
		/// response codes. When a 503 is returned the system will retry the web
		/// request a number of times however after a web request has been sent it
		/// cannot be reused hence the need to create a new object for every attempt.</para>
		/// </summary>
		///
		/// <returns>The web request object.</returns>
		///
		/// <param name="url">The URL that the request is targetting.</param>
		/// <param name="headers">The headers for the HTTP request.</param>
		/// <param name="body">The body of the request. If null, a GET request will be sent.</param>
		private UnityWebRequest CreateUnityWebRequest(String url, IDictionary<string, string> headers, byte[] body)
		{
			var webRequest = new UnityWebRequest(url);
			webRequest.method = UnityWebRequest.kHttpVerbPOST;

			// Set the headers
			foreach(var pair in headers)
			{
				webRequest.SetRequestHeader(pair.Key, pair.Value);
			}

			// Handlers
			webRequest.uploadHandler = new UploadHandlerRaw(body);
			webRequest.downloadHandler = new DownloadHandlerBuffer();

			return webRequest;
		}

		/// <summary>
		/// <para>Processes a web request that has been sent succuessfully, i.e. a
		/// request with a reponse code that is not 503 or has been retried the specified
		/// number of times.</para>
		/// </summary>
		///
		/// <param name="webRequest">The Web Request object that has been sent.</param>
		/// <param name="responseCode">The Web response code from the web request.</param>
		/// <param name="callback">The callback providing the response from the server.</param>
		private void ProcessSentRequest(UnityWebRequest webRequest, int responseCode, Action<HttpResponse> callback)
		{
			HttpResponseDesc desc = null;

			if(responseCode <= 0)
			{
				desc = new HttpResponseDesc(HttpResult.CouldNotConnect);
			}
			else
			{
				desc = new HttpResponseDesc(HttpResult.Success);
			}

			// Populate the request response
			if(webRequest.GetResponseHeaders() == null)
			{
				desc.Headers = new Dictionary<string, string>();
			}
			else
			{
				desc.Headers = new Dictionary<string, string>(webRequest.GetResponseHeaders());
			}

			desc.HttpResponseCode = responseCode;

			// Fill the response data
			if (webRequest.downloadedBytes > 0)
			{
				desc.Body = webRequest.downloadHandler.data;
			}

			HttpResponse response = new HttpResponse(desc);
			m_taskScheduler.ScheduleBackgroundTask(() =>
				{
					callback(response);
				});
		}
	}
}
