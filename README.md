# HLC.Expression 字符串表达式解析库
[![NuGet Badge](https://buildstats.info/nuget/HLCExpression)](https://www.nuget.org/packages/HLCExpression)
![GitHub](https://img.shields.io/github/license/houlongchao/HLC.Expression?style=social)

一款支持**数学运算**，**逻辑运算**，**字符串运算**，常用**数学函数运算**以及其它常用**自定义函数**运算的工具库。

**高度抽象，快速扩展自定义函数**

## 1. 功能说明

- [x] 对字符串表达式进行解析计算



- [x] 支持常用数学计算
- [x] 支持常用逻辑运算
- [x] 支持常用字符串运算
- [x] 支持带入参数计算
- [x] 支持参数负载属性计算
- [x] 支持牛顿法求解一元函数
- [x] 高度抽象，支持快速扩展自定义函数



## 2. 用途

- 自己想写个计算器但是不会做字符串解析（使用，研究，`Ctrl-CV`大法皆可）
- 项目中需要做数学运算来动态获取值
- 项目中需要自定义配置逻辑表达式来进行条件约束



## 3. 使用说明

### 3.1 安装使用

#### 3.1.1 NuGet安装

`NuGet`中搜索`HLCExpression`进行安装使用

``` shell
dotnet add package HLCExpression
```



#### 3.1.2 不含参数计算

``` C#
// 计算返回数值结果
Expression.From("CEILING(50.12+1)").Invoke().NumberResult
// 计算返回Boolean结果
Expression.From("2.1<=1.1").Invoke().BooleanResult
// 计算返回String结果
Expression.From("TOSTR(DATETIME('2021-01-01'), 'yyyy-MM-dd')").Invoke().StringResult 
// 更多示例见代码单元测试
```

#### 3.1.3 含参数计算

``` C#
// 构建参数字典
var parameters = new Parameters()
{
    {"num", 123456},
    {"numlist1", new List<decimal>(){1,2,3,4,5}},
    {"str", "abcde"},
    {"strnum", "123.3"},
    {"empty", ""},
    {"true", true },
    {"false", false },
    {"strlist1", new List<string>(){"aaa","bbb","ccc"}},
    {"range", new Parameter("(1,5)", ParameterType.Range)},
    {"dt", new Parameter(DateTime.Parse("2021-01-01"))},
};
// 添加属性测试参数
parameters["metadata"] = new Parameter("metadataValue");
parameters["metadata"].Metadata["meta01"] = "value01";
parameters["metadata"].Metadata["meta02"] = "True";
parameters["metadata"].Metadata["meta03"] = "123";

// 获取指定参数的指定属性值
Expression.From("META({metadata}, 'meta01') == 'value01'").Invoke(parameters).BooleanResult
Expression.From("META({metadata}, 'meta03', 'num') == 123").Invoke(parameters).BooleanResult

// 计算表达式
var aaa = Expression.From("IN({str4},{strlist1}) && IN({str1},{strlist1}) || (1+2*3-4)==MAX(1+2,2)");
aaa.Invoke(parameters);

// 计算并返回boolean值
Expression.From("{strnum}<134").Invoke(parameters).BooleanResult
```

#### 3.1.4 表达式设置

``` C#
// 继承ExpressionSetting
// 重写里面的属性和方法
public class MySetting : ExpressionSetting
{
    public override bool HasStringStartChar => false;
    public override bool HasStringEndChar => false;
    public override bool HasVariableEndChar => false;
    public override char VariableStartChar => '$';
}

// 应用设置
ExpressionSetting.SetSetting(new MySetting());
```



#### 3.1.5 牛顿法求一元函数

``` C#
// 求参数num2的值
Expression.From("{num1}+{num2}+{num1}*{num2}-{num1}*4.5+{num2}*0.5").NewtonSolve("num2", parameters);
```

### 3.2 支持的二元运算符

运算符前后空格会被忽略，但运算符不能被空格拆分。

| 符号   | 说明   | 优先级  | 约束                         | 示例              |
| ---- | ---- | ---- | -------------------------- | --------------- |
| ^    | 指数   | 100  | 左右只能是数字或表达式结果为数字           | 5^2             |
| %    | 取模   | 50   | 左右只能是整数或表达式结果为整数           | 5%2             |
| *    | 乘法   | 50   | 左右只能是数字或表达式结果为数字           | 5*2             |
| /    | 除法   | 50   | 左右只能是数字或表达式结果为数字           | 5/2             |
| +    | 加法   | 40   | 左右只能是数字或表达式结果为数字           | 5+2             |
| -    | 减法   | 40   | 左右只能是数字或表达式结果为数字           | 5-2             |
| >    | 大于   | 30   | 左右只能是数字或表达式结果为数字           | 5>2             |
| >=   | 大于等于 | 30   | 左右只能是数字或表达式结果为数字           | 5>=2            |
| <    | 小于   | 30   | 左右只能是数字或表达式结果为数字           | 5<2             |
| <=   | 小于等于 | 30   | 左右只能是数字或表达式结果为数字           | 5<=2            |
| ==   | 等于   | 30   | 左右只能是数字或表达式结果为数字           | 5==2            |
| !=   | 不等于  | 30   | 左右只能是数字或表达式结果为数字           | 5!=2            |
| &&   | 与 运算 | 20   | 左右只能是Boolean或表达式结果为Boolean | true && false   |
| \|\| | 或 运算 | 10   | 左右只能是Boolean或表达式结果为Boolean | true \|\| false |
|      |      |      |                            |                 |

### 3.3 支持的函数运算

函数名与后面的左括号之间不能有空格，参数前后可以有空格。

#### 3.3.1 条件判断

| 函数          | 说明     | 简单示例                                     | 返回类型 |
| ----------- | ------ | ---------------------------------------- | ---- |
| `IF()`      | 逻辑判断   | `IF(1>2, true, 1)`    `IF(1 > 2, 2 > 4, false)` | 任意值  |
| `SWITCH()`  | 精准开关匹配 | `SWITCH({A}, 1:'str', 2:'2')`    `SWITCH({A}, 'A':1, 'B':2)` | 任意值  |
| `SWITCHC()` | 模糊开关匹配 | `SWITCHC({A}, '1':'str', '2':'2')`      `SWITCHC({A}, 'A':1, 'B':2)` | 任意值  |
|             |        |                                          |      |

#####  IF 逻辑判断

- 必须三个参数，第一个为要判断的逻辑值，在参数一为ture时返回第二个参数结果，否则返回第三个参数结果

> `IF(1>2,11,22)` ==> `22`
>
> `IF(1<2,11,22)` ==> `11`

##### SWITCH 精准开关匹配

- 第一个参数与后面参数匹配值(:前)相同时返回对应的结果值(:后)

> `SWITCH(1, 1:'A', 2:22, 3:'C')`  ==> `A`
>
> `SWITCH(2, 1:'A', 2:22, 3:'C')`  ==> `22`

##### SWITCHC 模糊开关匹配

- 第一个参数包含后面参数匹配值(:前)相同时返回对应的结果值(:后)

> `SWITCHC(123, 1:'A', 2:22, 3:'C')`  ==> `A`
>
> `SWITCHC(222, 1:'A', 2:22, 3:'C')`  ==> `22`



#### 3.3.2. 数值函数

| 函数           | 说明   | 简单示例                                     | 返回类型 |
| ------------ | ---- | ---------------------------------------- | ---- |
| `CEILING()`  | 向上取整 | `CEILING(1.1)`    `CEILING(1.1+2.2)`     | 数值   |
| `FLOORING()` | 向下取整 | `FLOORING(1.1)`   `FLOORING(1.1+2.2)`    | 数值   |
| `ROUNDING`   | 四舍五入 | `ROUNDING(1.1)`   `ROUNDING(1.1+2.2, 1)` | 数值   |
| `MAX()`      | 取最大值 | `MAX(1.1, 2, 3)`   `MAX(1.1+2.2, 3, 4)`  | 数值   |
| `MIN()`      | 取最小值 | `MIN(1.1, 2, 3)`   `MIN(1.1+2.2, 3, 4)`  | 数值   |
|              |      |                                          |      |

##### CEILING 向上取整

- 一个参数

> `CEILING(1.1)`  ==> `2`
>
> `CEILING(1.1+2.2)` ==> `4`

##### FLOORING 向下取整

- 一个参数

> `FLOORING(1.1)`  ==> `1`
>
> `FLOORING(1.1+2.2)` ==> `3`

##### ROUNDING 四舍五入

- 第二个参数可选，表示要四舍五入的小数位数

> `ROUNDING(1.4)`  ==> `1`
>
> `ROUNDING(1.5)`  ==> `2`
>
> `ROUNDING(1.44, 1)`  ==> `1.4`
>
> `ROUNDING(1.45, 1)`  ==> `1.5`

##### MAX 取最大值

- 多个参数，参数之间用半角逗号分隔

> `MAX(1.1, 2, 3)`  ==> `3`
>
> `MAX(3.3+2.2, 3, 4)` ==> `5.5`

##### MIN 取最小值

- 多个参数，参数之间用半角逗号分隔

> `MAX(1.1, 2, 3)`  ==> `3`
>
> `MAX(3.3+2.2, 3, 4)` ==> `5.5`



#### 3.3.3. 文本函数

| 函数          | 说明         | 简单示例                                     | 返回类型 |
| ----------- | ---------- | ---------------------------------------- | ---- |
| `CONCAT()`  | 字符串拼接      | `CONCAT({A}, 'str')`   `CONCAT({A}, 1, 'str')` | 字符串  |
| `SUBSTR()`  | 取子串        | `SUBSTR({A}, 1, 2)`   `SWITCH({A}, 0, 2)` | 字符串  |
| `SUBNUM()`  | 取字串转数值     | `SUBNUM({A}, 1, 2)`   `SUBNUM({A}, 0, 2)` | 数值   |
| `LEFT()`    | 从左侧取指定长度子串 | `LEFT({OPT:A}, 2)`  `LEFT('123456', 3)`  | 字符串  |
| `RIGHT()`   | 从右侧取指定长度子串 | `RIGHT({OPT:A}, 2)`  `RIGHT('123456', 3)` | 字符串  |
| `REVERSE()` | 反转字符串      | `REVERSE({OPT:A})`  `REVERSE('123456')`  | 字符串  |
| `FIND()`    | 获取下标       | `FIND({OPT:A}, '2')`  `FIND('123456', '23')` | 整数   |
| `LENGTH()`  | 获取字符串长度    | `LENGTH({OPT:A})`  `LENGTH('123456')`    | 整数   |
|             |            |                                          |      |

##### CONCAT 字符串拼接

- 对所有参数进行字符串方式拼接

> `CONCAT(1,'23', 4)` ==> `1234`
>
> `CONCAT('A','BC', 4)` ==> `ABC4`

##### SUBSTR 取子串

- 第二个参数为开始下标（下标从0开始），第三个参数为取串长度（不传时取到尾）

> `SUBSTR('ABCDE', 0)` ==> `ABCDE`
>
> `SUBSTR('ABCDE', 0, 3)` ==> `ABC`
>
> `SUBSTR('ABCDE', 2)` ==> `CDE`
>
> `SUBSTR('ABCDE', 2, 2)` ==> `CD`

##### SUBNUM 取字串转数值

- 第二个参数为开始下标（下标从0开始），第三个参数为取串长度（不传时取到尾）

> `SUBNUM('ABC01230321', 3)` ==> `1230321`
>
> `SUBNUM('ABC01230321', 3, 3)` ==> `12`
>
> `SUBNUM('ABC01230321', 7, 1)` ==> `0`
>
> `SUBNUM('ABC01230321', 7, 2)` ==> `3`



#### 3.3.4. 逻辑函数

| 函数           | 说明          | 简单示例                                     | 返回类型 |
| ------------ | ----------- | ---------------------------------------- | ---- |
| `NOT()`      | 逻辑取反        | `NOT(true)`   `NOT(true && false)`   `NOT(1 > 2)` | 逻辑值  |
| `AND()`      | 逻辑与运算       | `AND(true, true, false)`    `AND(1 > 2, 2 > 4, false)` | 逻辑值  |
| `OR()`       | 逻辑或运算       | `OR(true, true, false)`   `OR(1 > 2, 2 > 4, false)` | 逻辑值  |
| `IN()`       | 包含判断        | `IN({A}, 1, 2)`   `IN({A}, 1, 2+3)`      | 逻辑值  |
| `HASVALUE()` | 是否有数据       | `HASVALUE({A})`   `HASVALUE({B})`        | 逻辑值  |
| `ISNUMBER()` | 是否是数值       | `ISNUMBER({A})`   `ISNUMBER({A}+{B})`    | 逻辑值  |
| `ISSTART()`  | 是否以指定字符串开始  | `ISSTART({OPT:A}, 'a')`  `ISSTART({OPT:A}, '123')` | 逻辑值  |
| `ISEND()`    | 是否以指定字符串结尾  | `ISEND({OPT:A}, 'a')`  `ISEND({OPT:A}, '123')` | 逻辑值  |
| `ISMATCH()`  | 是否可以用指定正则匹配 | `ISMATCH('123456', '^\\d*$')`  `ISMATCH('123456', '^1\\d{5}$')` | 逻辑值  |
|              |             |                                          |      |

##### IN 包含判断

- 判断第一个参数是否在后面的参数中

> `IN(1,5,4,3,2,1)`  ==> `true`
>
> `IN(1,2,3,4,5)`  ==> `false`



#### 3.3.5. 参数函数

| 函数           | 说明        | 简单示例                                     | 返回类型                 |
| ------------ | --------- | ---------------------------------------- | -------------------- |
| `DATETIME()` | 字符串转日期时间  | `DATETIME('2021-02-01 12:00')` `   DATETIME('202102011200', 'yyyyMMddHHmm')` | 日期                   |
| `GET()`      | 获取值或返回默认值 | `GET({A}, 'AAA')`    `GET({NUM},123)`    | 任意值                  |
| `TONUM()`    | 转为数值      | `TONUM({NUM1})`   `TONUM('123')`         | 数值                   |
| `TOSTR()`    | 转字符串      | `TOSTR({DATE})` `TOSTR({DATE}, 'yyyy-MM-dd')` | 字符串                  |
| `META()`     | 获取参数属性    | `META({A}, 'name')` `META({A}, 'age', 'num')` | 字符串或指定类型txt/num/bool |
| `DATAMETA()` | 获取数据值属性   | `DATAMETA({OPT:A}, 'meta03')`  `DATAMETA({OPT:A}, 'meta02', 'bool')` | 字符串或指定类型txt/num/bool |
| `NOW()`      | 获取当前系统时间  | `NOW()` `   NOW('UTC')`                  | 日期                   |

##### DATETIME 转日期

- 第二个参数为日期格式化模式

> 日期格式化字符串示例：`yyyy-MM-dd HH:mm:ss`

##### TOSTR 转日期

- 第二个参数为格式化字符串

> 日期格式化字符串示例：`yyyy-MM-dd HH:mm:ss`

##### NOW 获取当前系统时间

- 无参数时返回当前系统本地时间，可选参数`UTC`返回当前系统UTC时间`



#### 3.3.6. 数学函数

| 函数        | 说明      | 简单示例                              | 返回类型 |
| --------- | ------- | --------------------------------- | ---- |
| `ABS()`   | 绝对值函数   | `ABS(1.1)`    `ABS(1.1+2.2)`      | 数值   |
| `SIN()`   | 正弦函数    | `SIN(1.1)`    `SIN(1.1+2.2)`      | 数值   |
| `ASIN()`  | 反正弦函数   | `ASIN(1.1)`    `ASIN(1.1+2.2)`    | 数值   |
| `COS()`   | 余弦函数    | `COS(1.1)`    `COS(1.1+2.2)`      | 数值   |
| `ACOS()`  | 反余弦函数   | `ACOS(1.1)`    `ACOS(1.1+2.2)`    | 数值   |
| `TAN()`   | 正切函数    | `TAN(1.1)`    `TAN(1.1+2.2)`      | 数值   |
| `ATAN()`  | 反正切函数   | `ATAN(1.1)`    `ATAN(1.1+2.2)`    | 数值   |
| `ATAN2()` | 反余切函数   | `ATAN2(1,2)`    `ATAN2(1+1, 2+2)` | 数值   |
| `SINH()`  | 双曲正弦函数  | `SINH(1.1)`    `SINH(1.1+2.2)`    | 数值   |
| `COSH()`  | 双曲余弦函数  | `COSH(1.1)`    `COSH(1.1+2.2)`    | 数值   |
| `TANH()`  | 双曲正切函数  | `TANH(1.1)`    `TANH(1.1+2.2)`    | 数值   |
| `RAD()`   | 角度转弧度   | `RAD(1.1)`    `RAD(1.1+2.2)`      | 数值   |
| `DEG()`   | 弧度转角度   | `DEG(1.1)`    `DEG(1.1+2.2)`      | 数值   |
| `LOG()`   | 自然对数    | `LOG(2)`    `LOG(2, 5)`           | 数值   |
| `LOG10()` | 以10为底对数 | `LOG10(2)`    `LOG10(2+5)`        | 数值   |
| `EXP()`   | e的指数次幂  | `EXP(1.1)`    `EXP(1.1+2.2)`      | 数值   |
| `FACT()`  | 阶乘      | `FACT(5)`    `FACT(2+3)`          | 数值   |
| `SQRT()`  | 平方根     | `SQRT(20)`    `SQRT(1.1+2.2)`     | 数值   |
| `MOD()`   | 取余数     | `MOD(20, 3)`    `MOD(10+20, 1+2)` | 数值   |
| `POW()`   | 指数      | `POW(20, 3)`    `POW(10+20, 1+2)` | 数值   |
| `PI()`    | 圆周率PI   | `PI()`                            | 数值   |
|           |         |                                   |      |

#### 3.3.7 数组函数

| 函数           | 说明       | 简单示例                   | 返回类型 |
| ------------ | -------- | ---------------------- | ---- |
| `ASUM()`     | 数组求和     | `ASUM({Array})`        | 数值   |
| `ACOUNT()`   | 数组长度     | `ACOUNT({Array})`      | 整数   |
| `AINDEX()`   | 获取指定下标数据 | `AINDEX({Array}, 1)`   | 任意值  |
| `AMATCH()`   | 下标搜索     | `AMATCH({Array}, 100)` | 整数   |
| `AMAX()`     | 数组最大值    | `AMAX({Array})`        | 数值   |
| `AMIN()`     | 数组最小值    | `AMIN({Array})`        | 数值   |
| `AMAXDIFF()` | 数组最大相邻差  | `AMAXDIFF({Array})`    | 数值   |
| `AMINDIFF()` | 数组最小相邻差  | `AMINDIFF({Array})`    | 数值   |
|              |          |                        |      |


### 3.4 约束说明

#### 字符串

- 字符串前后使用单引号，如：'ABC'
- 字符串可以使用大小写字母/数字/中文/小数点/下划线，如: 'ABC_123.45中文'
- 其他特殊符号不允许使用，否则解析出错

#### Boolean值

- 公式中直接输入小写：true / false

#### 变量

- 变量前后使用大括号括起来，如：{key}
- 变量可以使用大小写字母/数字/小数点/下划线，如: {ABC_123.45}
- 其他特殊符号不允许使用，否则解析出错

## 4. 表达式示例

```C#
12.3+4.5*6/7-8^9+10%3
12.3+4.5*6/(7-8)^9+(10%3)
12.3+4.5*6/7-8^9+10%3+MIN(1,2,3)-MAX(1,2,3)
12.3+4.5*6/7-8^9+10%3+MIN(1,2,3)-MAX(1,2,3)+CEILING(1+2.3)
12.3+4.5*6/7-8^9+10%3+MIN(1,2,3)-MAX(1,2,3)+FLOORING(1+2.3)
12.3+4.5*6/7-8^9+10%3+MIN(1,2,3)-MAX(1,2,3)+ROUNDING(1+2.3)
true && IN(3,1,2,3,1+2)
true && IN('AA','A','AA','AAA')
true && IF(1>2,true,false)
true || IF(1<2,true,false)
true || NOT(IF(1<2,true,false))
NOT(true || NOT(IF(1<2,true,false)))
ADD(1<2,1<3,1<4)
OR(1<2,1<3,1<4)
IF((1+{aaa})>2,{bbb},{ccc})
IF(IN({aaa},'AAA','BBB','CCC') && IN({bbb},'AAA','BBB','CCC'), {bbb}, {ccc})
ROUNDING(SIN(RAD(30)), 6)
ROUNDING(DEG(ASIN(0.5)), 6)
ROUNDING(COS(RAD(60)), 6)
ROUNDING(DEG(ACOS(0.5)), 6)
ROUNDING(TAN(RAD(45)), 6)
ATAN(1/2)==ATAN2(2,1)
ROUNDING(SINH(1), 6)
ROUNDING(COSH(2), 6)
ROUNDING(TANH(2), 6)
ROUNDING(RAD(2), 6)
ROUNDING(DEG(2), 6)
ROUNDING(LOG(2), 6)
ROUNDING(LOG(2,5), 6)
//更多请看代码单元测试
```



## 5. 自定义扩展

### 5.1 扩展二元运算

``` C#
// 在Segment目录中复制一个BinarySegment*
// 修改Segment的命名以BinarySegment开头，以自定义扩展名结尾
public class BinarySegmentAdd : BinarySegment     // 必须继承BinarySegment基类
{
    public override string MatchString => "+";    // 表达式中要匹配的二元运算符
    public override Operator MatchOperator => Operator.Add;    // 表达式匹配时的枚举标识，在Operator枚举中新建
    public override ExpressionType ExpressionType => ExpressionType.Add;   // 构建的表达式枚举标识，在ExpressionType枚举中新建

    // 二元运算执行计算过程
    // left 二元运算符左侧的表达式
    // right 二元运算符右侧的表达式
    // parameters 计算时带入的变量参数值
    public override ResultExpression Invoke(Expression left, Expression right, Parameters parameters)  
    {
        ResultExpression leftResult = left.Invoke(parameters);
        ResultExpression rightResult = right.Invoke(parameters);
        try
        {
            var result = leftResult.NumberResult + rightResult.NumberResult;
            return Expression.Result(result);
        }
        catch
        {
            string result = leftResult.StringResult + rightResult.StringResult;
            return Expression.Result(result);
        }
    }
  
    // 二元运算描述说明信息
    public override ExpressionSymbolDefinitionItem GetDefinistion()        
    {
        return new ExpressionSymbolDefinitionItem("+", "加法") { Demo = "5+2", Details = "左右只能是数字或表达式结果为数字" };
    }
}
```

### 5.2 扩展函数运算

``` C#
// 在Segment目录中复制一个FunctionSegment*
// 修改Segment的命名以FunctionSegment开头，以自定义扩展名结尾
public class FunctionSegmentNOW : FunctionSegment   			// 必须继承FunctionSegment基类
{
    public override string MatchString { get; } = "NOW(";		// 表达式中要匹配的函数名，带左括号
    public override Operator MatchOperator { get; } = Operator.NOW;         // 表达式匹配时的枚举标识，在Operator枚举中新建
    public override ExpressionType ExpressionType => ExpressionType.NOW;    // 构建的表达式枚举标识，在ExpressionType枚举中新建

    // 函数运算执行计算过程
    // children 函数括号中的表达式
    // parameters 计算时带入的变量参数值
    public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)   
    {
        if (children.Count == 0)
        {
            return Expression.Result(DateTime.Now);
        }
        else
        {
            var format = children[0].Invoke(parameters).StringResult;
            if ("UTC".Equals(format, StringComparison.CurrentCultureIgnoreCase))
            {
                return Expression.Result(DateTime.UtcNow);
            }
            else
            {
                return Expression.Result(DateTime.Now);
            }
        }
    }

    // 函数运算描述说明信息
    public override ExpressionFunctionDefinitionItem GetDefinistion()
    {
        return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Value, "NOW()", "获取当前系统时间")
        {
            Demo = "NOW()   NOW('UTC')",
            Input = "无参数时返回本地时间，参数为UTC时返回UTC时间",
            Output = "当前系统时间，日期类型",
        };
    }
}
```



