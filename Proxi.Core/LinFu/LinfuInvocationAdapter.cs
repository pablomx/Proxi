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
using LinFu.DynamicProxy;

namespace Proxi
{
	internal class LinFuInvocationAdapter : IMethodInvocation
	{
		private InvocationInfo invocation;
		private object target;


        public LinFuInvocationAdapter(InvocationInfo invocation, object target)
        {
            this.invocation = invocation;
            this.target = target;
        }

		#region IMethodInvocation Members

	    public object[] Arguments
		{
			get { return invocation.Arguments; }
		}

        public Type[] GenericArguments
        {
            get { return invocation.TypeArguments; }
        }

        MethodInfo method;
		public MethodInfo Method
		{
			get 
            {
                if (method == null) 
                {
                    method = invocation.TargetMethod.IsGenericMethodDefinition ? // is an open generic method?
                        invocation.TargetMethod.MakeGenericMethod(invocation.TypeArguments) :
                        invocation.TargetMethod;
                } 

                return method; 
            }
		}

		public object Proxy
		{
			get { return invocation.Target; }
		}

		public object Target
		{
			get { return target; } // return target ?? invocation.Target; 
		}

		#endregion
	}
}
