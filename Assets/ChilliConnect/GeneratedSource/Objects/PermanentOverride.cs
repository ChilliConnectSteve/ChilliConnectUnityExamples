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
	/// <para>A container used to describe a Permanent Override campaign.</para>
	///
	/// <para>This is immutable after construction and is therefore thread safe.</para>
	/// </summary>
	public sealed class PermanentOverride
	{
		/// <summary>
		/// The name of the Permanent Override.
		/// </summary>
        public string Name { get; private set; }
	
		/// <summary>
		/// The key of the Permanent Override.
		/// </summary>
        public string Key { get; private set; }
	
		/// <summary>
		/// True if the Permanent Override status is On. Note: The assignment holds for the
		/// duration of the Player session even if the Permanent Override is turned off
		/// during the session.
		/// </summary>
        public bool IsOn { get; private set; }
	
		/// <summary>
		/// The custom data of the Permanent Override.
		/// </summary>
		public MultiTypeValue CustomData { get; private set; }

		/// <summary>
		/// Initialises a new instance with the given description.
		/// </summary>
		///
		/// <param name="desc">The description.</param>
		public PermanentOverride(PermanentOverrideDesc desc)
		{
			ReleaseAssert.IsNotNull(desc, "A description object cannot be null.");
			
			ReleaseAssert.IsNotNull(desc.Name, "Name cannot be null.");
			ReleaseAssert.IsNotNull(desc.Key, "Key cannot be null.");
	
            Name = desc.Name;
            Key = desc.Key;
            IsOn = desc.IsOn;
            CustomData = desc.CustomData;
		}
		
		/// <summary>
		/// Initialises a new instance from the given Json dictionary.
		/// </summary>
		///
		/// <param name="jsonDictionary">The dictionary containing the Json data.</param>
		public PermanentOverride(IDictionary<string, object> jsonDictionary)
		{
			ReleaseAssert.IsNotNull(jsonDictionary, "JSON dictionary cannot be null.");
			ReleaseAssert.IsTrue(jsonDictionary.ContainsKey("Name"), "Json is missing required field 'Name'");
			ReleaseAssert.IsTrue(jsonDictionary.ContainsKey("Key"), "Json is missing required field 'Key'");
			ReleaseAssert.IsTrue(jsonDictionary.ContainsKey("IsOn"), "Json is missing required field 'IsOn'");
	
			foreach (KeyValuePair<string, object> entry in jsonDictionary)
			{
				// Name
				if (entry.Key == "Name")
				{
                    ReleaseAssert.IsTrue(entry.Value is string, "Invalid serialised type.");
                    Name = (string)entry.Value;
				}
		
				// Key
				else if (entry.Key == "Key")
				{
                    ReleaseAssert.IsTrue(entry.Value is string, "Invalid serialised type.");
                    Key = (string)entry.Value;
				}
		
				// Is On
				else if (entry.Key == "IsOn")
				{
                    ReleaseAssert.IsTrue(entry.Value is bool, "Invalid serialised type.");
                    IsOn = (bool)entry.Value;
				}
		
				// Custom Data
				else if (entry.Key == "CustomData")
				{
					if (entry.Value != null)
					{
                        ReleaseAssert.IsTrue(entry.Value is object, "Invalid serialised type.");
                        CustomData = new MultiTypeValue((object)entry.Value);	
                    }
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
			
			// Name
			dictionary.Add("Name", Name);
			
			// Key
			dictionary.Add("Key", Key);
			
			// Is On
			dictionary.Add("IsOn", IsOn);
			
			// Custom Data
			if (CustomData != null)
			{
                dictionary.Add("CustomData", CustomData.Serialise());
            }
			
			return dictionary;
		}
	}
}
