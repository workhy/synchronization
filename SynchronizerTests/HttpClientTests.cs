using Microsoft.VisualStudio.TestTools.UnitTesting;
using synchronization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synchronization.Tests
{
    [TestClass()]
    public class HttpClientTests
    {
        [TestMethod()]
        public void GetContentTest()
        {
            HttpClient clinet = new HttpClient();
            string msg = clinet.GetContent(); 
            Assert.Fail();
        }
    }
}