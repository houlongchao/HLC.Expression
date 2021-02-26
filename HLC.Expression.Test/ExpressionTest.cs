using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace HLC.Expression.Test
{
    public class ExpressionTest
    {
        private const double Delta = 0.00001;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestSimpleInvoke()
        {
            Assert.AreEqual(52, Expression.From("50+2").Invoke().NumberResult, Delta);
            Assert.AreEqual(48, Expression.From("50-2").Invoke().NumberResult, Delta);
            Assert.AreEqual(100, Expression.From("50*2").Invoke().NumberResult, Delta);
            Assert.AreEqual(25, Expression.From("50/2").Invoke().NumberResult, Delta);

            Assert.AreEqual(-48.2, Expression.From("-50.2+2").Invoke().NumberResult, Delta);
            Assert.AreEqual(-52.2, Expression.From("-50.2-2").Invoke().NumberResult, Delta);
            Assert.AreEqual(-100.4, Expression.From("-50.2*2").Invoke().NumberResult, Delta);
            Assert.AreEqual(-25.1, Expression.From("-50.2/2").Invoke().NumberResult, Delta);

            Assert.AreEqual(-52.2, Expression.From("-50.2+-2").Invoke().NumberResult, Delta);
            Assert.AreEqual(-48.2, Expression.From("-50.2--2").Invoke().NumberResult, Delta);
            Assert.AreEqual(100.4, Expression.From("-50.2*-2").Invoke().NumberResult, Delta);
            Assert.AreEqual(25.1, Expression.From("-50.2/-2").Invoke().NumberResult, Delta);

            Assert.AreEqual(2500, Expression.From("50^2").Invoke().NumberResult, Delta);
            Assert.AreEqual(0, Expression.From("50%2").Invoke().NumberResult, Delta);
            Assert.AreEqual(1, Expression.From("51%2").Invoke().NumberResult, Delta);

            Assert.AreEqual(51, Expression.From("CEILING(50.12)").Invoke().NumberResult, Delta);
            Assert.AreEqual(52, Expression.From("CEILING(50.12+1)").Invoke().NumberResult, Delta);
            Assert.AreEqual(51, Expression.From("CEILING(50.12+1-1)").Invoke().NumberResult, Delta);

            Assert.AreEqual(50, Expression.From("FLOORING(50.12)").Invoke().NumberResult, Delta);
            Assert.AreEqual(51, Expression.From("FLOORING(50.12+1)").Invoke().NumberResult, Delta);
            Assert.AreEqual(50, Expression.From("FLOORING(50.12+1-1)").Invoke().NumberResult, Delta);

            Assert.AreEqual(50, Expression.From("ROUNDING(50.12)").Invoke().NumberResult, Delta);
            Assert.AreEqual(50.1, Expression.From("ROUNDING(50.12, 1)").Invoke().NumberResult, Delta);
            Assert.AreEqual(50.13, Expression.From("ROUNDING(50.125, 2)").Invoke().NumberResult, Delta);

            Assert.AreEqual(60.12, Expression.From("MAX(50.12,1,50.12+10)").Invoke().NumberResult, Delta);
            Assert.AreEqual(0.12, Expression.From("MIN(50.12,1, 50.12-50)").Invoke().NumberResult, Delta);
            Assert.AreEqual(1, Expression.From("IF(50.12>12,1, 50.12-50)").Invoke().NumberResult, Delta);
            Assert.AreEqual(0.12, Expression.From("IF(50.12==12,1, 50.12-50)").Invoke().NumberResult, Delta);


            Assert.AreEqual(true, Expression.From("2.1>1.1").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("2.1>=1.1").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("2.1<1.1").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("2.1<=1.1").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("2.1==1.1").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("2.1!=1.1").Invoke().BooleanResult);

            Assert.AreEqual(false, Expression.From("AND(2.1!=1.1,2.1<1.1)").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("OR(2.1!=1.1,2.1<1.1)").Invoke().BooleanResult);

            Assert.AreEqual(true, Expression.From("(1+2*3-4)==MAX(1+2,2)").Invoke().BooleanResult);

            Assert.AreEqual(false, Expression.From("2.1!=1.1&&2.1<1.1").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("2.1!=1.1||2.1<1.1").Invoke().BooleanResult);

            Assert.AreEqual(true, Expression.From("IN(1,2,3,4,1)").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("IN(1.1,2,1.1,4,5)").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("IN(2.1,2,1.1,4,5)").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("IN('AA','A','AAA','BB')").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("IN('西','一路','向西')").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("IN('西','一','路','向','西')").Invoke().BooleanResult);

            Assert.AreEqual(true, Expression.From("NOT(2.1!=1.1&&2.1<1.1)").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("NOT(2.1!=1.1||2.1<1.1)").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("NOT(1==1)").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("NOT(true)").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("NOT(false)").Invoke().BooleanResult);

            Assert.AreEqual(true, Expression.From("IF(1==1,true,false)").Invoke().BooleanResult);
            Assert.AreEqual("1", Expression.From("IF(1==1,'1','2')").Invoke().ToString());

            var expression = Expression.From("NOT(2.1!=1.1||2.1<1.1)");
            var serialize = JsonConvert.SerializeObject(expression);

            var @from = Expression.From("IF(NOT({test}>{test2}&&({test}>5||{test}<=1)),{test2},{test3})");
        }

        [Test]
        public void TestParameterInvoke()
        {
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
            Assert.AreEqual(4.2, Expression.From("{num1}+{num2}").Invoke(parameters).NumberResult, Delta);
            Assert.AreEqual(true, Expression.From("IN({num1},{numlist1})").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("IN({num2},{numlist1},2.2)").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("IN({str1},{strlist1})").Invoke(parameters).BooleanResult);
            Assert.AreEqual(false, Expression.From("IN({str4},{strlist1})").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("IN({str4},{strlist1}) && IN({str1},{strlist1}) || (1+2*3-4)==MAX(1+2,2)").Invoke(parameters).BooleanResult);

            Assert.AreEqual("2", Expression.From("IF({str4}=='a','1','2')").Invoke(parameters).Data);

            var expression = Expression.From("IN({num2},{numlist1},2.2)");
            Assert.AreEqual(true, Expression.From("IN({str4},'ddd') && IN({str1},{strlist1})").Invoke(parameters).BooleanResult);

            Expression from = Expression.From("1+2*3-4/5+4^2-5%2+{aaa}+'b'+IN(1,2,3)-CEILING(1+2+MIN(1,2,3))");
            var s = from.ToString();
            var s2 = Expression.From(s).ToString();
            var serializeObject = JsonConvert.SerializeObject(@from);
            Console.WriteLine(s);
            Console.WriteLine(s2);
            Assert.AreEqual(s, s2);
        }
    }
}