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
                {"numlist1", new List<double>(){1,2,3,4,5}},
                {"str1", "aaa"},
                {"str2", "bbb"},
                {"str3", "ccc"},
                {"str4", "ddd"},
                {"strlist1", new List<string>(){"aaa","bbb","ccc"}},
            };
            Assert.AreEqual(4.2, Expression.From("$num1+$num2").Invoke(parameters).NumberResult, Delta);
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
                {"numlist1", new List<double>(){1,2,3,4,5}},
                {"str1", "aaa"},
                {"str2", "bbb"},
                {"str3", "ccc"},
                {"str4", "ddd"},
                {"strlist1", new List<string>(){"aaa","bbb","ccc"}},
            };
            Assert.AreEqual(4.2, Expression.From("num1+num2").Invoke(parameters).NumberResult, Delta);
            Assert.AreEqual(true, Expression.From("IN(num1,numlist1)").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("IN(num2,numlist1,2.2)").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("IN(str1,strlist1)").Invoke(parameters).BooleanResult);
            Assert.AreEqual(false, Expression.From("IN(str4,strlist1)").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("IN(str4,strlist1) && IN(str1,strlist1) || (1+2*3-4)==MAX(1+2,2)").Invoke(parameters).BooleanResult);


            ExpressionSetting.SetSetting(new ExpressionSetting());
        }
    }
    
    public class MySetting : ExpressionSetting
    {
        public override bool HasStringStartChar()
        {
            return false;
        }

        public override bool HasStringEndChar()
        {
            return false;
        }

        public override bool HasVariableEndChar()
        {
            return false;
        }

        public override bool IsVariableStartChar(char c)
        {
            return c == '$';
        }
    }

    public class MySetting2 : ExpressionSetting
    {
        public override bool HasStringStartChar()
        {
            return true;
        }

        public override bool HasStringEndChar()
        {
            return true;
        }

        public override bool IsStringStartChar(char c)
        {
            return c == StringStartChar;
        }

        public override bool IsStringEndChar(char c)
        {
            return c == StringEndChar;
        }

        public override bool HasVariableStartChar()
        {
            return false;
        }

        public override bool HasVariableEndChar()
        {
            return false;
        }
    }
}