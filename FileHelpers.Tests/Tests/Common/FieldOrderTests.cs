using System;
using NUnit.Framework;
using NFluent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileHelpers.Tests.CommonTests
{
    [TestFixture]
    public class FieldOrderTests
    {
        string data_standard = "Field1,field2,Field3,Field4,Field5" + Environment.NewLine +
                          "1,2,Three,4,31012019" + Environment.NewLine +
                          "11,22,ThreeThree,44,28022019" + Environment.NewLine;
        string[] fields_standard = new string[] { "Field1", "field2", "Field3", "Field4", "Field5" };

        string data_WithExtraFields = "Field1,field2,Field3,Field4,Field5,Field6" + Environment.NewLine +
                          "1,2,Three,4,31012019,a" + Environment.NewLine +
                          "11,22,ThreeThree,44,28022019,a" + Environment.NewLine;

        string data_WithExtraFieldsInMiddle = "Field1,field6,field2,Field3,Field4,Field5" + Environment.NewLine +
                          "1,a,2,Three,4,31012019" + Environment.NewLine +
                          "11,b,22,ThreeThree,44,28022019" + Environment.NewLine;
        string[] fields_withextrainmiddle = new string[] { "Field1", "field6", "field2", "Field3", "Field4", "Field5" };

        [Test]
        public void SimpleOrder()
        {
            var engine = new FileHelperEngine<FieldOrderType>();

            Assert.AreEqual(6, engine.Options.FieldCount);
            Assert.AreEqual("Field1", engine.Options.FieldsNames[0]);
            Assert.AreEqual("Field2", engine.Options.FieldsNames[1]);
            Assert.AreEqual("Field3", engine.Options.FieldsNames[2]);
            Assert.AreEqual("Field4", engine.Options.FieldsNames[3]);
            Assert.AreEqual("Field5", engine.Options.FieldsNames[4]);
        }


        [Test]
        public void UsingAttributeToChangeOrder()
        {
            var engine = new FileHelperEngine<FieldOrderTypeSorted>();

            Assert.AreEqual(5, engine.Options.FieldCount);
            Assert.AreEqual("Field2", engine.Options.FieldsNames[0]);
            Assert.AreEqual("Field1", engine.Options.FieldsNames[1]);
            Assert.AreEqual("Field5", engine.Options.FieldsNames[2]);
            Assert.AreEqual("Field4", engine.Options.FieldsNames[3]);
            Assert.AreEqual("Field3", engine.Options.FieldsNames[4]);
        }


        [Test]
        public void UsingAttributeToChangeOrderAutoProperties()
        {
            var engine = new FileHelperEngine<FieldOrderTypeSortedAutoProperties>();

            Assert.AreEqual(5, engine.Options.FieldCount);
            Assert.AreEqual("Field2", engine.Options.FieldsNames[0]);
            Assert.AreEqual("Field1", engine.Options.FieldsNames[1]);
            Assert.AreEqual("Field5", engine.Options.FieldsNames[2]);
            Assert.AreEqual("Field4", engine.Options.FieldsNames[3]);
            Assert.AreEqual("Field3", engine.Options.FieldsNames[4]);
        }

        [IgnoreFirst]
        [DelimitedRecord(",")]
        [IgnoreInheritedClass]
        public class FieldOrderType
        {
            [FieldCaption("Field1")]
            public int Field1;

            [FieldCaption("Field2")]
            public int Field2;

            [FieldCaption("Field3")]
            public string Field3;

            [FieldCaption("Field4")]
            public int Field4;

            [FieldCaption("Field5")]
            public DateTime Field5;

            [FieldQuoted('"', QuoteMode.OptionalForBoth), FieldOptional]
            public string[] Extras;
        }


        [DelimitedRecord("\t")]
        public class FieldOrderTypeSorted
        {
            [FieldOrder(-5)]
            public int Field1;

            [FieldOrder(-10)]
            public int Field2;

            [FieldOrder(10)]
            public string Field3;

            [FieldOrder(5)]
            public int Field4;

            [FieldOrder(1)]
            public DateTime Field5;
        }



        [DelimitedRecord("\t")]
        public class FieldOrderTypeSortedAutoProperties
        {
            [FieldOrder(-5)]
            public int Field1 { get; set; }

            [FieldOrder(-10)]
            public int Field2 { get; set; }

            [FieldOrder(10)]
            public string Field3 { get; set; }

            [FieldOrder(5)]
            public int Field4 { get; set; }

            [FieldOrder(1)]
            public DateTime Field5 { get; set; }
        }


        [Test]
        public void FieldOrderWithSameNumber1()
        {
            Assert.Throws<BadUsageException>
                (
                    () => new FileHelperEngine<FieldOrderSameNumber1>(),
                    "The field: Field5 has the same FieldOrder that: Field3 you must use different values"
                );
        }

        [Test]
        public void FieldOrderWithSameNumber2()
        {
            Assert.Throws<BadUsageException>
                (
                    () => new FileHelperEngine<FieldOrderSameNumber2>(),
                    "The field: Field2 has the same FieldOrder that: Field1 you must use different values"
                );
        }


        [DelimitedRecord("\t")]
        public class FieldOrderSameNumber1
        {
            [FieldOrder(-5)]
            public int Field1;

            [FieldOrder(-10)]
            public int Field2;

            [FieldOrder(10)]
            public string Field3;

            [FieldOrder(5)]
            public int Field4;

            [FieldOrder(10)]
            public DateTime Field5;
        }

        [DelimitedRecord("\t")]
        public class FieldOrderSameNumber2
        {
            [FieldOrder(5)]
            public int Field1;

            [FieldOrder(5)]
            public int Field2;

            [FieldOrder(10)]
            public string Field3;

            [FieldOrder(5)]
            public int Field4;

            [FieldOrder(1)]
            public DateTime Field5;
        }

        [Test]
        public void PartialFieldOrderAppliedMiddle()
        {
            Assert.Throws<BadUsageException>
                (
                    () => new FileHelperEngine<FieldOrderPartialAppliedMiddle>(),
                    "The field: Field3 must be marked with FielOrder because if you use this attribute in one field you must also use it in all."
                );
        }

        [Test]
        public void PartialFieldOrderAppliedLast()
        {
            Assert.Throws<BadUsageException>
                (
                    () => new FileHelperEngine<FieldOrderPartialAppliedLast>(),
                    "The field: Field5 must be marked with FielOrder because if you use this attribute in one field you must also use it in all."
                );
        }


        [Test]
        public void PartialFieldOrderAppliedFirst()
        {
            Assert.Throws<BadUsageException>
                (
                    () => new FileHelperEngine<FieldOrderPartialAppliedFirst>(),
                    "The field: Field1 must be marked with FielOrder because if you use this attribute in one field you must also use it in all."
                );
        }


        [DelimitedRecord("\t")]
        public class FieldOrderPartialAppliedMiddle
        {
            [FieldOrder(4)]
            public int Field1;

            [FieldOrder(1)]
            public int Field2;

            public string Field3;

            [FieldOrder(5)]
            public int Field4;

            [FieldOrder(2)]
            public DateTime Field5;
        }

        [DelimitedRecord("\t")]
        public class FieldOrderPartialAppliedFirst
        {
            public int Field1;

            [FieldOrder(8)]
            public int Field2;

            [FieldOrder(5)]
            public string Field3;

            [FieldOrder(2)]
            public int Field4;

            [FieldOrder(1)]
            public DateTime Field5;
        }

        [DelimitedRecord("\t")]
        public class FieldOrderPartialAppliedLast
        {
            [FieldOrder(1)]
            public int Field1;

            [FieldOrder(2)]
            public int Field2;

            [FieldOrder(5)]
            public string Field3;

            [FieldOrder(4)]
            public int Field4;

            public DateTime Field5;
        }


        [Test]
        public void FieldOptionalPlusFieldOrderWrong1()
        {
            Assert.Throws<BadUsageException>
                (
                    () => new FileHelperEngine<FieldOptionalPlusFieldOrderTypeWrong1>(),
                    ""
                );
        }


        [Test]
        public void FieldOptionalPlusFieldOrderWrong2()
        {
            Assert.Throws<BadUsageException>
                (
                    () => new FileHelperEngine<FieldOptionalPlusFieldOrderTypeWrong2>(),
                    ""
                );
        }

        [DelimitedRecord("\t")]
        public class FieldOptionalPlusFieldOrderTypeWrong1
        {
            [FieldOrder(-5)]
            public int Field1;

            [FieldOrder(-10)]
            public int Field2;

            [FieldOrder(10)]
            public string Field3;

            [FieldOrder(5)]
            public int Field4;

            [FieldOrder(1)]
            [FieldOptional]
            public DateTime Field5;
        }

        [DelimitedRecord("\t")]
        public class FieldOptionalPlusFieldOrderTypeWrong2
        {
            [FieldOrder(-5)]
            public int Field1;

            [FieldOrder(-10)]
            public int Field2;

            [FieldOrder(10)]
            public string Field3;

            [FieldOptional]
            [FieldOrder(5)]
            public int Field4;

            [FieldOrder(1)]
            public DateTime Field5;
        }

        [Test]
        public void FieldOptionalPlusFieldOrderGood1()
        {
            var engine = new FileHelperEngine<FieldOptionalPlusFieldOrderTypeGood1>();

            Check.That(engine.Options.FieldCount).IsEqualTo(5);
        }

        [Test]
        public void FieldOptionalPlusFieldOrderGood2()
        {
            var engine = new FileHelperEngine<FieldOptionalPlusFieldOrderTypeGood2>();

            Check.That(engine.Options.FieldCount).IsEqualTo(5);
        }

        [DelimitedRecord("\t")]
        public class FieldOptionalPlusFieldOrderTypeGood1
        {
            [FieldOrder(-5)]
            public int Field1;

            [FieldOrder(-10)]
            public int Field2;

            [FieldOptional]
            [FieldOrder(10)]
            public string Field3;

            [FieldOrder(5)]
            public int Field4;

            [FieldOrder(1)]
            public DateTime Field5;
        }

        [DelimitedRecord("\t")]
        public class FieldOptionalPlusFieldOrderTypeGood2
        {
            [FieldOrder(-5)]
            public int Field1;

            [FieldOrder(-10)]
            public int Field2;

            [FieldOptional]
            [FieldOrder(10)]
            public string Field3;

            [FieldOptional]
            [FieldOrder(5)]
            public int Field4;

            [FieldOrder(1)]
            public DateTime Field5;
        }

        [Test]
        public void ReOrderFields()
        {
            // arrange
            var engine = new FileHelperEngine<FieldOrderType>();
            List<string> newOrder = new List<string>() { "Field5", "Field4", "Field3" };

            // act
            engine.SetFieldOrder(newOrder.ToArray());

            // assert
            Assert.AreEqual(6, engine.Options.FieldCount);
            Assert.AreEqual("Field5", engine.Options.FieldsNames[0]);
            Assert.AreEqual("Field4", engine.Options.FieldsNames[1]);
            Assert.AreEqual("Field3", engine.Options.FieldsNames[2]);
            Assert.AreEqual("Field1", engine.Options.FieldsNames[3]);
            Assert.AreEqual("Field2", engine.Options.FieldsNames[4]);
        }

        [Test]
        public void ReOrderFieldsWithQuotesInHeaders()
        {
            // arrange
            var engine = new FileHelperEngine<FieldOrderType>();
            List<string> newOrder = new List<string>() { "\"Field5\"", "\"Field4\"", "\"Field3\"" };

            // act
            engine.SetFieldOrder(newOrder.ToArray());

            // assert
            Assert.AreEqual(6, engine.Options.FieldCount);
            Assert.AreEqual("Field5", engine.Options.FieldsNames[0]);
            Assert.AreEqual("Field4", engine.Options.FieldsNames[1]);
            Assert.AreEqual("Field3", engine.Options.FieldsNames[2]);
            Assert.AreEqual("Field1", engine.Options.FieldsNames[3]);
            Assert.AreEqual("Field2", engine.Options.FieldsNames[4]);
        }

        [Test]
        public void ReOrderDelimitedFields()
        {
            // arrange
            var engine = new DelimitedFileEngine<FieldOrderType>();
            List<string> newOrder = new List<string>() { "Field5", "Field4", "Field3" };

            // act
            //engine.SetFieldOrder(newOrder);
            engine.SetFieldOrder(newOrder.ToArray());

            // assert
            Assert.AreEqual(6, engine.Options.FieldCount);
            Assert.AreEqual("Field5", engine.Options.FieldsNames[0]);
            Assert.AreEqual("Field4", engine.Options.FieldsNames[1]);
            Assert.AreEqual("Field3", engine.Options.FieldsNames[2]);
            Assert.AreEqual("Field1", engine.Options.FieldsNames[3]);
            Assert.AreEqual("Field2", engine.Options.FieldsNames[4]);
        }



        [Test]
        public void StandardImport_WithoutColumnOrderingFromHeader()
        {
            // arrange
            var engine = new DelimitedFileEngine<FieldOrderType>();
            engine.Options.IgnoreEmptyLines = true;
            engine.ErrorMode = ErrorMode.SaveAndContinue;

            // act
            var res = engine.ReadStream(new StringReader(data_standard), Int32.MaxValue).ToList();

            // assert
            Assert.IsFalse(engine.ErrorManager.HasErrors);
            Assert.IsTrue(res.Count == 2);
            Validate_Data_Standard(res);
        }

        [Test]
        public void StandardImport()
        {
            // arrange
            var engine = new DelimitedFileEngine<FieldOrderType>();
            engine.Options.IgnoreEmptyLines = true;
            engine.ErrorMode = ErrorMode.SaveAndContinue;

            // act
            var res = engine.ReadStream(new StringReader(data_standard), Int32.MaxValue).ToList();

            // assert
            Assert.IsFalse(engine.ErrorManager.HasErrors);
            Assert.IsTrue(res.Count == 2);
            Validate_Data_Standard(res);
        }

        [Test]
        public void StandardImport_WithColumnOrdering()
        {
            // arrange
            var engine = new DelimitedFileEngine<FieldOrderType>();
            engine.Options.IgnoreEmptyLines = true;
            engine.ErrorMode = ErrorMode.SaveAndContinue;

            engine.SetFieldOrder(fields_standard);

            // act
            var res = engine.ReadStream(new StringReader(data_standard), Int32.MaxValue).ToList();

            // assert
            Assert.IsFalse(engine.ErrorManager.HasErrors);
            Assert.IsTrue(res.Count == 2);
            Validate_Data_Standard(res);

        }

        [Test]
        public void StandardImport_WithExtraColumnsAndColumnOrdering()
        {
            // arrange
            var engine = new DelimitedFileEngine<FieldOrderType>();
            engine.Options.IgnoreEmptyLines = true;
            engine.ErrorMode = ErrorMode.SaveAndContinue;
            engine.SetFieldOrder(fields_standard);

            // act
            var res = engine.ReadStream(new StringReader(data_WithExtraFields), Int32.MaxValue).ToList();

            // assert
            Assert.IsFalse(engine.ErrorManager.HasErrors);
            Assert.IsTrue(res.Count == 2);
            Validate_Data_Standard(res);
        }

        [Test]
        public void StandardImport_WithExtraColumnsInMiddleAndColumnOrdering()
        {
            // arrange
            var engine = new DelimitedFileEngine<FieldOrderType>();
            engine.Options.IgnoreEmptyLines = true;
            engine.ErrorMode = ErrorMode.SaveAndContinue;
            engine.SetFieldOrder(fields_withextrainmiddle);

            // act
            var res = engine.ReadStream(new StringReader(data_WithExtraFieldsInMiddle), Int32.MaxValue).ToList();

            // assert
            Assert.IsFalse(engine.ErrorManager.HasErrors);
            Assert.IsTrue(res.Count == 2);
            Validate_Data_Standard(res);

        }

        private void Validate_Data_Standard(List<FieldOrderType> res)
        {
            Assert.AreEqual(1, res[0].Field1);
            Assert.AreEqual(2, res[0].Field2);
            Assert.AreEqual("Three", res[0].Field3);
            Assert.AreEqual(4, res[0].Field4);
            Assert.AreEqual(new DateTime(2019, 01, 31), res[0].Field5);
            Assert.AreEqual(11, res[1].Field1);
            Assert.AreEqual(22, res[1].Field2);
            Assert.AreEqual("ThreeThree", res[1].Field3);
            Assert.AreEqual(44, res[1].Field4);
            Assert.AreEqual(new DateTime(2019, 02, 28), res[1].Field5);
        }

    }
}