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
	/// <para>A container used to describe VirtualPurchase Items and whether they are available
	/// to be purchased.</para>
	///
	/// <para>This is immutable after construction and is therefore thread safe.</para>
	/// </summary>
	public sealed class OneTimeVirtualPurchaseAvailabilityDefinition
	{
		/// <summary>
		/// The key of the VirtualPurchase item.
		/// </summary>
        public string Key { get; private set; }
	
		/// <summary>
		/// The VirtualPurchase item.
		/// </summary>
        public VirtualPurchaseDefinition Item { get; private set; }
	
		/// <summary>
		/// Whether the attached VirtualPurchase item is available to be purchased.
		/// </summary>
        public bool Available { get; private set; }

		/// <summary>
		/// Initialises a new instance with the given properties.
		/// </summary>
		///
		/// <param name="key">The key of the VirtualPurchase item.</param>
		/// <param name="item">The VirtualPurchase item.</param>
		/// <param name="available">Whether the attached VirtualPurchase item is available to be purchased.</param>
		public OneTimeVirtualPurchaseAvailabilityDefinition(string key, VirtualPurchaseDefinition item, bool available)
		{
			ReleaseAssert.IsNotNull(key, "Key cannot be null.");
			ReleaseAssert.IsNotNull(item, "Item cannot be null.");
	
            Key = key;
            Item = item;
            Available = available;
		}
		
		/// <summary>
		/// Initialises a new instance from the given Json dictionary.
		/// </summary>
		///
		/// <param name="jsonDictionary">The dictionary containing the Json data.</param>
		public OneTimeVirtualPurchaseAvailabilityDefinition(IDictionary<string, object> jsonDictionary)
		{
			ReleaseAssert.IsNotNull(jsonDictionary, "JSON dictionary cannot be null.");
			ReleaseAssert.IsTrue(jsonDictionary.ContainsKey("Key"), "Json is missing required field 'Key'");
			ReleaseAssert.IsTrue(jsonDictionary.ContainsKey("Item"), "Json is missing required field 'Item'");
			ReleaseAssert.IsTrue(jsonDictionary.ContainsKey("Available"), "Json is missing required field 'Available'");
	
			foreach (KeyValuePair<string, object> entry in jsonDictionary)
			{
				// Key
				if (entry.Key == "Key")
				{
                    ReleaseAssert.IsTrue(entry.Value is string, "Invalid serialised type.");
                    Key = (string)entry.Value;
				}
		
				// Item
				else if (entry.Key == "Item")
				{
                    ReleaseAssert.IsTrue(entry.Value is IDictionary<string, object>, "Invalid serialised type.");
                    Item = new VirtualPurchaseDefinition((IDictionary<string, object>)entry.Value);	
				}
		
				// Available
				else if (entry.Key == "Available")
				{
                    ReleaseAssert.IsTrue(entry.Value is bool, "Invalid serialised type.");
                    Available = (bool)entry.Value;
				}
			}
		}

		/// <summary>
		/// Serialises all properties. The output will be a dictionary containing the
		/// objects properties in a form that can easily be converted to Json. 
		/// </summary>
		///
		/// <returns>The serialised object in dictionary form.</returns>
		public IDictionary<string, object> Serialise()
		{
            var dictionary = new Dictionary<string, object>();
			
			// Key
			dictionary.Add("Key", Key);
			
			// Item
            dictionary.Add("Item", Item.Serialise());
			
			// Available
			dictionary.Add("Available", Available);
			
			return dictionary;
		}
	}
}
