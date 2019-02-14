using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Tests
{
    [TestClass]
    public class CsvLoaderTests
    {
        class Player
        {
            [CsvColumn(0)]
            public int No { get; set; }

            [CsvColumn(1)]
            public string Name { get; set; }
        }

        class PlayerHasOutBoundaryColumnPosition : Player
        {
            [CsvColumn(19)]
            public DateTime BirthDate { get; set; }
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Given_WrongFilePath_Should_ThrowFileNotFoundException()
        {
            CsvLoader.Load<Player>("\\not-exist-file-path\not-exist.csv");
        }

        [TestMethod]
        public void Given_WrongFilePath_ExceptionMessage_Should_ReflectFilePath()
        {
            string invalidFilePath = "\\not-exist-file-path\not-exist.csv";
            try
            {
                CsvLoader.Load<Player>(invalidFilePath);
            }
            catch (FileNotFoundException ex)
            {
                Assert.AreEqual(ex.Message, $"File not found: {invalidFilePath}");
            }
        }

        [TestMethod]
        public void Given_ValidCsvRow_Should_GetValidEntity()
        {
            string csvLine = "23,Michael Jordan";

            var player = CsvLoader.Map<Player>(csvLine);

            Assert.IsNotNull(player);
            Assert.AreEqual(player.No, 23);
            Assert.AreEqual(player.Name, "Michael Jordan");
        }

        [TestMethod]
        public void Given_InvalidTypeMapping_Should_BeAbleToIgnoreConvertError()
        {
            // wrong positions expected there would be converting error
            string csvLine = "Michael Jordan,23";

            // set ignoreConvertErro = true to avoid exceptions
            var player = CsvLoader.Map<Player>(csvLine, ignoreConvertError: true);

            Assert.IsNotNull(player);
            Assert.AreEqual(player.No, default(int));
            Assert.AreEqual(player.Name, "23");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Given_InvalidTypeMapping_Should_ThrowConvertError()
        {
            // wrong positions expected there would be converting error
            string csvLine = "Michael Jordan,23";

            // set ignoreConvertErro = false
            var player = CsvLoader.Map<Player>(csvLine, ignoreConvertError: false);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Given_IncorrectColumnIndex_Should_ThrowIndexOutOfRangeException()
        {
            string csvLine = "23,Michael Jordan";

            var player = CsvLoader.Map<PlayerHasOutBoundaryColumnPosition>(csvLine, ignoreConvertError: false, ignoreIndexOutOfRange: false);
        }

        [TestMethod]
        public void Given_IncorrectColumnIndex_Should_BeableToAvoidIndexOutOfRangeException()
        {
            string csvLine = "23,Michael Jordan";

            var player = CsvLoader.Map<PlayerHasOutBoundaryColumnPosition>(csvLine, ignoreConvertError: false, ignoreIndexOutOfRange: true);

            Assert.IsNotNull(player);
            Assert.AreEqual(player.No, 23);
            Assert.AreEqual(player.Name, "Michael Jordan");
        }
    }
}
