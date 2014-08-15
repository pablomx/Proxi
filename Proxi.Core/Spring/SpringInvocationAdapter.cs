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
using System.Reflection;
using SpringProxy = global::AopAlliance.Intercept;

namespace Proxi
{
	internal class SpringInvocationAdapter : IMethodInvocation
	{
		private SpringProxy.IMethodInvocation invocation;

        public SpringInvocationAdapter(SpringProxy.IMethodInvocation invocation)
        {
            this.invocation = invocation;
        }

		#region IMethodInvocation Members

		public object Proxy
		{
			get { return invocation.Proxy; }
		}

		public MethodInfo Method
		{
			get { return invocation.Method; }
		}

		public object[] Arguments
		{
			get { return invocation.Arguments; }
		}

        public Type[] GenericArguments
        {
            get { throw new NotImplementedException("Spring method definintion does not provide generc types. Utilize a different provider."); }
        }

		public object Target
		{
			get { return invocation.Target; }
		}

		#endregion
	}
}
