# DataRangeListRelateValider
Check whether the set relationship of data interval objects has interval error, interval crossing, interval not set, etc(校验范围区间对象的区间集合关系，包括：各区间内部，是否存在区间交叉，未设置导致空白的区间)
一般的使用场景：：一些不同积分区间需要设置不同待遇政策，不同价格区间设置不同折扣政策这些场景的时候，通常要校验各个区间是否有交叉，空白未设置，超出范围等。
【用法示例1】
 DataRangeListRelateValider drv = new DataRangeListRelateValider();
            List<OpenRangeData> listRange = new List<OpenRangeData>();
            listRange.Add(new OpenRangeData() { Min = 0, Max = 5 });//区间1
            listRange.Add(new OpenRangeData() { Min = 4, Max = 8 });//区间2
            listRange.Add(new OpenRangeData() { Min = 9, Max = 25 });//区间3
            RangeValideResult  result_rd= drv.ValideRangeDataRelate(listRange,new OpenRangeData() { Min=0,Max=25 });//第二个参数是本次校验需考虑的总区间(要求所有区间属于该区间内).
 返回结果：{"isSuccess":false,"listErrRangeData":[{"Min":0,"Max":5},{"Min":4,"Max":8}],"errMsg":"出现区间之间交叉的错误"}
 
 【用法示例2】
             DataRangeListRelateValider drv = new DataRangeListRelateValider();
            List<OpenRangeData> listRange = new List<OpenRangeData>();
            listRange.Add(new OpenRangeData() { Min = 0, Max = 5 });//区间1
            listRange.Add(new OpenRangeData() { Min = 6, Max = 8 });//区间2
            listRange.Add(new OpenRangeData() { Min = 12, Max = 25 });//区间3
            RangeValideResult  result_rd= drv.ValideRangeDataRelate(listRange,new OpenRangeData() { Min=0,Max=25 });//第二个参数是本次校验需考虑的总区间(要求所有区间属于该区间内).
 返回结果：{"isSuccess":false,"listErrRangeData":[{"Min":8,"Max":12}],"errMsg":"出现没有被覆盖的空白区间"}
 【用法示例3】
             DataRangeListRelateValider drv = new DataRangeListRelateValider();
            List<OpenRangeData> listRange = new List<OpenRangeData>();
            listRange.Add(new OpenRangeData() { Min = 0, Max = 5 });//区间1
            listRange.Add(new OpenRangeData() { Min = 8, Max = 5 });//区间2
            listRange.Add(new OpenRangeData() { Min = 12, Max = 25 });//区间3
            RangeValideResult  result_rd= drv.ValideRangeDataRelate(listRange,new OpenRangeData() { Min=0,Max=25 });//第二个参数是本次校验需考虑的总区间(要求所有区间属于该区间内).
 返回结果：{"isSuccess":false,"listErrRangeData":[{"Min":6,"Max":5}],"errMsg":"设置的区间中起始值和结束值关系错误"}
  
