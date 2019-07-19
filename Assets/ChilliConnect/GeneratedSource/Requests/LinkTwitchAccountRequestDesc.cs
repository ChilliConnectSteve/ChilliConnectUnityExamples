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
	/// </para>A mutable description of a LinkTwitchAccountRequest.</para>
	///
	/// </para>This is not thread-safe and should typically only be used to create new 
	/// instances of LinkTwitchAccountRequest.</para>
	/// </summary>
	public sealed class LinkTwitchAccountRequestDesc
	{
		/// <summary>
		/// 30 Character OAuth code that is used by ChilliConnect to verify and link a
		/// Players Account with a Twitch Account. Sent back from Twitch during the users
		/// Login Process via callback URL
		/// </summary>
        public string AuthCode { get; set; }
	
		/// <summary>
		/// When set to true, any existing Twitch association for the current use will be
		/// replaced. Otherwise, an error will be returned. Default false.
		/// </summary>
        public bool? Replace { get; set; }
	
		/// <summary>
		/// When set to true will remove any existing associations this Twitch account
		/// already has with another player of this game. Otherwise, an error will be
		/// returned. Default false.
		/// </summary>
        public bool? Update { get; set; }

		/// <summary>
		/// Initialises a new instance of the description with the given required properties.
		/// </summary>
		///
		/// <param name="authCode">30 Character OAuth code that is used by ChilliConnect to verify and link a
		/// Players Account with a Twitch Account. Sent back from Twitch during the users
		/// Login Process via callback URL</param>
		public LinkTwitchAccountRequestDesc(string authCode)
		{
			ReleaseAssert.IsNotNull(authCode, "Auth Code cannot be null.");
	
            AuthCode = authCode;
		}
	}
}
