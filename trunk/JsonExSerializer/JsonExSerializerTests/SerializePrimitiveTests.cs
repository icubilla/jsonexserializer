using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using JsonExSerializer;

namespace JsonExSerializerTests
{
    [TestFixture]
    public class SerializePrimitiveTests
    {

        [Test]
        public void SerializeLongTest()
        {
            Serializer s = Serializer.GetSerializer(typeof(long));
            s.Options.SetJsonStrictOptions();
            long expected = 32;
            string result = s.Serialize(expected);
            Assert.AreEqual(expected.ToString(), result.Trim(), "long Positive did not serialize correctly");
            long actual = (long)s.Deserialize(result);
            Assert.AreEqual(expected, actual, "long Positive did not deserialize correctly");

            expected = 0;
            result = s.Serialize(expected);
            Assert.AreEqual(expected.ToString(), result.Trim(), "long Zero did not serialize correctly");
            actual = (long)s.Deserialize(result);
            Assert.AreEqual(expected, actual, "long zero did not deserialize correctly");

            expected = -23;
            result = s.Serialize(expected);
            Assert.AreEqual(expected.ToString(), result.Trim(), "long Negative did not serialize correctly");
            actual = (long)s.Deserialize(result);
            Assert.AreEqual(expected, actual, "long Negative did not deserialize correctly");

        }


        [Test]
        public void SerializeIntTest()
        {
            Serializer s = Serializer.GetSerializer(typeof(int));
            s.Options.SetJsonStrictOptions();
            int expected = 32;
            string result = s.Serialize(expected);
            Assert.AreEqual(expected.ToString(), result.Trim(), "Int Positive did not serialize correctly");

            expected = 0;
            result = s.Serialize(expected);
            Assert.AreEqual(expected.ToString(), result.Trim(), "Int Zero did not serialize correctly");

            expected = -23;
            result = s.Serialize(expected);
            Assert.AreEqual(expected.ToString(), result.Trim(), "Int Negative did not serialize correctly");

        }

        [Test]
        public void SerializeUIntTest()
        {
            Serializer s = Serializer.GetSerializer(typeof(uint));
            s.Options.SetJsonStrictOptions();
            uint expected = 32;
            string result = s.Serialize(expected);
            Assert.AreEqual(expected.ToString(), result.Trim(), "UInt Positive did not serialize correctly");

            expected = 0;
            result = s.Serialize(expected);
            Assert.AreEqual(expected.ToString(), result.Trim(), "UInt Zero did not serialize correctly");

            expected = uint.MaxValue;
            result = s.Serialize(expected);
            Assert.AreEqual(expected.ToString(), result.Trim(), "UInt Max did not serialize correctly");

            expected = uint.MinValue;
            result = s.Serialize(expected);
            Assert.AreEqual(expected.ToString(), result.Trim(), "UInt Min did not serialize correctly");
        }

        [Test]
        public void SerializeShortTest()
        {
            Serializer s = Serializer.GetSerializer(typeof(short));
            s.Options.SetJsonStrictOptions();
            short expected = 32;
            string result = s.Serialize(expected);
            Assert.AreEqual(expected.ToString(), result.Trim(), "Short Positive did not serialize correctly");

            expected = 0;
            result = s.Serialize(expected);
            Assert.AreEqual(expected.ToString(), result.Trim(), "Short Zero did not serialize correctly");

            expected = -23;
            result = s.Serialize(expected);
            Assert.AreEqual(expected.ToString(), result.Trim(), "Short Negative did not serialize correctly");

        }

        [Test]
        public void SerializeBoolTest()
        {
            Serializer s = Serializer.GetSerializer(typeof(bool));
            s.Options.SetJsonStrictOptions();
            bool expected = true;
            string result = s.Serialize(expected);
            Assert.AreEqual(expected.ToString(), result.Trim(), "Bool true did not serialize correctly");

            expected = false;

            result = s.Serialize(expected);
            Assert.AreEqual(expected.ToString(), result.Trim(), "Bool false did not serialize correctly");
        }

