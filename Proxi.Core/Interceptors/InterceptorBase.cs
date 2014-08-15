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
    // TODO: Create a non-abstract class, extending this one, which provides events and auto-subscription model to avoid dependencies
    public abstract class InterceptorBase : IInterceptor
	{
        protected InterceptorBase() { }

		protected virtual void OnBefore(IMethodInvocation mi) { }
		protected virtual object OnInvoke(IMethodInvocation mi) { return mi.Method.Invoke(mi.Target, mi.Arguments); }
		protected virtual void OnCatch(IMethodInvocation mi, Exception ex) { throw ex; }
		protected virtual void OnFinally(IMethodInvocation mi) { }
		protected virtual void OnAfter(IMethodInvocation mi) { }
		protected virtual object OnReturn(IMethodInvocation mi, object result) { return result; }

		#region IInterceptor Members

		public object Run(IMethodInvocation mi)
		{
			object result = null;
			OnBefore(mi);
			try
			{
				result = OnInvoke(mi);
			}
			catch (Exception ex)
			{
				OnCatch(mi, ex);
			}
			finally
			{
				OnFinally(mi);
			}
			OnAfter(mi);
			return OnReturn(mi, result);
		}

		#endregion
	}
}
