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
using SpringProxy = global::Spring.Aop.Framework;

namespace Proxi
{
	internal class SpringBehavior : IBehavior
	{
		private SpringProxy.ProxyFactory factory = new SpringProxy.ProxyFactory();
        

		#region IBehavior Members

		public TProxy Create<TProxy>(IInterceptor interceptor, params Type[] interfaces)
		{
            return (TProxy)Create(typeof(TProxy), null, interceptor, interfaces);
        }

		public TProxy Create<TProxy>(TProxy target, IInterceptor interceptor, params Type[] interfaces)
		{
            return (TProxy)Create(typeof(TProxy), target, interceptor, interfaces);
		}

        public object Create(Type proxyType, IInterceptor interceptor, params Type[] interfaces)
        {
            return Create(proxyType, null, interceptor, interfaces);
        }

		#endregion

        private object Create(Type proxyType, object target, IInterceptor interceptor, params Type[] interfaces)
        {
            if (!proxyType.IsInterface && target == null && proxyType.GetConstructor(Type.EmptyTypes) == null)
                throw new InvalidOperationException("Spring behavior does not support non-default constructors. Provide a target type with a default constructor or a target instance.");

            var adapter = new SpringInterceptorAdapter(interceptor);
            factory.ProxyTargetType = !proxyType.IsInterface;     
            factory.Interfaces = interfaces;
            factory.AddAdvice(adapter);
            if (proxyType.IsInterface) factory.AddInterface(proxyType);
            if(target != null) factory.Target = target;
            else if(!proxyType.IsInterface) factory.Target = Activator.CreateInstance(proxyType);
            return factory.GetProxy();
        }
    
    }
}
