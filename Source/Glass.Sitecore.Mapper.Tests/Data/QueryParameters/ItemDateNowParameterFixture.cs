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
using Glass.Sitecore.Mapper.Data.QueryParameters;
using System.Text.RegularExpressions;

namespace Glass.Sitecore.Mapper.Tests.Data.QueryParameters
{
    [TestFixture]
    public class ItemDateNowParameterFixture
    {
        #region GetValue

        [Test]
        public void GetValue_ReturnsDateNow()
        {
            //Assign
            ItemDateNowParameter param = new ItemDateNowParameter();

            //Act
            var result = param.GetValue(null);

            //Assert
            Assert.IsTrue(new Regex(@"\d{8}T\d{6}").IsMatch(result));

        }

        #endregion

    }
}
