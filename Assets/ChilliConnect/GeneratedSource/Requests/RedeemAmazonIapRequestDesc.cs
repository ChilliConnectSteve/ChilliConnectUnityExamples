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
	/// </para>A mutable description of a RedeemAmazonIapRequest.</para>
	///
	/// </para>This is not thread-safe and should typically only be used to create new 
	/// instances of RedeemAmazonIapRequest.</para>
	/// </summary>
	public sealed class RedeemAmazonIapRequestDesc
	{
		/// <summary>
		/// The key of the real money purchase that defines the rewards to be applied to the
		/// players account on successful verification. The real money purchase should
		/// specify an amazon product id that matches the product id of the submitted
		/// Receipt.
		/// </summary>
        public string Key { get; set; }
	
		/// <summary>
		/// ReceiptID returned from the Amazon App Store as a result of a successful
		/// purchase. See the Amazon Documentation at
		/// 'https://developer.amazon.com/public/apis/earn/in-app-purchasing/docs-v2/implementing-iap-2.0'
		/// for more information to on how to access this value from your app.
		/// </summary>
        public string ReceiptId { get; set; }
	
		/// <summary>
		/// UserID returned from the Amazon App Store as a result of a successful purchase.
		/// See the Amazon Documentation at
		/// 'https://developer.amazon.com/public/apis/earn/in-app-purchasing/docs-v2/implementing-iap-2.0'
		/// for more information to on how to access this value from your app.
		/// </summary>
        public string UserId { get; set; }
	
		/// <summary>
		/// The amount of local currency paid by the player for the IAP e.g. 12.99
		/// </summary>
        public float? LocalCost { get; set; }
	
		/// <summary>
		/// The local currency with which the player purchased the IAP. This must be a valid
		/// ISO-4217 currency code.
		/// </summary>
        public string LocalCurrency { get; set; }

		/// <summary>
		/// Initialises a new instance of the description with the given required properties.
		/// </summary>
		///
		/// <param name="key">The key of the real money purchase that defines the rewards to be applied to the
		/// players account on successful verification. The real money purchase should
		/// specify an amazon product id that matches the product id of the submitted
		/// Receipt.</param>
		/// <param name="receiptId">ReceiptID returned from the Amazon App Store as a result of a successful
		/// purchase. See the Amazon Documentation at
		/// 'https://developer.amazon.com/public/apis/earn/in-app-purchasing/docs-v2/implementing-iap-2.0'
		/// for more information to on how to access this value from your app.</param>
		/// <param name="userId">UserID returned from the Amazon App Store as a result of a successful purchase.
		/// See the Amazon Documentation at
		/// 'https://developer.amazon.com/public/apis/earn/in-app-purchasing/docs-v2/implementing-iap-2.0'
		/// for more information to on how to access this value from your app.</param>
		public RedeemAmazonIapRequestDesc(string key, string receiptId, string userId)
		{
			ReleaseAssert.IsNotNull(key, "Key cannot be null.");
			ReleaseAssert.IsNotNull(receiptId, "Receipt Id cannot be null.");
			ReleaseAssert.IsNotNull(userId, "User Id cannot be null.");
	
            Key = key;
            ReceiptId = receiptId;
            UserId = userId;
		}
	}
}
