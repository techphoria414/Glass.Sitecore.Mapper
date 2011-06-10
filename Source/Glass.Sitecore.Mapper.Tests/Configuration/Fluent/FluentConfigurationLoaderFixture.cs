/*
   Copyright 2011 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Tests.Configuration.Fluent.SitecoreClassFixtureNS;
using NUnit.Framework;
using Glass.Sitecore.Mapper.Configuration.Fluent;

namespace Glass.Sitecore.Mapper.Tests.Configuration.Fluent
{
    [TestFixture]
    public class FluentConfigurationLoaderFixture
    {
        [Test]
        public void MultipleClassTypesTest()
        {
            SitecoreClass<FluentConfigurationLoaderFixtureNS.Test1> cls1 = new SitecoreClass<FluentConfigurationLoaderFixtureNS.Test1>();
            SitecoreClass<FluentConfigurationLoaderFixtureNS.Test2> cls2 = new SitecoreClass<FluentConfigurationLoaderFixtureNS.Test2>();

            FluentConfigurationLoader loader = new FluentConfigurationLoader(
                new ISitecoreClass []{ cls1, cls2 });

        }


    }

    namespace FluentConfigurationLoaderFixtureNS
    {
        public class Test1 { }
        public class Test2 { }
    }
}
