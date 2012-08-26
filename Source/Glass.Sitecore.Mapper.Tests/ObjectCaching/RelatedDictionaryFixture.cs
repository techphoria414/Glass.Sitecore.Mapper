using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Sitecore.Mapper.ObjectCaching;

namespace Glass.Sitecore.Mapper.Tests.ObjectCaching
{
    [TestFixture]
    public class RelatedDictionaryFixture
    {
        RelatedDictionary _dictionary;
        [SetUp]
        public void Setup()
        {
            _dictionary = new RelatedDictionary();

        }

        //[Test]
        //[TestCase(new Guid("e4b5a23d-0d56-4912-8921-aa12bcd01bf7"), "master", typeof(CacheKey)))
        //public void RelatedDictionary_AddRelatedKey_HasBeenAdded(Guid rivisionID, string database, Type type)
        //{
        //    //Assign
        //    var relatedDictionary = new RelatedDictionary();
        //    var ownerGuid = new Guid("a6c9fe7c-08c8-40cb-95d7-d40bdea31b8b");

        //    var cacheKey1 = new CacheKey(rivisionID,database, type);



        //    //Act
        //    relatedDictionary.Add(ownerGuid, cacheKey1);

        //    var cacheKeys = relatedDictionary.FlushKeys(ownerGuid);

        //    //Assert
        //    Assert.IsNotEmpty(cacheKeys);
        //    Assert.AreEqual(cacheKeys.Count, 3);

        //    //assuming that the list is returning the items in the order that they were added.  Maybe each one of there should be a test case

        //    Assert.AreEqual(cacheKeys[0].RevisionId, new Guid("e4b5a23d-0d56-4912-8921-aa12bcd01bf7"));
        //    Assert.AreEqual(cacheKeys[0].Database, "master");
        //    Assert.AreEqual(cacheKeys[0].Type, typeof(CacheKey));

        //    Assert.AreEqual(cacheKeys[1].RevisionId, new Guid("1547e1dc-2f62-4ad3-b51d-e51c51d5b191"));
        //    Assert.AreEqual(cacheKeys[1].Database, "web");
        //    Assert.AreEqual(cacheKeys[1].Type, typeof(String));

        //    Assert.AreEqual(cacheKeys[2].RevisionId, new Guid("11a600cb-6bf0-48b6-a2ec-ecce13fd2368"));
        //    Assert.AreEqual(cacheKeys[2].Database, "staging");
        //    Assert.AreEqual(cacheKeys[2].Type, typeof(int));


        //}


        [Test]
        public void Add_CacheKeyAddedAndFlushedCorrectly(){

            //Assign
            var key = new CacheKey(Guid.NewGuid(), "mydb", this.GetType());
            Guid owner = Guid.NewGuid();

            //Act
            _dictionary.Add(owner, key);
            var list = _dictionary.FlushKeys(owner);               

            //Assert
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(key, list.First());

        }

        [Test]
        public void Add_CacheKeyAddedAndFlushedCorrectly_SecondGetReturnsAnEmptyList(){

            //Assign
            var key = new CacheKey(Guid.NewGuid(), "mydb", this.GetType());
            Guid owner = Guid.NewGuid();

            //Act
            _dictionary.Add(owner, key);
            var list = _dictionary.FlushKeys(owner);               
            var list2 = _dictionary.FlushKeys(owner);               

            //Assert
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(key, list.First());
            Assert.AreEqual(0, list2.Count);
            
        }

        [Test]
        public void Add_CacheKeyAddedAndFlushedCorrectlyWithTwoKeys_ReturnsTwoKeys(){

            //Assign
            var key1 = new CacheKey(Guid.NewGuid(), "mydb", this.GetType());
            Guid owner1 = Guid.NewGuid();

            var key2 = new CacheKey(Guid.NewGuid(), "mydb", this.GetType());

            //Act
            _dictionary.Add(owner1, key1);
            _dictionary.Add(owner1, key2);
            var list = _dictionary.FlushKeys(owner1);               

            //Assert
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(key1, list.First());
            Assert.AreEqual(key2, list.Skip(1).First());
            
        }

        [Test]
        public void Add_CacheKeyAddedAndFlushedCorrectlyWithTwoOwners_ReturnsTwoLists()
        {

            //Assign
            var key1 = new CacheKey(Guid.NewGuid(), "mydb", this.GetType());
            Guid owner1 = Guid.NewGuid();

            var key2 = new CacheKey(Guid.NewGuid(), "mydb", this.GetType());
            Guid owner2 = Guid.NewGuid();

            //Act
            _dictionary.Add(owner1, key1);
            _dictionary.Add(owner2, key2);
            var list = _dictionary.FlushKeys(owner1);
            var list2 = _dictionary.FlushKeys(owner2);

            //Assert
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(key1, list.First());
            Assert.AreEqual(1, list2.Count);
            Assert.AreEqual(key2, list2.First());

        }
    }
}
