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
	/// A container for information on the response from a GetActiveCampaignsRequest.
	/// </summary>
	public sealed class GetActiveCampaignsResponse
	{
		/// <summary>
		/// The Test that the Player is assigned to for the current session.
		/// </summary>
		public AssignedTest Test { get; private set; }
	
		/// <summary>
		/// A list of Permanent Overrides that are assigned to the Player for the current
		/// session.
		/// </summary>
        public ReadOnlyCollection<PermanentOverride> PermanentOverrides { get; private set; }
	
		/// <summary>
		/// A list of Scheduled Events that are assigned to the Player for the current
		/// session.
		/// </summary>
        public ReadOnlyCollection<ScheduledEvent> ScheduledEvents { get; private set; }

		/// <summary>
		/// Initialises the response with the given json dictionary.
		/// </summary>
		///
		/// <param name="jsonDictionary">The dictionary containing the JSON data.</param>
		public GetActiveCampaignsResponse(IDictionary<string, object> jsonDictionary)
		{
			ReleaseAssert.IsNotNull(jsonDictionary, "JSON dictionary cannot be null.");
			ReleaseAssert.IsTrue(jsonDictionary.ContainsKey("PermanentOverrides"), "Json is missing required field 'PermanentOverrides'");
			ReleaseAssert.IsTrue(jsonDictionary.ContainsKey("ScheduledEvents"), "Json is missing required field 'ScheduledEvents'");
	
			foreach (KeyValuePair<string, object> entry in jsonDictionary)
			{
				// Test
				if (entry.Key == "Test")
				{
					if (entry.Value != null)
					{
                        ReleaseAssert.IsTrue(entry.Value is IDictionary<string, object>, "Invalid serialised type.");
                        Test = new AssignedTest((IDictionary<string, object>)entry.Value);	
                    }
				}
		
				// Permanent Overrides
				else if (entry.Key == "PermanentOverrides")
				{
                    ReleaseAssert.IsTrue(entry.Value is IList<object>, "Invalid serialised type.");
                    PermanentOverrides = JsonSerialisation.DeserialiseList((IList<object>)entry.Value, (object element) =>
                    {
                        ReleaseAssert.IsTrue(element is IDictionary<string, object>, "Invalid element type.");
                        return new PermanentOverride((IDictionary<string, object>)element);	
                    });
				}
		
				// Scheduled Events
				else if (entry.Key == "ScheduledEvents")
				{
                    ReleaseAssert.IsTrue(entry.Value is IList<object>, "Invalid serialised type.");
                    ScheduledEvents = JsonSerialisation.DeserialiseList((IList<object>)entry.Value, (object element) =>
                    {
                        ReleaseAssert.IsTrue(element is IDictionary<string, object>, "Invalid element type.");
                        return new ScheduledEvent((IDictionary<string, object>)element);	
                    });
				}
			}
		}
	}
}
