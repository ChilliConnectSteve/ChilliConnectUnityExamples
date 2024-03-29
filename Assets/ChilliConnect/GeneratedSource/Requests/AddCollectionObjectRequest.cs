//
//  This file was auto-generated using the ChilliConnect SDK Generator.
//
//  The MIT License (MIT)
//
//  Copyright (c) 2019 ChilliConnect Ltd
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
	/// <para>A container for all information that will be sent to the server during a
 	/// Add Collection Object api call.</para>
	///
	/// <para>This is immutable after construction and is therefore thread safe.</para>
	/// </summary>
	public sealed class AddCollectionObjectRequest : IImmediateServerRequest
	{
		/// <summary>
		/// The url the request will be sent to.
		/// </summary>
		public string Url { get; private set; }
		
		/// <summary>
		/// The HTTP request method that should be used.
		/// </summary>
		public HttpRequestMethod HttpRequestMethod { get; private set; }
		
		/// <summary>
		/// A valid session ConnectAccessToken obtained through one of the login endpoints.
		/// </summary>
        public string ConnectAccessToken { get; private set; }
	
		/// <summary>
		/// The Collection Key.
		/// </summary>
        public string Key { get; private set; }
	
		/// <summary>
		/// The data to be saved. When serialised the maximum size is 400kb.
		/// </summary>
        public MultiTypeValue Value { get; private set; }

		/// <summary>
		/// Initialises a new instance of the request with the given properties.
		/// </summary>
		///
		/// <param name="key">The Collection Key.</param>
		/// <param name="value">The data to be saved. When serialised the maximum size is 400kb.</param>
		/// <param name="connectAccessToken">A valid session ConnectAccessToken obtained through one of the login endpoints.</param>
		/// <param name="serverUrl">The server url for this call.</param>
		public AddCollectionObjectRequest(string key, MultiTypeValue value, string connectAccessToken, string serverUrl)
		{
			ReleaseAssert.IsNotNull(key, "Key cannot be null.");
			ReleaseAssert.IsNotNull(value, "Value cannot be null.");
	
			ReleaseAssert.IsNotNull(connectAccessToken, "Connect Access Token cannot be null.");
	
            Key = key;
            Value = value;
            ConnectAccessToken = connectAccessToken;
			
			Url = serverUrl + "/1.0/data/collection/object/add";
			HttpRequestMethod = HttpRequestMethod.Post;
		}

		/// <summary>
		/// Serialises all header properties. The output will be a dictionary containing 
		/// the extra header key-value pairs in addition the standard headers sent with 
		/// all server requests. Will return an empty dictionary if there are no headers.
		/// </summary>
		///
		/// <returns>The header key-value pairs.</returns>
		public IDictionary<string, string> SerialiseHeaders()
		{
			var dictionary = new Dictionary<string, string>();
			
			// Connect Access Token
			dictionary.Add("Connect-Access-Token", ConnectAccessToken.ToString());
		
			return dictionary;
		}
		
		/// <summary>
		/// Serialises all body properties. The output will be a dictionary containing the 
		/// body of the request in a form that can easily be converted to Json. Will return
		/// an empty dictionary if there is no body.
		/// </summary>
		///
		/// <returns>The body Json in dictionary form.</returns>
		public IDictionary<string, object> SerialiseBody()
		{
            var dictionary = new Dictionary<string, object>();
			
			// Key
			dictionary.Add("Key", Key);
			
			// Value
            dictionary.Add("Value", Value.Serialise());
	
			return dictionary;
		}
	}
}
