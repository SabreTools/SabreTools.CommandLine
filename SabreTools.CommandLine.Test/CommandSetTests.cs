using System;
using System.Collections.Generic;
using SabreTools.CommandLine.Inputs;
using Xunit;

namespace SabreTools.CommandLine.Test
{
    public class CommandSetTests
    {
        [Fact]
        public void AddAndRetrieveTest()
        {
            var input1 = new FlagInput("input1", "--input1", "input1");
            var input2 = new FlagInput("input2", "--input2", "input2");

            var feature1 = new MockFeature("feature1", "feature1", "feature1");
            var inputA = new FlagInput("inputA", "--inputA", "inputA");
            var inputB = new FlagInput("inputB", "--inputB", "inputB");
            feature1.Add(inputA);
            feature1.Add(inputB);

            var featureSet = new CommandSet();
            featureSet.Add(input1);
            featureSet.Add(input2);
            featureSet.AddChildrenFrom(feature1);

            var actualInput1 = featureSet["input1"];
            Assert.NotNull(actualInput1);
            Assert.Equal("input1", actualInput1.Name);

            var actualInput2 = featureSet[input2];
            Assert.NotNull(actualInput2);
            Assert.Equal("input2", actualInput2.Name);

            var actualInput3 = featureSet["input3"];
            Assert.Null(actualInput3);

            var actualInputA = featureSet["inputA"];
            Assert.NotNull(actualInputA);
            Assert.Equal("inputA", actualInputA.Name);

            var actualinputB = featureSet["inputB"];
            Assert.NotNull(actualinputB);
            Assert.Equal("inputB", actualinputB.Name);
        }

        [Fact]
        public void GetInputNameTest()
        {
            var input1 = new FlagInput("input1", "--input1", "input1");
            var input2 = new FlagInput("input2", "--input2", "input2");

            var featureSet = new CommandSet();
            featureSet.Add(input1);
            featureSet.Add(input2);

            var actualName1 = featureSet.GetInputName("input1");
            Assert.NotEmpty(actualName1);
            Assert.Equal("input1", actualName1);

            var actualName2 = featureSet.GetInputName("--input2");
            Assert.NotEmpty(actualName2);
            Assert.Equal("input2", actualName2);

            var actualName3 = featureSet.GetInputName("input3");
            Assert.Empty(actualName3);
        }

        [Fact]
        public void GetTopLevelTest()
        {
            var input1 = new FlagInput("input1", "--input1", "input1");
            var input2 = new FlagInput("input2", "--input2", "input2");

            var featureSet = new CommandSet();
            featureSet.Add(input1);
            featureSet.Add(input2);

            var actualInput1 = featureSet.GetTopLevel("input1");
            Assert.NotNull(actualInput1);
            Assert.Equal("input1", actualInput1.Name);

            var actualInput2 = featureSet.GetTopLevel("--input2");
            Assert.NotNull(actualInput2);
            Assert.Equal("input2", actualInput2.Name);

            var actualInput3 = featureSet.GetTopLevel("input3");
            Assert.Null(actualInput3);
        }

        [Fact]
        public void IsTopLevelTest()
        {
            var input1 = new FlagInput("input1", "--input1", "input1");
            var input2 = new FlagInput("input2", "--input2", "input2");

            var featureSet = new CommandSet();
            featureSet.Add(input1);
            featureSet.Add(input2);

            bool actualTop1 = featureSet.IsTopLevel("input1");
            Assert.True(actualTop1);

            bool actualTop2 = featureSet.IsTopLevel("--input2");
            Assert.True(actualTop2);

            bool actualTop3 = featureSet.IsTopLevel("input3");
            Assert.False(actualTop3);
        }

        #region GetBoolean

        [Fact]
        public void GetBoolean_InvalidKey_DefaultValue()
        {
            var commandSet = new CommandSet();
            var child = new BooleanInput("b", "b", "b");
            commandSet.Add(child);

            bool actual = commandSet.GetBoolean("c");
            Assert.False(actual);
        }

        [Fact]
        public void GetBoolean_Exists_WrongType_Throws()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);