        [Test]
        public void SerializeFloatTest()
        {
            Serializer s = Serializer.GetSerializer(typeof(float));
            s.Options.SetJsonStrictOptions();
            float expected = 32.34f;
            float actual;
            string result = s.Serialize(expected);
            Assert.AreEqual(expected, float.Parse(result), "float Positive did not serialize correctly");
            actual = (float)s.Deserialize(result);
            Assert.AreEqual(expected, actual, "float positive did not deserialize correctly");

            expected = 0.0f;
            result = s.Serialize(expected);
            Assert.AreEqual(expected, float.Parse(result), "float Zero did not serialize correctly");
            actual = (float)s.Deserialize(result);
            Assert.AreEqual(expected, actual, "float Zero did not deserialize correctly");

            expected = -23.234f;
            result = s.Serialize(expected);
            Assert.AreEqual(expected, float.Parse(result), "float Negative did not serialize correctly");
            actual = (float)s.Deserialize(result);
            Assert.AreEqual(expected, actual, "float Negative did not deserialize correctly");

            expected = float.MaxValue;
            result = s.Serialize(expected);
            Assert.AreEqual(expected, float.Parse(result), "float MaxValue did not serialize correctly");
            actual = (float)s.Deserialize(result);
            Assert.AreEqual(expected, actual, "float MaxValue did not deserialize correctly");

            expected = float.MinValue;
            result = s.Serialize(expected);
            Assert.AreEqual(expected, float.Parse(result), "float MinValue did not serialize correctly");
            actual = (float)s.Deserialize(result);
            Assert.AreEqual(expected, actual, "float MinValue did not deserialize correctly");

            expected = float.NaN;
            result = s.Serialize(expected);
            Assert.AreEqual(expected, float.Parse(result), "float NaN did not serialize correctly");
            actual = (float)s.Deserialize(result);
            Assert.AreEqual(expected, actual, "float NaN did not deserialize correctly");

            expected = float.PositiveInfinity;
            result = s.Serialize(expected);
            Assert.AreEqual(expected, float.Parse(result), "float PositiveInfinity did not serialize correctly");
            actual = (float)s.Deserialize(result);
            Assert.AreEqual(expected, actual, "float PositiveInfinity did not deserialize correctly");

            //TODO: Implement serialization support for NegativeInfinity...not too common I would think
            /*            
            expected = float.NegativeInfinity;
            result = s.Serialize(expected);
            Assert.AreEqual(expected, float.Parse(result), "float NegativeInfinity did not serialize correctly");
            actual = (float)s.Deserialize(result);
            Assert.AreEqual(expected, actual, "float NegativeInfinity did not deserialize correctly");
             */
        }

        [Test]
        public void SerializeDoubleTest()
        {
            Serializer s = Serializer.GetSerializer(typeof(double));
            s.Options.SetJsonStrictOptions();
            double expected = 32.34;
            string result = s.Serialize(expected);
            Assert.AreEqual(expected, double.Parse(result), "double Positive did not serialize correctly");

            expected = 0.0;
            result = s.Serialize(expected);
            Assert.AreEqual(expected, double.Parse(result), "double Zero did not serialize correctly");

            expected = -23.234;
            result = s.Serialize(expected);
            Assert.AreEqual(expected, double.Parse(result), "double Negative did not serialize correctly");
        }

        [Test]
        public void SerializeByteTest()
        {
            Serializer s = Serializer.GetSerializer(typeof(byte));
            s.Options.SetJsonStrictOptions();
            byte expected = 0xff;
            string result = s.Serialize(expected);
            Assert.AreEqual(expected, byte.Parse(result), "byte Positive did not serialize correctly");

            expected = 0x0;
            result = s.Serialize(expected);
            Assert.AreEqual(expected, byte.Parse(result), "byte Zero did not serialize correctly");


            expected = 0x1;
            result = s.Serialize(expected);
            Assert.AreEqual(expected, byte.Parse(result), "byte 1 did not serialize correctly");
        }

        [Test]
        public void SerializeStringTest()
        {
            Serializer s = Serializer.GetSerializer(typeof(string));
            s.Options.SetJsonStrictOptions();
            string expected = "simple";
            string result = s.Serialize(expected);
            Assert.AreEqual("\"simple\"", result, "String did not serialize correctly.");

        }
    }
}
