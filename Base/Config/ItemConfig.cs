﻿
using ExcelMapper;
using Base.ConfigParse;
using Message;

namespace Base.Config
{

    [SheetName(new string[] { "材料", "道具" })]
    public class ItemConfig : IExcelConfig<int>
    {
        [ExcelColumnName("##ID")]
        public int Id { get; set; }

        [ExcelColumnName("名称")]
        public string Name { get; set; }

        [ExcelColumnName("最大堆叠数")]
        public uint MaxCount { get; set; }

        [ExcelColumnName("是用可用")]
        public uint used { get; set; }
    }

    [Config("Item")]
    public partial class ItemConfigCategory : ACategory<int, ItemConfig>
    {
        public static ItemConfigCategory Instance;
        public ItemConfigCategory()
        {
            Instance = this;
        }
    }



    [SheetName(new string[] { "产出包" })]
    public class AssetsConfig : IExcelConfig<int>
    {
        [ExcelColumnName("##ID")]
        public int Id { get; set; }

        [ExcelColumnName("名称")]
        public string Name { get; set; }

        [ExcelColumnName("最大堆叠数")]
        public uint MaxCount { get; set; }
    }

    [Config("Item")]
    public partial class AssetsConfigCategory : ACategory<int, AssetsConfig> { }
}