/* •——————————————————————————————————————————————————————————————————————————•
   | Copyright © Pablo Orozco (pablo@orozco.me).                              |
   | All rights reserved.                                                     |
   |                                                                          |
   | Licensed under the Apache License, Version 2.0 (the "License");          |
   | you may not use this file except in compliance with the License.         |
   | You may obtain a copy of the License at                                  |
   |                                                                          |
   | http://www.apache.org/licenses/LICENSE-2.0                               |
   |                                                                          |
   | Unless required by applicable law or agreed to in writing, software      |
   | distributed under the License is distributed on an "AS IS" BASIS,        |
   | WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. |
   | See the License for the specific language governing permissions and      |
   | limitations under the License.                                           |
   •——————————————————————————————————————————————————————————————————————————• */

using System;
using System.Linq;
using Xunit;
using Proxi;

namespace Proxi.Tests
{
	public class Foo : IFoo
	{
		public bool Executed { get; set; }

		#region IFoo Members
		public void GoTo(string to) { Executed = true; }
		public string Go() { Executed = true; return "ack"; }
		public void Fail() { Executed = true; throw new InvalidOperationException(); }
        //public string Go(out int a) { a = 1;  return "ack"; }
		#endregion
	}
}
