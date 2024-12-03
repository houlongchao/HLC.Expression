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
        [SetUp]
        public void Setup()
        {
            ExpressionSetting.SetSetting(new ExpressionSetting());
        }

        private Parameters InitParameters()
        {
            var parameters = new Parameters()
            {
                {"num", 123456},
                {"num1", 2},
                {"num2", 2.2},
                {"num3", -2},
                {"num4", -2.2},
                {"numlist1", new List<decimal>(){0,1,2,3,4,5}},
                {"numlist2", new List<decimal>(){1,1,2,3,5,8,11,19,8,5,3,2,1,1}},
                {"numlist3", new List<string>(){"1","1","2","3","5","8","11","19","8","5","3","2","1","1"}},
                {"str", "abcde"},
                {"str1", "aaa"},
                {"str2", "bbb"},
                {"str3", "ccc"},
                {"str4", "ddd"},
                {"str5", "{d'd''d}'"},
                {"{str6}", "{d'd''d}'"},
                {"strnum", "123.3"},
                {"empty", ""},
                {"A", "是" },
                {"B", "是" },
                {"是", "是" },
                {"否", "否" },
                {"true", true },
                {"false", false },
                {"true2", "true" },
                {"false2", "false" },
                {"strlist1", new List<string>(){"aaa","bbb","ccc"}},
                {"range", new Parameter("(1,5)", ParameterType.Range)},
                {"dt", new Parameter(DateTime.Parse("2021-01-01"))},
                {"10", "10" },
                {"1", 1 },
                {"+", "+" },
                {"range2", "[1, 10]" },
            };

            parameters["metadata"] = new Parameter("metadataValue");
            parameters["metadata"].Metadata["meta01"] = "value01";
            parameters["metadata"].Metadata["meta02"] = "True";
            parameters["metadata"].Metadata["meta03"] = "123";
            parameters["metadata"].DataMetadata["metadataValue", "name"] = "metadataName";


            return parameters;
        }

        [Test]
        public void TestSimpleInvoke()
        {
            Assert.AreEqual(3m, Expression.From("ASNUM('3')").Invoke().NumberResult);
            Assert.AreEqual(3m, Expression.From("TONUM('3')").Invoke().NumberResult);

            Assert.AreEqual(true, Expression.From("ISSTART('123456', '123')").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("ISSTART('123456', '1233')").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("ISEND('123456', '456')").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("ISEND('123456', '567')").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("ISMATCH('123456', '^\\d*$')").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("ISMATCH('123456', '^\\D*$')").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("ISMATCH('123456', '^1\\d{5}$')").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("ISMATCH('123456', '^1\\d{4}$')").Invoke().BooleanResult);

            Assert.AreEqual(0, Expression.From("11/0").Invoke().NumberResult);

            Assert.AreEqual("2021-01-01", Expression.From("TOSTR(DATETIME('2021-01-01'), 'yyyy-MM-dd')").Invoke().StringResult);
            Assert.AreEqual("2021-01-01 00:00:00", Expression.From("TOSTR(DATETIME('2021-01-01'))").Invoke().StringResult);

            Assert.AreEqual(DateTime.Parse("2021-01-01"), Expression.From("DATETIME('2021-01-01')").Invoke().DateTimeResult);
            Assert.AreEqual(false, Expression.From("DATETIME('2021-01-01 12:00')>'2021-01-02'").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("DATETIME('2021-01-01')<'2021-01-02'").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("DATETIME('202101011200', 'yyyyMMddHHmm')<'2021-01-02'").Invoke().BooleanResult);

            Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Expression.From("NOW()").Invoke().StringResult);
            Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Expression.From("NOW('other')").Invoke().StringResult);
            Assert.AreEqual(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), Expression.From("NOW('UTC')").Invoke().StringResult);

            Assert.AreEqual("bc", Expression.From("SUBSTR('abcde', 1, 2)").Invoke().ToString());
            Assert.AreEqual("ab", Expression.From("SUBSTR('abcde', 0, 2)").Invoke().ToString());
            Assert.AreEqual("abcde", Expression.From("SUBSTR('abcde', 0)").Invoke().ToString());
            Assert.AreEqual("bcde", Expression.From("SUBSTR('abcde', 1)").Invoke().ToString());
            Assert.AreEqual("4", Expression.From("SWITCH('ab','bc':1, 12:2, 'A':'3', '':'4')").Invoke().ToString());

            Assert.AreEqual(12, Expression.From("SUBNUM('1234', 0, 2)").Invoke().NumberResult);
            Assert.AreEqual(23, Expression.From("SUBNUM('1234', 1, 2)").Invoke().NumberResult);
            Assert.AreEqual(234, Expression.From("SUBNUM('1234', 1)").Invoke().NumberResult);
            Assert.AreEqual("abc", Expression.From("LEFT('abcde', 3)").Invoke().ToString());
            Assert.AreEqual("cde", Expression.From("RIGHT('abcde', 3)").Invoke().ToString());
            Assert.AreEqual("edcba", Expression.From("REVERSE('abcde')").Invoke().ToString());
            Assert.AreEqual(2, Expression.From("FIND('abcde', 'c')").Invoke().NumberResult);
            Assert.AreEqual(1, Expression.From("FIND('abcde', 'bc')").Invoke().NumberResult);
            Assert.AreEqual(-1, Expression.From("FIND('abcde', 'cc')").Invoke().NumberResult);
            Assert.AreEqual(5, Expression.From("LENGTH('abcde')").Invoke().NumberResult);

            Assert.AreEqual(2, Expression.From("SWITCH('a','bc':1, 'a':2, '2':'3')").Invoke().NumberResult);
            Assert.AreEqual("3", Expression.From("SWITCH(15,'bc':1, 12:2, 4+11:'3')").Invoke().ToString());
            Assert.AreEqual("3", Expression.From("SWITCH('a','bc':1, 12:2, 'A':'3')").Invoke().ToString());

            Assert.AreEqual("2", Expression.From("SWITCHC('asdf','bc':1, 'as':'2', 'DF':'3')").Invoke().ToString());
            Assert.AreEqual("3", Expression.From("SWITCHC('asdf','bc':1, 'ass':'2', 'DF':'3')").Invoke().ToString());
            Assert.AreEqual("4", Expression.From("SWITCHC('aaaa','bc':1, 12:2, 'Ab':'3', '':'4')").Invoke().ToString());

            Assert.AreEqual(2, Expression.From("2").Invoke().NumberResult);
            Assert.AreEqual(2, Expression.From("2.00").Invoke().NumberResult);
            Assert.AreEqual(2, Expression.From("(2.00)").Invoke().NumberResult);

            Assert.AreEqual(52, Expression.From("50+2").Invoke().NumberResult);
            Assert.AreEqual(48, Expression.From("50-2").Invoke().NumberResult);
            Assert.AreEqual(100, Expression.From("50*2").Invoke().NumberResult);
            Assert.AreEqual(25, Expression.From("50/2").Invoke().NumberResult);
            Assert.AreEqual(3.373m, Expression.From("2.139+1.234").Invoke().NumberResult);

            Assert.AreEqual(-48.2m, Expression.From("-50.2+2").Invoke().NumberResult);
            Assert.AreEqual(-52.2m, Expression.From("-50.2-2").Invoke().NumberResult);
            Assert.AreEqual(-100.4m, Expression.From("-50.2*2").Invoke().NumberResult);
            Assert.AreEqual(-25.1m, Expression.From("-50.2/2").Invoke().NumberResult);

            Assert.AreEqual(-52.2m, Expression.From("-50.2+-2").Invoke().NumberResult);
            Assert.AreEqual(-48.2m, Expression.From("-50.2--2").Invoke().NumberResult);
            Assert.AreEqual(100.4m, Expression.From("-50.2*-2").Invoke().NumberResult);
            Assert.AreEqual(25.1m, Expression.From("-50.2/-2").Invoke().NumberResult);

            Assert.AreEqual(2500, Expression.From("50^2").Invoke().NumberResult);
            Assert.AreEqual(0, Expression.From("50%2").Invoke().NumberResult);
            Assert.AreEqual(1, Expression.From("51%2").Invoke().NumberResult);

            Assert.AreEqual(51, Expression.From("CEILING(50.12)").Invoke().NumberResult);
            Assert.AreEqual(52, Expression.From("CEILING(50.12+1)").Invoke().NumberResult);
            Assert.AreEqual(51, Expression.From("CEILING(50.12+1-1)").Invoke().NumberResult);

            Assert.AreEqual(50, Expression.From("FLOORING(50.12)").Invoke().NumberResult);
            Assert.AreEqual(51, Expression.From("FLOORING(50.12+1)").Invoke().NumberResult);
            Assert.AreEqual(50, Expression.From("FLOORING(50.12+1-1)").Invoke().NumberResult);

            Assert.AreEqual(50, Expression.From("ROUNDING(50.12)").Invoke().NumberResult);
            Assert.AreEqual(50.1m, Expression.From("ROUNDING(50.12, 1)").Invoke().NumberResult);
            Assert.AreEqual(50.13m, Expression.From("ROUNDING(50.125, 2)").Invoke().NumberResult);

            Assert.AreEqual(60.12m, Expression.From("MAX(50.12,1,50.12+10)").Invoke().NumberResult);
            Assert.AreEqual(0.12m, Expression.From("MIN(50.12,1, 50.12-50)").Invoke().NumberResult);
            Assert.AreEqual(1, Expression.From("IF(50.12>12,1, 50.12-50)").Invoke().NumberResult);
            Assert.AreEqual(0.12m, Expression.From("IF(50.12==12,1, 50.12-50)").Invoke().NumberResult);

            Assert.AreEqual(true, Expression.From("true").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("false").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("(true)").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("(false)").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("2.1>1.1").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("2.1>=1.1").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("2.1<1.1").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("2.1<=1.1").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("2.1==1.1").Invoke().BooleanResult);

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
            Assert.AreEqual(true, Expression.From(@"IN('西''','一','路','向','西''')").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("IN('西','一','路','向','西')").Invoke().BooleanResult);

            Assert.AreEqual(true, Expression.From("NOT(2.1!=1.1&&2.1<1.1)").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("NOT(2.1!=1.1||2.1<1.1)").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("NOT(1==1)").Invoke().BooleanResult);
            Assert.AreEqual(false, Expression.From("NOT(true)").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("NOT(false)").Invoke().BooleanResult);

            Assert.AreEqual(true, Expression.From("IF(1==1,true,false)").Invoke().BooleanResult);
            Assert.AreEqual("1", Expression.From("IF(1==1,'1','2')").Invoke().ToString());

            Assert.AreEqual(true, Expression.From("2.2==(1,3.5)").Invoke().BooleanResult);
            Assert.AreEqual(true, Expression.From("2.2==(1,3.5) && 2.2==[1,10]").Invoke().BooleanResult);
            Assert.AreEqual("abc_def", Expression.From("CONCAT('a','bc','_','def')").Invoke().Data);


            var expression = Expression.From("NOT(2.1!=1.1||2.1<1.1)");
            Assert.AreEqual(true, expression.IsBoolResult());

            var serialize = JsonConvert.SerializeObject(expression);
            Console.WriteLine(serialize);

            var testFormula = Expression.From("IF(NOT({a}>{b}&&({c}>3||{d}<=1)),{e},{f})");
            Assert.IsNotNull(testFormula);
        }

        [Test(Description = "测试非侵入式自定义函数")]
        public void TestCallFuncInvoke()
        {
            var parameters = new Parameters();
            parameters.RegisterCallFunc("Concat", (arg) =>
            {
                return string.Concat(arg);
            });

            Assert.AreEqual("12345", Expression.From("CALL('concat', 1,2,3,4,2+3)").Invoke(parameters).StringResult);
        }

        [Test]
        public void TestParameterInvoke()
        {
            var parameters = InitParameters();

            Assert.AreEqual("aaa", Expression.From("CONCAT({str0},{str1})").Invoke(parameters).StringResult);

            Assert.AreEqual(false, Expression.From("{num}=={range2}").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("{num1}=={range2}").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("{num}!={range2}").Invoke(parameters).BooleanResult);
            Assert.AreEqual(false, Expression.From("{num1}!={range2}").Invoke(parameters).BooleanResult);

            Assert.AreEqual(true, Expression.From("{+}=='+'").Invoke(parameters).BooleanResult);

            var aaa = Expression.From("IN({str4},{strlist1}) && IN({str1},{strlist1}) || (1+2*3-4)==MAX(1+2,2)");
            aaa.Invoke(parameters);
            var data = Expression.From("{10}+{1}>=12").Invoke(parameters).Data;
            Assert.AreEqual(true, Expression.From("{strnum}<134").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("{strnum}<=134").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("{strnum}>34").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("{strnum}>=34").Invoke(parameters).BooleanResult);

            Assert.AreEqual(true, Expression.From("{strnum5}<134").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("{strnum5}<=134").Invoke(parameters).BooleanResult);
            Assert.AreEqual(false, Expression.From("{strnum5}>34").Invoke(parameters).BooleanResult);
            Assert.AreEqual(false, Expression.From("{strnum5}>=34").Invoke(parameters).BooleanResult);

            Assert.AreEqual("AA", Expression.From("{strnum5}+'AA'").Invoke(parameters).StringResult);

            Assert.AreEqual(2m, Expression.From("ASNUM({num1})").Invoke(parameters).NumberResult);
            Assert.AreEqual(2.2m, Expression.From("ASNUM({num2})").Invoke(parameters).NumberResult);
            Assert.AreEqual(3m, Expression.From("ASNUM('3')").Invoke(parameters).NumberResult);
            Assert.AreEqual(3m, Expression.From("TONUM('3')").Invoke(parameters).NumberResult);

            Assert.IsFalse(Expression.From("IF({a},true, false)").Invoke(parameters).BooleanResult);
            Assert.IsFalse(Expression.From("IN({a},{b})").Invoke(parameters).BooleanResult);

            Assert.AreEqual("是", Expression.From("GET({是}, 'AA')").Invoke(parameters).StringResult);
            Assert.AreEqual("A", Expression.From("GET({AA}, 'A')").Invoke(parameters).StringResult);
            Assert.AreEqual(2, Expression.From("GET({num1}, 11)").Invoke(parameters).NumberResult);
            Assert.AreEqual(2, Expression.From("GET({num11}, 2)").Invoke(parameters).NumberResult);
            Assert.IsTrue(Expression.From("HASVALUE({A})").Invoke(parameters).BooleanResult);
            Assert.IsFalse(Expression.From("HASVALUE({C})").Invoke(parameters).BooleanResult);
            Assert.IsFalse(Expression.From("HASVALUE({empty})").Invoke(parameters).BooleanResult);
            Assert.IsTrue(Expression.From("ISNUMBER({num})").Invoke(parameters).BooleanResult);
            Assert.IsTrue(Expression.From("ISNUMBER({num}+{num2})").Invoke(parameters).BooleanResult);
            Assert.IsFalse(Expression.From("ISNUMBER({num}+{str1})").Invoke(parameters).BooleanResult);
            Assert.AreEqual(DateTime.Parse("2021-01-01"), Expression.From("{dt}").Invoke(parameters).DateTimeResult);
            Assert.AreEqual(false, Expression.From("{dt}>'2021-01-02'").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("{dt}<'2021-01-02'").Invoke(parameters).BooleanResult);

            var a = Expression.From("{num1}+{num2}+{num1}*{num2}-{num1}*4.5+{num2}*0.5").NewtonSolve("num2", parameters);
            Assert.AreEqual(2, a);
            Assert.IsTrue(Expression.From("{true}").Invoke(parameters).BooleanResult);
            Assert.IsFalse(Expression.From("{false}").Invoke(parameters).BooleanResult);
            Assert.IsTrue(Expression.From("{true}==true&&{false}!={是}&&{true}").Invoke(parameters).BooleanResult);

            Assert.IsFalse(Expression.From("{A}=={是}&&{B}!={是}").Invoke(parameters).BooleanResult);
            Assert.IsFalse(Expression.From("{A}==''").Invoke(parameters).BooleanResult);
            Assert.IsFalse(Expression.From("{A_}==''").Invoke(parameters).BooleanResult);
            Assert.IsTrue(Expression.From("{A_}!=''").Invoke(parameters).BooleanResult);
            Assert.IsTrue(Expression.From("{empty}==''").Invoke(parameters).BooleanResult);
            Assert.IsTrue(Expression.From("{true}=={true2}").Invoke(parameters).BooleanResult);
            Assert.IsTrue(Expression.From("{false}=={false2}").Invoke(parameters).BooleanResult);
            Assert.IsTrue(Expression.From("{true}!={false2}").Invoke(parameters).BooleanResult);
            Assert.IsTrue(Expression.From("{false}!={true2}").Invoke(parameters).BooleanResult);

            Assert.AreEqual("bc", Expression.From("SUBSTR({str}, 1, 2)").Invoke(parameters).ToString());
            Assert.AreEqual("ab", Expression.From("SUBSTR({str}, 0, 2)").Invoke(parameters).ToString());

            Assert.AreEqual(12, Expression.From("SUBNUM({num}, 0, 2)").Invoke(parameters).NumberResult);
            Assert.AreEqual(23, Expression.From("SUBNUM({num}, 1, 2)").Invoke(parameters).NumberResult);

            Assert.AreEqual(2, Expression.From("SWITCH({str},'bc':1, 'abcde':2, '2':'3')").Invoke(parameters).NumberResult);
            Assert.AreEqual("3", Expression.From("SWITCH({num},'bc':1, 12:2, 123000+456:'3')").Invoke(parameters).ToString());
            Assert.AreEqual("3", Expression.From("SWITCH({str},'bc':1, 12:2, 'ABcde':'3')").Invoke(parameters).ToString());

            Assert.AreEqual("2", Expression.From("SWITCHC({str},'bb':1, 'ab':'2', 'de':'3')").Invoke(parameters).ToString());
            Assert.AreEqual("3", Expression.From("SWITCHC({str},'bb':1, 'abB':'2', 'dE':'3')").Invoke(parameters).ToString());


            Assert.AreEqual(4.2m, Expression.From("{num1}+{num2}").Invoke(parameters).NumberResult);
            Assert.AreEqual(true, Expression.From("IN({num1},{numlist1})").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("IN({num2},{numlist1},2.2)").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("IN({str1},{strlist1})").Invoke(parameters).BooleanResult);
            Assert.AreEqual(false, Expression.From("IN({str4},{strlist1})").Invoke(parameters).BooleanResult);
            Assert.AreEqual(false, Expression.From("IN({str4},{strlist1_})").Invoke(parameters).BooleanResult);
            Assert.AreEqual(false, Expression.From("IN({str4_},{str4})").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("IN({str4},{strlist1}) && IN({str1},{strlist1}) || (1+2*3-4)==MAX(1+2,2)").Invoke(parameters).BooleanResult);

            Assert.AreEqual("2", Expression.From("IF({str4}=='a','1','2')").Invoke(parameters).Data);
            Assert.AreEqual("2", Expression.From("IF({str4_},'1','2')").Invoke(parameters).Data);
            Assert.AreEqual("2", Expression.From("IF({str4_}=={str4},'1','2')").Invoke(parameters).Data);

            Assert.AreEqual(true, Expression.From("{str1}=='aaa'").Invoke(parameters).Data);

            Assert.AreEqual(true, Expression.From("2=={range}").Invoke(parameters).Data);

            Assert.AreEqual(true, Expression.From("2=={range}").CheckParameters(parameters));
            Assert.AreEqual(true, Expression.From("{num1}==(1,10)").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("{num1}==[1,10]").Invoke(parameters).BooleanResult);

            Assert.AreEqual(false, Expression.From("2=={null}").CheckParameters(parameters));

            Assert.AreEqual("true == false", Expression.From("true==false").ToString());

            Assert.AreEqual("aaabbb_ccc", Expression.From("CONCAT({str1},{str2},'_',{str3})").Invoke(parameters).Data);
            Assert.AreEqual("aaa", Expression.From("CONCAT({str1})").Invoke(parameters).Data);
            Assert.AreEqual("aaa", Expression.From("CONCAT({str1},{str1_})").Invoke(parameters).Data);
            Assert.AreEqual("aaabbbccc", Expression.From("CONCAT({strlist1})").Invoke(parameters).Data);
            Assert.AreEqual("aaabbbcccaaa", Expression.From("CONCAT({strlist1},{str1})").Invoke(parameters).Data);

            var expression = Expression.From("IN({num2},{numlist1},2.2)");
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Converters.Add(new StringEnumConverter());
            var serialize = JsonConvert.SerializeObject(expression, settings);
            Console.WriteLine(serialize);

            Assert.AreEqual(true, Expression.From("IN({str4},'ddd') && IN({str1},{strlist1})").Invoke(parameters).BooleanResult);

            Expression from = Expression.From("1+2*3-4/5+4^2-5%2+{aaa}+'b'+IN(1,2,3)-CEILING(1+2+MIN(1,2,3))");
            var s = from.ToString();
            var s2 = Expression.From(s).ToString();
            var serializeObject = JsonConvert.SerializeObject(@from);
            Console.WriteLine(s);
            Console.WriteLine(s2);
            Assert.AreEqual(s, s2);
        }

        [Test(Description = "测试换行")]
        public void TestNewLineInvoke()
        {

            #region 测试换行

            Assert.AreEqual(true, Expression.From(@"2.2==(1,3.5)
                                                    && 2.2==[1,10]").Invoke().BooleanResult);
            Assert.AreEqual("abc_def", Expression.From(@"CONCAT('a',
                                                                 'bc',
                                                                '_',
                                                                'def')").Invoke().Data);

            #endregion

        }

        [Test(Description = "测试注释")]
        public void TestCommentInvoke()
        {
            #region 测试注释

            Assert.AreEqual(true, Expression.From(@"2.2==(1,3.5)  # 注释
# 注释
                                                    && 2.2==[1,10]# 注释").Invoke().BooleanResult);
            Assert.AreEqual("abc_def", Expression.From(@"CONCAT('a',# 注释
                                                                 'bc',# 注释
                                                                '_',# 注释
'def')
                        # 注释
").Invoke().Data);

            #endregion
        }

        [Test(Description = "测试数学函数")]
        public void TestMathFunctionInvoke()
        {
            #region 测试数学函数

            Assert.AreEqual(1.234m, Expression.From("ABS(-1.234)").Invoke().NumberResult);
            Assert.AreEqual(1.234m, Expression.From("ABS(1.234)").Invoke().NumberResult);
            Assert.AreEqual(0.5m, Expression.From("ROUNDING(SIN(RAD(30)), 6)").Invoke().NumberResult);
            Assert.AreEqual(30m, Expression.From("ROUNDING(DEG(ASIN(0.5)), 6)").Invoke().NumberResult);
            Assert.AreEqual(0.5m, Expression.From("ROUNDING(COS(RAD(60)), 6)").Invoke().NumberResult);
            Assert.AreEqual(60m, Expression.From("ROUNDING(DEG(ACOS(0.5)), 6)").Invoke().NumberResult);
            Assert.AreEqual(1m, Expression.From("ROUNDING(TAN(RAD(45)), 6)").Invoke().NumberResult);
            Console.WriteLine(Expression.From("ATAN2(2,1)").Invoke().NumberResult);
            Console.WriteLine(Expression.From("ATAN(1/12)").Invoke().NumberResult);
            Assert.AreEqual(true, Expression.From("ATAN(1/2)==ATAN2(2,1)").Invoke().BooleanResult);
            Assert.AreEqual(1.175201m, Expression.From("ROUNDING(SINH(1), 6)").Invoke().NumberResult);
            Assert.AreEqual(3.762196m, Expression.From("ROUNDING(COSH(2), 6)").Invoke().NumberResult);
            Assert.AreEqual(0.964028m, Expression.From("ROUNDING(TANH(2), 6)").Invoke().NumberResult);
            Assert.AreEqual(0.034907m, Expression.From("ROUNDING(RAD(2), 6)").Invoke().NumberResult);
            Assert.AreEqual(114.591559m, Expression.From("ROUNDING(DEG(2), 6)").Invoke().NumberResult);
            Assert.AreEqual(0.693147m, Expression.From("ROUNDING(LOG(2), 6)").Invoke().NumberResult);
            Assert.AreEqual(0.430677m, Expression.From("ROUNDING(LOG(2,5), 6)").Invoke().NumberResult);
            Assert.AreEqual(0.301030m, Expression.From("ROUNDING(LOG10(2), 6)").Invoke().NumberResult);
            Assert.AreEqual(7.389056m, Expression.From("ROUNDING(EXP(2), 6)").Invoke().NumberResult);
            Assert.AreEqual(120m, Expression.From("ROUNDING(FACT(5), 6)").Invoke().NumberResult);
            Assert.AreEqual(1m, Expression.From("ROUNDING(FACT(0), 6)").Invoke().NumberResult);
            Assert.AreEqual(4.472136m, Expression.From("ROUNDING(SQRT(20), 6)").Invoke().NumberResult);
            Assert.AreEqual(2m, Expression.From("ROUNDING(MOD(20, 3), 6)").Invoke().NumberResult);
            Assert.AreEqual(8000m, Expression.From("ROUNDING(POW(20, 3), 6)").Invoke().NumberResult);
            Assert.AreEqual(3.141593m, Expression.From("ROUNDING(PI(), 6)").Invoke().NumberResult);
            Assert.AreEqual((decimal)Math.PI, Expression.From("PI()").Invoke().NumberResult);

            #endregion
        }

        [Test(Description = "测试公式解析异常")]
        public void TestFormatExceptionInvoke()
        {
            try
            {
                var a = Expression.From("(1+2");
                Assert.IsNotNull(a);
            }
            catch
            {
            }
            try
            {
                var a = Expression.From("(1+2))");
                Assert.IsNotNull(a);
            }
            catch
            {
            }
            try
            {
                var a = Expression.From("MAX((1+2),2,(3+4)");
                Assert.IsNotNull(a);
            }
            catch
            {
            }
            try
            {
                var a = Expression.From("MAX((1+2),2,(3+4)))");
                Assert.IsNotNull(a);
            }
            catch
            {
            }

            _ = Expression.From("(1+2)");
            _ = Expression.From("MAX((1+2),(2),(3+4))");
        }

        [Test(Description = "测试转义")]
        public void TestEscapeInvoke()
        {
            var parameters = InitParameters();

            Assert.AreEqual("{d'd''d}'", Expression.From("{str5}").Invoke(parameters).StringResult);
            Assert.AreEqual("{d'd''d}'", Expression.From("'{d''d''''d}'''").Invoke(parameters).StringResult);
            Assert.AreEqual(true, Expression.From("{str5} == '{d''d''''d}'''").Invoke(parameters).BooleanResult);
            Assert.AreEqual("{d'd''d}'", Expression.From("{{str6}}}").Invoke(parameters).StringResult);
            Assert.AreEqual("{d'd''d}'", Expression.From("'{d''d''''d}'''").Invoke(parameters).StringResult);
            Assert.AreEqual(true, Expression.From("{{str6}}} == '{d''d''''d}'''").Invoke(parameters).BooleanResult);

        }

        [Test(Description = "测试Metadata")]
        public void TestMetadataInvoke()
        {
            var parameters = InitParameters();

            Assert.AreEqual(true, Expression.From("META({metadata}, 'meta01') == 'value01'").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("META({metadata}, 'meta02') == 'True'").Invoke(parameters).BooleanResult);
            Assert.AreEqual(false, Expression.From("META({metadata}, 'meta02') == true").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("IF(META({metadata}, 'meta02'), true, false)").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("META({metadata}, 'meta03') == '123'").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("META({metadata}, 'meta02', 'bool') == true").Invoke(parameters).BooleanResult);
            Assert.AreEqual(true, Expression.From("META({metadata}, 'meta03', 'num') == 123").Invoke(parameters).BooleanResult);

            Assert.AreEqual(true, Expression.From("DATAMETA({metadata}, 'name') == 'metadataName'").Invoke(parameters).BooleanResult);
            Assert.AreEqual("", Expression.From("DATAMETA({metadata}, 'name2')").Invoke(parameters).StringResult);
            Assert.AreEqual("", Expression.From("DATAMETA({metadata2}, 'name2')").Invoke(parameters).StringResult);
        }

        [Test(Description = "测试除0")]
        public void TestDivideZeroInvoke()
        {
            Assert.AreEqual(0, Expression.From("0/0").Invoke().NumberResult);
            Assert.AreEqual(0, Expression.From("12/0").Invoke().NumberResult);
        }

        [Test(Description = "测试数组")]
        public void TestArrayInvoke()
        {
            var parameters = InitParameters();

            Assert.AreEqual(15, Expression.From("ASUM({numlist1})").Invoke(parameters).NumberResult);
            Assert.AreEqual(2, Expression.From("AINDEX({numlist1}, 2)").Invoke(parameters).NumberResult);
            Assert.AreEqual(4, Expression.From("AMATCH({numlist1}, 4)").Invoke(parameters).NumberResult);
            Assert.AreEqual(-1, Expression.From("AMATCH({numlist1}, 6)").Invoke(parameters).NumberResult);
            Assert.AreEqual(6, Expression.From("ACOUNT({numlist1})").Invoke(parameters).NumberResult);

            Assert.AreEqual("bbb", Expression.From("AINDEX({strlist1}, 1)").Invoke(parameters).StringResult);
            Assert.AreEqual(1, Expression.From("AMATCH({strlist1}, 'bbb')").Invoke(parameters).NumberResult);
            Assert.AreEqual(-1, Expression.From("AMATCH({strlist1}, 'b')").Invoke(parameters).NumberResult);
            Assert.AreEqual(3, Expression.From("ACOUNT({strlist1})").Invoke(parameters).NumberResult);

            Assert.AreEqual(70, Expression.From("ASUM({numlist3})").Invoke(parameters).NumberResult);
            Assert.AreEqual(70, Expression.From("{numlist3}").Invoke(parameters).NumberResult);
            Assert.AreEqual(71, Expression.From("1+{numlist3}").Invoke(parameters).NumberResult);
            Assert.AreEqual(true, Expression.From("1<{numlist3}").Invoke(parameters).BooleanResult);
            Assert.AreEqual(false, Expression.From("1>{numlist3}").Invoke(parameters).BooleanResult);
            Assert.AreEqual(2, Expression.From("AINDEX({numlist3}, 2)").Invoke(parameters).NumberResult);
            Assert.AreEqual(-1, Expression.From("AMATCH({numlist3}, 4)").Invoke(parameters).NumberResult);
            Assert.AreEqual(-1, Expression.From("AMATCH({numlist3}, 6)").Invoke(parameters).NumberResult);
            Assert.AreEqual(14, Expression.From("ACOUNT({numlist3})").Invoke(parameters).NumberResult);
            Assert.AreEqual(5, Expression.From("AMAX({numlist1})").Invoke(parameters).NumberResult);
            Assert.AreEqual(0, Expression.From("AMIN({numlist1})").Invoke(parameters).NumberResult);
            Assert.AreEqual(11, Expression.From("AMAXDIFF({numlist2})").Invoke(parameters).NumberResult);
            Assert.AreEqual(0, Expression.From("AMINDIFF({numlist2})").Invoke(parameters).NumberResult);
        }
    }
}
