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
	/// A container for information on the response from a GetCollectionObjectsRequest.
	/// </summary>
	public sealed class GetCollectionObjectsResponse
	{
		/// <summary>
		/// A list of objects. Note: If an Object does not exist for a given ObjectID it will
		/// not be returned in the response. The rest of the Objects will be returned as
		/// normal.
		/// </summary>
        public ReadOnlyCollection<CollectionDataObject> Objects { get; private set; }

		/// <summary>
		/// Initialises the response with the given json dictionary.
		/// </summary>
		///
		/// <param name="jsonDictionary">The dictionary containing the JSON data.</param>
		public GetCollectionObjectsResponse(IDictionary<string, object> jsonDictionary)
		{
			ReleaseAssert.IsNotNull(jsonDictionary, "JSON dictionary cannot be null.");
			ReleaseAssert.IsTrue(jsonDictionary.ContainsKey("Objects"), "Json is missing required field 'Objects'");
	
			foreach (KeyValuePair<string, object> entry in jsonDictionary)
			{
				// Objects
				if (entry.Key == "Objects")
				{
                    ReleaseAssert.IsTrue(entry.Value is IList<object>, "Invalid serialised type.");
                    Objects = JsonSerialisation.DeserialiseList((IList<object>)entry.Value, (object element) =>
                    {
                        ReleaseAssert.IsTrue(element is IDictionary<string, object>, "Invalid element type.");
                        return new CollectionDataObject((IDictionary<string, object>)element);	
                    });
				}
			}
		}
	}
}