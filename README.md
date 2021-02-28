# HLC.Expression

## 1. 功能说明

- [x] 计算字符串数学表达式的值
- [x] 计算字符串逻辑表达式的值
- [x] 将字符串表达式解析成表达式树
- [x] 对表达式树进行计算
- [x] 字符串表达式中支持变量



## 2. 用途

- 自己想写个计算器但是不会做字符串解析（使用，研究，`Ctrl-CV`大法皆可）

- 项目中需要做数学运算来动态获取值
- 项目中需要自定义配置逻辑表达式来进行条件约束



## 3. 使用说明

### 3.1 安装

1. `NuGet`中搜索`HLCExpression`进行安装使用（`HLC.Expression`不让上传`Nuget`）
2. 下载源代码编译引入

### 3.2 支持的二元运算符

运算符前后空格会被忽略，但运算符不能被空格拆分。

| 符号 | 说明     | 优先级 | 约束                                   | 示例            |
| ---- | -------- | ------ | -------------------------------------- | --------------- |
| ^    | 指数     | 100    | 左右只能是数字或表达式结果为数字       | 5^2             |
| %    | 取模     | 50     | 左右只能是整数或表达式结果为整数       | 5%2             |
| *    | 乘法     | 50     | 左右只能是数字或表达式结果为数字       | 5*2             |
| /    | 除法     | 50     | 左右只能是数字或表达式结果为数字       | 5/2             |
| +    | 加法     | 40     | 左右只能是数字或表达式结果为数字       | 5+2             |
| -    | 减法     | 40     | 左右只能是数字或表达式结果为数字       | 5-2             |
| >    | 大于     | 30     | 左右只能是数字或表达式结果为数字       | 5>2             |
| >=   | 大于等于 | 30     | 左右只能是数字或表达式结果为数字       | 5>=2            |
| <    | 小于     | 30     | 左右只能是数字或表达式结果为数字       | 5<2             |
| <=   | 小于等于 | 30     | 左右只能是数字或表达式结果为数字       | 5<=2            |
| ==   | 等于     | 30     | 左右只能是数字或表达式结果为数字       | 5==2            |
| !=   | 不等于   | 30     | 左右只能是数字或表达式结果为数字       | 5!=2            |
| &&   | 与 运算  | 20     | 左右只能是Boolean或表达式结果为Boolean | true && false   |
| \|\| | 或 运算  | 10     | 左右只能是Boolean或表达式结果为Boolean | true \|\| false |
|      |          |        |                                        |                 |

### 3.3 支持的函数运算

函数名与后面的左括号之间不能有空格，参数前后可以有空格。

| 函数          | 说明                                           | 约束                                          | 示例                                    |
| ------------- | ---------------------------------------------- | --------------------------------------------- | --------------------------------------- |
| CEILING( )    | 向上取整                                       | 参数为数字                                    | CEILING(1+2)                            |
| FLOORING()    | 向下取整                                       | 参数为数字                                    | FLOORING(1+2)                           |
| ROUNDING()    | 四舍五入取整                                   | 参数为数字                                    | ROUNDING(1+2)                           |
| MAX( , , ...) | 最大值                                         | 参数为数字                                    | MAX(1,2,3,1+3)                          |
| MIN( , , ...) | 最小值                                         | 参数为数字                                    | MIN(1,2,3,1+3)                          |
| IN( ,  , ...) | 第一个参数是否在后面的参数中存在               | 参数为数字或字符串                            | IN(3,1,2,3,1+3) IN('AA','A','AA','AAA') |
| IF( , , )     | 第一个参数结果为true则取第二个参数，否则第三个 | 第一个参数为boolean值，后面两个根据上下文决定 | IF(true, 123, 'ABC')                    |
| NOT( )        | 取参数的非值                                   | 参数为boolean值                               | NOT(1>2)                                |
| AND( , , ...) | 对所有参数进行逻辑与运算                       | 参数为boolean值                               | AND(1==2,1>2,1<=2)                      |
| OR( , , ...)  | 对所有参数进行逻辑非运算                       | 参数为boolean值                               | OR(1==2,1>2,1<=2)                       |
|               |                                                |                                               |                                         |

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

### 4. 表达式示例

```
12.3+4.5*6/7-8^9+10%3
```

```
12.3+4.5*6/(7-8)^9+(10%3)
```

```
12.3+4.5*6/7-8^9+10%3+MIN(1,2,3)-MAX(1,2,3)
```

```
12.3+4.5*6/7-8^9+10%3+MIN(1,2,3)-MAX(1,2,3)+CEILING(1+2.3)
```

```
12.3+4.5*6/7-8^9+10%3+MIN(1,2,3)-MAX(1,2,3)+FLOORING(1+2.3)
```

```
12.3+4.5*6/7-8^9+10%3+MIN(1,2,3)-MAX(1,2,3)+ROUNDING(1+2.3)
```

```
true && IN(3,1,2,3,1+2)
```

```
true && IN('AA','A','AA','AAA')
```

```
true && IF(1>2,true,false)
```

```
true || IF(1<2,true,false)
```

```
true || NOT(IF(1<2,true,false))
```

```
NOT(true || NOT(IF(1<2,true,false)))
```

```
ADD(1<2,1<3,1<4)
```

```
OR(1<2,1<3,1<4)
```

```
IF((1+{aaa})>2,{bbb},{ccc})
```

```
IF(IN({aaa},'AAA','BBB','CCC') && IN({bbb},'AAA','BBB','CCC'), {bbb}, {ccc})
```

```
.......
```

**具体使用方式见单元测试**

