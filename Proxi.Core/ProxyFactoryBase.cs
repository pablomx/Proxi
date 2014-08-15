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
	public abstract class ProxyFactoryBase
	{
        protected ProxyFactoryBase(ProxyBehavior behavior)
        {
            this.behavior = BehaviorFactory.Create(behavior);
        }

        protected ProxyFactoryBase() 
        {
            behavior = BehaviorFactory.Create(ProxyBehavior.Default);
        }

        protected readonly IBehavior behavior;

		public virtual TProxy Create<TProxy>(IInterceptor interceptor, params Type[] interfaces)
		{
            Require.ArgumentNotNull(interceptor, "interceptor");
			return behavior.Create<TProxy>(interceptor, interfaces);
		}

		public virtual TProxy Create<TProxy>(TProxy target, IInterceptor interceptor, params Type[] interfaces)
		{
            Require.ArgumentNotNull(target, "target");
            Require.ArgumentNotNull(interceptor, "interceptor");
			return behavior.Create<TProxy>(target, interceptor, interfaces);
		}

        public virtual object Create(Type proxyType, IInterceptor interceptor, params Type[] interfaces)
        {
            Require.ArgumentNotNull(proxyType, "proxyType");
            Require.ArgumentNotNull(interceptor, "interceptor");
            return behavior.Create(proxyType, interceptor, interfaces);
        }

	}
}
