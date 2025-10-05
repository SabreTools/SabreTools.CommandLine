using System.Net.NetworkInformation;
using Xunit;

namespace SabreTools.CommandLine.Test
{
    public class FeatureTests
    {
        [Fact]
        public void ProcessArgs_EmptyArgs_Success()
        {
            Feature feature = new MockFeature("", "", "");

            string[] args = [];
            int index = 0;

            bool actual = feature.ProcessArgs(args, index);
            Assert.True(actual);
            Assert.Empty(feature.Inputs);
        }

        [Fact]
        public void ProcessArgs_NegativeIndex_Failure()
        {
            Feature feature = new MockFeature("", "", "");

            string[] args = ["a", "b", "c"];
            int index = -1;

            bool actual = feature.ProcessArgs(args, index);
            Assert.False(actual);
            Assert.Empty(feature.Inputs);
        }

        [Fact]
        public void ProcessArgs_OverIndex_Failure()
        {
            Feature feature = new MockFeature("", "", "");

            string[] args = ["a", "b", "c"];
            int index = 3;

            bool actual = feature.ProcessArgs(args, index);
            Assert.False(actual);
            Assert.Empty(feature.Inputs);
        }

        [Fact]
        public void ProcessArgs_ValidArgs_Success()
        {
            Feature feature = new MockFeature("a", "a", "a");
            feature.Add(new MockFeature("b", "b", "b"));
            feature.Add(new MockFeature("c", "c", "c"));

            string[] args = ["a", "b", "c"];
            int index = 0;

            bool actual = feature.ProcessArgs(args, index);
            Assert.True(actual);
            Assert.Empty(feature.Inputs);
        }

        [Fact]
        public void ProcessArgs_InvalidArg_AddedAsGeneric()
        {
            Feature feature = new MockFeature("a", "a", "a");
            feature.Add(new MockFeature("b", "b", "b"));
            feature.Add(new MockFeature("d", "d", "d"));

            string[] args = ["a", "b", "c"];
            int index = 0;

            bool actual = feature.ProcessArgs(args, index);
            Assert.True(actual);
            string input = Assert.Single(feature.Inputs);
            Assert.Equal("c", input);
        }

        [Fact]
        public void ProcessArgs_NestedArgs_Success()
        {
            Feature feature = new MockFeature("a", "a", "a");
            var sub = new MockFeature("b", "b", "b");
            sub.Add(new MockFeature("c", "c", "c"));
            feature.Add(sub);

            string[] args = ["a", "b", "c"];
            int index = 0;

            bool actual = feature.ProcessArgs(args, index);
            Assert.True(actual);
            Assert.Empty(feature.Inputs);
        }

        [Fact]
        public void FormatLongDescriptionTest()
        {
            var feature = new MockFeature("a", "a", "a", "Some long description that is normal");
            var o = feature.FormatLongDescription(pre: 0);
            Assert.Equal(2, o.Count);

            feature = new MockFeature("a", "a", "a", "Some long description\nwith a newline");
            o = feature.FormatLongDescription(pre: 0);
            Assert.Equal(3, o.Count);

            feature = new MockFeature("a", "a", "a", "Some long description\nwith\nmultiple\nnewlines");
            o = feature.FormatLongDescription(pre: 0);
            Assert.Equal(5, o.Count);

            feature = new MockFeature("a", "a", "a", "Some long description\n    - With formatting");
            o = feature.FormatLongDescription(pre: 0);
            Assert.Equal(3, o.Count);
        }

        /// <summary>
        /// Mock Feature implementation for testing
        /// </summary>
        private class MockFeature : Feature
        {
            public MockFeature(string name, string flag, string description, string? longDescription = null)
                : base(name, flag, description, longDescription)
            {
            }

            public MockFeature(string name, string[] flags, string description, string? longDescription = null)
                : base(name, flags, description, longDescription)
            {
            }

            /// <inheritdoc/>
            public override bool VerifyInputs() => true;

            /// <inheritdoc/>
            public override bool Execute() => true;
        }
    }
}
