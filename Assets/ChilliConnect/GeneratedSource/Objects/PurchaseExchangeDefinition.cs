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
	/// <para>A container used to describe currency and item definitions exchanged as either
	/// costs or rewards of a purchase.</para>
	///
	/// <para>This is immutable after construction and is therefore thread safe.</para>
	/// </summary>
	public sealed class PurchaseExchangeDefinition
	{
		/// <summary>
		/// The Currency items to be exchanged.
		/// </summary>
        public ReadOnlyCollection<PurchaseCurrencyExchangeDefinition> Currencies { get; private set; }
	
		/// <summary>
		/// The Inventory items to be exchanged.
		/// </summary>
        public ReadOnlyCollection<PurchaseInventoryExchangeDefinition> Items { get; private set; }

		/// <summary>
		/// Initialises a new instance with the given properties.
		/// </summary>
		///
		/// <param name="currencies">The Currency items to be exchanged.</param>
		/// <param name="items">The Inventory items to be exchanged.</param>
		public PurchaseExchangeDefinition(IList<PurchaseCurrencyExchangeDefinition> currencies, IList<PurchaseInventoryExchangeDefinition> items)
		{
			ReleaseAssert.IsNotNull(currencies, "Currencies cannot be null.");
			ReleaseAssert.IsNotNull(items, "Items cannot be null.");
	
            Currencies = Mutability.ToImmutable(currencies);
            Items = Mutability.ToImmutable(items);
		}
		
		/// <summary>
		/// Initialises a new instance from the given Json dictionary.
		/// </summary>
		///
		/// <param name="jsonDictionary">The dictionary containing the Json data.</param>
		public PurchaseExchangeDefinition(IDictionary<string, object> jsonDictionary)
		{
			ReleaseAssert.IsNotNull(jsonDictionary, "JSON dictionary cannot be null.");
			ReleaseAssert.IsTrue(jsonDictionary.ContainsKey("Currencies"), "Json is missing required field 'Currencies'");
			ReleaseAssert.IsTrue(jsonDictionary.ContainsKey("Items"), "Json is missing required field 'Items'");
	
			foreach (KeyValuePair<string, object> entry in jsonDictionary)
			{
				// Currencies
				if (entry.Key == "Currencies")
				{
                    ReleaseAssert.IsTrue(entry.Value is IList<object>, "Invalid serialised type.");
                    Currencies = JsonSerialisation.DeserialiseList((IList<object>)entry.Value, (object element) =>
                    {
                        ReleaseAssert.IsTrue(element is IDictionary<string, object>, "Invalid element type.");
                        return new PurchaseCurrencyExchangeDefinition((IDictionary<string, object>)element);	
                    });
				}
		
				// Items
				else if (entry.Key == "Items")
				{
                    ReleaseAssert.IsTrue(entry.Value is IList<object>, "Invalid serialised type.");
                    Items = JsonSerialisation.DeserialiseList((IList<object>)entry.Value, (object element) =>
                    {
                        ReleaseAssert.IsTrue(element is IDictionary<string, object>, "Invalid element type.");
                        return new PurchaseInventoryExchangeDefinition((IDictionary<string, object>)element);	
                    });
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
			
			// Currencies
            var serialisedCurrencies = JsonSerialisation.Serialise(Currencies, (PurchaseCurrencyExchangeDefinition element) =>
            {
                return element.Serialise();
            });
            dictionary.Add("Currencies", serialisedCurrencies);
			
			// Items
            var serialisedItems = JsonSerialisation.Serialise(Items, (PurchaseInventoryExchangeDefinition element) =>
            {
                return element.Serialise();
            });
            dictionary.Add("Items", serialisedItems);
			
			return dictionary;
		}
	}
}
