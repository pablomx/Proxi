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
using LinFuProxy = global::LinFu.DynamicProxy;

namespace Proxi
{
	internal class LinFuBehavior : IBehavior
	{
		private LinFuProxy.ProxyFactory factory = new LinFuProxy.ProxyFactory();

		#region IBehavior Members

		public TProxy Create<TProxy>(IInterceptor interceptor, params Type[] interfaces)
		{
			return Create<TProxy>(default(TProxy), interceptor, interfaces);
		}

		public TProxy Create<TProxy>(TProxy target, IInterceptor interceptor, params Type[] interfaces)
		{
			var adapter = new LinFuInterceptorAdapter(interceptor, target);
			return factory.CreateProxy<TProxy>(adapter, interfaces);
		}

        public object Create(Type proxyType, IInterceptor interceptor, params Type[] interfaces)
        {
            var adapter = new LinFuInterceptorAdapter(interceptor, null);
            return factory.CreateProxy(proxyType, adapter, interfaces);
        }

		#endregion
	}
}
