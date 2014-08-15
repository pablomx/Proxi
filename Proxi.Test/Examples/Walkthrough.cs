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
using Xunit;

namespace Proxi.Tests.Examples
{
    public class Walkthrough
    {
        public void Example_of_how_to_create_an_interface_proxy_extending_InterceptorBase()
        {
            var target = new Foo();
            var interceptor = new InterceptorBaseImpl();
            var factory = new ProxyFactory(ProxyBehavior.Castle);
            var foo = factory.Create<IFoo>(target, interceptor);

            foo.Go("SLC");
            foo.Go("SLC","MEX");
            Assert.Throws<ArgumentNullException>(()=> foo.Go("SLC", null));
        }

        public void Example_of_how_to_create_an_interface_proxy_implementign_IInterceptor()
        {
            var target = new Foo();
            var interceptor = new IInterceptorImpl();
            var factory = new ProxyFactory(ProxyBehavior.Castle);
            var foo = factory.Create<IFoo>(target, interceptor);

            foo.Go("SLC");
            foo.Go("SLC", "MEX");
            Assert.Throws<ArgumentNullException>(() => foo.Go("SLC", null));
        }

    }

    #region Interceptors
    class IInterceptorImpl : IInterceptor
    {
        #region IInterceptor Members

        object IInterceptor.Run(IMethodInvocation mi)
        {
            if (mi.Target == null) throw new ArgumentNullException("Target");
            foreach (var arg in mi.Arguments)
                if (arg == null) throw new ArgumentNullException();

            return mi.Method.Invoke(mi.Target, mi.Arguments);
        }

        #endregion
    }

    class InterceptorBaseImpl : InterceptorBase
    {

        protected override void OnBefore(IMethodInvocation mi)
        {
            foreach (var arg in mi.Arguments)
                if (arg == null) throw new ArgumentNullException();
        }
    }
    #endregion

    #region Helpers
    public interface IFoo
    {
        void Go(string to);
        void Go(string from, string to);
    }

    public class Foo : IFoo 
    {
        public void Go(string to) 
        {
            Console.WriteLine("going to: " + to); 
        }

        public void Go(string from, string to)
        {
            Console.WriteLine("going from: " + from + " to: " + to);
        }
    }

    #endregion

}
