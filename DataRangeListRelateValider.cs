using ConfigLab.Comp.MetaData.MetaEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigLab.Comp.DataType.SceneData
{
    /// <summary>
    /// summary：针对(正整数,认为相邻值相差大于1则有间隔)的区间数据集合的关系校验
    /// remark：一些不同积分区间的不同待遇政策，不同价格区间的折扣政策这些设置的时候，通常要校验各个区间是否有交叉，空白未设置，超出范围等。
    /// create date：2020-4-11
    /// author：pcw 
    /// blogs:  http://cnblogs.com/taohuadaozhu
    /// github地址:https://github.com/configlab/DataRangeListRelateValider
    /// </summary>
    public class DataRangeListRelateValider
    {
        /// <summary>
        /// 校验并返回错误的区间集合（如果没有错误则返回null)
        /// </summary>
        /// <returns></returns>
        public RangeValideResult ValideRangeDataRelate(List<OpenRangeData> listSettingRangeData, OpenRangeData totalRange)
        {
            if (totalRange == null)
            {
                return new RangeValideResult()
                {
                    isSuccess = false,
                    errMsg = "总区间为null"
                };
            }
            if (listSettingRangeData == null || listSettingRangeData.Count == 0)
            {
                return new RangeValideResult()
                {
                    isSuccess = false,
                    errMsg = "未设置任何区间",
                    listErrRangeData = new List<OpenRangeData>() { totalRange }
                };
            }
            if (totalRange.Max < totalRange.Min)
            {
                return new RangeValideResult()
                {
                    isSuccess = false,
                    errMsg = "总区间的起始值和结束值关系错误",
                    listErrRangeData = new List<OpenRangeData>() { totalRange }
                };
            }
            if (listSettingRangeData.Count == 1)
            {
                if (listSettingRangeData[0].Min <= totalRange.Min &&
                    listSettingRangeData[0].Max >= totalRange.Max)
                {
                    return new RangeValideResult()
                    {
                        isSuccess = true,
                        errMsg = ""
                    };
                }
                return new RangeValideResult()
                {
                    isSuccess = false,
                    errMsg = "区间内部分区域没有定义",
                    listErrRangeData = new List<OpenRangeData>() { totalRange }
                };
            }
            //检查各个区间的内部关系
            for (int i = 0; i < listSettingRangeData.Count; i++)
            {
                if (listSettingRangeData[i].Max < listSettingRangeData[i].Min)
                {
                    return new RangeValideResult()
                    {
                        isSuccess = false,
                        errMsg = "设置的区间中起始值和结束值关系错误",
                        listErrRangeData = new List<OpenRangeData>() { listSettingRangeData[i] }
                    };
                }
            }
            //检查是否有交叉
            for (int i = 0; i < listSettingRangeData.Count; i++)
            {
                for (int j = i + 1; j < listSettingRangeData.Count; j++)
                {
                    //正
                    if (listSettingRangeData[j].Min >= listSettingRangeData[i].Min
                        && listSettingRangeData[j].Min <= listSettingRangeData[i].Max)
                    {
                        return new RangeValideResult()
                        {
                            isSuccess = false,
                            errMsg = "出现区间之间交叉的错误",
                            listErrRangeData = new List<OpenRangeData>() { listSettingRangeData[i], listSettingRangeData[j] }
                        };
                    }
                    if (listSettingRangeData[j].Max >= listSettingRangeData[i].Min
                        && listSettingRangeData[j].Max <= listSettingRangeData[i].Max)
                    {
                        return new RangeValideResult()
                        {
                            isSuccess = false,
                            errMsg = "出现区间之间交叉的错误",
                            listErrRangeData = new List<OpenRangeData>() { listSettingRangeData[i], listSettingRangeData[j] }
                        };
                    }
                    //反
                    if (listSettingRangeData[i].Min >= listSettingRangeData[j].Min
                        && listSettingRangeData[i].Min <= listSettingRangeData[j].Max)
                    {
                        return new RangeValideResult()
                        {
                            isSuccess = false,
                            errMsg = "出现区间之间交叉的错误",
                            listErrRangeData = new List<OpenRangeData>() { listSettingRangeData[i], listSettingRangeData[j] }
                        };
                    }
                    if (listSettingRangeData[i].Max >= listSettingRangeData[j].Min
                        && listSettingRangeData[i].Max <= listSettingRangeData[j].Max)
                    {
                        return new RangeValideResult()
                        {
                            isSuccess = false,
                            errMsg = "出现区间之间交叉的错误",
                            listErrRangeData = new List<OpenRangeData>() { listSettingRangeData[i], listSettingRangeData[j] }
                        };
                    }
                }
            }
            //检查是否有无定义区
            listSettingRangeData.Sort((x, y) => { return x.Min.CompareTo(y.Min); });//排序
            for (int i = 0; i < listSettingRangeData.Count - 1; i++)
            {
                if (listSettingRangeData[i + 1].Min - listSettingRangeData[i].Max > 1)
                {
                    return new RangeValideResult()
                    {
                        isSuccess = false,
                        errMsg = "出现没有被覆盖的空白区间",
                        listErrRangeData = new List<OpenRangeData>() { new OpenRangeData() { Min = listSettingRangeData[i].Max, Max = listSettingRangeData[i + 1].Min } }
                    };
                }
            }
            if (listSettingRangeData[0].Min - totalRange.Min > 0)
            {
                return new RangeValideResult()
                {
                    isSuccess = false,
                    errMsg = "出现没有被覆盖的空白区间",
                    listErrRangeData = new List<OpenRangeData>() { new OpenRangeData() { Min = totalRange.Min, Max = listSettingRangeData[0].Min } }
                };
            }
            if (totalRange.Max - listSettingRangeData[listSettingRangeData.Count - 1].Max > 0)
            {
                return new RangeValideResult()
                {
                    isSuccess = false,
                    errMsg = "出现没有被覆盖的空白区间",
                    listErrRangeData = new List<OpenRangeData>() { new OpenRangeData() { Min = listSettingRangeData[listSettingRangeData.Count - 1].Max, Max = totalRange.Max } }
                };
            }
            return new RangeValideResult()
            {
                isSuccess = true,
                errMsg = ""
            };
        }
    }
    public class RangeValideResult
    {
        public bool isSuccess = false;
        public List<OpenRangeData> listErrRangeData = null;
        public string errMsg = "";
    }
}
