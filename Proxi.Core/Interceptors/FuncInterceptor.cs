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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proxi
{
	internal class FuncInterceptor : Proxi.IInterceptor
	{
		Func<Proxi.IMethodInvocation, object> func;
        Action<Proxi.IMethodInvocation> action;

        public FuncInterceptor(Action<Proxi.IMethodInvocation> action)
        {
            this.action = action;
        }


		public FuncInterceptor(Func<Proxi.IMethodInvocation, object> func)
		{
			this.func = func;
		}

		#region IInterceptor Members

		object Proxi.IInterceptor.Run(Proxi.IMethodInvocation mi)
		{
            object result = null;
            if (func != null) result = func(mi); 
            else action(mi);			
            return  result;
		}

		#endregion
	}
}
