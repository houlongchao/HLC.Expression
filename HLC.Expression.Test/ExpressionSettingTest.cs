using System.Collections.Generic;
using NUnit.Framework;

namespace HLC.Expression.Test
{
    public class ExpressionSettingTest
    {
        private const double Delta = 0.00001;

        [Test]
        public void Test()
        {
            ExpressionSetting.SetSetting(new MySetting());

            Assert.AreEqual(false, Expression.From("NOT(true)").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("NOT(false)").Invoke().BooleanResult);

            Assert.AreEqual(true, Expression.From("IF(1==1,true,false)").Invoke().BooleanResult);
            Assert.AreEqual("A1", Expression.From("IF(1==1,A1,B2)").Invoke().ToString());


            var parameters = new Parameters()
            {
                {"num1", 2},
                {"num2", 2.2},
                {"num3", -2},
                {"num4", -2.2},
                {"numlist1", new List<decimal>(){1,2,3,4,5}},
                {"str1", "aaa"},
                {"str2", "bbb"},
                {"str3", "ccc"},
                {"str4", "ddd"},
                {"strlist1", new List<string>(){"aaa","bbb","ccc"}},
            };
            Assert.AreEqual(4.2m, Expression.From("$num1+$num2").Invoke(parameters).NumberResult);
            Assert.AreEqual(true, Expression.From("IN($num1,$numlist1)").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("IN($num2,$numlist1,2.2)").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("IN($str1,$strlist1)").Invoke(parameters).BooleanResult);
            Assert.AreEqual(false, Expression.From("IN($str4,$strlist1)").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("IN($str4,$strlist1) && IN($str1,$strlist1) || (1+2*3-4)==MAX(1+2,2)").Invoke(parameters).BooleanResult);

            ExpressionSetting.SetSetting(new ExpressionSetting());
        }

        [Test]
        public void Test2()
        {
            ExpressionSetting.SetSetting(new MySetting2());

            Assert.AreEqual(false, Expression.From("NOT(true)").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("NOT(false)").Invoke().BooleanResult);

            Assert.AreEqual(true, Expression.From("IF(1==1,true,false)").Invoke().BooleanResult);
            Assert.AreEqual("A1", Expression.From("IF(1==1,'A1','B2')").Invoke().ToString());


            var parameters = new Parameters()
            {
                {"num1", 2},
                {"num2", 2.2},
                {"num3", -2},
                {"num4", -2.2},
                {"numlist1", new List<decimal>(){1,2,3,4,5}},
                {"str1", "aaa"},
                {"str2", "bbb"},
                {"str3", "ccc"},
                {"str4", "ddd"},
                {"strlist1", new List<string>(){"aaa","bbb","ccc"}},
            };
            Assert.AreEqual(4.2m, Expression.From("num1+num2").Invoke(parameters).NumberResult);
            Assert.AreEqual(true, Expression.From("IN(num1,numlist1)").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("IN(num2,numlist1,2.2)").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("IN(str1,strlist1)").Invoke(parameters).BooleanResult);
            Assert.AreEqual(false, Expression.From("IN(str4,strlist1)").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("IN(str4,strlist1) && IN(str1,strlist1) || (1+2*3-4)==MAX(1+2,2)").Invoke(parameters).BooleanResult);
        }

        [Test]
        public void Test3()
        {
            Assert.AreEqual(0, Expression.From("0/0").Invoke().NumberResult);
            Assert.AreEqual(0, Expression.From("12/0").Invoke().NumberResult);
        }
    }

    public class MySetting : ExpressionSetting
    {
        public override bool HasStringStartChar => false;
        public override bool HasStringEndChar => false;
        public override bool HasVariableEndChar => false;
        public override char VariableStartChar => '$';
    }

    public class MySetting2 : ExpressionSetting
    {
        public override bool HasVariableStartChar => false;
        public override bool HasVariableEndChar => false;
    }
}
