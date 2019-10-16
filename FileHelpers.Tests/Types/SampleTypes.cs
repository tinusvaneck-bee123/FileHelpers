using System;

namespace FileHelpers.Tests
{
    [FixedLengthRecord]
    public class SampleType
    {
        [FieldFixedLength(8)]
        [FieldConverter(ConverterKind.Date, "ddMMyyyy")]
        public DateTime Field1;

        [FieldFixedLength(3)]
        [FieldAlign(AlignMode.Left, ' ')]
        [FieldTrim(TrimMode.Both)]
        public string Field2;

        [FieldFixedLength(3)]
        [FieldAlign(AlignMode.Right, '0')]
        [FieldTrim(TrimMode.Both)]
        public int Field3;
    }

    [FixedLengthRecord]
    [IgnoreFirst]
    public class SampleTypeIgnoreFirst
    {
        [FieldFixedLength(8)]
        [FieldConverter(ConverterKind.Date, "ddMMyyyy")]
        public DateTime Field1;

        [FieldFixedLength(3)]
        [FieldAlign(AlignMode.Left, ' ')]
        [FieldTrim(TrimMode.Both)]
        public string Field2;

        [FieldFixedLength(3)]
        [FieldAlign(AlignMode.Right, '0')]
        [FieldTrim(TrimMode.Both)]
        [FieldConverter(ConverterKind.Int32)]
        public int Field3;
    }

    [FixedLengthRecord]
    [IgnoreFirst(2)]
    [IgnoreLast(2)]
    public class SampleTypeIgnoreFirstLast
    {
        [FieldFixedLength(8)]
        [FieldConverter(ConverterKind.Date, "ddMMyyyy")]
        public DateTime Field1;

        [FieldFixedLength(3)]
        [FieldAlign(AlignMode.Left, ' ')]
        [FieldTrim(TrimMode.Both)]
        public string Field2;

        [FieldFixedLength(3)]
        [FieldAlign(AlignMode.Right, '0')]
        [FieldTrim(TrimMode.Both)]
        [FieldConverter(ConverterKind.Int32)]
        public int Field3;
    }


    [FixedLengthRecord]
    public class SampleTypeInt
    {
        [FieldFixedLength(8)]
        public int Field1;

        [FieldFixedLength(3)]
        [FieldAlign(AlignMode.Left, ' ')]
        [FieldTrim(TrimMode.Both)]
        public int Field2;
    }

    [DelimitedRecord(",")]
    public class SampleTypeNullableGuid
    {
        public Guid? Field1;
    }

    [IgnoreFirst] // For the header.
    [DelimitedRecord(",")]
    [IgnoreInheritedClass]
    public class SampleType5Fields
    {
        [FieldCaption("Field1")]
        public string Field1;
        [FieldCaption("Field2")]
        public string Field2;
        [FieldCaption("Field3")]
        public string Field3;
        [FieldCaption("Field4")]
        public string Field4;
        [FieldCaption("Field5")]
        public string Field5;

        [FieldCaption("Extras")]
        [FieldOptional]
        public string[] Extras;
    }
}