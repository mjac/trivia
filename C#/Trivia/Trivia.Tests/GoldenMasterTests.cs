using System;
using System.IO;
using NUnit.Framework;

namespace Trivia.Tests
{
    public class GoldenMasterTests
    {
        private static readonly string Seed = "";

        [Test]
        public void FirstGoldenMasterTest()
        {
            Assert.DoesNotThrow(() => GameRunner.Main(new[] { Seed }));
        }

        [Test]
        public void GoldenMasterHasNotChanged()
        {
            var gameOutput = new StringWriter();
            Console.SetOut(gameOutput);

            GameRunner.Main(new[] { Seed });

            var originalGoldenMaster = File.ReadAllText("goldenmasteremptyseed.txt");

            Assert.That(gameOutput.ToString(), Is.EqualTo(originalGoldenMaster));
        }

        [Test]
        public void GoldenMasterHasChangedIfSeedIsDifferent()
        {
            var gameOutput = new StringWriter();
            Console.SetOut(gameOutput);

            GameRunner.Main(new[] { "Hello" });

            var originalGoldenMaster = File.ReadAllText("goldenmasteremptyseed.txt");

            Assert.That(gameOutput.ToString(), Is.Not.EqualTo(originalGoldenMaster));
        }

        // Used for initially setting golden master
        [Test, Ignore]
        public void CaptureOutput()
        {
            OutToFile(@"D:\goldenmasteremptyseed.txt");
            GameRunner.Main(new[] { Seed });
        }

        public void OutToFile(string outFileName)
        {
            var fileOutput = new StreamWriter(
                new FileStream(outFileName, FileMode.Create)
                );
            fileOutput.AutoFlush = true;
            Console.SetOut(fileOutput);
        }
    }
}
