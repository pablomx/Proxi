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
using CastleProxy = global::Castle.DynamicProxy;

namespace Proxi
{
    internal class CastleInterceptorAdapter : CastleProxy.IInterceptor
	{
		private IInterceptor interceptor;
		private object target;

		public CastleInterceptorAdapter(IInterceptor interceptor, object target)
		{
			this.interceptor = interceptor;
			this.target = target;
		}

		#region IInterceptor Members

        public void Intercept(CastleProxy.IInvocation invocation)
		{
			invocation.ReturnValue = interceptor.Run(new CastleInvocationAdapter(invocation, target));
		}

		#endregion
	}
}
