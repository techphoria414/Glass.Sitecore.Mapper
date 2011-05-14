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
using NUnit.Framework;
using Glass.Sitecore.Persistence.Configuration.Attributes;

namespace Glass.Sitecore.Persistence.Tests.Configuration.Attributes
{
    [TestFixture]
    public class AttributeConfigurationLoaderFixture
    {
        [Test]
        public void Constructor_CreateInstance_NoErrors()
        {
            //Assign
            AttributeConfigurationLoader loader = null;

            //Act 
            loader = new AttributeConfigurationLoader(new string[] { });

            //Assert
            Assert.IsNotNull(loader);
        }

        #region Test Set 1
        [Test]
        public void Load_SingleClass_LoadsClassNoMembers()
        {
            //Assign 
            Type testType = typeof(AttributeConfigurationLoaderFixtureNS.TestSet1.TestSet1);
            var namespaces = new string []{string.Format("{0},{1}", testType.Namespace,testType.Assembly)};
            AttributeConfigurationLoader loader = new AttributeConfigurationLoader(namespaces);

            //Act
            var result = loader.Load();

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(testType, result.First().Type);

        }
        #endregion

        #region Test Set 2
        [Test]
        public void Load_TwoClassesInNamespace_OnlyOneShouldLoad()
        {
            //Assign 
            Type testType = typeof(AttributeConfigurationLoaderFixtureNS.TestSet2.TestSet2);
            var namespaces = new string[] { string.Format("{0},{1}", testType.Namespace, testType.Assembly) };
            AttributeConfigurationLoader loader = new AttributeConfigurationLoader(namespaces);

            //Act
            var result = loader.Load();

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(testType, result.First().Type);

        }
        #endregion

        #region Test Set 3
        [Test]
        public void Load_TwoClassesInNamespace_BothShouldLoad()
        {
            //Assign 
            Type testType1 = typeof(AttributeConfigurationLoaderFixtureNS.TestSet3.TestSet3);
            Type testType2 = typeof(AttributeConfigurationLoaderFixtureNS.TestSet3.TestSet31);
            var namespaces = new string[] { string.Format("{0},{1}", testType1.Namespace, testType1.Assembly) };
            AttributeConfigurationLoader loader = new AttributeConfigurationLoader(namespaces);

            //Act
            var result = loader.Load();

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(testType1, result.First().Type);
            Assert.AreEqual(testType2, result.Last().Type);

        }
        #endregion

        #region Test Set 4
        [Test]
        public void Load_LoadClassWithProperties_SinglePropertyLoaded()
        {
            //Assign 
            Type testType = typeof(AttributeConfigurationLoaderFixtureNS.TestSet4.TestSet4);
            var namespaces = new string[] { string.Format("{0},{1}", testType.Namespace, testType.Assembly) };
            AttributeConfigurationLoader loader = new AttributeConfigurationLoader(namespaces);

            //Act
            var result = loader.Load();

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(testType, result.First().Type);
            Assert.AreEqual(1, result.First().Properties.Count());
            
        }

        #endregion

    }

    namespace AttributeConfigurationLoaderFixtureNS.TestSet1
    {
        [SitecoreClass]
        public class TestSet1
        {

        }
    }
    namespace AttributeConfigurationLoaderFixtureNS.TestSet2
    {
        [SitecoreClass]
        public class TestSet2
        {

        }
        public class TestSet2DontLoad
        {

        }
    }
    namespace AttributeConfigurationLoaderFixtureNS.TestSet3
    {
        [SitecoreClass]
        public class TestSet3
        {

        }
        [SitecoreClass]
        public class TestSet31
        {

        }
    }
    namespace AttributeConfigurationLoaderFixtureNS.TestSet4
    {
        [SitecoreClass]
        public class TestSet4
        {
            [SitecoreId]
            public Guid Id { get; set; }

        }
    }
}