            Assert.Throws<ArgumentException>(() => _ = commandSet.GetBoolean("b"));
        }

        [Fact]
        public void GetBoolean_Exists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new BooleanInput("b", "b", "b");
            commandSet.Add(child);

            int index = 0;
            child.ProcessInput(["b", "true"], ref index);

            bool actual = commandSet.GetBoolean("b");
            Assert.True(actual);
        }

        [Fact]
        public void GetBoolean_NestedExists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);
            var subChild = new BooleanInput("c", "c", "c");
            child.Add(subChild);

            int index = 0;
            subChild.ProcessInput(["c", "true"], ref index);

            bool actual = commandSet.GetBoolean("c");
            Assert.True(actual);
        }

        #endregion

        #region GetFeature

        [Fact]
        public void GetFeature_InvalidKey_Null()
        {
            var commandSet = new CommandSet();
            var child = new MockFeature("b", "b", "b");
            commandSet.Add(child);

            Feature? actual = commandSet.GetFeature("c");
            Assert.Null(actual);
        }

        [Fact]
        public void GetFeature_Exists_WrongType_Throws()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);

            Assert.Throws<ArgumentException>(() => _ = commandSet.GetFeature("b"));
        }

        [Fact]
        public void GetFeature_Exists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new MockFeature("b", "b", "b");
            commandSet.Add(child);

            int index = 0;
            child.ProcessInput(["b"], ref index);

            Feature? actual = commandSet.GetFeature("b");
            Assert.NotNull(actual);
            Assert.Equal("b", actual.Name);
            Assert.True(actual.Value);
        }

        [Fact]
        public void GetFeature_NestedExists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);
            var subChild = new MockFeature("c", "c", "c");
            child.Add(subChild);

            int index = 0;
            subChild.ProcessInput(["c"], ref index);

            Feature? actual = commandSet.GetFeature("c");
            Assert.NotNull(actual);
            Assert.Equal("c", actual.Name);
            Assert.True(actual.Value);
        }

        #endregion

        #region GetInt8

        [Fact]
        public void GetInt8_InvalidKey_DefaultValue()
        {
            var commandSet = new CommandSet();
            var child = new Int8Input("b", "b", "b");
            commandSet.Add(child);

            sbyte actual = commandSet.GetInt8("c");
            Assert.Equal(sbyte.MinValue, actual);
        }

        [Fact]
        public void GetInt8_Exists_WrongType_Throws()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);

            Assert.Throws<ArgumentException>(() => _ = commandSet.GetInt8("b"));
        }

        [Fact]
        public void GetInt8_Exists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new Int8Input("b", "b", "b");
            commandSet.Add(child);

            int index = 0;
            child.ProcessInput(["b", "5"], ref index);

            sbyte actual = commandSet.GetInt8("b");
            Assert.Equal(5, actual);
        }

        [Fact]
        public void GetInt8_NestedExists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);
            var subChild = new Int8Input("c", "c", "c");
            child.Add(subChild);

            int index = 0;
            subChild.ProcessInput(["c", "5"], ref index);

            sbyte actual = commandSet.GetInt8("c");
            Assert.Equal(5, actual);
        }

        #endregion

        #region GetInt16

        [Fact]
        public void GetInt16_InvalidKey_DefaultValue()
        {
            var commandSet = new CommandSet();
            var child = new Int16Input("b", "b", "b");
            commandSet.Add(child);

            short actual = commandSet.GetInt16("c");
            Assert.Equal(short.MinValue, actual);
        }

        [Fact]
        public void GetInt16_Exists_WrongType_Throws()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);

            Assert.Throws<ArgumentException>(() => _ = commandSet.GetInt16("b"));
        }

        [Fact]
        public void GetInt16_Exists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new Int16Input("b", "b", "b");
            commandSet.Add(child);

            int index = 0;
            child.ProcessInput(["b", "5"], ref index);

            short actual = commandSet.GetInt16("b");
            Assert.Equal(5, actual);
        }

        [Fact]
        public void GetInt16_NestedExists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);
            var subChild = new Int16Input("c", "c", "c");
            child.Add(subChild);

            int index = 0;
            subChild.ProcessInput(["c", "5"], ref index);

            short actual = commandSet.GetInt16("c");
            Assert.Equal(5, actual);
        }

        #endregion

        #region GetInt32

        [Fact]
        public void GetInt32_InvalidKey_DefaultValue()
        {
            var commandSet = new CommandSet();
            var child = new Int32Input("b", "b", "b");
            commandSet.Add(child);

            int actual = commandSet.GetInt32("c");
            Assert.Equal(int.MinValue, actual);
        }

        [Fact]
        public void GetInt32_Exists_WrongType_Throws()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);

            Assert.Throws<ArgumentException>(() => _ = commandSet.GetInt32("b"));
        }

        [Fact]
        public void GetInt32_Exists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new Int32Input("b", "b", "b");
            commandSet.Add(child);

            int index = 0;
            child.ProcessInput(["b", "5"], ref index);

            int actual = commandSet.GetInt32("b");
            Assert.Equal(5, actual);
        }

        [Fact]
        public void GetInt32_NestedExists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);
            var subChild = new Int32Input("c", "c", "c");
            child.Add(subChild);

            int index = 0;
            subChild.ProcessInput(["c", "5"], ref index);

            int actual = commandSet.GetInt32("c");
            Assert.Equal(5, actual);
        }

        #endregion

        #region GetInt64

        [Fact]
        public void GetInt64_InvalidKey_DefaultValue()
        {
            var commandSet = new CommandSet();
            var child = new Int64Input("b", "b", "b");
            commandSet.Add(child);

            long actual = commandSet.GetInt64("c");
            Assert.Equal(long.MinValue, actual);
        }

        [Fact]
        public void GetInt64_Exists_WrongType_Throws()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);

            Assert.Throws<ArgumentException>(() => _ = commandSet.GetInt64("b"));
        }

        [Fact]
        public void GetInt64_Exists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new Int64Input("b", "b", "b");
            commandSet.Add(child);

            int index = 0;
            child.ProcessInput(["b", "5"], ref index);

            long actual = commandSet.GetInt64("b");
            Assert.Equal(5, actual);
        }

        [Fact]
        public void GetInt64_NestedExists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);
            var subChild = new Int64Input("c", "c", "c");
            child.Add(subChild);

            int index = 0;
            subChild.ProcessInput(["c", "5"], ref index);

            long actual = commandSet.GetInt64("c");
            Assert.Equal(5, actual);
        }

        #endregion

        #region GetString

        [Fact]
        public void GetString_InvalidKey_DefaultValue()
        {
            var commandSet = new CommandSet();
            var child = new StringInput("b", "b", "b");
            commandSet.Add(child);

            string? actual = commandSet.GetString("c");
            Assert.Null(actual);
        }

        [Fact]
        public void GetString_Exists_WrongType_Throws()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);

            Assert.Throws<ArgumentException>(() => _ = commandSet.GetString("b"));
        }

        [Fact]
        public void GetString_Exists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new StringInput("b", "b", "b");
            commandSet.Add(child);

            int index = 0;
            child.ProcessInput(["b", "value"], ref index);

            string? actual = commandSet.GetString("b");
            Assert.Equal("value", actual);
        }

        [Fact]
        public void GetString_NestedExists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);
            var subChild = new StringInput("c", "c", "c");
            child.Add(subChild);

            int index = 0;
            subChild.ProcessInput(["c", "value"], ref index);

            string? actual = commandSet.GetString("c");
            Assert.Equal("value", actual);
        }

        #endregion

        #region GetStringList

        [Fact]
        public void GetStringList_InvalidKey_DefaultValue()
        {
            var commandSet = new CommandSet();
            var child = new StringListInput("b", "b", "b");
            commandSet.Add(child);

            List<string> actual = commandSet.GetStringList("c");
            Assert.Empty(actual);
        }

        [Fact]
        public void GetStringList_Exists_WrongType_Throws()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);

            Assert.Throws<ArgumentException>(() => _ = commandSet.GetStringList("b"));
        }

        [Fact]
        public void GetStringList_Exists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new StringListInput("b", "b", "b");
            commandSet.Add(child);

            int index = 0;
            child.ProcessInput(["b", "value"], ref index);

            List<string> actual = commandSet.GetStringList("b");
            string value = Assert.Single(actual);
            Assert.Equal("value", value);
        }

        [Fact]
        public void GetStringList_NestedExists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);
            var subChild = new StringListInput("c", "c", "c");
            child.Add(subChild);

            int index = 0;
            subChild.ProcessInput(["c", "value"], ref index);

            List<string> actual = commandSet.GetStringList("c");
            string value = Assert.Single(actual);
            Assert.Equal("value", value);
        }

        #endregion

        #region GetUInt8

        [Fact]
        public void GetUInt8_InvalidKey_DefaultValue()
        {
            var commandSet = new CommandSet();
            var child = new UInt8Input("b", "b", "b");
            commandSet.Add(child);

            byte actual = commandSet.GetUInt8("c");
            Assert.Equal(byte.MinValue, actual);
        }

        [Fact]
        public void GetUInt8_Exists_WrongType_Throws()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);

            Assert.Throws<ArgumentException>(() => _ = commandSet.GetUInt8("b"));
        }

        [Fact]
        public void GetUInt8_Exists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new UInt8Input("b", "b", "b");
            commandSet.Add(child);

            int index = 0;
            child.ProcessInput(["b", "5"], ref index);

            byte actual = commandSet.GetUInt8("b");
            Assert.Equal(5, actual);
        }

        [Fact]
        public void GetUInt8_NestedExists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);
            var subChild = new UInt8Input("c", "c", "c");
            child.Add(subChild);

            int index = 0;
            subChild.ProcessInput(["c", "5"], ref index);

            byte actual = commandSet.GetUInt8("c");
            Assert.Equal(5, actual);
        }

        #endregion

        #region GetUInt16

        [Fact]
        public void GetUInt16_InvalidKey_DefaultValue()
        {
            var commandSet = new CommandSet();
            var child = new UInt16Input("b", "b", "b");
            commandSet.Add(child);

            ushort actual = commandSet.GetUInt16("c");
            Assert.Equal(ushort.MinValue, actual);
        }

        [Fact]
        public void GetUInt16_Exists_WrongType_Throws()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);

            Assert.Throws<ArgumentException>(() => _ = commandSet.GetUInt16("b"));
        }

        [Fact]
        public void GetUInt16_Exists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new UInt16Input("b", "b", "b");
            commandSet.Add(child);

            int index = 0;
            child.ProcessInput(["b", "5"], ref index);

            ushort actual = commandSet.GetUInt16("b");
            Assert.Equal(5, actual);
        }

        [Fact]
        public void GetUInt16_NestedExists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);
            var subChild = new UInt16Input("c", "c", "c");
            child.Add(subChild);

            int index = 0;
            subChild.ProcessInput(["c", "5"], ref index);

            ushort actual = commandSet.GetUInt16("c");
            Assert.Equal(5, actual);
        }

        #endregion

        #region GetUInt32

        [Fact]
        public void GetUInt32_InvalidKey_DefaultValue()
        {
            var commandSet = new CommandSet();
            var child = new UInt32Input("b", "b", "b");
            commandSet.Add(child);

            uint actual = commandSet.GetUInt32("c");
            Assert.Equal(uint.MinValue, actual);
        }

        [Fact]
        public void GetUInt32_Exists_WrongType_Throws()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);

            Assert.Throws<ArgumentException>(() => _ = commandSet.GetUInt32("b"));
        }

        [Fact]
        public void GetUInt32_Exists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new UInt32Input("b", "b", "b");
            commandSet.Add(child);

            int index = 0;
            child.ProcessInput(["b", "5"], ref index);

            uint actual = commandSet.GetUInt32("b");
            Assert.Equal(5u, actual);
        }

        [Fact]
        public void GetUInt32_NestedExists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);
            var subChild = new UInt32Input("c", "c", "c");
            child.Add(subChild);

            int index = 0;
            subChild.ProcessInput(["c", "5"], ref index);

            uint actual = commandSet.GetUInt32("c");
            Assert.Equal(5u, actual);
        }

        #endregion

        #region GetUInt64

        [Fact]
        public void GetUInt64_InvalidKey_DefaultValue()
        {
            var commandSet = new CommandSet();
            var child = new UInt64Input("b", "b", "b");
            commandSet.Add(child);

            ulong actual = commandSet.GetUInt64("c");
            Assert.Equal(ulong.MinValue, actual);
        }

        [Fact]
        public void GetUInt64_Exists_WrongType_Throws()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);

            Assert.Throws<ArgumentException>(() => _ = commandSet.GetUInt64("b"));
        }

        [Fact]
        public void GetUInt64_Exists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new UInt64Input("b", "b", "b");
            commandSet.Add(child);

            int index = 0;
            child.ProcessInput(["b", "5"], ref index);

            ulong actual = commandSet.GetUInt64("b");
            Assert.Equal(5u, actual);
        }

        [Fact]
        public void GetUInt64_NestedExists_Returns()
        {
            var commandSet = new CommandSet();
            var child = new MockUserInput("b", "b", "b");
            commandSet.Add(child);
            var subChild = new UInt64Input("c", "c", "c");
            child.Add(subChild);

            int index = 0;
            subChild.ProcessInput(["c", "5"], ref index);

            ulong actual = commandSet.GetUInt64("c");
            Assert.Equal(5u, actual);
        }

        #endregion

        #region ProcessArgs

        [Fact]
        public void ProcessArgs_EmptyArgs_Success()
        {
            CommandSet commandSet = new CommandSet();

            string[] args = [];

            bool actual = commandSet.ProcessArgs(args);
            Assert.True(actual);
        }

        [Fact]
        public void ProcessArgs_ValidArgs_Success()
        {
            CommandSet commandSet = new CommandSet();
            Feature feature = new MockFeature("a", "a", "a");
            feature.Add(new FlagInput("b", "b", "b"));
            feature.Add(new FlagInput("c", "c", "c"));
            commandSet.Add(feature);

            string[] args = ["a", "b", "c"];

            bool actual = commandSet.ProcessArgs(args);
            Assert.True(actual);
            Assert.Empty(feature.Inputs);
        }

        [Fact]
        public void ProcessArgs_InvalidArg_AddedAsGeneric()
        {
            CommandSet commandSet = new CommandSet();
            Feature feature = new MockFeature("a", "a", "a");
            feature.Add(new FlagInput("b", "b", "b"));
            feature.Add(new FlagInput("d", "d", "d"));
            commandSet.Add(feature);

            string[] args = ["a", "b", "c"];

            bool actual = commandSet.ProcessArgs(args);
            Assert.True(actual);
            string input = Assert.Single(feature.Inputs);
            Assert.Equal("c", input);
        }

        [Fact]
        public void ProcessArgs_NestedArgs_Success()
        {
            CommandSet commandSet = new CommandSet();
            Feature feature = new MockFeature("a", "a", "a");
            var sub = new FlagInput("b", "b", "b");
            sub.Add(new FlagInput("c", "c", "c"));
            feature.Add(sub);
            commandSet.Add(feature);

            string[] args = ["a", "b", "c"];

            bool actual = commandSet.ProcessArgs(args);
            Assert.True(actual);
            Assert.Empty(feature.Inputs);
        }

        #endregion

        /// <summary>
        /// Mock Feature implementation for testing
        /// </summary>
        private class MockFeature : Feature
        {
            public MockFeature(string name, string flag, string description, string? detailed = null)
                : base(name, flag, description, detailed)
            {
            }

            public MockFeature(string name, string[] flags, string description, string? detailed = null)
                : base(name, flags, description, detailed)
            {
            }

            /// <inheritdoc/>
            public override bool Execute() => true;

            /// <inheritdoc/>
            public override bool VerifyInputs() => true;
        }

        /// <summary>
        /// Mock UserInput implementation for testing
        /// </summary>
        private class MockUserInput : UserInput<object?>
        {
            public MockUserInput(string name, string flag, string description, string? detailed = null)
                : base(name, flag, description, detailed)
            {
            }

            public MockUserInput(string name, string[] flags, string description, string? detailed = null)
                : base(name, flags, description, detailed)
            {
            }

            /// <inheritdoc/>
            public override bool ProcessInput(string[] args, ref int index) => true;

            /// <inheritdoc/>
            protected override string FormatFlags() => string.Empty;
        }
    }
}
