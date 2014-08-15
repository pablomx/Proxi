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
using Castle.DynamicProxy;

namespace Proxi
{
	internal class CastleBehavior : IBehavior
	{
		private ProxyGenerator factory = new ProxyGenerator();

		#region IBehavior Members

		public TProxy Create<TProxy>(IInterceptor interceptor, params Type[] interfaces)
		{
			return (TProxy) Create(typeof(TProxy), interceptor, interfaces);
		}

		public TProxy Create<TProxy>(TProxy target, IInterceptor interceptor, params Type[] interfaces)
		{
			var adapter = new CastleInterceptorAdapter(interceptor, target);
			return typeof(TProxy).IsInterface ?
                (TProxy)factory.CreateInterfaceProxyWithoutTarget(typeof(TProxy), interfaces, adapter) :
				(TProxy)factory.CreateClassProxy(typeof(TProxy), interfaces, adapter);
		}

        public object Create(Type proxyType, IInterceptor interceptor, params Type[] interfaces) 
        {
            var adapter = new CastleInterceptorAdapter(interceptor, null);
            return proxyType.IsInterface ?
                factory.CreateInterfaceProxyWithoutTarget(proxyType, interfaces, adapter) :
                factory.CreateClassProxy(proxyType, interfaces, adapter);
        }

        public TProxy Create<TProxy>(object[] ctorArguments, IInterceptor interceptor, params Type[] interfaces)
            where TProxy: class
        {
            //TODO: It should infer the ctor paramenters, no reason to pass in ctorArguments, proxy has no target            
            var target = (TProxy) Activator.CreateInstance(typeof(TProxy), ctorArguments);
            var adapter = new CastleInterceptorAdapter(interceptor, target);
            return (TProxy) factory.CreateClassProxy(typeof(TProxy), interfaces, Castle.DynamicProxy.ProxyGenerationOptions.Default, ctorArguments, adapter);
        }
		#endregion
    }
}
